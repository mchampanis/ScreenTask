using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ScreenTask {
    public partial class frmMain : Form {
        private ScreenCapturePInvoke screenCapture = new ScreenCapturePInvoke();

        private static frmMain instance;
        private bool isWorking;
        private bool actuallyQuit = false;
        private String firewallRuleName = "ScreenTask";

        private String lastIp = "";
        private int lastPort = -1;

        private object locker = new object();
        private ReaderWriterLock rwl = new ReaderWriterLock();
        private List<Tuple<string, string>> _ips;
        HttpListener serv;
        private Version currentVersion;
        private AppSettings _currentSettings = new AppSettings();
        private static NotifyIcon trayIcon;

        public frmMain() {
            InitializeComponent();
            currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            CheckForIllegalCrossThreadCalls = false; // For Visual Studio Debuging Only !
            serv = new HttpListener();
            serv.IgnoreWriteExceptions = true;

            foreach (var screen in Screen.AllScreens) {
                comboScreens.Items.Add(screen.DeviceName.Replace("\\", "").Replace(".", ""));
            }

            comboScreens.SelectedIndex = 0;
            this.Text = $"ScreenTask {currentVersion}";
        }

        public static frmMain GetForm {
            get {
                if (instance == null || instance.IsDisposed) {
                    instance = new frmMain();
                }

                return instance;
            }
        }

        public static NotifyIcon getIconRef {
            get {
                if (trayIcon == null) {
                    trayIcon = new NotifyIcon();
                }

                return trayIcon;
            }
        }

        private async void btnStartServer_Click(object sender, EventArgs e) {
            var ip = _ips.ElementAt(comboIPs.SelectedIndex).Item2.ToString();
            var port = (int)numPort.Value;

            if (btnStartServer.Tag.ToString() != "start") {
                btnStartServer.Tag = "start";
                btnStartServer.Text = "Start Server";
                numPort.Enabled = true;
                isWorking = false;
                btnLaunch.Enabled = false;
                trayIcon.Icon = Properties.Resources.iconOff;

                if (serv.IsListening) {
                    serv.Stop();
                    serv.Close();
                }

                Log("Server stopped.");

                await DeleteFirewallRule(ip, port);

                return;
            }

            try {
                serv = new HttpListener();
                serv.IgnoreWriteExceptions = true;
                isWorking = true;
                Log("Starting server...");
                numPort.Enabled = false;

                await AddFirewallRule(ip, port);

                lastIp = ip;
                lastPort = port;

                _ = Task.Factory.StartNew(() => CaptureScreenEvery((int)numShotEvery.Value), TaskCreationOptions.LongRunning);
                btnStartServer.Tag = "stop";
                btnStartServer.Text = "Stop Server";
                trayIcon.Icon = Properties.Resources.iconOn;
                btnLaunch.Enabled = true;
                await StartServer();

            } catch (ObjectDisposedException) {
                serv = new HttpListener();
                serv.IgnoreWriteExceptions = true;
            } catch (HttpListenerException httpEx) {
                if (httpEx.ErrorCode == 32) // port used
                {
                    btnStartServer.Tag = "start";
                    btnStartServer.Text = "Start Server";
                    isWorking = false;
                    Log($"Port {port} is in use");
                    var msgResult = MessageBox.Show($"Port {port} is in use, do you want to use another random one?", "Port is in use", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msgResult == DialogResult.Yes) {
                        numPort.Value += DateTime.Now.Second;
                        port = (int)numPort.Value;
                        Log($"New port is {port}");
                        btnStartServer_Click(sender, e);
                        return;
                    } else {
                        Log("Port change request declined");
                    }
                } else if (httpEx.ErrorCode == 183) {
                    MessageBox.Show(httpEx.Message);
                }
            } catch (Exception ex) {
                Log("Error: " + ex.Message);
            }
        }
        private async Task StartServer() {
            var ip = _ips.ElementAt(comboIPs.SelectedIndex).Item2;
            var port = (int)numPort.Value;

            var url = string.Format("http://{0}:{1}/", ip, port.ToString());
            var localUrl = "http://localhost:" + port.ToString() + "/";

            txtURL.Text = url;

            serv.Prefixes.Clear();
            serv.Prefixes.Add(localUrl);
            serv.Prefixes.Add(url);
            serv.Start();

            Log("Server started successfully");

            Log("Network URL: " + url);
            Log("Localhost URL: " + localUrl);

            while (isWorking) {
                var ctx = await serv.GetContextAsync();
                var resPath = ctx.Request.Url.LocalPath;

                if (resPath == "/") {
                    resPath += "index.html";
                }

                var page = Application.StartupPath + "/WebServer" + resPath;
                bool fileExist;
                
                lock (locker) {
                   fileExist = File.Exists(page);
                }

                if (!fileExist) {
                    var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">Error 404 , File Not Found </h1><hr><a href=\".\\\">Back to Home</a>");
                    ctx.Response.ContentType = "text/html";
                    ctx.Response.StatusCode = 404;
                    try {
                        await ctx.Response.OutputStream.WriteAsync(errorPage, 0, errorPage.Length);
                    } catch {
                    }
                    ctx.Response.Close();
                    continue;
                }

                if (_currentSettings.IsPrivateSession) {
                    if (!ctx.Request.Headers.AllKeys.Contains("Authorization")) {
                        ctx.Response.StatusCode = 401;
                        ctx.Response.AddHeader("WWW-Authenticate", "Basic realm=ScreenTask Authentication : ");
                        ctx.Response.Close();
                        continue;
                    } else {
                        var auth1 = ctx.Request.Headers["Authorization"];
                        auth1 = auth1.Remove(0, 6); // Remove "Basic " from the header value
                        auth1 = Encoding.UTF8.GetString(Convert.FromBase64String(auth1));
                        var auth2 = string.Format("{0}:{1}", txtUser.Text, txtPassword.Text);

                        if (auth1 != auth2) {
                            Log(string.Format("Bad Login from {0} using {1}", ctx.Request.RemoteEndPoint.Address.ToString(), auth1));
                            var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">Not Authorized !!! </h1><hr><a href=\"./\">Back to Home</a>");
                            ctx.Response.ContentType = "text/html";
                            ctx.Response.StatusCode = 401;
                            ctx.Response.AddHeader("WWW-Authenticate", "Basic realm=ScreenTask Authentication : ");
                            try {
                                await ctx.Response.OutputStream.WriteAsync(errorPage, 0, errorPage.Length);
                            } catch {
                            }
                            ctx.Response.Close();
                            continue;
                        }
                    }
                }

                byte[] filedata;

                // Required for One-Time Access of the file {Reader\Writer Problem in OS}
                rwl.AcquireReaderLock(Timeout.Infinite);
                filedata = File.ReadAllBytes(page);
                rwl.ReleaseReaderLock();

                var fileinfo = new FileInfo(page);
                if (fileinfo.Extension == ".css") // important for IE -> Content-Type must be defiend for CSS files unless will ignored !!!
                    ctx.Response.ContentType = "text/css";
                else if (fileinfo.Extension == ".svg")
                    ctx.Response.ContentType = "image/svg+xml";
                else if (fileinfo.Extension == ".html" || fileinfo.Extension == ".htm")
                    ctx.Response.ContentType = "text/html"; // Important For Chrome Otherwise will display the HTML as plain text.

                ctx.Response.StatusCode = 200;
                try {
                    await ctx.Response.OutputStream.WriteAsync(filedata, 0, filedata.Length);
                } catch {
                }

                ctx.Response.Close();
            }

        }
        private async Task CaptureScreenEvery(int msec) {
            while (isWorking) {
                TakeScreenshot(_currentSettings.IsShowMouseEnabled);
                msec = (int)numShotEvery.Value;
                await Task.Delay(msec);
            }
        }
        private void TakeScreenshot(bool captureMouse) {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            var encoderQuality = System.Drawing.Imaging.Encoder.Quality;
            var encoderParam = new EncoderParameter(encoderQuality, _currentSettings.ImageQuality);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = encoderParam;
            if (captureMouse) {
                var bmp = screenCapture.CaptureSelectedScreen(true, comboScreens.SelectedIndex);
                rwl.AcquireWriterLock(Timeout.Infinite);
                bmp.Save(Application.StartupPath + "/WebServer" + "/ScreenTask.jpg", jpgEncoder, encoderParams);
                rwl.ReleaseWriterLock();

                bmp.Dispose();
                bmp = null;
                return;
            }

            Rectangle bounds = Screen.AllScreens[comboScreens.SelectedIndex].Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.CopyFromScreen(new Point(bounds.X, bounds.Y), Point.Empty, bounds.Size);
                }
                rwl.AcquireWriterLock(Timeout.Infinite);

                bitmap.Save(Application.StartupPath + "/WebServer" + "/ScreenTask.jpg", jpgEncoder, encoderParams);
                rwl.ReleaseWriterLock();
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }
        private string GetIPv4Address() {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName())) {
                if (IPA.AddressFamily == AddressFamily.InterNetwork) {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        private List<Tuple<string, string>> GetAllIPv4Addresses() {
            List<Tuple<string, string>> ipList = new List<Tuple<string, string>>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {

                foreach (var ua in ni.GetIPProperties().UnicastAddresses) {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork) {
                        ipList.Add(Tuple.Create(ni.Name, ua.Address.ToString()));
                    }
                }
            }
            return ipList;
        }
        private async Task AddFirewallRule(String ip, int port) {
            await Task.Run(() => {
                FirewallConf cnf = new FirewallConf();

                try {
                    cnf.AddRule(firewallRuleName, "Allow incoming network traffic", ip, port);
                    Log("ScreenTask rule added to firewall for " + ip + ":" + port.ToString());
                } catch (Exception ex) {
                    Log(ex.Message);
                }
            });
        }

        private async Task DeleteFirewallRule(String ip, int port) {
            await Task.Run(() => {
                FirewallConf cnf = new FirewallConf();

                try {
                    cnf.DeleteRule(firewallRuleName, ip, port);
                    Log("ScreenTask rule removed from firewall");
                } catch (Exception ex) {
                    Log(ex.Message);
                }
            });
        }

        private void Log(string text) {
            txtLog.Text += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " : " + text + "\r\n";

        }
        private void cbPrivate_CheckedChanged(object sender, EventArgs e) {
            if (cbPrivate.Checked == true) {
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                _currentSettings.IsPrivateSession = true;
            } else {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                _currentSettings.IsPrivateSession = false;
            }
        }
        private void cbCaptureMouse_CheckedChanged(object sender, EventArgs e) {
            if (cbCaptureMouse.Checked) {
                _currentSettings.IsShowMouseEnabled = true;
            } else {
                _currentSettings.IsShowMouseEnabled = false;
            }
        }
        private void txtLog_TextChanged(object sender, EventArgs e) {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void frmMain_Load(object sender, EventArgs e) {
            _ips = GetAllIPv4Addresses();

            foreach (var ip in _ips) {
                this.comboIPs.Items.Add(ip.Item2 + " - " + ip.Item1);
            }

            try {
                this.comboIPs.SelectedIndex = 1;
            } catch { }

            var recommendedIP = _ips.FirstOrDefault(ip => ip.Item2.StartsWith("192.") || ip.Item2.StartsWith("10."));
            if (recommendedIP != null) {
                this.comboIPs.SelectedIndex = _ips.IndexOf(recommendedIP);
            }

            try {
                if (File.Exists("appsettings.xml")) {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    using (var appSettingsFile = File.OpenRead("appsettings.xml")) {
                        _currentSettings = (AppSettings)serializer.Deserialize(appSettingsFile);
                    }

                    this.cbPrivate.Checked = _currentSettings.IsPrivateSession;
                    this.cbCaptureMouse.Checked = _currentSettings.IsShowMouseEnabled;
                    this.cbAutoStart.Checked = _currentSettings.IsAutoStartServerEnabled;
                    this.txtUser.Text = _currentSettings.Username;
                    this.txtPassword.Text = _currentSettings.Password;
                    this.numPort.Value = _currentSettings.Port;
                    this.numShotEvery.Value = _currentSettings.ScreenshotsSpeed;
                    this.qualitySlider.Value = _currentSettings.ImageQuality != default ? _currentSettings.ImageQuality : 75;

                    var savedIndex = _ips.IndexOf(_ips.FirstOrDefault(ip => ip.Item2.Contains(_currentSettings.IP)));

                    if (savedIndex > 0) {
                        this.comboIPs.SelectedIndex = savedIndex;
                    }                    

                    if (_currentSettings.SelectedScreenIndex > -1 && comboScreens.Items.Count > 0 && _currentSettings.SelectedScreenIndex <= comboScreens.Items.Count - 1) {
                        this.comboScreens.SelectedIndex = _currentSettings.SelectedScreenIndex;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Failed to load local appsettings.xml file.\r\n{ex.Message}", "ScreenTask", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_currentSettings.IsAutoStartServerEnabled) {
                btnStartServer_Click(null, null);
            }        
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                _currentSettings.Port = (int)numPort.Value;
                _currentSettings.Username = txtUser.Text;
                _currentSettings.Password = txtPassword.Text;
                _currentSettings.IsShowMouseEnabled = cbCaptureMouse.Checked;
                _currentSettings.IsPrivateSession = cbPrivate.Checked;
                _currentSettings.IsAutoStartServerEnabled = cbAutoStart.Checked;
                _currentSettings.ScreenshotsSpeed = (int)numShotEvery.Value;

                try {
                    _currentSettings.IP = _ips.ElementAt(comboIPs.SelectedIndex).Item2;
                } catch { }
                
                _currentSettings.SelectedScreenIndex = comboScreens.SelectedIndex;
                _currentSettings.ImageQuality = qualitySlider.Value;

                using (var appSettingsFile = new FileStream("appsettings.xml", FileMode.Create, FileAccess.Write)) {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    serializer.Serialize(appSettingsFile, _currentSettings);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Cannot save the settings file next to the executable file.\r\n{ex.Message}", "ScreenTask", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e) {
            if (txtURL.Text.StartsWith("http")) {
                Process.Start(txtURL.Text);
            }
        }

        private void qualitySlider_Scroll(object sender, EventArgs e) {
            _currentSettings.ImageQuality = qualitySlider.Value;

        }

        public async void Shutdown() {
            await DeleteFirewallRule(lastIp, lastPort);
            actuallyQuit = true;
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown || actuallyQuit) {
                return;
            }

            e.Cancel = true;

            this.Hide();
        }
    }
}

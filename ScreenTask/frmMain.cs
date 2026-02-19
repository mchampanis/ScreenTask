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
    public partial class FrmMain : Form {
        private const string APPNAME = "ScreenTask"; // can be whatever
        private const string APPNAME_SAFE = "ScreenTask"; // mustn't contain spaces or special chars

        private readonly ScreenCapturePInvoke screenCapture = new ScreenCapturePInvoke();

        private static FrmMain instance;
        private bool isWorking;
        private bool actuallyQuit;
        private readonly string firewallRuleName = APPNAME;

        private string lastIp = "";
        private int lastPort = -1;

        private object locker = new object();
        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private List<Tuple<string, string>> _ips;
        HttpListener serv;
        private readonly Version currentVersion;
        private AppSettings _currentSettings = new AppSettings();
        private static NotifyIcon trayIcon;
        private readonly string _settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            APPNAME,
            "appsettings.xml"
        );

        public FrmMain() {
            InitializeComponent();
            currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            CheckForIllegalCrossThreadCalls = false; // for Visual Studio debugging only
            foreach (Screen screen in Screen.AllScreens) {
                comboScreens.Items.Add(screen.DeviceName.Replace("\\", "").Replace(".", ""));
            }

            comboScreens.SelectedIndex = 0;
            Text = $"{APPNAME} {currentVersion}";
        }

        public static FrmMain GetForm {
            get {
                if (instance == null || instance.IsDisposed) {
                    instance = new FrmMain();
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
            string ip = _ips.ElementAt(comboIPs.SelectedIndex).Item2;
            int port = (int)numPort.Value;

            if (btnStartServer.Tag.ToString() != "start") {
                btnStartServer.Tag = "start";
                btnStartServer.Text = "Start Server";
                numPort.Enabled = true;
                isWorking = false;
                btnLaunch.Enabled = false;
                trayIcon.Icon = Properties.Resources.iconOff;

                if (serv != null && serv.IsListening) {
                    serv.Stop();
                    serv.Close();
                }

                Log("Server stopped.");

                await DeleteFirewallRule(ip, port);
                return;
            }

            try {
                serv = new HttpListener {
                    IgnoreWriteExceptions = true
                };

                isWorking = true;
                Log("Starting server...");
                numPort.Enabled = false;

                await AddFirewallRule(ip, port);

                lastIp = ip;
                lastPort = port;

                _ = Task.Factory.StartNew(() => CaptureScreenEvery(), TaskCreationOptions.LongRunning);
                btnStartServer.Tag = "stop";
                btnStartServer.Text = "Stop Server";
                trayIcon.Icon = Properties.Resources.iconOn;
                btnLaunch.Enabled = true;
                await StartServer();

            } catch (HttpListenerException httpEx) {
                if (httpEx.ErrorCode == 32) // port already in use
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
            string ip = _ips.ElementAt(comboIPs.SelectedIndex).Item2;
            int port = (int)numPort.Value;

            string url = $"http://{ip}:{port}/";
            string localUrl = $"http://localhost:{port}/";

            txtURL.Text = url;

            serv.Prefixes.Clear();
            serv.Prefixes.Add(localUrl);
            serv.Prefixes.Add(url);
            serv.Start();

            Log("Server started successfully");

            Log("Network URL: " + url);
            Log("Localhost URL: " + localUrl);

            while (isWorking) {
                HttpListenerContext ctx;
                try {
                    ctx = await serv.GetContextAsync();
                } catch (ObjectDisposedException) {
                    break; // listener was stopped, exit cleanly
                }

                string resPath = ctx.Request.Url.LocalPath;

                if (resPath == "/") {
                    resPath += "index.html";
                }

                string page = Application.StartupPath + "/WebServer" + resPath;
                bool fileExist;

                lock (locker) {
                    fileExist = File.Exists(page);
                }

                if (!fileExist) {
                    var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">404 - File not found</h1><hr><a href=\"./\">Back to home</a>");
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
                        ctx.Response.AddHeader("WWW-Authenticate", $"Basic realm={APPNAME}");
                        ctx.Response.Close();
                        continue;
                    } else {
                        var auth1 = ctx.Request.Headers["Authorization"];
                        auth1 = auth1.Remove(0, 6); // remove "Basic " prefix from the header value
                        auth1 = Encoding.UTF8.GetString(Convert.FromBase64String(auth1));
                        var auth2 = $"{txtUser.Text}:{txtPassword.Text}";

                        if (auth1 != auth2) {
                            Log($"Bad login from {ctx.Request.RemoteEndPoint.Address} using {auth1}");
                            var errorPage = Encoding.UTF8.GetBytes("<h1 style=\"color:red\">Not authorized</h1><hr><a href=\"./\">Back to home</a>");
                            ctx.Response.ContentType = "text/html";
                            ctx.Response.StatusCode = 401;
                            ctx.Response.AddHeader("WWW-Authenticate", $"Basic realm={APPNAME}");
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

                // read lock required for concurrent file access
                rwl.EnterReadLock();
                filedata = File.ReadAllBytes(page);
                rwl.ExitReadLock();

                var fileinfo = new FileInfo(page);
                if (fileinfo.Extension == ".css") // content-type must be set for css files or ie will ignore them
{
                    ctx.Response.ContentType = "text/css";
                } else if (fileinfo.Extension == ".svg") {
                    ctx.Response.ContentType = "image/svg+xml";
                } else if (fileinfo.Extension == ".html" || fileinfo.Extension == ".htm") {
                    ctx.Response.ContentType = "text/html"; // content-type must be set for html or chrome renders it as plain text
                }

                ctx.Response.StatusCode = 200;
                try {
                    await ctx.Response.OutputStream.WriteAsync(filedata, 0, filedata.Length);
                } catch {
                }

                ctx.Response.Close();
            }

        }
        private async Task CaptureScreenEvery() {
            while (isWorking) {
                TakeScreenshot(_currentSettings.IsShowMouseEnabled);
                int msec = (int)numShotEvery.Value;
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
                rwl.EnterWriteLock();
                bmp.Save(Application.StartupPath + "/WebServer" + $"/{APPNAME_SAFE}.jpg", jpgEncoder, encoderParams);
                rwl.ExitWriteLock();

                bmp.Dispose();
                bmp = null;
                return;
            }

            Rectangle bounds = Screen.AllScreens[comboScreens.SelectedIndex].Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.CopyFromScreen(new Point(bounds.X, bounds.Y), Point.Empty, bounds.Size);
                }
                rwl.EnterWriteLock();

                bitmap.Save(Application.StartupPath + "/WebServer" + $"/{APPNAME_SAFE}.jpg", jpgEncoder, encoderParams);
                rwl.ExitWriteLock();
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
        private async Task AddFirewallRule(string ip, int port) {
            await Task.Run(() => {
                FirewallConf cnf = new FirewallConf();

                try {
                    cnf.AddRule(firewallRuleName, "Allow incoming network traffic", ip, port);
                    Log($"{APPNAME_SAFE} rule added to firewall for {ip}:{port}");
                } catch (Exception ex) {
                    Log(ex.Message);
                }
            });
        }

        private async Task DeleteFirewallRule(string ip, int port) {
            await Task.Run(() => {
                FirewallConf cnf = new FirewallConf();

                try {
                    cnf.DeleteRule(firewallRuleName, ip, port);
                    Log($"{APPNAME_SAFE} rule removed from firewall");
                } catch (Exception ex) {
                    Log(ex.Message);
                }
            });
        }

        private void Log(string text) {
            txtLog.Text += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " : " + text + "\r\n";

        }
        private void cbPrivate_CheckedChanged(object sender, EventArgs e) {
            txtUser.Enabled = cbPrivate.Checked;
            txtPassword.Enabled = cbPrivate.Checked;
            _currentSettings.IsPrivateSession = cbPrivate.Checked;
        }
        private void cbCaptureMouse_CheckedChanged(object sender, EventArgs e) {
            _currentSettings.IsShowMouseEnabled = cbCaptureMouse.Checked;
        }
        private void txtLog_TextChanged(object sender, EventArgs e) {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void frmMain_Load(object sender, EventArgs e) {
            _ips = GetAllIPv4Addresses();

            foreach (var ip in _ips) {
                comboIPs.Items.Add(ip.Item2 + " - " + ip.Item1);
            }

            try {
                comboIPs.SelectedIndex = 1;
            } catch { }

            var recommendedIP = _ips.FirstOrDefault(ip => ip.Item2.StartsWith("192.") || ip.Item2.StartsWith("10."));
            if (recommendedIP != null) {
                comboIPs.SelectedIndex = _ips.IndexOf(recommendedIP);
            }

            try {
                if (File.Exists(_settingsPath)) {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    using (FileStream appSettingsFile = File.OpenRead(_settingsPath)) {
                        _currentSettings = (AppSettings)serializer.Deserialize(appSettingsFile);
                    }

                    cbPrivate.Checked = _currentSettings.IsPrivateSession;
                    cbCaptureMouse.Checked = _currentSettings.IsShowMouseEnabled;
                    cbAutoStart.Checked = _currentSettings.IsAutoStartServerEnabled;
                    txtUser.Text = _currentSettings.Username;
                    txtPassword.Text = _currentSettings.Password;
                    numPort.Value = _currentSettings.Port;
                    numShotEvery.Value = _currentSettings.ScreenshotsSpeed;
                    qualitySlider.Value = _currentSettings.ImageQuality != default ? _currentSettings.ImageQuality : 90;

                    int savedIndex = _ips.IndexOf(_ips.FirstOrDefault(ip => ip.Item2.Contains(_currentSettings.IP)));

                    if (savedIndex > 0) {
                        comboIPs.SelectedIndex = savedIndex;
                    }

                    if (_currentSettings.SelectedScreenIndex > -1 && comboScreens.Items.Count > 0 && _currentSettings.SelectedScreenIndex <= comboScreens.Items.Count - 1) {
                        comboScreens.SelectedIndex = _currentSettings.SelectedScreenIndex;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Failed to load settings file.\r\n{ex.Message}", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath));
                using (FileStream appSettingsFile = new FileStream(_settingsPath, FileMode.Create, FileAccess.Write)) {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    serializer.Serialize(appSettingsFile, _currentSettings);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Cannot save the settings file.\r\n{ex.Message}", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rwl.Dispose();
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

            Hide();
        }
    }
}

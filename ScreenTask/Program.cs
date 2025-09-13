using System;
using System.Windows.Forms;

namespace ScreenTask {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (NotifyIcon icon = frmMain.getIconRef) {
                icon.Icon = Properties.Resources.iconOff;
                icon.ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Show main windcow", (s, e) => { frmMain.GetForm.Show(); }),
                    new MenuItem("Hide main window", (s, e) => { frmMain.GetForm.Hide(); }),
                    new MenuItem("Exit", (s, e) => { frmMain.GetForm.Shutdown(); }),
                });
                icon.Click += new System.EventHandler(showApp);
                icon.Visible = true;

                frmMain.GetForm.Show();

                Application.Run();
                icon.Visible = false;
            }
        }

        static void showApp(object sender, EventArgs e) {
            frmMain.GetForm.Show();
        }

    }
}

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

            using (NotifyIcon icon = FrmMain.getIconRef) {
                icon.Icon = Properties.Resources.iconOff;
                icon.ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Show main window", (s, e) => { FrmMain.GetForm.Show(); }),
                    new MenuItem("Hide main window", (s, e) => { FrmMain.GetForm.Hide(); }),
                    new MenuItem("Exit", (s, e) => { FrmMain.GetForm.Shutdown(); }),
                });
                icon.Click += new System.EventHandler(ShowApp);
                icon.Visible = true;

                FrmMain.GetForm.Show();

                Application.Run();
                icon.Visible = false;
            }
        }

        static void ShowApp(object sender, EventArgs e) {
            FrmMain.GetForm.Show();
        }

    }
}

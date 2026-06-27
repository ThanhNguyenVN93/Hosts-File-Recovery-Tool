using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace frm_hosts_file_recovery
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!IsSupportedWindowsVersion())
            {
                MessageBox.Show(
                    "This application requires Windows 7 SP1 (build 7601) or later.\n" +
                    "Your current version is not supported.",
                    "Unsupported OS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsAdmin())
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName        = Application.ExecutablePath,
                        Verb            = "runas",
                        UseShellExecute = true
                    });
                }
                catch
                {
                    MessageBox.Show(
                        "This application requires Administrator privileges.\n" +
                        "Please re-run it and click \"Yes\" at the UAC prompt.",
                        "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static bool IsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static bool IsSupportedWindowsVersion()
        {
            var ver = Environment.OSVersion.Version;
            return !(ver.Major < 6 || (ver.Major == 6 && ver.Minor == 1 && ver.Build < 7601));
        }
    }
}

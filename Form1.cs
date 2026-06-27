using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace frm_hosts_file_recovery
{
    public partial class Form1 : Form
    {
        private static readonly string HostsPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                         @"drivers\etc\hosts");

        private static readonly string BackupsDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");

        private static readonly Encoding Utf8NoBom =
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        // ─────────────────────────────────────────────
        // PHASE 1 – App Startup
        // ─────────────────────────────────────────────

        public Form1()
        {
            InitializeComponent();
            ReloadBackupList();
            LoadHostsFile();
        }

        private void LoadHostsFile()
        {
            if (!File.Exists(HostsPath))
            {
                txtHostsContent.Text       = string.Empty;
                SetStatus("🔴 Hosts file missing – Click \"Restore Default\" to recreate it.", false);
                btnBackup.Enabled          = false;
                btnRestoreSelected.Enabled = cboBackups.Items.Count > 0;
                btnRestoreDefault.Enabled  = true;
                return;
            }

            try
            {
                txtHostsContent.Text       = File.ReadAllText(HostsPath, Utf8NoBom);
                SetStatus("🟢 Hosts file found and loaded successfully.", true);
                btnBackup.Enabled          = true;
                btnRestoreDefault.Enabled  = true;
                btnRestoreSelected.Enabled = cboBackups.Items.Count > 0;
            }
            catch (UnauthorizedAccessException) { HandleUnauthorized(); }
            catch (IOException ex)              { HandleIOError(ex);    }
            catch (Exception ex)                { HandleGeneral(ex);    }
        }

        private void ReloadBackupList()
        {
            cboBackups.Items.Clear();

            if (!Directory.Exists(BackupsDir))
            {
                btnRestoreSelected.Enabled = false;
                return;
            }

            var files = new DirectoryInfo(BackupsDir).GetFiles("*.bak");
            Array.Sort(files, (a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

            foreach (var f in files)
                cboBackups.Items.Add(f.Name);

            if (cboBackups.Items.Count > 0)
                cboBackups.SelectedIndex = 0;

            btnRestoreSelected.Enabled = cboBackups.Items.Count > 0;
        }

        // ─────────────────────────────────────────────
        // PHASE 2 – Safe Backup Workflow
        // ─────────────────────────────────────────────

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (!File.Exists(HostsPath))
            {
                MessageBox.Show("No data to back up.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string content = File.ReadAllText(HostsPath, Utf8NoBom);

                if (!Directory.Exists(BackupsDir))
                    Directory.CreateDirectory(BackupsDir);

                string fileName = "hosts_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
                string destPath = Path.Combine(BackupsDir, fileName);
                File.WriteAllText(destPath, content, Utf8NoBom);

                ReloadBackupList();
                MessageBox.Show("Backup saved to:\n" + destPath,
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException) { HandleUnauthorized(); }
            catch (IOException ex)              { HandleIOError(ex);    }
            catch (Exception ex)                { HandleGeneral(ex);    }
        }

        // ─────────────────────────────────────────────
        // PHASE 3 – Restore Workflow
        // ─────────────────────────────────────────────

        private void btnRestoreDefault_Click(object sender, EventArgs e)
        {
            PerformRestore(DefaultHostsContent(), "Windows default content");
        }

        private void btnRestoreSelected_Click(object sender, EventArgs e)
        {
            if (cboBackups.SelectedItem == null)
            {
                MessageBox.Show("No backup file selected.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string bakPath = Path.Combine(BackupsDir, cboBackups.SelectedItem.ToString());
            if (!File.Exists(bakPath))
            {
                MessageBox.Show("Backup file no longer exists:\n" + bakPath,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReloadBackupList();
                return;
            }

            try
            {
                string content = File.ReadAllText(bakPath, Utf8NoBom);

                if (string.IsNullOrWhiteSpace(content))
                {
                    MessageBox.Show(
                        "The selected backup file is empty or contains only whitespace.\n" +
                        "Restore aborted to prevent overwriting the system hosts file with blank content.",
                        "Invalid Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PerformRestore(content, "backup: " + cboBackups.SelectedItem);
            }
            catch (UnauthorizedAccessException) { HandleUnauthorized(); }
            catch (IOException ex)              { HandleIOError(ex);    }
            catch (Exception ex)                { HandleGeneral(ex);    }
        }

        /// <summary>Safe write pipeline: strip Read-Only → write → re-lock → flush DNS.</summary>
        private void PerformRestore(string newContent, string sourceLabel)
        {
            var confirm = MessageBox.Show(
                $"Are you sure you want to restore the hosts file from {sourceLabel}?",
                "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            bool wasReadOnly = false;
            try
            {
                string dir = Path.GetDirectoryName(HostsPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(HostsPath))
                {
                    var attrs = File.GetAttributes(HostsPath);
                    wasReadOnly = (attrs & FileAttributes.ReadOnly) != 0;
                    if (wasReadOnly)
                        File.SetAttributes(HostsPath, attrs & ~FileAttributes.ReadOnly);
                }

                File.WriteAllText(HostsPath, newContent, Utf8NoBom);

                File.SetAttributes(HostsPath,
                    File.GetAttributes(HostsPath) | FileAttributes.ReadOnly);

                FlushDns();

                LoadHostsFile();
                MessageBox.Show("Restore successful! DNS cache has been flushed.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException)
            {
                TryRestoreReadOnly(wasReadOnly);
                HandleUnauthorized();
            }
            catch (IOException ex)
            {
                TryRestoreReadOnly(wasReadOnly);
                HandleIOError(ex);
            }
            catch (Exception ex)
            {
                TryRestoreReadOnly(wasReadOnly);
                HandleGeneral(ex);
            }
        }

        private static void TryRestoreReadOnly(bool shouldRestore)
        {
            if (!shouldRestore || !File.Exists(HostsPath)) return;
            try
            {
                File.SetAttributes(HostsPath,
                    File.GetAttributes(HostsPath) | FileAttributes.ReadOnly);
            }
            catch { }
        }

        private static void FlushDns()
        {
            try
            {
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo("ipconfig", "/flushdns")
                    {
                        CreateNoWindow  = true,
                        UseShellExecute = false
                    };
                    p.Start();
                    p.WaitForExit();
                }
            }
            catch { }
        }

        // ─────────────────────────────────────────────
        // PHASE 4 – Exception Handling
        // ─────────────────────────────────────────────

        private static void HandleUnauthorized()
        {
            MessageBox.Show(
                "The file is locked by another application (e.g. Antivirus or Notepad).\n" +
                "Close that application and try again.",
                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void HandleIOError(IOException ex)
        {
            MessageBox.Show(
                "The disk is full or a hardware error prevented the file from being written.\n\nDetails: " + ex.Message,
                "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void HandleGeneral(Exception ex)
        {
            MessageBox.Show(
                "An unexpected error occurred:\n\n" + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ─────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────

        private void cboBackups_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRestoreSelected.Enabled = cboBackups.SelectedItem != null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadBackupList();
            LoadHostsFile();
        }

        private void SetStatus(string message, bool ok)
        {
            lblStatus.Text      = message;
            lblStatus.ForeColor = ok
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Crimson;
        }

        private static string DefaultHostsContent()
        {
            return
                "# Copyright (c) 1993-2009 Microsoft Corp."                                         + Environment.NewLine +
                "#"                                                                                  + Environment.NewLine +
                "# This is a sample HOSTS file used by Microsoft TCP/IP for Windows."               + Environment.NewLine +
                "#"                                                                                  + Environment.NewLine +
                "# This file contains the mappings of IP addresses to host names. Each"             + Environment.NewLine +
                "# entry should be kept on an individual line. The IP address should"               + Environment.NewLine +
                "# be placed in the first column followed by the corresponding host name."          + Environment.NewLine +
                "# The IP address and the host name should be separated by at least one"            + Environment.NewLine +
                "# space."                                                                           + Environment.NewLine +
                "#"                                                                                  + Environment.NewLine +
                "# Additionally, comments (such as these) may be inserted on individual"            + Environment.NewLine +
                "# lines or following the machine name denoted by a '#' symbol."                   + Environment.NewLine +
                "#"                                                                                  + Environment.NewLine +
                "# For example:"                                                                     + Environment.NewLine +
                "#"                                                                                  + Environment.NewLine +
                "#      102.54.94.97     rhino.acme.com          # source server"                   + Environment.NewLine +
                "#       38.25.63.10     x.acme.com              # x client host"                  + Environment.NewLine +
                                                                                                      Environment.NewLine +
                "# localhost name resolution is handled within DNS itself."                          + Environment.NewLine +
                "#\t127.0.0.1       localhost"                                                       + Environment.NewLine +
                "#\t::1             localhost"                                                        + Environment.NewLine;
        }
    }
}

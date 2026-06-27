namespace frm_hosts_file_recovery
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblStatus          = new System.Windows.Forms.Label();
            this.txtHostsContent    = new System.Windows.Forms.TextBox();
            this.lblBackupList      = new System.Windows.Forms.Label();
            this.cboBackups         = new System.Windows.Forms.ComboBox();
            this.btnBackup          = new System.Windows.Forms.Button();
            this.btnRestoreDefault  = new System.Windows.Forms.Button();
            this.btnRestoreSelected = new System.Windows.Forms.Button();
            this.btnRefresh         = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ── lblStatus ──────────────────────────────────
            this.lblStatus.AutoSize  = false;
            this.lblStatus.Location  = new System.Drawing.Point(12, 9);
            this.lblStatus.Size      = new System.Drawing.Size(576, 20);
            this.lblStatus.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Text      = "Starting...";

            // ── txtHostsContent ────────────────────────────
            this.txtHostsContent.Location   = new System.Drawing.Point(12, 34);
            this.txtHostsContent.Size       = new System.Drawing.Size(576, 230);
            this.txtHostsContent.Multiline  = true;
            this.txtHostsContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHostsContent.ReadOnly   = true;
            this.txtHostsContent.Font       = new System.Drawing.Font("Consolas", 9F);
            this.txtHostsContent.WordWrap   = false;
            this.txtHostsContent.BackColor  = System.Drawing.Color.FromArgb(245, 245, 245);

            // ── lblBackupList ──────────────────────────────
            this.lblBackupList.AutoSize = true;
            this.lblBackupList.Location = new System.Drawing.Point(12, 276);
            this.lblBackupList.Text     = "Backup list:";
            this.lblBackupList.Font     = new System.Drawing.Font("Segoe UI", 9F);

            // ── cboBackups ─────────────────────────────────
            this.cboBackups.Location              = new System.Drawing.Point(90, 273);
            this.cboBackups.Size                  = new System.Drawing.Size(498, 24);
            this.cboBackups.DropDownStyle         = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBackups.Font                  = new System.Drawing.Font("Consolas", 9F);
            this.cboBackups.SelectedIndexChanged += new System.EventHandler(this.cboBackups_SelectedIndexChanged);

            // ── btnBackup ──────────────────────────────────
            this.btnBackup.Location = new System.Drawing.Point(12, 310);
            this.btnBackup.Size     = new System.Drawing.Size(120, 32);
            this.btnBackup.Text     = "Back Up";
            this.btnBackup.Enabled  = false;
            this.btnBackup.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBackup.Click   += new System.EventHandler(this.btnBackup_Click);

            // ── btnRestoreDefault ──────────────────────────
            this.btnRestoreDefault.Location = new System.Drawing.Point(140, 310);
            this.btnRestoreDefault.Size     = new System.Drawing.Size(148, 32);
            this.btnRestoreDefault.Text     = "Restore Default";
            this.btnRestoreDefault.Enabled  = false;
            this.btnRestoreDefault.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRestoreDefault.Click   += new System.EventHandler(this.btnRestoreDefault_Click);

            // ── btnRestoreSelected ─────────────────────────
            this.btnRestoreSelected.Location = new System.Drawing.Point(296, 310);
            this.btnRestoreSelected.Size     = new System.Drawing.Size(160, 32);
            this.btnRestoreSelected.Text     = "Restore from Backup";
            this.btnRestoreSelected.Enabled  = false;
            this.btnRestoreSelected.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRestoreSelected.Click   += new System.EventHandler(this.btnRestoreSelected_Click);

            // ── btnRefresh ─────────────────────────────────
            this.btnRefresh.Location = new System.Drawing.Point(464, 310);
            this.btnRefresh.Size     = new System.Drawing.Size(124, 32);
            this.btnRefresh.Text     = "Refresh";
            this.btnRefresh.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.Click   += new System.EventHandler(this.btnRefresh_Click);

            // ── Form1 ──────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(600, 355);
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox         = false;
            this.Name                = "Form1";
            this.Text                = "Hosts File Recovery";
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtHostsContent);
            this.Controls.Add(this.lblBackupList);
            this.Controls.Add(this.cboBackups);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnRestoreDefault);
            this.Controls.Add(this.btnRestoreSelected);
            this.Controls.Add(this.btnRefresh);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label    lblStatus;
        private System.Windows.Forms.TextBox  txtHostsContent;
        private System.Windows.Forms.Label    lblBackupList;
        private System.Windows.Forms.ComboBox cboBackups;
        private System.Windows.Forms.Button   btnBackup;
        private System.Windows.Forms.Button   btnRestoreDefault;
        private System.Windows.Forms.Button   btnRestoreSelected;
        private System.Windows.Forms.Button   btnRefresh;
    }
}

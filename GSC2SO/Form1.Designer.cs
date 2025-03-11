namespace GSC2SO
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.SaveGSCSelector = new System.Windows.Forms.SaveFileDialog();
            this.dragdrop = new DevExpress.XtraEditors.SimpleButton();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.BackgroundWorker_gsc = new System.ComponentModel.BackgroundWorker();
            this.canceler = new DevExpress.XtraEditors.SimpleButton();
            this.GSCFiles = new System.Windows.Forms.OpenFileDialog();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.AttachHeader = new DevExpress.XtraEditors.SimpleButton();
            this.ClearHeaders = new DevExpress.XtraEditors.SimpleButton();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit5 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.StringCount = new System.Windows.Forms.Label();
            this.StringUsage = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit6 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StringUsage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 318);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(213, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Drag and Drop Your Files into the box above";
            // 
            // Console
            // 
            this.Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(68)))));
            this.Console.ForeColor = System.Drawing.SystemColors.Info;
            this.Console.Location = new System.Drawing.Point(12, 337);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Console.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.Console.Size = new System.Drawing.Size(777, 244);
            this.Console.TabIndex = 2;
            this.Console.Text = "";
            // 
            // dragdrop
            // 
            this.dragdrop.AllowDrop = true;
            this.dragdrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dragdrop.BackgroundImage = global::GSC2SO.Properties.Resources.drag_and_drop_256;
            this.dragdrop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.dragdrop.Image = global::GSC2SO.Properties.Resources.drag_and_drop_256;
            this.dragdrop.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.dragdrop.Location = new System.Drawing.Point(12, 12);
            this.dragdrop.Name = "dragdrop";
            this.dragdrop.Size = new System.Drawing.Size(917, 300);
            this.dragdrop.TabIndex = 3;
            this.dragdrop.Click += new System.EventHandler(this.dragdrop_Click);
            // 
            // checkEdit1
            // 
            this.checkEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(795, 540);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Optimize Globals";
            this.checkEdit1.Size = new System.Drawing.Size(129, 19);
            this.checkEdit1.TabIndex = 4;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // canceler
            // 
            this.canceler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.canceler.Enabled = false;
            this.canceler.Location = new System.Drawing.Point(795, 401);
            this.canceler.Name = "canceler";
            this.canceler.Size = new System.Drawing.Size(129, 33);
            this.canceler.TabIndex = 5;
            this.canceler.Text = "Cancel";
            this.canceler.Visible = false;
            this.canceler.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // GSCFiles
            // 
            this.GSCFiles.FileName = "openFileDialog1";
            // 
            // checkEdit2
            // 
            this.checkEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit2.Location = new System.Drawing.Point(795, 515);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "Compile for PC";
            this.checkEdit2.Size = new System.Drawing.Size(129, 19);
            this.checkEdit2.TabIndex = 6;
            this.checkEdit2.CheckedChanged += new System.EventHandler(this.checkEdit2_CheckedChanged);
            // 
            // AttachHeader
            // 
            this.AttachHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AttachHeader.Location = new System.Drawing.Point(795, 362);
            this.AttachHeader.Name = "AttachHeader";
            this.AttachHeader.Size = new System.Drawing.Size(129, 33);
            this.AttachHeader.TabIndex = 7;
            this.AttachHeader.Text = "Attach Header";
            this.AttachHeader.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // ClearHeaders
            // 
            this.ClearHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearHeaders.Enabled = false;
            this.ClearHeaders.Location = new System.Drawing.Point(795, 401);
            this.ClearHeaders.Name = "ClearHeaders";
            this.ClearHeaders.Size = new System.Drawing.Size(129, 33);
            this.ClearHeaders.TabIndex = 8;
            this.ClearHeaders.Text = "Clear Headers";
            this.ClearHeaders.Visible = false;
            this.ClearHeaders.Click += new System.EventHandler(this.ClearHeaders_Click);
            // 
            // checkEdit3
            // 
            this.checkEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit3.EditValue = true;
            this.checkEdit3.Location = new System.Drawing.Point(795, 490);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "Default Protections";
            this.checkEdit3.Size = new System.Drawing.Size(129, 19);
            this.checkEdit3.TabIndex = 9;
            this.checkEdit3.CheckedChanged += new System.EventHandler(this.checkEdit3_CheckedChanged);
            // 
            // checkEdit4
            // 
            this.checkEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit4.EditValue = true;
            this.checkEdit4.Location = new System.Drawing.Point(795, 465);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "Remove .txt";
            this.checkEdit4.Size = new System.Drawing.Size(129, 19);
            this.checkEdit4.TabIndex = 10;
            this.checkEdit4.CheckedChanged += new System.EventHandler(this.checkEdit4_CheckedChanged);
            // 
            // checkEdit5
            // 
            this.checkEdit5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit5.EditValue = true;
            this.checkEdit5.Location = new System.Drawing.Point(795, 440);
            this.checkEdit5.Name = "checkEdit5";
            this.checkEdit5.Properties.Caption = "Use Symbols";
            this.checkEdit5.Size = new System.Drawing.Size(129, 19);
            this.checkEdit5.TabIndex = 11;
            this.checkEdit5.CheckedChanged += new System.EventHandler(this.checkEdit5_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(638, 318);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 13);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "Estimated String Count:";
            // 
            // StringCount
            // 
            this.StringCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StringCount.AutoSize = true;
            this.StringCount.Location = new System.Drawing.Point(758, 318);
            this.StringCount.Name = "StringCount";
            this.StringCount.Size = new System.Drawing.Size(13, 13);
            this.StringCount.TabIndex = 13;
            this.StringCount.Text = "0";
            this.StringCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StringUsage
            // 
            this.StringUsage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StringUsage.Location = new System.Drawing.Point(795, 562);
            this.StringUsage.Name = "StringUsage";
            this.StringUsage.Properties.Caption = "Show String Usage";
            this.StringUsage.Size = new System.Drawing.Size(129, 19);
            this.StringUsage.TabIndex = 14;
            this.StringUsage.CheckedChanged += new System.EventHandler(this.StringUsage_CheckedChanged);
            // 
            // checkEdit6
            // 
            this.checkEdit6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit6.Location = new System.Drawing.Point(795, 337);
            this.checkEdit6.Name = "checkEdit6";
            this.checkEdit6.Properties.Caption = "Compile Only";
            this.checkEdit6.Size = new System.Drawing.Size(129, 19);
            this.checkEdit6.TabIndex = 15;
            this.checkEdit6.CheckedChanged += new System.EventHandler(this.checkEdit6_CheckedChanged);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 593);
            this.Controls.Add(this.checkEdit6);
            this.Controls.Add(this.StringUsage);
            this.Controls.Add(this.StringCount);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.checkEdit5);
            this.Controls.Add(this.checkEdit4);
            this.Controls.Add(this.checkEdit3);
            this.Controls.Add(this.ClearHeaders);
            this.Controls.Add(this.AttachHeader);
            this.Controls.Add(this.checkEdit2);
            this.Controls.Add(this.canceler);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.dragdrop);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Sharp Plus";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GSC2 String Optimizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StringUsage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.SaveFileDialog SaveGSCSelector;
        private DevExpress.XtraEditors.SimpleButton dragdrop;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private System.ComponentModel.BackgroundWorker BackgroundWorker_gsc;
        private DevExpress.XtraEditors.SimpleButton canceler;
        private System.Windows.Forms.OpenFileDialog GSCFiles;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.SimpleButton AttachHeader;
        private DevExpress.XtraEditors.SimpleButton ClearHeaders;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.CheckEdit checkEdit4;
        private DevExpress.XtraEditors.CheckEdit checkEdit5;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.Label StringCount;
        private DevExpress.XtraEditors.CheckEdit StringUsage;
        private DevExpress.XtraEditors.CheckEdit checkEdit6;
    }
}


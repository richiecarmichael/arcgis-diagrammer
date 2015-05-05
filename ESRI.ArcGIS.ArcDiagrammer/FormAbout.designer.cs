namespace ESRI.ArcGIS.ArcDiagrammer
{
    partial class FormAbout
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
            this.linkLabelContactAuthor = new System.Windows.Forms.LinkLabel();
            this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
            this.labelDevelopedBy = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelApplicationVersion = new System.Windows.Forms.Label();
            this.labelApplicationName = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.pictureBoxBanner = new System.Windows.Forms.PictureBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelAttribution = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLabelContactAuthor
            // 
            this.linkLabelContactAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelContactAuthor.AutoSize = true;
            this.linkLabelContactAuthor.Location = new System.Drawing.Point(86, 211);
            this.linkLabelContactAuthor.Name = "linkLabelContactAuthor";
            this.linkLabelContactAuthor.Size = new System.Drawing.Size(82, 13);
            this.linkLabelContactAuthor.TabIndex = 19;
            this.linkLabelContactAuthor.TabStop = true;
            this.linkLabelContactAuthor.Text = "XContactAuthor";
            this.linkLabelContactAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // linkLabelWebsite
            // 
            this.linkLabelWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelWebsite.AutoSize = true;
            this.linkLabelWebsite.Location = new System.Drawing.Point(86, 191);
            this.linkLabelWebsite.Name = "linkLabelWebsite";
            this.linkLabelWebsite.Size = new System.Drawing.Size(53, 13);
            this.linkLabelWebsite.TabIndex = 18;
            this.linkLabelWebsite.TabStop = true;
            this.linkLabelWebsite.Text = "XWebsite";
            this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // labelDevelopedBy
            // 
            this.labelDevelopedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDevelopedBy.Location = new System.Drawing.Point(86, 55);
            this.labelDevelopedBy.Name = "labelDevelopedBy";
            this.labelDevelopedBy.Size = new System.Drawing.Size(361, 24);
            this.labelDevelopedBy.TabIndex = 16;
            this.labelDevelopedBy.Text = "XDeveloped By";
            // 
            // labelCopyright
            // 
            this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyright.Location = new System.Drawing.Point(86, 30);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(361, 23);
            this.labelCopyright.TabIndex = 15;
            this.labelCopyright.Text = "XCopyright";
            // 
            // labelApplicationVersion
            // 
            this.labelApplicationVersion.AutoSize = true;
            this.labelApplicationVersion.Location = new System.Drawing.Point(282, 8);
            this.labelApplicationVersion.Name = "labelApplicationVersion";
            this.labelApplicationVersion.Size = new System.Drawing.Size(79, 13);
            this.labelApplicationVersion.TabIndex = 14;
            this.labelApplicationVersion.Text = "XVersion X.X.X";
            // 
            // labelApplicationName
            // 
            this.labelApplicationName.AutoSize = true;
            this.labelApplicationName.Location = new System.Drawing.Point(86, 8);
            this.labelApplicationName.Name = "labelApplicationName";
            this.labelApplicationName.Size = new System.Drawing.Size(97, 13);
            this.labelApplicationName.TabIndex = 13;
            this.labelApplicationName.Text = "XApplication Name";
            // 
            // labelWarning
            // 
            this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWarning.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWarning.Location = new System.Drawing.Point(86, 118);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(361, 73);
            this.labelWarning.TabIndex = 17;
            this.labelWarning.Text = "XWarning";
            // 
            // pictureBoxBanner
            // 
            this.pictureBoxBanner.Image = global::ESRI.ArcGIS.ArcDiagrammer.Properties.Resources.BITMAP_ESRI_LOGO;
            this.pictureBoxBanner.Location = new System.Drawing.Point(-7, -16);
            this.pictureBoxBanner.Name = "pictureBoxBanner";
            this.pictureBoxBanner.Size = new System.Drawing.Size(100, 134);
            this.pictureBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxBanner.TabIndex = 12;
            this.pictureBoxBanner.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(382, 202);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(72, 24);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "XOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // labelAttribution
            // 
            this.labelAttribution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAttribution.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAttribution.Location = new System.Drawing.Point(86, 79);
            this.labelAttribution.Name = "labelAttribution";
            this.labelAttribution.Size = new System.Drawing.Size(361, 39);
            this.labelAttribution.TabIndex = 20;
            this.labelAttribution.Text = "XAttribution";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 234);
            this.Controls.Add(this.labelAttribution);
            this.Controls.Add(this.linkLabelContactAuthor);
            this.Controls.Add(this.linkLabelWebsite);
            this.Controls.Add(this.labelDevelopedBy);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelApplicationVersion);
            this.Controls.Add(this.labelApplicationName);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.pictureBoxBanner);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "X";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelContactAuthor;
        private System.Windows.Forms.LinkLabel linkLabelWebsite;
        private System.Windows.Forms.Label labelDevelopedBy;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelApplicationVersion;
        private System.Windows.Forms.Label labelApplicationName;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.PictureBox pictureBoxBanner;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelAttribution;

    }
}
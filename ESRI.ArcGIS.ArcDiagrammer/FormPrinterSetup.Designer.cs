namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class FormPrinterSetup {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.sandBarManager1 = new TD.SandBar.SandBarManager(this.components);
            this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
            this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
            this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
            this.topSandBarDock = new TD.SandBar.ToolBarContainer();
            this.menuBar1 = new TD.SandBar.MenuBar();
            this.contextMenuBarItemGrid = new TD.SandBar.ContextMenuBarItem();
            this.menuButtonItemReset = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemDescription = new TD.SandBar.MenuButtonItem();
            this.statusBar1 = new TD.SandBar.StatusBar();
            this.statusBarItem1 = new TD.SandBar.StatusBarItem();
            this.propertyPrinter = new System.Windows.Forms.PropertyGrid();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.topSandBarDock.SuspendLayout();
            this.SuspendLayout();
            // 
            // sandBarManager1
            // 
            this.sandBarManager1.OwnerForm = this;
            this.sandBarManager1.Renderer = new TD.SandBar.Office2007Renderer();
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("64a1fd27-890b-4197-9207-788aadd8f97d");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 23);
            this.leftSandBarDock.Manager = this.sandBarManager1;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 316);
            this.leftSandBarDock.TabIndex = 0;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("28770843-dda5-4421-aebb-ba9fec09b5fe");
            this.rightSandBarDock.Location = new System.Drawing.Point(421, 23);
            this.rightSandBarDock.Manager = this.sandBarManager1;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 316);
            this.rightSandBarDock.TabIndex = 1;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("7012c61c-9177-4ec7-87ce-5676552305af");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 339);
            this.bottomSandBarDock.Manager = this.sandBarManager1;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(421, 0);
            this.bottomSandBarDock.TabIndex = 2;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.menuBar1);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("9f318d61-ddae-4a2b-81d0-33be758cf6f0");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager1;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(421, 23);
            this.topSandBarDock.TabIndex = 3;
            // 
            // menuBar1
            // 
            this.menuBar1.Guid = new System.Guid("11796dca-aa1e-4e7b-9f13-7537e8295888");
            this.menuBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.contextMenuBarItemGrid});
            this.menuBar1.Location = new System.Drawing.Point(2, 0);
            this.menuBar1.Name = "menuBar1";
            this.menuBar1.OwnerForm = this;
            this.menuBar1.Size = new System.Drawing.Size(419, 23);
            this.menuBar1.TabIndex = 0;
            this.menuBar1.Text = "menuBar1";
            this.menuBar1.Visible = false;
            // 
            // contextMenuBarItemGrid
            // 
            this.contextMenuBarItemGrid.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuButtonItemReset,
            this.menuButtonItemDescription});
            this.contextMenuBarItemGrid.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.MenuItem_BeforePopup);
            // 
            // menuButtonItemReset
            // 
            this.menuButtonItemReset.Text = "XReset";
            this.menuButtonItemReset.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemDescription
            // 
            this.menuButtonItemDescription.BeginGroup = true;
            this.menuButtonItemDescription.Text = "XDescription";
            this.menuButtonItemDescription.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // statusBar1
            // 
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar1.Guid = new System.Guid("10575ba7-479a-438a-ad10-870c6086c2f8");
            this.statusBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.statusBarItem1});
            this.statusBar1.Location = new System.Drawing.Point(0, 339);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.OwnerForm = this;
            this.statusBar1.Renderer = new TD.SandBar.Office2007Renderer();
            this.statusBar1.Size = new System.Drawing.Size(421, 19);
            this.statusBar1.TabIndex = 4;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarItem1
            // 
            this.statusBarItem1.Stretch = true;
            this.statusBarItem1.Text = "";
            // 
            // propertyPrinter
            // 
            this.propertyPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyPrinter.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyPrinter.LineColor = System.Drawing.Color.Silver;
            this.propertyPrinter.Location = new System.Drawing.Point(0, 0);
            this.propertyPrinter.Name = "propertyPrinter";
            this.propertyPrinter.Size = new System.Drawing.Size(421, 304);
            this.propertyPrinter.TabIndex = 12;
            this.propertyPrinter.ToolbarVisible = false;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Location = new System.Drawing.Point(12, 310);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 16;
            this.buttonReset.Text = "XReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(172, 310);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 15;
            this.buttonOK.Text = "XOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(253, 310);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "XCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(334, 310);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 13;
            this.buttonApply.Text = "XApply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.Button_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // FormPrinterSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 358);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.propertyPrinter);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Controls.Add(this.statusBar1);
            this.Name = "FormPrinterSetup";
            this.Text = "XPageSetup";
            this.topSandBarDock.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TD.SandBar.SandBarManager sandBarManager1;
        private TD.SandBar.ToolBarContainer leftSandBarDock;
        private TD.SandBar.ToolBarContainer rightSandBarDock;
        private TD.SandBar.ToolBarContainer bottomSandBarDock;
        private TD.SandBar.ToolBarContainer topSandBarDock;
        private TD.SandBar.MenuBar menuBar1;
        private TD.SandBar.StatusBar statusBar1;
        private TD.SandBar.StatusBarItem statusBarItem1;
        private System.Windows.Forms.PropertyGrid propertyPrinter;
        private TD.SandBar.ContextMenuBarItem contextMenuBarItemGrid;
        private TD.SandBar.MenuButtonItem menuButtonItemReset;
        private TD.SandBar.MenuButtonItem menuButtonItemDescription;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
    }
}
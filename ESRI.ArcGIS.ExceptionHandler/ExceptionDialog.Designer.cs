namespace ESRI.ArcGIS.ExceptionHandler
{
	partial class ExceptionDialog
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sandBarManager1 = new TD.SandBar.SandBarManager(this.components);
            this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
            this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
            this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
            this.topSandBarDock = new TD.SandBar.ToolBarContainer();
            this.menuBar1 = new TD.SandBar.MenuBar();
            this.menuBarItemFile = new TD.SandBar.MenuBarItem();
            this.menuButtonItemExit = new TD.SandBar.MenuButtonItem();
            this.toolBar1 = new TD.SandBar.ToolBar();
            this.buttonItemNew = new TD.SandBar.ButtonItem();
            this.buttonItemSave = new TD.SandBar.ButtonItem();
            this.buttonItemCopy = new TD.SandBar.ButtonItem();
            this.buttonItemPlay = new TD.SandBar.ButtonItem();
            this.buttonItemPause = new TD.SandBar.ButtonItem();
            this.buttonItemMessage = new TD.SandBar.ButtonItem();
            this.statusBar1 = new TD.SandBar.StatusBar();
            this.statusBarItemMain = new TD.SandBar.StatusBarItem();
            this.buttonItemPrint = new TD.SandBar.ButtonItem();
            this.buttonItemPrintPreview = new TD.SandBar.ButtonItem();
            this.buttonItemPageSetup = new TD.SandBar.ButtonItem();
            this.topSandBarDock.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // sandBarManager1
            // 
            this.sandBarManager1.OwnerForm = this;
            this.sandBarManager1.Renderer = new TD.SandBar.Office2007Renderer();
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("017254f8-46f6-4194-80ae-13d48e5764a6");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 46);
            this.leftSandBarDock.Manager = this.sandBarManager1;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 298);
            this.leftSandBarDock.TabIndex = 0;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("bde8d596-7e90-46e9-89a5-e7d365f05a8f");
            this.rightSandBarDock.Location = new System.Drawing.Point(495, 46);
            this.rightSandBarDock.Manager = this.sandBarManager1;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 298);
            this.rightSandBarDock.TabIndex = 1;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("3da831bc-1101-403f-b189-3e609f9cab56");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 344);
            this.bottomSandBarDock.Manager = this.sandBarManager1;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(495, 0);
            this.bottomSandBarDock.TabIndex = 2;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.menuBar1);
            this.topSandBarDock.Controls.Add(this.toolBar1);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("0e2e7d44-a16b-44fb-a03a-ca4922b4e4a3");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager1;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(495, 46);
            this.topSandBarDock.TabIndex = 3;
            // 
            // menuBar1
            // 
            this.menuBar1.Guid = new System.Guid("960d68bc-bc13-459c-aa73-ea3aacdb245a");
            this.menuBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuBarItemFile});
            this.menuBar1.Location = new System.Drawing.Point(2, 0);
            this.menuBar1.Name = "menuBar1";
            this.menuBar1.OwnerForm = this;
            this.menuBar1.Size = new System.Drawing.Size(493, 23);
            this.menuBar1.TabIndex = 0;
            this.menuBar1.Text = "menuBar1";
            // 
            // menuBarItemFile
            // 
            this.menuBarItemFile.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuButtonItemExit});
            this.menuBarItemFile.Text = "XFile";
            this.menuBarItemFile.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.MenuBarItem_BeforePopup);
            // 
            // menuButtonItemExit
            // 
            this.menuButtonItemExit.Text = "XExit";
            this.menuButtonItemExit.Activate += new System.EventHandler(this.MenuButtonItem_Activate);
            // 
            // toolBar1
            // 
            this.toolBar1.Closable = false;
            this.toolBar1.DockLine = 1;
            this.toolBar1.Guid = new System.Guid("722d3381-bf09-4948-8085-279e1bc64454");
            this.toolBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.buttonItemNew,
            this.buttonItemSave,
            this.buttonItemMessage,
            this.buttonItemPrint,
            this.buttonItemPrintPreview,
            this.buttonItemPageSetup,
            this.buttonItemCopy,
            this.buttonItemPlay,
            this.buttonItemPause});
            this.toolBar1.Location = new System.Drawing.Point(2, 23);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(474, 23);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.Text = "toolBar1";
            this.toolBar1.ButtonClick += new TD.SandBar.ToolBar.ButtonClickEventHandler(this.ToolBar_ButtonClick);
            // 
            // buttonItemNew
            // 
            this.buttonItemNew.Text = "XNnew";
            // 
            // buttonItemSave
            // 
            this.buttonItemSave.Text = "XSave";
            // 
            // buttonItemCopy
            // 
            this.buttonItemCopy.BeginGroup = true;
            this.buttonItemCopy.Text = "XCopy";
            // 
            // buttonItemPlay
            // 
            this.buttonItemPlay.BeginGroup = true;
            this.buttonItemPlay.Text = "XPlay";
            // 
            // buttonItemPause
            // 
            this.buttonItemPause.Text = "XPause";
            // 
            // buttonItemMessage
            // 
            this.buttonItemMessage.Text = "XMessage";
            // 
            // statusBar1
            // 
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar1.Guid = new System.Guid("a9c47137-4c3f-44ba-bf9e-ab42b0e4f254");
            this.statusBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.statusBarItemMain});
            this.statusBar1.Location = new System.Drawing.Point(0, 344);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.OwnerForm = this;
            this.statusBar1.Renderer = new TD.SandBar.Office2007Renderer();
            this.statusBar1.Size = new System.Drawing.Size(495, 19);
            this.statusBar1.TabIndex = 4;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarItemMain
            // 
            this.statusBarItemMain.Stretch = true;
            this.statusBarItemMain.Text = "XReady";
            // 
            // buttonItemPrint
            // 
            this.buttonItemPrint.BeginGroup = true;
            this.buttonItemPrint.Text = "XPrint";
            // 
            // buttonItemPrintPreview
            // 
            this.buttonItemPrintPreview.Text = "XPrintPreview";
            // 
            // buttonItemPageSetup
            // 
            this.buttonItemPageSetup.Text = "XPageSetup";
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 363);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Controls.Add(this.statusBar1);
            this.MinimizeBox = false;
            this.Name = "ExceptionDialog";
            this.Text = "X";
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.Form_VisibleChanged);
            this.topSandBarDock.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Timer timer1;
        private TD.SandBar.SandBarManager sandBarManager1;
        private TD.SandBar.ToolBarContainer leftSandBarDock;
        private TD.SandBar.ToolBarContainer rightSandBarDock;
        private TD.SandBar.ToolBarContainer bottomSandBarDock;
        private TD.SandBar.ToolBarContainer topSandBarDock;
        private TD.SandBar.MenuBar menuBar1;
        private TD.SandBar.MenuBarItem menuBarItemFile;
        private TD.SandBar.ToolBar toolBar1;
        private TD.SandBar.StatusBar statusBar1;
        private TD.SandBar.StatusBarItem statusBarItemMain;
        private TD.SandBar.MenuButtonItem menuButtonItemExit;
        private TD.SandBar.ButtonItem buttonItemPlay;
        private TD.SandBar.ButtonItem buttonItemPause;
        private TD.SandBar.ButtonItem buttonItemMessage;
        private TD.SandBar.ButtonItem buttonItemSave;
        private TD.SandBar.ButtonItem buttonItemCopy;
        private TD.SandBar.ButtonItem buttonItemNew;
        private TD.SandBar.ButtonItem buttonItemPrint;
        private TD.SandBar.ButtonItem buttonItemPrintPreview;
        private TD.SandBar.ButtonItem buttonItemPageSetup;

	}
}
namespace ESRI.ArcGIS.Diagrammer {
    partial class Catalog {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sandBarManager1 = new TD.SandBar.SandBarManager(this.components);
            this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
            this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
            this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
            this.topSandBarDock = new TD.SandBar.ToolBarContainer();
            this.menuBar1 = new TD.SandBar.MenuBar();
            this.contextMenuBarItem1 = new TD.SandBar.ContextMenuBarItem();
            this.menuButtonItemScroll = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemFlash = new TD.SandBar.MenuButtonItem();
            this.toolBar1 = new TD.SandBar.ToolBar();
            this.buttonItemCatalog = new TD.SandBar.ButtonItem();
            this.buttonItemCategorized = new TD.SandBar.ButtonItem();
            this.buttonItemAlphabetical = new TD.SandBar.ButtonItem();
            this.buttonItemRefresh = new TD.SandBar.ButtonItem();
            this.topSandBarDock.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 45);
            this.treeView1.Name = "treeView1";
            this.menuBar1.SetSandBarMenu(this.treeView1, this.contextMenuBarItem1);
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(194, 280);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseDoubleClick);
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCollapse);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeView_KeyDown);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterExpand);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // sandBarManager1
            // 
            this.sandBarManager1.OwnerForm = null;
            this.sandBarManager1.Renderer = new TD.SandBar.Office2007Renderer();
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("4b7f7f18-5dd2-49c5-9868-957836401af0");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 45);
            this.leftSandBarDock.Manager = this.sandBarManager1;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 280);
            this.leftSandBarDock.TabIndex = 1;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("cd351d83-6ca8-48d6-a944-7149ea989f2e");
            this.rightSandBarDock.Location = new System.Drawing.Point(194, 45);
            this.rightSandBarDock.Manager = this.sandBarManager1;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 280);
            this.rightSandBarDock.TabIndex = 2;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("690903a6-bcf1-4d2e-8b03-b592ae118e46");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 325);
            this.bottomSandBarDock.Manager = this.sandBarManager1;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(194, 0);
            this.bottomSandBarDock.TabIndex = 3;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.menuBar1);
            this.topSandBarDock.Controls.Add(this.toolBar1);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("9bd05ad0-6342-40b1-b1f6-7247947b6dd1");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager1;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(194, 45);
            this.topSandBarDock.TabIndex = 4;
            // 
            // menuBar1
            // 
            this.menuBar1.Guid = new System.Guid("c5b52640-47c4-4252-a900-c2d73a9ef9a9");
            this.menuBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.contextMenuBarItem1});
            this.menuBar1.Location = new System.Drawing.Point(2, 0);
            this.menuBar1.Name = "menuBar1";
            this.menuBar1.OwnerForm = null;
            this.menuBar1.Size = new System.Drawing.Size(192, 23);
            this.menuBar1.TabIndex = 2;
            this.menuBar1.Text = "menuBar1";
            this.menuBar1.Visible = false;
            // 
            // contextMenuBarItem1
            // 
            this.contextMenuBarItem1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuButtonItemScroll,
            this.menuButtonItemFlash});
            this.contextMenuBarItem1.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.ContextMenu_BeforePopup);
            // 
            // menuButtonItemScroll
            // 
            this.menuButtonItemScroll.BeginGroup = true;
            this.menuButtonItemScroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuButtonItemScroll.Text = "XScroll";
            this.menuButtonItemScroll.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemFlash
            // 
            this.menuButtonItemFlash.Text = "XFlash";
            this.menuButtonItemFlash.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // toolBar1
            // 
            this.toolBar1.AllowVerticalDock = false;
            this.toolBar1.Closable = false;
            this.toolBar1.DockLine = 1;
            this.toolBar1.Guid = new System.Guid("54ce7fb3-262e-4ba4-a9d4-2e4cbe408d94");
            this.toolBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.buttonItemCatalog,
            this.buttonItemCategorized,
            this.buttonItemAlphabetical,
            this.buttonItemRefresh});
            this.toolBar1.Location = new System.Drawing.Point(2, 23);
            this.toolBar1.Movable = false;
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(192, 22);
            this.toolBar1.Stretch = true;
            this.toolBar1.TabIndex = 1;
            this.toolBar1.Tearable = false;
            this.toolBar1.Text = "toolBar1";
            this.toolBar1.ButtonClick += new TD.SandBar.ToolBar.ButtonClickEventHandler(this.ToolBar_ButtonClick);
            // 
            // buttonItemCatalog
            // 
            this.buttonItemCatalog.Checked = true;
            // 
            // buttonItemRefresh
            // 
            this.buttonItemRefresh.BeginGroup = true;
            // 
            // Catalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Name = "Catalog";
            this.Size = new System.Drawing.Size(194, 325);
            this.topSandBarDock.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private TD.SandBar.SandBarManager sandBarManager1;
        private TD.SandBar.ToolBarContainer leftSandBarDock;
        private TD.SandBar.ToolBarContainer rightSandBarDock;
        private TD.SandBar.ToolBarContainer bottomSandBarDock;
        private TD.SandBar.ToolBarContainer topSandBarDock;
        private TD.SandBar.ToolBar toolBar1;
        private TD.SandBar.ButtonItem buttonItemCatalog;
        private TD.SandBar.ButtonItem buttonItemCategorized;
        private TD.SandBar.ButtonItem buttonItemAlphabetical;
        private TD.SandBar.ButtonItem buttonItemRefresh;
        private System.Windows.Forms.ImageList imageList1;
        private TD.SandBar.MenuBar menuBar1;
        private TD.SandBar.ContextMenuBarItem contextMenuBarItem1;
        private TD.SandBar.MenuButtonItem menuButtonItemScroll;
        private TD.SandBar.MenuButtonItem menuButtonItemFlash;
    }
}

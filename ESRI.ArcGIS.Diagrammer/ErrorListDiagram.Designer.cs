namespace ESRI.ArcGIS.Diagrammer {
    partial class ErrorListDiagram {
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
            this.listViewErrorList = new System.Windows.Forms.ListView();
            this.imageListErrorList = new System.Windows.Forms.ImageList(this.components);
            this.sandBarManager1 = new TD.SandBar.SandBarManager(this.components);
            this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
            this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
            this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
            this.topSandBarDock = new TD.SandBar.ToolBarContainer();
            this.menuBar1 = new TD.SandBar.MenuBar();
            this.contextMenuBarItemErrorList = new TD.SandBar.ContextMenuBarItem();
            this.menuButtonItemScroll = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemSelect = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemFlashError = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemClearError = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemClearAllErrors = new TD.SandBar.MenuButtonItem();
            this.toolBarErrorList = new TD.SandBar.ToolBar();
            this.buttonItemErrors = new TD.SandBar.ButtonItem();
            this.buttonItemWarnings = new TD.SandBar.ButtonItem();
            this.dropDownMenuItemValidator = new TD.SandBar.DropDownMenuItem();
            this.menuButtonItemPGdb = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemFGdb = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemSdeConnection = new TD.SandBar.MenuButtonItem();
            this.menuButtonItemSelectGeodatabase = new TD.SandBar.MenuButtonItem();
            this.topSandBarDock.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewErrorList
            // 
            this.listViewErrorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewErrorList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewErrorList.Location = new System.Drawing.Point(0, 44);
            this.listViewErrorList.Name = "listViewErrorList";
            this.menuBar1.SetSandBarMenu(this.listViewErrorList, this.contextMenuBarItemErrorList);
            this.listViewErrorList.Size = new System.Drawing.Size(344, 229);
            this.listViewErrorList.SmallImageList = this.imageListErrorList;
            this.listViewErrorList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewErrorList.TabIndex = 1;
            this.listViewErrorList.UseCompatibleStateImageBehavior = false;
            this.listViewErrorList.View = System.Windows.Forms.View.Details;
            this.listViewErrorList.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
            // 
            // imageListErrorList
            // 
            this.imageListErrorList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListErrorList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListErrorList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // sandBarManager1
            // 
            this.sandBarManager1.OwnerForm = null;
            this.sandBarManager1.Renderer = new TD.SandBar.Office2007Renderer();
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("25f15d36-3656-4bc6-96e1-942c0531283a");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 23);
            this.leftSandBarDock.Manager = this.sandBarManager1;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 250);
            this.leftSandBarDock.TabIndex = 2;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("91b8c14f-d917-4a5f-91f7-53440c17294f");
            this.rightSandBarDock.Location = new System.Drawing.Point(344, 23);
            this.rightSandBarDock.Manager = this.sandBarManager1;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 250);
            this.rightSandBarDock.TabIndex = 3;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("9e794b83-5fbb-4dda-b9fe-f17c0f42efb2");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 273);
            this.bottomSandBarDock.Manager = this.sandBarManager1;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(344, 0);
            this.bottomSandBarDock.TabIndex = 4;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.menuBar1);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("755adc2b-ec85-452e-8f8e-ea3fba129910");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager1;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(344, 23);
            this.topSandBarDock.TabIndex = 5;
            // 
            // menuBar1
            // 
            this.menuBar1.Guid = new System.Guid("97f544e5-21a3-4a78-8df5-990153abc768");
            this.menuBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.contextMenuBarItemErrorList});
            this.menuBar1.Location = new System.Drawing.Point(2, 0);
            this.menuBar1.Name = "menuBar1";
            this.menuBar1.OwnerForm = null;
            this.menuBar1.Size = new System.Drawing.Size(342, 23);
            this.menuBar1.TabIndex = 3;
            this.menuBar1.Text = "menuBar1";
            this.menuBar1.Visible = false;
            // 
            // contextMenuBarItemErrorList
            // 
            this.contextMenuBarItemErrorList.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuButtonItemScroll,
            this.menuButtonItemSelect,
            this.menuButtonItemFlashError,
            this.menuButtonItemClearError,
            this.menuButtonItemClearAllErrors});
            this.contextMenuBarItemErrorList.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.MenuItem_BeforePopup);
            // 
            // menuButtonItemScroll
            // 
            this.menuButtonItemScroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuButtonItemScroll.Text = "XScroll";
            this.menuButtonItemScroll.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemSelect
            // 
            this.menuButtonItemSelect.Text = "XSelect";
            this.menuButtonItemSelect.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemFlashError
            // 
            this.menuButtonItemFlashError.Text = "XFlash";
            this.menuButtonItemFlashError.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemClearError
            // 
            this.menuButtonItemClearError.Text = "XClear";
            this.menuButtonItemClearError.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemClearAllErrors
            // 
            this.menuButtonItemClearAllErrors.Text = "XClear All";
            this.menuButtonItemClearAllErrors.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // toolBarErrorList
            // 
            this.toolBarErrorList.AllowVerticalDock = false;
            this.toolBarErrorList.Closable = false;
            this.toolBarErrorList.Guid = new System.Guid("86668989-d5f6-4ad5-bb47-f90dd00bcdff");
            this.toolBarErrorList.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.buttonItemErrors,
            this.buttonItemWarnings,
            this.dropDownMenuItemValidator});
            this.toolBarErrorList.Location = new System.Drawing.Point(0, 23);
            this.toolBarErrorList.Movable = false;
            this.toolBarErrorList.Name = "toolBarErrorList";
            this.toolBarErrorList.Renderer = new TD.SandBar.Office2007Renderer();
            this.toolBarErrorList.Size = new System.Drawing.Size(344, 21);
            this.toolBarErrorList.Stretch = true;
            this.toolBarErrorList.TabIndex = 2;
            this.toolBarErrorList.Tearable = false;
            this.toolBarErrorList.Text = "";
            this.toolBarErrorList.ButtonClick += new TD.SandBar.ToolBar.ButtonClickEventHandler(this.ToolBar_ButtonClick);
            // 
            // buttonItemErrors
            // 
            this.buttonItemErrors.AutoToggle = TD.SandBar.AutoToggleType.Single;
            this.buttonItemErrors.Checked = true;
            this.buttonItemErrors.Text = "X0 Errors";
            // 
            // buttonItemWarnings
            // 
            this.buttonItemWarnings.AutoToggle = TD.SandBar.AutoToggleType.Single;
            this.buttonItemWarnings.BeginGroup = true;
            this.buttonItemWarnings.Checked = true;
            this.buttonItemWarnings.Text = "X0 Warnings";
            // 
            // dropDownMenuItemValidator
            // 
            this.dropDownMenuItemValidator.BeginGroup = true;
            this.dropDownMenuItemValidator.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.menuButtonItemFGdb,
            this.menuButtonItemPGdb,
            this.menuButtonItemSdeConnection,
            this.menuButtonItemSelectGeodatabase});
            this.dropDownMenuItemValidator.Text = "XValidator";
            this.dropDownMenuItemValidator.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.MenuItem_BeforePopup);
            // 
            // menuButtonItemPGdb
            // 
            this.menuButtonItemPGdb.Text = "XPersonal Geodatabase";
            this.menuButtonItemPGdb.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemFGdb
            // 
            this.menuButtonItemFGdb.Text = "XFile Geodatabase";
            this.menuButtonItemFGdb.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // menuButtonItemSdeConnection
            // 
            this.menuButtonItemSdeConnection.BeginGroup = true;
            this.menuButtonItemSdeConnection.Text = "XSDE Connection";
            this.menuButtonItemSdeConnection.BeforePopup += new TD.SandBar.MenuItemBase.BeforePopupEventHandler(this.MenuItem_BeforePopup);
            // 
            // menuButtonItemSelectGeodatabase
            // 
            this.menuButtonItemSelectGeodatabase.BeginGroup = true;
            this.menuButtonItemSelectGeodatabase.Text = "XSelect...";
            this.menuButtonItemSelectGeodatabase.Activate += new System.EventHandler(this.MenuItem_Activate);
            // 
            // ErrorListDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewErrorList);
            this.Controls.Add(this.toolBarErrorList);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Name = "ErrorListDiagram";
            this.Size = new System.Drawing.Size(344, 273);
            this.topSandBarDock.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewErrorList;
        private TD.SandBar.SandBarManager sandBarManager1;
        private TD.SandBar.ToolBarContainer leftSandBarDock;
        private TD.SandBar.ToolBarContainer rightSandBarDock;
        private TD.SandBar.ToolBarContainer bottomSandBarDock;
        private TD.SandBar.ToolBarContainer topSandBarDock;
        private TD.SandBar.ToolBar toolBarErrorList;
        private TD.SandBar.ButtonItem buttonItemErrors;
        private TD.SandBar.ButtonItem buttonItemWarnings;
        private TD.SandBar.DropDownMenuItem dropDownMenuItemValidator;
        private TD.SandBar.MenuButtonItem menuButtonItemPGdb;
        private TD.SandBar.MenuButtonItem menuButtonItemFGdb;
        private TD.SandBar.MenuButtonItem menuButtonItemSdeConnection;
        private TD.SandBar.MenuButtonItem menuButtonItemSelectGeodatabase;
        private TD.SandBar.ContextMenuBarItem contextMenuBarItemErrorList;
        private TD.SandBar.MenuButtonItem menuButtonItemScroll;
        private TD.SandBar.MenuButtonItem menuButtonItemFlashError;
        private TD.SandBar.MenuButtonItem menuButtonItemClearError;
        private TD.SandBar.MenuButtonItem menuButtonItemClearAllErrors;
        private System.Windows.Forms.ImageList imageListErrorList;
        private TD.SandBar.MenuBar menuBar1;
        private TD.SandBar.MenuButtonItem menuButtonItemSelect;
    }
}

namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class FormOptions {
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
            this.tabControlOptions = new TD.SandDock.TabControl();
            this.tabPageDiagramColors = new TD.SandDock.TabPage();
            this.propertyGridColors = new System.Windows.Forms.PropertyGrid();
            this.tabPageLayout = new TD.SandDock.TabPage();
            this.tabControlLayout = new TD.SandDock.TabControl();
            this.tabPageCircular = new TD.SandDock.TabPage();
            this.propertyGridCircular = new System.Windows.Forms.PropertyGrid();
            this.tabPageForcedDirect = new TD.SandDock.TabPage();
            this.propertyGridForcedDirect = new System.Windows.Forms.PropertyGrid();
            this.tabPageHierarchical = new TD.SandDock.TabPage();
            this.propertyGridHierarchical = new System.Windows.Forms.PropertyGrid();
            this.tabPageOrthogonal = new TD.SandDock.TabPage();
            this.propertyGridOrthogonal = new System.Windows.Forms.PropertyGrid();
            this.tabPageTree = new TD.SandDock.TabPage();
            this.propertyGridTree = new System.Windows.Forms.PropertyGrid();
            this.tabPageReport = new TD.SandDock.TabPage();
            this.tabControlReport = new TD.SandDock.TabControl();
            this.tabPageDataReport = new TD.SandDock.TabPage();
            this.propertyGridDataReport = new System.Windows.Forms.PropertyGrid();
            this.tabPageSchemaReport = new TD.SandDock.TabPage();
            this.propertyGridSchemaReport = new System.Windows.Forms.PropertyGrid();
            this.tabPageXmlReport = new TD.SandDock.TabPage();
            this.propertyGridXmlReport = new System.Windows.Forms.PropertyGrid();
            this.tabPageModel = new TD.SandDock.TabPage();
            this.propertyGridModel = new System.Windows.Forms.PropertyGrid();
            this.tabPageWindow = new TD.SandDock.TabPage();
            this.propertyGridWindow = new System.Windows.Forms.PropertyGrid();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonResetAll = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.tabControlOptions.SuspendLayout();
            this.tabPageDiagramColors.SuspendLayout();
            this.tabPageLayout.SuspendLayout();
            this.tabControlLayout.SuspendLayout();
            this.tabPageCircular.SuspendLayout();
            this.tabPageForcedDirect.SuspendLayout();
            this.tabPageHierarchical.SuspendLayout();
            this.tabPageOrthogonal.SuspendLayout();
            this.tabPageTree.SuspendLayout();
            this.tabPageReport.SuspendLayout();
            this.tabControlReport.SuspendLayout();
            this.tabPageDataReport.SuspendLayout();
            this.tabPageSchemaReport.SuspendLayout();
            this.tabPageXmlReport.SuspendLayout();
            this.tabPageModel.SuspendLayout();
            this.tabPageWindow.SuspendLayout();
            this.topSandBarDock.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlOptions
            // 
            this.tabControlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlOptions.BorderStyle = TD.SandDock.Rendering.BorderStyle.None;
            this.tabControlOptions.Controls.Add(this.tabPageDiagramColors);
            this.tabControlOptions.Controls.Add(this.tabPageLayout);
            this.tabControlOptions.Controls.Add(this.tabPageReport);
            this.tabControlOptions.Controls.Add(this.tabPageModel);
            this.tabControlOptions.Controls.Add(this.tabPageWindow);
            this.tabControlOptions.Location = new System.Drawing.Point(0, 0);
            this.tabControlOptions.Name = "tabControlOptions";
            this.tabControlOptions.Renderer = new TD.SandDock.Rendering.Office2007Renderer();
            this.tabControlOptions.Size = new System.Drawing.Size(446, 333);
            this.tabControlOptions.TabIndex = 1;
            // 
            // tabPageDiagramColors
            // 
            this.tabPageDiagramColors.Controls.Add(this.propertyGridColors);
            this.tabPageDiagramColors.Location = new System.Drawing.Point(3, 26);
            this.tabPageDiagramColors.Name = "tabPageDiagramColors";
            this.tabPageDiagramColors.Size = new System.Drawing.Size(440, 304);
            this.tabPageDiagramColors.TabIndex = 1;
            this.tabPageDiagramColors.Text = "XDiagramColors";
            // 
            // propertyGridColors
            // 
            this.propertyGridColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridColors.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridColors.LineColor = System.Drawing.Color.Silver;
            this.propertyGridColors.Location = new System.Drawing.Point(0, 0);
            this.propertyGridColors.Name = "propertyGridColors";
            this.propertyGridColors.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridColors, this.contextMenuBarItemGrid);
            this.propertyGridColors.Size = new System.Drawing.Size(440, 304);
            this.propertyGridColors.TabIndex = 11;
            this.propertyGridColors.ToolbarVisible = false;
            // 
            // tabPageLayout
            // 
            this.tabPageLayout.Controls.Add(this.tabControlLayout);
            this.tabPageLayout.Location = new System.Drawing.Point(3, 26);
            this.tabPageLayout.Name = "tabPageLayout";
            this.tabPageLayout.Size = new System.Drawing.Size(440, 304);
            this.tabPageLayout.TabIndex = 2;
            this.tabPageLayout.Text = "XLayout";
            // 
            // tabControlLayout
            // 
            this.tabControlLayout.BorderStyle = TD.SandDock.Rendering.BorderStyle.None;
            this.tabControlLayout.Controls.Add(this.tabPageCircular);
            this.tabControlLayout.Controls.Add(this.tabPageForcedDirect);
            this.tabControlLayout.Controls.Add(this.tabPageHierarchical);
            this.tabControlLayout.Controls.Add(this.tabPageOrthogonal);
            this.tabControlLayout.Controls.Add(this.tabPageTree);
            this.tabControlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLayout.Location = new System.Drawing.Point(0, 0);
            this.tabControlLayout.Name = "tabControlLayout";
            this.tabControlLayout.Renderer = new TD.SandDock.Rendering.Office2007Renderer();
            this.tabControlLayout.Size = new System.Drawing.Size(440, 304);
            this.tabControlLayout.TabIndex = 3;
            // 
            // tabPageCircular
            // 
            this.tabPageCircular.Controls.Add(this.propertyGridCircular);
            this.tabPageCircular.Location = new System.Drawing.Point(3, 26);
            this.tabPageCircular.Name = "tabPageCircular";
            this.tabPageCircular.Size = new System.Drawing.Size(434, 275);
            this.tabPageCircular.TabIndex = 2;
            this.tabPageCircular.Text = "XCircular";
            // 
            // propertyGridCircular
            // 
            this.propertyGridCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridCircular.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridCircular.LineColor = System.Drawing.Color.Silver;
            this.propertyGridCircular.Location = new System.Drawing.Point(0, 0);
            this.propertyGridCircular.Name = "propertyGridCircular";
            this.propertyGridCircular.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridCircular, this.contextMenuBarItemGrid);
            this.propertyGridCircular.Size = new System.Drawing.Size(434, 275);
            this.propertyGridCircular.TabIndex = 10;
            this.propertyGridCircular.ToolbarVisible = false;
            // 
            // tabPageForcedDirect
            // 
            this.tabPageForcedDirect.Controls.Add(this.propertyGridForcedDirect);
            this.tabPageForcedDirect.Location = new System.Drawing.Point(3, 26);
            this.tabPageForcedDirect.Name = "tabPageForcedDirect";
            this.tabPageForcedDirect.Size = new System.Drawing.Size(434, 275);
            this.tabPageForcedDirect.TabIndex = 1;
            this.tabPageForcedDirect.Text = "XForcedDirect";
            // 
            // propertyGridForcedDirect
            // 
            this.propertyGridForcedDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridForcedDirect.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridForcedDirect.LineColor = System.Drawing.Color.Silver;
            this.propertyGridForcedDirect.Location = new System.Drawing.Point(0, 0);
            this.propertyGridForcedDirect.Name = "propertyGridForcedDirect";
            this.propertyGridForcedDirect.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridForcedDirect, this.contextMenuBarItemGrid);
            this.propertyGridForcedDirect.Size = new System.Drawing.Size(434, 275);
            this.propertyGridForcedDirect.TabIndex = 13;
            this.propertyGridForcedDirect.ToolbarVisible = false;
            // 
            // tabPageHierarchical
            // 
            this.tabPageHierarchical.Controls.Add(this.propertyGridHierarchical);
            this.tabPageHierarchical.Location = new System.Drawing.Point(3, 26);
            this.tabPageHierarchical.Name = "tabPageHierarchical";
            this.tabPageHierarchical.Size = new System.Drawing.Size(434, 275);
            this.tabPageHierarchical.TabIndex = 0;
            this.tabPageHierarchical.Text = "XHierarchical";
            // 
            // propertyGridHierarchical
            // 
            this.propertyGridHierarchical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridHierarchical.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridHierarchical.LineColor = System.Drawing.Color.Silver;
            this.propertyGridHierarchical.Location = new System.Drawing.Point(0, 0);
            this.propertyGridHierarchical.Name = "propertyGridHierarchical";
            this.propertyGridHierarchical.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridHierarchical, this.contextMenuBarItemGrid);
            this.propertyGridHierarchical.Size = new System.Drawing.Size(434, 275);
            this.propertyGridHierarchical.TabIndex = 14;
            this.propertyGridHierarchical.ToolbarVisible = false;
            // 
            // tabPageOrthogonal
            // 
            this.tabPageOrthogonal.Controls.Add(this.propertyGridOrthogonal);
            this.tabPageOrthogonal.Location = new System.Drawing.Point(3, 26);
            this.tabPageOrthogonal.Name = "tabPageOrthogonal";
            this.tabPageOrthogonal.Size = new System.Drawing.Size(434, 275);
            this.tabPageOrthogonal.TabIndex = 3;
            this.tabPageOrthogonal.Text = "XOrthogonal";
            // 
            // propertyGridOrthogonal
            // 
            this.propertyGridOrthogonal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridOrthogonal.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridOrthogonal.LineColor = System.Drawing.Color.Silver;
            this.propertyGridOrthogonal.Location = new System.Drawing.Point(0, 0);
            this.propertyGridOrthogonal.Name = "propertyGridOrthogonal";
            this.propertyGridOrthogonal.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridOrthogonal, this.contextMenuBarItemGrid);
            this.propertyGridOrthogonal.Size = new System.Drawing.Size(434, 275);
            this.propertyGridOrthogonal.TabIndex = 15;
            this.propertyGridOrthogonal.ToolbarVisible = false;
            // 
            // tabPageTree
            // 
            this.tabPageTree.Controls.Add(this.propertyGridTree);
            this.tabPageTree.Location = new System.Drawing.Point(3, 26);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Size = new System.Drawing.Size(434, 275);
            this.tabPageTree.TabIndex = 4;
            this.tabPageTree.Text = "XTree";
            // 
            // propertyGridTree
            // 
            this.propertyGridTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridTree.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridTree.LineColor = System.Drawing.Color.Silver;
            this.propertyGridTree.Location = new System.Drawing.Point(0, 0);
            this.propertyGridTree.Name = "propertyGridTree";
            this.propertyGridTree.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridTree, this.contextMenuBarItemGrid);
            this.propertyGridTree.Size = new System.Drawing.Size(434, 275);
            this.propertyGridTree.TabIndex = 17;
            this.propertyGridTree.ToolbarVisible = false;
            // 
            // tabPageReport
            // 
            this.tabPageReport.Controls.Add(this.tabControlReport);
            this.tabPageReport.Location = new System.Drawing.Point(3, 26);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Size = new System.Drawing.Size(440, 304);
            this.tabPageReport.TabIndex = 3;
            this.tabPageReport.Text = "XReport";
            // 
            // tabControlReport
            // 
            this.tabControlReport.BorderStyle = TD.SandDock.Rendering.BorderStyle.None;
            this.tabControlReport.Controls.Add(this.tabPageDataReport);
            this.tabControlReport.Controls.Add(this.tabPageSchemaReport);
            this.tabControlReport.Controls.Add(this.tabPageXmlReport);
            this.tabControlReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlReport.Location = new System.Drawing.Point(0, 0);
            this.tabControlReport.Name = "tabControlReport";
            this.tabControlReport.Renderer = new TD.SandDock.Rendering.Office2007Renderer();
            this.tabControlReport.Size = new System.Drawing.Size(440, 304);
            this.tabControlReport.TabIndex = 2;
            // 
            // tabPageDataReport
            // 
            this.tabPageDataReport.Controls.Add(this.propertyGridDataReport);
            this.tabPageDataReport.Location = new System.Drawing.Point(3, 26);
            this.tabPageDataReport.Name = "tabPageDataReport";
            this.tabPageDataReport.Size = new System.Drawing.Size(434, 275);
            this.tabPageDataReport.TabIndex = 0;
            this.tabPageDataReport.Text = "XData";
            // 
            // propertyGridDataReport
            // 
            this.propertyGridDataReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridDataReport.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridDataReport.LineColor = System.Drawing.Color.Silver;
            this.propertyGridDataReport.Location = new System.Drawing.Point(0, 0);
            this.propertyGridDataReport.Name = "propertyGridDataReport";
            this.propertyGridDataReport.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridDataReport, this.contextMenuBarItemGrid);
            this.propertyGridDataReport.Size = new System.Drawing.Size(434, 275);
            this.propertyGridDataReport.TabIndex = 12;
            this.propertyGridDataReport.ToolbarVisible = false;
            // 
            // tabPageSchemaReport
            // 
            this.tabPageSchemaReport.Controls.Add(this.propertyGridSchemaReport);
            this.tabPageSchemaReport.Location = new System.Drawing.Point(3, 26);
            this.tabPageSchemaReport.Name = "tabPageSchemaReport";
            this.tabPageSchemaReport.Size = new System.Drawing.Size(434, 275);
            this.tabPageSchemaReport.TabIndex = 1;
            this.tabPageSchemaReport.Text = "XSchema";
            // 
            // propertyGridSchemaReport
            // 
            this.propertyGridSchemaReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSchemaReport.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridSchemaReport.LineColor = System.Drawing.Color.Silver;
            this.propertyGridSchemaReport.Location = new System.Drawing.Point(0, 0);
            this.propertyGridSchemaReport.Name = "propertyGridSchemaReport";
            this.propertyGridSchemaReport.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridSchemaReport, this.contextMenuBarItemGrid);
            this.propertyGridSchemaReport.Size = new System.Drawing.Size(434, 275);
            this.propertyGridSchemaReport.TabIndex = 16;
            this.propertyGridSchemaReport.ToolbarVisible = false;
            // 
            // tabPageXmlReport
            // 
            this.tabPageXmlReport.Controls.Add(this.propertyGridXmlReport);
            this.tabPageXmlReport.Location = new System.Drawing.Point(3, 26);
            this.tabPageXmlReport.Name = "tabPageXmlReport";
            this.tabPageXmlReport.Size = new System.Drawing.Size(434, 275);
            this.tabPageXmlReport.TabIndex = 2;
            this.tabPageXmlReport.Text = "XXmlReport";
            // 
            // propertyGridXmlReport
            // 
            this.propertyGridXmlReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridXmlReport.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridXmlReport.LineColor = System.Drawing.Color.Silver;
            this.propertyGridXmlReport.Location = new System.Drawing.Point(0, 0);
            this.propertyGridXmlReport.Name = "propertyGridXmlReport";
            this.propertyGridXmlReport.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridXmlReport, this.contextMenuBarItemGrid);
            this.propertyGridXmlReport.Size = new System.Drawing.Size(434, 275);
            this.propertyGridXmlReport.TabIndex = 17;
            this.propertyGridXmlReport.ToolbarVisible = false;
            // 
            // tabPageModel
            // 
            this.tabPageModel.Controls.Add(this.propertyGridModel);
            this.tabPageModel.Location = new System.Drawing.Point(3, 26);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Size = new System.Drawing.Size(440, 304);
            this.tabPageModel.TabIndex = 4;
            this.tabPageModel.Text = "XDiagram";
            // 
            // propertyGridModel
            // 
            this.propertyGridModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridModel.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridModel.LineColor = System.Drawing.Color.Silver;
            this.propertyGridModel.Location = new System.Drawing.Point(0, 0);
            this.propertyGridModel.Name = "propertyGridModel";
            this.propertyGridModel.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridModel, this.contextMenuBarItemGrid);
            this.propertyGridModel.Size = new System.Drawing.Size(440, 304);
            this.propertyGridModel.TabIndex = 12;
            this.propertyGridModel.ToolbarVisible = false;
            // 
            // tabPageWindow
            // 
            this.tabPageWindow.Controls.Add(this.propertyGridWindow);
            this.tabPageWindow.Location = new System.Drawing.Point(3, 26);
            this.tabPageWindow.Name = "tabPageWindow";
            this.tabPageWindow.Size = new System.Drawing.Size(440, 304);
            this.tabPageWindow.TabIndex = 5;
            this.tabPageWindow.Text = "XWindow";
            // 
            // propertyGridWindow
            // 
            this.propertyGridWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridWindow.HelpBackColor = System.Drawing.SystemColors.Window;
            this.propertyGridWindow.LineColor = System.Drawing.Color.Silver;
            this.propertyGridWindow.Location = new System.Drawing.Point(0, 0);
            this.propertyGridWindow.Name = "propertyGridWindow";
            this.propertyGridWindow.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.menuBar1.SetSandBarMenu(this.propertyGridWindow, this.contextMenuBarItemGrid);
            this.propertyGridWindow.Size = new System.Drawing.Size(440, 304);
            this.propertyGridWindow.TabIndex = 13;
            this.propertyGridWindow.ToolbarVisible = false;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(367, 339);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 6;
            this.buttonApply.Text = "XApply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(286, 339);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "XCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(205, 339);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "XOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Location = new System.Drawing.Point(4, 339);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 9;
            this.buttonReset.Text = "XReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.Button_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // buttonResetAll
            // 
            this.buttonResetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonResetAll.Location = new System.Drawing.Point(85, 339);
            this.buttonResetAll.Name = "buttonResetAll";
            this.buttonResetAll.Size = new System.Drawing.Size(75, 23);
            this.buttonResetAll.TabIndex = 10;
            this.buttonResetAll.Text = "XReset All";
            this.buttonResetAll.UseVisualStyleBackColor = true;
            this.buttonResetAll.Click += new System.EventHandler(this.Button_Click);
            // 
            // sandBarManager1
            // 
            this.sandBarManager1.OwnerForm = this;
            this.sandBarManager1.Renderer = new TD.SandBar.Office2007Renderer();
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("d176f8de-98d6-494a-83d0-7160c48f5215");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 23);
            this.leftSandBarDock.Manager = this.sandBarManager1;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 341);
            this.leftSandBarDock.TabIndex = 11;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("769d45e2-3c26-4d86-867f-b0669c810337");
            this.rightSandBarDock.Location = new System.Drawing.Point(446, 23);
            this.rightSandBarDock.Manager = this.sandBarManager1;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 341);
            this.rightSandBarDock.TabIndex = 12;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("8dc7c032-ca3c-4206-9a11-8a7fdf576da1");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 364);
            this.bottomSandBarDock.Manager = this.sandBarManager1;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(446, 0);
            this.bottomSandBarDock.TabIndex = 13;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.menuBar1);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("e3231383-0470-41eb-adb3-942e6fc13c1c");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager1;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(446, 23);
            this.topSandBarDock.TabIndex = 14;
            // 
            // menuBar1
            // 
            this.menuBar1.Guid = new System.Guid("a49e0947-2c58-414e-94a1-9803dcb8b80f");
            this.menuBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.contextMenuBarItemGrid});
            this.menuBar1.Location = new System.Drawing.Point(2, 0);
            this.menuBar1.Movable = false;
            this.menuBar1.Name = "menuBar1";
            this.menuBar1.OwnerForm = this;
            this.menuBar1.ShowShortcutsInToolTips = true;
            this.menuBar1.Size = new System.Drawing.Size(444, 23);
            this.menuBar1.TabIndex = 0;
            this.menuBar1.Tearable = false;
            this.menuBar1.Text = "";
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
            this.statusBar1.Guid = new System.Guid("6926b84c-e61d-4769-b607-e26b714e05b4");
            this.statusBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.statusBarItem1});
            this.statusBar1.Location = new System.Drawing.Point(0, 364);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.OwnerForm = this;
            this.statusBar1.Renderer = new TD.SandBar.Office2007Renderer();
            this.statusBar1.Size = new System.Drawing.Size(446, 19);
            this.statusBar1.TabIndex = 15;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarItem1
            // 
            this.statusBarItem1.Stretch = true;
            this.statusBarItem1.Text = "";
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(446, 383);
            this.Controls.Add(this.buttonResetAll);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.tabControlOptions);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Controls.Add(this.statusBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOptions";
            this.Text = "XOptions";
            this.tabControlOptions.ResumeLayout(false);
            this.tabPageDiagramColors.ResumeLayout(false);
            this.tabPageLayout.ResumeLayout(false);
            this.tabControlLayout.ResumeLayout(false);
            this.tabPageCircular.ResumeLayout(false);
            this.tabPageForcedDirect.ResumeLayout(false);
            this.tabPageHierarchical.ResumeLayout(false);
            this.tabPageOrthogonal.ResumeLayout(false);
            this.tabPageTree.ResumeLayout(false);
            this.tabPageReport.ResumeLayout(false);
            this.tabControlReport.ResumeLayout(false);
            this.tabPageDataReport.ResumeLayout(false);
            this.tabPageSchemaReport.ResumeLayout(false);
            this.tabPageXmlReport.ResumeLayout(false);
            this.tabPageModel.ResumeLayout(false);
            this.tabPageWindow.ResumeLayout(false);
            this.topSandBarDock.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TD.SandDock.TabControl tabControlOptions;
        private TD.SandDock.TabPage tabPageDiagramColors;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.PropertyGrid propertyGridColors;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Timer timer1;
        private TD.SandDock.TabPage tabPageLayout;
        private TD.SandDock.TabControl tabControlLayout;
        private TD.SandDock.TabPage tabPageHierarchical;
        private System.Windows.Forms.PropertyGrid propertyGridHierarchical;
        private TD.SandDock.TabPage tabPageForcedDirect;
        private System.Windows.Forms.PropertyGrid propertyGridForcedDirect;
        private TD.SandDock.TabPage tabPageCircular;
        private System.Windows.Forms.PropertyGrid propertyGridCircular;
        private TD.SandDock.TabPage tabPageOrthogonal;
        private System.Windows.Forms.PropertyGrid propertyGridOrthogonal;
        private TD.SandDock.TabPage tabPageTree;
        private System.Windows.Forms.PropertyGrid propertyGridTree;
        private TD.SandDock.TabPage tabPageReport;
        private System.Windows.Forms.PropertyGrid propertyGridDataReport;
        private TD.SandDock.TabControl tabControlReport;
        private TD.SandDock.TabPage tabPageDataReport;
        private TD.SandDock.TabPage tabPageSchemaReport;
        private System.Windows.Forms.PropertyGrid propertyGridSchemaReport;
        private System.Windows.Forms.Button buttonResetAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private TD.SandBar.SandBarManager sandBarManager1;
        private TD.SandBar.ToolBarContainer leftSandBarDock;
        private TD.SandBar.ToolBarContainer rightSandBarDock;
        private TD.SandBar.ToolBarContainer bottomSandBarDock;
        private TD.SandBar.ToolBarContainer topSandBarDock;
        private TD.SandBar.MenuBar menuBar1;
        private TD.SandBar.ContextMenuBarItem contextMenuBarItemGrid;
        private TD.SandBar.StatusBar statusBar1;
        private TD.SandBar.StatusBarItem statusBarItem1;
        private TD.SandBar.MenuButtonItem menuButtonItemReset;
        private TD.SandBar.MenuButtonItem menuButtonItemDescription;
        private TD.SandDock.TabPage tabPageModel;
        private System.Windows.Forms.PropertyGrid propertyGridModel;
        private TD.SandDock.TabPage tabPageWindow;
        private System.Windows.Forms.PropertyGrid propertyGridWindow;
        private TD.SandDock.TabPage tabPageXmlReport;
        private System.Windows.Forms.PropertyGrid propertyGridXmlReport;

    }
}
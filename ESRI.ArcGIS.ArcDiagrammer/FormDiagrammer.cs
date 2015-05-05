/*=============================================================================
 * 
 * Copyright © 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished—all rights reserved.
 * Use of this ESRI commercial Software, Data, and Documentation is limited to
 * the ESRI License Agreement. In no event shall the Government acquire greater
 * than Restricted/Limited Rights. At a minimum Government rights to use,
 * duplicate, or disclose is subject to restrictions as set for in FAR 12.211,
 * FAR 12.212, and FAR 52.227-19 (June 1987), FAR 52.227-14 (ALT I, II, and III)
 * (June 1987), DFARS 227.7202, DFARS 252.227-7015 (NOV 1995).
 * Contractor/Manufacturer is ESRI, 380 New York Street, Redlands,
 * CA 92373-8100, USA.
 * 
 * SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE, ARE DISCLAIMED.  IN NO EVENT SHALL ESRI OR CONTRIBUTORS
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT; STRICT LIABILITY; OR TORT ARISING
 * IN ANY WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE TO THE FULL EXTENT ALLOWED BY APPLICABLE LAW.
 * 
 * =============================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Crainiate.ERM4;
using Crainiate.ERM4.Layouts;
using ESRI.ArcGIS.ArcDiagrammer.Properties;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandBar;
using TD.SandDock;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class FormDiagrammer : Form {
        private Bitmap _bitmapError = null;
        private Bitmap _bitmapNoError = null;
        private DropDownMenuItem _zoomMenu = null;

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_CUT = 0x0300;
        private const int WM_COPY = 0x0301;
        private const int WM_PASTE = 0x0302; // 770
        private const int WM_CLEAR = 0x0303;
        private const int WM_UNDO = 0x0304;  // 0xc7
        private const int WM_REDO = 0x0304;
        //
        // CONSTRUCTOR/DESTRUCTOR
        //
        public FormDiagrammer(string[] args) {
            try {
                // Call Partial Class Constructor
                InitializeComponent();

                // Assign ArcDiagrammer Icon to Application
                this.Icon = Resources.DIAGRAMMER;
                
                // Main Menu
                this.menuBar1.Text = Resources.TEXT_MAIN_MENU;

                // MenuBarItem Text
                this.menuBarItemFile.Text = Resources.TEXT_FILE_;
                this.menuBarItemEdit.Text = Resources.TEXT_EDIT_;
                this.menuBarItemView.Text = Resources.TEXT_VIEW_;
                this.menuBarItemTools.Text = Resources.TEXT_TOOLS_;
                this.menuBarItemWindow.Text = Resources.TEXT_WINDOW_;
                this.menuBarItemHelp.Text = Resources.TEXT_HELP_;

                // MenuButtonItem Text (File)
                this.menuButtonItemNew.Text = Resources.TEXT_NEW_;
                this.menuButtonItemOpen.Text = Resources.TEXT_OPEN_;
                this.menuButtonItemClose.Text = Resources.TEXT_CLOSE_;
                this.menuButtonItemSave.Text = Resources.TEXT_SAVE_;
                this.menuButtonItemSaveAs.Text = Resources.TEXT_SAVE_AS_;
                this.menuButtonItemPublish.Text = Resources.TEXT_PUBLISH_;
                this.menuButtonItemExport.Text = Resources.TEXT_EXPORT_;
                this.menuButtonItemPrintSetup.Text = Resources.TEXT_PRINT_SETUP_;
                this.menuButtonItemPrintPreview.Text = Resources.TEXT_PRINT_PREVIEW_;
                this.menuButtonItemPrint.Text = Resources.TEXT_PRINT_;
                this.menuButtonItemExit.Text = Resources.TEXT_EXIT_;

                // MenuButtonItem Text (Edit)
                this.menuButtonItemUndo.Text = Resources.TEXT_UNDO_;
                this.menuButtonItemRedo.Text = Resources.TEXT_REDO_;
                this.menuButtonItemCut.Text = Resources.TEXT_CUT_;
                this.menuButtonItemCopy.Text = Resources.TEXT_COPY_;
                this.menuButtonItemPaste.Text = Resources.TEXT_PASTE_;
                this.menuButtonItemDelete.Text = Resources.TEXT_DELETE_;
                this.menuButtonItemSelectAll.Text = Resources.TEXT_SELECTALL_;

                // MenuButtonItem Text (View)
                this.menuButtonItemColorScheme.Text = Resources.TEXT_COLOR_SCHEME_;
                this.menuButtonItemLayout.Text = Resources.TEXT_LAYOUT_;
                this.menuButtonItemToolbars.Text = Resources.TEXT_TOOLBARS_;
                this.menuButtonItemZoom.Text = Resources.TEXT_ZOOM_;
                this.menuButtonItemCatalogWindow.Text = Resources.TEXT_CATALOG_;
                this.menuButtonItemOverviewWindow.Text = Resources.TEXT_OVERVIEW_;
                this.menuButtonItemPaletteWindow.Text = Resources.TEXT_PALETTE_;
                this.menuButtonItemPropertiesWindow.Text = Resources.TEXT_PROPERTIES_;
                this.menuButtonItemErrorList.Text = Resources.TEXT_ERROR_LIST_;
                this.menuButtonItemExceptions.Text = Resources.TEXT_EXCEPTIONS_;
                this.menuButtonItemStatusBar.Text = Resources.TEXT_STATUSBAR_;

                // MenuButtonItem Text (View - Color Scheme)
                this.menuButtonItemColorSchemeBlack.Text = Resources.TEXT_BLACK_;
                this.menuButtonItemColorSchemeBlue.Text = Resources.TEXT_BLUE_;
                this.menuButtonItemColorSchemeSilver.Text = Resources.TEXT_SILVER_;

                // MenuButtonItem Text (View - Layout)
                this.menuButtonItemCircular.Text = Resources.TEXT_CIRCULAR_;
                this.menuButtonItemForcedDirect.Text = Resources.TEXT_FORCED_DIRECT_;
                this.menuButtonItemHierachical.Text = Resources.TEXT_HIERARCHICAL_;
                this.menuButtonItemOrthogonal.Text = Resources.TEXT_ORTHOGONAL_;
                this.menuButtonItemTree.Text = Resources.TEXT_TREE_;
                this.menuButtonItemAlignLeft.Text = Resources.TEXT_ALIGN_LEFT_;
                this.menuButtonItemAlignCenter.Text = Resources.TEXT_ALIGN_CENTER_;
                this.menuButtonItemAlignRight.Text = Resources.TEXT_ALIGN_RIGHT_;
                this.menuButtonItemAlignTop.Text = Resources.TEXT_ALIGN_TOP_;
                this.menuButtonItemAlignMiddle.Text = Resources.TEXT_ALIGN_MIDDLE_;
                this.menuButtonItemAlignBottom.Text = Resources.TEXT_ALIGN_BOTTOM_;

                // MenuButtonItem Text (View - Toolbars)
                this.menuButtonItemStandardToolbar.Text = Resources.TEXT_STANDARD_;
                this.menuButtonItemLayoutToolbar.Text = Resources.TEXT_LAYOUT_;
                this.menuButtonItemInteractiveModeToolbar.Text = Resources.TEXT_INTERACTIVE_MODE_;

                // MenuButtonItem Text (View - Zoom)
                this.menuButtonItemZoom300.Text = Resources.TEXT_300_PERCENT_;
                this.menuButtonItemZoom200.Text = Resources.TEXT_200_PERCENT_;
                this.menuButtonItemZoom100.Text = Resources.TEXT_100_PERCENT_;
                this.menuButtonItemZoom75.Text = Resources.TEXT_75_PERCENT_;
                this.menuButtonItemZoom50.Text = Resources.TEXT_50_PERCENT_;
                this.menuButtonItemZoom25.Text = Resources.TEXT_25_PERCENT_;
                this.menuButtonItemZoomFull.Text = Resources.TEXT_FULL_;

                // MenuButtonItem Text (Tools)
                this.menuButtonItemNormalMode.Text = Resources.TEXT_NORMAL_MODE_;
                this.menuButtonItemLinkMode.Text = Resources.TEXT_LINK_MODE_;
                this.menuButtonItemSchemaReport.Text = Resources.TEXT_SCHEMA_REPORT_;
                this.menuButtonItemDataReport.Text = Resources.TEXT_DATA_REPORT_;
                this.menuButtonItemXmlReport.Text = Resources.TEXT_XML_REPORT_;
                this.menuButtonItemValidate.Text = Resources.TEXT_REPORTER_;
                this.menuButtonItemOptions.Text = Resources.TEXT_OPTIONS_;

                // MenuButtonItem Text (Window)
                this.menuButtonItemNewHorizontalTabGroup.Text = Resources.TEXT_NEW_HORIZONTAL_TAB_GROUP_;
                this.menuButtonItemNewVerticalTabGroup.Text = Resources.TEXT_NEW_VERTICAL_TAB_GROUP_;
                this.menuButtonItemRestoreTabGroup.Text = Resources.TEXT_RESTORE_TAB_GROUP_;
                this.menuButtonItemCloseAllDocuments.Text = Resources.TEXT_CLOSE_ALL_DOCUMENTS_;
                this.menuButtonItemWindows.Text = Resources.TEXT_TABWINDOWS_;

                // MenuButtonItem Text (Help)
                this.menuButtonItemUserGuide.Text = Resources.TEXT_ARCGIS_DIAGRAMMER_USERGUIDE_;
                this.menuButtonItemTutorials.Text = Resources.TEXT_TUTORIALS_;
                this.menuButtonItemSendFeedback.Text = Resources.TEXT_SEND_FEEDBACK_;
                this.menuButtonItemDiscussionForum.Text = Resources.TEXT_DISCUSSION_FORUM_;
                this.menuButtonItemHistory.Text = Resources.TEXT_HISTORY_;
                this.menuButtonItemEsriHome.Text = Resources.TEXT_ESRI_HOME_;
                this.menuButtonItemEsriSupportCenter.Text = Resources.TEXT_ARCGIS_IDEAS_;
                this.menuButtonItemAbout.Text = Resources.TEXT_ABOUT_ARCDIAGRAMMER_;
                this.menuButtonItemCreateSchemaReport.Text = Resources.TEXT_CREATE_SCHEMA_REPORT_;
                this.menuButtonItemCreateDataReport.Text = Resources.TEXT_CREATE_DATA_REPORT_;
                this.menuButtonItemReorderFields.Text = Resources.TEXT_REORDER_FIELDS_;
                this.menuButtonItemAddSubtype.Text = Resources.TEXT_ADD_SUBTYPE;
                this.menuButtonItemCreateOneToManyRelationship.Text = Resources.TEXT_CREATE_ONE_TO_MANY_RELATIONSHIP;

                // Clear Toolbar Text
                foreach (TD.SandBar.ToolBar toolbar in this.sandBarManager1.GetToolBars()) {
                    if (toolbar is TD.SandBar.MenuBar) { continue; }
                    foreach (TD.SandBar.ToolbarItemBase toolbarItem in toolbar.Items) {
                        toolbarItem.Text = string.Empty;
                    }
                }

                // Toolbars
                this.toolBarStandard.Text = Resources.TEXT_STANDARD;
                this.toolBarLayout.Text = Resources.TEXT_LAYOUT;
                this.toolBarInteractiveMode.Text = Resources.TEXT_INTERACTIVE_MODE;

                // MenuButtonItem Bitmaps
                this.menuButtonItemCopy.Image = Resources.BITMAP_COPY;
                this.menuButtonItemCut.Image = Resources.BITMAP_CUT;
                this.menuButtonItemDelete.Image = Resources.BITMAP_DELETE;
                this.menuButtonItemSelectAll.Image = Resources.BITMAP_SELECT_ALL;
                this.menuButtonItemExport.Image = Resources.BITMAP_EXPORT;
                this.menuButtonItemUserGuide.Image = Resources.BITMAP_HELP;
                this.menuButtonItemCircular.Image = Resources.BITMAP_LAYOUT_CIRCULAR;
                this.menuButtonItemForcedDirect.Image = Resources.BITMAP_LAYOUT_FORCEDDIRECT;
                this.menuButtonItemHierachical.Image = Resources.BITMAP_LAYOUT_HIERACHICAL;
                this.menuButtonItemOrthogonal.Image = Resources.BITMAP_LAYOUT_ORTHOGONAL;
                this.menuButtonItemTree.Image = Resources.BITMAP_LAYOUT_TREE;
                this.menuButtonItemAlignLeft.Image = Resources.BITMAP_ALIGN_LEFT;
                this.menuButtonItemAlignCenter.Image = Resources.BITMAP_ALIGN_CENTER;
                this.menuButtonItemAlignRight.Image = Resources.BITMAP_ALIGN_RIGHT;
                this.menuButtonItemAlignTop.Image = Resources.BITMAP_ALIGN_TOP;
                this.menuButtonItemAlignMiddle.Image = Resources.BITMAP_ALIGN_MIDDLE;
                this.menuButtonItemAlignBottom.Image = Resources.BITMAP_ALIGN_BOTTOM;
                this.menuButtonItemNew.Image = Resources.BITMAP_NEW;
                this.menuButtonItemOpen.Image = Resources.BITMAP_OPEN;
                this.menuButtonItemClose.Image = Resources.BITMAP_CLOSE;
                this.menuButtonItemOptions.Image = Resources.BITMAP_OPTIONS;
                this.menuButtonItemPaste.Image = Resources.BITMAP_PASTE;
                this.menuButtonItemPrintSetup.Image = Resources.BITMAP_PRINT_SETUP;
                this.menuButtonItemPrint.Image = Resources.BITMAP_PRINT;
                this.menuButtonItemPrintPreview.Image = Resources.BITMAP_PRINT_PREVIEW;
                this.menuButtonItemPublish.Image = Resources.BITMAP_PUBLISH;
                this.menuButtonItemRedo.Image = Resources.BITMAP_REDO;
                this.menuButtonItemSave.Image = Resources.BITMAP_SAVE;
                this.menuButtonItemSaveAs.Image = Resources.BITMAP_SAVEAS;
                this.menuButtonItemNewHorizontalTabGroup.Image = Resources.BITMAP_SPLIT_HORIZONTAL;
                this.menuButtonItemNewVerticalTabGroup.Image = Resources.BITMAP_SPLIT_VERTICAL;
                this.menuButtonItemUndo.Image = Resources.BITMAP_UNDO;
                this.menuButtonItemNormalMode.Image = Resources.BITMAP_MODE_NORMAL;
                this.menuButtonItemLinkMode.Image = Resources.BITMAP_MODE_LINK;
                this.menuButtonItemSchemaReport.Image = Resources.BITMAP_SCHEMA_REPORT;
                this.menuButtonItemDataReport.Image = Resources.BITMAP_DATA_REPORT;
                this.menuButtonItemXmlReport.Image = Resources.BITMAP_XML_REPORT;
                this.menuButtonItemValidate.Image = Resources.BITMAP_VALIDATE;
                this.menuButtonItemSendFeedback.Image = Resources.BITMAP_MAIL;
                this.menuButtonItemEsriHome.Image = Resources.BITMAP_ESRI;
                this.menuButtonItemColorSchemeBlack.Image = Resources.BITMAP_BLACK;
                this.menuButtonItemColorSchemeBlue.Image = Resources.BITMAP_BLUE;
                this.menuButtonItemColorSchemeSilver.Image = Resources.BITMAP_SILVER;

                // ButtonItem Bitmaps
                this.buttonItemCopy.Image = Resources.BITMAP_COPY;
                this.buttonItemCut.Image = Resources.BITMAP_CUT;
                this.buttonItemDelete.Image = Resources.BITMAP_DELETE;
                this.buttonItemNew.Image = Resources.BITMAP_NEW;
                this.buttonItemOpen.Image = Resources.BITMAP_OPEN;
                this.buttonItemPaste.Image = Resources.BITMAP_PASTE;
                this.buttonItemExport.Image = Resources.BITMAP_EXPORT;
                this.buttonItemPrint.Image = Resources.BITMAP_PRINT;
                this.buttonItemPublish.Image = Resources.BITMAP_PUBLISH;
                this.buttonItemRedo.Image = Resources.BITMAP_REDO;
                this.buttonItemSave.Image = Resources.BITMAP_SAVE;
                this.buttonItemUndo.Image = Resources.BITMAP_UNDO;
                this.buttonItemCircular.Image = Resources.BITMAP_LAYOUT_CIRCULAR;
                this.buttonItemForcedDirect.Image = Resources.BITMAP_LAYOUT_FORCEDDIRECT;
                this.buttonItemHierachical.Image = Resources.BITMAP_LAYOUT_HIERACHICAL;
                this.buttonItemOrthogonal.Image = Resources.BITMAP_LAYOUT_ORTHOGONAL;
                this.buttonItemTree.Image = Resources.BITMAP_LAYOUT_TREE;
                this.buttonItemAlignLeft.Image = Resources.BITMAP_ALIGN_LEFT;
                this.buttonItemAlignCenter.Image = Resources.BITMAP_ALIGN_CENTER;
                this.buttonItemAlignRight.Image = Resources.BITMAP_ALIGN_RIGHT;
                this.buttonItemAlignTop.Image = Resources.BITMAP_ALIGN_TOP;
                this.buttonItemAlignMiddle.Image = Resources.BITMAP_ALIGN_MIDDLE;
                this.buttonItemAlignBottom.Image = Resources.BITMAP_ALIGN_BOTTOM;
                this.buttonItemNormal.Image = Resources.BITMAP_MODE_NORMAL;
                this.buttonItemLink.Image = Resources.BITMAP_MODE_LINK;

                // ButtonItem Text
                this.buttonItemCopy.ToolTipText = Resources.TEXT_COPY_.Replace("&", string.Empty);
                this.buttonItemCut.ToolTipText = Resources.TEXT_CUT_.Replace("&", string.Empty);
                this.buttonItemDelete.ToolTipText = Resources.TEXT_DELETE_.Replace("&", string.Empty);
                this.buttonItemNew.ToolTipText = Resources.TEXT_NEW_.Replace("&", string.Empty);
                this.buttonItemOpen.ToolTipText = Resources.TEXT_OPEN_.Replace("&", string.Empty);
                this.buttonItemPaste.ToolTipText = Resources.TEXT_PASTE_.Replace("&", string.Empty);
                this.buttonItemExport.ToolTipText = Resources.TEXT_EXPORT_.Replace("&", string.Empty);
                this.buttonItemPrint.ToolTipText = Resources.TEXT_PRINT_.Replace("&", string.Empty);
                this.buttonItemPublish.ToolTipText = Resources.TEXT_PUBLISH_.Replace("&", string.Empty);
                this.buttonItemRedo.ToolTipText = Resources.TEXT_REDO_.Replace("&", string.Empty);
                this.buttonItemSave.ToolTipText = Resources.TEXT_SAVE;
                this.buttonItemUndo.ToolTipText = Resources.TEXT_UNDO_.Replace("&", string.Empty);
                this.buttonItemCircular.ToolTipText = Resources.TEXT_CIRCULAR_.Replace("&", string.Empty);
                this.buttonItemForcedDirect.ToolTipText = Resources.TEXT_FORCED_DIRECT_.Replace("&", string.Empty);
                this.buttonItemHierachical.ToolTipText = Resources.TEXT_HIERARCHICAL_.Replace("&", string.Empty);
                this.buttonItemOrthogonal.ToolTipText = Resources.TEXT_ORTHOGONAL_.Replace("&", string.Empty);
                this.buttonItemAlignLeft.ToolTipText = Resources.TEXT_ALIGN_LEFT_.Replace("&", string.Empty);
                this.buttonItemAlignCenter.ToolTipText = Resources.TEXT_ALIGN_CENTER_.Replace("&", string.Empty);
                this.buttonItemAlignRight.ToolTipText = Resources.TEXT_ALIGN_RIGHT_.Replace("&", string.Empty);
                this.buttonItemAlignTop.ToolTipText = Resources.TEXT_ALIGN_TOP_.Replace("&", string.Empty);
                this.buttonItemAlignMiddle.ToolTipText = Resources.TEXT_ALIGN_MIDDLE_.Replace("&", string.Empty);
                this.buttonItemAlignBottom.ToolTipText = Resources.TEXT_ALIGN_BOTTOM_.Replace("&", string.Empty);
                this.buttonItemTree.ToolTipText = Resources.TEXT_TREE_.Replace("&", string.Empty);
                this.buttonItemNormal.ToolTipText = Resources.TEXT_NORMAL_MODE_.Replace("&", string.Empty);
                this.buttonItemLink.ToolTipText = Resources.TEXT_LINK_MODE_.Replace("&", string.Empty);

                // Windows
                this.dockableWindowOverview.Text = Resources.TEXT_OVERVIEW;
                this.dockableWindowCatalog.Text = Resources.TEXT_CATALOG;
                this.dockableWindowPalette.Text = Resources.TEXT_PALETTE;
                this.dockableWindowProperties.Text = Resources.TEXT_PROPERTIES;
                this.dockableWindowErrorList.Text = Resources.TEXT_ERROR_LIST;

                // StatusBar
                this._bitmapError = Resources.BITMAP_ERROR;
                this._bitmapNoError = Resources.BITMAP_OK;
                this.statusBarItemMain.Text = Resources.TEXT_READY;
                this.statusBarItemError.Text = string.Empty;
                this.statusBarItemError.Image = this._bitmapNoError;

                // Add StatusBar Zoom Menu
                this._zoomMenu = new DropDownMenuItem();
                //this._zoomMenu.Text = "XZoom";
                this._zoomMenu.BeginGroup = true;
                this._zoomMenu.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.MenuItem_BeforePopup);
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_300_PERCENT_, 300f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_200_PERCENT_, 200f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_100_PERCENT_, 100f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_75_PERCENT_, 75f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_50_PERCENT_, 50f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_25_PERCENT_, 25f, new EventHandler(this.MenuItem_Activate)));
                this._zoomMenu.Items.Add(new ZoomMenuItem(Resources.TEXT_FULL_, -1f, new EventHandler(this.MenuItem_Activate)));
                this.statusBar1.Items.Insert(1, this._zoomMenu);

                // Listen for Exceptions 
                ExceptionDialog exceptionDialog = ExceptionDialog.Default;
                exceptionDialog.FeedbackAddress = Resources.TEXT_URL_FEEDBACK;
                exceptionDialog.Exception += new EventHandler<ExceptionHandlerEventArgs>(this.Form_Exception);

                // Update Renderer
                ColorSchemeSettings.Default.PropertyChanged += new PropertyChangedEventHandler(this.ColorScheme_PropertyChanged);
                this.ColorScheme_PropertyChanged(null, null);

                // Get Diagrammer Environment
                DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                diagrammerEnvironment.ProgressChanged += new EventHandler<ProgressEventArgs>(this.DiagrammerEnvironment_ProgressChanged);
                diagrammerEnvironment.TableValidationRequest += new EventHandler<TableEventArgs>(this.DiagrammerEnvironment_TableValidationRequest);
                diagrammerEnvironment.MetadataViewerRequest += new EventHandler<DatasetEventArgs>(this.DiagrammerEnvironment_MetadataViewerRequest);

                // Changes the default font to Tahoma 
                Crainiate.ERM4.Component.Instance.DefaultFont = new Font("Tahoma", 8);
                Crainiate.ERM4.Component.Instance.DefaultStringFormat.FormatFlags = StringFormatFlags.NoWrap;
                Crainiate.ERM4.Component.Instance.DefaultStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                // Set the Help ComboBox text
                this.comboBoxItemHelp.DefaultText = Resources.TEXT_TYPE_A_QUESTION_FOR_HELP;

                // Assign this form to the PropertyEditor Window
                this.propertyEditor1.PropertyGrid.SetParent(this);

                if (args != null) {
                    if (args.Length > 0) {
                        string arg = args[0];
                        if (!string.IsNullOrEmpty(arg)) {
                            if (File.Exists(arg)) {
                                this.CreateTabbedDocument(arg);
                            }
                        }
                    }
                }
            }
            catch(Exception ex){
                ExceptionDialog.HandleException(ex);
            }
        }
        //
        // PROTECTED METHODS
        //
        protected override void OnLoad(EventArgs e) {
            // Call overriden method
            base.OnLoad(e);

            // Load Sandbar Layout
            if (!string.IsNullOrEmpty(SettingsWindow.Default.SandbarLayout)) {
                try {
                    this.sandBarManager1.SetLayout(SettingsWindow.Default.SandbarLayout);
                }
                catch { }
            }

            // Load Sanddock Layout
            if (!string.IsNullOrEmpty(SettingsWindow.Default.SanddockLayout)) {
                try {
                    this.sandDockManager1.SetLayout(SettingsWindow.Default.SanddockLayout);
                }
                catch { }
            }

            // Restore saved Window Position
            if (SettingsWindow.Default.SaveWindowPosition) {
                // Set Window Location
                if (!SettingsWindow.Default.WindowLocation.IsEmpty &&
                    !SettingsWindow.Default.WindowSize.IsEmpty) {
                    //
                    foreach (Screen screen in Screen.AllScreens) {
                        Rectangle rectangle = screen.Bounds;
                        Rectangle windowRectangle = new Rectangle(
                            SettingsWindow.Default.WindowLocation,
                            SettingsWindow.Default.WindowSize);

                        if (rectangle.IntersectsWith(windowRectangle)) {
                            this.Location = SettingsWindow.Default.WindowLocation;
                            this.StartPosition = FormStartPosition.Manual;
                            this.ClientSize = SettingsWindow.Default.WindowSize;
                            break;
                        }
                    }
                }
            }

            // Intialize ArcObjects
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            diagrammerEnvironment.InitializeArcObjects();
        }
        protected override void OnClosing(CancelEventArgs e) {
            // Call overriden method
            base.OnClosing(e);

            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;

            if (schemaModel != null) {
                if (schemaModel.Dirty) {
                    // Prompt
                    DialogResult dialogResult = MessageBox.Show(
                           "Do you want to save the diagram before exiting?",
                           Resources.TEXT_ARCDIAGRAMMER,
                           MessageBoxButtons.YesNoCancel,
                           MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button3);
                    switch (dialogResult) {
                        case DialogResult.Yes:
                            this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                        default:
                            e.Cancel = true;
                            return;
                    }
                }
            }

            // Save the Sandbar and Sanddock layout
            SettingsWindow.Default.SandbarLayout = this.sandBarManager1.GetLayout(true).Replace(Environment.NewLine, string.Empty);
            SettingsWindow.Default.SanddockLayout = this.sandDockManager1.GetLayout().Replace(Environment.NewLine, string.Empty);

            // Save Window Location and Position
            if (SettingsWindow.Default.SaveWindowPosition) {
                SettingsWindow.Default.WindowLocation = this.Location;
                SettingsWindow.Default.WindowSize = this.ClientSize;
                SettingsWindow.Default.WindowState = this.WindowState;
            }
            else {
                SettingsWindow.Default.WindowLocation = System.Drawing.Point.Empty;
                SettingsWindow.Default.WindowSize = Size.Empty;
                SettingsWindow.Default.WindowState = FormWindowState.Normal;
            }

            // Save Settings File
            SettingsWindow.Default.Save();

            // Shutdown the license.
            diagrammerEnvironment.ShutdownArcObjects();
        }
        //
        // PRIVATE METHODS
        //
        private void AddTabbedDocument(TabbedDocument tabbedDocument) {
            // Suspend Overview
            this.esriOverview1.Suspend();

            // Assign Manager and Open as Active Tab
            tabbedDocument.Manager = this.sandDockManager1;
            tabbedDocument.Open(WindowOpenMethod.OnScreenActivate);

            // Add  New Model Tabbed Document
            if (tabbedDocument is ITabModel) {
                ITabModel tabModel = (ITabModel)tabbedDocument;
                tabModel.Model.SelectedChanged += new EventHandler(this.Model_SelectedChanged);
                tabModel.Model.ElementInvalid += new ElementEventHandler(this.Model_ElementInvalid);

                if (tabModel.Model.GetType() == typeof(SchemaModel)) {
                    //
                    SchemaModel schemaModel = (SchemaModel)tabModel.Model;
                    schemaModel.DiagramRequest += new EventHandler<DiagramEventArgs>(this.SchemaModel_DiagramRequest);

                    // Store Reference to the Current Schema Model
                    DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                    diagrammerEnvironment.SchemaModel = schemaModel;

                    // Refresh Catalog Control
                    this.catalog1.Suspend();
                    this.catalog1.SchemaModel = schemaModel;
                    this.catalog1.Resume();
                    this.catalog1.RefreshCatalog();
                }
            }

            // Reset Overview Diagram Scale
            this.esriOverview1.Resume();
            this.esriOverview1.Refresh();
            this.esriOverview1.ZoomFullExtent();
        }
        private void ColorScheme_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            try {
                // Update SandBar
                Office2007Renderer sandBarRenderer = (Office2007Renderer)this.sandBarManager1.Renderer;
                if (sandBarRenderer.ColorScheme == ColorSchemeSettings.Default.ColorScheme) { return; }
                sandBarRenderer.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

                Office2007Renderer sandBarRenderer2 = (Office2007Renderer)this.statusBar1.Renderer;
                sandBarRenderer2.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

                // Update SandDock
                TD.SandDock.Rendering.Office2007Renderer sandDockRenderer = (TD.SandDock.Rendering.Office2007Renderer)this.sandDockManager1.Renderer;
                switch (ColorSchemeSettings.Default.ColorScheme) {
                    case Office2007ColorScheme.Black:
                        sandDockRenderer.ColorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Black;
                        break;
                    case Office2007ColorScheme.Blue:
                        sandDockRenderer.ColorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Blue;
                        break;
                    case Office2007ColorScheme.Silver:
                        sandDockRenderer.ColorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Silver;
                        break;
                }

                // Refresh User Interface
                this.Refresh();
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }  
        }
        private void CreateTabbedDocument(string filename) {
            // Create New Schema Model Tab
            TabbedDocumentSchema tabbedDocument = new TabbedDocumentSchema(filename);

            // Add Tab to Application
            this.AddTabbedDocument(tabbedDocument);
        }
        private void CreateTabbedDocument(Type reportType) {
            TabbedDocument tabbedDocument = null;
            if (reportType == typeof(DataReport)) {
                tabbedDocument = new TabbedDocumentDataReport();
            }
            else if (reportType == typeof(SchemaReport)) {
                tabbedDocument = new TabbedDocumentSchemaReport();
            }
            else if (reportType == typeof(XmlReport)) {
                tabbedDocument = new TabbedDocumentXmlReport();
            }
            if (tabbedDocument == null) { return; }
            this.AddTabbedDocument(tabbedDocument);
        }
        private void CreateTabbedDocument(EsriTable table, Type modelType) {
            TabbedDocument tabbedDocument = null;

            if (modelType == typeof(DomainModel)) {
                Domain domain = (Domain)table;
                tabbedDocument = new TabbedDocumentDomain(domain);
            }
            else if (modelType == typeof(GeometricNetworkModel)) {
                GeometricNetwork geometricNetwork = (GeometricNetwork)table;
                tabbedDocument = new TabbedDocumentGeometricNetwork(geometricNetwork);
            }
            else if (modelType == typeof(GeometricNetworkModelEE)) {
                GeometricNetwork geometricNetwork = (GeometricNetwork)table;
                tabbedDocument = new TabbedDocumentGeometricNetworkEE(geometricNetwork);
            }
            else if (modelType == typeof(GeometricNetworkModelEJ)) {
                GeometricNetwork geometricNetwork = (GeometricNetwork)table;
                tabbedDocument = new TabbedDocumentGeometricNetworkEJ(geometricNetwork);
            }
            else if (modelType == typeof(RelationshipModel)) {
                RelationshipClass relationshipClass = (RelationshipClass)table;
                tabbedDocument = new TabbedDocumentRelationship(relationshipClass);
            }
            else if (modelType == typeof(TopologyModel)) {
                Topology topology = (Topology)table;
                tabbedDocument = new TabbedDocumentTopology(topology);
            }
            else if (modelType == typeof(TopologyRuleModel)) {
                Topology topology = (Topology)table;
                tabbedDocument = new TabbedDocumentTopologyRule(topology);
            }
            else if (modelType == typeof(TerrainModel)) {
                Terrain terrain = (Terrain)table;
                tabbedDocument = new TabbedDocumentTerrain(terrain);
            }
            if (tabbedDocument == null) { return; }

            // Add Tabbed Document
            this.AddTabbedDocument(tabbedDocument);
        }
        private void DiagrammerEnvironment_ProgressChanged(object sender, ProgressEventArgs e) {
            try {
                // Exit if Invalid
                if (e == null) { return; }

                // Display Status Message
                if (string.IsNullOrEmpty(e.Message)) {
                    // If message is empty then display "Ready"
                    this.statusBarItemMain.Text = Resources.TEXT_READY;
                }
                else {
                    // Display Progress Message
                    this.statusBarItemMain.Text = e.Message;
                }

                // Call all queued windows events
                Application.DoEvents();
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void DiagrammerEnvironment_TableValidationRequest(object sender, TableEventArgs e) {
            // Validate Table
            this.errorList1.Validate2(e.Table);
        }
        private void DiagrammerEnvironment_MetadataViewerRequest(object sender, DatasetEventArgs e) {
            try {
                this.AddTabbedDocument(new TabbedDocumentMetadata(e.Dataset));
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Form_DragDrop(object sender, DragEventArgs e) {
            try {
                if (this.InvokeRequired) {
                    this.Invoke(new DragEventHandler(this.Form_DragDrop), new object[] { sender, e });
                }
                else {
                    // Get Xml Workspace Document from Dropped Object
                    string file = GeodatabaseUtility.ProcessDataObject(e);

                    // Exit if Dropped Object is Invalid
                    if (string.IsNullOrEmpty(file)) { return; }

                    // Create New Schema Tab using Xml Workspace Document
                    this.CreateTabbedDocument(file);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Form_DragEnter(object sender, DragEventArgs e) {
            try {
                if (this.sandDockManager1.GetDockControls(DockSituation.Document).Length == 0) {
                    e.Effect = GeodatabaseUtility.ValidDataObject(e) ? DragDropEffects.All : DragDropEffects.None;
                }
                else {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Form_DragLeave(object sender, EventArgs e) { }
        private void Form_DragOver(object sender, DragEventArgs e) {
            try {
                if (this.sandDockManager1.GetDockControls(DockSituation.Document).Length == 0) {
                    e.Effect = GeodatabaseUtility.ValidDataObject(e) ? DragDropEffects.All : DragDropEffects.None;
                }
                else {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Form_Exception(object sender, ExceptionHandlerEventArgs e) {
            if (this.statusBarItemError.Image != this._bitmapError) {
                this.statusBarItemError.Image = this._bitmapError;
            }
            if (this.statusBarItemError.ToolTipText != e.Exception.Message) {
                this.statusBarItemError.ToolTipText = e.Exception.Message;
            }
            if (SettingsWindow.Default.ShowExceptionWindow) {
                ExceptionDialog.Default.Show(this);
            }
        }
        private void MenuItem_BeforePopup(object sender, TD.SandBar.MenuPopupEventArgs e) {
            try {
                // Get Schema and ActiveModel
                SchemaModel schemaModel = null;
                EsriModel activeModel = null;
                DockControl[] dockControls = this.sandDockManager1.GetDockControls(DockSituation.Document);
                TabbedDocument activeTab = null;
                if (dockControls.Length > 0) {
                    foreach (TabbedDocument tab2 in dockControls) {
                        if (tab2 is ITabModel) {
                            ITabModel tabModel = (ITabModel)tab2;
                            if (tabModel.Model.GetType() == typeof(SchemaModel)) {
                                schemaModel = (SchemaModel)tabModel.Model;
                                break;
                            }
                        }
                    }
                    activeTab = this.sandDockManager1.ActiveTabbedDocument as TabbedDocument;
                    if (activeTab is ITabModel) {
                        ITabModel tabModel = (ITabModel)activeTab;
                        activeModel = tabModel.Model;
                    }
                }

                if (sender == this.menuBarItemFile) {
                    this.menuButtonItemNew.Enabled = true;
                    this.menuButtonItemOpen.Enabled = true;
                    this.menuButtonItemClose.Enabled = (schemaModel != null);
                    this.menuButtonItemSave.Enabled = (schemaModel != null);
                    this.menuButtonItemSaveAs.Enabled = (schemaModel != null);
                    this.menuButtonItemPublish.Enabled = (schemaModel != null);
                    this.menuButtonItemExport.Enabled = (dockControls.Length != 0);
                    this.menuButtonItemPrintSetup.Enabled = (activeTab != null && (activeTab is ITabModel || activeTab is ITabPrinter));
                    this.menuButtonItemPrint.Enabled = (dockControls.Length != 0);
                    this.menuButtonItemPrintPreview.Enabled = (dockControls.Length != 0);
                    this.menuButtonItemExit.Enabled = true;

                    string save = (schemaModel == null) ? string.Empty : schemaModel.Title;
                    this.menuButtonItemSave.Text = string.Format(Resources.TEXT_SAVE_, save);
                    this.menuButtonItemSaveAs.Text = string.Format(Resources.TEXT_SAVE_AS_, save);
                }
                else if (sender == this.menuBarItemEdit) {
                    bool cut = (activeModel != null) && (activeModel.CanCut);
                    bool copy = (activeModel != null) && (activeModel.CanCopy);
                    bool paste = (activeModel != null) && (activeModel.CanPaste);
                    bool delete = (activeModel != null) && (activeModel.CanDelete);
                    bool undo = (activeModel != null) && (activeModel.CanUndo);
                    bool redo = (activeModel != null) && (activeModel.CanRedo);
                    bool selectAll = (activeModel != null) && (activeModel.CanSelectAll);

                    this.menuButtonItemCut.Enabled = cut;
                    this.menuButtonItemCopy.Enabled = copy;
                    this.menuButtonItemPaste.Enabled = paste;
                    this.menuButtonItemDelete.Enabled = delete;
                    this.menuButtonItemSelectAll.Enabled = selectAll;
                    this.menuButtonItemUndo.Enabled = undo;
                    this.menuButtonItemRedo.Enabled = redo;
                }
                else if (sender == this.menuBarItemView) {
                    this.menuButtonItemColorScheme.Enabled = true;
                    this.menuButtonItemLayout.Enabled = true;
                    this.menuButtonItemToolbars.Enabled = true;
                    this.menuButtonItemZoom.Enabled = true;
                    this.menuButtonItemExceptions.Enabled = true;
                    this.menuButtonItemStatusBar.Enabled = true;
                    this.menuButtonItemOverviewWindow.Enabled = true;
                    this.menuButtonItemCatalogWindow.Enabled = true;
                    this.menuButtonItemPaletteWindow.Enabled = true;
                    this.menuButtonItemPropertiesWindow.Enabled = true;
                    this.menuButtonItemErrorList.Enabled = true;

                    this.menuButtonItemOverviewWindow.Checked = (this.dockableWindowOverview.IsOpen || this.dockableWindowOverview.DockSituation == DockSituation.Docked);
                    this.menuButtonItemCatalogWindow.Checked = (this.dockableWindowCatalog.IsOpen || this.dockableWindowCatalog.DockSituation == DockSituation.Docked);
                    this.menuButtonItemPaletteWindow.Checked = (this.dockableWindowPalette.IsOpen || this.dockableWindowPalette.DockSituation == DockSituation.Docked);
                    this.menuButtonItemPropertiesWindow.Checked = (this.dockableWindowProperties.IsOpen || this.dockableWindowProperties.DockSituation == DockSituation.Docked);
                    this.menuButtonItemErrorList.Checked = (this.dockableWindowErrorList.IsOpen || this.dockableWindowErrorList.DockSituation == DockSituation.Docked);
                    this.menuButtonItemStatusBar.Checked = this.statusBar1.Visible;
                }
                else if (sender == this.menuButtonItemColorScheme) {
                    this.menuButtonItemColorSchemeBlack.Checked = (ColorSchemeSettings.Default.ColorScheme == Office2007ColorScheme.Black);
                    this.menuButtonItemColorSchemeBlue.Checked = (ColorSchemeSettings.Default.ColorScheme == Office2007ColorScheme.Blue);
                    this.menuButtonItemColorSchemeSilver.Checked = (ColorSchemeSettings.Default.ColorScheme == Office2007ColorScheme.Silver);
                }
                else if (sender == this.menuButtonItemLayout) {
                    bool hasActiveModel = activeModel != null;
                    bool hasSelectedShapes = hasActiveModel && activeModel.SelectedShapes().Count > 1;

                    this.menuButtonItemCircular.Enabled = hasActiveModel;
                    this.menuButtonItemForcedDirect.Enabled = hasActiveModel;
                    this.menuButtonItemHierachical.Enabled = hasActiveModel;
                    this.menuButtonItemOrthogonal.Enabled = hasActiveModel;
                    this.menuButtonItemTree.Enabled = hasActiveModel;

                    this.menuButtonItemAlignLeft.Enabled = hasSelectedShapes;
                    this.menuButtonItemAlignCenter.Enabled = hasSelectedShapes;
                    this.menuButtonItemAlignRight.Enabled = hasSelectedShapes;
                    this.menuButtonItemAlignTop.Enabled = hasSelectedShapes;
                    this.menuButtonItemAlignMiddle.Enabled = hasSelectedShapes;
                    this.menuButtonItemAlignBottom.Enabled = hasSelectedShapes;
                }
                else if (sender == this.menuButtonItemToolbars) {
                    this.menuButtonItemStandardToolbar.Enabled = true;
                    this.menuButtonItemLayoutToolbar.Enabled = true;
                    this.menuButtonItemInteractiveModeToolbar.Enabled = true;

                    this.menuButtonItemStandardToolbar.Checked = (this.toolBarStandard.Visible);
                    this.menuButtonItemLayoutToolbar.Checked = (this.toolBarLayout.Visible);
                    this.menuButtonItemInteractiveModeToolbar.Checked = (this.toolBarInteractiveMode.Visible);
                }
                else if (sender == this.menuButtonItemZoom) {
                    this.menuButtonItemZoom300.Enabled = (activeModel != null);
                    this.menuButtonItemZoom200.Enabled = (activeModel != null);
                    this.menuButtonItemZoom100.Enabled = (activeModel != null);
                    this.menuButtonItemZoom75.Enabled = (activeModel != null);
                    this.menuButtonItemZoom50.Enabled = (activeModel != null);
                    this.menuButtonItemZoom25.Enabled = (activeModel != null);
                    this.menuButtonItemZoomFull.Enabled = (activeModel != null);

                    this.menuButtonItemZoom300.Checked = (activeModel != null && activeModel.Zoom == 300f);
                    this.menuButtonItemZoom200.Checked = (activeModel != null && activeModel.Zoom == 200f);
                    this.menuButtonItemZoom100.Checked = (activeModel != null && activeModel.Zoom == 100f);
                    this.menuButtonItemZoom75.Checked = (activeModel != null && activeModel.Zoom == 75f);
                    this.menuButtonItemZoom50.Checked = (activeModel != null && activeModel.Zoom == 50f);
                    this.menuButtonItemZoom25.Checked = (activeModel != null && activeModel.Zoom == 25f);
                    this.menuButtonItemZoomFull.Checked = false;
                }
                else if (sender == this.menuBarItemTools) {
                    this.menuButtonItemNormalMode.Enabled = (activeModel != null);
                    this.menuButtonItemLinkMode.Enabled = (activeModel != null);
                    this.menuButtonItemSchemaReport.Enabled = true;
                    this.menuButtonItemDataReport.Enabled = true;
                    this.menuButtonItemValidate.Enabled = (schemaModel != null);
                    this.menuButtonItemOptions.Enabled = true;
                    this.menuButtonItemNormalMode.Checked = (activeModel != null && activeModel.Runtime.InteractiveMode == InteractiveMode.Normal);
                    this.menuButtonItemLinkMode.Checked = (activeModel != null && activeModel.Runtime.InteractiveMode == InteractiveMode.AddLine);
                }
                else if (sender == this.menuBarItemWindow) {
                    this.menuButtonItemNewHorizontalTabGroup.Enabled = (dockControls.Length > 1);
                    this.menuButtonItemNewVerticalTabGroup.Enabled = (dockControls.Length > 1);
                    this.menuButtonItemRestoreTabGroup.Enabled = (dockControls.Length > 1);
                    this.menuButtonItemCloseAllDocuments.Enabled = (dockControls.Length > 0);
                    this.menuButtonItemWindows.Enabled = (dockControls.Length > 0);
                }
                else if (sender == this.menuBarItemHelp) {
                    this.menuButtonItemUserGuide.Enabled = true;
                    this.menuButtonItemSendFeedback.Enabled = true;
                    this.menuButtonItemEsriHome.Enabled = true;
                    this.menuButtonItemEsriSupportCenter.Enabled = true;
                    this.menuButtonItemAbout.Enabled = true;
                }
                else if (sender == this._zoomMenu) {
                    foreach (ZoomMenuItem item in this._zoomMenu.Items) {
                        item.Checked = (activeModel != null && activeModel.Zoom == item.Zoom);
                    }
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                if (this.InvokeRequired) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { sender, e });
                }
                else {
                    // Get Schema and ActiveModel
                    SchemaModel schemaModel = null;
                    EsriModel activeModel = null;
                    DockControl[] dockControls = this.sandDockManager1.GetDockControls(DockSituation.Document);
                    TabbedDocument activeTab = null;
                    if (dockControls.Length > 0) {
                        foreach (TabbedDocument tab2 in dockControls) {
                            if (tab2 is ITabModel) {
                                ITabModel tabModel = (ITabModel)tab2;
                                if (tabModel.Model.GetType() == typeof(SchemaModel)) {
                                    schemaModel = (SchemaModel)tabModel.Model;
                                    break;
                                }
                            }
                        }
                        activeTab = this.sandDockManager1.ActiveTabbedDocument as TabbedDocument;
                        if (activeTab is ITabModel) {
                            ITabModel tabModel = (ITabModel)activeTab;
                            activeModel = tabModel.Model;
                        }
                    }
                    //
                    // File Menu
                    //
                    if (sender == this.menuButtonItemNew) {
                        // Prompt to close the schema model (if available)
                        if (schemaModel != null) {
                            if (schemaModel.Dirty) {
                                DialogResult dialogResult = MessageBox.Show(
                                       Resources.TEXT_SAVE_DOCUMENT_BEFORE_CLOSING,
                                       Resources.TEXT_ARCDIAGRAMMER,
                                       MessageBoxButtons.YesNoCancel,
                                       MessageBoxIcon.Question,
                                       MessageBoxDefaultButton.Button3);
                                switch (dialogResult) {
                                    case DialogResult.Yes:
                                        this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                                        break;
                                    case DialogResult.No:
                                        break;
                                    case DialogResult.Cancel:
                                    default:
                                        return;
                                }
                            }
                        }

                        // Close all tabs
                        foreach (TabbedDocument tab in dockControls) {
                            if (tab is ITabModel) {
                                ITabModel tabModel = (ITabModel)tab;
                                EsriModel model = tabModel.Model;
                                model.SuspendLayout();
                            }
                            tab.Close();
                        }

                        // Clear Catalog
                        this.catalog1.Suspend();
                        this.catalog1.SchemaModel = null;
                        this.catalog1.Resume();
                        this.catalog1.RefreshCatalog();

                        // Clear Property Grid
                        this.propertyEditor1.PropertyGrid.SelectedObjects = null;

                        // Clear Overview
                        this.esriOverview1.Suspend();
                        this.esriOverview1.Diagram = null;
                        this.esriOverview1.Resume();
                        this.esriOverview1.Refresh();

                        // Clear Error List
                        this.errorList1.Clear();

                        // Clear Schema Model in Diagrammer Environment Singleton
                        DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                        diagrammerEnvironment.SchemaModel = null;

                        // Create new schema tab
                        string filename = null;
                        this.CreateTabbedDocument(filename);
                    }
                    else if (sender == this.menuButtonItemOpen) {
                        // Prompt to close the schema model (if available)
                        if (schemaModel != null) {
                            if (schemaModel.Dirty) {
                                DialogResult dialogResult = MessageBox.Show(
                                       Resources.TEXT_SAVE_DOCUMENT_BEFORE_OPENING,
                                       Resources.TEXT_ARCDIAGRAMMER,
                                       MessageBoxButtons.YesNoCancel,
                                       MessageBoxIcon.Question,
                                       MessageBoxDefaultButton.Button3);
                                switch (dialogResult) {
                                    case DialogResult.Yes:
                                        this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                                        break;
                                    case DialogResult.No:
                                        break;
                                    case DialogResult.Cancel:
                                    default:
                                        return;
                                }
                            }
                        }

                        // Close all tabs
                        foreach (TabbedDocument tab in dockControls) {
                            if (tab is ITabModel) {
                                ITabModel tabModel = (ITabModel)tab;
                                EsriModel model = tabModel.Model;
                                model.SuspendLayout();
                            }
                            tab.Close();
                        }

                        // Clear Catalog
                        this.catalog1.Suspend();
                        this.catalog1.SchemaModel = null;
                        this.catalog1.Resume();
                        this.catalog1.RefreshCatalog();

                        // Clear Property Grid
                        this.propertyEditor1.PropertyGrid.SelectedObjects = null;

                        // Clear Overview
                        this.esriOverview1.Suspend();
                        this.esriOverview1.Diagram = null;
                        this.esriOverview1.Resume();
                        this.esriOverview1.Refresh();

                        // Clear Error List
                        this.errorList1.Clear();

                        // Clear Schema Model in Diagrammer Environment Singleton
                        DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                        diagrammerEnvironment.SchemaModel = null;

                        // Display OpenFileDialog
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.Filter =
                            Resources.TEXT_ALL_DIAGRAMMER_DOCUMENTS + " (*.xml; *.diagram)|*.xml;*.diagram|" +
                            Resources.TEXT_XML_WORKSPACE_DOCUMENT + " (*.xml)|*.xml|" +
                            Resources.TEXT_ARCGIS_DIAGRAMMER_DOCUMENT + " (*.diagram)|*.diagram";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.Multiselect = false;
                        openFileDialog.ShowHelp = true;
                        openFileDialog.Title = Resources.TEXT_OPEN_XML_WORKSPACE_DOCUMENT;

                        // Display Open File Dialog
                        if (openFileDialog.ShowDialog(this) != DialogResult.OK) { return; }

                        // Get Filename
                        string filename = openFileDialog.FileName;
                        if (string.IsNullOrEmpty(filename)) { return; }

                        // Open File
                        this.CreateTabbedDocument(filename);
                    }
                    else if (sender == this.menuButtonItemClose) {
                        // Prompt to close the schema model (if available)
                        if (schemaModel != null) {
                            if (schemaModel.Dirty) {
                                DialogResult dialogResult = MessageBox.Show(
                                       Resources.TEXT_SAVE_DOCUMENT_BEFORE_CLOSING,
                                       Resources.TEXT_ARCDIAGRAMMER,
                                       MessageBoxButtons.YesNoCancel,
                                       MessageBoxIcon.Question,
                                       MessageBoxDefaultButton.Button3);
                                switch (dialogResult) {
                                    case DialogResult.Yes:
                                        //this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                                        this.MenuItem_Activate(this.menuButtonItemSaveAs, EventArgs.Empty);
                                        break;
                                    case DialogResult.No:
                                        break;
                                    case DialogResult.Cancel:
                                    default:
                                        return;
                                }
                            }
                        }

                        // Close all tabs
                        foreach (TabbedDocument tab in dockControls) {
                            if (tab is ITabModel) {
                                ITabModel tabModel = (ITabModel)tab;
                                EsriModel model = tabModel.Model;
                                model.SuspendLayout();
                            }
                            tab.Close();
                        }

                        // Clear Catalog
                        this.catalog1.Suspend();
                        this.catalog1.SchemaModel = null;
                        this.catalog1.Resume();
                        this.catalog1.RefreshCatalog();

                        // Clear Property Grid
                        this.propertyEditor1.PropertyGrid.SelectedObjects = null;

                        // Clear Overview
                        this.esriOverview1.Suspend();
                        this.esriOverview1.Diagram = null;
                        this.esriOverview1.Resume();
                        this.esriOverview1.Refresh();

                        // Clear Error List
                        this.errorList1.Clear();

                        // Clear Schema Model in Diagrammer Environment Singleton
                        DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                        diagrammerEnvironment.SchemaModel = null;
                    }
                    else if (sender == this.menuButtonItemSave) {
                        // Exit if no model
                        if (schemaModel == null) { return; }

                        // If not path then SaveAs
                        if (string.IsNullOrEmpty(schemaModel.Document)) {
                            this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                        }
                        else {
                            // Get Directory
                            string directory = Path.GetDirectoryName(schemaModel.Document);

                            // If Directory is in
                            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) {
                                this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                            }
                            else {
                                // Save Model
                                schemaModel.Save(schemaModel.Document, SaveFormat.Binary);
                                schemaModel.Dirty = false;
                            }
                        }

                        // Refresh PropertyGrid
                        this.propertyEditor1.PropertyGrid.Refresh();
                    }
                    else if (sender == this.menuButtonItemSaveAs) {
                        // Exit if no model
                        if (schemaModel == null) { return; }

                        // Show SaveAs Dialog
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.DefaultExt = "diagram";
                        saveFileDialog.FileName = schemaModel.Title;
                        saveFileDialog.Filter = Resources.TEXT_ARCGIS_DIAGRAMMER_DOCUMENT + " (*.diagram)|*.diagram"; //  +
                        saveFileDialog.FilterIndex = 1;
                        saveFileDialog.OverwritePrompt = true;
                        saveFileDialog.RestoreDirectory = false;
                        saveFileDialog.ShowHelp = true;
                        saveFileDialog.Title = Resources.TEXT_SAVE_DIAGRAM;

                        // Check if user pressed "Save" and File is OK.
                        if (saveFileDialog.ShowDialog(this) != DialogResult.OK) { return; }
                        if (string.IsNullOrEmpty(saveFileDialog.FileName)) { return; }

                        // Save to binary file
                        schemaModel.Save(saveFileDialog.FileName, SaveFormat.Binary);
                        schemaModel.Document = saveFileDialog.FileName;
                        schemaModel.Dirty = false;

                        // Refresh PropertyGrid
                        this.propertyEditor1.PropertyGrid.Refresh();
                    }
                    else if (sender == this.menuButtonItemPublish) {
                        // Exit if no model
                        if (schemaModel == null) { return; }

                        // Show SaveAs Dialog
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.DefaultExt = "xml";
                        saveFileDialog.FileName = schemaModel.Title;
                        saveFileDialog.Filter = Resources.TEXT_XML_WORKSPACE_DOCUMENT + " (*.xml)|*.xml";
                        saveFileDialog.FilterIndex = 1;
                        saveFileDialog.OverwritePrompt = true;
                        saveFileDialog.RestoreDirectory = false;
                        saveFileDialog.ShowHelp = true;
                        saveFileDialog.Title = Resources.TEXT_SAVE_DIAGRAM_AS_XML_WORKSPACE_DOCUMENT;

                        // Check if user pressed "Save" and File is OK.
                        if (saveFileDialog.ShowDialog(this) != DialogResult.OK) { return; }
                        if (string.IsNullOrEmpty(saveFileDialog.FileName)) { return; }

                        // Save Model to an Xml Workspace Document
                        schemaModel.PublishModel(saveFileDialog.FileName);
                    }
                    else if (sender == this.menuButtonItemExport) {
                        if (activeTab is ITabModel) {
                            // Exit if not Active Model
                            if (activeModel == null) { return; }

                            // Show SaveAs Dialog
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.DefaultExt = "bmp";
                            saveFileDialog.FileName = activeTab.Text;
                            saveFileDialog.Filter =
                                "Windows Bitmap" + " (*.bmp)|*.bmp|" +
                                "Windows Metafile" + " (*.wmf)|*.wmf|" +
                                "GIF" + " (*.gif)|*.gif|" +
                                "JPEG" + " (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Graphics" + " (*.png)|*.png|" +
                                "Scalar Vector Graphic" + " (*.svg)|*.svg";
                            saveFileDialog.FilterIndex = 1;
                            saveFileDialog.OverwritePrompt = true;
                            saveFileDialog.RestoreDirectory = false;
                            saveFileDialog.ShowHelp = true;
                            saveFileDialog.Title = Resources.TEXT_EXPORT_DIAGRAM;

                            // Check if user pressed "Save" and File is OK.
                            if (saveFileDialog.ShowDialog(this) != DialogResult.OK) { return; }
                            if (string.IsNullOrEmpty(saveFileDialog.FileName)) { return; }

                            //
                            switch (saveFileDialog.FilterIndex) {
                                case 1:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Bmp);
                                    break;
                                case 2:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Metafile);
                                    break;
                                case 3:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Gif);
                                    break;
                                case 4:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Jpeg);
                                    break;
                                case 5:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Png);
                                    break;
                                case 6:
                                    activeModel.ExportModel(saveFileDialog.FileName, SaveFormat.Svg);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (activeTab is ITabPrinter) {
                            ITabPrinter tabPrinter = (ITabPrinter)activeTab;
                            tabPrinter.SaveAs();
                        }
                    }
                    else if (sender == this.menuButtonItemPrintSetup) {
                        if (activeTab is ITabModel) {
                            // Display Diagrammer Printer Setup Window
                            Office2007Renderer renderer = (Office2007Renderer)this.sandBarManager1.Renderer;
                            FormPrinterSetup form = new FormPrinterSetup();
                            form.BackColor = renderer.BackgroundGradientColor1;
                            form.Font = this.Font;
                            form.ShowDialog(this);
                        }
                        else if (activeTab is ITabPrinter) {
                            ITabPrinter tabPrinter = (ITabPrinter)activeTab;
                            tabPrinter.PageSetup();
                        }
                    }
                    else if (sender == this.menuButtonItemPrint) {
                        if (activeTab is ITabModel) {
                            // Exit if not Active Model
                            if (activeModel == null) { return; }

                            // Get Print Document
                            Crainiate.ERM4.Printing.PrintDocument printDocument = new Crainiate.ERM4.Printing.PrintDocument(activeModel);

                            //
                            DiagramPrinterSettings printer = DiagramPrinterSettings.Default;

                            // Set Printer Properties
                            if (printer.PrinterName != string.Empty) {
                                printDocument.PrinterSettings.PrinterName = printer.PrinterName;
                            }
                            if (printer.PrinterPaperSize != string.Empty) {
                                foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes) {
                                    if (paperSize.PaperName == printer.PrinterPaperSize) {
                                        printDocument.DefaultPageSettings.PaperSize = paperSize;
                                        break;
                                    }
                                }
                            }
                            printDocument.DefaultPageSettings.Landscape = printer.Landscape;
                            printDocument.DefaultPageSettings.Margins = printer.Margins;

                            // Assign Print Document to Print Dialog
                            this.printDialog1.Document = printDocument;
                            DialogResult dialogResult = this.printDialog1.ShowDialog(this);

                            // If the result is OK then print the document.
                            switch (dialogResult) {
                                case DialogResult.OK:
                                    printDocument.Print();
                                    break;
                                case DialogResult.Cancel:
                                default:
                                    break;
                            }
                        }
                        else if (activeTab is ITabPrinter) {
                            ITabPrinter tabPrinter = (ITabPrinter)activeTab;
                            tabPrinter.Print();
                        }
                    }
                    else if (sender == this.menuButtonItemPrintPreview) {
                        if (activeTab is ITabModel) {
                            // Exit if not Active Model
                            if (activeModel == null) { return; }

                            // Preview Printing
                            Crainiate.ERM4.Printing.PrintDocument printDocument = new Crainiate.ERM4.Printing.PrintDocument(activeModel);

                            //
                            DiagramPrinterSettings printer = DiagramPrinterSettings.Default;

                            if (printer.PrinterName != string.Empty) {
                                printDocument.PrinterSettings.PrinterName = printer.PrinterName;
                            }
                            if (printer.PrinterPaperSize != string.Empty) {
                                foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes) {
                                    if (paperSize.PaperName == printer.PrinterPaperSize) {
                                        printDocument.DefaultPageSettings.PaperSize = paperSize;
                                        break;
                                    }
                                }
                            }
                            printDocument.DefaultPageSettings.Landscape = printer.Landscape;
                            printDocument.DefaultPageSettings.Margins = printer.Margins;

                            // Assign Print Document to Print Dialog
                            this.printPreviewDialog1.Font = new Font("Tahoma", 8);
                            this.printPreviewDialog1.Icon = Resources.DIAGRAMMER;
                            this.printPreviewDialog1.Document = printDocument;
                            DialogResult dialogResult = this.printPreviewDialog1.ShowDialog(this);
                        }
                        else if (activeTab is ITabPrinter) {
                            ITabPrinter tabPrinter = (ITabPrinter)activeTab;
                            tabPrinter.PrintPreview();
                        }
                    }
                    else if (sender == this.menuButtonItemExit) {
                        this.Close();
                    }
                    //
                    // Edit Menu
                    //
                    else if (sender == this.menuButtonItemUndo) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            SendMessage(this.propertyEditor1.PropertyGrid.Handle, WM_UNDO, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Exit if no model
                            if (activeModel == null || !activeModel.CanUndo) { return; }

                            // Select All Elements
                            activeModel.DoCommand("undo");

                            // Fire Model SelectionChanged Event
                            activeModel.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { activeModel, EventArgs.Empty });
                        }
                    }
                    else if (sender == this.menuButtonItemRedo) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            SendMessage(this.propertyEditor1.PropertyGrid.Handle, WM_UNDO, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Exit if no model
                            if (activeModel == null || !activeModel.CanRedo) { return; }

                            // Select All Elements
                            activeModel.DoCommand("redo");

                            // Fire Model SelectionChanged Event
                            activeModel.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { activeModel, EventArgs.Empty });
                        }
                    }
                    else if (sender == this.menuButtonItemCut) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            SendMessage(this.propertyEditor1.PropertyGrid.Handle, WM_CUT, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Exit if no model
                            if (activeModel == null || !activeModel.CanCut) { return; }

                            // Select All Elements
                            activeModel.DoCommand("cut");

                            // Fire Model SelectionChanged Event
                            activeModel.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { activeModel, EventArgs.Empty });
                        }
                    }
                    else if (sender == this.menuButtonItemCopy) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            FormDiagrammer.SendMessage(this.propertyEditor1.PropertyGrid.Handle, FormDiagrammer.WM_COPY, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Exit if no model
                            if (activeModel == null || !activeModel.CanCopy) { return; }

                            // Select All Elements
                            activeModel.DoCommand("copy");
                        }
                    }
                    else if (sender == this.menuButtonItemPaste) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            SendMessage(this.propertyEditor1.PropertyGrid.Handle, WM_PASTE, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Select All Elements
                            activeModel.DoCommand("paste");

                            // Fire Model SelectionChanged Event
                            activeModel.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { activeModel, EventArgs.Empty });
                        }
                    }
                    else if (sender == this.menuButtonItemDelete) {
                        if (this.propertyEditor1.PropertyGrid.RectangleToScreen(this.propertyEditor1.PropertyGrid.ClientRectangle).Contains(Cursor.Position)) {
                            SendMessage(this.propertyEditor1.PropertyGrid.Handle, WM_CLEAR, IntPtr.Zero, IntPtr.Zero);
                        }
                        else {
                            // Exit if no model
                            if (activeModel == null || !activeModel.CanDelete) { return; }

                            // Select All Elements
                            activeModel.DoCommand("delete");

                            // Fire Model SelectionChanged Event
                            activeModel.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { activeModel, EventArgs.Empty });
                        }
                    }
                    else if (sender == this.menuButtonItemSelectAll) {
                        // Exit if no model
                        if (activeModel == null || !activeModel.CanSelectAll) { return; }

                        // Select All Elements
                        activeModel.SelectElements(true);
                    }
                    //
                    // View Menu
                    //
                    else if (sender == this.menuButtonItemOverviewWindow) {
                        if (this.dockableWindowOverview.IsOpen) {
                            this.dockableWindowOverview.Close();
                        }
                        else {
                            if (this.dockableWindowOverview.DockSituation != DockSituation.Docked) {
                                this.dockableWindowOverview.Open();
                            }
                        }
                    }
                    else if (sender == this.menuButtonItemCatalogWindow) {
                        if (this.dockableWindowCatalog.IsOpen) {
                            this.dockableWindowCatalog.Close();
                        }
                        else {
                            if (this.dockableWindowCatalog.DockSituation != DockSituation.Docked) {
                                this.dockableWindowCatalog.Open();
                            }
                        }
                    }
                    else if (sender == this.menuButtonItemPaletteWindow) {
                        if (this.dockableWindowPalette.IsOpen) {
                            this.dockableWindowPalette.Close();
                        }
                        else {
                            if (this.dockableWindowPalette.DockSituation != DockSituation.Docked) {
                                this.dockableWindowPalette.Open();
                            }
                        }
                    }
                    else if (sender == this.menuButtonItemPropertiesWindow) {
                        if (this.dockableWindowProperties.IsOpen) {
                            this.dockableWindowProperties.Close();
                        }
                        else {
                            if (this.dockableWindowProperties.DockSituation != DockSituation.Docked) {
                                this.dockableWindowProperties.Open();
                            }
                        }
                    }
                    else if (sender == this.menuButtonItemErrorList) {
                        if (this.dockableWindowErrorList.IsOpen) {
                            this.dockableWindowErrorList.Close();
                        }
                        else {
                            if (this.dockableWindowErrorList.DockSituation != DockSituation.Docked) {
                                this.dockableWindowErrorList.Open();
                            }
                        }
                    }
                    else if (sender == this.menuButtonItemExceptions) {
                        ExceptionDialog.Default.Show(this);
                    }
                    else if (sender == this.menuButtonItemStatusBar) {
                        this.statusBar1.Visible = !(this.statusBar1.Visible);
                    }
                    //
                    // View > Color Scheme Menu
                    //
                    else if (sender == this.menuButtonItemColorSchemeBlack) {
                        ColorSchemeSettings.Default.ColorScheme = Office2007ColorScheme.Black;
                        ColorSchemeSettings.Default.Save();
                    }
                    else if (sender == this.menuButtonItemColorSchemeBlue) {
                        ColorSchemeSettings.Default.ColorScheme = Office2007ColorScheme.Blue;
                        ColorSchemeSettings.Default.Save();
                    }
                    else if (sender == this.menuButtonItemColorSchemeSilver) {
                        ColorSchemeSettings.Default.ColorScheme = Office2007ColorScheme.Silver;
                        ColorSchemeSettings.Default.Save();
                    }
                    //
                    // View > Layout Menu
                    //
                    else if (sender == this.menuButtonItemCircular) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // 
                        activeModel.ExecuteLayout(typeof(CircularLayout), false);

                        // Recalculate scale of Overview Window
                        this.esriOverview1.ZoomFullExtent();
                    }
                    else if (sender == this.menuButtonItemForcedDirect) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        //
                        activeModel.ExecuteLayout(typeof(ForceDirectedLayout), false);

                        // Recalculate scale of Overview Window
                        this.esriOverview1.ZoomFullExtent();
                    }
                    else if (sender == this.menuButtonItemHierachical) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        //
                        activeModel.ExecuteLayout(typeof(HierarchicalLayout), false);

                        // Recalculate scale of Overview Window
                        this.esriOverview1.ZoomFullExtent();
                    }
                    else if (sender == this.menuButtonItemOrthogonal) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        //
                        activeModel.ExecuteLayout(typeof(OrthogonalLayout), false);

                        // Recalculate scale of Overview Window
                        this.esriOverview1.ZoomFullExtent();
                    }
                    else if (sender == this.menuButtonItemTree) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        //
                        activeModel.ExecuteLayout(typeof(TreeLayout), false);

                        // Recalculate scale of Overview Window
                        this.esriOverview1.ZoomFullExtent();
                    }
                    else if (sender == this.menuButtonItemAlignLeft) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Min Left
                        float left = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(left)) {
                                left = shape.Location.X;
                                continue;
                            }
                            left = Math.Min(left, shape.Location.X);
                        }
                        if (float.IsNaN(left)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float leftTest = shape.Location.X;
                            if (leftTest == left) { continue; }
                            shape.Move(left - leftTest, 0f);
                        }
                    }
                    else if (sender == this.menuButtonItemAlignCenter) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Max Right
                        float center = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(center)) {
                                center = shape.Location.X + (shape.Width / 2);
                                break;
                            }
                        }
                        if (float.IsNaN(center)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float centerTest = shape.Location.X + (shape.Width / 2);
                            if (centerTest == center) { continue; }
                            shape.Move(center - centerTest, 0f);
                        }
                    }
                    else if (sender == this.menuButtonItemAlignRight) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Max Right
                        float right = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(right)) {
                                right = shape.Location.X + shape.Width;
                                continue;
                            }
                            right = Math.Max(right, shape.Location.X + shape.Width);
                        }
                        if (float.IsNaN(right)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float rightTest = shape.Location.X + shape.Width;
                            if (rightTest == right) { continue; }
                            shape.Move(right - rightTest, 0f);
                        }
                    }
                    else if (sender == this.menuButtonItemAlignTop) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Max Right
                        float top = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(top)) {
                                top = shape.Location.Y;
                                continue;
                            }
                            top = Math.Min(top, shape.Location.Y);
                        }
                        if (float.IsNaN(top)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float topTest = shape.Location.Y;
                            if (topTest == top) { continue; }
                            shape.Move(0f, top - topTest);
                        }
                    }
                    else if (sender == this.menuButtonItemAlignMiddle) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Max Right
                        float middle = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(middle)) {
                                middle = shape.Location.Y + (shape.Height / 2);
                                break;
                            }
                        }
                        if (float.IsNaN(middle)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float middleTest = shape.Location.Y + (shape.Height / 2);
                            if (middleTest == middle) { continue; }
                            shape.Move(0f, middle - middleTest);
                        }
                    }
                    else if (sender == this.menuButtonItemAlignBottom) {
                        // Exit if no model
                        if (activeModel == null) { return; }

                        // Get Elements
                        Elements elements = activeModel.SelectedShapes();
                        if (elements.Count < 2) { return; }

                        // Get Max Right
                        float bottom = float.NaN;
                        foreach (Shape shape in elements.Values) {
                            if (float.IsNaN(bottom)) {
                                bottom = shape.Location.Y + shape.Height;
                                continue;
                            }
                            bottom = Math.Max(bottom, shape.Location.Y + shape.Height);
                        }
                        if (float.IsNaN(bottom)) { return; }

                        // Move Elements
                        foreach (Shape shape in elements.Values) {
                            float bottomTest = shape.Location.Y + shape.Height;
                            if (bottomTest == bottom) { continue; }
                            shape.Move(0f, bottom - bottomTest);
                        }
                    }
                    //
                    // View > Toolbars Menu
                    //
                    else if (sender == this.menuButtonItemStandardToolbar) {
                        this.toolBarStandard.Visible = !(this.toolBarStandard.Visible);
                    }
                    else if (sender == this.menuButtonItemLayoutToolbar) {
                        this.toolBarLayout.Visible = !(this.toolBarLayout.Visible);
                    }
                    else if (sender == this.menuButtonItemInteractiveModeToolbar) {
                        this.toolBarInteractiveMode.Visible = !(this.toolBarInteractiveMode.Visible);
                    }
                    //
                    // View > Zoom Menu
                    //
                    else if (sender == this.menuButtonItemZoom300) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(300f);
                    }
                    else if (sender == this.menuButtonItemZoom200) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(200f);
                    }
                    else if (sender == this.menuButtonItemZoom100) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(100f);
                    }
                    else if (sender == this.menuButtonItemZoom75) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(75f);
                    }
                    else if (sender == this.menuButtonItemZoom50) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(50f);
                    }
                    else if (sender == this.menuButtonItemZoom25) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomModel(25f);
                    }
                    else if (sender == this.menuButtonItemZoomFull) {
                        // Exit if no model
                        if (activeModel == null) { return; }
                        activeModel.ZoomFullExtent();
                    }
                    //
                    // Tools Menu
                    //
                    else if (sender == this.menuButtonItemNormalMode) {
                        // Set Interactive Mode to Normal
                        if (activeModel == null) { return; }
                        activeModel.Runtime.InteractiveMode = InteractiveMode.Normal;
                    }
                    else if (sender == this.menuButtonItemLinkMode) {
                        // Set Interactive Mode to Link
                        if (activeModel == null) { return; }
                        activeModel.Runtime.InteractiveMode = InteractiveMode.AddLine;
                    }
                    else if (sender == this.menuButtonItemSchemaReport) {
                        // Add Schema Report Tabbed Documenet
                        this.CreateTabbedDocument(typeof(SchemaReport));
                    }
                    else if (sender == this.menuButtonItemDataReport) {
                        // Add Data Report Tabbed Document
                        this.CreateTabbedDocument(typeof(DataReport));
                    }
                    else if (sender == this.menuButtonItemXmlReport) {
                        // Add Data Report Tabbed Document
                        this.CreateTabbedDocument(typeof(XmlReport));
                    }
                    else if (sender == this.menuButtonItemValidate) {
                        // Exit if no model
                        if (schemaModel == null) { return; }

                        // Valid Schema Model
                        this.errorList1.Validate2();
                    }
                    else if (sender == this.menuButtonItemOptions) {
                        // Dispaly Options Window
                        Office2007Renderer r = (Office2007Renderer)this.sandBarManager1.Renderer;
                        FormOptions options = new FormOptions();
                        options.BackColor = r.BackgroundGradientColor1;
                        options.Font = this.Font;
                        options.ShowDialog(this);
                    }
                    //
                    // Window Menu
                    //
                    else if (sender == this.menuButtonItemNewHorizontalTabGroup ||
                             sender == this.menuButtonItemNewVerticalTabGroup) {
                        SplitLayoutSystem splitLayoutSystem = new SplitLayoutSystem();
                        if (sender == this.menuButtonItemNewHorizontalTabGroup) {
                            splitLayoutSystem.SplitMode = Orientation.Horizontal;
                        }
                        else if (sender == this.menuButtonItemNewVerticalTabGroup) {
                            splitLayoutSystem.SplitMode = Orientation.Vertical;
                        }
                        LayoutSystemBase[] layoutSystemBase = new LayoutSystemBase[this.sandDockManager1.GetDockControls(DockSituation.Document).Length];
                        foreach (DockControl dockControl in this.sandDockManager1.GetDockControls(DockSituation.Document)) {
                            DocumentLayoutSystem documentLayoutSystem = new DocumentLayoutSystem();
                            documentLayoutSystem.Controls.Add(dockControl);
                            splitLayoutSystem.LayoutSystems.Add(documentLayoutSystem);
                        }
                        this.sandDockManager1.DocumentContainer.LayoutSystem = splitLayoutSystem;
                    }
                    else if (sender == this.menuButtonItemRestoreTabGroup) {
                        SplitLayoutSystem splitLayoutSystem = new SplitLayoutSystem();
                        splitLayoutSystem.SplitMode = Orientation.Horizontal;
                        LayoutSystemBase[] layoutSystemBase = new LayoutSystemBase[1];
                        DocumentLayoutSystem documentLayoutSystem = new DocumentLayoutSystem();
                        foreach (DockControl dockControl in this.sandDockManager1.GetDockControls(DockSituation.Document)) {
                            documentLayoutSystem.Controls.Add(dockControl);
                        }
                        splitLayoutSystem.LayoutSystems.Add(documentLayoutSystem);
                        this.sandDockManager1.DocumentContainer.LayoutSystem = splitLayoutSystem;
                    }
                    else if (sender == this.menuButtonItemCloseAllDocuments) {
                        //
                        if (schemaModel != null) {
                            DialogResult dialogResult = MessageBox.Show(
                                Resources.TEXT_SAVE_DOCUMENT_BEFORE_CLOSING,
                                Resources.TEXT_ARCDIAGRAMMER,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button3);
                            switch (dialogResult) {
                                case DialogResult.Yes:
                                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSaveAs, EventArgs.Empty });
                                    break;
                                case DialogResult.No:
                                    break;
                                case DialogResult.Cancel:
                                default:
                                    return;
                            }
                        }

                        //
                        foreach (TabbedDocument tabbedDocument in this.sandDockManager1.GetDockControls(DockSituation.Document)) {
                            tabbedDocument.Close();
                        }

                        // Clear Catalog
                        this.catalog1.Suspend();
                        this.catalog1.SchemaModel = null;
                        this.catalog1.Resume();
                        this.catalog1.RefreshCatalog();

                        // Clear Property Grid
                        this.propertyEditor1.PropertyGrid.SelectedObjects = null;

                        // Clear Overview
                        this.esriOverview1.Suspend();
                        this.esriOverview1.Diagram = null;
                        this.esriOverview1.Resume();
                        this.esriOverview1.Refresh();

                        // Clear Error List
                        this.errorList1.Clear();

                        // Clear Schema Model in Diagrammer Environment Singleton
                        DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                        diagrammerEnvironment.SchemaModel = null;
                    }
                    else if (sender == this.menuButtonItemWindows) {
                        // Display Window Manager
                        Office2007Renderer r = (Office2007Renderer)this.sandBarManager1.Renderer;
                        FormWindows form = new FormWindows(this.sandDockManager1);
                        form.BackColor = r.BackgroundGradientColor1;
                        form.Font = this.Font;
                        form.ShowDialog(this);
                    }
                    //
                    // Help Menu
                    //
                    else if (sender == this.menuButtonItemUserGuide) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_USERGUIDE;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemCreateSchemaReport) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_CREATE_SCHEMA_REPORT;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemCreateDataReport) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_CREATE_DATA_REPORT;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemReorderFields) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_REORDER_FIELDS;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemAddSubtype) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_ADDSUBTYPE;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemCreateOneToManyRelationship) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_ADDRELATIONSHIP;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemSendFeedback) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_FEEDBACK;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemDiscussionForum) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_DISCUSSION;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemHistory) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_HISTORY;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemEsriHome) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_ESRI_HOME;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemEsriSupportCenter) {
                        Process process = new Process();
                        process.StartInfo.FileName = Resources.TEXT_URL_ARCGIS_IDEAS;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    else if (sender == this.menuButtonItemAbout) {
                        // Display About Dialog
                        Office2007Renderer r = (Office2007Renderer)this.sandBarManager1.Renderer;
                        FormAbout about = new FormAbout();
                        about.BackColor = r.BackgroundGradientColor1;
                        about.Font = this.Font;
                        about.ShowDialog(this);
                    }
                    else if (sender is ZoomMenuItem) {
                        if (activeModel == null) { return; }
                        ZoomMenuItem item = (ZoomMenuItem)sender;
                        if (item.Zoom == -1) {
                            activeModel.ZoomFullExtent();
                        }
                        else {
                            activeModel.ZoomModel(item.Zoom);
                        }
                    }
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Model_SelectedChanged(object sender, EventArgs e) {
            try {
                // Get Active Model
                if (sender == null) { return; }
                EsriModel model = (EsriModel)sender;

                // Get Selected Elements
                Elements elements = model.SelectedElements();

                // Get List of Elements
                List<object> list = new List<object>();

                foreach (Element element in elements.Values) {
                    if (element is EsriTable) {
                        list.Add(element);
                    }
                    else if (element is IParentObject) {
                        IParentObject parentObject = (IParentObject)element;
                        list.Add(parentObject.ParentObject);
                    }
                }

                // Examine Object Set
                switch (list.Count) {
                    case 0:
                        list.Add(model);
                        break;
                    case 1:
                        if (model.GetType() == typeof(SchemaModel)) {
                            Table table = (Table)list[0];
                            if (table.SelectedItem != null) {
                                list.Clear();
                                list.Add(table.SelectedItem);
                            }
                        }
                        break;
                    default:
                        break;
                }

                // Assign Object(s) to PropertyGrid
                switch (list.Count) {
                    case 0:
                        this.propertyEditor1.PropertyGrid.SelectedObject = null;
                        break;
                    default:
                        this.propertyEditor1.PropertyGrid.SelectedObjects = list.ToArray();
                        break;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Model_ElementInvalid(object sender, ElementEventArgs e) {
            this.propertyEditor1.PropertyGrid.Refresh();
        }
        private void SandDockManager_ActiveTabbedDocumentChanged(object sender, EventArgs e) {
            try {
                // Overview Window
                this.esriOverview1.Suspend();

                //
                TabbedDocument tabbedDocument = this.sandDockManager1.ActiveTabbedDocument as TabbedDocument;
                if (tabbedDocument is ITabModel) {
                    ITabModel tabModel = (ITabModel)tabbedDocument;
                    this.esriOverview1.Diagram = tabModel.Model;
                    this.esriOverview1.ZoomFullExtent();
                    tabModel.Model.Invoke(new EventHandler(this.Model_SelectedChanged), new object[] { tabModel.Model, EventArgs.Empty });
                }
                else if (tabbedDocument is ITabReport) {
                    ITabReport tabReport = (ITabReport)tabbedDocument;
                    this.propertyEditor1.PropertyGrid.SelectedObject = tabReport.Report;
                    this.esriOverview1.Diagram = null;
                }
                else if (tabbedDocument is TabbedDocumentMetadata) {
                    this.esriOverview1.Diagram = null;
                    this.propertyEditor1.PropertyGrid.SelectedObject = tabbedDocument;
                }
                else {
                    this.esriOverview1.Diagram = null;
                    this.propertyEditor1.PropertyGrid.SelectedObject = null;
                }

                //
                this.esriOverview1.Resume();
                this.esriOverview1.Refresh();
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void SchemaModel_DiagramRequest(object sender, DiagramEventArgs e) {
            try {
                this.CreateTabbedDocument(e.Table, e.Type);
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Timer_Tick(object sender, EventArgs e) {
            try {
                if (this.IsDisposed) { return; }
                if (this.Disposing) { return; }
                if (!this.Visible) { return; }
                if (this.WindowState == FormWindowState.Minimized) { return; }

                SchemaModel schemaModel = null;
                EsriModel activeModel = null;
                TabbedDocument tabbedDocumentSchema = null;
                DockControl[] dockControls = this.sandDockManager1.GetDockControls(DockSituation.Document);
                if (dockControls.Length > 0) {
                    foreach (TabbedDocument tab2 in dockControls) {
                        if (tab2 is ITabModel) {
                            ITabModel tabModel = (ITabModel)tab2;
                            if (tabModel.Model.GetType() == typeof(SchemaModel)) {
                                tabbedDocumentSchema = tab2;
                                schemaModel = (SchemaModel)tabModel.Model;
                                break;
                            }
                        }
                    }
                    TabbedDocument tabbedDocument = this.sandDockManager1.ActiveTabbedDocument as TabbedDocument;
                    if (tabbedDocument is ITabModel) {
                        ITabModel tabModel = (ITabModel)tabbedDocument;
                        activeModel = tabModel.Model;
                    }
                }

                // Update Application Window Title
                string title = string.Empty;
                if (schemaModel != null) {
                    title += string.Format("{0}", schemaModel.Title);
                    if (schemaModel.Dirty) {
                        title += string.Format("{0}", "*");
                    }
                    title += string.Format(" - ", schemaModel.Title);
                }
                title += Resources.TEXT_ARCDIAGRAMMER;
                if (this.Text != title) {
                    this.Text = title;
                }

                // Update Schema Tab Text
                if (tabbedDocumentSchema != null) {
                    string title2 = string.Empty;
                    if (schemaModel != null) {
                        if (!string.IsNullOrEmpty(schemaModel.Title)) {
                            title2 += string.Format("{0}", schemaModel.Title);
                        }
                    }
                    if (tabbedDocumentSchema.Text != title2) {
                        tabbedDocumentSchema.Text = title2;
                    }
                }

                // Main Toolbar
                if (this.toolBarStandard.Visible) {
                    // Save Toolbar Button
                    bool save = (schemaModel != null);
                    if (this.buttonItemSave.Enabled != save) {
                        this.buttonItemSave.Enabled = save;
                    }

                    // Export Toolbar Button
                    bool export = (dockControls.Length != 0);
                    if (this.buttonItemExport.Enabled != export) {
                        this.buttonItemExport.Enabled = export;
                    }

                    // Print Toolbar Button
                    bool print = (dockControls.Length != 0);
                    if (this.buttonItemPrint.Enabled != print) {
                        this.buttonItemPrint.Enabled = print;
                    }

                    //  Cut Toolbar Button
                    bool cut = (activeModel != null) && (activeModel.CanCut);
                    if (this.buttonItemCut.Enabled != cut) {
                        this.buttonItemCut.Enabled = cut;
                    }

                    //  Copy Toolbar Button
                    bool copy = (activeModel != null) && (activeModel.CanCut);
                    if (this.buttonItemCopy.Enabled != copy) {
                        this.buttonItemCopy.Enabled = copy;
                    }

                    //  Paste Toolbar Button
                    bool paste = (activeModel != null) && (activeModel.CanPaste);
                    if (this.buttonItemPaste.Enabled != paste) {
                        this.buttonItemPaste.Enabled = paste;
                    }

                    //  Delete Toolbar Button
                    bool delete = (activeModel != null) && (activeModel.CanDelete);
                    if (this.buttonItemDelete.Enabled != delete) {
                        this.buttonItemDelete.Enabled = delete;
                    }

                    //  Undo Toolbar Button
                    bool undo = (activeModel != null) && (activeModel.CanUndo);
                    if (this.buttonItemUndo.Enabled != undo) {
                        this.buttonItemUndo.Enabled = undo;
                    }

                    //  Redo Toolbar Button
                    bool redo = (activeModel != null) && (activeModel.CanRedo);
                    if (this.buttonItemRedo.Enabled != redo) {
                        this.buttonItemRedo.Enabled = redo;
                    }

                    //  Publish Toolbar Button
                    bool publish = (schemaModel != null);
                    if (this.buttonItemPublish.Enabled != publish) {
                        this.buttonItemPublish.Enabled = publish;
                    }
                }

                // Layout Toolbar
                if (this.toolBarLayout.Visible) {
                    // Only enable buttons if model present
                    bool hasActiveModel = (activeModel != null);
                    bool hasSelectedShapes = hasActiveModel && activeModel.SelectedShapes().Count > 1;

                    // Circular Toolbar Button
                    if (this.buttonItemCircular.Enabled != hasActiveModel) {
                        this.buttonItemCircular.Enabled = hasActiveModel;
                    }

                    // ForcedDirect Toolbar Button
                    if (this.buttonItemForcedDirect.Enabled != hasActiveModel) {
                        this.buttonItemForcedDirect.Enabled = hasActiveModel;
                    }

                    // Hierachical Toolbar Button
                    if (this.buttonItemHierachical.Enabled != hasActiveModel) {
                        this.buttonItemHierachical.Enabled = hasActiveModel;
                    }

                    // Orthogonal Toolbar Button
                    if (this.buttonItemOrthogonal.Enabled != hasActiveModel) {
                        this.buttonItemOrthogonal.Enabled = hasActiveModel;
                    }

                    // Tree Toolbar Button
                    if (this.buttonItemTree.Enabled != hasActiveModel) {
                        this.buttonItemTree.Enabled = hasActiveModel;
                    }

                    // Align Left
                    if (this.buttonItemAlignLeft.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignLeft.Enabled = hasSelectedShapes;
                    }

                    // Align Center
                    if (this.buttonItemAlignCenter.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignCenter.Enabled = hasSelectedShapes;
                    }

                    // Align Right
                    if (this.buttonItemAlignRight.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignRight.Enabled = hasSelectedShapes;
                    }

                    // Align Top
                    if (this.buttonItemAlignTop.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignTop.Enabled = hasSelectedShapes;
                    }

                    // Align Middle
                    if (this.buttonItemAlignMiddle.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignMiddle.Enabled = hasSelectedShapes;
                    }

                    // Align Bottom
                    if (this.buttonItemAlignBottom.Enabled != hasSelectedShapes) {
                        this.buttonItemAlignBottom.Enabled = hasSelectedShapes;
                    }
                }

                // Interactive Mode Toolbar
                if (this.toolBarInteractiveMode.Visible) {
                    // Only enable buttons if model present
                    bool enabled = (activeModel != null);

                    // Normal Interactive Mode Button
                    if (this.buttonItemNormal.Enabled != enabled) {
                        this.buttonItemNormal.Enabled = enabled;
                    }

                    // Link Interactive Mode Button
                    if (this.buttonItemLink.Enabled != enabled) {
                        this.buttonItemLink.Enabled = enabled;
                    }

                    bool normal = (activeModel != null && activeModel.Runtime.InteractiveMode == InteractiveMode.Normal);
                    if (this.buttonItemNormal.Checked != normal) {
                        this.buttonItemNormal.Checked = normal;
                    }

                    bool link = (activeModel != null && activeModel.Runtime.InteractiveMode == InteractiveMode.AddLine);
                    if (this.buttonItemLink.Checked != link) {
                        this.buttonItemLink.Checked = link;
                    }
                }

                // StatusBar
                if (this.statusBar1.Visible) {
                    if (activeModel == null) {
                        if (this._zoomMenu.Visible != false) {
                            this._zoomMenu.Visible = false;
                        }
                    }
                    else {
                        if (this._zoomMenu.Visible != true) {
                            this._zoomMenu.Visible = true;
                        }
                        string zoom = string.Format("{0}%", activeModel.Zoom.ToString());
                        if (this._zoomMenu.Text != zoom) {
                            this._zoomMenu.Text = zoom;
                        }
                    }
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }   
        }
        private void ToolBar_ButtonClick(object sender, ToolBarItemEventArgs e) {
            try {
                if (e.Item == this.buttonItemNew) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemNew, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemOpen) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemOpen, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemSave) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemSave, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemPublish) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemPublish, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemExport) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemExport, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemPrint) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemPrint, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemCut) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemCut, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemCopy) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemCopy, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemPaste) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemPaste, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemDelete) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemDelete, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemUndo) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemUndo, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemRedo) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemRedo, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemCircular) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemCircular, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemForcedDirect) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemForcedDirect, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemHierachical) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemHierachical, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemOrthogonal) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemOrthogonal, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemTree) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemTree, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignLeft) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignLeft, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignCenter) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignCenter, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignRight) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignRight, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignTop) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignTop, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignMiddle) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignMiddle, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemAlignBottom) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemAlignBottom, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemNormal) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemNormalMode, EventArgs.Empty });
                }
                else if (e.Item == this.buttonItemLink) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemLinkMode, EventArgs.Empty });
                }
                else if (e.Item == this._zoomMenu) {
                    this._zoomMenu.Show();
                }
                else if (e.Item == this.statusBarItemError) {
                    this.statusBarItemError.Image = this._bitmapNoError;
                    this.statusBarItemError.ToolTipText = string.Empty;

                    ExceptionDialog exceptionDialog = ExceptionDialog.Default;
                    if (!exceptionDialog.Visible) {
                        exceptionDialog.Show();
                    }
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        //
        // NESTED CLASSES
        //
        private class ZoomMenuItem : MenuButtonItem {
            private readonly float _zoom = 0f;
            public ZoomMenuItem(string text, float action, EventHandler eventHandler) : base() {
                this.Text = text;
                this._zoom = action;
                this.Activate += eventHandler;
            }
            public float Zoom {
                get { return this._zoom; }
            }
        }
    }
}
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
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.Geodatabase;
using TD.SandBar;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public partial class ErrorListDiagram : UserControl {
        private const string ERRORS_ITEM = "errors_item";
        private const string WARNINGS_ITEM = "warnings_item";
        private List<Error> m_errors = null;
        //
        // CONSTRUCTOR
        //
        public ErrorListDiagram() {
            InitializeComponent();

            // Error List
            this.buttonItemErrors.Text = string.Format(Resources.TEXT_ERRORS_, "0");
            this.buttonItemWarnings.Text = string.Format(Resources.TEXT_WARNINGS_, "0");
            this.buttonItemErrors.Image = Resources.BITMAP_ERRORS;
            this.buttonItemWarnings.Image = Resources.BITMAP_WARNINGS;
            this.dropDownMenuItemValidator.Text = Resources.TEXT_VALIDATOR_;
            this.menuButtonItemPGdb.Text = Resources.TEXT_PERSONAL_GEODATABASE_;
            this.menuButtonItemFGdb.Text = Resources.TEXT_FILE_GEODATABASE_;
            this.menuButtonItemSdeConnection.Text = Resources.TEXT_SDE_CONNECTION_;
            this.menuButtonItemSelectGeodatabase.Text = Resources.TEXT_SELECT__;

            // MenuButtonItem Text (Zoom Context Menu)
            this.menuButtonItemScroll.Text = Resources.TEXT_SCROLL_;
            this.menuButtonItemSelect.Text = Resources.TEXT_SELECT_;
            this.menuButtonItemFlashError.Text = Resources.TEXT_FLASH_;
            this.menuButtonItemClearError.Text = Resources.TEXT_CLEAR_;
            this.menuButtonItemClearAllErrors.Text = Resources.TEXT_CLEAR_ALL_;

            // Load Error List Image List
            this.listViewErrorList.SmallImageList.Images.Add(ErrorListDiagram.ERRORS_ITEM, Resources.BITMAP_ERRORS_ITEM);
            this.listViewErrorList.SmallImageList.Images.Add(ErrorListDiagram.WARNINGS_ITEM, Resources.BITMAP_WARNINGS_ITEM);

            // Update Error List
            this.m_errors = new List<Error>();
            this.RefreshErrorList();

            // Update Renderer
            ColorSchemeSettings.Default.PropertyChanged += new PropertyChangedEventHandler(this.ColorScheme_PropertyChanged);
            this.ColorScheme_PropertyChanged(null, null);
        }
        //
        // PUBLIC METHODS
        //
        public void Validate2() {
            // Get Schema Model
            SchemaModel schemaModel = DiagrammerEnvironment.Default.SchemaModel;
            if (schemaModel == null) { return; }

            // Clear Errors
            this.m_errors.Clear();

            // Get Errors
            this.m_errors = schemaModel.Errors;

            // Refresh Error List
            this.RefreshErrorList();
        }
        public void Validate2(EsriTable table) {
            // Clear Errors
            this.m_errors.Clear();
            
            // Get Errors
            table.Errors(this.m_errors);

            // Refresh Error List
            this.RefreshErrorList();
        }
        public void Clear() {
            // Clear Errors
            this.m_errors.Clear();

            // Refresh Error List
            this.RefreshErrorList();
        }
        //
        // PRIVATE METHODS
        //
        private void ColorScheme_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            try {
                // Update SandBar
                Office2007Renderer sandBarRenderer = (Office2007Renderer)this.sandBarManager1.Renderer;
                if (sandBarRenderer.ColorScheme == ColorSchemeSettings.Default.ColorScheme) { return; }
                sandBarRenderer.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

                Office2007Renderer sandBarRenderer2 = (Office2007Renderer)this.toolBarErrorList.Renderer;
                sandBarRenderer2.ColorScheme = ColorSchemeSettings.Default.ColorScheme;
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void ListView_DoubleClick(object sender, EventArgs e) {
            try {
                if (sender == this.listViewErrorList) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuButtonItemScroll, EventArgs.Empty });
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void ToolBar_ButtonClick(object sender, ToolBarItemEventArgs e) {
            try {
                if (e.Item == this.buttonItemErrors){
                    this.RefreshErrorList();
                }else if( e.Item == this.buttonItemWarnings) {
                    this.RefreshErrorList();
                }
                else if (e.Item == this.dropDownMenuItemValidator) {
                    this.dropDownMenuItemValidator.Show();
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_BeforePopup(object sender, TD.SandBar.MenuPopupEventArgs e) {
            try {
                if (sender == this.dropDownMenuItemValidator) {
                    // Get Validator
                    this.menuButtonItemPGdb.Checked = (WorkspaceValidator.Default.Validator is PersonalGeodatabaseValidator);
                    this.menuButtonItemFGdb.Checked = (WorkspaceValidator.Default.Validator is FileGeodatabaseValidator);

                    // Clear SDE Connection Items
                    this.menuButtonItemSdeConnection.Items.Clear();

                    // Find ArcCatalog SDE Connections
                    string applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string esri = System.IO.Path.Combine(applicationData, "ESRI");
                    string arcCatalog = System.IO.Path.Combine(esri, "ArcCatalog");
                    if (!Directory.Exists(arcCatalog)) {
                        MenuButtonItem menuButton = new MenuButtonItem();
                        menuButton.Enabled = false;
                        menuButton.Text = Resources.TEXT_NOT_AVAILABLE_BR;
                        this.menuButtonItemSdeConnection.Items.Add(menuButton);
                        return;
                    }

                    // Get List of SDE Connections
                    string[] files = Directory.GetFiles(arcCatalog, "*.sde", SearchOption.TopDirectoryOnly);
                    if (files.Length == 0) {
                        MenuButtonItem menuButton = new MenuButtonItem();
                        menuButton.Enabled = false;
                        menuButton.Text = Resources.TEXT_NONE_BR;
                        this.menuButtonItemSdeConnection.Items.Add(menuButton);
                        return;
                    }

                    // Add an Item for each SDE Connection
                    foreach (string file in files) {
                        string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                        MenuButtonItem menuButton = new MenuButtonItem();
                        menuButton.Enabled = true;
                        menuButton.Text = filename;
                        menuButton.Tag = file;
                        menuButton.Activate += new EventHandler(this.MenuItem_Activate);
                        this.menuButtonItemSdeConnection.Items.Add(menuButton);
                    }
                }
                else if (sender == this.menuButtonItemSdeConnection) {
                    // Get SDE Validator
                    SdeValidator sdeValidator = WorkspaceValidator.Default.Validator as SdeValidator;
                    if (sdeValidator == null) { return; }

                    // Check if SDE Connection File match
                    foreach (MenuButtonItem item in this.menuButtonItemSdeConnection.Items) {
                        if (item.Tag == null) { continue; }
                        string itemTag = item.Tag.ToString();
                        item.Checked = (itemTag == sdeValidator.SdeFile);
                    }
                }
                else if (sender == this.contextMenuBarItemErrorList) {
                    this.menuButtonItemScroll.Enabled = (this.listViewErrorList.SelectedItems.Count == 1);
                    this.menuButtonItemSelect.Enabled = (this.listViewErrorList.SelectedItems.Count > 0);
                    this.menuButtonItemFlashError.Enabled = (this.listViewErrorList.SelectedItems.Count > 0);
                    this.menuButtonItemClearError.Enabled = (this.listViewErrorList.SelectedItems.Count > 0);
                    this.menuButtonItemClearAllErrors.Enabled = (this.listViewErrorList.Items.Count > 0);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                //
                // Error List Context Menu
                //
                if (sender == this.menuButtonItemScroll) {
                    // Can only zoom to one item
                    if (this.listViewErrorList.SelectedItems.Count != 1) { return; }

                    // Get EsriTable containing the error
                    ListViewItemError item = (ListViewItemError)this.listViewErrorList.SelectedItems[0];
                    EsriTable table = null;
                    if (item.Error is ErrorTable) {
                        ErrorTable errorTable = (ErrorTable)item.Error;
                        table = errorTable.Table;
                    }
                    else if (item.Error is ErrorTableRow) {
                        ErrorTableRow errorTableRow = (ErrorTableRow)item.Error;
                        EsriTableRow esriTableRow = (EsriTableRow)errorTableRow.TableRow;
                        table = (EsriTable)esriTableRow.Table;
                    }
                    else if (item.Error is ErrorObject) {
                        ErrorObject errorObject = (ErrorObject)item.Error;
                        table = errorObject.Table;
                    }
                    if (table == null) { return; }

                    // Get Containing Model
                    EsriModel model = (EsriModel)table.Container;

                    // Scroll to Table
                    model.ScrollToElement(table);

                    // Flash Table
                    table.Flash();
                }
                else if (sender == this.menuButtonItemSelect) {
                    // Can only zoom to one item
                    if (this.listViewErrorList.SelectedItems.Count == 0) { return; }

                    foreach (ListViewItemError item in this.listViewErrorList.SelectedItems) {
                        EsriModel model = null;
                        if (item.Error is ErrorTable) {
                            ErrorTable errorTable = (ErrorTable)item.Error;
                            EsriTable table = errorTable.Table;
                            model = (EsriModel)table.Container;
                        }
                        else if (item.Error is ErrorTableRow) {
                            ErrorTableRow errorTableRow = (ErrorTableRow)item.Error;
                            EsriTableRow esriTableRow = (EsriTableRow)errorTableRow.TableRow;
                            EsriTable table = (EsriTable)esriTableRow.Table;
                            model = (EsriModel)table.Container;
                        }
                        else if (item.Error is ErrorObject) {
                            ErrorObject errorObject = (ErrorObject)item.Error;
                            EsriTable table = errorObject.Table;
                            model = (EsriModel)table.Container;
                        }
                        if (model == null) { continue; }

                        // Clear Model Selection
                        model.SelectElements(false);
                    }
                    // 
                    foreach (ListViewItemError item in this.listViewErrorList.SelectedItems) {
                        // Get Table
                        EsriTable table = null;
                        if (item.Error is ErrorTable) {
                            ErrorTable errorTable = (ErrorTable)item.Error;
                            table = errorTable.Table;
                        }
                        else if (item.Error is ErrorTableRow) {
                            ErrorTableRow errorTableRow = (ErrorTableRow)item.Error;
                            EsriTableRow esriTableRow = (EsriTableRow)errorTableRow.TableRow;
                            table = (EsriTable)esriTableRow.Table;
                        }
                        else if (item.Error is ErrorObject) {
                            ErrorObject errorObject = (ErrorObject)item.Error;
                            table = errorObject.Table;
                        }
                        if (table == null) { continue; }

                        // Flash Table
                        table.Selected = true;
                    }
                }
                else if (sender == this.menuButtonItemFlashError) {
                    // Can only zoom to one item
                    if (this.listViewErrorList.SelectedItems.Count == 0) { return; }

                    // 
                    foreach (ListViewItemError item in this.listViewErrorList.SelectedItems) {
                        // Get Table
                        EsriTable table = null;
                        if (item.Error is ErrorTable) {
                            ErrorTable errorTable = (ErrorTable)item.Error;
                            table = errorTable.Table;
                        }
                        else if (item.Error is ErrorTableRow) {
                            ErrorTableRow errorTableRow = (ErrorTableRow)item.Error;
                            EsriTableRow esriTableRow = (EsriTableRow)errorTableRow.TableRow;
                            table = (EsriTable)esriTableRow.Table;
                        }
                        else if (item.Error is ErrorObject) {
                            ErrorObject errorObject = (ErrorObject)item.Error;
                            table = errorObject.Table;
                        }
                        if (table == null) { continue; }

                        // Flash Table
                        table.Flash();
                    }
 
                }
                else if (sender == this.menuButtonItemClearError) {
                    // Remove Selected Items
                    foreach (ListViewItemError item in this.listViewErrorList.SelectedItems) {
                        if (this.m_errors.Contains(item.Error)) {
                            this.m_errors.Remove(item.Error);
                        }
                    }

                    // Refresh Error List
                    this.RefreshErrorList();
                }
                else if (sender == menuButtonItemClearAllErrors) {
                    // Remove All Errors
                    this.m_errors.Clear();

                    // Refresh Error List
                    this.RefreshErrorList();
                }
                //
                // Validator Dropdown Menu
                //
                else if (sender == this.menuButtonItemPGdb) {
                    WorkspaceValidator.Default.Validator = new PersonalGeodatabaseValidator();
                }
                else if (sender == this.menuButtonItemFGdb) {
                    WorkspaceValidator.Default.Validator = new FileGeodatabaseValidator();
                }
                else if (sender == this.menuButtonItemSelectGeodatabase) {
                    // Create GxObjectFilter for GxDialog
                    IGxObjectFilter gxObjectFilter = new GxFilterWorkspacesClass();

                    // Create GxDialog
                    IGxDialog gxDialog = new GxDialogClass();
                    gxDialog.AllowMultiSelect = false;
                    gxDialog.ButtonCaption = Resources.TEXT_SELECT;
                    gxDialog.ObjectFilter = gxObjectFilter;
                    gxDialog.RememberLocation = true;
                    gxDialog.Title = Resources.TEXT_SELECT_EXISTING_GEODATABASE;

                    // Declare Enumerator to hold selected objects
                    IEnumGxObject enumGxObject = null;

                    // Open Dialog
                    if (!gxDialog.DoModalOpen(0, out enumGxObject)) { return; }
                    if (enumGxObject == null) { return; }

                    // Get Selected Object (if any)
                    IGxObject gxObject = enumGxObject.Next();
                    if (gxObject == null) { return; }
                    if (!gxObject.IsValid) { return; }

                    // Get GxDatabase
                    if (!(gxObject is IGxDatabase)) { return; }
                    IGxDatabase gxDatabase = (IGxDatabase)gxObject;

                    // Get Workspace
                    IWorkspace workspace = gxDatabase.Workspace;
                    if (workspace == null) { return; }

                    // Get Workspace Factory
                    IWorkspaceFactory workspaceFactory = workspace.WorkspaceFactory;
                    if (workspaceFactory == null) { return; }

                    // Get Workspace Factory ID
                    IUID uid = workspaceFactory.GetClassID();
                    string guid = uid.Value.ToString().ToUpper();

                    switch (guid) {
                        case EsriRegistry.GEODATABASE_PERSONAL:
                            WorkspaceValidator.Default.Validator = new PersonalGeodatabaseValidator(workspace);
                            break;
                        case EsriRegistry.GEODATABASE_FILE:
                            WorkspaceValidator.Default.Validator = new FileGeodatabaseValidator(workspace);
                            break;
                        case EsriRegistry.GEODATABASE_SDE:
                            WorkspaceValidator.Default.Validator = new SdeValidator(workspace);
                            break;
                        default:
                            break;
                    }
                }
                else if ((sender is MenuButtonItem) && (((MenuButtonItem)sender).Parent == this.menuButtonItemSdeConnection)) {
                    MenuButtonItem item = (MenuButtonItem)sender;
                    if (item.Tag == null) { return; }
                    WorkspaceValidator.Default.Validator = new SdeValidator(item.Tag.ToString());
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void RefreshErrorList() {
            // Clear Rows and Columns
            this.listViewErrorList.Clear();

            // Clear Groups
            this.listViewErrorList.Groups.Clear();

            // Begin Update
            this.listViewErrorList.BeginUpdate();

            // Create Groups
            ListViewGroup listViewGroupError = null;
            ListViewGroup listViewGroupWarning = null;

            // Add Groups
            if (this.buttonItemErrors.Checked) {
                listViewGroupError = new ListViewGroup(Resources.TEXT_ERRORS, HorizontalAlignment.Left);
                this.listViewErrorList.Groups.Add(listViewGroupError);
            }
            if (this.buttonItemWarnings.Checked) {
                listViewGroupWarning = new ListViewGroup(Resources.TEXT_WARNINGS, HorizontalAlignment.Left);
                this.listViewErrorList.Groups.Add(listViewGroupWarning);
            }

            // Counters
            int errors = 0;
            int warnings = 0;
            int counter = 0;

            // Add Columns
            ColumnHeader columnDescription = this.listViewErrorList.Columns.Add(Resources.TEXT_DESCRIPTION);

            // Get DiagramEnvironment Singleton
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;

            // Add Errors
            foreach (Error error in this.m_errors) {
                // Exit if Error Type is not selected. But increment counter.
                switch (error.ErrorType) {
                    case ErrorType.Error:
                        errors++;
                        if (!this.buttonItemErrors.Checked) { continue; }
                        break;
                    case ErrorType.Warning:
                        warnings++;
                        if (!this.buttonItemWarnings.Checked) { continue; }
                        break;
                    default:
                        continue;
                }

                // Increment Counter
                counter++;

                // Display Error Loading Message
                string message = string.Format("Loading Error {0} of {1}", counter.ToString(), this.m_errors.Count.ToString());
                diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                // Create  New ListViewItem
                ListViewItemError listViewItemError = new ListViewItemError();

                // Store Error
                listViewItemError.Error = error;

                // Set Item Text
                listViewItemError.Text = error.Description;

                // Set Item Tooltip
                listViewItemError.ToolTipText = error.Description;

                // Set ListViewItem Group
                switch (error.ErrorType) {
                    case ErrorType.Error:
                        listViewItemError.Group = listViewGroupError;
                        listViewItemError.ImageKey = ErrorListDiagram.ERRORS_ITEM;
                        break;
                    case ErrorType.Warning:
                        listViewItemError.Group = listViewGroupWarning;
                        listViewItemError.ImageKey = ErrorListDiagram.WARNINGS_ITEM;
                        break;
                }

                // Add to ListView
                this.listViewErrorList.Items.Add(listViewItemError);
            }

            // Update Error Count in Toolbar Button
            this.buttonItemErrors.Text = string.Format(Resources.TEXT_ERRORS_, errors.ToString());
            this.buttonItemWarnings.Text = string.Format(Resources.TEXT_WARNINGS_, warnings.ToString());

            // Adjust column widths to size of longest entry
            foreach (ColumnHeader column in this.listViewErrorList.Columns) {
                column.Width = -2;
            }

            // End Update
            this.listViewErrorList.EndUpdate();

            // Clear Messages
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(string.Empty));
        }
        //
        // PRIVATE CLASS
        //
        private class ListViewItemError : ListViewItem {
            private Error m_error = null;
            public Error Error {
                get { return this.m_error; }
                set { this.m_error = value; }
            }
        }
    }
}

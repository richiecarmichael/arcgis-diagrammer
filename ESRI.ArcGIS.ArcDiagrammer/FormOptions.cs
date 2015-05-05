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
using System.Configuration;
using System.Windows.Forms;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandBar;
using ESRI.ArcGIS.ArcDiagrammer.Properties;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class FormOptions : Form {
        private bool m_edited = false;
        private ColorSettings m_color = ColorSettings.Default;
        private CircularLayoutSettings m_circularLayout = CircularLayoutSettings.Default;
        private ForcedDirectLayoutSettings m_forcedDirectLayout = ForcedDirectLayoutSettings.Default;
        private HierarchicalLayoutSettings m_hierarchicalLayout = HierarchicalLayoutSettings.Default;
        private OrthogonalLayoutSettings m_orthogonalLayout = OrthogonalLayoutSettings.Default;
        private TreeLayoutSettings m_treeLayout = TreeLayoutSettings.Default;
        private DataReportSettings m_dataReport = DataReportSettings.Default;
        private SchemaReportSettings m_schemaReport = SchemaReportSettings.Default;
        private XmlReportSettings m_xmlReport = XmlReportSettings.Default;
        private ModelSettings m_model = ModelSettings.Default;
        private SettingsWindow m_window = SettingsWindow.Default;
        //
        // CONSTRUCTOR
        //
        public FormOptions() {
            InitializeComponent();

            // Icon
            this.Icon = Resources.DIAGRAMMER;

            // Title
            this.Text = Resources.TEXT_OPTIONS;

            // Menu
            this.menuButtonItemReset.Text = Resources.TEXT_RESET;
            this.menuButtonItemDescription.Text = Resources.TEXT_DESCRIPTION;

            // Buttons
            this.buttonApply.Text = Resources.TEXT_APPLY;
            this.buttonCancel.Text = Resources.TEXT_CANCEL;
            this.buttonReset.Text = Resources.TEXT_RESET;
            this.buttonResetAll.Text = Resources.TEXT_RESET_ALL;
            this.buttonOK.Text = Resources.TEXT_OK;

            // ToolTips
            this.toolTip1.SetToolTip(this.buttonReset, Resources.TEXT_RESET_CURRENT_TAB);
            this.toolTip1.SetToolTip(this.buttonResetAll, Resources.TEXT_RESET_ALL_TABS);

            // Update SandBar Color Scheme
            Office2007Renderer sandBarRenderer = (Office2007Renderer)this.sandBarManager1.Renderer;
            sandBarRenderer.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

            // Update StatusBar Color Scheme
            Office2007Renderer sandBarRenderer2 = (Office2007Renderer)this.statusBar1.Renderer;
            sandBarRenderer2.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

            // Get SandDock Color Scheme
            TD.SandDock.Rendering.Office2007ColorScheme colorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Black;
            switch (ColorSchemeSettings.Default.ColorScheme) {
                case Office2007ColorScheme.Black:
                    colorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Black;
                    break;
                case Office2007ColorScheme.Blue:
                    colorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Blue;
                    break;
                case Office2007ColorScheme.Silver:
                    colorScheme = TD.SandDock.Rendering.Office2007ColorScheme.Silver;
                    break;
            }

            // Update TabControl Color Scheme
            TD.SandDock.Rendering.Office2007Renderer renderer1 = (TD.SandDock.Rendering.Office2007Renderer)this.tabControlLayout.Renderer;
            TD.SandDock.Rendering.Office2007Renderer renderer2 = (TD.SandDock.Rendering.Office2007Renderer)this.tabControlOptions.Renderer;
            TD.SandDock.Rendering.Office2007Renderer renderer3 = (TD.SandDock.Rendering.Office2007Renderer)this.tabControlReport.Renderer;
            renderer1.ColorScheme = colorScheme;
            renderer2.ColorScheme = colorScheme;
            renderer3.ColorScheme = colorScheme;

            // Tabs
            this.tabPageDiagramColors.Text = Resources.TEXT_DIAGRAM_COLORS;
            this.tabPageLayout.Text = Resources.TEXT_LAYOUT;
            this.tabPageCircular.Text = Resources.TEXT_CIRCULAR;
            this.tabPageForcedDirect.Text = Resources.TEXT_FORCED_DIRECT;
            this.tabPageHierarchical.Text = Resources.TEXT_HIERARCHICAL;
            this.tabPageOrthogonal.Text = Resources.TEXT_ORTHOGONAL;
            this.tabPageTree.Text = Resources.TEXT_TREE;
            this.tabPageReport.Text = Resources.TEXT_REPORT;
            this.tabPageDataReport.Text = Resources.TEXT_DATA;
            this.tabPageSchemaReport.Text = Resources.TEXT_SCHEMA;
            this.tabPageXmlReport.Text = Resources.TEXT_XML;
            this.tabPageModel.Text = Resources.TEXT_DIAGRAM;
            this.tabPageWindow.Text = Resources.TEXT_WINDOW;

            // PropertyGrids
            this.propertyGridColors.SelectedObject = this.m_color;
            this.propertyGridCircular.SelectedObject = this.m_circularLayout;
            this.propertyGridForcedDirect.SelectedObject = this.m_forcedDirectLayout;
            this.propertyGridHierarchical.SelectedObject = this.m_hierarchicalLayout;
            this.propertyGridOrthogonal.SelectedObject = this.m_orthogonalLayout;
            this.propertyGridTree.SelectedObject = this.m_treeLayout;
            this.propertyGridDataReport.SelectedObject = this.m_dataReport;
            this.propertyGridSchemaReport.SelectedObject = this.m_schemaReport;
            this.propertyGridXmlReport.SelectedObject = this.m_xmlReport;
            this.propertyGridModel.SelectedObject = this.m_model;
            this.propertyGridWindow.SelectedObject = this.m_window;

            // Listen for changes in the property grid
            this.m_color.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_circularLayout.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_forcedDirectLayout.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_hierarchicalLayout.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_orthogonalLayout.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_treeLayout.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_dataReport.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_schemaReport.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_xmlReport.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_model.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
            this.m_window.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
        }
        //
        // PRIVATE METHODS
        //
        private void Diagrammer_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            this.m_edited = true;
        }
        private void Button_Click(object sender, EventArgs e) {
            try {
                if (sender == this.buttonApply) {
                    // Get Property Grids
                    List<PropertyGrid> propertyGrids = this.GetPropertyGrids(this.tabControlOptions);

                    // Loop through each Property Grid
                    foreach (PropertyGrid propertyGrid in propertyGrids) {
                        // Get Application Settings
                        ApplicationSettingsBase settings = (ApplicationSettingsBase)propertyGrid.SelectedObject;

                        // Reload Application Settings
                        settings.Save();

                        // Reload Application Settings
                        propertyGrid.Refresh();
                    }

                    // Disable Edited Button
                    this.m_edited = false;
                }
                else if (sender == this.buttonCancel) {
                    // Get Property Grids
                    List<PropertyGrid> propertyGrids = this.GetPropertyGrids(this.tabControlOptions);

                    // Reload
                    foreach (PropertyGrid propertyGrid in propertyGrids) {
                        // Get Application Settings
                        ApplicationSettingsBase settings = (ApplicationSettingsBase)propertyGrid.SelectedObject;

                        // Reload Application Settings
                        settings.Reload();
                    }

                    // Close
                    this.Close();
                }
                else if (sender == this.buttonReset) {
                    DialogResult dialogResult = MessageBox.Show(
                        Resources.TEXT_RESET_CURRENT_TAB_WARNING,
                        Resources.TEXT_ARCDIAGRAMMER,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3,
                        MessageBoxOptions.DefaultDesktopOnly);
                    switch (dialogResult) {
                        case DialogResult.Cancel:
                        case DialogResult.No:
                            break;
                        case DialogResult.Yes:
                            // Find Current Tab
                            PropertyGrid propertyGrid = this.SelectedPropertGrid(this.tabControlOptions);
                            if (propertyGrid == null) { return; }
                            if (propertyGrid.SelectedObject == null) { return; }
                            if (!(propertyGrid.SelectedObject is ApplicationSettingsBase)) { return; }

                            // Get Application Settings
                            ApplicationSettingsBase settings = (ApplicationSettingsBase)propertyGrid.SelectedObject;

                            // Reset Application Settings
                            settings.Reset();

                            // Refresh Application Settings
                            propertyGrid.Refresh();

                            break;
                    }
                }
                else if (sender == this.buttonResetAll) {
                    DialogResult dialogResult = MessageBox.Show(
                        Resources.TEXT_RESET_ALL_TABS_WARNING,
                        Resources.TEXT_ARCDIAGRAMMER,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3,
                        MessageBoxOptions.DefaultDesktopOnly);
                    switch (dialogResult) {
                        case DialogResult.Cancel:
                        case DialogResult.No:
                            break;
                        case DialogResult.Yes:
                            // Get Property Grids
                            List<PropertyGrid> propertyGrids = this.GetPropertyGrids(this.tabControlOptions);

                            // Loop for each Grid
                            foreach (PropertyGrid propertyGrid in propertyGrids) {
                                // Get Application Settings
                                ApplicationSettingsBase settings = (ApplicationSettingsBase)propertyGrid.SelectedObject;

                                // Reset Application Settings
                                settings.Reset();

                                // Refresh Application Settings
                                propertyGrid.Refresh();
                            }

                            // Disable Edited Button
                            this.m_edited = false;
                            break;
                    }
                }
                else if (sender == this.buttonOK) {
                    // Get Property Grids
                    List<PropertyGrid> propertyGrids = this.GetPropertyGrids(this.tabControlOptions);

                    // Loop for each Grid
                    foreach (PropertyGrid propertyGrid in propertyGrids) {
                        // Get Application Settings
                        ApplicationSettingsBase settings = (ApplicationSettingsBase)propertyGrid.SelectedObject;

                        // Save Application Settings
                        settings.Save();
                    }

                    //Close
                    this.Close();
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Timer_Tick(object sender, EventArgs e) {
            try {
                if (this.IsDisposed) { return; }
                if (this.Disposing) { return; }
                if (!(this.Visible)) { return; }
                if (this.WindowState == FormWindowState.Minimized) { return; }

                this.buttonApply.Enabled = this.m_edited;
                this.buttonCancel.Enabled = true;
                this.buttonReset.Enabled = true;
                this.buttonOK.Enabled = true;
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_BeforePopup(object sender, MenuPopupEventArgs e) {
            try {
                if (sender == this.contextMenuBarItemGrid) {
                    // Get Selected Property Grid
                    PropertyGrid propertyGrid = null;
                    if (e.Control != null && e.Control is PropertyGrid) { 
                        propertyGrid = (PropertyGrid)e.Control;
                    }

                    // Enable/Check Menu Items
                    this.menuButtonItemReset.Enabled = propertyGrid != null && propertyGrid.SelectedGridItem != null;
                    this.menuButtonItemDescription.Enabled = propertyGrid != null;
                    this.menuButtonItemDescription.Checked = propertyGrid != null && propertyGrid.HelpVisible;

                    // Store Reference of Selected PropertyGrid
                    this.menuButtonItemReset.Tag = propertyGrid;
                    this.menuButtonItemDescription.Tag = propertyGrid;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                if (sender == this.menuButtonItemReset) {
                    if (this.menuButtonItemReset.Tag != null) {
                        if (this.menuButtonItemReset.Tag is PropertyGrid) {
                            PropertyGrid propertyGrid = (PropertyGrid)this.menuButtonItemReset.Tag;
                            propertyGrid.ResetSelectedProperty();
                        }
                    }
                }
                else if (sender == this.menuButtonItemDescription) {
                    if (this.menuButtonItemDescription.Tag != null) {
                        if (this.menuButtonItemDescription.Tag is PropertyGrid) {
                            PropertyGrid propertyGrid = (PropertyGrid)this.menuButtonItemDescription.Tag;
                            propertyGrid.HelpVisible = !(propertyGrid.HelpVisible);
                        }
                    }
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private PropertyGrid SelectedPropertGrid(TD.SandDock.TabControl tabControl){
            TD.SandDock.TabPage tabPage = tabControl.SelectedPage;
            if (tabPage == null){return null;}
            if (tabPage.Controls.Count == 0){return null;}

            if (tabPage.Controls[0] is TD.SandDock.TabControl) {
                return this.SelectedPropertGrid((TD.SandDock.TabControl)tabPage.Controls[0]);
            }

            if (tabPage.Controls[0] is PropertyGrid) {
                return (PropertyGrid)tabPage.Controls[0];
            }

            return null;
        }
        private List<PropertyGrid> GetPropertyGrids(TD.SandDock.TabControl tabControl) {
            List<PropertyGrid> propertyGrids = new List<PropertyGrid>();
            foreach (TD.SandDock.TabPage tabPage in tabControl.TabPages) {
                if (tabPage.Controls.Count == 0) { continue; }
                if (tabPage.Controls[0] is TD.SandDock.TabControl) {
                    TD.SandDock.TabControl tabControl2 = (TD.SandDock.TabControl)tabPage.Controls[0];
                    List<PropertyGrid> propertyGrids2 = this.GetPropertyGrids(tabControl2);
                    foreach (PropertyGrid propertyGrid2 in propertyGrids2) {
                        propertyGrids.Add(propertyGrid2);
                    }
                }
                else if (tabPage.Controls[0] is PropertyGrid){
                    propertyGrids.Add((PropertyGrid)tabPage.Controls[0]);
                }
            }
            return propertyGrids;
        }
    }
}
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
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandBar;
using ESRI.ArcGIS.ArcDiagrammer.Properties;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class FormPrinterSetup : Form {
        private bool m_edited = false;
        private DiagramPrinterSettings m_printer = DiagramPrinterSettings.Default;
        //
        // CONSTRUCTOR
        //
        public FormPrinterSetup() {
            InitializeComponent();

            //
            this.Icon = Resources.DIAGRAMMER;
            this.Text = Resources.TEXT_PRINTER_SETUP;

            // Menu
            this.menuButtonItemReset.Text = Resources.TEXT_RESET;
            this.menuButtonItemDescription.Text = Resources.TEXT_DESCRIPTION;

            // Buttons
            this.buttonApply.Text = Resources.TEXT_APPLY;
            this.buttonCancel.Text = Resources.TEXT_CANCEL;
            this.buttonReset.Text = Resources.TEXT_RESET;
            this.buttonOK.Text = Resources.TEXT_OK;

            // ToolTips
            this.toolTip1.SetToolTip(this.buttonReset, Resources.TEXT_RESET_CURRENT_TAB);

            // Update SandBar Color Scheme
            Office2007Renderer sandBarRenderer = (Office2007Renderer)this.sandBarManager1.Renderer;
            sandBarRenderer.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

            // Update StatusBar Color Scheme
            Office2007Renderer sandBarRenderer2 = (Office2007Renderer)this.statusBar1.Renderer;
            sandBarRenderer2.ColorScheme = ColorSchemeSettings.Default.ColorScheme;

            // PropertyGrids
            this.propertyPrinter.SelectedObject = this.m_printer;

            // Listen for changes in the property grid
            this.m_printer.PropertyChanged += new PropertyChangedEventHandler(this.Diagrammer_PropertyChanged);
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
                    this.m_printer.Save();
                    this.propertyPrinter.Refresh();
                    this.m_edited = false;
                }
                else if (sender == this.buttonCancel) {
                    this.m_printer.Reload();
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
                        case DialogResult.Yes:
                            this.m_printer.Reset();
                            this.propertyPrinter.Refresh();
                            break;
                        case DialogResult.Cancel:
                        case DialogResult.No:
                        default:
                            break;
                    }
                }
                else if (sender == this.buttonOK) {
                    this.m_printer.Save();
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
                    // Enable/Check Menu Items
                    this.menuButtonItemReset.Enabled = this.propertyPrinter.SelectedGridItem != null;
                    this.menuButtonItemDescription.Enabled = true;
                    this.menuButtonItemDescription.Checked = this.propertyPrinter.HelpVisible;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                if (sender == this.menuButtonItemReset) {
                    this.propertyPrinter.ResetSelectedProperty();
                }
                else if (sender == this.menuButtonItemDescription) {
                    this.propertyPrinter.HelpVisible = !(this.propertyPrinter.HelpVisible);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
    }
}
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
using System.Windows.Forms;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandDock;
using ESRI.ArcGIS.ArcDiagrammer.Properties;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class FormWindows : Form {
        private readonly SandDockManager m_sanDockManager = null;
        //
        // CONSTRUCTOR
        //
        public FormWindows(SandDockManager sandDockManager) {
            InitializeComponent();

            //
            this.Icon = Resources.DIAGRAMMER;
            this.Text = Resources.TEXT_WINDOWS;

            this.columnHeaderName.Text = Resources.TEXT_NAME;
            this.columnHeaderPath.Text = Resources.TEXT_PATH;
            this.buttonActivate.Text = Resources.TEXT_ACTIVATE;
            this.buttonClose.Text = Resources.TEXT_CLOSE;
            this.buttonOK.Text = Resources.TEXT_OK;

            //
            this.m_sanDockManager = sandDockManager;

            //
            this.LoadDockControls();
        }
        //
        // PRIVATE METHODS
        //
        private void Timer_Tick(object sender, EventArgs e) {
            try {
                if (this.IsDisposed) { return; }
                if (this.Disposing) { return; }
                if (!(this.Visible)) { return; }
                if (this.WindowState == FormWindowState.Minimized) { return; }

                // Get Count
                int count = this.listViewWindows.SelectedItems.Count;

                // Activate Button
                if (this.buttonActivate.Enabled != (count == 1)) {
                    this.buttonActivate.Enabled = (count == 1);
                }

                // Close Button
                if (this.buttonClose.Enabled != (count > 0)) {
                    this.buttonClose.Enabled = (count > 0);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Activate_Click(object sender, EventArgs e) {
            try{
                if (sender == this.buttonActivate) {
                    if (this.listViewWindows.SelectedItems.Count != 1) { return; }

                    ListViewItemTabbedDocument item = (ListViewItemTabbedDocument)this.listViewWindows.SelectedItems[0];
                    item.TabbedDocument.Activate();

                    //
                    this.LoadDockControls();
                }
                else if (sender == this.buttonClose) {
                    foreach (ListViewItemTabbedDocument item in this.listViewWindows.SelectedItems) {
                        if (item.TabbedDocument.AllowClose) {
                            item.TabbedDocument.Close();
                        }
                    }

                    //
                    this.LoadDockControls();
                }
                else if (sender == this.buttonOK) {
                    this.Close();
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void LoadDockControls() {
            // Start Updated. Clear All Items
            this.listViewWindows.BeginUpdate();
            this.listViewWindows.Items.Clear();

            // Add All Tabs
            foreach (TabbedDocument tabbedDocument in this.m_sanDockManager.GetDockControls(DockSituation.Document)) {
                ListViewItemTabbedDocument item = new ListViewItemTabbedDocument(tabbedDocument);

                this.listViewWindows.Items.Add(item);
            }

            // End Update
            this.listViewWindows.EndUpdate();
        }
        //
        // PRIVATE CLASS
        //
        private class ListViewItemTabbedDocument : ListViewItem {
            private readonly TabbedDocument m_tabbedDocument = null;
            public ListViewItemTabbedDocument(TabbedDocument tabbedDocument) : base() {
                this.m_tabbedDocument = tabbedDocument;
                this.Text = this.m_tabbedDocument.TabText;
                this.UseItemStyleForSubItems = false;
            }
            public TabbedDocument TabbedDocument {
                get { return this.m_tabbedDocument; }
            }
        }
    }
}
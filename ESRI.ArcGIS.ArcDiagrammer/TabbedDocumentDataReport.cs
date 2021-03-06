/*=============================================================================
 * 
 * Copyright � 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished�all rights reserved.
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
using ESRI.ArcGIS.ArcDiagrammer.Properties;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandDock;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class TabbedDocumentDataReport : UserTabbedDocument, ITabPrinter, ITabReport {
        private DataReport _dataReport = null;
        //
        // CONSTRUCTOR
        //
        public TabbedDocumentDataReport() {
            InitializeComponent(); 

            //
            this._dataReport = new DataReport();
            this._dataReport.Invalidated += new EventHandler<EventArgs>(this.Report_Invalidated);

            //
            this.TabImage = Resources.BITMAP_DATA_REPORT;
            this.Text = "Data Report";
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public Report Report {
            get { return this._dataReport; }
        }
        //
        // PRIVATE METHODS
        //
        private void Report_Invalidated(object sender, EventArgs e) {
            this.LoadReport();
        }
        private void LoadReport() {
            try {
                // Create Report
                this._dataReport.Export();
                if (string.IsNullOrEmpty(this._dataReport.Html)) { return; }

                // Update Web Browser Contents
                if (this.webBrowser1 == null) { return; }
                if (this.webBrowser1.IsDisposed) { return; }
                if (this.webBrowser1.Disposing) { return; }
                 this.webBrowser1.Navigate(this._dataReport.Html);
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        public void Print() {
            this.webBrowser1.ShowPrintDialog();
        }
        public void PrintPreview() {
            this.webBrowser1.ShowPrintPreviewDialog();
        }
        public void SaveAs() {
            this.webBrowser1.ShowSaveAsDialog();
        }
        public void PageSetup() {
            this.webBrowser1.ShowPageSetupDialog();
        }
    }
}

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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using ESRI.ArcGIS.ArcDiagrammer.Properties;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using MSXML;
using TD.SandDock;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class TabbedDocumentMetadata : UserTabbedDocument, ITabPrinter {
        private Dataset _dataset = null;
        private string _stylesheet = string.Empty;
        //
        // CONSTRUCTOR
        public TabbedDocumentMetadata(Dataset dataset) {
            InitializeComponent();

            // Store Dataset
            this._dataset = dataset;

            // Update Tab Propeties
            this.TabImage = Resources.BITMAP_METADATA;
            this.Text = dataset.Name;

            // Load Metadata
            this.LoadMetadata();
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Metadata")]
        [DefaultValueAttribute("")]
        [Description("Metadata Stylesheet")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(MetadataStylesheetConverter))]
        public string Stylesheet {
            get { return this._stylesheet; }
            set { this._stylesheet = value; this.LoadMetadata(); }
        }
        //
        // PUBLIC METHODS
        //
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
        private void LoadMetadata() {
            try {
                string documentText = string.Empty;

                // Check if Stylesheet Exists
                //if (!File.Exists(this._stylesheet)) { return; }

                // Load XML
                DOMDocument source = new DOMDocumentClass();
                source.loadXML(this._dataset.Metadata);

                // Load XSL
                TextReader textReaderXsl = new StringReader(Resources.ISO);
                string xsl = textReaderXsl.ReadToEnd();
                textReaderXsl.Close();

                DOMDocumentClass style = new DOMDocumentClass();
                style.load(xsl);

                // Transform XML (to HTML)
                documentText = source.transformNode(style);

                //if (string.IsNullOrEmpty(this._stylesheet)) {
                //    // Load XML
                //    TextReader textReaderXml = new StringReader(this._dataset.Metadata);
                //    XPathDocument xml = new XPathDocument(textReaderXml);

                //    // Load XSL
                //    TextReader textReaderXsl = new StringReader(Resources.FILE_DEFAULT);
                //    XPathDocument xsl = new XPathDocument(textReaderXsl); ;

                //    // Compile Transformer
                //    XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                //    xslCompiledTransform.Load(xsl);

                //    // Transform XML (to HTML)
                //    StringBuilder stringBuilder = new StringBuilder();
                //    XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xslCompiledTransform.OutputSettings);
                //    xslCompiledTransform.Transform(xml, xmlWriter);
                //    documentText = stringBuilder.ToString();

                //    // Close Readers
                //    xmlWriter.Close();
                //    textReaderXsl.Close();
                //    textReaderXml.Close();
                //}
                //else {
                //    //// Check if Stylesheet Exists
                //    //if (!File.Exists(this._stylesheet)) { return; }

                //    //// Load XML
                //    //DOMDocument60Class source = new DOMDocument60Class();
                //    //source.loadXML(this._dataset.Metadata);

                //    //// Load XSL
                //    //DOMDocument60Class style = new DOMDocument60Class();
                //    //style.load(this._stylesheet);

                //    //// Transform XML (to HTML)
                //    //documentText = source.transformNode(style);

                //    // Load XML
                //    TextReader textReaderXml = new StringReader(this._dataset.Metadata);
                //    XPathDocument xml = new XPathDocument(textReaderXml);

                //    // Compile Transformer
                //    XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                //    xslCompiledTransform.Load(this._stylesheet);

                //    // Transform XML (to HTML)
                //    StringBuilder stringBuilder = new StringBuilder();
                //    XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xslCompiledTransform.OutputSettings);
                //    xslCompiledTransform.Transform(xml, xmlWriter);
                //    documentText = stringBuilder.ToString();

                //    // Close Readers
                //    xmlWriter.Close();
                //    textReaderXml.Close();
                //}

                // Assign Transfomed XML (HTML) to Web Browser
                this.webBrowser1.DocumentText = documentText;
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
    }
}

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
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public class XmlReport : Report {
        private string _workspaceDocument = null;
        private XmlWriter _writer = null;
        //
        // CONSTRUCTOR
        //
        public XmlReport() : base() {
            this.Xsl = Resources.FILE_XML_REPORT;
            SchemaReportSettings.Default.SettingsSaving += new SettingsSavingEventHandler(this.SettingsSaving);
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Xml Report")]
        [DefaultValue(null)]
        [Description("Any valid ESRI XML Workspace Document")]
        [DisplayName("XML Workspace Document")]
        [EditorAttribute(typeof(WorkspaceDocumentEditor), typeof(UITypeEditor))]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string WorkspaceDocument {
            get { return this._workspaceDocument; }
            set {
                // Set Source XML
                this._workspaceDocument = value;

                // Create Report
                this.CreateReport();
            }
        }
        // PROTECTED METHODS
        //
        protected override XsltArgumentList GetArgumentList() {
            XsltArgumentList argsList = base.GetArgumentList();

            argsList.AddParam("font", string.Empty, XmlReportSettings.Default.FontName);
            argsList.AddParam("backcolor", string.Empty, ColorTranslator.ToHtml(XmlReportSettings.Default.BackColor));
            argsList.AddParam("forecolor", string.Empty, ColorTranslator.ToHtml(XmlReportSettings.Default.ForeColor));
            argsList.AddParam("size1", string.Empty, XmlReportSettings.Default.Size1.ToString());
            argsList.AddParam("size2", string.Empty, XmlReportSettings.Default.Size2.ToString());
            argsList.AddParam("size3", string.Empty, XmlReportSettings.Default.Size3.ToString());
            argsList.AddParam("size4", string.Empty, XmlReportSettings.Default.Size4.ToString());
            argsList.AddParam("size5", string.Empty, XmlReportSettings.Default.Size5.ToString());

            return argsList;
        }
        //
        // PRIVATE METHODS
        //
        private void CreateReport() {
            // Exit if Workspace is NULL
            if (this._workspaceDocument == null) { return; }

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs("Creating Report"));

            // Get Temporary File
            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N").ToUpper() + ".xml");

            // Specific XML Settings
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.Default;
            writerSettings.Indent = false;
            writerSettings.NewLineHandling = NewLineHandling.Entitize;
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.NewLineOnAttributes = false;

            // Create the XmlWriter object and write some content.
            this._writer = XmlWriter.Create(filename, writerSettings);

            // <DataReport>
            this._writer.WriteStartElement("XmlReport");

            // Get ESRI Namespace
            XPathDocument document = new XPathDocument(this._workspaceDocument, XmlSpace.None);
            XPathNavigator navigator = document.CreateNavigator();
            bool ok = navigator.MoveToFirstChild();
            string esriNamespace = navigator.LookupNamespace("esri");

            // Open Geodatabase Schema Document
            XmlDocument schemaDocument = new XmlDocument();
            schemaDocument.LoadXml(Resources.FILE_GEODATABASE_EXCHANGE);

            // Add Namespaces to Schema Document
            XPathNavigator navigator2 = schemaDocument.CreateNavigator();
            bool ok2 = navigator2.MoveToFirstChild();
            XmlWriter writer = navigator2.CreateAttributes();
            writer.WriteAttributeString("xmlns", esriNamespace);
            writer.WriteAttributeString("targetNamespace", esriNamespace);
            writer.Close();

            // Load Schema Document
            StringReader stringReader = new StringReader(schemaDocument.OuterXml);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);

            // Create XML Reader Settings
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.Schemas.Add(null, xmlTextReader);
            readerSettings.Schemas.Compile();
            readerSettings.CheckCharacters = true;
            readerSettings.IgnoreComments = true;
            readerSettings.IgnoreWhitespace = true;
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.XmlResolver = new XmlUrlResolver();
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            readerSettings.ValidationEventHandler += new ValidationEventHandler(this.ValidationEventHandler);

            // Load Source XML
            XmlTextReader txtreader = new XmlTextReader(this._workspaceDocument);

            // Validate Against XSL
            XmlReader reader = XmlReader.Create(txtreader, readerSettings);
            while (reader.Read()) ;

            // Close Readers
            xmlTextReader.Close();
            stringReader.Close();
            txtreader.Close();
            reader.Close();

            // </DataReport>
            this._writer.WriteEndElement();

            // Close Writer
            this._writer.Close();

            // Set Source XML
            this.Xml = filename;

            // Fire Invalidate Event so that the Report Tabbed Document can Reload
            this.OnInvalidated(new EventArgs());

            // Clear Messages
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(string.Empty));
        }
        private void ValidationEventHandler(object sender, ValidationEventArgs e) {
            IXmlSchemaInfo schema = sender as IXmlSchemaInfo;
            IXmlLineInfo line = sender as IXmlLineInfo;
            IXmlNamespaceResolver name = sender as IXmlNamespaceResolver;
            if (line == null) { return; }

            // <Error>
            this._writer.WriteStartElement("Error");

            // <Severity></Severity>
            this._writer.WriteStartElement("Severity");
            this._writer.WriteValue(e.Severity.ToString());
            this._writer.WriteEndElement();

            // <Message></Message>
            this._writer.WriteStartElement("Message");
            this._writer.WriteValue(e.Message);
            this._writer.WriteEndElement();

            // <LineNumber></LineNumber>
            this._writer.WriteStartElement("LineNumber");
            this._writer.WriteValue(line.LineNumber.ToString());
            this._writer.WriteEndElement();

            // <LinePosition></LinePosition>
            this._writer.WriteStartElement("LinePosition");
            this._writer.WriteValue(line.LinePosition.ToString());
            this._writer.WriteEndElement();

            // </Error>
            this._writer.WriteEndElement();
        }
        private void SettingsSaving(object sender, CancelEventArgs e) {
            this.OnInvalidated(EventArgs.Empty);
        }
    }
}

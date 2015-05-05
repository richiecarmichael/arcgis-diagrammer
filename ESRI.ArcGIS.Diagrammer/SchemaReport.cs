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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public class SchemaReport : Report {
        private const string CATEGORY = "Schema Report";
        //
        // CONSTRUCTOR
        //
        public SchemaReport() : base() {
            SchemaReportSettings.Default.SettingsSaving += new SettingsSavingEventHandler(this.SettingsSaving);
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category(SchemaReport.CATEGORY)]
        [DefaultValue(null)]
        [Description("Any valid ESRI XML Workspace Document")]
        [DisplayName("XML Workspace Document")]
        [EditorAttribute(typeof(WorkspaceDocumentEditor), typeof(UITypeEditor))]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string WorkspaceDocument {
            get { return this.Xml; }
            set {
                // Set Source XML
                this.Xml = value;

                // Create Report
                this.CreateReport();
            }
        }
        //
        // PROTECTED METHODS
        //
        protected override XsltArgumentList GetArgumentList() {
            XsltArgumentList argsList = base.GetArgumentList();

            argsList.AddParam("font", string.Empty, SchemaReportSettings.Default.FontName);
            argsList.AddParam("backcolor", string.Empty, ColorTranslator.ToHtml(SchemaReportSettings.Default.BackColor));
            argsList.AddParam("forecolor", string.Empty, ColorTranslator.ToHtml(SchemaReportSettings.Default.ForeColor));
            argsList.AddParam("size1", string.Empty, SchemaReportSettings.Default.Size1.ToString());
            argsList.AddParam("size2", string.Empty, SchemaReportSettings.Default.Size2.ToString());
            argsList.AddParam("size3", string.Empty, SchemaReportSettings.Default.Size3.ToString());
            argsList.AddParam("size4", string.Empty, SchemaReportSettings.Default.Size4.ToString());
            argsList.AddParam("size5", string.Empty, SchemaReportSettings.Default.Size5.ToString());
            argsList.AddParam("file", string.Empty, this.Xml);

            return argsList;
        }
        //
        // PRIVATE METHODS
        //
        private void CreateReport() {
            if (string.IsNullOrEmpty(this.Xml)) { return; }

            // Get ESRI Namespace
            XPathDocument document = new XPathDocument(this.Xml, XmlSpace.None);
            XPathNavigator navigator = document.CreateNavigator();
            navigator.MoveToFirstChild();
            string esriNamespace = navigator.LookupNamespace("esri");

            // Open XSL
            XmlDocument schemaDocument = new XmlDocument();
            schemaDocument.LoadXml(Resources.FILE_SCHEMA_REPORT);

            // Add Namespaces to Schema Document
            XPathNavigator navigator2 = schemaDocument.CreateNavigator();
            bool ok = navigator2.MoveToFirstChild();
            navigator2.CreateAttribute("xmlns", "esri", null, esriNamespace);

            // Store XSL
            this.Xsl = schemaDocument.OuterXml;

            // Fire Invalidate Event so that the Report Tabbed Document can Reload
            this.OnInvalidated(new EventArgs());
        }
        private void SettingsSaving(object sender, CancelEventArgs e) {
            this.OnInvalidated(EventArgs.Empty);
        }
    }
}

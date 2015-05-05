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
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public abstract class Report {
        private string _xml = null;
        private string _xsl = null;
        private string _html = null;
        //
        // CONSTRUCTOR
        //
        public Report() { }
        //
        // EVENTS
        //
        public event EventHandler<EventArgs> Invalidated;
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public string Xml {
            get { return this._xml; }
            set { this._xml = value; }
        }
        [Browsable(false)]
        public string Xsl {
            get { return this._xsl; }
            set { this._xsl = value; }
        }
        [Browsable(false)]
        public string Html {
            get { return this._html; }
            set { this._html = value; }
        }
        //
        // PROTECTED METHODS
        // 
        protected virtual XsltArgumentList GetArgumentList() {
            XsltArgumentList argsList = new XsltArgumentList();

            argsList.AddParam("title", string.Empty, Resources.TEXT_REPORT_TITLE);
            argsList.AddParam("credit", string.Empty, Resources.TEXT_REPORT_CREDIT);
            argsList.AddParam("disclaimer", string.Empty, Resources.TEXT_REPORT_DISCLAIMER);
            argsList.AddParam("creationdate", string.Empty, DateTime.Now.ToLongDateString());
            argsList.AddParam("username", string.Empty, Environment.UserName);
            argsList.AddParam("userdomainname", string.Empty, Environment.UserDomainName);
            argsList.AddParam("machinename", string.Empty, Environment.MachineName);
            argsList.AddParam("osversion", string.Empty, Environment.OSVersion.ToString());
            argsList.AddParam("dotnetversion", string.Empty, Environment.Version.ToString());
            argsList.AddParam("assemblyversion", string.Empty, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            return argsList;
        }
        //
        // PUBLIC METHODS
        //
        public virtual void Export() {
            // XML
            if (string.IsNullOrEmpty(this._xml)) { return; }
            if (!File.Exists(this._xml)) { return; }
            XPathDocument xml = new XPathDocument(this._xml);

            // XSL
            if (string.IsNullOrEmpty(this._xsl)) { return; }
            TextReader textReaderXsl = new StringReader(this._xsl);
            XPathDocument xsl = new XPathDocument(textReaderXsl);

            // Compile Transformer
            XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
            xslCompiledTransform.Load(xsl);                     

            // Transform XML (to HTML)
            this._html = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N").ToUpper() + ".htm");
            XmlWriter xmlWriter = XmlWriter.Create(this._html, xslCompiledTransform.OutputSettings);
            xslCompiledTransform.Transform(xml, this.GetArgumentList(), xmlWriter);

            // Close Writers
            xmlWriter.Close();
            textReaderXsl.Close();
        }
        //
        // PROTECTED METHOD
        //
        protected virtual void OnInvalidated(EventArgs e) {
            EventHandler<EventArgs> handler = Invalidated;
            if (handler != null) {
                handler(this, e);
            }
        }
    }
}

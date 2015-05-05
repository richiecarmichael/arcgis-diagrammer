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
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Xsl;
using System.Diagnostics;

namespace ESRI.ArcGIS.ExceptionHandler {
    public sealed partial class FormException : Form {
        private WebBrowser m_webBrowser = null;
        private XmlDocument m_xmlDocument = null;
        private bool m_handlerEnabled = true;
        private string m_feedbackAddress = null;
        //
        // CONSTRUCTOR
        //
        public FormException() {
            //
            InitializeComponent();

            //
            this.Text = Properties.Resources.FORM_TEXT;

            //
            this.m_xmlDocument = new XmlDocument();
            XmlNode xmlNodeRoot = this.m_xmlDocument.AppendChild(this.m_xmlDocument.CreateElement("ROOT"));

            //
            this.Closing += new CancelEventHandler(this.Form_Closing);

            //
            this.timer1.Interval = 500;
            this.timer1.Start();
        }
        //
        // EVENTS
        //
        public event EventHandler<ExceptionHandlerEventArgs> Exception;
        //
        // PROPERTY
        //
        private static FormException formException;
        public string FeedbackAddress {
            get { return this.m_feedbackAddress; }
            set { this.m_feedbackAddress = value; }
        }
        //
        // PUBLIC METHODS
        //
        public static FormException GetInstance() {
            if (formException == null) {
                formException = new FormException();
                formException.CreateHandle();
            }
            return formException;
        }
        public static void HandleException(Exception exception) {
            // Exit if Exception is NULL
            if (exception == null) { return; }

            // Get New of Existing Instance of Exception Window
            FormException formException = FormException.GetInstance();

            // Get Calling Assembly
            Assembly assembly = Assembly.GetCallingAssembly();

            // Add Exception to Webbrowser
            formException.AddException(null, new ExceptionHandlerEventArgs(exception, assembly));
        }
        /// <summary>
        /// The exception that is to be displayed.
        /// </summary>
        public void AddException(object sender, ExceptionHandlerEventArgs e) {
            if (this.InvokeRequired) {
                this.Invoke(new EventHandler<ExceptionHandlerEventArgs>(this.AddException), new object[] { sender, e });
            }
            else {
                // Re-enable handler if windows is not currently visible
                if (!this.m_handlerEnabled && !this.Visible) {
                    this.m_handlerEnabled = true;
                }

                // Exit if handler is disabled
                if (!this.m_handlerEnabled) { return; }

                //
                XmlNode xmlNodeRoot = this.m_xmlDocument.DocumentElement;
                XmlNode xmlNodeError = xmlNodeRoot.AppendChild(this.m_xmlDocument.CreateElement("EXCEPTION"));
                XmlNode xmlNodeDate = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("DATE"));
                XmlNode xmlNodeAssembly = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("ASSEMBLY"));
                XmlNode xmlNodeMessage = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("MESSAGE"));
                XmlNode xmlNodeStack = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("STACK"));

                //
                xmlNodeDate.InnerText = DateTime.Now.ToString();
                xmlNodeAssembly.InnerText = e.Assembly.FullName;
                xmlNodeMessage.InnerText = e.Exception.Message;
                xmlNodeStack.InnerText = e.Exception.StackTrace;

                //
                this.UpdateExceptionHandler();

                // Raise Exception Event
                if (Exception != null) {
                    this.Exception(this, e);
                }
            }
        }
        //
        // PRIVATE METHODS
        //
        private void Form_Closing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }
        private void Form_VisibleChanged(object sender, EventArgs e) {
            this.UpdateExceptionHandler();
        }
        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem == this.toolStripButtonEnabled) {
                this.m_handlerEnabled = !(this.m_handlerEnabled);
            }
            else if (e.ClickedItem == this.toolStripButtonFeedback) {
                if (this.m_feedbackAddress != null) {
                    Process process = new Process();
                    process.StartInfo.FileName = this.m_feedbackAddress;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.CreateNoWindow = false;
                    process.Start();
                }
            }
            else if (e.ClickedItem == this.toolStripButtonCopy) {
                if (this.m_webBrowser != null &&
                    this.m_webBrowser.Document != null &&
                    this.m_webBrowser.Document.Body != null &&
                    this.m_webBrowser.Document.Body.OuterHtml != null) {
                    Clipboard.SetData(DataFormats.Html, this.m_webBrowser.Document.Body.OuterHtml);
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e) {
            // Quit if Messenger Window is not visible
            if (this.IsDisposed) { return; }
            if (this.Disposing) { return; }
            if (!this.Visible) { return; }

            // Enable Handled Button
            if (this.toolStripButtonEnabled.Checked != this.m_handlerEnabled) {
                this.toolStripButtonEnabled.Checked = this.m_handlerEnabled;
            }

            // Enable Feedback Button
            bool boolFeedback = (this.m_feedbackAddress != null);
            if (this.toolStripButtonFeedback.Enabled != boolFeedback) {
                this.toolStripButtonFeedback.Enabled = boolFeedback;
            }
        }
        private void UpdateExceptionHandler() {
            if (this.Visible) {
                if (this.m_webBrowser == null) {
                    this.m_webBrowser = new WebBrowser();
                    this.m_webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.toolStripContainer1.ContentPanel.Controls.Add(this.m_webBrowser);
                }

                // XML
                TextReader textReaderXml = new StringReader(this.m_xmlDocument.OuterXml);
                XPathDocument xml = new XPathDocument(textReaderXml);

                // XSL
                TextReader textReaderXsl = new StringReader(Properties.Resources.FILE_HTMLREPORT);
                XPathDocument xsl = new XPathDocument(textReaderXsl);

                string OSVersion = Environment.OSVersion.VersionString;
                string CLRVersion = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;

                //
                XsltArgumentList argsList = new XsltArgumentList();
                argsList.AddParam("title", "", Properties.Resources.HTML_TITLE);
                argsList.AddParam("environment", "", Properties.Resources.HTML_ENVIRONMENT);
                argsList.AddParam("os", "", Properties.Resources.HTML_OS);
                argsList.AddParam("osvalue", "", OSVersion);
                argsList.AddParam("clr", "", Properties.Resources.HTML_CLR);
                argsList.AddParam("clrvalue", "", CLRVersion);
                argsList.AddParam("exceptions", "", Properties.Resources.HTML_EXCEPTIONS);
                argsList.AddParam("date", "", Properties.Resources.HTML_DATE);
                argsList.AddParam("assembly", "", Properties.Resources.HTML_ASSEMBLY);
                argsList.AddParam("message", "", Properties.Resources.HTML_MESSAGE);
                argsList.AddParam("stack", "", Properties.Resources.HTML_STACK);

                //
                XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                xslCompiledTransform.Load(xsl);

                // Create the writer.
                StringBuilder stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xslCompiledTransform.OutputSettings);
                xslCompiledTransform.Transform(xml, argsList, xmlWriter);
                string documentText = stringBuilder.ToString();

                //
                xmlWriter.Close();
                textReaderXsl.Close();
                textReaderXml.Close();

                //
                this.m_webBrowser.DocumentText = documentText;
            }
        }
    }
}
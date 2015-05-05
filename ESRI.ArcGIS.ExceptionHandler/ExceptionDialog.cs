/*=============================================================================
 * 
 * Copyright © 2008 ESRI. All rights reserved. 
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using ESRI.ArcGIS.ExceptionHandler.Properties;
using TD.SandBar;

namespace ESRI.ArcGIS.ExceptionHandler {
    public partial class ExceptionDialog : Form {
        private WebBrowser m_webBrowser = null;
        private XmlDocument m_xmlDocument = null;
        private bool m_enabled = true;
        private string m_feedbackAddress = string.Empty;
        private delegate void ExceptionDialogHandler(Exception exception, Assembly assembly);
        //
        // CONSTRUCTOR
        //
        public ExceptionDialog() {
            //
            InitializeComponent();

            // Update Window and Statusbar Text
            this.Text = Resources.TEXT_EXCEPTION_HANDLER;
            this.statusBarItemMain.Text = Resources.TEXT_READY;

            // Update Menu Text
            this.menuBarItemFile.Text = Resources.TEXT_FILE;
            this.menuButtonItemExit.Text = Resources.TEXT_EXIT;

            // Update Toolbar Button Text
            this.buttonItemNew.Text = string.Empty;
            this.buttonItemSave.Text = string.Empty;
            this.buttonItemMessage.Text = string.Empty;
            this.buttonItemPrint.Text = string.Empty;
            this.buttonItemPrintPreview.Text = string.Empty;
            this.buttonItemPageSetup.Text = string.Empty;
            this.buttonItemCopy.Text = string.Empty;
            this.buttonItemPlay.Text = string.Empty;
            this.buttonItemPause.Text = string.Empty;
            
            // Update Toolbar Button Tooltip
            this.buttonItemNew.ToolTipText = "Clear Exception Text";
            this.buttonItemSave.ToolTipText = "Save Exception to a File";
            this.buttonItemMessage.ToolTipText = "Send Feedback to Application Author";
            this.buttonItemPrint.ToolTipText = "Displays a print dialog";
            this.buttonItemPrintPreview.ToolTipText = "Displays a print preview dialog";
            this.buttonItemPageSetup.ToolTipText = "Displays a page setup dialog";
            this.buttonItemCopy.ToolTipText = "Copy the Exception Text to the Clipboard";
            this.buttonItemPlay.ToolTipText = "Enable Exception Handling";
            this.buttonItemPause.ToolTipText = "Pause Exception Handling";

            // Update Toolbar Button Images
            this.buttonItemNew.Image = Resources.BITMAP_NEW;
            this.buttonItemSave.Image = Resources.BITMAP_SAVE;
            this.buttonItemMessage.Image = Resources.BITMAP_MESSAGE;
            this.buttonItemPrint.Image = Resources.BITMAP_PRINT;
            this.buttonItemPrintPreview.Image = Resources.BITMAP_PRINT_PREVIEW;
            this.buttonItemPageSetup.Image = Resources.BITMAP_PRINT_SETUP;
            this.buttonItemCopy.Image = Resources.BITMAP_COPY;
            this.buttonItemPlay.Image = Resources.BITMAP_PLAY;
            this.buttonItemPause.Image = Resources.BITMAP_PAUSE;

            // Create New XML Document
            this.m_xmlDocument = new XmlDocument();
            XmlNode xmlNodeRoot = this.m_xmlDocument.AppendChild(this.m_xmlDocument.CreateElement("ROOT"));

            // Listen to the Form Closing Event
            this.Closing += new CancelEventHandler(this.Form_Closing);
        }
        //
        // EVENTS
        //
        public event EventHandler<ExceptionHandlerEventArgs> Exception;
        //
        // PROPERTY
        //
        private static ExceptionDialog defaultInstance = new ExceptionDialog();
        public static ExceptionDialog Default {
            get { return defaultInstance; }
        }
        public string FeedbackAddress {
            get { return this.m_feedbackAddress; }
            set { this.m_feedbackAddress = value; }
        }
        //
        // PROTECTED METHODS
        //
        protected virtual void OnException(ExceptionHandlerEventArgs e) {
            EventHandler<ExceptionHandlerEventArgs> handler = Exception;
            if (handler != null) {
                handler(this, e);
            }
        }
        //
        // PUBLIC METHODS
        //
        public static void HandleException(Exception exception) {
            // Exit if Exception is NULL
            if (exception == null) { return; }

            // Get New of Existing Instance of Exception Window
            ExceptionDialog formException = ExceptionDialog.Default;

            // Get Calling Assembly
            Assembly assembly = Assembly.GetCallingAssembly();

            // Add Exception to Webbrowser
            formException.AddException(exception, assembly);
        }
        /// <summary>
        /// The exception that is to be displayed.
        /// </summary>
        public void AddException(Exception exception, Assembly assembly) {
            if (this.InvokeRequired) {
                this.Invoke(new ExceptionDialogHandler(this.AddException), new object[] { exception, assembly });
            }
            else {
                // Re-enable handler if windows is not currently visible
                if (!this.m_enabled && !this.Visible) {
                    this.m_enabled = true;
                }

                // Exit if handler is disabled
                if (!this.m_enabled) { return; }

                // Add Exception
                this.AppendException(exception, assembly);

                // Update Exception Handler
                this.UpdateExceptionHandler();

                // Raise Exception Event
                this.OnException(new ExceptionHandlerEventArgs(exception, assembly));
            }
        }
        private void AppendException(Exception exception, Assembly assembly) {
            //
            XmlNode xmlNodeRoot = this.m_xmlDocument.DocumentElement;
            XmlNode xmlNodeError = xmlNodeRoot.AppendChild(this.m_xmlDocument.CreateElement("EXCEPTION"));
            XmlNode xmlNodeDate = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("DATE"));
            XmlNode xmlNodeAssembly = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("ASSEMBLY"));
            XmlNode xmlNodeMessage = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("MESSAGE"));
            XmlNode xmlNodeStack = xmlNodeError.AppendChild(this.m_xmlDocument.CreateElement("STACK"));

            string date = DateTime.Now.ToString();
            string ass = assembly.FullName.ToString();
            string message = exception.Message;
            COMException com = exception as COMException;
            if (com != null) {
                message += string.Format(" HRESULT[{0}]", com.ErrorCode.ToString());
            }
            string stack = exception.StackTrace;

            //
            xmlNodeDate.InnerText = date;
            xmlNodeAssembly.InnerText = ass;
            xmlNodeMessage.InnerText = message;
            xmlNodeStack.InnerText = stack;

            // 
            Exception inner = exception.InnerException;
            if (inner != null) {
                this.AppendException(inner, assembly);
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
        private void Timer_Tick(object sender, EventArgs e) {
            if (this.IsDisposed) { return; }
            if (this.Disposing) { return; }
            if (!this.Visible) { return; }

            // Play
            if (this.buttonItemPlay.Checked != this.m_enabled) {
                this.buttonItemPlay.Checked = this.m_enabled;
            }

            // Pause
            if (this.buttonItemPause.Checked != !this.m_enabled) {
                this.buttonItemPause.Checked = !this.m_enabled;
            }

            // Enable Feedback Button
            if (this.buttonItemMessage.Enabled != !string.IsNullOrEmpty(this.m_feedbackAddress)) {
                this.buttonItemMessage.Enabled = !string.IsNullOrEmpty(this.m_feedbackAddress);
            }
        }
        private void UpdateExceptionHandler() {
            if (this.Visible) {
                if (this.m_webBrowser == null) {
                    this.m_webBrowser = new WebBrowser();
                    this.m_webBrowser.Dock = DockStyle.Fill;
                    this.Controls.Add(this.m_webBrowser);
                    this.Controls.SetChildIndex(this.m_webBrowser, 0);
                }

                // XML
                TextReader textReaderXml = new StringReader(this.m_xmlDocument.OuterXml);
                XPathDocument xml = new XPathDocument(textReaderXml);

                // XSL
                TextReader textReaderXsl = new StringReader(Resources.FILE_HTMLREPORT);
                XPathDocument xsl = new XPathDocument(textReaderXsl);

                string OSVersion = Environment.OSVersion.VersionString;
                string CLRVersion = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;

                //
                XsltArgumentList argsList = new XsltArgumentList();
                argsList.AddParam("title", string.Empty, Resources.HTML_TITLE);
                argsList.AddParam("environment", string.Empty, Resources.HTML_ENVIRONMENT);
                argsList.AddParam("os", string.Empty, Resources.HTML_OS);
                argsList.AddParam("osvalue", string.Empty, OSVersion);
                argsList.AddParam("clr", string.Empty, Resources.HTML_CLR);
                argsList.AddParam("clrvalue", string.Empty, CLRVersion);
                argsList.AddParam("exceptions", string.Empty, Resources.HTML_EXCEPTIONS);
                argsList.AddParam("date", string.Empty, Resources.HTML_DATE);
                argsList.AddParam("assembly", string.Empty, Resources.HTML_ASSEMBLY);
                argsList.AddParam("message", string.Empty, Resources.HTML_MESSAGE);
                argsList.AddParam("stack", string.Empty, Resources.HTML_STACK);

                //
                XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                xslCompiledTransform.Load(xsl);

                // Create the writer.
                StringBuilder stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xslCompiledTransform.OutputSettings);
                xslCompiledTransform.Transform(xml, argsList, xmlWriter);
                string documentText = stringBuilder.ToString();

                // Close Readers and Writers
                xmlWriter.Close();
                textReaderXsl.Close();
                textReaderXml.Close();

                // Add text to browser
                this.m_webBrowser.DocumentText = documentText;
            }
        }
        private void MenuBarItem_BeforePopup(object sender, MenuPopupEventArgs e) {
            if (sender == this.menuBarItemFile) {
                this.menuButtonItemExit.Enabled = true;
            }
        }
        private void MenuButtonItem_Activate(object sender, EventArgs e) {
            if (sender == this.menuButtonItemExit) {
                this.Hide();
            }
        }
        private void ToolBar_ButtonClick(object sender, ToolBarItemEventArgs e) {
            if (e.Item == this.buttonItemNew) {
                XmlNode root = this.m_xmlDocument.SelectSingleNode("ROOT");
                root.RemoveAll();
                this.UpdateExceptionHandler();
            }
            else if (e.Item == this.buttonItemSave) {
                this.m_webBrowser.ShowSaveAsDialog();
            }
            else if (e.Item == this.buttonItemMessage) {
                if (string.IsNullOrEmpty(this.m_feedbackAddress)) { return; }
                Process process = new Process();
                process.StartInfo.FileName = this.m_feedbackAddress;
                process.StartInfo.Verb = "Open";
                process.StartInfo.CreateNoWindow = false;
                process.Start();
            }
            else if (e.Item == this.buttonItemPrint) {
                this.m_webBrowser.ShowPrintDialog();
            }
            else if (e.Item == this.buttonItemPrintPreview) {
                this.m_webBrowser.ShowPrintPreviewDialog();
            }
            else if (e.Item == this.buttonItemPageSetup) {
                this.m_webBrowser.ShowPageSetupDialog();
            }
            else if (e.Item == this.buttonItemCopy) {
                if (this.m_webBrowser == null) { return; }
                this.m_webBrowser.Document.ExecCommand("SelectAll", false, null);
                this.m_webBrowser.Document.ExecCommand("Copy", false, null);
                this.m_webBrowser.Document.ExecCommand("Unselect", false, null);
            }
            else if (e.Item == this.buttonItemPlay) {
                this.m_enabled = true;
                this.Invoke(new EventHandler(this.Timer_Tick), new object[] { this, EventArgs.Empty });
            }
            else if (e.Item == this.buttonItemPause) {
                this.m_enabled = false;
                this.Invoke(new EventHandler(this.Timer_Tick), new object[] { this, EventArgs.Empty });
            }
        }
    }
}
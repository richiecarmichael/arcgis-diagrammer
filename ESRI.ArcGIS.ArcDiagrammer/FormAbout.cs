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
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.ArcDiagrammer.Properties;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class FormAbout : Form {
        public FormAbout() {
            //
            InitializeComponent();

            // Get Assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Assembly Version
            this.labelApplicationVersion.Text = string.Format(
                "{0} {1}",
                Resources.TEXT_VERSION,
                assembly.GetName().Version.ToString(3));

            // Labelling
            this.labelApplicationName.Text = Resources.TEXT_ARCDIAGRAMMER;
            this.labelAttribution.Text = Resources.TEXT_ATTRIBUTION_STATEMENT.Replace("|", Environment.NewLine);
            this.labelCopyright.Text = Resources.TEXT_COPYRIGHT_STATEMENT;
            this.labelDevelopedBy.Text = Resources.TEXT_DEVELOPED_BY_STATEMENT;
            this.labelWarning.Text = Resources.TEXT_WARNING_STATEMENT.Replace("|", Environment.NewLine);
            this.linkLabelWebsite.Text = Resources.TEXT_URL_ESRI_HOME;
            this.buttonOK.Text = Resources.TEXT_OK;
            this.linkLabelContactAuthor.Text = Resources.TEXT_CONTACT_AUTHOR;

            // Icon and Text
            this.Text = Resources.TEXT_ABOUT_ARCDIAGRAMMER;
            this.Icon = Resources.DIAGRAMMER;
        }
        //
        // PRIVATE METHODS
        //
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                if (sender == null) { return; }
                if (!(sender is LinkLabel)) { return; }
                LinkLabel linkLabel = (LinkLabel)sender;

                string link = null;
                if (linkLabel == this.linkLabelWebsite) {
                    link = Resources.TEXT_URL_ESRI_HOME;
                }
                else if (linkLabel == this.linkLabelContactAuthor) {
                    link = Resources.TEXT_URL_FEEDBACK;
                }
                if (link == null) { return; }

                Process process = new Process();
                process.StartInfo.FileName = link;
                process.StartInfo.Verb = "Open";
                process.StartInfo.CreateNoWindow = false;
                process.Start();
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
    }
}
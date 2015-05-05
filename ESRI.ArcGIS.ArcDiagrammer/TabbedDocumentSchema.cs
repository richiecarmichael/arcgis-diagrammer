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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Crainiate.ERM4;
using ESRI.ArcGIS.ArcDiagrammer.Properties;
using ESRI.ArcGIS.Diagrammer;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandDock;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public partial class TabbedDocumentSchema : UserTabbedDocument, ITabModel {
        //
        // CONSTRUCTOR
        //
        public TabbedDocumentSchema(string filename) {
            InitializeComponent();

            // Set TabbedDocument Properties
            this.TabImage = this.schemaModel1.Icon;
            this.Text = string.Empty;

            // If Empty then Open Balance Diagram
            if (string.IsNullOrEmpty(filename)) {
                this.schemaModel1.OpenModel();
                return;
            }

            // Get File Extension
            string extension = Path.GetExtension(filename);

            // Load Based on Extension
            switch (extension.ToUpper()) {
                case ".XML":
                    try {
                        this.schemaModel1.OpenModel(filename);
                    }
                    catch (Exception ex) {
                        ExceptionDialog.HandleException(ex);
                        MessageBox.Show(
                           "This is not a valid Xml Workspace file",
                           Resources.TEXT_ARCDIAGRAMMER,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                    }
                    break;
                case ".DIAGRAM":
                    try {
                        AppDomain domain = AppDomain.CurrentDomain;
                        domain.AssemblyResolve += new ResolveEventHandler(this.AssemblyResolve);
                        this.schemaModel1.Open(filename, LoadFormat.Binary);
                        domain.AssemblyResolve -= new ResolveEventHandler(this.AssemblyResolve);
                        this.schemaModel1.Document = filename;
                    }
                    catch (Exception ex) {
                        ExceptionDialog.HandleException(ex);
                        MessageBox.Show(
                           "This is not a valid Diagrammer file",
                           Resources.TEXT_ARCDIAGRAMMER,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                    }
                    break;
                default:
                    MessageBox.Show(
                           "File has unsupported file extension",
                           Resources.TEXT_ARCDIAGRAMMER,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                    break;
            }            
        }
        private Assembly AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args == null) { return null; }
            if (string.IsNullOrEmpty(args.Name)) { return null; }
            string[] parts = args.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) { return null; }
            string name = parts[0];
            if (string.IsNullOrEmpty(name)) { return null; }
            AppDomain domain = AppDomain.CurrentDomain;
            foreach (Assembly assembly in domain.GetAssemblies()) {
                AssemblyName s = assembly.GetName();
                if (s == null) { continue; }
                string assemblyName = s.Name;
                if (string.IsNullOrEmpty(assemblyName)) { continue; }
                if (assemblyName.ToUpper() == name.ToUpper()) {
                    return assembly;
                }
            }
            return null;
        }
        //
        // PROPERTIES
        //
        public EsriModel Model {
            get { return this.schemaModel1; }
        }
    }
}

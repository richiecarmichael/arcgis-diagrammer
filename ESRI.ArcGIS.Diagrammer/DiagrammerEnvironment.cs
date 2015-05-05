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
using ESRI.ArcGIS.esriSystem;
using System.IO;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// Singleton class that contains diagrammer specific preferences
    /// </summary>
    public class DiagrammerEnvironment {
        private AoInitialize m_aoInitialize = null;
        private SchemaModel m_schemaModel = null;
        //
        // CONSTRUCTOR
        //
        private DiagrammerEnvironment() { }
        //
        // EVENTS
        //
        public event EventHandler<ProgressEventArgs> ProgressChanged;
        public event EventHandler<TableEventArgs> TableValidationRequest;
        public event EventHandler<DatasetEventArgs> MetadataViewerRequest;
        //
        // PROPERTIES
        //
        private static DiagrammerEnvironment defaultInstance = new DiagrammerEnvironment();
        public static DiagrammerEnvironment Default {
            get { return defaultInstance; }
        }
        public SchemaModel SchemaModel {
            get { return this.m_schemaModel; }
            set { this.m_schemaModel = value; }
        }
        public string DesktopInstallationFolder {
            get {
                RuntimeInfo info = RuntimeManager.ActiveRuntime;
                if (info == null) { return null; }
                return info.Path;
            }
        }
        //
        // PUBLIC METHODS
        //
        public virtual void OnProgressChanged(ProgressEventArgs e) {
            EventHandler<ProgressEventArgs> handler = ProgressChanged;
            if (handler != null) {
                handler(this, e);
            }
        }
        public virtual void OnTableValidationRequest(TableEventArgs e) {
            EventHandler<TableEventArgs> handler = TableValidationRequest;
            if (handler != null) {
                handler(this, e);
            }
        }
        public virtual void OnMetadataViewerRequest(DatasetEventArgs e) {
            EventHandler<DatasetEventArgs> handler = MetadataViewerRequest;
            if (handler != null) {
                handler(this, e);
            }
        }
        public void InitializeArcObjects() {
            // Binding to ArcGIS Desktop
            RuntimeManager.Bind(ProductCode.Desktop);

            // Get ESRI License
            this.m_aoInitialize = new AoInitializeClass();

            if (this.m_aoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeBasic) == esriLicenseStatus.esriLicenseAvailable) {
                this.m_aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);
            }
            else if (this.m_aoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeStandard) == esriLicenseStatus.esriLicenseAvailable) {
                this.m_aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);
            }
            else if (this.m_aoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeAdvanced) == esriLicenseStatus.esriLicenseAvailable) {
                this.m_aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
            }
        }
        public void ShutdownArcObjects() {
            if (this.m_aoInitialize != null) {
                this.m_aoInitialize.Shutdown();
            }
        }
        public string ArcGISMetadataStyleSheet {
            get {
                string path = RuntimeManager.ActiveRuntime.Path;
                string metadata = Path.Combine(path, "Metadata");
                string stylesheets = Path.Combine(metadata, "Stylesheets");
                string arcgis = Path.Combine(stylesheets, "ArcGIS.xsl");
                if (!File.Exists(arcgis)) { return null; }
                return arcgis;
            }
        }
    }
}

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
using System.Drawing.Design;
using System.Security;
using System.Security.Permissions;

namespace ESRI.ArcGIS.Diagrammer {
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class MetadataEditor : UITypeEditor {
        public MetadataEditor() { }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            //
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }
            if (provider == null) { return null; }

            // Cannot handle multiple objects
            if (context.Instance is object[]) { return null; }

            // Check if valid
            if (!(value is string)) { return null; }
            string metadata = (string)value;

            //// If metadata is empty then seed
            //if (string.IsNullOrEmpty(metadata)){
            //    // TODO: Add current date/time
            //    metadata = "<?xml version=\"1.0\"?><metadata xml:lang=\"en\"><Esri><MetaID>{707C35B6-8BAD-469E-8A46-AF465FBAE45D}</MetaID><CreaDate>20071221</CreaDate><CreaTime>17351100</CreaTime><SyncOnce>TRUE</SyncOnce></Esri></metadata>";
            //}

            //// Load Metadata
            //IXmlPropertySet2 xmlPropertySet = new XmlPropertySetClass();
            //IPropertySet propertySet = (IPropertySet)xmlPropertySet;
            //xmlPropertySet.SetXml(metadata);

            //// Load Metadata Editor UI
            //IMetadataEditor metadataEditor = null;
            //switch (ModelSettings.Default.MetadataEditorType) {
            //    case MetadataEditorType.FGDC:
            //        //metadataEditor = new MetaEditClass();
            //        break;
            //    case MetadataEditorType.ISO:
            //        metadataEditor = new EditorClass();
            //        break;
            //}
            //if (metadataEditor == null) { return metadata; }

            //// Display UI
            //bool modified = metadataEditor.Edit(propertySet, 0);

            //// Get Modified Metadata
            //if (modified) {
            //    metadata = xmlPropertySet.GetXml("");
            //}

            // Return Modified Metadata
            return metadata;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

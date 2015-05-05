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
using System.Windows.Forms;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geometry;

namespace ESRI.ArcGIS.Diagrammer {
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WktEditor : UITypeEditor {
        public WktEditor() { }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            //
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }
            if (provider == null) { return null; }

            // Cannot handle multiple objects
            if (context.Instance is object[]) { return null; }

            //
            string wkt = null;
            if (value != null) {
                wkt = value.ToString();
            }
            
            // Do ArcGIS Desktop Test
            object dialog = null;
            try {
                dialog = new SpatialReferenceDialogClass();
            }
            catch { }
            if (dialog == null) {
                MessageBox.Show(
                    "You do not have ArcGIS Desktop Installed",
                    Resources.TEXT_APPLICATION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return null;
            }

            // Variable to hold edited or created spatial reference
            ISpatialReference spatialReferenceOut = null;

            // Use DoModelCreate (if NULL) or DoModelEdit (if not NULL)
            if (string.IsNullOrEmpty(wkt)) {
               
                ISpatialReferenceDialog2 spatialReferenceDialog = (ISpatialReferenceDialog2)dialog;
                spatialReferenceOut = spatialReferenceDialog.DoModalCreate(false, false, false, 0);
            }
            else {
                // Recreate ESRI Spatial Reference from String
                ISpatialReferenceFactory3 spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReferenceInfo spatialReferenceInfo = null;
                int size;
                spatialReferenceFactory.CreateESRISpatialReferenceInfo(wkt, out spatialReferenceInfo, out size);
                ISpatialReference spatialReference = (ISpatialReference)spatialReferenceInfo;

                // Display Spatial Reference Dialog
                ISpatialReferenceDialog3 spatialReferenceDialog = (ISpatialReferenceDialog3)dialog;
                spatialReferenceOut = spatialReferenceDialog.DoModalEdit3(spatialReference, true, 0);
            }

            // Exit if Output SpatialReference is NULL
            if (spatialReferenceOut == null) { return wkt; }

            // Convert ESRI Spatial Reference to a String
            IESRISpatialReferenceGEN spatialReferenceGEN = (IESRISpatialReferenceGEN)spatialReferenceOut;
            string projection;
            int size2;
            spatialReferenceGEN.ExportToESRISpatialReference(out projection, out size2);
            if (string.IsNullOrEmpty(projection)) { return wkt; }

            // Return Result
            return projection;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

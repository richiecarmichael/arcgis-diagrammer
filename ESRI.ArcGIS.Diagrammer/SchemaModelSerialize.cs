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
using System.Runtime.Serialization;
using Crainiate.ERM4.Serialization;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    public class SchemaModelSerialize : ModelSerialize {
        public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context) {
            base.GetObjectData(obj, info, context);
            SchemaModel schemaModel = (SchemaModel)obj;
            info.AddValue("workspaceType", Convert.ToInt32(schemaModel.WorkspaceType).ToString());
            info.AddValue("version", schemaModel.Version);
            info.AddValue("metadata", schemaModel.Metadata);
            info.AddValue("encoding", Convert.ToInt32(schemaModel.XmlEncoding).ToString());
            info.AddValue("indent", schemaModel.XmlIndent);
        }
        public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            base.SetObjectData(obj, info, context, selector);
            SchemaModel schemaModel = (SchemaModel)obj;
            schemaModel.Suspend();
            try {
                schemaModel.WorkspaceType = (esriWorkspaceType)Enum.Parse(typeof(esriWorkspaceType), info.GetString("workspaceType"), true);
                schemaModel.Version = info.GetString("version");
                schemaModel.Metadata = info.GetString("metadata");
                schemaModel.XmlEncoding = (DiagramEncoding)Enum.Parse(typeof(DiagramEncoding), info.GetString("encoding"), true);
                schemaModel.XmlIndent = info.GetBoolean("indent");
            }
            catch { }
            schemaModel.Resume();
            return schemaModel;
        }
    }
}

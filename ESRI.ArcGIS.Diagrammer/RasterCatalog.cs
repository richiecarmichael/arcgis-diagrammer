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
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Raster Catalog
    /// </summary>
    [Serializable]
    public class RasterCatalog : FeatureClass {
        //
        // CONSTRUCTOR
        //
        public RasterCatalog(IXPathNavigable path): base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();
        }
        public RasterCatalog(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public RasterCatalog(RasterCatalog prototype) : base(prototype) { }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/RCAT=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RasterCatalog(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:DERasterCatalog");

            // Writer Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Write Base Errors
            base.Errors(list);

            // TODO: Write Raster Catalog Errors

            // CLSID
            if (string.IsNullOrEmpty(this._clsid)) {
                list.Add(new ErrorTable(this, "CLSID can not be empty", ErrorType.Error));
            }
            else {
                Guid guid = Guid.Empty;
                try {
                    guid = new Guid(this._clsid);
                }
                catch (FormatException) { }
                catch (OverflowException) { }
                if (guid == Guid.Empty) {
                    list.Add(new ErrorTable(this, "CLSID is not a valid guid {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}", ErrorType.Error));
                }
                else {
                    if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_RASTERCATALOG) {
                        list.Add(new ErrorTable(this, string.Format("Raster Catalogs must have a CLSID set to '{0}'.", EsriRegistry.CLASS_RASTERCATALOG), ErrorType.Error));
                    }
                }
            }

            // EXTCLSID
            if (!string.IsNullOrEmpty(this.EXTCLSID)) {
                Guid guid = Guid.Empty;
                try {
                    guid = new Guid(this.EXTCLSID);
                }
                catch (FormatException) { }
                catch (OverflowException) { }
                if (guid == Guid.Empty) {
                    list.Add(new ErrorTable(this, "EXTCLSID is not a valid guid {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}", ErrorType.Error));
                }
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.RasterCatalogColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            base.WriteInnerXml(writer);
        }
        protected override void Initialize() {
            this.GradientColor = ColorSettings.Default.RasterCatalogColor;
            this.SubHeading = Resources.TEXT_RASTER_CATALOG;
        }
    }
}

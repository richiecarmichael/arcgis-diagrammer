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
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Raster Definition
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class RasterDef : EsriObject {
        private string _description = string.Empty;
        private bool _isByRef = false;
        private SpatialReference _spatialReference = null;
        //
        // CONSTRUCTOR
        //
        public RasterDef() : base() { }
        public RasterDef(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Description></Description>
            XPathNavigator navigatorDescription = navigator.SelectSingleNode("Description");
            if (navigatorDescription != null) {
                this._description = navigatorDescription.Value;
            }

            // <IsByRef></IsByRef>
            XPathNavigator navigatorIsByRef = navigator.SelectSingleNode("IsByRef");
            if (navigatorIsByRef != null) {
                this._isByRef = navigatorIsByRef.ValueAsBoolean;
            }

            // <SpatialReference></SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode("SpatialReference");
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
            else {
                this._spatialReference = new SpatialReference();
            }
        }
        public RasterDef(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._description = info.GetString("description");
            this._isByRef = info.GetBoolean("isByRef");
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
        }
        public RasterDef(RasterDef prototype) : base(prototype) {
            this._description = prototype.Description;
            this._isByRef = prototype.IsByRef;
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The raster column description
        /// </summary>
        [Browsable(true)]
        [Category("Raster Definition")]
        [DefaultValue("")]
        [Description("The raster column description")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Description {
            get { return this._description; }
            set { this._description = value; }
        }
        /// <summary>
        /// Indicates if the Raster column value is to be managed by GeoDatabase
        /// </summary>
        [Browsable(true)]
        [Category("Raster Definition")]
        [DefaultValue(false)]
        [Description("Indicates if the Raster column value is to be managed by GeoDatabase")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsByRef {
            get { return this._isByRef; }
            set { this._isByRef = value; }
        }
        /// <summary>
        /// The spatial reference for the dataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Definition")]
        [DefaultValue(null)]
        [Description("The spatial reference for the dataset")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("description", this._description);
            info.AddValue("isByRef", this._isByRef);
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RasterDef(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <GeometryDef>
            writer.WriteStartElement("RasterDef");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:RasterDef");

            // Writer Inner XML
            this.WriteInnerXml(writer);

            // </GeometryDef>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Add RasterDef Errors

            // Add Spatial Reference Errors
            if (this._spatialReference == null) {
                list.Add(new ErrorObject(this, table, "SpatialReference cannot be null", ErrorType.Error));
            }
            else {
                this._spatialReference.Errors(list, table);
            }
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Call base method
            base.WriteInnerXml(writer);

            // <Description></Description>
            writer.WriteStartElement("Description");
            writer.WriteValue(this._description);
            writer.WriteEndElement();

            // <IsByRef></IsByRef>
            writer.WriteStartElement("IsByRef");
            writer.WriteValue(this._isByRef);
            writer.WriteEndElement();

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }
        }
    }
}

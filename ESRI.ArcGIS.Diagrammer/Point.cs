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
    /// ESRI Storage Definition
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class Point : EsriObject {
        private double _x = 0d;
        private double _y = 0d;
        private double _m = 0d;
        private double _z = 0d;
        private int _id = 0;
        private SpatialReference _spatialReference = null;
        private bool _mAware = false;
        private bool _zAware = false;
        private bool _pointIDAware = false;
        //
        // CONSTRUCTOR
        //
        public Point() : base() {
            this._spatialReference = new SpatialReference();
        }
        public Point(IXPathNavigable path): base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <X></X>
            XPathNavigator navigatorX = navigator.SelectSingleNode("X");
            if (navigatorX != null) {
                this._x = navigatorX.ValueAsDouble;
            }

            // <Y></Y>
            XPathNavigator navigatorY = navigator.SelectSingleNode("Y");
            if (navigatorY != null) {
                this._y = navigatorY.ValueAsDouble;
            }

            // <M></M>
            XPathNavigator navigatorM = navigator.SelectSingleNode("M");
            if (navigatorM != null) {
                this._m = navigatorM.ValueAsDouble;
                this._mAware = true;
            }
            else {
                this._mAware = false;
            }

            // <Z></Z>
            XPathNavigator navigatorZ = navigator.SelectSingleNode("Z");
            if (navigatorZ != null) {
                this._z = navigatorZ.ValueAsDouble;
                this._zAware = true;
            }
            else {
                this._zAware = false;
            }

            // <ID></ID>
            XPathNavigator navigatorId = navigator.SelectSingleNode("ID");
            if (navigatorId != null) {
                this._id = navigatorId.ValueAsInt;
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
        public Point(SerializationInfo info, StreamingContext context): base(info, context) {
            this._x = info.GetDouble("x");
            this._y = info.GetDouble("y");
            this._m = info.GetDouble("m");
            this._z = info.GetDouble("z");
            this._id = info.GetInt32("id");
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
            this._mAware = info.GetBoolean("mAware");
            this._zAware = info.GetBoolean("zAware");
            this._pointIDAware = info.GetBoolean("pointIDAware");
        }
        public Point(Point prototype) : base(prototype) {
            this._x = prototype.X;
            this._y = prototype.Y;
            this._m = prototype.M;
            this._z = prototype.Z;
            this._id = prototype.ID;
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._mAware = prototype.MAware;
            this._zAware = prototype.ZAware;
            this._pointIDAware = prototype.PointIDAware;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The X coordinate
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(0d)]
        [Description("The X coordinate")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double X {
            get { return this._x; }
            set { this._x = value; }
        }
        /// <summary>
        /// The Y coordinate
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(0d)]
        [Description("The Y coordinate")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double Y {
            get { return this._y; }
            set { this._y = value; }
        }
        /// <summary>
        /// The measure attribute
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(0d)]
        [Description("The measure attribute")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double M {
            get { return this._m; }
            set { this._m = value; }
        }
        /// <summary>
        /// The Z attribute
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(0d)]
        [Description("The Z attribute")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double Z {
            get { return this._z; }
            set { this._z = value; }
        }
        /// <summary>
        /// The Point ID attribute
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(0)]
        [Description("The Point ID attribute")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int ID {
            get { return this._id; }
            set { this._id = value; }
        }
        /// <summary>
        /// The spatial reference associated with this geometry
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(null)]
        [Description("The spatial reference associated with this geometry")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// Indicates whether or not the geometry is aware of and capable of handling Ms
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(false)]
        [Description("Indicates whether or not the geometry is aware of and capable of handling Ms")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool MAware {
            get { return this._mAware; }
            set { this._mAware = value; }
        }
        /// <summary>
        /// Indicates whether or not the geometry is aware of and capable of handling Zs
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(false)]
        [Description("Indicates whether or not the geometry is aware of and capable of handling Zs")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ZAware {
            get { return this._zAware; }
            set { this._zAware = value; }
        }
        /// <summary>
        /// Indicates whether or not the geometry is aware of and capable of handling PointIDs
        /// </summary>
        [Browsable(true)]
        [Category("Point")]
        [DefaultValue(false)]
        [Description("Indicates whether or not the geometry is aware of and capable of handling PointIDs")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool PointIDAware {
            get { return this._pointIDAware; }
            set { this._pointIDAware = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("x", this._x);
            info.AddValue("y", this._y);
            info.AddValue("m", this._m);
            info.AddValue("z", this._z);
            info.AddValue("id", this._id);
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));
            info.AddValue("mAware", this._mAware);
            info.AddValue("zAware", this._zAware);
            info.AddValue("pointIDAware", this._pointIDAware);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Point(this);
        }
        public override void WriteXml(XmlWriter writer) {
            this.WriteXml(writer, true);
        }
        public void WriteXml(XmlWriter writer, bool writeSpatialReference) {
            // <Point>
            writer.WriteStartElement("Origin");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PointN");

            // Writer Inner XML
            this.WriteInnerXml(writer, writeSpatialReference);

            // </Point>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Add Point Errors

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
            this.WriteInnerXml(writer, true);
        }
        protected void WriteInnerXml(XmlWriter writer, bool writeSpatialReference) {
            // Call base method
            base.WriteInnerXml(writer);

            // <X></X>
            writer.WriteStartElement("X");
            writer.WriteValue(this._x);
            writer.WriteEndElement();

            // <Y></Y>
            writer.WriteStartElement("Y");
            writer.WriteValue(this._y);
            writer.WriteEndElement();

            // <M></M>
            if (this._mAware) {
                writer.WriteStartElement("M");
                writer.WriteValue(this._m);
                writer.WriteEndElement();
            }

            // <Z></Z>
            if (this._zAware) {
                writer.WriteStartElement("Z");
                writer.WriteValue(this._z);
                writer.WriteEndElement();
            }

            // <ID></ID>
            if (this._pointIDAware) {
                writer.WriteStartElement("ID");
                writer.WriteValue(this._id);
                writer.WriteEndElement();
            }

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null && writeSpatialReference) {
                this._spatialReference.WriteXml(writer);
            }
        }
    }
}

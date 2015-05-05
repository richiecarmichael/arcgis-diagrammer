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
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Spatial Reference
    /// </summary>
    [DefaultPropertyAttribute("WKT")]
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class SpatialReference : EsriObject {
        private string _wkt = string.Empty;
        private double _xOrigin = -1d;
        private double _yOrigin = -1d;
        private double _xyScale = -1d;
        private double _zOrigin = -1d;
        private double _zScale = -1d;
        private double _mOrigin = -1d;
        private double _mScale = -1d;
        private double _xyTolerance = -1d;
        private double _zTolerance = -1d;
        private double _mTolerance = -1d;
        private bool _highPrecision = false;
        private double _leftLongitude = -180d;
        //
        // CONTRUCTOR
        //
        public SpatialReference() : base() { }
        public SpatialReference(IXPathNavigable path) : base(path){
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // WKT
            XPathNavigator navigatorWKT = navigator.SelectSingleNode("WKT");
            if (navigatorWKT != null) {
                this._wkt = navigatorWKT.Value;
            }

            // XOrigin
            XPathNavigator navigatorXOrigin = navigator.SelectSingleNode("XOrigin");
            if (navigatorXOrigin != null) {
                this._xOrigin = navigatorXOrigin.ValueAsDouble;
            }

            // YOrigin
            XPathNavigator navigatorYOrigin = navigator.SelectSingleNode("YOrigin");
            if (navigatorYOrigin != null) {
                this._yOrigin = navigatorYOrigin.ValueAsDouble;
            }

            // XYScale
            XPathNavigator navigatorXYScale = navigator.SelectSingleNode("XYScale");
            if (navigatorXYScale != null) {
                this._xyScale = navigatorXYScale.ValueAsDouble;
            }

            // ZOrigin
            XPathNavigator navigatorZOrigin = navigator.SelectSingleNode("ZOrigin");
            if (navigatorZOrigin != null) {
                this._zOrigin = navigatorZOrigin.ValueAsDouble;
            }

            // ZScale
            XPathNavigator navigatorZScale = navigator.SelectSingleNode("ZScale");
            if (navigatorZScale != null) {
                this._zScale = navigatorZScale.ValueAsDouble;
            }

            // MOrigin
            XPathNavigator navigatorMOrigin = navigator.SelectSingleNode("MOrigin");
            if (navigatorMOrigin != null) {
                this._mOrigin = navigatorMOrigin.ValueAsDouble;
            }

            // MScale
            XPathNavigator navigatorMScale = navigator.SelectSingleNode("MScale");
            if (navigatorMScale != null) {
                this._mScale = navigatorMScale.ValueAsDouble;
            }

            // XYTolerance
            XPathNavigator navigatorXYTolerance = navigator.SelectSingleNode("XYTolerance");
            if (navigatorXYTolerance  != null) {
                this._xyTolerance = navigatorXYTolerance.ValueAsDouble;
            }

            // ZTolerance
            XPathNavigator navigatorZTolerance = navigator.SelectSingleNode("ZTolerance");
            if (navigatorZTolerance != null) {
                this._zTolerance = navigatorZTolerance.ValueAsDouble;
            }

            // MTolerance
            XPathNavigator navigatorMTolerance = navigator.SelectSingleNode("MTolerance");
            if (navigatorMTolerance != null) {
                this._mTolerance = navigatorMTolerance.ValueAsDouble;
            }

            // HighPrecision
            XPathNavigator navigatorHighPrecision = navigator.SelectSingleNode("HighPrecision");
            if (navigatorHighPrecision != null) {
                this._highPrecision = navigatorHighPrecision.ValueAsBoolean;
            }

            // LeftLongitude
            XPathNavigator navigatorLeftLongitude = navigator.SelectSingleNode("LeftLongitude");
            if (navigatorLeftLongitude != null) {
                this._leftLongitude = navigatorLeftLongitude.ValueAsDouble;
            }
        }
        public SpatialReference(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._wkt = info.GetString("wkt");
            this._xOrigin = info.GetDouble("xOrigin");
            this._yOrigin = info.GetDouble("yOrigin");
            this._xyScale = info.GetDouble("xyScale");
            this._zOrigin = info.GetDouble("zOrigin");
            this._zScale = info.GetDouble("zScale");
            this._mOrigin = info.GetDouble("mOrigin");
            this._mScale = info.GetDouble("mScale");
            this._xyTolerance = info.GetDouble("xyTolerance");
            this._zTolerance = info.GetDouble("zTolerance");
            this._mTolerance = info.GetDouble("mTolerance");
            this._highPrecision = info.GetBoolean("highPrecision");
            this._leftLongitude = info.GetDouble("leftLongitude");
        }
        public SpatialReference(SpatialReference prototype) : base(prototype) {
            this._wkt = prototype.WKT;
            this._xOrigin = prototype.XOrigin;
            this._yOrigin = prototype.YOrigin;
            this._xyScale = prototype.XYSCale;
            this._zOrigin = prototype.ZOrigin;
            this._zScale = prototype.ZScale;
            this._mOrigin = prototype.MOrigin;
            this._mScale = prototype.MScale;
            this._xyTolerance = prototype.XYTolerance;
            this._zTolerance = prototype.ZTolerance;
            this._mTolerance = prototype.MTolerance;
            this._highPrecision = prototype.HighPrecision;
            this._leftLongitude = prototype.LeftLongitude;
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(null)]
        [Description("Spatial Reference Definition")]
        [EditorAttribute(typeof(WktEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverterAttribute(typeof(WktConverter))]
        public string WKT {
            get { return this._wkt; }
            set { this._wkt = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The origin of the grid on the X axis")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double XOrigin {
            get { return this._xOrigin; }
            set { this._xOrigin = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The origin of the grid on the Y axis")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double YOrigin {
            get { return this._yOrigin; }
            set { this._yOrigin = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("XY Precision")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double XYSCale {
            get { return this._xyScale; }
            set { this._xyScale = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The origin of the grid on the Z axis")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ZOrigin {
            get { return this._zOrigin; }
            set { this._zOrigin = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("Z Precision")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ZScale {
            get { return this._zScale; }
            set { this._zScale = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("M Origin")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double MOrigin {
            get { return this._mOrigin; }
            set { this._mOrigin = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("M Precision")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double MScale {
            get { return this._mScale; }
            set { this._mScale = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The xy tolerance used to control point coalescing in the X and Y dimensions")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double XYTolerance {
            get { return this._xyTolerance; }
            set { this._xyTolerance = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The tolerance used to control point coalescing strictly along the Z axis")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ZTolerance {
            get { return this._zTolerance; }
            set { this._zTolerance = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(-1d)]
        [Description("The tolerance used to determine equality of M values")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double MTolerance {
            get { return this._mTolerance; }
            set { this._mTolerance = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(true)]
        [Description("High Precision")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool HighPrecision {
            get { return this._highPrecision; }
            set { this._highPrecision = value; }
        }
        [Browsable(true)]
        [Category("Spatial Reference")]
        [DefaultValue(typeof(double), "-180")]
        [Description("The least (left) longitude bounding a 360 degree range")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double LeftLongitude {
            get { return this._leftLongitude; }
            set { this._leftLongitude = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("wkt", this._wkt);
            info.AddValue("xOrigin", this._xOrigin);
            info.AddValue("yOrigin", this._yOrigin);
            info.AddValue("xyScale", this._xyScale);
            info.AddValue("zOrigin", this._zOrigin);
            info.AddValue("zScale", this._zScale);
            info.AddValue("mOrigin", this._mOrigin);
            info.AddValue("mScale", this._mScale);
            info.AddValue("xyTolerance", this._xyTolerance);
            info.AddValue("zTolerance", this._zTolerance);
            info.AddValue("mTolerance", this._mTolerance);
            info.AddValue("highPrecision", this._highPrecision);
            info.AddValue("leftLongitude", this._leftLongitude);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new SpatialReference(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <SpatialReference>
            writer.WriteStartElement("SpatialReference");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, this.GetSpatialReferenceType());

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </SpatialReference>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Add Errors
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Call base method
            base.WriteInnerXml(writer);

            // <WKT></WKT>
            if (!string.IsNullOrEmpty(this._wkt)) {
                writer.WriteStartElement("WKT");
                writer.WriteValue(this._wkt);
                writer.WriteEndElement();
            }

            // <XOrigin></XOrigin>
            writer.WriteStartElement("XOrigin");
            writer.WriteValue(this._xOrigin);
            writer.WriteEndElement();

            // <YOrigin></YOrigin>
            writer.WriteStartElement("YOrigin");
            writer.WriteValue(this._yOrigin);
            writer.WriteEndElement();

            // <XYScale></XYScale>
            writer.WriteStartElement("XYScale");
            writer.WriteValue(this._xyScale);
            writer.WriteEndElement();

            if (this._zOrigin != -1d &&
                this._zScale != -1d) {
                // <ZOrigin></ZOrigin>
                writer.WriteStartElement("ZOrigin");
                writer.WriteValue(this._zOrigin);
                writer.WriteEndElement();

                // <ZScale></ZScale>
                writer.WriteStartElement("ZScale");
                writer.WriteValue(this._zScale);
                writer.WriteEndElement();
            }

            if (this._mOrigin != -1d &&
                this._mScale != -1d) {
                // <MOrigin></MOrigin>
                writer.WriteStartElement("MOrigin");
                writer.WriteValue(this._mOrigin);
                writer.WriteEndElement();

                // <MScale></MScale>
                writer.WriteStartElement("MScale");
                writer.WriteValue(this._mScale);
                writer.WriteEndElement();
            }

            // <XYTolerance></XYTolerance>
            writer.WriteStartElement("XYTolerance");
            writer.WriteValue(this._xyTolerance);
            writer.WriteEndElement();

            if (this._zTolerance != -1d) {
                // <ZTolerance></ZTolerance>
                writer.WriteStartElement("ZTolerance");
                writer.WriteValue(this._zTolerance);
                writer.WriteEndElement();
            }

            if (this._mTolerance != -1d) {
                // <MTolerance></MTolerance>
                writer.WriteStartElement("MTolerance");
                writer.WriteValue(this._mTolerance);
                writer.WriteEndElement();
            }

            // <HighPrecision></HighPrecision>
            writer.WriteStartElement("HighPrecision");
            writer.WriteValue(this._highPrecision);
            writer.WriteEndElement();

            // <LeftLongitude></LeftLongitude>
            string type = this.GetSpatialReferenceType();
            if (type == "esri:GeographicCoordinateSystem") {
                writer.WriteStartElement("LeftLongitude");
                writer.WriteValue(this._leftLongitude);
                writer.WriteEndElement();
            }
        }
        //
        // PRIVATE METHODS
        //
        private string GetSpatialReferenceType() {
            // Get SpatialReference Type
            string type = "esri:UnknownCoordinateSystem";
            if (!string.IsNullOrEmpty(this._wkt)) {
                if (this._wkt.Length >= 6) {
                    switch (this._wkt.Substring(0, 6).ToUpper()) {
                        case "GEOGCS":
                            type = "esri:GeographicCoordinateSystem";
                            break;
                        case "PROJCS":
                            type = "esri:ProjectedCoordinateSystem";
                            break;
                    }
                }
            }
            return type;
        }
    }
}

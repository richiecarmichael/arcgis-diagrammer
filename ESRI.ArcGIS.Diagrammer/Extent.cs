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
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class Extent : EsriObject {
        private double _xMin = 0d;
        private double _yMin = 0d;
        private double _xMax = 0d;
        private double _yMax = 0d;
        private SpatialReference _spatialReference = null;
        //
        // CONSTRUCTOR
        //
        public Extent() : base() {
            this._spatialReference = new SpatialReference();
        }
        public Extent(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // XMin
            XPathNavigator navigatorXMin = navigator.SelectSingleNode("XMin");
            if (navigatorXMin != null) {
                this._xMin = navigatorXMin.ValueAsDouble;
            }

            // YMin
            XPathNavigator navigatorYMin = navigator.SelectSingleNode("YMin");
            if (navigatorYMin != null) {
                this._yMin = navigatorYMin.ValueAsDouble;
            }

            // XMax
            XPathNavigator navigatorXMax = navigator.SelectSingleNode("XMax");
            if (navigatorXMax != null) {
                this._xMax = navigatorXMax.ValueAsDouble;
            }

            // YMax
            XPathNavigator navigatorYMax = navigator.SelectSingleNode("YMax");
            if (navigatorYMax != null) {
                this._yMax = navigatorYMax.ValueAsDouble;
            }

            // Spatial Reference
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode("SpatialReference");
            if (navigatorSpatialReference == null) {
                this._spatialReference = new SpatialReference();
            }
            else {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
        }
        public Extent(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._xMin = info.GetDouble("xMin");
            this._yMin = info.GetDouble("yMin");
            this._xMax = info.GetDouble("xMax");
            this._yMax = info.GetDouble("yMax");
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
        }
        public Extent(Extent prototype) : base(prototype) {
            this._xMin = prototype.XMin;
            this._yMin = prototype.YMin;
            this._xMax = prototype.XMax;
            this._yMax = prototype.YMax;
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Extent")]
        [DefaultValue(typeof(double), "0")]
        [Description("The position of the left side")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double XMin {
            get { return this._xMin; }
            set { this._xMin = value; }
        }
        [Browsable(true)]
        [Category("Extent")]
        [DefaultValue(typeof(double), "0")]
        [Description("The position of the bottom")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double YMin {
            get { return this._yMin; }
            set { this._yMin = value; }
        }
        [Browsable(true)]
        [Category("Extent")]
        [DefaultValue(typeof(double), "0")]
        [Description("The position of the right side")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double XMax {
            get { return this._xMax; }
            set { this._xMax = value; }
        }
        [Browsable(true)]
        [Category("Extent")]
        [DefaultValue(typeof(double), "0")]
        [Description("The position of the top")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double YMax {
            get { return this._yMax; }
            set { this._yMax = value; }
        }
        [Browsable(true)]
        [Category("Extent")]
        [DefaultValue(null)]
        [Description("Spatial Reference of Extent")]
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
            info.AddValue("xMin", this._xMin);
            info.AddValue("yMin", this._yMin);
            info.AddValue("xMax", this._xMax);
            info.AddValue("yMax", this._yMax);
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Add Spatial Reference Errors
            if (this._spatialReference == null) {
                list.Add(new ErrorObject(this, table, "SpatialReference cannot be null", ErrorType.Error));
            }
            else {
                this._spatialReference.Errors(list, table);
            }

            // XMin > XMax
            if (this._xMin > this._xMax) {
                list.Add(new ErrorObject(this, table, "XMin is larger thatn XMax", ErrorType.Error));
            }

            // YMin < YMax
            if (this._yMin > this._yMax) {
                list.Add(new ErrorObject(this, table, "YMin is larger thatn YMax", ErrorType.Error));
            }
        }
        public override object Clone() {
            return new Extent(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Extent>
            writer.WriteStartElement("Extent");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:EnvelopeN");

            // Write Coordinate Extent
            if (this._xMin != 0 &&
                this._yMin != 0 &&
                this._xMax != 0 &&
                this._yMax != 0) {
                // <XMin></XMin>
                writer.WriteStartElement("XMin");
                writer.WriteValue(this._xMin);
                writer.WriteEndElement();

                // <YMin></YMin>
                writer.WriteStartElement("YMin");
                writer.WriteValue(this._yMin);
                writer.WriteEndElement();

                // <XMax></XMax>
                writer.WriteStartElement("XMax");
                writer.WriteValue(this._xMax);
                writer.WriteEndElement();

                // <YMax></YMax>
                writer.WriteStartElement("YMax");
                writer.WriteValue(this._yMax);
                writer.WriteEndElement();
            }

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            // </Extent>
            writer.WriteEndElement();
        }
    }
}

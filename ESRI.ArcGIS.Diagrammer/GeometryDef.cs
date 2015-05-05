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
using ESRI.ArcGIS.Geometry;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Geometry Definition
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class GeometryDef : EsriObject {
        private const string AVGNUMPOINTS = "avgNumPoints";
        private const string GEOMETRYTYPE = "geometryType";
        private const string HASM = "hasM";
        private const string HASZ = "hasZ";
        private const string SPATIALREFERENCE = "spatialReference";
        private const string GRIDSIZE0 = "gridSize0";
        private const string GRIDSIZE1 = "gridSize1";
        private const string GRIDSIZE2 = "gridSize2";

        private int _avgNumPoints = 0;
        private esriGeometryType _geometryType = esriGeometryType.esriGeometryPolygon;
        private bool _hasM = false;
        private bool _hasZ = false;
        private SpatialReference _spatialReference = null;
        private double _gridSize0 = -1;
        private double _gridSize1 = -1;
        private double _gridSize2 = -1;
        //
        // CONSTRUCTOR
        //
        public GeometryDef() : base() { }
        public GeometryDef(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <AvgNumPoints></AvgNumPoints>
            XPathNavigator navigatorAvgNumPoints = navigator.SelectSingleNode(Xml.AVGNUMPOINTS);
            if (navigatorAvgNumPoints != null) {
                this._avgNumPoints = navigatorAvgNumPoints.ValueAsInt;
            }

            // <GeometryType></GeometryType>
            XPathNavigator navigatorGeometryType = navigator.SelectSingleNode(Xml.GEOMETRYTYPE);
            if (navigatorGeometryType != null) {
                this._geometryType = (esriGeometryType)Enum.Parse(typeof(esriGeometryType), navigatorGeometryType.Value, true);
            }

            // <HasM></HasM>
            XPathNavigator navigatorHasM = navigator.SelectSingleNode(Xml.HASM);
            if (navigatorHasM != null) {
                this._hasM = navigatorHasM.ValueAsBoolean;
            }

            // <HasZ></HasZ>
            XPathNavigator navigatorHasZ = navigator.SelectSingleNode(Xml.HASZ);
            if (navigatorHasZ != null) {
                this._hasZ = navigatorHasZ.ValueAsBoolean;
            }

            // <SpatialReference></SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode(Xml.SPATIALREFERENCE);
            this._spatialReference = new SpatialReference(navigatorSpatialReference);

            // <GridSize0></GridSize0>
            XPathNavigator navigatorGridSize0 = navigator.SelectSingleNode(Xml.GRIDSIZE0);
            if (navigatorGridSize0 != null) {
                this._gridSize0 = navigatorGridSize0.ValueAsDouble;
            }

            // <GridSize1></GridSize1>
            XPathNavigator navigatorGridSize1 = navigator.SelectSingleNode(Xml.GRIDSIZE1);
            if (navigatorGridSize1 != null) {
                this._gridSize1 = navigatorGridSize1.ValueAsDouble;
            }

            // <GridSize2></GridSize2>
            XPathNavigator navigatorGridSize2 = navigator.SelectSingleNode(Xml.GRIDSIZE2);
            if (navigatorGridSize2 != null) {
                this._gridSize2 = navigatorGridSize2.ValueAsDouble;
            }
        }
        public GeometryDef(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._avgNumPoints = info.GetInt32(GeometryDef.AVGNUMPOINTS);
            this._geometryType = (esriGeometryType)Enum.Parse(typeof(esriGeometryType), info.GetString(GeometryDef.GEOMETRYTYPE), true);
            this._hasM = info.GetBoolean(GeometryDef.HASM);
            this._hasZ = info.GetBoolean(GeometryDef.HASZ);
            this._spatialReference = (SpatialReference)info.GetValue(GeometryDef.SPATIALREFERENCE, typeof(SpatialReference));
            this._gridSize0 = info.GetDouble(GeometryDef.GRIDSIZE0);
            this._gridSize1 = info.GetDouble(GeometryDef.GRIDSIZE1);
            this._gridSize2 = info.GetDouble(GeometryDef.GRIDSIZE2);
        }
        public GeometryDef(GeometryDef prototype) : base(prototype) {
            this._avgNumPoints = prototype.AvgNumPoints;
            this._geometryType = prototype.GeometryType;
            this._hasM = prototype.HasM;
            this._hasZ = prototype.HasZ;
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._gridSize0 = prototype.GridSize0;
            this._gridSize1 = prototype.GridSize1;
            this._gridSize2 = prototype.GridSize2;
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(typeof(int), "0")]
        [Description("Estimated average number of points per feature")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int AvgNumPoints {
            get { return this._avgNumPoints; }
            set { this._avgNumPoints = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(typeof(double), "0")]
        [Description("The enumerated geometry type")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriGeometryType GeometryType {
            get { return this._geometryType; }
            set { this._geometryType = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(false)]
        [Description("Indicates if the feature class has measure (M) values")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool HasM {
            get { return this._hasM; }
            set { this._hasM = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(false)]
        [Description("Indicates if the featureClass has Z values")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool HasZ {
            get { return this._hasZ; }
            set { this._hasZ = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(null)]
        [Description("The spatial reference for the dataset")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(typeof(double), "0")]
        [Description("The size of the first spatial index grid")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(DoubleConverter))]
        public double GridSize0 {
            get { return this._gridSize0; }
            set { this._gridSize0 = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(typeof(double), "0")]
        [Description("The size of the second spatial index grid")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(DoubleConverter))]
        public double GridSize1 {
            get { return this._gridSize1; }
            set { this._gridSize1 = value; }
        }
        [Browsable(true)]
        [Category("Geometry Definition")]
        [DefaultValue(typeof(double), "0")]
        [Description("The size of the third spatial index grid")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(DoubleConverter))]
        public double GridSize2 {
            get { return this._gridSize2; }
            set { this._gridSize2 = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(GeometryDef.AVGNUMPOINTS, this._avgNumPoints);
            info.AddValue(GeometryDef.GEOMETRYTYPE, this._geometryType);
            info.AddValue(GeometryDef.HASM, this._hasM);
            info.AddValue(GeometryDef.HASZ, this._hasZ);
            info.AddValue(GeometryDef.SPATIALREFERENCE, this._spatialReference, typeof(SpatialReference));
            info.AddValue(GeometryDef.GRIDSIZE0, this._gridSize0);
            info.AddValue(GeometryDef.GRIDSIZE1, this._gridSize1);
            info.AddValue(GeometryDef.GRIDSIZE2, this._gridSize2);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new GeometryDef(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <GeometryDef>
            writer.WriteStartElement("GeometryDef");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:GeometryDef");

            // Writer Inner XML
            this.WriteInnerXml(writer);

            // </GeometryDef>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Export SpatialReference Errors
            if (this._spatialReference != null) {
                this._spatialReference.Errors(list, table);
            }

            // AvgNumPoints
            if (this._avgNumPoints < 0) {
                // Must be positive
                list.Add(new ErrorObject(this, table, "AvgNumPoints must be positive", ErrorType.Error));
            }

            // Check GridSize Configuration
            if (this._gridSize0 == -1 && this._gridSize1 == -1 && this._gridSize2 == -1) {
                // OK
            }
            else if (this._gridSize0 != -1 && this._gridSize1 == -1 && this._gridSize2 == -1) {
                // OK
            }
            else if (this._gridSize0 != -1 && this._gridSize1 != -1 && this._gridSize2 == -1) {
                // OK
                if (this._gridSize0 > this._gridSize1) {
                    list.Add(new ErrorObject(this, table, "GridSize0 cannot be larger than GridSize1", ErrorType.Error));
                }
            }
            else if (this._gridSize0 != -1 && this._gridSize1 != -1 && this._gridSize2 != -1) {
                // OK
                if (this._gridSize0 > this._gridSize1) {
                    list.Add(new ErrorObject(this, table, "GridSize0 cannot be larger than GridSize1", ErrorType.Error));
                }
                if (this._gridSize1 > this._gridSize2) {
                    list.Add(new ErrorObject(this, table, "GridSize1 cannot be larger than GridSize2", ErrorType.Error));
                }
            }
            else {
                // Invalid Combination
                string message = string.Format(
                    "Grid configuration '{0}'/'{1}'/'{2}' is invalid",
                    this._gridSize0.ToString(),
                    this._gridSize1.ToString(),
                    this._gridSize2.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }

            // GridSize0
            if (this._gridSize0 != -1){
                if (this._gridSize0 < 0) {
                    // Must be greater than zero
                    list.Add(new ErrorObject(this, table, "GridSize0 must be greater than (or equal to) zero", ErrorType.Error));
                }
            }

            // GridSize1
            if (this._gridSize1 != -1) {
                if (this._gridSize1 < 0) {
                    // Must be positive
                    list.Add(new ErrorObject(this, table, "GridSize1 must be zero or a positive", ErrorType.Error));
                }
            }

            // GridSize2
            if (this._gridSize2 != -1) {
                if (this._gridSize2 < 0) {
                    // Must be positive
                    list.Add(new ErrorObject(this, table, "GridSize2 must be zero or a positive", ErrorType.Error));
                }
            }
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Call base method
            base.WriteInnerXml(writer);

             // <AvgNumPoints></AvgNumPoints>
            writer.WriteStartElement("AvgNumPoints");
            writer.WriteValue(this._avgNumPoints);
            writer.WriteEndElement();

            // <GeometryType></GeometryType>
            writer.WriteStartElement("GeometryType");
            writer.WriteValue(this._geometryType.ToString());
            writer.WriteEndElement();

            // <HasM></HasM>
            writer.WriteStartElement("HasM");
            writer.WriteValue(this._hasM);
            writer.WriteEndElement();

            // <HasZ></HasZ>
            writer.WriteStartElement("HasZ");
            writer.WriteValue(this._hasZ);
            writer.WriteEndElement();

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            // Write Grid Size
            if (this._gridSize0 != -1) {
                // <GridSize0></GridSize0>
                writer.WriteStartElement("GridSize0");
                writer.WriteValue(this._gridSize0);
                writer.WriteEndElement();

                if (this._gridSize1 != -1) {
                    // <GridSize1></GridSize1>
                    writer.WriteStartElement("GridSize1");
                    writer.WriteValue(this._gridSize1);
                    writer.WriteEndElement();

                    if (this._gridSize2 != -1) {
                        // <GridSize2></GridSize2>
                        writer.WriteStartElement("GridSize2");
                        writer.WriteValue(this._gridSize2);
                        writer.WriteEndElement();
                    }
                }
            }
        }
    }
}

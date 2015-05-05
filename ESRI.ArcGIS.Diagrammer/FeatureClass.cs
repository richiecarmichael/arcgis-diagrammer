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
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI FeatureClass
    /// </summary>
    [Serializable]
    public class FeatureClass : ObjectClass {
        private esriFeatureType _featureType = esriFeatureType.esriFTSimple;
        private esriGeometryType _shapeType = esriGeometryType.esriGeometryPolygon;
        private string _shapeFieldName = string.Empty;
        private bool _hasM = false;
        private bool _hasZ = false;
        private string _areaFieldName = string.Empty;
        private string _lengthFieldName = string.Empty;
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        //
        // CONSTRUCTOR
        //
        public FeatureClass(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <FeatureType>
            XPathNavigator navigatorFeatureType = navigator.SelectSingleNode("FeatureType");
            if (navigatorFeatureType != null) {
                string featureType = navigatorFeatureType.Value;
                this._featureType = (esriFeatureType)Enum.Parse(typeof(esriFeatureType), featureType, true);
            }

            // <ShapeType>
            XPathNavigator navigatorShapeType = navigator.SelectSingleNode("ShapeType");
            if (navigatorShapeType != null) {
                string shapeType = navigatorShapeType.Value;
                this._shapeType = (esriGeometryType)Enum.Parse(typeof(esriGeometryType), shapeType, true);
            }

            // <ShapeFieldName>
            XPathNavigator navigatorShapeFieldName = navigator.SelectSingleNode("ShapeFieldName");
            if (navigatorShapeFieldName != null) {
                this._shapeFieldName = navigatorShapeFieldName.Value;
            }

            // <HasM>
            XPathNavigator navigatorHasM = navigator.SelectSingleNode("HasM");
            if (navigatorHasM != null) {
                this._hasM = navigatorHasM.ValueAsBoolean;
            }

            // <HasZ>
            XPathNavigator navigatorHasZ = navigator.SelectSingleNode("HasZ");
            if (navigatorHasZ != null) {
                this._hasZ = navigatorHasZ.ValueAsBoolean;
            }

            // <AreaFieldName>
            XPathNavigator navigatorAreaFieldName = navigator.SelectSingleNode("AreaFieldName");
            if (navigatorAreaFieldName != null) {
                this._areaFieldName = navigatorAreaFieldName.Value;
            }

            // <LengthFieldName>
            XPathNavigator navigatorLengthFieldName = navigator.SelectSingleNode("LengthFieldName");
            if (navigatorLengthFieldName != null) {
                this._lengthFieldName = navigatorLengthFieldName.Value;
            }

            // <Extent>
            XPathNavigator navigatorExtent = navigator.SelectSingleNode("Extent");
            if (navigatorExtent != null) {
                this._extent = new Extent(navigatorExtent);
            }
            else {
                this._extent = new Extent();
            }
            // <SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode("SpatialReference");
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
            else {
                this._spatialReference = new SpatialReference();
            }
        }
        public FeatureClass(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._featureType = (esriFeatureType)Enum.Parse(typeof(esriFeatureType), info.GetString("featureType"), true);
            this._shapeType = (esriGeometryType)Enum.Parse(typeof(esriGeometryType), info.GetString("shapeType"), true);
            this._shapeFieldName = info.GetString("shapeFieldName");
            this._hasM = info.GetBoolean("hasM");
            this._hasZ = info.GetBoolean("hasZ");
            this._areaFieldName = info.GetString("areaFieldName");
            this._lengthFieldName = info.GetString("lengthFieldName");
            this._extent = info.GetValue("extent", typeof(Extent)) as Extent;
            this._spatialReference = info.GetValue("spatialReference", typeof(SpatialReference)) as SpatialReference;
        }
        public FeatureClass(FeatureClass prototype) : base(prototype) {
            this._featureType = prototype.FeatureType;
            this._shapeType = prototype.ShapeType;
            this._shapeFieldName = prototype.ShapeFieldName;
            this._hasM = prototype.HasM;
            this._hasZ = prototype.HasZ;
            this._areaFieldName = prototype.AreaFieldName;
            this._lengthFieldName = prototype.LengthFieldName;
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
        }
        //
        // Properties
        //
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue(esriFeatureType.esriFTSimple)]
        [Description("The feature type of the feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriFeatureType FeatureType {
            get { return this._featureType; }
            set { this._featureType = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue(esriGeometryType.esriGeometryPolygon)]
        [Description("The geometry type of the feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriGeometryType ShapeType {
            get { return this._shapeType; }
            set { this._shapeType = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue("")]
        [Description("The shape field name of the feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string ShapeFieldName {
            get { return this._shapeFieldName; }
            set { this._shapeFieldName = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue(false)]
        [Description("Indicates if the feature class supports Ms")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool HasM {
            get { return this._hasM; }
            set { this._hasM = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue(false)]
        [Description("Indicates if the feature class supports Zs")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool HasZ {
            get { return this._hasZ; }
            set { this._hasZ = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue("")]
        [Description("The geometry area field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string AreaFieldName {
            get { return this._areaFieldName; }
            set { this._areaFieldName = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue("")]
        [Description("The geometry length field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string LengthFieldName {
            get { return this._lengthFieldName; }
            set { this._lengthFieldName = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue(null)]
        [Description("The extent of the GeoDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        [Browsable(true)]
        [Category("FeatureClass")]
        [DefaultValue("")]
        [Description("The spatial reference of the GeoDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/FC=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("featureType", this._featureType);
            info.AddValue("shapeType", this._shapeType);
            info.AddValue("shapeFieldName", this._shapeFieldName);
            info.AddValue("hasM", this._hasM);
            info.AddValue("hasZ", this._hasZ);
            info.AddValue("areaFieldName", this._areaFieldName);
            info.AddValue("lengthFieldName", this._lengthFieldName);
            info.AddValue("extent", this._extent, typeof(Extent));
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new FeatureClass(this);
        }
        public override void Errors(List<Error> list) {
            // Get Base Errors
            base.Errors(list);

            // Add Extent Errors
            if (this._extent == null) {
                list.Add(new ErrorTable(this, "Extent cannot be null", ErrorType.Error));
            }
            else {
                this._extent.Errors(list, this);
            }

            // Add Spatial Reference Errors
            if (this._spatialReference == null) {
                list.Add(new ErrorTable(this, "SpatialReference cannot be null", ErrorType.Error));
            }
            else {
                this._spatialReference.Errors(list, this);
            }

            // Get List of Fields
            List<Field> fields = base.GetFields();

            // ShapeType
            switch (this._shapeType) {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                case esriGeometryType.esriGeometryMultiPatch:
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryPolygon:
                    break;
                default:
                    list.Add(new ErrorTable(this, "Unsupported ShapeType. Must be Point, Multipoint, Polyline or Polygon.", ErrorType.Error));
                    break;
            }

            // Length Field
            switch (this._shapeType) {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                case esriGeometryType.esriGeometryMultiPatch:
                    if (!string.IsNullOrEmpty(this._lengthFieldName)) {
                        list.Add(new ErrorTable(this, "Length field must be empty for this ShapeType", ErrorType.Error));
                    }
                    break;
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryPolygon:
                    if (string.IsNullOrEmpty(this._lengthFieldName)) {
                        list.Add(new ErrorTable(this, "Length field cannot be empty for this ShapeType", ErrorType.Error));
                    }
                    else {
                        Field field = base.FindField(this._lengthFieldName);
                        if (field == null) {
                            list.Add(new ErrorTable(this, "Length field does not exist", ErrorType.Error));
                        }
                        else {
                            if (field.FieldType != esriFieldType.esriFieldTypeDouble) {
                                list.Add(new ErrorTable(this, "Length field must be of type double", ErrorType.Error));
                            }
                        }
                    }
                    break;               
            }

            // Area Field
            switch (this._shapeType) {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryMultiPatch:
                    if (!string.IsNullOrEmpty(this._areaFieldName)) {
                        list.Add(new ErrorTable(this, "Area field must be empty for this ShapeType", ErrorType.Error));
                    }
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    if (string.IsNullOrEmpty(this._areaFieldName)) {
                        list.Add(new ErrorTable(this, "Area field cannot be empty for this ShapeType", ErrorType.Error));
                    }
                    else {
                        Field field = base.FindField(this._areaFieldName);
                        if (field == null) {
                            list.Add(new ErrorTable(this, "Area field does not exist", ErrorType.Error));
                        }
                        else {
                            if (field.FieldType != esriFieldType.esriFieldTypeDouble) {
                                list.Add(new ErrorTable(this, "Area field must be of type double", ErrorType.Error));
                            }
                        }
                    }
                    break;
            }

            // FeatureType
            switch (this._featureType) {
                case esriFeatureType.esriFTAnnotation:
                case esriFeatureType.esriFTComplexEdge:
                case esriFeatureType.esriFTComplexJunction:
                case esriFeatureType.esriFTDimension:
                case esriFeatureType.esriFTRasterCatalogItem:
                case esriFeatureType.esriFTSimple:
                case esriFeatureType.esriFTSimpleEdge:
                case esriFeatureType.esriFTSimpleJunction:
                    break;
                case esriFeatureType.esriFTCoverageAnnotation:
                    list.Add(new ErrorTable(this, "Unsupported FeatureType", ErrorType.Error));
                    break;
            }


            // ShapeFieldName
            if (string.IsNullOrEmpty(this._shapeFieldName)) {
                list.Add(new ErrorTable(this, "ShapeFieldName cannot be empty", ErrorType.Error));
            }
            else {
                Field field = base.FindField(this._shapeFieldName);
                if (field == null) {
                    list.Add(new ErrorTable(this, "ShapeFieldName does not exist", ErrorType.Error));
                }
                else {
                    if (field.FieldType == esriFieldType.esriFieldTypeGeometry) {
                        if (field.GeometryDef != null) {
                            if (field.GeometryDef.HasM != this._hasM) {
                                list.Add(new ErrorTable(this, "FeatureClass 'HasM' does not much GeometryDef 'HasM'.", ErrorType.Error));
                            }
                            if (field.GeometryDef.HasZ != this._hasZ) {
                                list.Add(new ErrorTable(this, "FeatureClass 'HasZ' does not much GeometryDef 'HasZ'.", ErrorType.Error));
                            }
                            if (field.GeometryDef.GeometryType != this._shapeType) {
                                list.Add(new ErrorTable(this, "FeatureClass 'ShapeType' does not much GeometryDef 'GeometryType'.", ErrorType.Error));
                            }
                        }
                    }
                    else{
                        list.Add(new ErrorTable(this, "ShapeFieldName must be of type geometry", ErrorType.Error));
                    }
                }
            }

            // Only one shapefield permitted in FeatureClass
            int count = 0;
            foreach (Field field in base.GetFields()) {
                if (field.FieldType == esriFieldType.esriFieldTypeGeometry) {
                    count++;
                }
            }
            switch (count) {
                case 0:
                    list.Add(new ErrorTable(this, "A FeatureClass must have one geometry field", ErrorType.Error));
                    break;
                case 1:

                    break;
                default:
                    list.Add(new ErrorTable(this, "A FeatureClass cannot have more than one geometry field", ErrorType.Error));
                    break;
            }

            // CLSID
            if (this.GetType() == typeof(FeatureClass)){
                if (string.IsNullOrEmpty(this.CLSID)) {
                    list.Add(new ErrorTable(this, "CLSID can not be empty", ErrorType.Error));
                }
                else {
                    Guid guid = Guid.Empty;
                    try {
                        guid = new Guid(this.CLSID);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                    if (guid == Guid.Empty) {
                        list.Add(new ErrorTable(this, "CLSID is not a valid guid {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}", ErrorType.Error));
                    }
                    else {
                        switch (this._featureType) {
                            case esriFeatureType.esriFTAnnotation:
                                 if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_ANNOTATION) {
                                    list.Add(new ErrorTable(this, string.Format("Annotation FeatureClasses must have a CLSID set to '{0}'.", EsriRegistry.CLASS_ANNOTATION), ErrorType.Error));
                                 }
                                break;
                            case esriFeatureType.esriFTComplexEdge:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_COMPLEXEDGE) {
                                    list.Add(new ErrorTable(this, string.Format("Complex Edge FeatureClasses must have a CLSID set to '{0}'.", EsriRegistry.CLASS_COMPLEXEDGE), ErrorType.Error));
                                 }
                                break;
                            case esriFeatureType.esriFTComplexJunction:
                                // Not supported in ArcGIS Diagrammer
                                break;
                            case esriFeatureType.esriFTDimension:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_DIMENSION) {
                                    list.Add(new ErrorTable(this, string.Format("Dimension FeatureClasses must have a CLSID set to '{0}'.", EsriRegistry.CLASS_DIMENSION), ErrorType.Error));
                                }
                                break;
                            case esriFeatureType.esriFTRasterCatalogItem:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_RASTERCATALOG) {
                                    list.Add(new ErrorTable(this, string.Format("Raster Catalog must have a CLSID set to '{0}'.", EsriRegistry.CLASS_RASTERCATALOG), ErrorType.Error));
                                }
                                break;
                            case esriFeatureType.esriFTSimple:
                                // Note: Could be custom featureclass?
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_FEATURECLASS) {
                                    list.Add(new ErrorTable(this, string.Format("The featureclass guid is valid but normally '{0}'.", EsriRegistry.CLASS_FEATURECLASS), ErrorType.Warning));
                                }
                                break;
                            case esriFeatureType.esriFTSimpleEdge:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_SIMPLEEDGE) {
                                    list.Add(new ErrorTable(this, string.Format("Simple Edge must have a CLSID set to '{0}'.", EsriRegistry.CLASS_SIMPLEEDGE), ErrorType.Error));
                                }
                                break;
                            case esriFeatureType.esriFTSimpleJunction:
                                 if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_SIMPLEJUNCTION) {
                                    list.Add(new ErrorTable(this, string.Format("Simple Junction must have a CLSID set to '{0}'.", EsriRegistry.CLASS_SIMPLEJUNCTION), ErrorType.Error));
                                }
                                break;
                        }
                    }
                }
            }

            // EXTCLSID
            if (this.GetType() == typeof(FeatureClass)) {
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
                    else {
                        switch (this._featureType) {
                            case esriFeatureType.esriFTAnnotation:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_ANNOTATION_EXTENSION) {
                                    list.Add(new ErrorTable(this, string.Format("Annotation FeatureClasses must have a EXTCLSID set to '{0}'.", EsriRegistry.CLASS_ANNOTATION), ErrorType.Error));
                                }
                                break;
                            case esriFeatureType.esriFTDimension:
                                if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_DIMENSION_EXTENSION) {
                                    list.Add(new ErrorTable(this, string.Format("Dimension FeatureClasses must have a EXTCLSID set to '{0}'.", EsriRegistry.CLASS_DIMENSION_EXTENSION), ErrorType.Error));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DEFEATURECLASS);

            // Writer Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.FeatureClassColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // Get Model
            SchemaModel model = (SchemaModel)base.Container;

            // <FeatureType>
            writer.WriteStartElement("FeatureType");
            writer.WriteValue(this._featureType.ToString());
            writer.WriteEndElement();

            // <ShapeType>
            writer.WriteStartElement("ShapeType");
            writer.WriteValue(this._shapeType.ToString());
            writer.WriteEndElement();

            // <ShapeFieldName>
            writer.WriteStartElement("ShapeFieldName");
            writer.WriteValue(this._shapeFieldName);
            writer.WriteEndElement();

            // <HasM>
            writer.WriteStartElement("HasM");
            writer.WriteValue(this._hasM);
            writer.WriteEndElement();

            // <HasZ>
            writer.WriteStartElement("HasZ");
            writer.WriteValue(this._hasZ);
            writer.WriteEndElement();

            // <HasSpatialIndex>
            bool hasSpatialIndex = false;
            if (!string.IsNullOrEmpty(this._shapeFieldName)) {
                foreach (Index index in this.Indexes) {
                    foreach(IndexField indexField in index.IndexFields){
                        if (string.IsNullOrEmpty(indexField.Name)) { continue; }
                        if (indexField.Name.ToUpper() == this._shapeFieldName.ToUpper()) {
                            hasSpatialIndex = true;
                            break;
                        }
                    }
                    if (hasSpatialIndex) { break; }
                }
            }
            writer.WriteStartElement("HasSpatialIndex");
            writer.WriteValue(hasSpatialIndex);
            writer.WriteEndElement();

            // <AreaFieldName>
            writer.WriteStartElement("AreaFieldName");
            writer.WriteValue(this._areaFieldName);
            writer.WriteEndElement();

            // <LengthFieldName>
            writer.WriteStartElement("LengthFieldName");
            writer.WriteValue(this._lengthFieldName);
            writer.WriteEndElement();

            // <Extent>
            if (this._extent != null) {
                this._extent.WriteXml(writer);
            }

            // <SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }
        }
        protected override void Initialize() {
            this.GradientColor = ColorSettings.Default.FeatureClassColor;
            this.SubHeading = Resources.TEXT_FEATURE_CLASS;
        }
    }
}

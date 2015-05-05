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
using Crainiate.Diagramming;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Raster Band
    /// </summary>
    [Serializable]
    public class RasterBand : Dataset {
        private string _oidFieldName = string.Empty;
        private bool _isInteger = false;
        private double _meanCellHeight = 0d;
        private double _meanCellWidth = 0d;
        private int _height = 0;
        private int _width = 0;
        private rstPixelType _pixelType = rstPixelType.PT_UNKNOWN;
        private int _primaryField = 0;
        private esriRasterTableTypeEnum _tableType = esriRasterTableTypeEnum.esriRasterTableIndex;
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        //
        // CONSTRUCTOR
        //
        public RasterBand(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <OIDFieldName>
            XPathNavigator navigatorOIDFieldName = navigator.SelectSingleNode("OIDFieldName");
            if (navigatorOIDFieldName != null) {
                this._oidFieldName = navigatorOIDFieldName.Value;
            }

            // Create Fields Group
            TableGroup tableGroupFields = new TableGroup();
            tableGroupFields.Expanded = true;
            tableGroupFields.Text = "Fields";
            this.Groups.Add(tableGroupFields);

            XPathNodeIterator interatorField = navigator.Select("Fields/FieldArray/Field");
            while (interatorField.MoveNext()) {
                // Create Field
                XPathNavigator navigatorField = interatorField.Current;
                Field field = new Field(navigatorField, this);

                // Add Field To Group
                tableGroupFields.Rows.Add(field);
            }

            // Create Indexes Group
            TableGroup tableGroupIndexes = new TableGroup();
            tableGroupIndexes.Expanded = true;
            tableGroupIndexes.Text = "Indexes";
            this.Groups.Add(tableGroupIndexes);

            XPathNodeIterator interatorIndex = navigator.Select("Indexes/IndexArray/Index");
            while (interatorIndex.MoveNext()) {
                // Add Index
                XPathNavigator navigatorIndex = interatorIndex.Current;
                Index index = new Index(navigatorIndex);
                tableGroupIndexes.Groups.Add(index);

                // Add Field Index
                XPathNodeIterator interatorIndexField = navigatorIndex.Select("Fields/FieldArray/Field");
                while (interatorIndexField.MoveNext()) {
                    XPathNavigator navigatorIndexField = interatorIndexField.Current;
                    IndexField indexField = new IndexField(navigatorIndexField);
                    index.Rows.Add(indexField);
                }
            }

            // <IsInteger>
            XPathNavigator navigatorIsInteger = navigator.SelectSingleNode("IsInteger");
            if (navigatorIsInteger != null) {
                this._isInteger = navigatorIsInteger.ValueAsBoolean;
            }

            // <MeanCellHeight>
            XPathNavigator navigatorMeanCellHeight = navigator.SelectSingleNode("MeanCellHeight");
            if (navigatorMeanCellHeight != null) {
                this._meanCellHeight = navigatorMeanCellHeight.ValueAsDouble;
            }

            // <MeanCellWidth>
            XPathNavigator navigatorMeanCellWidth = navigator.SelectSingleNode("MeanCellWidth");
            if (navigatorMeanCellWidth != null) {
                this._meanCellWidth = navigatorMeanCellWidth.ValueAsDouble;
            }

            // <Height>
            XPathNavigator navigatorHeight = navigator.SelectSingleNode("Height");
            if (navigatorHeight != null) {
                this._height = navigatorHeight.ValueAsInt;
            }

            // <Width>
            XPathNavigator navigatorWidth = navigator.SelectSingleNode("Width");
            if (navigatorWidth != null) {
                this._width = navigatorWidth.ValueAsInt;
            }

            // <PixelType>
            XPathNavigator navigatorPixelType = navigator.SelectSingleNode("PixelType");
            if (navigatorPixelType != null) {
                this._pixelType = GeodatabaseUtility.GetPixelType(navigatorPixelType.Value);
            }

            // <PrimaryField>
            XPathNavigator navigatorPrimaryField = navigator.SelectSingleNode("PrimaryField");
            if (navigatorPrimaryField != null) {
                this._primaryField = navigatorPrimaryField.ValueAsInt;
            }

            // <TableType>
            XPathNavigator navigatorTableType = navigator.SelectSingleNode("TableType");
            if (navigatorTableType != null) {
                this._tableType = (esriRasterTableTypeEnum)Enum.Parse(typeof(esriRasterTableTypeEnum), navigatorTableType.Value, true);
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
        public RasterBand(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._oidFieldName = info.GetString("oidFieldName");
            this._isInteger = info.GetBoolean("isInteger");
            this._meanCellHeight = info.GetDouble("meanCellHeight");
            this._meanCellWidth = info.GetDouble("meanCellWidth");
            this._height = info.GetInt32("height");
            this._width = info.GetInt32("width");
            this._pixelType = (rstPixelType)Enum.Parse(typeof(rstPixelType), info.GetString("pixelType"), true);
            this._primaryField = info.GetInt32("primaryField");
            this._tableType = (esriRasterTableTypeEnum)Enum.Parse(typeof(esriRasterTableTypeEnum), info.GetString("tableType"), true);
            this._extent = (Extent)info.GetValue("extent", typeof(Extent));
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
        }
        public RasterBand(RasterBand prototype) : base(prototype) {
            this._oidFieldName = prototype.OidFieldName;
            this._isInteger = prototype.IsInteger;
            this._meanCellHeight = prototype.MeanCellHeight;
            this._meanCellWidth = prototype.MeanCellWidth;
            this._height = prototype.Height2;
            this._width = prototype.Width2;
            this._pixelType = prototype.PixelType;
            this._primaryField = prototype.PrimaryField;
            this._tableType = prototype.TableType;
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The name of the OID Field
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue("")]
        [Description("The name of the OID Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string OidFieldName {
            get { return this._oidFieldName; }
            set { this._oidFieldName = value; }
        }
        /// <summary>
        /// Indicates if the data is integer
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(false)]
        [Description("Indicates if the data is integer")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsInteger {
            get { return this._isInteger; }
            set { this._isInteger = value; }
        }
        /// <summary>
        /// The approximate cell height of the raster
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(0d)]
        [Description("The approximate cell height of the raster")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double MeanCellHeight {
            get { return this._meanCellHeight; }
            set { this._meanCellHeight = value; }
        }
        /// <summary>
        /// The approximate cell width of the raster
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(0d)]
        [Description("The approximate cell width of the raster")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double MeanCellWidth {
            get { return this._meanCellWidth; }
            set { this._meanCellWidth = value; }
        }
        /// <summary>
        /// Number of Rows
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(0)]
        [Description("Number of Rows")]
        [DisplayName("Height")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Height2 {
            get { return this._height; }
            set { this._height = value; }
        }
        /// <summary>
        /// Number of Columns
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(0)]
        [Description("Number of Columns")]
        [DisplayName("Width")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Width2 {
            get { return this._width; }
            set { this._width = value; }
        }
        /// <summary>
        /// Data type of the pixels
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(rstPixelType.PT_UNKNOWN)]
        [Description("Data type of the pixels")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public rstPixelType PixelType {
            get { return this._pixelType; }
            set { this._pixelType = value; }
        }
        /// <summary>
        /// The primary field of the table
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(0)]
        [Description("The primary field of the table")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int PrimaryField {
            get { return this._primaryField; }
            set { this._primaryField = value; }
        }
        /// <summary>
        /// The class names of the table
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(esriRasterTableTypeEnum.esriRasterTableIndex)]
        [Description("The class names of the table")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRasterTableTypeEnum TableType {
            get { return this._tableType; }
            set { this._tableType = value; }
        }
        /// <summary>
        /// The extent of the GeoDataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(null)]
        [Description("The extent of the GeoDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        /// <summary>
        /// The spatial reference of the GeoDataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(null)]
        [Description("The spatial reference of the GeoDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// Get Selected Field
        /// </summary>
        [Browsable(false)]
        [Category("Raster Band")]
        [DefaultValue("")]
        [Description("Get Selected Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Field SelectedField {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                Field field = tableItem as Field;
                return field;
            }
        }
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(null)]
        [Description("Collection of Fields")]
        [Editor(typeof(FieldCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems Fields {
            get {
                TableGroup group = (TableGroup)this.Groups[0];
                TableItems tableItems = group.Rows;
                return tableItems;
            }
        }
        /// <summary>
        /// Get Selected Index
        /// </summary>
        [Browsable(false)]
        [Category("Raster Band")]
        [DefaultValue("")]
        [Description("Get Selected Index")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Index SelectedIndex {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                Index index = tableItem as Index;
                return index;
            }
        }
        [Browsable(true)]
        [Category("Raster Band")]
        [DefaultValue(null)]
        [Description("Collection of Indexes")]
        [Editor(typeof(IndexCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems Indexes {
            get {
                TableGroup group = (TableGroup)this.Groups[1];
                TableItems tableItems = group.Groups;
                return tableItems;
            }
        }
        /// <summary>
        /// Get Selected Index Field
        /// </summary>
        [Browsable(false)]
        [Category("Raster Band")]
        [DefaultValue("")]
        [Description("Get Selected Index Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public IndexField SelectedIndexField {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                IndexField indexField = tableItem as IndexField;
                return indexField;
            }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/RB=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("oidFieldName", this._oidFieldName);
            info.AddValue("isInteger", this._isInteger);
            info.AddValue("meanCellHeight", this._meanCellHeight);
            info.AddValue("meanCellWidth", this._meanCellWidth);
            info.AddValue("height", this._height);
            info.AddValue("width", this._width);
            info.AddValue("pixelType", Convert.ToInt32(this._pixelType).ToString());
            info.AddValue("primaryField", this._primaryField);
            info.AddValue("tableType", Convert.ToInt32(this._tableType).ToString());
            info.AddValue("extent", this._extent, typeof(Extent));
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RasterBand(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DERASTERBAND);

            // Writer Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // TODO: Write RasterBand Errors
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
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.RasterBandColor;
        }
        public void AddField(Field field) {
            TableGroup groupField = (TableGroup)this.Groups[0];
            groupField.Rows.Add(field);
            this.SelectedItem = field;
        }
        public void AddIndex(Index index) {
            TableGroup groupIndex = (TableGroup)this.Groups[1];
            groupIndex.Groups.Add(index);
            this.SelectedItem = index;
        }
        public void RemoveField(Field field) {
            // Get Field Group
            TableGroup groupField = (TableGroup)this.Groups[0];

            // Get Field
            int i = groupField.Rows.IndexOf(field);
            if (i == -1) { return; }

            // Remove Field
            groupField.Rows.RemoveAt(i);

            // Select Next Coded Value
            if (groupField.Rows.Count == 0) {
                this.SelectedItem = groupField;
            }
            else {
                if (i != groupField.Rows.Count) {
                    this.SelectedItem = groupField.Rows[i];
                }
                else {
                    this.SelectedItem = groupField.Rows[groupField.Rows.Count - 1];
                }
            }
        }
        public void RemoveIndex(Index index) {
            // Get Index Grouo
            TableGroup groupIndex = (TableGroup)this.Groups[1];

            // Get Index
            int i = groupIndex.Groups.IndexOf(index);
            if (i == -1) { return; }

            // Remove Index
            groupIndex.Groups.RemoveAt(i);

            // Select Next Index
            if (groupIndex.Groups.Count == 0) {
                this.SelectedItem = groupIndex;
            }
            else {
                if (i != groupIndex.Groups.Count) {
                    this.SelectedItem = groupIndex.Groups[i];
                }
                else {
                    this.SelectedItem = groupIndex.Groups[groupIndex.Groups.Count - 1];
                }
            }
        }
        public Field FindField(string name) {
            Field field = null;
            TableGroup groupField = (TableGroup)this.Groups[0];
            foreach (Field fieldTest in groupField.Rows) {
                if (name.ToUpper() == fieldTest.Name.ToUpper()) {
                    field = fieldTest;
                    break;
                }
            }
            return field;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Inner Xml
            base.WriteInnerXml(writer);

            // <HasOID>
            bool hasOID = !string.IsNullOrEmpty(this._oidFieldName);
            writer.WriteStartElement("HasOID");
            writer.WriteValue(hasOID);
            writer.WriteEndElement();

            // <OIDFieldName>
            writer.WriteStartElement("OIDFieldName");
            writer.WriteValue(this._oidFieldName);
            writer.WriteEndElement();

            // <Fields>
            TableGroup tableGroupField = (TableGroup)base.Groups[0];
            writer.WriteStartElement("Fields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Fields");

            // <FieldArray>
            writer.WriteStartElement("FieldArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfField");

            // <Field></Field>
            foreach (Field field in tableGroupField.Rows) {
                field.WriteXml(writer);
            }

            // </FieldArray>
            writer.WriteEndElement();

            // </Fields>
            writer.WriteEndElement();

            // <Indexes>
            TableGroup tableGroupIndex = (TableGroup)base.Groups[1];
            writer.WriteStartElement("Indexes");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Indexes");

            // <IndexArray>
            writer.WriteStartElement("IndexArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfIndex");

            // <Index></Index>
            foreach (Index index in tableGroupIndex.Groups) {
                index.WriteXml(writer);
            }

            // </IndexArray>
            writer.WriteEndElement();

            // </Indexes>
            writer.WriteEndElement();

            // <IsInteger>
            writer.WriteStartElement("IsInteger");
            writer.WriteValue(this._isInteger);
            writer.WriteEndElement();

            // <MeanCellHeight>
            writer.WriteStartElement("MeanCellHeight");
            writer.WriteValue(this._meanCellHeight);
            writer.WriteEndElement();

            // <MeanCellWidth>
            writer.WriteStartElement("MeanCellWidth");
            writer.WriteValue(this._meanCellWidth);
            writer.WriteEndElement();

            // <Height>
            writer.WriteStartElement("Height");
            writer.WriteValue(this._height);
            writer.WriteEndElement();

            // <Width>
            writer.WriteStartElement("Width");
            writer.WriteValue(this._width);
            writer.WriteEndElement();

            // <PixelType>
            writer.WriteStartElement("PixelType");
            writer.WriteValue(GeodatabaseUtility.GetDescription(this._pixelType));
            writer.WriteEndElement();

            // <PrimaryField>
            writer.WriteStartElement("PrimaryField");
            writer.WriteValue(this._primaryField);
            writer.WriteEndElement();

            // <TableType>
            writer.WriteStartElement("TableType");
            writer.WriteValue(this._tableType.ToString());
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
            this.GradientColor = ColorSettings.Default.RasterBandColor;
            this.SubHeading = Resources.TEXT_RASTER_BAND;
        }
    }
}

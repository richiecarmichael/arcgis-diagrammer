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
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Terrain Data Source
    /// </summary>
    /// <remarks>
    /// Returns one of the feature classes participating in the terrain.
    /// </remarks>
    [Serializable]
    public class TerrainDataSource : EsriObject, IDiagramProperty {
        private int _featureClassID = 0;
        private string _featureClassName = null;
        private int _groupID = 0;
        private TerrainElementStatus _sourceStatus = TerrainElementStatus.Resident;
        private TerrainDataSourceType _sourceType = TerrainDataSourceType.Referenced;
        private esriTinSurfaceType _surfaceFeatureType = esriTinSurfaceType.esriTinMassPoint;
        private bool _isBase = false;
        private bool _anchored = false;
        private bool _applyToOverview = false;
        private bool _autoGeneralize = false;
        private double _resolutionLowerBound = 0;
        private double _resolutionUpperBound = 0;
        private string _sourceName = null;
        private string _heightField = null;
        private string _tagValueField = null;
        private List<string> _reservedFields = null;
        private bool _showLabels = true;
        //
        // CONSTRUCTOR
        //
        public TerrainDataSource() : base() {
            this._reservedFields = new List<string>();
        }
        public TerrainDataSource(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            //<FeatureClassID>5</FeatureClassID> 
            XPathNavigator navigatorFeatureClassID = navigator.SelectSingleNode("FeatureClassID");
            if (navigatorFeatureClassID != null) {
                this._featureClassID = navigatorFeatureClassID.ValueAsInt;
            }

            //<FeatureClassName>topo_clip_poly</FeatureClassName> 
            XPathNavigator navigatorFeatureClassName = navigator.SelectSingleNode("FeatureClassName");
            if (navigatorFeatureClassName != null) {
                this._featureClassName = navigatorFeatureClassName.Value;
            }

            //<GroupID>1</GroupID> 
            XPathNavigator navigatorGroupID = navigator.SelectSingleNode("GroupID");
            if (navigatorGroupID != null) {
                this._groupID = navigatorGroupID.ValueAsInt;
            }

            //<SourceStatus>1</SourceStatus> 
            XPathNavigator navigatorSourceStatus = navigator.SelectSingleNode("SourceStatus");
            if (navigatorSourceStatus != null) {
                this._sourceStatus = (TerrainElementStatus)Enum.Parse(typeof(TerrainElementStatus), navigatorSourceStatus.Value, true);
            }

            //<SourceType>0</SourceType> 
            XPathNavigator navigatorSourceType = navigator.SelectSingleNode("SourceType");
            if (navigatorSourceType != null) {
                this._sourceType = (TerrainDataSourceType)Enum.Parse(typeof(TerrainDataSourceType), navigatorSourceType.Value, true);
            }

            //<SurfaceFeatureType>18</SurfaceFeatureType> 
            XPathNavigator navigatorSurfaceFeatureType = navigator.SelectSingleNode("SurfaceFeatureType");
            if (navigatorSurfaceFeatureType != null) {
                this._surfaceFeatureType = (esriTinSurfaceType)Enum.Parse(typeof(esriTinSurfaceType), navigatorSurfaceFeatureType.Value, true);
            }

            //<IsBase>false</IsBase> 
            XPathNavigator navigatorIsBase = navigator.SelectSingleNode("IsBase");
            if (navigatorIsBase != null) {
                this._isBase = navigatorIsBase.ValueAsBoolean;
            }

            //<Anchored>false</Anchored> 
            XPathNavigator navigatorAnchored = navigator.SelectSingleNode("Anchored");
            if (navigatorAnchored != null) {
                this._anchored = navigatorAnchored.ValueAsBoolean;
            }

            //<ApplyToOverview>true</ApplyToOverview> 
            XPathNavigator navigatorApplyToOverview = navigator.SelectSingleNode("ApplyToOverview");
            if (navigatorApplyToOverview != null) {
                this._applyToOverview = navigatorApplyToOverview.ValueAsBoolean;
            }

            //<AutoGeneralize>false</AutoGeneralize> 
            XPathNavigator navigatorAutoGeneralize = navigator.SelectSingleNode("AutoGeneralize");
            if (navigatorAutoGeneralize != null) {
                this._autoGeneralize = navigatorAutoGeneralize.ValueAsBoolean;
            }

            //<ResolutionLowerBound>0</ResolutionLowerBound> 
            XPathNavigator navigatorResolutionLowerBound = navigator.SelectSingleNode("ResolutionLowerBound");
            if (navigatorResolutionLowerBound != null) {
                this._resolutionLowerBound = navigatorResolutionLowerBound.ValueAsInt;
            }

            //<ResolutionUpperBound>32</ResolutionUpperBound> 
            XPathNavigator navigatorResolutionUpperBound = navigator.SelectSingleNode("ResolutionUpperBound");
            if (navigatorResolutionUpperBound != null) {
                this._resolutionUpperBound = navigatorResolutionUpperBound.ValueAsInt;
            }

            //<SourceName /> 
            XPathNavigator navigatorSourceName = navigator.SelectSingleNode("SourceName");
            if (navigatorSourceName != null) {
                this._sourceName = navigatorSourceName.Value;
            }

            //<HeightField /> 
            XPathNavigator navigatorHeightField = navigator.SelectSingleNode("HeightField");
            if (navigatorHeightField != null) {
                this._heightField = navigatorHeightField.Value;
            }

            //<TagValueField /> 
            XPathNavigator navigatorTagValueField = navigator.SelectSingleNode("TagValueField");
            if (navigatorTagValueField != null) {
                this._tagValueField = navigatorTagValueField.Value;
            }

            // <ReservedFields xsi:type="esri:ArrayOfString" />
            // TODO Verify TerrainDataSource::ReservedFields
            this._reservedFields = new List<string>();
            XPathNodeIterator interatorReservedField = navigator.Select("ReservedFields/String");
            while (interatorReservedField.MoveNext()) {
                // Get <Property>
                XPathNavigator navigatorReservedField = interatorReservedField.Current;

                // Add Property
                this._reservedFields.Add(navigatorReservedField.Value);
            }
        }
        public TerrainDataSource(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._featureClassID = info.GetInt32("featureClassID");
            this._featureClassName = info.GetString("featureClassName");
            this._groupID = info.GetInt32("groupID");
            this._sourceStatus = (TerrainElementStatus)Enum.Parse(typeof(TerrainElementStatus?), info.GetString("sourceStatus"), true);
            this._sourceType = (TerrainDataSourceType)Enum.Parse(typeof(TerrainDataSourceType?), info.GetString("sourceType"), true);
            this._surfaceFeatureType = (esriTinSurfaceType)Enum.Parse(typeof(esriTinSurfaceType?), info.GetString("surfaceFeatureType"), true);
            this._isBase = info.GetBoolean("isBase");
            this._anchored = info.GetBoolean("anchored");
            this._applyToOverview = info.GetBoolean("applyToOverview");
            this._autoGeneralize = info.GetBoolean("autoGeneralize");
            this._resolutionLowerBound = info.GetDouble("resolutionLowerBound");
            this._resolutionUpperBound = info.GetDouble("resolutionUpperBound");
            this._sourceName = info.GetString("sourceName");
            this._heightField = info.GetString("heightField");
            this._tagValueField = info.GetString("tagValueField");
            this._reservedFields = (List<string>)info.GetValue("reservedFields", typeof(List<string>));
        }
        public TerrainDataSource(TerrainDataSource prototype) : base(prototype) {
            this._featureClassID = prototype.FeatureClassID;
            this._featureClassName = prototype.FeatureClassName;
            this._groupID = prototype.GroupID;
            this._sourceStatus = prototype.SourceStatus;
            this._sourceType = prototype.SourceType;
            this._surfaceFeatureType = prototype.SurfaceFeatureType;
            this._isBase = prototype.IsBase;
            this._anchored = prototype.Anchored;
            this._applyToOverview = prototype.ApplyToOverview;
            this._autoGeneralize = prototype.AutoGeneralize;
            this._resolutionLowerBound = prototype.ResolutionLowerBound;
            this._resolutionUpperBound = prototype.ResolutionUpperBound;
            this._sourceName = prototype.SourceName;
            this._heightField = prototype.HeightField;
            this._tagValueField = prototype.TagValueField;
            this._reservedFields = new List<string>();
            foreach (string reservedField in prototype.ReservedFields) {
                this._reservedFields.Add(reservedField);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The unique database identifier for the feature class.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(0)]
        [Description("The unique database identifier for the feature class.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int FeatureClassID {
            get { return this._featureClassID; }
            set { this._featureClassID = value; }
        }
        /// <summary>
        /// Feature class Name
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue("")]
        [Description("Feature class Name")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public string FeatureClassName {
            get { return this._featureClassName; }
            set { this._featureClassName = value; }
        }
        /// <summary>
        /// The identifier of the terrain's thematic group to which this feature class belongs.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(0)]
        [Description("The identifier of the terrain's thematic group to which this feature class belongs.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int GroupID {
            get { return this._groupID; }
            set { this._groupID = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Source Status
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(TerrainElementStatus.Resident)]
        [Description("Source Status")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TerrainElementStatus SourceStatus {
            get { return this._sourceStatus; }
            set { this._sourceStatus = value; }
        }
        /// <summary>
        /// Source Type
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(TerrainDataSourceType.Referenced)]
        [Description("Source Type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TerrainDataSourceType SourceType {
            get { return this._sourceType; }
            set { this._sourceType = value; }
        }
        /// <summary>
        /// Indicates how the features are used to define the terrain surface.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(esriTinSurfaceType.esriTinContour)]
        [Description("Indicates how the features are used to define the terrain surface.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriTinSurfaceType SurfaceFeatureType {
            get { return this._surfaceFeatureType; }
            set { this._surfaceFeatureType = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Is base
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(false)]
        [Description("Is base")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsBase {
            get { return this._isBase; }
            set { this._isBase = value; }
        }
        /// <summary>
        /// Indicates if this is an anchor-points data source.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(false)]
        [Description("Indicates if this is an anchor-points data source.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Anchored {
            get { return this._anchored; }
            set { this._anchored = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Indicates if the 'breakline' data source should be added to the overview Terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(false)]
        [Description("Indicates if the 'breakline' data source should be added to the overview Terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ApplyToOverview {
            get { return this._applyToOverview; }
            set { this._applyToOverview = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Auto Generalize
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(false)]
        [Description("Auto Generalize")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool AutoGeneralize {
            get { return this._autoGeneralize; }
            set { this._autoGeneralize = value; }
        }
        /// <summary>
        /// Lower pyramid levels in which the line/area data source has its features enforced as break edges in the triangulation.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(0d)]
        [Description("Lower pyramid levels in which the line/area data source has its features enforced as break edges in the triangulation.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ResolutionLowerBound {
            get { return this._resolutionLowerBound; }
            set { this._resolutionLowerBound = value; }
        }
        /// <summary>
        /// Upper pyramid levels in which the line/area data source has its features enforced as break edges in the triangulation.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue(0d)]
        [Description("Upper pyramid levels in which the line/area data source has its features enforced as break edges in the triangulation.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ResolutionUpperBound {
            get { return this._resolutionUpperBound; }
            set { this._resolutionUpperBound = value; }
        }
        /// <summary>
        /// The name of the embedded data source.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue("")]
        [Description("The name of the embedded data source.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string SourceName {
            get { return this._sourceName; }
            set { this._sourceName = value; }
        }
        /// <summary>
        /// The database column providing heights for the features.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue("")]
        [Description("The database column providing heights for the features.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string HeightField {
            get { return this._heightField; }
            set { this._heightField = value; }
        }
        /// <summary>
        /// The database column providing tag values for TIN elements derived from the terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue("")]
        [Description("The database column providing tag values for TIN elements derived from the terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string TagValueField {
            get { return this._tagValueField; }
            set { this._tagValueField = value; }
        }
        /// <summary>
        /// Returns the names of the database fields associated with the data source that are copied into the terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Data Source")]
        [DefaultValue("")]
        [Description("Returns the names of the database fields associated with the data source that are copied into the terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<string> ReservedFields {
            get { return this._reservedFields; }
        }
        /// <summary>
        /// Show/Hide Text Labels
        /// </summary>
        [Browsable(true)]
        [Category("Labelling")]
        [DefaultValue(false)]
        [Description("Show/Hide Text Labels")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ShowLabels {
            get { return this._showLabels; }
            set { this._showLabels = value; }
        }
        [Browsable(false)]
        public string Label {
            get {               
                string hs = string.IsNullOrEmpty(this._heightField) ? "None" : this._heightField;
                string gr = this._groupID.ToString();
                string st = GeodatabaseUtility.GetDescription(this._surfaceFeatureType);
                string ov = this._applyToOverview ? "Yes" : "No";
                string ap = this._anchored ? "Yes" : "No";
                
                string label = string.Empty;
                label += string.Format("Source: {0}", hs) + Environment.NewLine;
                label += string.Format("Group: {0}", gr) + Environment.NewLine;
                label += string.Format("Type: {0}", st) + Environment.NewLine;
                label += string.Format("Overview: {0}", ov) + Environment.NewLine;
                label += string.Format("Anchored: {0}", ap) + Environment.NewLine;

                return label; 
            }
        }
        [Browsable(false)]
        public float BorderWidth {
            get { return 1f; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("featureClassID", this._featureClassID);
            info.AddValue("featureClassName", this._featureClassName);
            info.AddValue("groupID", this._groupID);
            info.AddValue("sourceStatus", this._sourceStatus.ToString("d"));
            info.AddValue("sourceType", this._sourceType.ToString("d"));
            info.AddValue("surfaceFeatureType", this._surfaceFeatureType.ToString("d"));
            info.AddValue("isBase", this._isBase);
            info.AddValue("anchored", this._anchored);
            info.AddValue("applyToOverview", this._applyToOverview);
            info.AddValue("autoGeneralize", this._autoGeneralize);
            info.AddValue("resolutionLowerBound", this._resolutionLowerBound);
            info.AddValue("resolutionUpperBound", this._resolutionUpperBound);
            info.AddValue("sourceName", this._sourceName);
            info.AddValue("heightField", this._heightField);
            info.AddValue("tagValueField", this._tagValueField);
            info.AddValue("reservedFields", this._reservedFields, typeof(List<string>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new TerrainDataSource(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO TerrainDataSource Errors
        }
        public override void WriteXml(XmlWriter writer) {
            // <ControllerMembership>
            writer.WriteStartElement(Xml.TERRAINDATASOURCE);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._TERRAINDATASOURCE);

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </ControllerMembership>
            writer.WriteEndElement();
        }
        public override string ToString() {
            if (!string.IsNullOrEmpty(this._featureClassName)) {
                return this._featureClassName;
            }
            return base.ToString();
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            //<FeatureClassID>5</FeatureClassID> 
            writer.WriteStartElement("FeatureClassID");
            writer.WriteValue(this._featureClassID);
            writer.WriteEndElement();

            //<FeatureClassName>topo_clip_poly</FeatureClassName> 
            writer.WriteStartElement("FeatureClassName");
            writer.WriteValue(this._featureClassName);
            writer.WriteEndElement();

            //<GroupID>1</GroupID> 
            writer.WriteStartElement("GroupID");
            writer.WriteValue(this._groupID);
            writer.WriteEndElement();

            //<SourceStatus>1</SourceStatus> 
            writer.WriteStartElement("SourceStatus");
            writer.WriteValue(this._sourceStatus.ToString("d"));
            writer.WriteEndElement();

            //<SourceType>0</SourceType> 
            writer.WriteStartElement("SourceType");
            writer.WriteValue(this._sourceType.ToString("d"));
            writer.WriteEndElement();

            //<SurfaceFeatureType>18</SurfaceFeatureType> 
            writer.WriteStartElement("SurfaceFeatureType");
            writer.WriteValue(this._surfaceFeatureType.ToString("d"));
            writer.WriteEndElement();

            //<IsBase>false</IsBase> 
            writer.WriteStartElement("IsBase");
            writer.WriteValue(this._isBase);
            writer.WriteEndElement();

            //<Anchored>false</Anchored> 
            writer.WriteStartElement("Anchored");
            writer.WriteValue(this._anchored);
            writer.WriteEndElement();

            //<ApplyToOverview>true</ApplyToOverview> 
            writer.WriteStartElement("ApplyToOverview");
            writer.WriteValue(this._applyToOverview);
            writer.WriteEndElement();

            //<AutoGeneralize>false</AutoGeneralize> 
            writer.WriteStartElement("AutoGeneralize");
            writer.WriteValue(this._autoGeneralize);
            writer.WriteEndElement();

            //<ResolutionLowerBound>0</ResolutionLowerBound> 
            writer.WriteStartElement("ResolutionLowerBound");
            writer.WriteValue(this._resolutionLowerBound);
            writer.WriteEndElement();

            //<ResolutionUpperBound>32</ResolutionUpperBound> 
            writer.WriteStartElement("ResolutionUpperBound");
            writer.WriteValue(this._resolutionUpperBound);
            writer.WriteEndElement();

            //<SourceName /> 
            writer.WriteStartElement("SourceName");
            writer.WriteValue(this._sourceName);
            writer.WriteEndElement();

            //<HeightField /> 
            writer.WriteStartElement("HeightField");
            writer.WriteValue(this._heightField);
            writer.WriteEndElement();

            //<TagValueField /> 
            writer.WriteStartElement("TagValueField");
            writer.WriteValue(this._tagValueField);
            writer.WriteEndElement();

            //<ReservedFields xsi:type="esri:ArrayOfString" /> 
            writer.WriteStartElement("ReservedFields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFSTRING);
            foreach (string s in this._reservedFields) {
                writer.WriteStartElement("String");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._STRING);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

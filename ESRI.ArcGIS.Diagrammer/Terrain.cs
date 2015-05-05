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
using ESRI.ArcGIS.GeoDatabaseExtensions;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Terrain
    /// </summary>
    [Serializable]
    public class Terrain : Dataset {
        private string _configurationKeyword = string.Empty;
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        private esriTerrainPyramidType _pyramidType = esriTerrainPyramidType.esriTerrainPyramidWindowSize;
        private esriTerrainWindowSizeMethod _windowSizeMethod = esriTerrainWindowSizeMethod.esriTerrainWindowSizeZmin;
        private double _windowSizeZThreshold = 0;
        private esriTerrainZThresholdStrategy _windowSizeZThresholdStrategy = esriTerrainZThresholdStrategy.esriTerrainZThresholdMildThinning;
        private double _tileSize = 0;
        private int _maxShapeSize = 0;
        private int _maxOverviewSize = 0;
        private Extent _extentDomain = null;
        private Extent _extentAOI = null;
        private List<TerrainDataSource> _terrainDataSources = null;
        private List<TerrainPyramid> _terrainPyramids = null;
        private int _terrainID = 0;
        private int _version = 0;
        //
        // CONSTRUCTOR
        //
        public Terrain(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <ConfigurationKeyword></ConfigurationKeyword>
            XPathNavigator navigatorConfigurationKeyword = navigator.SelectSingleNode("ConfigurationKeyword");
            if (navigatorConfigurationKeyword != null) {
                this._configurationKeyword = navigatorConfigurationKeyword.Value;
            }

            // <Extent></Extent>
            XPathNavigator navigatorExtent = navigator.SelectSingleNode("Extent");
            if (navigatorExtent != null && !string.IsNullOrEmpty(navigatorExtent.InnerXml)) {
                this._extent = new Extent(navigatorExtent);
            }

            // <SpatialReference></SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode("SpatialReference");
            if (navigatorSpatialReference != null && !string.IsNullOrEmpty(navigatorSpatialReference.InnerXml)) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }

            // <FeatureDatasetName></FeatureDatasetName> 
            // Do not load. This can be inferred.

            // <PyramidType></PyramidType>
            XPathNavigator navigatorPyramidType = navigator.SelectSingleNode("PyramidType");
            if (navigatorPyramidType != null) {
                this._pyramidType = (esriTerrainPyramidType)Enum.Parse(typeof(esriTerrainPyramidType), navigatorPyramidType.Value, true);
            }

            // <WindowSizeMethod></WindowSizeMethod>
            XPathNavigator navigatorWindowSizeMethod = navigator.SelectSingleNode("WindowSizeMethod");
            if (navigatorWindowSizeMethod != null) {
                this._windowSizeMethod = (esriTerrainWindowSizeMethod)Enum.Parse(typeof(esriTerrainWindowSizeMethod), navigatorWindowSizeMethod.Value, true);
            }

            // <WindowSizeZThreshold></WindowSizeZThreshold>
            XPathNavigator navigatorWindowSizeZThreshold = navigator.SelectSingleNode("WindowSizeZThreshold");
            if (navigatorWindowSizeZThreshold != null) {
                this._windowSizeZThreshold = navigatorWindowSizeZThreshold.ValueAsDouble;
            }

            // <WindowSizeZThresholdStrategy></WindowSizeZThresholdStrategy>
            XPathNavigator navigatorWindowSizeZThresholdStrategy = navigator.SelectSingleNode("WindowSizeZThresholdStrategy");
            if (navigatorWindowSizeZThresholdStrategy != null) {
                this._windowSizeZThresholdStrategy = (esriTerrainZThresholdStrategy)Enum.Parse(typeof(esriTerrainZThresholdStrategy), navigatorWindowSizeZThresholdStrategy.Value, true);
            }

            // <TileSize></TileSize>
            XPathNavigator navigatorTileSize = navigator.SelectSingleNode("TileSize");
            if (navigatorTileSize != null) {
                this._tileSize = navigatorTileSize.ValueAsDouble;
            }

            // <MaxShapeSize></MaxShapeSize>
            XPathNavigator navigatorMaxShapeSize = navigator.SelectSingleNode("MaxShapeSize");
            if (navigatorMaxShapeSize != null) {
                this._maxShapeSize = navigatorMaxShapeSize.ValueAsInt;
            }

            // <MaxOverviewSize></MaxOverviewSize>
            XPathNavigator navigatorMaxOverviewSize = navigator.SelectSingleNode("MaxOverviewSize");
            if (navigatorMaxOverviewSize != null) {
                this._maxOverviewSize = navigatorMaxOverviewSize.ValueAsInt;
            }

            // <ExtentDomain></ExtentDomain>
            XPathNavigator navigatorExtentDomain = navigator.SelectSingleNode("ExtentDomain");
            if (navigatorExtentDomain != null && !string.IsNullOrEmpty(navigatorExtentDomain.InnerXml)) {
                this._extentDomain = new Extent(navigatorExtentDomain);
            }

            // <ExtentAOI></ExtentAOI>
            XPathNavigator navigatorExtentAOI = navigator.SelectSingleNode("ExtentAOI");
            if (navigatorExtentAOI != null && !string.IsNullOrEmpty(navigatorExtentAOI.InnerXml)) {
                this._extentAOI = new Extent(navigatorExtentAOI);
            }

            // <TerrainDataSources>
            //     <TerrainDataSource>
            //     </TerrainDataSource>
            // </TerrainDataSources>
            this._terrainDataSources = new List<TerrainDataSource>();
            XPathNodeIterator interatorTerrainDataSource = navigator.Select("TerrainDataSources/TerrainDataSource");
            while (interatorTerrainDataSource.MoveNext()) {
                // Get <ConnectivityRule>
                XPathNavigator navigatorTerrainDataSource = interatorTerrainDataSource.Current;
                TerrainDataSource terrainDataSource = new TerrainDataSource(navigatorTerrainDataSource);
                this._terrainDataSources.Add(terrainDataSource);
            }

            // TerrainPyramids
            this._terrainPyramids = new List<TerrainPyramid>();
            string pyramidPath = null;
            switch(this._pyramidType){
                case esriTerrainPyramidType.esriTerrainPyramidWindowSize:
                    // <TerrainPyramidLevelWindowSizes>
                    //    <TerrainPyramidLevelWindowSize>
                    //    </TerrainPyramidLevelWindowSize>
                    // </TerrainPyramidLevelWindowSizes>
                    pyramidPath = "TerrainPyramidLevelWindowSizes/TerrainPyramidLevelWindowSize";
                    break;
                case esriTerrainPyramidType.esriTerrainPyramidZTolerance:
                    // <TerrainPyramidLevelZTols>
                    //    <TerrainPyramidLevelZTol>
                    //    </TerrainPyramidLevelZTol>
                    // </TerrainPyramidLevelZTols>
                    pyramidPath = "TerrainPyramidLevelZTols/TerrainPyramidLevelZTol";
                    break;
            }
            XPathNodeIterator interatorTerrainPyramid = navigator.Select(pyramidPath);
            while (interatorTerrainPyramid.MoveNext()) {
                // Get <ConnectivityRule>
                XPathNavigator navigatorTerrainPyramid = interatorTerrainPyramid.Current;
                TerrainPyramid terrainPyramid = new TerrainPyramid(navigatorTerrainPyramid);
                this._terrainPyramids.Add(terrainPyramid);
            }
            
            // <TerrainID></TerrainID>
            XPathNavigator navigatorTerrainID = navigator.SelectSingleNode("TerrainID");
            if (navigatorTerrainID != null) {
                this._terrainID = navigatorTerrainID.ValueAsInt;
            }

            // <Version></Version>
            XPathNavigator navigatorVersion = navigator.SelectSingleNode("Version");
            if (navigatorVersion != null) {
                this._version = navigatorVersion.ValueAsInt;
            }
        }
        public Terrain(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._configurationKeyword = info.GetString("configurationKeyword");
            this._extent = info.GetValue("extent", typeof(Extent)) as Extent;
            this._spatialReference = info.GetValue("spatialReference", typeof(SpatialReference)) as SpatialReference;
            this._pyramidType = (esriTerrainPyramidType)Enum.Parse(typeof(esriTerrainPyramidType), info.GetString("pyramidType"), true);
            this._windowSizeMethod = (esriTerrainWindowSizeMethod)Enum.Parse(typeof(esriTerrainWindowSizeMethod), info.GetString("windowSizeMethod"), true);
            this._windowSizeZThreshold = info.GetDouble("windowSizeZThreshold");
            this._windowSizeZThresholdStrategy = (esriTerrainZThresholdStrategy)Enum.Parse(typeof(esriTerrainZThresholdStrategy), info.GetString("windowSizeZThresholdStrategy"), true);
            this._tileSize = info.GetDouble("tileSize");
            this._maxShapeSize = info.GetInt32("maxShapeSize");
            this._maxOverviewSize = info.GetInt32("maxOverviewSize");
            this._extentDomain = info.GetValue("extentDomain", typeof(Extent)) as Extent;
            this._extentAOI = info.GetValue("extentAOI", typeof(Extent)) as Extent;
            this._terrainDataSources = (List<TerrainDataSource>)info.GetValue("terrainDataSources", typeof(List<TerrainDataSource>));
            this._terrainPyramids = (List<TerrainPyramid>)info.GetValue("terrainPyramids", typeof(List<TerrainPyramid>));
            this._terrainID = info.GetInt32("terrainID");
            this._version = info.GetInt32("version");
        }
        public Terrain(Terrain prototype) : base(prototype) {
            this._configurationKeyword = prototype.ConfigurationKeyword;
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._pyramidType = prototype.PyramidType;
            this._windowSizeMethod = prototype.WindowSizeMethod;
            this._windowSizeZThreshold = prototype.WindowSizeZThreshold;
            this._windowSizeZThresholdStrategy = prototype.WindowSizeZThresholdStrategy;
            this._tileSize = prototype.TileSize;
            this._maxShapeSize = prototype.MaxShapeSize;
            this._maxOverviewSize = prototype.MaxOverviewSize;
            if (prototype.ExtentDomain != null) {
                this._extentDomain = prototype.ExtentDomain.Clone() as Extent;
            }
            if (prototype.ExtentAOI != null) {
                this._extentAOI = prototype.ExtentAOI.Clone() as Extent;
            }
            this._terrainDataSources = new List<TerrainDataSource>();
            foreach (TerrainDataSource terrainDataSource in prototype.TerrainDataSources) {
                this._terrainDataSources.Add((TerrainDataSource)terrainDataSource.Clone());
            }
            this._terrainPyramids = new List<TerrainPyramid>();
            foreach (TerrainPyramid terrainPyramid in prototype.TerrainPyramids) {
                this._terrainPyramids.Add((TerrainPyramid)terrainPyramid.Clone());
            }
            this._terrainID = prototype.TerrainID;
            this._version = prototype.Version;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The storage parameter used with SDE databases.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue("")]
        [Description("The storage parameter used with SDE databases.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ConfigurationKeyword {
            get { return this._configurationKeyword; }
            set { this._configurationKeyword = value; }
        }
        /// <summary>
        /// The approximate xyz extent of the terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(null)]
        [Description("The approximate xyz extent of the terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        /// <summary>
        /// The coordinate system and xyz domain of the terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(null)]
        [Description("The coordinate system and xyz domain of the terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// The kind of the pyramid as defined by the type of filter it uses to thin points.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(esriTerrainPyramidType.esriTerrainPyramidWindowSize)]
        [Description("The kind of the pyramid as defined by the type of filter it uses to thin points.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriTerrainPyramidType PyramidType {
            get { return this._pyramidType; }
            set { this._pyramidType = value; }
        }
        /// <summary>
        /// The method used by the windowsize filter to select points.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(esriTerrainWindowSizeMethod.esriTerrainWindowSizeZmin)]
        [Description("The method used by the windowsize filter to select points.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriTerrainWindowSizeMethod WindowSizeMethod {
            get { return this._windowSizeMethod; }
            set { this._windowSizeMethod = value; }
        }
        /// <summary>
        /// The maximum vertical displacement property associated with the secondary thinning filter.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(-1)]
        [Description("The maximum vertical displacement property associated with the secondary thinning filter.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double WindowSizeZThreshold {
            get { return this._windowSizeZThreshold; }
            set { this._windowSizeZThreshold = value; }
        }
        /// <summary>
        /// Controls how liberal secondary thinning is permitted to be.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(esriTerrainZThresholdStrategy.esriTerrainZThresholdMildThinning)]
        [Description("Controls how liberal secondary thinning is permitted to be.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriTerrainZThresholdStrategy WindowSizeZThresholdStrategy {
            get { return this._windowSizeZThresholdStrategy; }
            set { this._windowSizeZThresholdStrategy = value; }
        }
        /// <summary>
        /// The horizontal distance used to spatially index and partition terrain data.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(0)]
        [Description("The horizontal distance used to spatially index and partition terrain data.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double TileSize {
            get { return this._tileSize; }
            set { this._tileSize = value; }
        }
        /// <summary>
        /// The maximum number of vertices per multipoint stored in the terrain pyramid.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(0)]
        [Description("The maximum number of vertices per multipoint stored in the terrain pyramid.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int MaxShapeSize {
            get { return this._maxShapeSize; }
            set { this._maxShapeSize = value; }
        }
        /// <summary>
        /// The maximum number of points in the most generalized representation of a Terrain.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(0)]
        [Description("The maximum number of points in the most generalized representation of a Terrain.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int MaxOverviewSize {
            get { return this._maxOverviewSize; }
            set { this._maxOverviewSize = value; }
        }
        /// <summary>
        /// Extent Domain
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(null)]
        [Description("Extent Domain")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent ExtentDomain {
            get { return this._extentDomain; }
            set { this._extentDomain = value; }
        }
        /// <summary>
        /// Extent AOI
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(null)]
        [Description("Extent AOI")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent ExtentAOI {
            get { return this._extentAOI; }
            set { this._extentAOI = value; }
        }
        /// <summary>
        /// Reference feature classes.
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(-1)]
        [Description("Reference feature classes.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<TerrainDataSource> TerrainDataSources {
            get { return this._terrainDataSources; }
        }
        /// <summary>
        /// Pyramid Levels
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(-1)]
        [Description("Pyramid Levels")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<TerrainPyramid> TerrainPyramids {
            get { return this._terrainPyramids; }
        }
        /// <summary>
        /// Terrain ID
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(-1)]
        [Description("Terrain ID")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int TerrainID {
            get { return this._terrainID; }
            set { this._terrainID = value; }
        }
        /// <summary>
        /// Version
        /// </summary>
        [Browsable(true)]
        [Category("Terrain")]
        [DefaultValue(0)]
        [Description("Version")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Version {
            get { return this._version; }
            set { this._version = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/TERR=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("configurationKeyword", this._configurationKeyword);
            info.AddValue("extent", this._extent, typeof(Extent));
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));
            info.AddValue("pyramidType", this._pyramidType.ToString("d"));
            info.AddValue("windowSizeMethod", this._windowSizeMethod.ToString("d"));
            info.AddValue("windowSizeZThreshold", this._windowSizeZThreshold);
            info.AddValue("windowSizeZThresholdStrategy", this._windowSizeZThresholdStrategy.ToString("d"));
            info.AddValue("tileSize", this._tileSize);
            info.AddValue("maxShapeSize", this._maxShapeSize);
            info.AddValue("maxOverviewSize", this._maxOverviewSize);
            info.AddValue("extentDomain", this._extentDomain, typeof(Extent));
            info.AddValue("extentAOI", this._extentAOI, typeof(Extent));
            info.AddValue("terrainDataSources", this._terrainDataSources, typeof(List<TerrainDataSource>));
            info.AddValue("terrainPyramids", this._terrainPyramids, typeof(List<TerrainPyramid>));
            info.AddValue("terrainID", this._terrainID);
            info.AddValue("version", this._version);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Terrain(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DETERRAIN);

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
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
            this.GradientColor = ColorSettings.Default.TerrainColor;
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            //<ConfigurationKeyword>TERRAIN_DEFAULTS</ConfigurationKeyword>
            writer.WriteStartElement("ConfigurationKeyword");
            writer.WriteValue(this._configurationKeyword);
            writer.WriteEndElement();

            //<Extent xsi:type="esri:EnvelopeN"></Extent>
            if (this._extent != null) {
                this._extent.WriteXml(writer);
            }

            //<SpatialReference xsi:type="esri:ProjectedCoordinateSystem"></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            //<FeatureDatasetName>topography</FeatureDatasetName> 
            Dataset dataset = this.GetParent();
            if (dataset != null && dataset.GetType() == typeof(FeatureDataset)) {
                FeatureDataset featureDataset = (FeatureDataset)dataset;
                writer.WriteStartElement("FeatureDatasetName");
                writer.WriteValue(featureDataset.Name);
                writer.WriteEndElement();
            }

            //<PyramidType>0</PyramidType> 
            writer.WriteStartElement("PyramidType");
            writer.WriteValue(this._pyramidType.ToString("d"));
            writer.WriteEndElement();

            //<WindowSizeMethod>0</WindowSizeMethod> 
            writer.WriteStartElement("WindowSizeMethod");
            writer.WriteValue(this._windowSizeMethod.ToString("d"));
            writer.WriteEndElement();

            //<WindowSizeZThreshold>0</WindowSizeZThreshold> 
            writer.WriteStartElement("WindowSizeZThreshold");
            writer.WriteValue(this._windowSizeZThreshold);
            writer.WriteEndElement();

            //<WindowSizeZThresholdStrategy>0</WindowSizeZThresholdStrategy> 
            writer.WriteStartElement("WindowSizeZThresholdStrategy");
            writer.WriteValue(this._windowSizeZThresholdStrategy.ToString("d"));
            writer.WriteEndElement();

            //<TileSize>447</TileSize> 
            writer.WriteStartElement("TileSize");
            writer.WriteValue(this._tileSize);
            writer.WriteEndElement();

            //<MaxShapeSize>3500</MaxShapeSize> 
            writer.WriteStartElement("MaxShapeSize");
            writer.WriteValue(this._maxShapeSize);
            writer.WriteEndElement();

            //<MaxOverviewSize>50000</MaxOverviewSize> 
            writer.WriteStartElement("MaxOverviewSize");
            writer.WriteValue(this._maxOverviewSize);
            writer.WriteEndElement();

            //<ExtentDomain xsi:type="esri:EnvelopeN"></ExtentDomain>
            if (this._extentDomain != null) {
                this._extentDomain.WriteXml(writer);
            }

            //<ExtentAOI xsi:nil="true" />
            if (this._extentAOI != null) {
                this._extentAOI.WriteXml(writer);
            }

            //<TerrainDataSources xsi:type="esri:ArrayOfTerrainDataSource">
            //   <TerrainDataSource xsi:type="esri:TerrainDataSource">
            //   </TerrainDataSource>
            //</TerrainDataSources>
            writer.WriteStartElement(Xml.TERRAINDATASOURCES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFTERRAINDATASOURCE);
            foreach (TerrainDataSource terrainDataSource in this._terrainDataSources) {
                terrainDataSource.WriteXml(writer);
            }          
            writer.WriteEndElement();

            // Terrain Pyramids
            switch (this._pyramidType) {
                case esriTerrainPyramidType.esriTerrainPyramidWindowSize:
                    //<TerrainPyramidLevelWindowSizes xsi:type="esri:ArrayOfTerrainPyramidLevelWindowSize">
                    //</TerrainPyramidLevelWindowSizes>
                    writer.WriteStartElement(Xml.TERRAINPYRAMIDLEVELWINDOWSIZES);
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFTERRAINPYRAMIDLEVELWINDOWSIZE);
                    foreach (TerrainPyramid terrainPyramid in this._terrainPyramids) {
                        terrainPyramid.WriteXml(writer, this._pyramidType);
                    }
                    writer.WriteEndElement();
                    break;
                case esriTerrainPyramidType.esriTerrainPyramidZTolerance:
                    //<TerrainPyramidLevelZTols xsi:type="esri:ArrayOfTerrainPyramidLevelZTol">
                    //</TerrainPyramidLevelZTols>
                    writer.WriteStartElement(Xml.TERRAINPYRAMIDLEVELZTOLS);
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFTERRAINPYRAMIDLEVELZTOL);
                    foreach (TerrainPyramid terrainPyramid in this._terrainPyramids) {
                        terrainPyramid.WriteXml(writer, this._pyramidType);
                    }
                    writer.WriteEndElement();
                    break;
            }

            //<TerrainID>1</TerrainID> 
            writer.WriteStartElement("TerrainID");
            writer.WriteValue(this._terrainID);
            writer.WriteEndElement();

            //<Version>13</Version> 
            writer.WriteStartElement("Version");
            writer.WriteValue(this._version);
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.DrawExpand = false;
            this.GradientColor = ColorSettings.Default.TerrainColor;
            this.SubHeading = Resources.TEXT_TERRAIN;
        }
    }
}

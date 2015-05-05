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
using ESRI.ArcGIS.GeoDatabaseExtensions;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Terrain Pyramid Level Window Size
    /// </summary>
    /// <remarks>
    /// The vertical deviation permitted in one level of a terrain pyramid tile relative to the full resolution data.
    /// </remarks>
    [Serializable]
    public class TerrainPyramid : EsriObject {
        private TerrainElementStatus _pyramidLevelStatus = TerrainElementStatus.Resident;
        private int _pointCount = -1;
        private int _maxScale = 0;
        private double _resolution = 0d;
        //
        // CONSTRUCTOR
        //
        public TerrainPyramid() : base() { }
        public TerrainPyramid(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            //<PyramidLevelStatus>1</PyramidLevelStatus> 
            XPathNavigator navigatorPyramidLevelStatus = navigator.SelectSingleNode("PyramidLevelStatus");
            if (navigatorPyramidLevelStatus != null) {
                this._pyramidLevelStatus = (TerrainElementStatus)Enum.Parse(typeof(TerrainElementStatus), navigatorPyramidLevelStatus.Value, true);
            }

            //<PointCount>-1</PointCount> 
            XPathNavigator navigatorPointCount = navigator.SelectSingleNode("PointCount");
            if (navigatorPointCount != null) {
                this._pointCount = navigatorPointCount.ValueAsInt;
            }

            //<MaxScale>1000</MaxScale> 
            XPathNavigator navigatorMaxScale = navigator.SelectSingleNode("MaxScale");
            if (navigatorMaxScale != null) {
                this._maxScale = navigatorMaxScale.ValueAsInt;
            }

            //<Resolution>2</Resolution> 
            XPathNavigator navigatorSourceStatus = navigator.SelectSingleNode("Resolution");
            if (navigatorSourceStatus != null) {
                this._resolution = navigatorSourceStatus.ValueAsDouble;
            }
        }
        public TerrainPyramid(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._pyramidLevelStatus = (TerrainElementStatus)Enum.Parse(typeof(TerrainElementStatus), info.GetString("pyramidLevelStatus"), true);
            this._pointCount = info.GetInt32("pointCount");
            this._maxScale = info.GetInt32("maxScale");
            this._resolution = info.GetDouble("resolution");
        }
        public TerrainPyramid(TerrainPyramid prototype) : base(prototype) {
            this._pyramidLevelStatus = prototype.PyramidLevelStatus;
            this._pointCount = prototype.PointCount;
            this._maxScale = prototype.MaxScale;
            this._resolution = prototype.Resolution;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Pyramid Level Status
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Pyramid")]
        [DefaultValue(TerrainElementStatus.Resident)]
        [Description("Pyramid Level Status")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TerrainElementStatus PyramidLevelStatus {
            get { return this._pyramidLevelStatus; }
            set { this._pyramidLevelStatus = value; }
        }
        /// <summary>
        /// Point Count
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Pyramid")]
        [DefaultValue(-1)]
        [Description("Point Count")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int PointCount {
            get { return this._pointCount; }
            set { this._pointCount = value; }
        }
        /// <summary>
        /// Maximum Scale
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Pyramid")]
        [DefaultValue("0")]
        [Description("Maximum Scale")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int MaxScale {
            get { return this._maxScale; }
            set { this._maxScale = value; }
        }
        /// <summary>
        /// Resolution
        /// </summary>
        [Browsable(true)]
        [Category("Terrain Pyramid")]
        [DefaultValue(0d)]
        [Description("Resolution")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double Resolution {
            get { return this._resolution; }
            set { this._resolution = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("pyramidLevelStatus", this._pyramidLevelStatus.ToString("d"));
            info.AddValue("pointCount", this._pointCount);
            info.AddValue("maxScale", this._maxScale);
            info.AddValue("resolution", this._resolution);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Terrain Pyramid Errors
        }
        public override object Clone() {
            return new TerrainPyramid(this);
        }
        public void WriteXml(XmlWriter writer, esriTerrainPyramidType pyramidType) {
            switch (pyramidType) {
                case esriTerrainPyramidType.esriTerrainPyramidWindowSize:
                    //  <TerrainPyramidLevelWindowSize xsi:type="esri:TerrainPyramidLevelWindowSize">
                    //  </TerrainPyramidLevelWindowSize>
                    writer.WriteStartElement(Xml.TERRAINPYRAMIDLEVELWINDOWSIZE);
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._TERRAINPYRAMIDLEVELWINDOWSIZE);
                    this.WriteInnerXml(writer);
                    writer.WriteEndElement();
                    break;
                case esriTerrainPyramidType.esriTerrainPyramidZTolerance:
                    // <TerrainPyramidLevelZTol xsi:type="esri:TerrainPyramidLevelZTol">
                    // </TerrainPyramidLevelZTol>
                    writer.WriteStartElement(Xml.TERRAINPYRAMIDLEVELZTOL);
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._TERRAINPYRAMIDLEVELZTOL);
                    this.WriteInnerXml(writer);
                    writer.WriteEndElement();
                    break;
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // Write Xml (use "WindowSize" as the default pyramid type)
            this.WriteXml(writer, esriTerrainPyramidType.esriTerrainPyramidWindowSize);
        }
        public override string ToString() {
            return string.Format("{0}", this._resolution);
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            //<PyramidLevelStatus>1</PyramidLevelStatus> 
            writer.WriteStartElement("PyramidLevelStatus");
            writer.WriteValue(this._pyramidLevelStatus.ToString("d"));
            writer.WriteEndElement();

            //<PointCount>-1</PointCount> 
            writer.WriteStartElement("PointCount");
            writer.WriteValue(this._pointCount);
            writer.WriteEndElement();

            //<MaxScale>2500</MaxScale> 
            writer.WriteStartElement("MaxScale");
            writer.WriteValue(this._maxScale);
            writer.WriteEndElement();

            //<Resolution>4</Resolution> 
            writer.WriteStartElement("Resolution");
            writer.WriteValue(this._resolution);
            writer.WriteEndElement();
        }
    }
}

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
    /// ESRI Storage Definition
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class StorageDef : EsriObject {
        private int _compressionQuality = 0;
        private esriRasterCompressionType _compressionType = esriRasterCompressionType.esriRasterCompressionUnknown;
        private int _pyramidLevel = 0;
        private rstResamplingTypes _pyramidResampleType = rstResamplingTypes.RSP_BilinearInterpolation;
        private bool _tiled = false;
        private int _tileHeight = 0;
        private int _tileWidth = 0;
        private Point _origin = null;
        private double _cellSizeX = 0d;
        private double _cellSizeY = 0d;
        //
        // CONSTRUCTOR
        //
        public StorageDef() : base() {
            this._origin = new Point();
        }
        public StorageDef(IXPathNavigable path): base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <CompressionQuality></CompressionQuality>
            XPathNavigator navigatorCompressionQuality = navigator.SelectSingleNode("CompressionQuality");
            if (navigatorCompressionQuality != null) {
                this._compressionQuality = navigatorCompressionQuality.ValueAsInt;
            }

            // <CompressionType></CompressionType>
            XPathNavigator navigatorCompressionType = navigator.SelectSingleNode("CompressionType");
            if (navigatorCompressionType != null) {
                this._compressionType = (esriRasterCompressionType)Enum.Parse(typeof(esriRasterCompressionType), navigatorCompressionType.Value, true);
            }

            // <PyramidLevel></PyramidLevel>
            XPathNavigator navigatorPyramidLevel = navigator.SelectSingleNode("PyramidLevel");
            if (navigatorPyramidLevel != null) {
                this._pyramidLevel = navigatorPyramidLevel.ValueAsInt;
            }

            // <PyramidResampleType></PyramidResampleType>
            XPathNavigator navigatorPyramidResampleType = navigator.SelectSingleNode("PyramidResampleType");
            if (navigatorPyramidResampleType != null) {
                this._pyramidResampleType = (rstResamplingTypes)Enum.Parse(typeof(rstResamplingTypes), navigatorPyramidResampleType.Value, true);
            }

            // <Tiled></Tiled>
            XPathNavigator navigatorTiled = navigator.SelectSingleNode("Tiled");
            if (navigatorTiled != null) {
                this._tiled = navigatorTiled.ValueAsBoolean;
            }

            // <TileHeight></TileHeight>
            XPathNavigator navigatorTileHeight = navigator.SelectSingleNode("TileHeight");
            if (navigatorTileHeight != null) {
                this._tileHeight = navigatorTileHeight.ValueAsInt;
            }

            // <TileWidth></TileWidth>
            XPathNavigator navigatorTileWidth = navigator.SelectSingleNode("TileWidth");
            if (navigatorTileWidth != null) {
                this._tileWidth = navigatorTileWidth.ValueAsInt;
            }

            // <Origin></Origin>
            XPathNavigator navigatorOrigin = navigator.SelectSingleNode("Origin");
            if (navigatorOrigin != null) {
                this._origin = new Point(navigatorOrigin);
            }
            else {
                this._origin = new Point();
                this._origin.X = -1d;
                this._origin.Y = -1d;
            }

            // <CellSizeX></CellSizeX>
            XPathNavigator navigatorCellSizeX = navigator.SelectSingleNode("CellSizeX");
            if (navigatorCellSizeX != null) {
                this._cellSizeX = navigatorCellSizeX.ValueAsDouble;
            }
            else {
                this._cellSizeX = -1d;
            }

            // <CellSizeY></CellSizeY>
            XPathNavigator navigatorCellSizeY = navigator.SelectSingleNode("CellSizeY");
            if (navigatorCellSizeY != null) {
                this._cellSizeY = navigatorCellSizeY.ValueAsDouble;
            }
            else {
                this._cellSizeY = -1d;
            }
        }
        public StorageDef(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._compressionQuality = info.GetInt32("compressionQuality");
            this._compressionType = (esriRasterCompressionType)Enum.Parse(typeof(esriRasterCompressionType), info.GetString("compressionType"), true);
            this._pyramidLevel = info.GetInt32("pyramidLevel");
            this._pyramidResampleType = (rstResamplingTypes)Enum.Parse(typeof(rstResamplingTypes), info.GetString("pyramidResampleType"), true);
            this._tiled = info.GetBoolean("tiled");
            this._tileHeight = info.GetInt32("tileHeight");
            this._tileWidth = info.GetInt32("tileWidth");
            this._origin = (Point)info.GetValue("origin", typeof(Point));
            this._cellSizeX = info.GetDouble("cellSizeX");
            this._cellSizeY = info.GetDouble("cellSizeY");
        }
        public StorageDef(StorageDef prototype) : base(prototype) {
            this._compressionQuality = prototype.CompressionQuality;
            this._compressionType = prototype.CompressionType;
            this._pyramidLevel = prototype.PyramidLevel;
            this._pyramidResampleType = prototype.PyramidResampleType;
            this._tiled = prototype.Tiled;
            this._tileHeight = prototype.TileHeight;
            this._tileWidth = prototype.TileWidth;
            this._origin = (Point)prototype.Origin.Clone();
            this._cellSizeX = prototype.CellSizeX;
            this._cellSizeY = prototype.CellSizeY;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The compression quality (in case of JPEG compression) to be applied to the raster being stored
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(75)]
        [Description("The compression quality (in case of JPEG compression) to be applied to the raster being stored")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int CompressionQuality {
            get { return this._compressionQuality; }
            set { this._compressionQuality = value; }
        }
        /// <summary>
        /// The compression type to be applied on the raster being stored
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(esriRasterCompressionType.esriRasterCompressionLZ77)]
        [Description("The compression type to be applied on the raster being stored")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRasterCompressionType CompressionType {
            get { return this._compressionType; }
            set { this._compressionType = value; }
        }
        /// <summary>
        /// The number of pyramid levels
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(-1)]
        [Description("The number of pyramid levels")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int PyramidLevel {
            get { return this._pyramidLevel; }
            set { this._pyramidLevel = value; }
        }
        /// <summary>
        /// The method used for pyramid resampling
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(rstResamplingTypes.RSP_NearestNeighbor)]
        [Description("The method used for pyramid resampling")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public rstResamplingTypes PyramidResampleType {
            get { return this._pyramidResampleType; }
            set { this._pyramidResampleType = value; }
        }
        /// <summary>
        /// Indicates if output raster dataset should be tiled if applicable
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(false)]
        [Description("Indicates if output raster dataset should be tiled if applicable")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Tiled {
            get { return this._tiled; }
            set { this._tiled = value; }
        }
        /// <summary>
        /// The raster storage tile height
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(128)]
        [Description("The raster storage tile height")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int TileHeight {
            get { return this._tileHeight; }
            set { this._tileHeight = value; }
        }
        /// <summary>
        /// The raster storage tile width
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(128)]
        [Description("The raster storage tile width")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int TileWidth {
            get { return this._tileWidth; }
            set { this._tileWidth = value; }
        }
        /// <summary>
        /// The origin of the raster to be stored
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(null)]
        [Description("The origin of the raster to be stored")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Point Origin {
            get { return this._origin; }
            set { this._origin = value; }
        }
        /// <summary>
        /// The cell width of the raster to be stored
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(-1d)]
        [Description("The cell width of the raster to be stored")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double CellSizeX {
            get { return this._cellSizeX; }
            set { this._cellSizeX = value; }
        }
        /// <summary>
        /// The cell height of the raster to be stored
        /// </summary>
        [Browsable(true)]
        [Category("Storage Definition")]
        [DefaultValue(-1d)]
        [Description("The cell height of the raster to be stored")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double CellSizeY {
            get { return this._cellSizeY; }
            set { this._cellSizeY = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("compressionQuality", this._compressionQuality);
            info.AddValue("compressionType", Convert.ToInt32(this._compressionType).ToString());
            info.AddValue("pyramidLevel", this._pyramidLevel);
            info.AddValue("pyramidResampleType", Convert.ToInt32(this._pyramidResampleType).ToString());
            info.AddValue("tiled", this._tiled);
            info.AddValue("tileHeight", this._tileHeight);
            info.AddValue("tileWidth", this._tileWidth);
            info.AddValue("origin", this._origin, typeof(Point));
            info.AddValue("cellSizeX", this._cellSizeX);
            info.AddValue("cellSizeY", this._cellSizeY);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new StorageDef(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <StorageDef>
            writer.WriteStartElement("StorageDef");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:RasterStorageDef");

            // Writer Inner XML
            this.WriteInnerXml(writer);

            // </StorageDef>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Add RasterDef Errors
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Call base method
            base.WriteInnerXml(writer);

            // <CompressionQuality></CompressionQuality>
            writer.WriteStartElement("CompressionQuality");
            writer.WriteValue(this._compressionQuality);
            writer.WriteEndElement();

            // <CompressionType></CompressionType>
            writer.WriteStartElement("CompressionType");
            writer.WriteValue(this._compressionType.ToString());
            writer.WriteEndElement();

            // <PyramidLevel></PyramidLevel>
            writer.WriteStartElement("PyramidLevel");
            writer.WriteValue(this._pyramidLevel);
            writer.WriteEndElement();

            // <PyramidResampleType></PyramidResampleType>
            writer.WriteStartElement("PyramidResampleType");
            writer.WriteValue(this._pyramidResampleType.ToString());
            writer.WriteEndElement();

            // <Tiled></Tiled>
            writer.WriteStartElement("Tiled");
            writer.WriteValue(this._tiled);
            writer.WriteEndElement();

            // <TileHeight></TileHeight>
            writer.WriteStartElement("TileHeight");
            writer.WriteValue(this._tileHeight);
            writer.WriteEndElement();

            // <TileWidth></TileWidth>
            writer.WriteStartElement("TileWidth");
            writer.WriteValue(this._tileWidth);
            writer.WriteEndElement();

            // <Origin></Origin>
            if (this._origin != null &&
                this._origin.X != -1d ||
                this._origin.Y != -1d) {
                // Do Not Write Spatial Reference
                this._origin.WriteXml(writer, false);
            }

            // <CellSizeX></CellSizeX>
            // <CellSizeY></CellSizeY>
            if (this._cellSizeX != -1d &&
                this._cellSizeY != -1d) {
                // <CellSizeX></CellSizeX>
                writer.WriteStartElement("CellSizeX");
                writer.WriteValue(this._cellSizeX);
                writer.WriteEndElement();

                // <CellSizeY></CellSizeY>
                writer.WriteStartElement("CellSizeY");
                writer.WriteValue(this._cellSizeY);
                writer.WriteEndElement();
            }

        }
    }
}

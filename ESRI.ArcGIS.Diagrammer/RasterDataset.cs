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
using Crainiate.Diagramming;
using Crainiate.ERM4.Navigation;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Raster Dataset
    /// </summary>
    [Serializable]
    public class RasterDataset : Dataset {
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        private string _format = string.Empty;
        private string _compressionType = string.Empty;
        private string _sensorType = string.Empty;
        private bool _permanent = false;
        private StorageDef _storageDef = null;
        //
        // CONSTRUCTOR
        //
        public RasterDataset(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

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
            this._spatialReference = new SpatialReference(navigatorSpatialReference);
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
            else {
                this._spatialReference = new SpatialReference();
            }

            // <Format>
            XPathNavigator navigatorFormat = navigator.SelectSingleNode("Format");
            if (navigatorFormat != null) {
                this._format = navigatorFormat.Value;
            }

            // <CompressionType>
            XPathNavigator navigatorCompressionType = navigator.SelectSingleNode("CompressionType");
            if (navigatorCompressionType != null) {
                this._compressionType = navigatorCompressionType.Value;
            }

            // <SensorType>
            XPathNavigator navigatorSensorType = navigator.SelectSingleNode("SensorType");
            if (navigatorSensorType != null) {
                this._sensorType = navigatorSensorType.Value;
            }

            // <Permanent>
            XPathNavigator navigatorPermanent = navigator.SelectSingleNode("Permanent");
            if (navigatorPermanent != null) {
                this._permanent = navigatorPermanent.ValueAsBoolean;
            }

            // <StorageDef>
            XPathNavigator navigatorStorageDef = navigator.SelectSingleNode("StorageDef");
            if (navigatorStorageDef != null) {
                this._storageDef = new StorageDef(navigatorStorageDef);
            }
            else {
                this._storageDef = new StorageDef();
            }
        }
        public RasterDataset(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._extent = (Extent)info.GetValue("extent", typeof(Extent));
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
            this._format = info.GetString("format");
            this._compressionType = info.GetString("compressionType");
            this._sensorType = info.GetString("sensorType");
            this._permanent = info.GetBoolean("permanent");
            this._storageDef = (StorageDef)info.GetValue("storageDef", typeof(StorageDef));
        }
        public RasterDataset(RasterDataset prototype) : base(prototype) {
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._format = prototype.Format;
            this._compressionType = prototype.CompressionType;
            this._sensorType = prototype.SensorType;
            this._permanent = prototype.Permanent;
            if (prototype.StorageDef != null) {
                this._storageDef = prototype.StorageDef.Clone() as StorageDef;
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The extent of the GeoDataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
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
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("The spatial reference of the GeoDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// The format of this RasterRataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("The format of this RasterRataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FormatConverter))]
        public string Format {
            get { return this._format; }
            set { this._format = value; }
        }
        /// <summary>
        /// The compression technique applied to this RasterDataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("The compression technique applied to this RasterDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(CompressionConverter))]
        public string CompressionType {
            get { return this._compressionType; }
            set { this._compressionType = value; }
        }
        /// <summary>
        /// The sensor type used for this RasterDataset
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("The sensor type used for this RasterDataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SensorConverter))]
        public string SensorType {
            get { return this._sensorType; }
            set { this._sensorType = value; }
        }
        /// <summary>
        /// Indicates if the dataset is temporary
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("Indicates if the dataset is temporary")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Permanent {
            get { return this._permanent; }
            set { this._permanent = value; }
        }
        /// <summary>
        /// Raster Value Storage Definition Class
        /// </summary>
        [Browsable(true)]
        [Category("Raster Dataset")]
        [DefaultValue(null)]
        [Description("Raster Value Storage Definition Class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public StorageDef StorageDef {
            get { return this._storageDef; }
            set { this._storageDef = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/RD=" + this.Name;
        }
        public override List<Dataset> GetChildren() {
            // Create Dataset List
            List<Dataset> datasets = new List<Dataset>();

            // Get Model
            EsriModel model = (EsriModel)base.Container;

            // Create Navigation
            Navigate navigate = model.Navigate;
            navigate.Start = this;
            Elements elements = navigate.Children();

            // Export Children
            foreach (Element element in elements.Values) {
                if (element is Dataset) {
                    Dataset dataset = (Dataset)element;
                    if (dataset.GetType() == typeof(RasterBand)) {
                        datasets.Add(dataset);
                    }
                }
            }

            // Return List
            return datasets;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("extent", this._extent, typeof(Extent));
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));
            info.AddValue("format", this._format);
            info.AddValue("compressionType", this._compressionType);
            info.AddValue("sensorType", this._sensorType);
            info.AddValue("permanent", this._permanent);
            info.AddValue("storageDef", this._storageDef, typeof(StorageDef));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RasterDataset(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DERASTERDATASET);

            // Writer Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Add Base Errors
            base.Errors(list);

            // Must have at least one RasterBand
            List<Dataset> datasets = this.GetChildren();
            if (datasets.Count == 0) {
                list.Add(new ErrorTable(this, "Raster Datasets must have at least one Raster Band", ErrorType.Error));
            }

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

            if (this._storageDef == null) {
                list.Add(new ErrorTable(this, "StorageDef cannot be null", ErrorType.Error));
            }
            else {
                this._storageDef.Errors(list, this);
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.RasterDatasetColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Inner Xml
            base.WriteInnerXml(writer);

            // <Extent>
            if (this._extent != null) {
                this._extent.WriteXml(writer);
            }

            // <SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            // <Format>
            writer.WriteStartElement("Format");
            writer.WriteValue(this._format);
            writer.WriteEndElement();

            // <CompressionType>
            writer.WriteStartElement("CompressionType");
            writer.WriteValue(this._compressionType);
            writer.WriteEndElement();

            // <SensorType >
            writer.WriteStartElement("SensorType");
            writer.WriteValue(this._sensorType);
            writer.WriteEndElement();

            // <BandCount>
            List<Dataset> datasets = this.GetChildren();
            writer.WriteStartElement("BandCount");
            writer.WriteValue(datasets.Count);
            writer.WriteEndElement();

            // <Permanent>
            writer.WriteStartElement("Permanent");
            writer.WriteValue(this._permanent);
            writer.WriteEndElement();

            // <StorageDef>
            if (this._storageDef != null) {
                this._storageDef.WriteXml(writer);
            }
        }
        protected override void Initialize() {
            this.DrawExpand = false;
            this.GradientColor = ColorSettings.Default.RasterDatasetColor;
            this.SubHeading = Resources.TEXT_RASTER_DATASET;
        }
    }
}

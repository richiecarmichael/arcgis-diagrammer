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

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Topology
    /// </summary>
    [Serializable]
    public class Topology : Dataset {
        private const string EXTENT_ = "extent";
        private const string SPATIALREFERENCE_ = "spatialReference";
        private const string CLUSTERTOLERANCE_ = "clusterTolerance";
        private const string ZCLUSTERTOLERANCE_ = "zClusterTolerance";
        private const string MAXGENERATEDERRORCOUNT_ = "maxGeneratedErrorCount";
        private const string TOPOLOGYRULES_ = "topologyRules";
        //
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        private double _clusterTolerance = -1d;
        private double _zClusterTolerance = -1d;
        private int _maxGeneratedErrorCount = -1;
        private List<TopologyRule> _topologyRules = null;
        //
        // CONSTRUCTOR
        //
        public Topology(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Extent></Extent>
            XPathNavigator navigatorExtent = navigator.SelectSingleNode(Xml.EXTENT);
            if (navigatorExtent != null) {
                this._extent = new Extent(navigatorExtent);
            }
            else {
                this._extent = new Extent();
            }

            // <SpatialReference></SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode(Xml.SPATIALREFERENCE);
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
            else {
                this._spatialReference = new SpatialReference();
            }

            // <ClusterTolerance></ClusterTolerance>
            XPathNavigator navigatorClusterTolerance = navigator.SelectSingleNode(Xml.CLUSTERTOLERANCE);
            if (navigatorClusterTolerance != null) {
                this._clusterTolerance = navigatorClusterTolerance.ValueAsDouble;
            }

            // <ZClusterTolerance></ZClusterTolerance>
            XPathNavigator navigatorZClusterTolerance = navigator.SelectSingleNode(Xml.ZCLUSTERTOLERANCE);
            if (navigatorZClusterTolerance != null) {
                this._zClusterTolerance = navigatorZClusterTolerance.ValueAsDouble;
            }

            // <MaxGeneratedErrorCount></MaxGeneratedErrorCount>
            XPathNavigator navigatorMaxGeneratedErrorCount = navigator.SelectSingleNode(Xml.MAXGENERATEDERRORCOUNT);
            if (navigatorMaxGeneratedErrorCount != null) {
                this._maxGeneratedErrorCount = navigatorMaxGeneratedErrorCount.ValueAsInt;
            }

            // <TopologyRules><TopologyRule></TopologyRule></TopologyRules>
            this._topologyRules = new List<TopologyRule>();
            XPathNodeIterator interatorTopologyRule = navigator.Select(string.Format("{0}/{1}", Xml.TOPOLOGYRULES, Xml.TOPOLOGYRULE));
            while (interatorTopologyRule.MoveNext()) {
                // Get <TopologyRule>
                XPathNavigator navigatorTopologyRule = interatorTopologyRule.Current;

                // Add Topology Rule
                TopologyRule topologyRule = new TopologyRule(navigatorTopologyRule);
                this._topologyRules.Add(topologyRule);
            }
        }
        public Topology(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._extent = (Extent)info.GetValue(Topology.EXTENT_, typeof(Extent));
            this._spatialReference = (SpatialReference)info.GetValue(Topology.SPATIALREFERENCE_, typeof(SpatialReference));
            this._clusterTolerance = info.GetDouble(Topology.CLUSTERTOLERANCE_);
            this._zClusterTolerance = info.GetDouble(Topology.ZCLUSTERTOLERANCE_);
            this._maxGeneratedErrorCount = info.GetInt32(Topology.MAXGENERATEDERRORCOUNT_);
            this._topologyRules = (List<TopologyRule>)info.GetValue(Topology.TOPOLOGYRULES_, typeof(List<TopologyRule>));
        }
        public Topology(Topology prototype) : base(prototype) {
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._clusterTolerance = prototype.ClusterTolerance;
            this._zClusterTolerance = prototype.ZClusterTolerance;
            this._maxGeneratedErrorCount = prototype.MaxGeneratedErrorCount;
            this._topologyRules = new List<TopologyRule>();
            foreach (TopologyRule topologyRule in prototype.TopologyRules) {
                this._topologyRules.Add((TopologyRule)topologyRule.Clone());
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The extent of the Topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(true)]
        [Description("The extent of the Topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        /// <summary>
        /// The spatial reference of the Topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(null)]
        [Description("The spatial reference of the Topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// The cluster tolerance of the topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(-1)]
        [Description("The cluster tolerance of the topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ClusterTolerance {
            get { return this._clusterTolerance; }
            set { this._clusterTolerance = value; }
        }
        /// <summary>
        /// The z cluster tolerance of the topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(-1)]
        [Description("The z cluster tolerance of the topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public double ZClusterTolerance {
            get { return this._zClusterTolerance; }
            set { this._zClusterTolerance = value; }
        }
        /// <summary>
        /// The maximum number of errors to generate when validating a topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(-1)]
        [Description("The maximum number of errors to generate when validating a topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int MaxGeneratedErrorCount {
            get { return this._maxGeneratedErrorCount; }
            set { this._maxGeneratedErrorCount = value; }
        }
        /// <summary>
        /// Collection of Topology Rules
        /// </summary>
        [Browsable(true)]
        [Category("Topology")]
        [DefaultValue(null)]
        [Description("Collection of Topology Rules")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<TopologyRule> TopologyRules {
            get { return this._topologyRules; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/TOPO=" + base.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(Topology.EXTENT_, this._extent, typeof(Extent));
            info.AddValue(Topology.SPATIALREFERENCE_, this._spatialReference, typeof(SpatialReference));
            info.AddValue(Topology.CLUSTERTOLERANCE_, this._clusterTolerance);
            info.AddValue(Topology.ZCLUSTERTOLERANCE_, this._zClusterTolerance);
            info.AddValue(Topology.MAXGENERATEDERRORCOUNT_, this._maxGeneratedErrorCount);
            info.AddValue(Topology.TOPOLOGYRULES_, this._topologyRules, typeof(List<TopologyRule>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Topology(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DETOPOLOGY);

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
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

            // Add Rule Errors
            foreach (Rule rule in this._topologyRules) {
                rule.Errors(list, this);
            }

            // TODO: Add Topology Errors
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.TopologyColor;
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            // <Extent></Extent>
            if (this._extent != null) {
                this._extent.WriteXml(writer);
            }

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            // <ClusterTolerance></ClusterTolerance>
            writer.WriteStartElement(Xml.CLUSTERTOLERANCE);
            writer.WriteValue(this._clusterTolerance);
            writer.WriteEndElement();

            // <ZClusterTolerance></ZClusterTolerance>
            writer.WriteStartElement(Xml.ZCLUSTERTOLERANCE);
            writer.WriteValue(this._zClusterTolerance);
            writer.WriteEndElement();

            // <MaxGeneratedErrorCount></MaxGeneratedErrorCount>
            writer.WriteStartElement(Xml.MAXGENERATEDERRORCOUNT);
            writer.WriteValue(this._maxGeneratedErrorCount);
            writer.WriteEndElement();

            // <FeatureClassNames>
            writer.WriteStartElement(Xml.FEATURECLASSNAMES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._NAMES);

            Dataset dataset = this.GetParent();
            if (dataset != null) {
                if (dataset.GetType() == typeof(FeatureDataset)) {
                    FeatureDataset featureDataset = (FeatureDataset)dataset;
                    List<Dataset> datasets = featureDataset.GetChildren();
                    foreach (Dataset dataset2 in datasets) {
                        if (dataset2.GetType() == typeof(FeatureClass)) {
                            FeatureClass featureClass = (FeatureClass)dataset2;
                            List<ControllerMembership> controllers = featureClass.ControllerMemberships;
                            foreach (ControllerMembership controller in controllers) {
                                if (controller is TopologyControllerMembership) {
                                    TopologyControllerMembership topologyControllerMembership = (TopologyControllerMembership)controller;
                                    if (topologyControllerMembership.TopologyName == this.Name) {
                                        // <Name></Name>
                                        writer.WriteStartElement(Xml.NAME);
                                        writer.WriteValue(featureClass.Name);
                                        writer.WriteEndElement();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // </FeatureClassNames>
            writer.WriteEndElement();

            // <TopologyRules>
            writer.WriteStartElement(Xml.TOPOLOGYRULES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFTOPOLOGYRULE);

            // <TopologyRule></TopologyRule>
            foreach (TopologyRule topologyRule in this._topologyRules) {
                topologyRule.WriteXml(writer);
            }

            // </TopologyRules>
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.DrawExpand = false;
            this.GradientColor = ColorSettings.Default.TopologyColor;
            this.SubHeading = Resources.TEXT_TOPOLOGY;
        }
    }
}

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

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Geometric Network
    /// </summary>
    [Serializable]
    public class GeometricNetwork : Dataset {
        private const string EXTENT_ = "extent";
        private const string SPATIALREFERENCE_ = "spatialReference";
        private const string NETWORKTYPE_ = "networkType";
        private const string ORPHANJUNCTIONFEATURECLASSNAME_ = "orphanJunctionFeatureClassName";
        private const string CONNECTIVITYRULES_ = "connectivityRules";
        private const string NETWEIGHTS_ = "netWeights";
        private const string NETWEIGHTASSOCIATIONS_ = "netWeightAssociations";

        //
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        private esriNetworkType _networkType = esriNetworkType.esriNTUtilityNetwork;
        private string _orphanJunctionFeatureClassName = string.Empty;
        private List<ConnectivityRule> _connectivityRules = null;
        private List<NetWeight> _netWeights = null;
        private List<NetWeightAssociation> _netWeightAssociations = null;
        //
        // CONSTRUCTOR
        //
        public GeometricNetwork(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Extent></Extent>
            XPathNavigator navigatorExtent = navigator.SelectSingleNode("Extent");
            if (navigatorExtent != null) {
                this._extent = new Extent(navigatorExtent);
            }

            // <SpatialReference></SpatialReference>
            XPathNavigator navigatorSpatialReference = navigator.SelectSingleNode("SpatialReference");
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }

            // <NetworkType></NetworkType>
            XPathNavigator navigatorNetworkType = navigator.SelectSingleNode("NetworkType");
            if (navigatorNetworkType != null) {
                this._networkType = (esriNetworkType)Enum.Parse(typeof(esriNetworkType), navigatorNetworkType.Value, true);
            }

            // <OrphanJunctionFeatureClassName></OrphanJunctionFeatureClassName>
            XPathNavigator navigatorOrphanJunctionFeatureClassName = navigator.SelectSingleNode("OrphanJunctionFeatureClassName");
            if (navigatorOrphanJunctionFeatureClassName != null) {
                this._orphanJunctionFeatureClassName = navigatorOrphanJunctionFeatureClassName.Value;
            }

            // Create ESRI Namespace Manager
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(navigator.NameTable);
            namespaceManager.AddNamespace(Xml._XSI, Xml.XMLSCHEMAINSTANCE);
            
            // <ConnectivityRules><ConnectivityRule></ConnectivityRule></ConnectivityRules>
            this._connectivityRules = new List<ConnectivityRule>();
            XPathNodeIterator interatorConnectivityRule = navigator.Select("ConnectivityRules/ConnectivityRule");
            while (interatorConnectivityRule.MoveNext()) {
                // Get <ConnectivityRule>
                XPathNavigator navigatorConnectivityRule = interatorConnectivityRule.Current;
                XPathNavigator type = navigatorConnectivityRule.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                switch (type.Value) {
                    case "esri:EdgeConnectivityRule":
                        this._connectivityRules.Add(new EdgeConnectivityRule(navigatorConnectivityRule));
                        break;
                    case "esri:JunctionConnectivityRule":
                        this._connectivityRules.Add(new JunctionConnectivityRule(navigatorConnectivityRule));
                        break;
                }
            }

            // <NetworkWeights><NetWeight></NetWeight></NetworkWeights>
            this._netWeights = new List<NetWeight>();
            XPathNodeIterator interatorNetWeight = navigator.Select("NetworkWeights/NetWeight");
            while (interatorNetWeight.MoveNext()) {
                // Get <NetWeight>
                XPathNavigator navigatorNetWeight = interatorNetWeight.Current;
                if (navigatorNetWeight != null) {
                    this._netWeights.Add(new NetWeight(navigatorNetWeight));
                }
            }

            // <WeightAssociations><NetWeightAssociation></NetWeightAssociation></WeightAssociations>
            this._netWeightAssociations = new List<NetWeightAssociation>();
            XPathNodeIterator interatorNetWeightAssociation = navigator.Select("WeightAssociations/NetWeightAssociation");
            while (interatorNetWeightAssociation.MoveNext()) {
                // Get <NetWeightAssociation>
                XPathNavigator navigatorNetWeightAssociation = interatorNetWeightAssociation.Current;
                if (navigatorNetWeightAssociation != null) {
                    this._netWeightAssociations.Add(new NetWeightAssociation(navigatorNetWeightAssociation));
                }
            }
        }
        public GeometricNetwork(SerializationInfo info, StreamingContext context): base(info, context) {
            this._extent = (Extent)info.GetValue(GeometricNetwork.EXTENT_, typeof(Extent));
            this._spatialReference = (SpatialReference)info.GetValue(GeometricNetwork.SPATIALREFERENCE_, typeof(SpatialReference));
            this._networkType = (esriNetworkType)Enum.Parse(typeof(esriNetworkType), info.GetString(GeometricNetwork.NETWORKTYPE_), true);
            this._orphanJunctionFeatureClassName = info.GetString(GeometricNetwork.ORPHANJUNCTIONFEATURECLASSNAME_);
            this._connectivityRules = (List<ConnectivityRule>)info.GetValue(GeometricNetwork.CONNECTIVITYRULES_, typeof(List<ConnectivityRule>));
            this._netWeights = (List<NetWeight>)info.GetValue(GeometricNetwork.NETWEIGHTS_, typeof(List<NetWeight>));
            this._netWeightAssociations = (List<NetWeightAssociation>)info.GetValue(GeometricNetwork.NETWEIGHTASSOCIATIONS_, typeof(List<NetWeightAssociation>));
        }
        public GeometricNetwork(GeometricNetwork prototype) : base(prototype) {
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._networkType = prototype.NetworkType;
            this._orphanJunctionFeatureClassName = prototype.OrphanJunctionFeatureClassName;

            // Add Cloned Rules
            this._connectivityRules = new List<ConnectivityRule>();
            foreach (ConnectivityRule connnectivityRule in prototype.ConnectivityRules) {
                this._connectivityRules.Add((ConnectivityRule)connnectivityRule.Clone());
            }

            // Add Cloned Network Weights
            this._netWeights = new List<NetWeight>();
            foreach (NetWeight netWeight in prototype.NetworkWeights) {
                this._netWeights.Add((NetWeight)netWeight.Clone());
            }

            // Add Cloned Network Weight Associations
            this._netWeightAssociations = new List<NetWeightAssociation>();
            foreach(NetWeightAssociation netWeightAssociation in prototype.NetworkWeightAssociations){
                this._netWeightAssociations.Add((NetWeightAssociation)netWeightAssociation.Clone());
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The extent of the Geoemtric Network
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(true)]
        [Description("The extent of the Geoemtric Network")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        /// <summary>
        /// The spatial reference of the Geoemtric Network
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(null)]
        [Description("The spatial reference of the Geoemtric Network")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// The type of associated logical network
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(esriNetworkType.esriNTUtilityNetwork)]
        [Description("The type of associated logical network")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkType NetworkType {
            get { return this._networkType; }
            set { this._networkType = value; }
        }
        /// <summary>
        /// The FeatureClass containing the OrphanJunctionFeatures
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(null)]
        [Description("The FeatureClass containing the OrphanJunctionFeatures")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string OrphanJunctionFeatureClassName {
            get { return this._orphanJunctionFeatureClassName; }
            set { this._orphanJunctionFeatureClassName = value; }
        }
        /// <summary>
        /// Collection of Geometric Network Connectivity Rules
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(null)]
        [Description("Collection of Geometric Network Connectivity Rules")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<ConnectivityRule> ConnectivityRules {
            get { return this._connectivityRules; }
        }
        /// <summary>
        /// Collection of Geometric Network Weights
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(null)]
        [Description("Collection of Network Weights")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetWeight> NetworkWeights {
            get { return this._netWeights; }
        }
        /// <summary>
        /// Collection of Geometric Network Weight Associations
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network")]
        [DefaultValue(null)]
        [Description("Collection of Geometric Network Weight Associations")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetWeightAssociation> NetworkWeightAssociations {
            get { return this._netWeightAssociations; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/GN=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(GeometricNetwork.EXTENT_, this._extent, typeof(Extent));
            info.AddValue(GeometricNetwork.SPATIALREFERENCE_, this._spatialReference, typeof(SpatialReference));
            info.AddValue(GeometricNetwork.NETWORKTYPE_, this._networkType.ToString("d"));
            info.AddValue(GeometricNetwork.ORPHANJUNCTIONFEATURECLASSNAME_, this._orphanJunctionFeatureClassName);
            info.AddValue(GeometricNetwork.CONNECTIVITYRULES_, this._connectivityRules, typeof(List<ConnectivityRule>));
            info.AddValue(GeometricNetwork.NETWEIGHTS_, this._netWeights, typeof(List<NetWeight>));
            info.AddValue(GeometricNetwork.NETWEIGHTASSOCIATIONS_, this._netWeightAssociations, typeof(List<NetWeightAssociation>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new GeometricNetwork(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DEGEOMETRICNETWORK);

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Call Base Errors
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
            foreach (Rule rule in this._connectivityRules) {
                rule.Errors(list, this);
            }

            // Add Network Weight Errors
            foreach (NetWeight netWeight in this._netWeights) {
                netWeight.Errors(list, this);
            }

            // Network Weight Association Errors
            foreach (NetWeightAssociation netWeightAssociation in this._netWeightAssociations) {
                netWeightAssociation.Errors(list, this);
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.GeometricNetworkColor;
        }
        //
        // PROTECTED METHDOS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            // Get Model
            SchemaModel model = (SchemaModel)base.Container;

            // <Extent></Extent>
            if (this._extent != null) {
                this._extent.WriteXml(writer);
            }

            // <SpatialReference></SpatialReference>
            if (this._spatialReference != null) {
                this._spatialReference.WriteXml(writer);
            }

            // <NetworkType></NetworkType>
            writer.WriteStartElement(Xml.NETWORKTYPE);
            writer.WriteValue(this._networkType.ToString());
            writer.WriteEndElement();

            // <OrphanJunctionFeatureClassName></OrphanJunctionFeatureClassName>
            writer.WriteStartElement(Xml.ORPHANJUNCTIONFEATURECLASSNAME);
            writer.WriteValue(this._orphanJunctionFeatureClassName);
            writer.WriteEndElement();

            // <FeatureClassNames>
            writer.WriteStartElement(Xml.FEATURECLASSNAMES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._NAMES);

            Dataset dataset = base.GetParent();
            if (dataset.GetType() == typeof(FeatureDataset)) {
                FeatureDataset featureDataset = (FeatureDataset)dataset;
                List<Dataset> datasets = featureDataset.GetChildren();
                foreach (Dataset dataset2 in datasets) {
                    if (dataset2.GetType() == typeof(FeatureClass)) {
                        FeatureClass featureClass = (FeatureClass)dataset2;
                        List<ControllerMembership> controllers = featureClass.ControllerMemberships;
                        foreach (ControllerMembership controller in controllers) {
                            if (controller is GeometricNetworkControllerMembership) {
                                GeometricNetworkControllerMembership geometricNetworkControllerMembership = (GeometricNetworkControllerMembership)controller;
                                if (geometricNetworkControllerMembership.GeometricNetworkName == base.Name) {
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

            // </FeatureClassNames>
            writer.WriteEndElement();

            // <ConnectivityRules>
            writer.WriteStartElement(Xml.CONNECTIVITYRULES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFCONNECTIVITYRULE);

            // <ConnectivityRule></ConnectivityRule>
            foreach (ConnectivityRule connectivityRule in this._connectivityRules) {
                connectivityRule.WriteXml(writer);
            }

            // </ConnectivityRules>
            writer.WriteEndElement();

            // <NetworkWeights>
            writer.WriteStartElement(Xml.NETWORKWEIGHTS);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFNETWEIGHT);

            // <NetWeight></NetWeight>
            foreach (NetWeight netWeight in this._netWeights) {
                netWeight.WriteXml(writer);
            }

            // </NetworkWeights>
            writer.WriteEndElement();

            // <WeightAssociations>
            writer.WriteStartElement(Xml.WEIGHTASSOCIATIONS);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFNETWEIGHTASSOCIATION);

            // <NetWeightAssociation></NetWeightAssociation>
            foreach (NetWeightAssociation netWeightAssociation in this._netWeightAssociations) {
                netWeightAssociation.WriteXml(writer);
            }

            // </WeightAssociations>
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.DrawExpand = false;
            this.GradientColor = ColorSettings.Default.GeometricNetworkColor;
            this.SubHeading = Resources.TEXT_GEOMETRIC_NETWORK;
        }
    }
}

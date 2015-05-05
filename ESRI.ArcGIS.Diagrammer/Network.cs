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
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Network Dataset
    /// </summary>
    [Serializable]
    public class Network : Dataset {
        private Extent _extent = null;
        private SpatialReference _spatialReference = null;
        private string _logicalNetworkName = string.Empty;
        private esriNetworkDatasetType _networkType = esriNetworkDatasetType.esriNDTUnknown;
        private bool _buildable = false;
        private bool _supportsTurns = false;
        private NetworkDirections _networkDirections = null;
        private List<Property> _properties = null;
        private List<Property> _userData = null;
        private List<NetworkSource> _networkSources = null;
        private List<NetworkAttribute> _networkAttributes = null;
        private List<NetworkAssignment> _networkAssignments = null;
        private string _configurationKeyword = string.Empty;
        //
        // CONSTRUCTOR
        //
        public Network(IXPathNavigable path) : base(path) {
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
            if (navigatorSpatialReference != null) {
                this._spatialReference = new SpatialReference(navigatorSpatialReference);
            }
            else {
                this._spatialReference = new SpatialReference();
            }

            // <LogicalNetworkName>
            XPathNavigator navigatorLogicalNetworkName = navigator.SelectSingleNode("LogicalNetworkName");
            if (navigatorLogicalNetworkName != null) {
                this._logicalNetworkName = navigatorLogicalNetworkName.Value;
            }

            // <NetworkType>
            XPathNavigator navigatorNetworkType = navigator.SelectSingleNode("NetworkType");
            if (navigatorNetworkType != null) {
                this._networkType = (esriNetworkDatasetType)Enum.Parse(typeof(esriNetworkDatasetType), navigatorNetworkType.Value, true);
            }

            // <Buildable>
            XPathNavigator navigatorBuildable = navigator.SelectSingleNode("Buildable");
            if (navigatorBuildable != null) {
                this._buildable = navigatorBuildable.ValueAsBoolean;
            }

            // <SupportsTurns>
            XPathNavigator navigatorSupportsTurns = navigator.SelectSingleNode("SupportsTurns");
            if (navigatorSupportsTurns != null) {
                this._supportsTurns = navigatorSupportsTurns.ValueAsBoolean;
            }

            // <NetworkDirections>
            XPathNavigator navigatorNetworkDirections = navigator.SelectSingleNode("NetworkDirections");
            if (navigatorNetworkDirections != null) {
                this._networkDirections = new NetworkDirections(navigatorNetworkDirections);
            }
            else {
                this._networkDirections = new NetworkDirections();
            }

            // <Properties><PropertyArray><PropertySetProperty>
            this._properties = new List<Property>();
            XPathNodeIterator interatorProperty = navigator.Select("Properties/PropertyArray/PropertySetProperty");
            while (interatorProperty.MoveNext()) {
                // Get <Property>
                XPathNavigator navigatorProperty = interatorProperty.Current;

                // Add Property
                Property property = new Property(navigatorProperty);
                this._properties.Add(property);
            }

            // <UserData><PropertyArray><PropertySetProperty>
            this._userData = new List<Property>();
            XPathNodeIterator interatorProperty2 = navigator.Select("UserData/PropertyArray/PropertySetProperty");
            while (interatorProperty2.MoveNext()) {
                // Get <Property>
                XPathNavigator navigatorProperty = interatorProperty2.Current;

                // Add Property
                Property property = new Property(navigatorProperty);
                this._userData.Add(property);
            }

            // <EdgeFeatureSources><EdgeFeatureSource>
            // <JunctionFeatureSources><JunctionFeatureSource>
            // <SystemJunctionSources><SystemJunctionSource>
            // <TurnFeatureSources><TurnFeatureSource>
            this._networkSources = new List<NetworkSource>();
            XPathNodeIterator interatorNetworkSource = navigator.Select(
                "EdgeFeatureSources/EdgeFeatureSource" + " | " + 
                "JunctionFeatureSources/JunctionFeatureSource" + " | " + 
                "SystemJunctionSources/SystemJunctionSource" + " | " + 
                "TurnFeatureSources/TurnFeatureSource");
            while (interatorNetworkSource.MoveNext()) {
                // 
                XPathNavigator navigatorNetworkSource = interatorNetworkSource.Current;

                NetworkSource networkSource = null;
                switch (navigatorNetworkSource.Name) {
                    case "EdgeFeatureSource":
                        networkSource = new EdgeFeatureSource(navigatorNetworkSource);
                        break;
                    case "JunctionFeatureSource":
                        networkSource = new JunctionFeatureSource(navigatorNetworkSource);
                        break;
                    case "SystemJunctionSource":
                        networkSource = new SystemJunctionSource(navigatorNetworkSource);
                        break;
                    case "TurnFeatureSource":
                        networkSource = new TurnFeatureSource(navigatorNetworkSource);
                        break;
                }
                // Add EdgeFeatureSource
                if (networkSource != null) {
                    this._networkSources.Add(networkSource);
                }
            }

            // <EvaluatedNetworkAttributes><EvaluatedNetworkAttribute>
            // <NetworkAttributes><NetworkAttribute>
            this._networkAttributes = new List<NetworkAttribute>();
            XPathNodeIterator interatorNetworkAttribute = navigator.Select(
                "EvaluatedNetworkAttributes/EvaluatedNetworkAttribute" + " | " +
                "NetworkAttributes/NetworkAttribute");
            while (interatorNetworkAttribute.MoveNext()) {
                // 
                XPathNavigator navigatorNetworkAttribute = interatorNetworkAttribute.Current;

                NetworkAttribute networkAttribute = null;
                switch (navigatorNetworkAttribute.Name) {
                    case "EvaluatedNetworkAttribute":
                        networkAttribute = new EvaluatedNetworkAttribute(navigatorNetworkAttribute);
                        break;
                    case "NetworkAttribute":
                        networkAttribute = new NetworkAttribute(navigatorNetworkAttribute);
                        break;
                }
                // Add EdgeFeatureSource
                if (networkAttribute != null) {
                    this._networkAttributes.Add(networkAttribute);
                }
            }

            // <NetworkAssignments><NetworkAssignment>
            this._networkAssignments = new List<NetworkAssignment>();
            XPathNodeIterator interatorNetworkAssignment = navigator.Select("NetworkAssignments/NetworkAssignment");
            while (interatorNetworkAssignment.MoveNext()) {
                // Get <NetworkAssignment>
                XPathNavigator navigatorNetworkAssignment = interatorNetworkAssignment.Current;

                // Add NetworkAssignment
                NetworkAssignment networkAssignment = new NetworkAssignment(navigatorNetworkAssignment);
                this._networkAssignments.Add(networkAssignment);
            }

            // <ConfigurationKeyword>
            XPathNavigator navigatorConfigurationKeyword = navigator.SelectSingleNode("ConfigurationKeyword");
            if (navigatorConfigurationKeyword != null) {
                this._configurationKeyword = navigatorConfigurationKeyword.Value;
            }
        }
        public Network(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._extent = (Extent)info.GetValue("extent", typeof(Extent));
            this._spatialReference = (SpatialReference)info.GetValue("spatialReference", typeof(SpatialReference));
            this._logicalNetworkName = info.GetString("logicalNetworkName");
            this._networkType = (esriNetworkDatasetType)Enum.Parse(typeof(esriNetworkDatasetType), info.GetString("networkType"), true);
            this._buildable = info.GetBoolean("buildable");
            this._supportsTurns = info.GetBoolean("supportsTurns");
            this._networkDirections = (NetworkDirections)info.GetValue("networkDirections", typeof(NetworkDirections));
            this._properties = (List<Property>)info.GetValue("properties", typeof(List<Property>));
            this._userData = (List<Property>)info.GetValue("userData", typeof(List<Property>));
            this._networkSources = (List<NetworkSource>)info.GetValue("networkSources", typeof(List<NetworkSource>));
            this._networkAttributes = (List<NetworkAttribute>)info.GetValue("networkAttributes", typeof(List<NetworkAttribute>));
            this._networkAssignments = (List<NetworkAssignment>)info.GetValue("networkAssignments", typeof(List<NetworkAssignment>));
            this._configurationKeyword = info.GetString("configurationKeyword");
        }
        public Network(Network prototype) : base(prototype) {
            if (prototype.Extent != null) {
                this._extent = prototype.Extent.Clone() as Extent;
            }
            if (prototype.SpatialReference != null) {
                this._spatialReference = prototype.SpatialReference.Clone() as SpatialReference;
            }
            this._logicalNetworkName = prototype.LogicalNetworkName;
            this._networkType = prototype.NetworkType;
            this._buildable = prototype.Buildable;
            this._supportsTurns = prototype.SupportsTurns;
            this._networkDirections = (NetworkDirections)prototype.NetworkDirections.Clone();
            this._properties = new List<Property>();
            foreach (Property property in prototype.PropertyCollection) {
                this._properties.Add((Property)property.Clone());
            }
            this._userData = new List<Property>();
            foreach (Property property in prototype.UserDataCollection) {
                this._userData.Add((Property)property.Clone());
            }
            this._networkSources = new List<NetworkSource>();
            foreach (NetworkSource networkSource in prototype.NetworkSourceCollection) {
                this._networkSources.Add((NetworkSource)networkSource.Clone());
            }
            this._networkAttributes = new List<NetworkAttribute>();
            foreach (NetworkAttribute networkAttribute in prototype.NetworkAttributeCollection) {
                this._networkAttributes.Add((NetworkAttribute)networkAttribute.Clone());
            }
            this._networkAssignments = new List<NetworkAssignment>();
            foreach (NetworkAssignment networkAssignment in prototype.NetworkAssignmentCollection) {
                this._networkAssignments.Add((NetworkAssignment)networkAssignment.Clone());
            }
            this._configurationKeyword = prototype.ConfigurationKeyword;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The extent of the Network Dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("The extent of the Network Dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Extent Extent {
            get { return this._extent; }
            set { this._extent = value; }
        }
        /// <summary>
        /// The spatial reference of the Network Dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("The spatial reference of the Network Dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SpatialReference SpatialReference {
            get { return this._spatialReference; }
            set { this._spatialReference = value; }
        }
        /// <summary>
        /// The name of the network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue("")]
        [Description("The name of the network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string LogicalNetworkName {
            get { return this._logicalNetworkName; }
            set { this._logicalNetworkName = value; }
        }
        /// <summary>
        /// The type of the network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(esriNetworkDatasetType.esriNDTUnknown)]
        [Description("The type of the network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkDatasetType NetworkType {
            get { return this._networkType; }
            set { this._networkType = value; }
        }
        /// <summary>
        /// Indicates if this network dataset is buildable
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(false)]
        [Description("Indicates if this network dataset is buildable")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Buildable {
            get { return this._buildable; }
            set { this._buildable = value; }
        }
        /// <summary>
        /// Indicates if this network dataset supports network turn elements
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(false)]
        [Description("Indicates if this network dataset supports network turn elements")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool SupportsTurns {
            get { return this._supportsTurns; }
            set { this._supportsTurns = value; }
        }
        /// <summary>
        /// The driving directions settings for the network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("The driving directions settings for the network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public NetworkDirections NetworkDirections {
            get { return this._networkDirections; }
            set { this._networkDirections = value; }
        }
        /// <summary>
        /// Property set of this network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("Property set of this network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> PropertyCollection {
            get { return this._properties; }
            set { this._properties = value; }
        }
        /// <summary>
        /// User specified property set of this network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("User specified property set of this network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> UserDataCollection {
            get { return this._userData; }
            set { this._userData = value; }
        }
        /// <summary>
        /// Collection of network sources in this network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("Collection of network sources in this network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetworkSource> NetworkSourceCollection {
            get { return this._networkSources; }
            set { this._networkSources = value; }
        }
        /// <summary>
        /// Collection of network attributes in this network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("Collection of network attributes in this network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetworkAttribute> NetworkAttributeCollection {
            get { return this._networkAttributes; }
            set { this._networkAttributes = value; }
        }
        /// <summary>
        /// Collection of Network Assignments
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue(null)]
        [Description("Collection of Network Assignments")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetworkAssignment> NetworkAssignmentCollection {
            get { return this._networkAssignments; }
            set { this._networkAssignments = value; }
        }
        /// <summary>
        /// The database configuration keyword for the network dataset
        /// </summary>
        [Browsable(true)]
        [Category("Network")]
        [DefaultValue("")]
        [Description("The database configuration keyword for the network dataset")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ConfigurationKeyword {
            get { return this._configurationKeyword; }
            set { this._configurationKeyword = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/ND=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("extent", this._extent, typeof(Extent));
            info.AddValue("spatialReference", this._spatialReference, typeof(SpatialReference));
            info.AddValue("logicalNetworkName", this._logicalNetworkName);
            info.AddValue("networkType", Convert.ToInt32(this._networkType).ToString());
            info.AddValue("buildable", this._buildable);
            info.AddValue("supportsTurns", this._supportsTurns);
            info.AddValue("networkDirections", this._networkDirections, typeof(NetworkDirections));
            info.AddValue("properties", this._properties, typeof(List<Property>));
            info.AddValue("userData", this._userData, typeof(List<Property>));
            info.AddValue("networkSources", this._networkSources, typeof(List<NetworkSource>));
            info.AddValue("networkAttributes", this._networkAttributes, typeof(List<NetworkAttribute>));
            info.AddValue("networkAssignments", this._networkAssignments, typeof(List<NetworkAssignment>));
            info.AddValue("configurationKeyword", this._configurationKeyword);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Network(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DENETWORKDATASET);

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
            this.GradientColor = ColorSettings.Default.NetworkColor;
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

            // <LogicalNetworkName></LogicalNetworkName>
            writer.WriteStartElement("LogicalNetworkName");
            writer.WriteValue(this._logicalNetworkName);
            writer.WriteEndElement();

            // <NetworkType></NetworkType>
            writer.WriteStartElement("NetworkType");
            int networkType = (int)this._networkType;
            writer.WriteValue(networkType);
            writer.WriteEndElement();

            // <Buildable></Buildable>
            writer.WriteStartElement("Buildable");
            writer.WriteValue(this._buildable);
            writer.WriteEndElement();

            // <SupportsTurns></SupportsTurns>
            writer.WriteStartElement("SupportsTurns");
            writer.WriteValue(this._supportsTurns);
            writer.WriteEndElement();

            // <NetworkDirections></NetworkDirections>
            this._networkDirections.WriteXml(writer);

            if (this._properties.Count == 0) {
                // <Properties></Properties>
                writer.WriteStartElement("Properties");
                writer.WriteAttributeString(Xml._XSI, "nil", null, "true");
                writer.WriteEndElement();
            }
            else {
                // <Properties>
                writer.WriteStartElement("Properties");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySet");

                // <PropertyArray>
                writer.WriteStartElement("PropertyArray");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfPropertySetProperty");

                // <PropertySetProperty></PropertySetProperty>
                foreach (Property property in this._properties) {
                    property.WriteXml(writer);
                }

                // </PropertyArray>
                writer.WriteEndElement();

                // </Properties>
                writer.WriteEndElement();
            }

            if (this._userData.Count == 0) {
                // <UserData></UserData>
                writer.WriteStartElement("UserData");
                writer.WriteAttributeString(Xml._XSI, "nil", null, "true");
                writer.WriteEndElement();
            }
            else {
                // <UserData>
                writer.WriteStartElement("UserData");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySet");

                // <PropertyArray>
                writer.WriteStartElement("PropertyArray");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfPropertySetProperty");

                // <PropertySetProperty></PropertySetProperty>
                foreach (Property property in this._userData) {
                    property.WriteXml(writer);
                }

                // </PropertyArray>
                writer.WriteEndElement();

                // </UserData>
                writer.WriteEndElement();
            }

            // <EdgeFeatureSources>
            writer.WriteStartElement("EdgeFeatureSources");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfEdgeFeatureSource");

            // <EdgeFeatureSource></EdgeFeatureSource>
            foreach (NetworkSource networkSource in this._networkSources) {
                if (networkSource.GetType() == typeof(EdgeFeatureSource)) {
                    networkSource.WriteXml(writer);
                }
            }

            // </EdgeFeatureSources>
            writer.WriteEndElement();

            // <JunctionFeatureSources>
            writer.WriteStartElement("JunctionFeatureSources");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfJunctionFeatureSource");

            // <JunctionFeatureSource></JunctionFeatureSource>
            foreach (NetworkSource networkSource in this._networkSources) {
                if (networkSource.GetType() == typeof(JunctionFeatureSource)) {
                    networkSource.WriteXml(writer);
                }
            }

            // </JunctionFeatureSources>
            writer.WriteEndElement();

            // <SystemJunctionSources>
            writer.WriteStartElement("SystemJunctionSources");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfSystemJunctionSource");

            // <SystemJunctionSource></SystemJunctionSource>
            foreach (NetworkSource networkSource in this._networkSources) {
                if (networkSource.GetType() == typeof(SystemJunctionSource)) {
                    networkSource.WriteXml(writer);
                }
            }

            // </SystemJunctionSources>
            writer.WriteEndElement();

            // <TurnFeatureSources>
            writer.WriteStartElement("TurnFeatureSources");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfTurnFeatureSource");

            // <TurnFeatureSource></TurnFeatureSource>
            foreach (NetworkSource networkSource in this._networkSources) {
                if (networkSource.GetType() == typeof(TurnFeatureSource)) {
                    networkSource.WriteXml(writer);
                }
            }

            // </TurnFeatureSources>
            writer.WriteEndElement();

            if (this._buildable) {
                // <EvaluatedNetworkAttributes>
                writer.WriteStartElement("EvaluatedNetworkAttributes");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfEvaluatedNetworkAttribute");

                // <EvaluatedNetworkAttribute></EvaluatedNetworkAttribute>
                foreach (NetworkAttribute networkAttribute in this._networkAttributes) {
                    if (networkAttribute.GetType() == typeof(EvaluatedNetworkAttribute)) {
                        networkAttribute.WriteXml(writer);
                    }
                }

                // </EvaluatedNetworkAttributes>
                writer.WriteEndElement();
            }
            else {
                // <NetworkAttributes>
                writer.WriteStartElement("NetworkAttributes");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfNetworkAttribute");

                // <NetworkAttribute></NetworkAttribute>
                foreach (NetworkAttribute networkAttribute in this._networkAttributes) {
                    if (networkAttribute.GetType() == typeof(NetworkAttribute)) {
                        networkAttribute.WriteXml(writer);
                    }
                }

                // </NetworkAttributes>
                writer.WriteEndElement();
            }

            // <NetworkAssignments>
            writer.WriteStartElement("NetworkAssignments");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfNetworkAssignment");

            // <NetworkAssignment></NetworkAssignment>
            foreach (NetworkAssignment networkAssignment in this._networkAssignments) {
                networkAssignment.WriteXml(writer);
            }

            // </NetworkAssignments>
            writer.WriteEndElement();

            // <ConfigurationKeyword></ConfigurationKeyword>
            writer.WriteStartElement("ConfigurationKeyword");
            writer.WriteValue(this._configurationKeyword);
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.DrawExpand = false;
            this.GradientColor = ColorSettings.Default.NetworkColor;
            this.SubHeading = Resources.TEXT_NETWORK;
        }
    }
}

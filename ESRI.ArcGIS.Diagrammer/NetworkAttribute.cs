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
    /// ESRI Network Attribute
    /// </summary>
    [Serializable]
    public class NetworkAttribute : EsriObject {
        private int _id = 0;
        private string _name = string.Empty;
        private esriNetworkAttributeUnits _units = esriNetworkAttributeUnits.esriNAUUnknown;
        private esriNetworkAttributeDataType _dataType = esriNetworkAttributeDataType.esriNADTBoolean;
        private esriNetworkAttributeUsageType _usageType = esriNetworkAttributeUsageType.esriNAUTCost;
        private List<Property> _userData = null;
        private bool _useByDefault = false;
        private List<NetworkAttributeParameter> _attributeParameters = null;
        //
        // CONSTRUCTOR
        //
        public NetworkAttribute(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <ID>
            XPathNavigator navigatorID = navigator.SelectSingleNode("ID");
            if (navigatorID != null) {
                this._id = navigatorID.ValueAsInt;
            }

            // <Name>
            XPathNavigator navigatorName = navigator.SelectSingleNode("Name");
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // <Units>
            XPathNavigator navigatorUnits = navigator.SelectSingleNode("Units");
            if (navigatorUnits != null) {
                this._units = GeodatabaseUtility.GetNetworkAttributeUnits(navigatorUnits.Value);
            }

            // <DataType>
            XPathNavigator navigatorDataType = navigator.SelectSingleNode("DataType");
            if (navigatorDataType != null) {
                this._dataType = (esriNetworkAttributeDataType)Enum.Parse(typeof(esriNetworkAttributeDataType), navigatorDataType.Value, true);
            }

            // <UsageType>
            XPathNavigator navigatorUsageType = navigator.SelectSingleNode("UsageType");
            if (navigatorUsageType != null) {
                this._usageType = (esriNetworkAttributeUsageType)Enum.Parse(typeof(esriNetworkAttributeUsageType), navigatorUsageType.Value, true);
            }

            // <UserData><PropertyArray><PropertySetProperty>
            this._userData = new List<Property>();
            XPathNodeIterator interatorProperty = navigator.Select("UserData/PropertyArray/PropertySetProperty");
            while (interatorProperty.MoveNext()) {
                // Get <PropertySetProperty>
                XPathNavigator navigatorProperty = interatorProperty.Current;

                // Add PropertySetProperty
                Property property = new Property(navigatorProperty);
                this._userData.Add(property);
            }

            // <UseByDefault>
            XPathNavigator navigatorUseByDefault = navigator.SelectSingleNode("UseByDefault");
            if (navigatorUseByDefault != null) {
                this._useByDefault = navigatorUseByDefault.ValueAsBoolean;
            }

            // <AttributedParameters><AttributedParameter>
            this._attributeParameters = new List<NetworkAttributeParameter>();
            XPathNodeIterator interatorAttributedParameter = navigator.Select("AttributedParameters/AttributedParameter");
            while (interatorAttributedParameter.MoveNext()) {
                // Get <AttributedParameter>
                XPathNavigator navigatorAttributedParameter = interatorAttributedParameter.Current;

                // Add AttributedParameter
                NetworkAttributeParameter networkAttributeParameter = new NetworkAttributeParameter(navigatorAttributedParameter);
                this._attributeParameters.Add(networkAttributeParameter);
            }
        }
        public NetworkAttribute(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._id = info.GetInt32("id");
            this._name = info.GetString("name");
            this._units = (esriNetworkAttributeUnits)Enum.Parse(typeof(esriNetworkAttributeUnits), info.GetString("units"), true);
            this._dataType = (esriNetworkAttributeDataType)Enum.Parse(typeof(esriNetworkAttributeDataType), info.GetString("dataType"), true);
            this._usageType = (esriNetworkAttributeUsageType)Enum.Parse(typeof(esriNetworkAttributeUsageType), info.GetString("usageType"), true);
            this._userData = (List<Property>)info.GetValue("userData", typeof(List<Property>));
            this._useByDefault = info.GetBoolean("useByDefault");
            this._attributeParameters = (List<NetworkAttributeParameter>)info.GetValue("attributeParameters", typeof(List<NetworkAttributeParameter>));
        }
        public NetworkAttribute(NetworkAttribute prototype) : base(prototype) {
            this._id = prototype.Id;
            this._name = prototype.Name;
            this._units = prototype.Units;
            this._dataType = prototype.DataType;
            this._usageType = prototype.UsageType;
            this._userData = new List<Property>();
            foreach (Property property in prototype.UserDataCollection) {
                Property propertyClone = (Property)property.Clone();
                this._userData.Add(propertyClone);
            }
            this._useByDefault = prototype.UseByDefault;
            this._attributeParameters = new List<NetworkAttributeParameter>();
            foreach (NetworkAttributeParameter networkAttributeParameter in prototype.AttributeParameterCollection) {
                NetworkAttributeParameter networkAttributeParameterClone = (NetworkAttributeParameter)networkAttributeParameter.Clone();
                this._attributeParameters.Add(networkAttributeParameterClone);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Unique identifier of this network attribute
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(0)]
        [Description("Unique identifier of this network attribute")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Id {
            get { return this._id; }
            set { this._id = value; }
        }
        /// <summary>
        /// Name of this network attribute
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue("")]
        [Description("Name of this network attribute")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// Units of this network attribute
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(esriNetworkAttributeUnits.esriNAUUnknown)]
        [Description("Units of this network attribute")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkAttributeUnits Units {
            get { return this._units; }
            set { this._units = value; }
        }
        /// <summary>
        /// Type of data used in this network attribute
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(esriNetworkAttributeDataType.esriNADTBoolean)]
        [Description("Type of data used in this network attribute")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkAttributeDataType DataType {
            get { return this._dataType; }
            set { this._dataType = value; }
        }
        /// <summary>
        /// Usage type of this network attribute
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(esriNetworkAttributeUsageType.esriNAUTCost)]
        [Description("Usage type of this network attribute")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkAttributeUsageType UsageType {
            get { return this._usageType; }
            set { this._usageType = value; }
        }
        /// <summary>
        /// User Data
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(null)]
        [Description("User Data")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> UserDataCollection {
            get { return this._userData; }
        }
        /// <summary>
        /// Indicates if this network attribute is to be used by default
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(false)]
        [Description("Indicates if this network attribute is to be used by default")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool UseByDefault {
            get { return this._useByDefault; }
            set { this._useByDefault = value; }
        }
        /// <summary>
        /// Array of parameters
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute")]
        [DefaultValue(null)]
        [Description("Array of parameters")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<NetworkAttributeParameter> AttributeParameterCollection {
            get { return this._attributeParameters; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("id", this._id);
            info.AddValue("name", this._name);
            info.AddValue("units", Convert.ToInt32(this._units).ToString());
            info.AddValue("dataType", Convert.ToInt32(this._dataType).ToString());
            info.AddValue("usageType", Convert.ToInt32(this._usageType).ToString());
            info.AddValue("userData", this._userData, typeof(List<Property>));
            info.AddValue("useByDefault", this._useByDefault);
            info.AddValue("attributeParameters", this._attributeParameters, typeof(List<NetworkAttributeParameter>));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Attribute Errors
        }
        public override object Clone() {
            return new NetworkAttribute(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetworkAttribute>
            writer.WriteStartElement("NetworkAttribute");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetworkAttribute");

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </NetworkAttribute>
            writer.WriteEndElement();
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            // Call Base Method
            base.WriteInnerXml(writer);

            // <ID></ID>
            writer.WriteStartElement("ID");
            writer.WriteValue(this._id);
            writer.WriteEndElement();

            // <Name></Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <Units></Units>
            writer.WriteStartElement("Units");
            writer.WriteValue(GeodatabaseUtility.GetDescription(this._units));
            writer.WriteEndElement();

            // <DataType></DataType>
            writer.WriteStartElement("DataType");
            writer.WriteValue(this._dataType.ToString());
            writer.WriteEndElement();

            // <UsageType></UsageType>
            writer.WriteStartElement("UsageType");
            writer.WriteValue(this._usageType.ToString());
            writer.WriteEndElement();

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
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertyArray");

                // <PropertySetProperty></PropertySetProperty>
                foreach (Property property in this._userData) {
                    property.WriteXml(writer);
                }

                // </PropertyArray>
                writer.WriteEndElement();

                // </UserData>
                writer.WriteEndElement();
            }

            // <UseByDefault></UseByDefault>
            writer.WriteStartElement("UseByDefault");
            writer.WriteValue(this._useByDefault);
            writer.WriteEndElement();

            // <AttributeParameters>
            writer.WriteStartElement("AttributeParameters");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfNetworkAttributeParameter");

            // <NetworkAttributeParameter></NetworkAttributeParameter>
            foreach (NetworkAttributeParameter networkAttributeParameter in this._attributeParameters) {
                networkAttributeParameter.WriteXml(writer);
            }

            // </AttributeParameters>
            writer.WriteEndElement();
        }
    }
}

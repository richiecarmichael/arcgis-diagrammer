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
    /// ESRI Network Assignment
    /// </summary>
    [Serializable]
    public class NetworkAssignment : EsriObject {
        private bool _isDefault = false;
        private int _id = 0;
        private string _networkAttributeName = string.Empty;
        private esriNetworkElementType _networkElementType = esriNetworkElementType.esriNETJunction;
        private string _networkSourceName = string.Empty;
        private string _networkEvaluatorCLSID = string.Empty;
        private esriNetworkEdgeDirection _networkEdgeDirection = esriNetworkEdgeDirection.esriNEDNone;
        private List<Property> _networkEvaluatorData = null;
        //
        // CONSTRUCTOR
        //
        public NetworkAssignment(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <IsDefault>
            XPathNavigator navigatorIsDefault = navigator.SelectSingleNode("IsDefault");
            if (navigatorIsDefault != null) {
                this._isDefault = navigatorIsDefault.ValueAsBoolean;
            }

            // <ID>
            XPathNavigator navigatorID = navigator.SelectSingleNode("ID");
            if (navigatorID != null) {
                this._id = navigatorID.ValueAsInt;
            }

            // <NetworkAttributeName>
            XPathNavigator navigatorNetworkAttributeName = navigator.SelectSingleNode("NetworkAttributeName");
            if (navigatorNetworkAttributeName != null) {
                this._networkAttributeName = navigatorNetworkAttributeName.Value;
            }

            // <NetworkElementType>
            XPathNavigator navigatorNetworkElementType = navigator.SelectSingleNode("NetworkElementType");
            if (navigatorNetworkElementType != null) {
                this._networkElementType = (esriNetworkElementType)Enum.Parse(typeof(esriNetworkElementType), navigatorNetworkElementType.Value, true);
            }

            // <NetworkSourceName>
            XPathNavigator navigatorNetworkSourceName = navigator.SelectSingleNode("NetworkSourceName");
            if (navigatorNetworkSourceName != null) {
                this._networkSourceName = navigatorNetworkSourceName.Value;
            }

            // <NetworkEvaluatorCLSID>
            XPathNavigator navigatorNetworkEvaluatorCLSID = navigator.SelectSingleNode("NetworkEvaluatorCLSID");
            if (navigatorNetworkEvaluatorCLSID != null) {
                this._networkEvaluatorCLSID = navigatorNetworkEvaluatorCLSID.Value;
            }

            // <NetworkEdgeDirection>
            XPathNavigator navigatorNetworkEdgeDirection = navigator.SelectSingleNode("NetworkEdgeDirection");
            if (navigatorNetworkEdgeDirection != null) {
                this._networkEdgeDirection = (esriNetworkEdgeDirection)Enum.Parse(typeof(esriNetworkEdgeDirection), navigatorNetworkEdgeDirection.Value, true);
            }

            // <NetworkEvaluatorData><PropertyArray><PropertySetProperty>
            this._networkEvaluatorData = new List<Property>();
            XPathNodeIterator interatorProperty = navigator.Select("NetworkEvaluatorData/PropertyArray/PropertySetProperty");
            while (interatorProperty.MoveNext()) {
                // Get <PropertySetProperty>
                XPathNavigator navigatorProperty = interatorProperty.Current;

                // Add PropertySetProperty
                Property property = new Property(navigatorProperty);
                this._networkEvaluatorData.Add(property);
            }
        }
        public NetworkAssignment(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._isDefault = info.GetBoolean("isDefault");
            this._id = info.GetInt32("id");
            this._networkAttributeName = info.GetString("networkAttributeName");
            this._networkElementType = (esriNetworkElementType)Enum.Parse(typeof(esriNetworkElementType), info.GetString("networkElementType"), true);
            this._networkSourceName = info.GetString("networkSourceName");
            this._networkEvaluatorCLSID = info.GetString("networkEvaluatorCLSID");
            this._networkEdgeDirection = (esriNetworkEdgeDirection)Enum.Parse(typeof(esriNetworkEdgeDirection), info.GetString("networkEdgeDirection"), true);
            this._networkEvaluatorData = (List<Property>)info.GetValue("networkEvaluatorData", typeof(List<Property>));
        }
        public NetworkAssignment(NetworkAssignment prototype) : base(prototype) {
            this._isDefault = prototype.IsDefault;
            this._id = prototype.ID;
            this._networkAttributeName = prototype.NetworkAttributeName;
            this._networkElementType = prototype.NetworkElementType;
            this._networkSourceName = prototype.NetworkSourceName;
            this._networkEvaluatorCLSID = prototype.NetworkEvaluatorCLSID;
            this._networkEdgeDirection = prototype.NetworkEdgeDirection;
            this._networkEvaluatorData = new List<Property>();
            foreach (Property property in prototype.NetworkEvaluatorDataCollection) {
                Property propertyClone = (Property)property.Clone();
                this._networkEvaluatorData.Add(propertyClone);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Indicates whether it is the default evaluator context
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue(false)]
        [Description("Indicates whether it is the default evaluator context")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsDefault {
            get { return this._isDefault; }
            set { this._isDefault = value; }
        }
        /// <summary>
        /// Unique identifier of this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue(0)]
        [Description("Unique identifier of this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int ID {
            get { return this._id; }
            set { this._id = value; }
        }
        /// <summary>
        /// Name of the class associated with this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue("")]
        [Description("Name of the class associated with this network source")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string NetworkAttributeName {
            get { return this._networkAttributeName; }
            set { this._networkAttributeName = value; }
        }
        /// <summary>
        /// The evaluator element type
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue(esriNetworkElementType.esriNETJunction)]
        [Description("The evaluator element type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkElementType NetworkElementType {
            get { return this._networkElementType; }
            set { this._networkElementType = value; }
        }
        /// <summary>
        /// The evaluator network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue("")]
        [Description("The evaluator network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string NetworkSourceName {
            get { return this._networkSourceName; }
            set { this._networkSourceName = value; }
        }
        /// <summary>
        /// Network Evalutor Class Id
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue("")]
        [Description("Network Evalutor Class Id")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string NetworkEvaluatorCLSID {
            get { return this._networkEvaluatorCLSID; }
            set { this._networkEvaluatorCLSID = value; }
        }
        /// <summary>
        /// The edge direction type
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue(esriNetworkEdgeDirection.esriNEDNone)]
        [Description("The edge direction type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkEdgeDirection NetworkEdgeDirection {
            get { return this._networkEdgeDirection; }
            set { this._networkEdgeDirection = value; }
        }
        /// <summary>
        /// Collection of Network Evaluator Data
        /// </summary>
        [Browsable(true)]
        [Category("Network Assignment")]
        [DefaultValue(null)]
        [Description("Collection of Network Evaluator Data")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> NetworkEvaluatorDataCollection {
            get { return this._networkEvaluatorData; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("isDefault", this._isDefault);
            info.AddValue("id", this._id);
            info.AddValue("networkAttributeName", this._networkAttributeName);
            info.AddValue("networkElementType", Convert.ToInt32(this._networkElementType).ToString());
            info.AddValue("networkSourceName", this._networkSourceName);
            info.AddValue("networkEvaluatorCLSID", this._networkEvaluatorCLSID);
            info.AddValue("networkEdgeDirection", Convert.ToInt32(this._networkEdgeDirection).ToString());
            info.AddValue("networkEvaluatorData", this._networkEvaluatorData, typeof(List<Property>));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Attribute Parameter Errors
        }
        public override object Clone() {
            return new NetworkAssignment(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetworkAssignment>
            writer.WriteStartElement("NetworkAssignment");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetworkAssignment");

            // <IsDefault></IsDefault>
            writer.WriteStartElement("IsDefault");
            writer.WriteValue(this._isDefault);
            writer.WriteEndElement();

            // <ID></ID>
            writer.WriteStartElement("ID");
            writer.WriteValue(this._id);
            writer.WriteEndElement();

            // <NetworkAttributeName></NetworkAttributeName>
            writer.WriteStartElement("NetworkAttributeName");
            writer.WriteValue(this._networkAttributeName);
            writer.WriteEndElement();

            if (this._isDefault) {
                // <NetworkElementType></NetworkElementType>
                writer.WriteStartElement("NetworkElementType");
                writer.WriteValue(this._networkElementType.ToString());
                writer.WriteEndElement();
            }
            else {
                // <NetworkSourceName></NetworkSourceName>
                writer.WriteStartElement("NetworkSourceName");
                writer.WriteValue(this._networkSourceName);
                writer.WriteEndElement();
            }

            // <NetworkEvaluatorCLSID></NetworkEvaluatorCLSID>
            writer.WriteStartElement("NetworkEvaluatorCLSID");
            writer.WriteValue(this._networkEvaluatorCLSID);
            writer.WriteEndElement();

            // <NetworkEdgeDirection></NetworkEdgeDirection>
            writer.WriteStartElement("NetworkEdgeDirection");
            writer.WriteValue(this._networkEdgeDirection.ToString());
            writer.WriteEndElement();

            // <NetworkEvaluatorData>
            writer.WriteStartElement("NetworkEvaluatorData");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySet");

            // <PropertyArray>
            writer.WriteStartElement("PropertyArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfPropertySetProperty");

            // <PropertySetProperty></PropertySetProperty>
            foreach (Property property in this._networkEvaluatorData) {
                property.WriteXml(writer);
            }

            // </PropertyArray>
            writer.WriteEndElement();

            // </NetworkEvaluatorData>
            writer.WriteEndElement();

            // </NetworkAssignment>
            writer.WriteEndElement();
        }
    }
}

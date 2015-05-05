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
    /// ESRI Network Source
    /// </summary>
    [Serializable]
    public abstract class NetworkSource : EsriObject {
        private int _id = 0;
        private int _classId = 0;
        private string _name = string.Empty;
        private esriNetworkElementType _elementType = esriNetworkElementType.esriNETTurn;
        private List<Property> _properties = null;
        private NetworkSourceDirections _networkSourceDirections = null;
        //
        // CONSTRUCTOR
        //
        public NetworkSource(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <ID></ID>
            XPathNavigator navigatorID = navigator.SelectSingleNode("ID");
            if (navigatorID != null) {
                this._id = navigatorID.ValueAsInt;
            }

            // <ClassID></ClassID>
            XPathNavigator navigatorClassID = navigator.SelectSingleNode("ClassID");
            if (navigatorClassID != null) {
                this._classId = navigatorClassID.ValueAsInt;
            }

            // <Name></Name>
            XPathNavigator navigatorName = navigator.SelectSingleNode("Name");
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // <ElementType></ElementType>
            XPathNavigator navigatorElementType = navigator.SelectSingleNode("ElementType");
            if (navigatorElementType != null) {
                this._elementType = (esriNetworkElementType)Enum.Parse(typeof(esriNetworkElementType), navigatorElementType.Value, true);
            }

            // <Properties><Property><Property><Properties>
            this._properties = new List<Property>();
            XPathNodeIterator interatorProperty = navigator.Select("Properties/Property");
            while (interatorProperty.MoveNext()) {
                // Get <Property>
                XPathNavigator navigatorProperty = interatorProperty.Current;

                // Add Property
                Property property = new Property(navigatorProperty);
                this._properties.Add(property);
            }

            // <NetworkSourceDirections>
            XPathNavigator navigatorNetworkSourceDirections = navigator.SelectSingleNode("NetworkSourceDirections");
            if (navigatorNetworkSourceDirections != null) {
                this._networkSourceDirections = new NetworkSourceDirections(navigatorNetworkSourceDirections);
            }
        }
        public NetworkSource(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._id = info.GetInt32("id");
            this._classId = info.GetInt32("classId");
            this._name = info.GetString("name");
            this._elementType = (esriNetworkElementType)Enum.Parse(typeof(esriNetworkElementType), info.GetString("elementType"), true);
            this._properties = (List<Property>)info.GetValue("properties", typeof(List<Property>));
            this._networkSourceDirections = (NetworkSourceDirections)info.GetValue("networkSourceDirections", typeof(NetworkSourceDirections));
        }
        public NetworkSource(NetworkSource prototype) : base(prototype) {
            this._id = prototype.ID;
            this._classId = prototype.ClassId;
            this._name = prototype.Name;
            this._elementType = prototype.ElementType;
            this._properties = new List<Property>();
            foreach (Property property in prototype.PropertyCollection) {
                Property propertyClone = (Property)property.Clone();
                this._properties.Add(propertyClone);
            }
            if (prototype.NetworkSourceDirections != null) {
                this._networkSourceDirections = (NetworkSourceDirections)prototype.NetworkSourceDirections.Clone();
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Unique identifier of this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue(0)]
        [Description("Unique identifier of this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int ID {
            get { return this._id; }
            set { this._id = value; }
        }
        /// <summary>
        /// The class id of this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue("")]
        [Description("The class id of this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int ClassId {
            get { return this._classId; }
            set { this._classId = value; }
        }
        /// <summary>
        /// Name of the class associated with this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue("")]
        [Description("Name of the class associated with this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// Network element type of this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue("")]
        [Description("Network element type of this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkElementType ElementType {
            get { return this._elementType; }
            set { this._elementType = value; }
        }
        /// <summary>
        /// Property set of this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue("")]
        [Description("Property set of this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> PropertyCollection {
            get { return this._properties; }
        }
        /// <summary>
        /// The driving directions settings for this network source
        /// </summary>
        [Browsable(true)]
        [Category("Network Source")]
        [DefaultValue("")]
        [Description("The driving directions settings for this network source")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public NetworkSourceDirections NetworkSourceDirections {
            get { return this._networkSourceDirections; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("id", this._id);
            info.AddValue("classId", this._classId);
            info.AddValue("name", this._name);
            info.AddValue("elementType", Convert.ToInt32(this._elementType).ToString());
            info.AddValue("properties", this._properties, typeof(List<Property>));
            info.AddValue("networkSourceDirections", this._networkSourceDirections, typeof(NetworkSourceDirections));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Direction Errors
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            // <ID></ID>
            writer.WriteStartElement("ID");
            writer.WriteValue(this._id);
            writer.WriteEndElement();

            // <ClassID></ClassID>
            writer.WriteStartElement("ClassID");
            writer.WriteValue(this._classId);
            writer.WriteEndElement();

            // <Name></Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <ElementType></ElementType>
            writer.WriteStartElement("ElementType");
            writer.WriteValue(this._elementType.ToString());
            writer.WriteEndElement();

            if (this._properties.Count == 0) {
                // <Properties></Properties>
                writer.WriteStartElement("Properties");
                writer.WriteAttributeString(Xml._XSI, "nil", null, "true");
                writer.WriteEndElement();
            }
            else {
                // <Properties>
                writer.WriteStartElement("Properties");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Properties");

                // <Property></Property>
                foreach (Property property in this._properties) {
                    property.WriteXml(writer);
                }

                // </Properties>
                writer.WriteEndElement();

                // <NetworkSourceDirections></NetworkSourceDirections>
                if (this._networkSourceDirections != null) {
                    this._networkSourceDirections.WriteXml(writer);
                }
            }
        }
    }
}

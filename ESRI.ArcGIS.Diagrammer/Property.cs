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

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Property
    /// </summary>
    /// <remarks>
    /// Used to store a key/value pair. ObjectClasses can contain one or more Properties.
    /// </remarks>
    [Serializable]
    public class Property : EsriObject {
        private string _key = string.Empty;
        private string _type = string.Empty;
        private string _value = string.Empty;
        //
        // CONSTRUCTOR
        //
        public Property() : base() { }
        public Property(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // Key
            XPathNavigator navigatorKey = navigator.SelectSingleNode("Key");
            if (navigatorKey != null) {
                this._key = navigatorKey.Value;
            }

            // Type & Value
            XPathNavigator navigatorValue = navigator.SelectSingleNode("Value");
            if (navigatorValue == null) {
                // Set VALUE to null if the node does not exist
                this._value = null;
            }
            else {
                // Type
                this._type = navigatorValue.GetAttribute(Xml._TYPE, Xml.XMLSCHEMAINSTANCE);

                // Value
                if (this._type == "esri:XMLPersistedObject") {
                    XPathNavigator navigatorBytes = navigatorValue.SelectSingleNode("Bytes");
                    if (navigatorBytes != null) {
                        this._value = navigatorBytes.Value;
                    }
                }
                else {
                    this._value = navigatorValue.Value;
                }
            }
        }
        public Property(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._key = info.GetString("key");
            this._type = info.GetString(Xml._TYPE);
            this._value = info.GetString("value");
        }
        public Property(Property prototype) : base(prototype) {
            this._key = prototype.Key;
            this._type = prototype.Type;
            this._value = prototype.Value;
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Property")]
        [DefaultValue(0)]
        [Description("Property Key")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Key {
            get { return this._key; }
            set { this._key = value; }
        }
        [Browsable(true)]
        [Category("Property")]
        [DefaultValue(0)]
        [Description("Property Type")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Type {
            get { return this._type; }
            set { this._type = value; }
        }
        [Browsable(true)]
        [Category("Property")]
        [DefaultValue(0)]
        [Description("Propety Value")]
        [NotifyParentProperty(true)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Value {
            get { return this._value; }
            set { this._value = value; }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("key", this._key);
            info.AddValue(Xml._TYPE, this._type);
            info.AddValue("value", this._value);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Property(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Key cannot be empty
            if (string.IsNullOrEmpty(this._key)) {
                list.Add(new ErrorObject(this, table, "Property Key cannot be empty", ErrorType.Error));
            }

            // Type cannot be empty
            if (string.IsNullOrEmpty(this._type)) {
                list.Add(new ErrorObject(this, table, "Property Type cannot be empty", ErrorType.Error));
            }

            // [Warning]
            if (string.IsNullOrEmpty(this._value)) {
                list.Add(new ErrorObject(this, table, "Property Value is empty", ErrorType.Warning));
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // <PropertySetProperty>
            writer.WriteStartElement("PropertySetProperty");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySetProperty");

            // <Key></Key>
            writer.WriteStartElement("Key");
            writer.WriteValue(this._key);
            writer.WriteEndElement();

            // <Value>
            if (this._value != null) {
                // If value is null then do NOT add the node. Empty string is OK.
                if (!string.IsNullOrEmpty(this._type)) {
                    // Do not proceed if the type attribute is not set. This will cause ArcCatalog to barf.
                    writer.WriteStartElement("Value");
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, this._type);
                    if (this._type == "esri:XMLPersistedObject") {
                        // <Bytes></Bytes>
                        writer.WriteStartElement("Bytes");
                        writer.WriteValue(this._value);
                        writer.WriteEndElement();
                    }
                    else {
                        writer.WriteValue(this._value);
                    }

                    // </Value>
                    writer.WriteEndElement();
                }
            }

            // </PropertySetProperty>
            writer.WriteEndElement();
        }
    }
}

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
    /// ESRI Network Attribute Parameter
    /// </summary>
    [Serializable]
    public class NetworkAttributeParameter : EsriObject {
        private string _name = string.Empty;
        private int _varType = 0;
        private string _value = string.Empty;
        private string _defaultValue = string.Empty;
        //
        // CONSTRUCTOR
        //
        public NetworkAttributeParameter() : base() { }
        public NetworkAttributeParameter(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Name>
            XPathNavigator navigatorName = navigator.SelectSingleNode("Name");
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // <VarType>
            XPathNavigator navigatorVarType = navigator.SelectSingleNode("VarType");
            if (navigatorVarType != null) {
                this._varType = navigatorVarType.ValueAsInt;
            }

            // <Value>
            XPathNavigator navigatorValue = navigator.SelectSingleNode("Value");
            if (navigatorValue != null) {
                this._value = navigatorValue.Value;
            }

            // <DefaultValue>
            XPathNavigator navigatorDefaultValue = navigator.SelectSingleNode("DefaultValue");
            if (navigatorDefaultValue != null) {
                this._defaultValue = navigatorDefaultValue.Value;
            }
        }
        public NetworkAttributeParameter(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._name = info.GetString("name");
            this._varType = info.GetInt32("varType");
            this._value = info.GetString("value");
            this._defaultValue = info.GetString("defaultValue");
        }
        public NetworkAttributeParameter(NetworkAttributeParameter prototype) : base(prototype) {
            this._name = prototype.Name;
            this._varType = prototype.VarType;
            this._value = prototype.Value;
            this._defaultValue = prototype.DefaultValue;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The name of the parameter
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute Parameter")]
        [DefaultValue("")]
        [Description("The name of the parameter")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// The VARTYPE of the parameter (e.g. VT_I4)
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute Parameter")]
        [DefaultValue(0)]
        [Description("The VARTYPE of the parameter (e.g. VT_I4)")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int VarType {
            get { return this._varType; }
            set { this._varType = value; }
        }
        /// <summary>
        /// The current value of the parameter
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute Parameter")]
        [DefaultValue("")]
        [Description("The current value of the parameter")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Value {
            get { return this._value; }
            set { this._value = value; }
        }
        /// <summary>
        /// The default value of the parameter
        /// </summary>
        [Browsable(true)]
        [Category("Network Attribute Parameter")]
        [DefaultValue("")]
        [Description("The default value of the parameter")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string DefaultValue {
            get { return this._defaultValue; }
            set { this._defaultValue = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", this._name);
            info.AddValue("varType", this._varType);
            info.AddValue("value", this._value);
            info.AddValue("defaultValue", this._defaultValue);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Attribute Parameter Errors
        }
        public override object Clone() {
            return new NetworkAttributeParameter(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetworkAttributeParameter>
            writer.WriteStartElement("NetworkAttributeParameter");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetworkAttributeParameter");

            // <Name></Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <VarType></VarType>
            writer.WriteStartElement("VarType");
            writer.WriteValue(this._varType);
            writer.WriteEndElement();

            // <Value></Value>
            writer.WriteStartElement("Value");
            writer.WriteValue(this._value);
            writer.WriteEndElement();

            // <DefaultValue></DefaultValue>
            writer.WriteStartElement("DefaultValue");
            writer.WriteValue(this._defaultValue);
            writer.WriteEndElement();

            // </NetworkAttributeParameter>
            writer.WriteEndElement();
        }
    }
}

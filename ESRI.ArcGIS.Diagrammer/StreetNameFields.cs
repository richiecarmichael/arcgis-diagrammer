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
    /// ESRI Street Name Fields
    /// </summary>
    [Serializable]
    public class StreetNameFields : EsriObject {
        private string _prefixDirectionFieldName = string.Empty;
        private string _prefixTypeFieldName = string.Empty;
        private string _streetNameFieldName = string.Empty;
        private string _suffixDirectionsFieldName = string.Empty;
        private string _suffixTypeFieldName = string.Empty;
        private int _priority = 0;
        //
        // CONSTRUCTOR
        //
        public StreetNameFields() : base() { }
        public StreetNameFields(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <PrefixDirectionFieldName>
            XPathNavigator navigatorPrefixDirectionFieldName = navigator.SelectSingleNode("PrefixDirectionFieldName");
            if (navigatorPrefixDirectionFieldName != null) {
                this._prefixDirectionFieldName = navigatorPrefixDirectionFieldName.Value;
            }

            // <PrefixTypeFieldName>
            XPathNavigator navigatorPrefixTypeFieldName = navigator.SelectSingleNode("PrefixTypeFieldName");
            if (navigatorPrefixTypeFieldName != null) {
                this._prefixTypeFieldName = navigatorPrefixTypeFieldName.Value;
            }

            // <StreetNameFieldName>
            XPathNavigator navigatorStreetNameFieldName = navigator.SelectSingleNode("StreetNameFieldName");
            if (navigatorStreetNameFieldName != null) {
                this._streetNameFieldName = navigatorStreetNameFieldName.Value;
            }

            // <SuffixDirectionsFieldName>
            XPathNavigator navigatorSuffixDirectionsFieldName = navigator.SelectSingleNode("SuffixDirectionsFieldName");
            if (navigatorSuffixDirectionsFieldName != null) {
                this._suffixDirectionsFieldName = navigatorSuffixDirectionsFieldName.Value;
            }

            // <SuffixTypeFieldName>
            XPathNavigator navigatorSuffixTypeFieldName = navigator.SelectSingleNode("SuffixTypeFieldName");
            if (navigatorSuffixTypeFieldName != null) {
                this._suffixTypeFieldName = navigatorSuffixTypeFieldName.Value;
            }

            // <Priority>
            XPathNavigator navigatorPriority = navigator.SelectSingleNode("Priority");
            if (navigatorPriority != null) {
                this._priority = navigatorPriority.ValueAsInt;
            }
        }
        public StreetNameFields(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._prefixDirectionFieldName = info.GetString("prefixDirectionFieldName");
            this._prefixTypeFieldName = info.GetString("prefixTypeFieldName");
            this._streetNameFieldName = info.GetString("streetNameFieldName");
            this._suffixDirectionsFieldName = info.GetString("suffixDirectionsFieldName");
            this._suffixTypeFieldName = info.GetString("suffixTypeFieldName");
            this._priority = info.GetInt32("priority");
        }
        public StreetNameFields(StreetNameFields prototype) : base(prototype) {
            this._prefixDirectionFieldName = prototype.PrefixDirectionFieldName;
            this._prefixTypeFieldName = prototype.PrefixTypeFieldName;
            this._streetNameFieldName = prototype.StreetNameFieldName;
            this._suffixDirectionsFieldName = prototype.SuffixDirectionsFieldName;
            this._suffixTypeFieldName = prototype.SuffixTypeFieldName;
            this._priority = prototype.Priority;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The field name used for prefix direction
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The field name used for prefix direction")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string PrefixDirectionFieldName {
            get { return this._prefixDirectionFieldName; }
            set { this._prefixDirectionFieldName = value; }
        }
        /// <summary>
        /// The field name used for prefix type
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The field name used for prefix type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string PrefixTypeFieldName {
            get { return this._prefixTypeFieldName; }
            set { this._prefixTypeFieldName = value; }
        }
        /// <summary>
        /// The field name used for street name
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The field name used for street name")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string StreetNameFieldName {
            get { return this._streetNameFieldName; }
            set { this._streetNameFieldName = value; }
        }
        /// <summary>
        /// The field name used for suffix direction
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The field name used for suffix direction")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string SuffixDirectionsFieldName {
            get { return this._suffixDirectionsFieldName; }
            set { this._suffixDirectionsFieldName = value; }
        }
        /// <summary>
        /// The field name used for suffix type
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The field name used for suffix type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string SuffixTypeFieldName {
            get { return this._suffixTypeFieldName; }
            set { this._suffixTypeFieldName = value; }
        }
        /// <summary>
        /// The priority for when these street name fields are used
        /// </summary>
        [Browsable(true)]
        [Category("Street Name Fields")]
        [DefaultValue("")]
        [Description("The priority for when these street name fields are used")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Priority {
            get { return this._priority; }
            set { this._priority = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("prefixDirectionFieldName", this._prefixDirectionFieldName);
            info.AddValue("prefixTypeFieldName", this._prefixTypeFieldName);
            info.AddValue("streetNameFieldName", this._streetNameFieldName);
            info.AddValue("suffixDirectionsFieldName", this._suffixDirectionsFieldName);
            info.AddValue("suffixTypeFieldName", this._suffixTypeFieldName);
            info.AddValue("priority", this._priority);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Street Name Fields Errors
        }
        public override object Clone() {
            return new StreetNameFields(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <StreetNameFields>
            writer.WriteStartElement("StreetNameFields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:StreetNameFields");

            // <PrefixDirectionFieldName></PrefixDirectionFieldName>
            writer.WriteStartElement("PrefixDirectionFieldName");
            writer.WriteValue(this._prefixDirectionFieldName);
            writer.WriteEndElement();

            // <PrefixTypeFieldName></PrefixTypeFieldName>
            writer.WriteStartElement("PrefixTypeFieldName");
            writer.WriteValue(this._prefixTypeFieldName);
            writer.WriteEndElement();

            // <StreetNameFieldName></StreetNameFieldName>
            writer.WriteStartElement("StreetNameFieldName");
            writer.WriteValue(this._streetNameFieldName);
            writer.WriteEndElement();

            // <SuffixDirectionsFieldName></SuffixDirectionsFieldName>
            writer.WriteStartElement("SuffixDirectionsFieldName");
            writer.WriteValue(this._suffixDirectionsFieldName);
            writer.WriteEndElement();

            // <SuffixTypeFieldName></SuffixTypeFieldName >
            writer.WriteStartElement("SuffixTypeFieldName");
            writer.WriteValue(this._suffixTypeFieldName);
            writer.WriteEndElement();

            // <Priority></Priority>
            writer.WriteStartElement("Priority");
            writer.WriteValue(this._priority);
            writer.WriteEndElement();

            // </StreetNameFields>
            writer.WriteEndElement();
        }
    }
}

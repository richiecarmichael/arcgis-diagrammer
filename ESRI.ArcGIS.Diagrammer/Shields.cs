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
    /// ESRI Shields
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class Shields : EsriObject {
        private string _typeFieldName = string.Empty;
        private string _numberFieldName = string.Empty;
        private string _combinedFieldName = string.Empty;
        private bool _useCombinedField = false;
        private List<Shield> _shields = null;
        //
        // CONSTRUCTOR
        //
        public Shields() : base() {
            this._shields = new List<Shield>();
        }
        public Shields(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <TypeFieldName>
            XPathNavigator navigatorTypeFieldName = navigator.SelectSingleNode("TypeFieldName");
            if (navigatorTypeFieldName != null) {
                this._typeFieldName = navigatorTypeFieldName.Value;
            }

            // <NumberFieldName>
            XPathNavigator navigatorNumberFieldName = navigator.SelectSingleNode("NumberFieldName");
            if (navigatorNumberFieldName != null) {
                this._numberFieldName = navigatorNumberFieldName.Value;
            }

            // <CombinedFieldName>
            XPathNavigator navigatorCombinedFieldName = navigator.SelectSingleNode("CombinedFieldName");
            if (navigatorCombinedFieldName != null) {
                this._combinedFieldName = navigatorCombinedFieldName.Value;
            }

            // <UseCombinedField>
            XPathNavigator navigatorUseCombinedField = navigator.SelectSingleNode("UseCombinedField");
            if (navigatorUseCombinedField != null) {
                this._useCombinedField = navigatorUseCombinedField.ValueAsBoolean;
            }

            // <ArrayOfShield><Shield></Shield></ArrayOfShield>
            this._shields = new List<Shield>();
            XPathNodeIterator interatorShield = navigator.Select("ArrayOfShield/Shield");
            while (interatorShield.MoveNext()) {
                // Get <Shield>
                XPathNavigator navigatorShield = interatorShield.Current;

                // Add Shield
                Shield shield = new Shield(navigatorShield);
                this._shields.Add(shield);
            }
        }
        public Shields(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._typeFieldName = info.GetString("typeFieldName");
            this._numberFieldName = info.GetString("numberFieldName");
            this._combinedFieldName = info.GetString("combinedFieldName");
            this._useCombinedField = info.GetBoolean("useCombinedField");
            this._shields = (List<Shield>)info.GetValue("shields", typeof(List<Shield>));
        }
        public Shields(Shields prototype) : base(prototype) {
            this._typeFieldName = prototype.TypeFieldName;
            this._numberFieldName = prototype.NumberFieldName;
            this._combinedFieldName = prototype.CombinedFieldName;
            this._useCombinedField = prototype.UseCombinedField;
            this._shields = new List<Shield>();
            foreach (Shield shield in prototype.ShieldCollection) {
                Shield shieldClone = (Shield)shield.Clone();
                this._shields.Add(shieldClone);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The field name whose values contain the street type
        /// </summary>
        [Browsable(true)]
        [Category("Shields")]
        [DefaultValue("")]
        [Description("The field name whose values contain the street type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string TypeFieldName {
            get { return this._typeFieldName; }
            set { this._typeFieldName = value; }
        }
        /// <summary>
        /// The field name whose values contain the house number
        /// </summary>
        [Browsable(true)]
        [Category("Shields")]
        [DefaultValue("")]
        [Description("The field name whose values contain the house number")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string NumberFieldName {
            get { return this._numberFieldName; }
            set { this._numberFieldName = value; }
        }
        /// <summary>
        /// The field name whose values contain the whole address description
        /// </summary>
        [Browsable(true)]
        [Category("Shields")]
        [DefaultValue("")]
        [Description("The field name whose values contain the whole address description")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string CombinedFieldName {
            get { return this._combinedFieldName; }
            set { this._combinedFieldName = value; }
        }
        /// <summary>
        /// Indicates if the combined field should be used for directions
        /// </summary>
        [Browsable(true)]
        [Category("Shields")]
        [DefaultValue("")]
        [Description("Indicates if the combined field should be used for directions")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool UseCombinedField {
            get { return this._useCombinedField; }
            set { this._useCombinedField = value; }
        }
        /// <summary>
        /// Collection of Shields
        /// </summary>
        [Browsable(true)]
        [Category("Shields")]
        [DefaultValue("")]
        [Description("Collection of Shields")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Shield> ShieldCollection {
            get { return this._shields; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("typeFieldName", this._typeFieldName);
            info.AddValue("numberFieldName", this._numberFieldName);
            info.AddValue("combinedFieldName", this._combinedFieldName);
            info.AddValue("useCombinedField", this._useCombinedField);
            info.AddValue("shields", this._shields, typeof(List<Shield>));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Shields Errors
        }
        public override object Clone() {
            return new Shields(this);
        }
        public override void WriteXml(XmlWriter writer) {
            if (this._shields.Count == 0) {
                // <Shields></Shields>
                writer.WriteStartElement("Shields");
                writer.WriteAttributeString(Xml._XSI, "nil", null, "true");
                writer.WriteEndElement();
            }
            else {
                // <Shields>
                writer.WriteStartElement("Shields");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Shields");

                // <TypeFieldName></TypeFieldName>
                writer.WriteStartElement("TypeFieldName");
                writer.WriteValue(this._typeFieldName);
                writer.WriteEndElement();

                // <NumberFieldName></NumberFieldName>
                writer.WriteStartElement("NumberFieldName");
                writer.WriteValue(this._numberFieldName);
                writer.WriteEndElement();

                // <CombinedFieldName></CombinedFieldName>
                writer.WriteStartElement("CombinedFieldName");
                writer.WriteValue(this._combinedFieldName);
                writer.WriteEndElement();

                // <UseCombinedField></UseCombinedField >
                writer.WriteStartElement("UseCombinedField");
                writer.WriteValue(this._useCombinedField);
                writer.WriteEndElement();

                // <ArrayOfShield>
                writer.WriteStartElement("ArrayOfShield");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfShield");

                // <Shield></Shield>
                foreach (Shield shield in this._shields) {
                    shield.WriteXml(writer);
                }

                // </ArrayOfShield>
                writer.WriteEndElement();

                // </Shields>
                writer.WriteEndElement();
            }
        }
    }
}

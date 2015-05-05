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
    /// ESRI Subtype Field
    /// </summary>
    /// <remarks>
    /// Represents a default value or domain override of the base objectclass
    /// </remarks>
    [DefaultPropertyAttribute("FieldName")]
    [Serializable]
    public class SubtypeField : EsriTableRow {
        private string _fieldName = string.Empty;
        private string _domainName = string.Empty;
        private string _defaultValue = string.Empty;
        //
        // CONSTRUCTOR
        //
        public SubtypeField(): base() {
            // Initialize
            this.UpdateText();

            // Set Domain Properties
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public SubtypeField(IXPathNavigable path) : base(path) {
            XPathNavigator navigator = path.CreateNavigator();

            // <FieldName></FieldName>
            XPathNavigator navigatorFieldName = navigator.SelectSingleNode("FieldName");
            if (navigatorFieldName != null) {
                this._fieldName = navigatorFieldName.Value;
            }

            // <DomainName></DomainName>
            XPathNavigator navigatorDomainName = navigator.SelectSingleNode("DomainName");
            if (navigatorDomainName != null) {
                this._domainName = navigatorDomainName.Value;
            }

            // <DefaultValue></DefaultValue>
            XPathNavigator navigatorDefaultValue = navigator.SelectSingleNode("DefaultValue");
            if (navigatorDefaultValue != null) {
                this._defaultValue = navigatorDefaultValue.Value;
            }

            // Initialize
            this.UpdateText();

            // Set Element Properties
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public SubtypeField(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._fieldName = info.GetString("fieldName");
            this._domainName = info.GetString("domainName");
            this._defaultValue = info.GetString("defaultValue");

            // Initialize
            this.UpdateText();

            // Set Element Properties
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public SubtypeField(SubtypeField prototype): base(prototype) {
            this._fieldName = prototype.FieldName;
            this._domainName = prototype.DomainName;
            this._defaultValue = prototype.DefaultValue;

            // Initialize
            this.UpdateText();
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Subtype Field")]
        [DefaultValue("")]
        [Description("Field Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string FieldName {
            get { return this._fieldName; }
            set { this._fieldName = value; this.UpdateText(); }
        }
        [Browsable(true)]
        [Category("Subtype Field")]
        [DefaultValue("")]
        [Description("Domain Name")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(DomainConverter))]
        public string DomainName {
            get { return this._domainName; }
            set { this._domainName = value; this.UpdateText(); }
        }
        [Browsable(true)]
        [Category("Subtype Field")]
        [DefaultValue(null)]
        [Description("Default Value")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string DefaultValue {
            get { return this._defaultValue; }
            set { this._defaultValue = value; this.UpdateText(); }
        }
        //
        // OVERRIDEM METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("fieldName", this._fieldName);
            info.AddValue("domainName", this._domainName);
            info.AddValue("defaultValue", this._defaultValue);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new SubtypeField(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <SubtypeFieldInfo>
            writer.WriteStartElement("SubtypeFieldInfo");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:SubtypeFieldInfo");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </SubtypeFieldInfo>
            writer.WriteEndElement();
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            base.WriteInnerXml(writer);

            // Get Model
            Subtype subtype = (Subtype)this.Table;
            ObjectClass objectClass = subtype.GetParent();

            // <FieldName></FieldName>
            writer.WriteStartElement("FieldName");
            writer.WriteValue(this._fieldName);
            writer.WriteEndElement();

            if (!string.IsNullOrEmpty(this._domainName)) {
                // <DomainName></DomainName>
                writer.WriteStartElement("DomainName");
                writer.WriteValue(this._domainName);
                writer.WriteEndElement();
            }

            // Get Field
            Field field = objectClass.FindField(this._fieldName);

            //
            if (!string.IsNullOrEmpty(this._defaultValue)) {
                // Get correct data type value
                string dataType = string.Empty;
                switch (field.FieldType) {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        dataType = "xs:short";
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        dataType = "xs:int";
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        dataType = "xs:float";
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        dataType = "xs:double";
                        break;
                    case esriFieldType.esriFieldTypeString:
                        dataType = "xs:string";
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        dataType = "xs:dateTime";
                        break;
                }

                // <DefaultValue></DefaultValue>
                writer.WriteStartElement("DefaultValue");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, dataType);
                writer.WriteValue(this._defaultValue);
                writer.WriteEndElement();
            }
        }
        public override void Errors(List<Error> list) {
            // Field Name Null or Empty
            if (string.IsNullOrEmpty(this._fieldName)) {
                list.Add(new ErrorTableRow(this, "Subtype Field names cannot be empty", ErrorType.Error));
            }

            // Get DiagrammerEnvironment Singleton
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;

            // Get Subtype
            Subtype subtype = (Subtype)this.Table;

            // Get ObjectClass
            ObjectClass objectClass = subtype.GetParent();
            if (objectClass == null) {
                // This error is handled by the Subtype class
                return;
            }

            // Get Field
            Field field = objectClass.FindField(this._fieldName);
            if (field == null) {
                // Field is missing in parent ObjectClass
                string message = string.Format("The subtype field [{0}] does not exist in the parent objectclass", this._fieldName);
                list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                return;
            }

            // Warning if parent default value and domain are identical
            if (this._defaultValue == field.DefaultValue &&
                this._domainName == field.Domain) {
                string message = string.Format("The default values and domain for the subtype field [{0}] are identical to those in the parent objectclass", this._fieldName);
                list.Add(new ErrorTableRow(this, message, ErrorType.Warning));
            }

            // Field can only be small int, long in, single, double, text, date, guid
            switch (field.FieldType) {
                case esriFieldType.esriFieldTypeDate:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeGUID:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeString:
                    // OK
                    break;
                case esriFieldType.esriFieldTypeRaster:
                case esriFieldType.esriFieldTypeBlob:
                case esriFieldType.esriFieldTypeGeometry:
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeGlobalID:
                case esriFieldType.esriFieldTypeXML:
                    string message = string.Format("The subtype field [{0}] must use a field of type Date, Double, Guid, Integer, Single, SmallInteger or String", this._fieldName);
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    break;
            }

            // Find Domain
            if (!string.IsNullOrEmpty(this._domainName)) {
                // Get Domain
                Domain domain = schemaModel.FindDomain(this._domainName);

                if (domain == null) {
                    // Domain does not exit
                    string message = string.Format("The domain [{0}] for field [{1}] does not exist", this._domainName, field.Name);
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                }
                else {
                    // Compare domain and field types
                    if (field.FieldType != domain.FieldType) {
                        string message = string.Format("The field [{0}] and assigned domain [{1}] do not have matching field types.", field.Name, this._domainName);
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    }

                    // Check Default Value (
                    if (!string.IsNullOrEmpty(this._defaultValue)) {
                        string message = null;
                        if (!domain.IsValid(this._defaultValue, out message)) {
                            list.Add(new ErrorTableRow(this, message, ErrorType.Warning));
                        }
                    }

                    // Check if a domain value is too long for the text field
                    if (field.FieldType == esriFieldType.esriFieldTypeString &&
                        domain.FieldType == esriFieldType.esriFieldTypeString &&
                        domain.GetType() == typeof(DomainCodedValue)) {
                        DomainCodedValue domain2 = (DomainCodedValue)domain;
                        foreach (DomainCodedValueRow x in domain2.CodedValues) {
                            if (string.IsNullOrEmpty(x.Code)) { continue; }
                            if (x.Code.Length > field.Length) {
                                string message = string.Format("The domain [{0}] has a value [{1}] that is too long for the field [{2}]", this._domainName, x, this._fieldName);
                                list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                            }
                        }
                    }
                }
            }

            // Check validity of default value against field type
            if (!string.IsNullOrEmpty(this._defaultValue)) {
                string message;
                if (!GeodatabaseUtility.IsValidateValue(field.FieldType, this._defaultValue, out message)) {
                    string message2 = string.Format("Default value [{0}] {1}", this._defaultValue, message);
                    list.Add(new ErrorTableRow(this, message2, ErrorType.Error));
                }
            }

            //
            if (!string.IsNullOrEmpty(this._defaultValue)) {
                if (!string.IsNullOrEmpty(this._domainName)) {
                    if (!string.IsNullOrEmpty(field.Domain)) {
                        if (this._domainName != field.Domain) {
                            Domain domain2 = schemaModel.FindDomain(field.Domain);
                            string message = null;
                            if (!domain2.IsValid(this._defaultValue, out message)) {
                                string message2 = string.Format("NIM013605: Field [{0}] - {1}", this._fieldName, message);
                                list.Add(new ErrorTableRow(this, message2, ErrorType.Error));
                            }
                        }
                    }
                }
            }
        }
        public override void UpdateText() {
            this.Text = this._fieldName;
        }
    }
}

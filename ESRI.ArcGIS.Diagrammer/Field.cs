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
    /// ESRI Field
    /// </summary>
    [Serializable]
    public class Field : EsriTableRow {
        private string _name = string.Empty;
        private esriFieldType _fieldType = esriFieldType.esriFieldTypeString;
        private bool _isNullable = true;
        private int _length = 0;
        private int _precision = 0;
        private int _scale = 0;
        private bool _required = false;
        private bool _editable = true;
        private bool _domainFixed = false;
        private GeometryDef _geometryDef = null;
        private string _aliasName = string.Empty;
        private string _modelName = string.Empty;
        private string _defaultValue = string.Empty;
        private string _domain = string.Empty;
        private RasterDef _rasterDef = null;
        //
        // CONSTRUCTOR
        //
        public Field() : base() {
            //
            this.SuspendEvents = true;

            // Initialize
            this.UpdateText();

            // Set Element Properties
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");

            //
            this.SuspendEvents = false;
        }
        public Field(IXPathNavigable path, EsriTable table) : base(path) {
            //
            this.SuspendEvents = true;

            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Name></Name>
            XPathNavigator navigatorFieldName = navigator.SelectSingleNode("Name");
            if (navigatorFieldName != null) {
                this._name = navigatorFieldName.Value;
            }

            // <Type></Type>
            XPathNavigator navigatorFieldType = navigator.SelectSingleNode("Type");
            if (navigatorFieldType != null) {
                this._fieldType = (esriFieldType)Enum.Parse(typeof(esriFieldType), navigatorFieldType.Value, true);
            }

            // <IsNullable></IsNullable>
            XPathNavigator navigatorFieldIsNullable = navigator.SelectSingleNode("IsNullable");
            if (navigatorFieldIsNullable != null) {
                this._isNullable = navigatorFieldIsNullable.ValueAsBoolean;
            }

            // <Length></Length>
            XPathNavigator navigatorFieldLength = navigator.SelectSingleNode("Length");
            if (navigatorFieldLength != null) {
                this._length = navigatorFieldLength.ValueAsInt;
            }

            // <Precision></Precision>
            XPathNavigator navigatorFieldPrecision = navigator.SelectSingleNode("Precision");
            if (navigatorFieldPrecision != null) {
                this._precision = navigatorFieldPrecision.ValueAsInt;
            }

            // <Scale></Scale>
            XPathNavigator navigatorFieldScale = navigator.SelectSingleNode("Scale");
            if (navigatorFieldScale != null) {
                this._scale = navigatorFieldScale.ValueAsInt;
            }

            // <Required></Required>
            XPathNavigator navigatorFieldRequired = navigator.SelectSingleNode("Required");
            if (navigatorFieldRequired != null) {
                this._required = navigatorFieldRequired.ValueAsBoolean;
            }

            // <Editable></Editable>
            XPathNavigator navigatorFieldEditable = navigator.SelectSingleNode("Editable");
            if (navigatorFieldEditable != null) {
                this._editable = navigatorFieldEditable.ValueAsBoolean;
            }

            // <DomainFixed></DomainFixed>
            XPathNavigator navigatorFieldDomainFixed = navigator.SelectSingleNode("DomainFixed");
            if (navigatorFieldDomainFixed != null) {
                this._domainFixed = navigatorFieldDomainFixed.ValueAsBoolean;
            }

            // <AliasName></AliasName>
            XPathNavigator navigatorFieldAliasName = navigator.SelectSingleNode("AliasName");
            if (navigatorFieldAliasName != null) {
                this._aliasName = navigatorFieldAliasName.Value;
            }

            // <GeometryDef></GeometryDef>
            XPathNavigator navigatorGeometryDef = navigator.SelectSingleNode("GeometryDef");
            if (navigatorGeometryDef != null) {
                this._geometryDef = new GeometryDef(navigatorGeometryDef);
            }

            // <ModelName></ModelName>
            XPathNavigator navigatorFieldModelName = navigator.SelectSingleNode("ModelName");
            if (navigatorFieldModelName != null) {
                this._modelName = navigatorFieldModelName.Value;
            }

            // <DefaultValue></DefaultValue>
            XPathNavigator navigatorFieldDefaultValue = navigator.SelectSingleNode("DefaultValue");
            if (navigatorFieldDefaultValue != null) {
                this._defaultValue = navigatorFieldDefaultValue.Value;
            }

            // <Domain></Domain>
            XPathNavigator navigatorFieldDomain= navigator.SelectSingleNode("Domain/DomainName");
            if (navigatorFieldDomain != null) {
                this._domain = navigatorFieldDomain.Value;
            }

            // <RasterDef></RasterDef>
            XPathNavigator navigatorRasterDef = navigator.SelectSingleNode("RasterDef");
            if (navigatorRasterDef != null) {
                this._rasterDef = new RasterDef(navigatorRasterDef);
            }

            // Initialize Text
            this.UpdateText();

            // Set Element Properties
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");

            //
            this.SuspendEvents = false;
        }
        public Field(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._name = info.GetString("name");
            this._fieldType = (esriFieldType)Enum.Parse(typeof(esriFieldType), info.GetString("fieldType"), true);
            this._isNullable = info.GetBoolean("isNullable");
            this._length = info.GetInt32("length");
            this._precision = info.GetInt32("precision");
            this._scale = info.GetInt32("scale");
            this._required = info.GetBoolean("required");
            this._editable = info.GetBoolean("editable");
            this._domainFixed = info.GetBoolean("domainFixed");
            this._geometryDef = (GeometryDef)info.GetValue("geometryDef", typeof(GeometryDef));
            this._aliasName = info.GetString("aliasName");
            this._modelName = info.GetString("modelName");
            this._defaultValue = info.GetString("defaultValue");
            this._domain = info.GetString("domain");
            this._rasterDef = (RasterDef)info.GetValue("rasterDef", typeof(RasterDef));
                        
            this.UpdateText();

            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public Field(Field prototype) : base(prototype) {
            this._name = prototype.Name;
            this._fieldType = prototype.FieldType;
            this._isNullable = prototype.IsNullable;
            this._length = prototype.Length;
            this._precision = prototype.Precision;
            this._scale = prototype.Scale;
            this._required = prototype.Required;
            this._editable = prototype.Editable;
            this._domainFixed = prototype.DomainFixed;
            if (prototype.GeometryDef != null) {
                this._geometryDef = (GeometryDef)prototype.GeometryDef.Clone();
            }
            this._aliasName = prototype.AliasName;
            this._modelName = prototype.ModelName;
            this._defaultValue = prototype.DefaultValue;
            this._domain = prototype.Domain;
            if (prototype.RasterDef != null) {
                this._rasterDef = (RasterDef)prototype.RasterDef.Clone();
            }
            
            this.UpdateText();

            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("Field Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; this.UpdateText(); }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(esriFieldType.esriFieldTypeString)]
        [Description("The type of the field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriFieldType FieldType {
            get { return this._fieldType; }
            set { this._fieldType = value; }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(true)]
        [Description("Indicates if the field can contain null values. ")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsNullable {
            get { return this._isNullable; }
            set { this._isNullable = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(0)]
        [Description("The maximum length, in bytes, for values described by the field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Length {
            get { return this._length; }
            set { this._length = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(0)]
        [Description("The precision for field values")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Precision {
            get { return this._precision; }
            set { this._precision = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(0)]
        [Description("The scale for field values")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Scale {
            get { return this._scale; }
            set { this._scale = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(false)]
        [Description("Indicates if the field is required. ")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Required {
            get { return this._required; }
            set { this._required = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(true)]
        [Description("Indicates if the field is editable")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Editable {
            get { return this._editable; }
            set { this._editable = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(false)]
        [Description("Indicates if the field's domain is fixed")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool DomainFixed {
            get { return this._domainFixed; }
            set { this._domainFixed = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("Geometry Definition")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public GeometryDef GeometryDef {
            get { return this._geometryDef; }
            set { this._geometryDef = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("Field Alias Name")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string AliasName {
            get { return this._aliasName; }
            set { this._aliasName = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("The model name of the field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ModelName {
            get { return this._modelName; }
            set { this._modelName = value;  }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("The default value of the field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string DefaultValue {
            get { return this._defaultValue; }
            set { this._defaultValue = value; }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue("")]
        [Description("The default domain of the field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(DomainConverter))]
        public string Domain {
            get { return this._domain; }
            set { this._domain = value; }
        }
        [Browsable(true)]
        [Category("Field")]
        [DefaultValue(null)]
        [Description("Raster Definition")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public RasterDef RasterDef {
            get { return this._rasterDef; }
            set { this._rasterDef = value;  }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", this._name);
            info.AddValue("fieldType", this._fieldType.ToString("d"));
            info.AddValue("isNullable", this._isNullable);
            info.AddValue("length", this._length);
            info.AddValue("precision", this._precision);
            info.AddValue("scale", this._scale);
            info.AddValue("required", this._required);
            info.AddValue("editable", this._editable);
            info.AddValue("domainFixed", this._domainFixed);
            info.AddValue("geometryDef", this._geometryDef, typeof(GeometryDef));
            info.AddValue("aliasName", this._aliasName);
            info.AddValue("modelName", this._modelName);
            info.AddValue("defaultValue", this._defaultValue);
            info.AddValue("domain", this._domain);
            info.AddValue("rasterDef", this._rasterDef, typeof(RasterDef));
            
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Field(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Field>
            writer.WriteStartElement("Field");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Field");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </Field>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Parent ObjectClass
            ObjectClass objectClass = (ObjectClass)this.Table;

            // Get ObjectClass Fields
            List<Field> fields = objectClass.GetFields();

            // Get Schema Model
            SchemaModel model = (SchemaModel)objectClass.Container;

            // Add GeometryDef Errors
            if (this._geometryDef != null) {
                this._geometryDef.Errors(list, (EsriTable)this.Table);
            }

            // GeometryDef only valid for Geometry Fields
            switch (this._fieldType) {
                case esriFieldType.esriFieldTypeGeometry:
                    if (this._geometryDef == null) {
                        list.Add(new ErrorTableRow(this, "Geometry Fields Must have a GeometryDef defined.", ErrorType.Error));
                    }
                    break;
                case esriFieldType.esriFieldTypeBlob:
                case esriFieldType.esriFieldTypeDate:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeGlobalID:
                case esriFieldType.esriFieldTypeGUID:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeRaster:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeString:
                case esriFieldType.esriFieldTypeXML:
                    if (this._geometryDef != null) {
                        list.Add(new ErrorTableRow(this, "Only Geometry Fields can have a GeometryDef defined.", ErrorType.Error));
                    }
                    break;
            }

            // Raster Fields can have a RasterDef
            switch (this._fieldType) {
                case esriFieldType.esriFieldTypeRaster:
                    if (this._rasterDef == null) {
                        string message = string.Format("The raster field [{0}] does not have a RasterDef assigned", this._name);
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    }
                    break;
                case esriFieldType.esriFieldTypeBlob:
                case esriFieldType.esriFieldTypeDate:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeGeometry:
                case esriFieldType.esriFieldTypeGUID:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeString:
                case esriFieldType.esriFieldTypeGlobalID:
                case esriFieldType.esriFieldTypeXML:
                    if (this._rasterDef != null) {
                        string message = string.Format("The field [{0}] is invalid. Only raster fields can have a RasterDef", this._name);
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    }
                    break;
            }

            // Field Name Null or Empty
            if (string.IsNullOrEmpty(this._name)) {
                list.Add(new ErrorTableRow(this, "Field names cannot be empty", ErrorType.Error));
            }	

            // Validate Field Name
            if (!string.IsNullOrEmpty(this._name)) {
                // Get Validator
                Validator validator = WorkspaceValidator.Default.Validator;
                string message = null;
                if (!validator.ValidateFieldName(this._name, out message)) {
                    string message2 = string.Format("Field [{0}] {1}", this._name, message);
                    list.Add(new ErrorTableRow(this, message2, ErrorType.Error));
                }
            }

            // Alias name more than 255 characters long
            if (!string.IsNullOrEmpty(this._aliasName)) {
                if (this._aliasName.Length > 255) {
                    string message = string.Format("Field [{0}]. Alias name cannot be longer than 255 characters", this._name);
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                }
            }

            // Find Domain
            Domain domain = null;
            if (!string.IsNullOrEmpty(this._domain)) {
                domain = model.FindDomain(this._domain);

                if (domain == null) {
                    // Domain does not exit
                    string message = string.Format("The domain [{0}] for field [{1}] does not exist", this._domain, this._name);
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                }
                else {
                    // Compare domain and field types
                    if (this._fieldType != domain.FieldType) {
                        string message = string.Format("The field [{0}] and assigned domain [{1}] do not have matching field types", this._name, this._domain);
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    }

                    // Check Default Value
                    if (!string.IsNullOrEmpty(this._defaultValue)) {
                        string message = null;
                        if (!domain.IsValid(this._defaultValue, out message)) {
                            list.Add(new ErrorTableRow(this, message, ErrorType.Warning));
                        }
                    }

                    // Check if a domain value is too long for the text field
                    if (this._fieldType == esriFieldType.esriFieldTypeString &&
                        domain.FieldType == esriFieldType.esriFieldTypeString &&
                        domain.GetType() == typeof(DomainCodedValue)) {
                        DomainCodedValue domain2 = (DomainCodedValue)domain;
                        foreach (DomainCodedValueRow x in domain2.CodedValues) {
                            if (string.IsNullOrEmpty(x.Code)) { continue; }
                            if (x.Code.Length > this._length) {
                                string message = string.Format("The domain [{0}] has a value [{1}] that is too long for the field [{2}]", this._domain, x, this._name);
                                list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                            }
                        }
                    }
                }
            }

            // Check validity of default value against field type
            if (!string.IsNullOrEmpty(this._defaultValue)) {
                string message;
                if (!GeodatabaseUtility.IsValidateValue(this._fieldType, this._defaultValue, out message)) {
                    string message2 = string.Format("Default value '{0}' {1}", this._defaultValue, message);
                    list.Add(new ErrorTableRow(this, message2, ErrorType.Error));
                }
            }

            // Check for unsupported field types
            switch (this._fieldType) {
                case esriFieldType.esriFieldTypeBlob:
                case esriFieldType.esriFieldTypeDate:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeGeometry:
                case esriFieldType.esriFieldTypeGUID:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeRaster:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeString:
                case esriFieldType.esriFieldTypeGlobalID:
                    break;
                case esriFieldType.esriFieldTypeXML:
                    string message = string.Format("Field type '{0}' is unsupported", this._fieldType.ToString());
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                    break;
            }

            // Length must be > 0
            if (this._length < 0) {
                list.Add(new ErrorTableRow(this, "Field length cannot be less than zero", ErrorType.Error));
            }

            // ModelName cannot be longer than 255 characters
            if (!(string.IsNullOrEmpty(this._modelName))) {
                if (this._modelName.Length > 255) {
                    list.Add(new ErrorTableRow(this, "Model name cannot be greater than 255 characters long", ErrorType.Error));
                }
            }

            // Precision must be > 0
            if (this._precision < 0) {
                list.Add(new ErrorTableRow(this, "Field precision cannot be less than zero", ErrorType.Error));
            }

            // Scale must be > 0
            if (this._scale < 0) {
                list.Add(new ErrorTableRow(this, "Field scake cannot be less than zero", ErrorType.Error));
            }

            // IsNullable
            if (this._isNullable) {
                switch (this._fieldType) {
                    case esriFieldType.esriFieldTypeBlob:
                    case esriFieldType.esriFieldTypeDate:
                    case esriFieldType.esriFieldTypeDouble:
                    case esriFieldType.esriFieldTypeGeometry:
                    case esriFieldType.esriFieldTypeGUID:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeRaster:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeString:
                    case esriFieldType.esriFieldTypeXML:
                        break;
                    case esriFieldType.esriFieldTypeGlobalID:
                    case esriFieldType.esriFieldTypeOID:
                        string message = string.Format("Field type '{0}' cannot have IsNullable = True", this._fieldType.ToString());
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        break;
                }
            }
        }
        public override void UpdateText() {
            this.Text = this._name;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // Get Model
            SchemaModel model = DiagrammerEnvironment.Default.SchemaModel;

            // <Name></Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <Type></Type>
            writer.WriteStartElement("Type");
            writer.WriteValue(this._fieldType.ToString());
            writer.WriteEndElement();

            // <IsNullable></IsNullable>
            writer.WriteStartElement("IsNullable");
            writer.WriteValue(this._isNullable);
            writer.WriteEndElement();

            // <Length></Length>
            writer.WriteStartElement("Length");
            writer.WriteValue(this._length);
            writer.WriteEndElement();

            // <Precision></Precision>
            writer.WriteStartElement("Precision");
            writer.WriteValue(this._precision);
            writer.WriteEndElement();

            // <Scale></Scale>
            writer.WriteStartElement("Scale");
            writer.WriteValue(this._scale);
            writer.WriteEndElement();

            // <Required></Required>
            if (this._required) {
                writer.WriteStartElement("Required");
                writer.WriteValue(this._required);
                writer.WriteEndElement();
            }

            // <Editable></Editable>
            if (!this._editable) {
                writer.WriteStartElement("Editable");
                writer.WriteValue(this._editable);
                writer.WriteEndElement();
            }

            // <DomainFixed></DomainFixed>
            if (this._domainFixed) {
                writer.WriteStartElement("DomainFixed");
                writer.WriteValue(this._domainFixed);
                writer.WriteEndElement();
            }

            // <GeometryDef></GeometryDef>
            if (this._geometryDef != null) {
                this._geometryDef.WriteXml(writer);
            }

            // <AliasName></AliasName>
            if (!string.IsNullOrEmpty(this._aliasName)) {
                writer.WriteStartElement("AliasName");
                writer.WriteValue(this._aliasName);
                writer.WriteEndElement();
            }

            // <ModelName></ModelName>
            if (!string.IsNullOrEmpty(this._modelName)) {
                writer.WriteStartElement("ModelName");
                writer.WriteValue(this._modelName);
                writer.WriteEndElement();
            }

            // <DefaultValue></DefaultValue>
            if (!string.IsNullOrEmpty(this._defaultValue)) {
                writer.WriteStartElement("DefaultValue");
                switch (this._fieldType) {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:short");
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:int");
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:float");
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:double");
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:dateTime");
                        break;
                    case esriFieldType.esriFieldTypeString:
                        writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "xs:string");
                        break;
                    default:
                        break;
                }
                writer.WriteValue(this._defaultValue);
                writer.WriteEndElement();
            }

            // <Domain></Domain>
            if (!string.IsNullOrEmpty(this._domain)) {
                //SchemaModel model = (SchemaModel)this.Table.Container;
                Domain domain = model.FindDomain(this._domain);
                if (domain != null) {
                    domain.WriteXml(writer);
                }
            }

            // <RasterDef></RasterDef>
            if (this._rasterDef != null) {
                this._rasterDef.WriteXml(writer);
            }
        }
    }
}

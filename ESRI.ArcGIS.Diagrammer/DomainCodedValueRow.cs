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
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class DomainCodedValueRow : EsriTableRow {
        private string _code = string.Empty;
        private string _name = string.Empty;
        //
        // CONSTRUCTOR
        //
        public DomainCodedValueRow() : base() {
            this.UpdateText();
        }
        public DomainCodedValueRow(IXPathNavigable path) : base(path) {
            XPathNavigator navigator = path.CreateNavigator();

            // <Code></Code>
            XPathNavigator navigatorCode = navigator.SelectSingleNode(Xml.CODE);
            if (navigatorCode != null) {
                this._code = navigatorCode.Value;
            }

            // <Name></Name>
            XPathNavigator navigatorName = navigator.SelectSingleNode(Xml.NAME);
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            //
            this.UpdateText();
        }
        public DomainCodedValueRow(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._code = info.GetString("code");
            this._name = info.GetString("name");
            this.UpdateText();
        }
        public DomainCodedValueRow(DomainCodedValueRow prototype) : base(prototype) {
            this._code = prototype.Code;
            this._name = prototype.Name;
            this.UpdateText();
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Domain Item Code (unique)
        /// </summary>
        [Browsable(true)]
        [Category("Domain Code Value")]
        [DefaultValue(null)]
        [Description("Coded value code. This is the value that used to updated the database.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Code {
            get { return this._code; }
            set { this._code = value; this.UpdateText(); }
        }
        /// <summary>
        /// Domain Item Description (non-unique)
        /// </summary>
        [Browsable(true)]
        [Category("Domain Code Value")]
        [DefaultValue(null)]
        [Description("Coded value name. Friendly name seen by user. The name should be unique.")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; this.UpdateText(); }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("code", this._code);
            info.AddValue("name", this._name);
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new DomainCodedValueRow (this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <CodedValue>
            writer.WriteStartElement("CodedValue");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:CodedValue");

            //
            this.WriteInnerXml(writer);

            // </CodedValue>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Table
            DomainCodedValue domain = (DomainCodedValue)this.Table;

            // Check if Name is null or empty
            if (string.IsNullOrEmpty(this._name)) {
                list.Add(new ErrorTableRow(this, "Name cannot be empty", ErrorType.Error));
            }

            // Check if Code can be converted to target type
            if (!string.IsNullOrEmpty(this._code)) {
                switch (domain.FieldType) {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        short value1;
                        bool valid1 = short.TryParse(this._code, out value1);
                        if (!valid1) {
                            string message = string.Format("Code '{0}' is not a valid short", this._code);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        int value2;
                        bool valid2 = int.TryParse(this._code, out value2);
                        if (!valid2) {
                            string message = string.Format("Code '{0}' is not a valid int", this._code);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        float value3;
                        bool valid3 = float.TryParse(this._code, out value3);
                        if (!valid3) {
                            string message = string.Format("Code '{0}' is not a valid float", this._code);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        double value4;
                        bool valid4 = double.TryParse(this._code, out value4);
                        if (!valid4) {
                            string message = string.Format("Code '{0}' is not a valid double", this._code);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        break;
                    case esriFieldType.esriFieldTypeString:
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                        DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.DateTimeFormat;
                        DateTime value5;
                        bool valid5 = DateTime.TryParseExact(this._code, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out value5);
                        if (!valid5) {
                            string message = string.Format("Code '{0}' is not a valid date", this._code);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        break;
                }
            }
        }
        public override void UpdateText() {
            string name = this._name == null ? "?" : this._name;
            string code = this._code == null ? "?" : this._code;
            string text = string.Format("{0} ({1})", name, code);
            this.Text = text;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // Get Table
            Domain domain = (Domain)this.Table;

            //
            string dataType = string.Empty;
            switch (domain.FieldType) {
                case esriFieldType.esriFieldTypeSmallInteger:
                    dataType = Xml._SHORT;
                    break;
                case esriFieldType.esriFieldTypeInteger:
                    dataType = Xml._INT;
                    break;
                case esriFieldType.esriFieldTypeSingle:
                    dataType = Xml._FLOAT;
                    break;
                case esriFieldType.esriFieldTypeDouble:
                    dataType = Xml._DOUBLE;
                    break;
                case esriFieldType.esriFieldTypeString:
                    dataType = Xml._STRING;
                    break;
                case esriFieldType.esriFieldTypeDate:
                    dataType = Xml._DATETIME;

                    break;
            }

            // <Name></Name>
            writer.WriteStartElement(Xml.NAME);
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <Code></Code>
            writer.WriteStartElement(Xml.CODE);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, dataType);
            if (domain.FieldType == esriFieldType.esriFieldTypeDate) {
                DateTime dateTime = Convert.ToDateTime(this._code);
                writer.WriteValue(dateTime.ToString("s"));
            }
            else {
                writer.WriteValue(this._code);
            }
            writer.WriteEndElement();
        }
    }
}

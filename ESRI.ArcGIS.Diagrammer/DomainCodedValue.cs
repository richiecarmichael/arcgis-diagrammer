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
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using Crainiate.Diagramming;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Coded Value Domain
    /// </summary>
    [Serializable]
    public class DomainCodedValue : Domain {
        //
        // CONSTRUCTOR
        //
        public DomainCodedValue(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();
            XPathNodeIterator interatorCodedValue = navigator.Select("CodedValues/CodedValue");

            // Add Coded Value Rows
            while (interatorCodedValue.MoveNext()) {
                XPathNavigator navigatorCodedValue = interatorCodedValue.Current;
                DomainCodedValueRow row = new DomainCodedValueRow(navigatorCodedValue);
                this.Rows.Add(row);
            }
        }
        public DomainCodedValue(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public DomainCodedValue(DomainCodedValue prototype) : base(prototype) { }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        [Category("Coded Value Domain")]
        [DefaultValue("")]
        [Description("Get Selected Index Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public DomainCodedValueRow SelectedCodedValue {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                if (!(tableItem is DomainCodedValueRow)) { return null; }

                DomainCodedValueRow codedValue = (DomainCodedValueRow)tableItem;
                return codedValue;
            }
        }
        [Browsable(true)]
        [Category("Coded Value Domain")]
        [DefaultValue(null)]
        [Description("Collection of Coded Values")]
        [Editor(typeof(DomainCodedValueRowCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems CodedValues {
            get { return this.Rows; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new DomainCodedValue(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Domain>
            writer.WriteStartElement("Domain");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._CODEDVALUEDOMAIN);

            //
            this.WriteInnerXml(writer);

            // </Domain>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Base Errors
            base.Errors(list);

            // Check Field Type
            switch (this.FieldType) {
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeString:
                case esriFieldType.esriFieldTypeDate:
                    break;
                default:
                    list.Add(new ErrorTable(this, string.Format("Field Type Cannot be {0}", this.FieldType.ToString()), ErrorType.Error));
                    break;
            }

            // Warn if domain does not contain any coded value items
            if (this.Rows.Count == 0) {
                list.Add(new ErrorTable(this, "Domain does not contain any coded values", ErrorType.Warning));
            }

            // Add Coded Value Domain Item Errors
            foreach (DomainCodedValueRow codedValue in this.Rows) {
                codedValue.Errors(list);
            }

            // Check for duplicate coded value codes
            Dictionary<string, DomainCodedValueRow> dictionary = new Dictionary<string, DomainCodedValueRow>();
            foreach (DomainCodedValueRow codedValue in this.Rows) {
                if (string.IsNullOrEmpty(codedValue.Code)) { continue; }
                DomainCodedValueRow c = null;
                if (dictionary.TryGetValue(codedValue.Code, out c)) {
                    string message1 = string.Format("Domain coded value code [{0}] is duplicated", codedValue.Code);
                    string message2 = string.Format("Domain coded value code [{0}] is duplicated", c.Code);
                    list.Add(new ErrorTableRow(codedValue, message1, ErrorType.Error));
                    list.Add(new ErrorTableRow(c, message2, ErrorType.Error));
                }
                else {
                    dictionary.Add(codedValue.Code, codedValue);
                }
            }

            // Check for duplicate code value descriptions (warning only)
            Dictionary<string, DomainCodedValueRow> dictionary2 = new Dictionary<string, DomainCodedValueRow>();
            foreach (DomainCodedValueRow codedValue in this.Rows) {
                if (string.IsNullOrEmpty(codedValue.Name)) { continue; }
                DomainCodedValueRow c = null;
                if (dictionary2.TryGetValue(codedValue.Name, out c)) {
                    string message1 = string.Format("Domain coded value name [{0}] is duplicated", codedValue.Name);
                    string message2 = string.Format("Domain coded value name [{0}] is duplicated", c.Name);
                    list.Add(new ErrorTableRow(codedValue, message1, ErrorType.Warning));
                    list.Add(new ErrorTableRow(c, message2, ErrorType.Warning));
                }
                else {
                    dictionary2.Add(codedValue.Name, codedValue);
                }
            }
        }
        public void AddCodedValue(DomainCodedValueRow codedValue) {
            this.Rows.Add(codedValue);
            this.SelectedItem = codedValue;
        }
        public void RemoveCodedValue(DomainCodedValueRow codedValue) {
            // Get Index
            int index = this.Rows.IndexOf(codedValue);
            if (index == -1) { return; }

            // Remove
            this.Rows.RemoveAt(index);

            // Select Next Coded Value
            if (this.Rows.Count == 0) {return;}
            if (index != this.Rows.Count) {
                this.SelectedItem = this.Rows[index];
            }
            else {
                this.SelectedItem = this.Rows[this.Rows.Count - 1];
            }
        }
        public override bool IsValid(string test, out string message) {
            message = null;
            foreach (DomainCodedValueRow codedvalueCode in this.Rows) {
                switch (this.FieldType) {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        short valueCode1;
                        short valueTest1;
                        bool validCode1 = short.TryParse(codedvalueCode.Code, out valueCode1);
                        bool validTest1 = short.TryParse(test, out valueTest1);
                        if (validCode1 && validTest1) {
                            if (valueCode1 == valueTest1) {
                                return true;
                            }
                        }
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        int valueCode2;
                        int valueTest2;
                        bool validCode2 = int.TryParse(codedvalueCode.Code, out valueCode2);
                        bool validTest2 = int.TryParse(test, out valueTest2);
                        if (validCode2 && validTest2) {
                            if (valueCode2 == valueTest2) {
                                return true;
                            }
                        }
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        float valueCode3;
                        float valueTest3;
                        bool validCode3 = float.TryParse(codedvalueCode.Code, out valueCode3);
                        bool validTest3 = float.TryParse(test, out valueTest3);
                        if (validCode3 && validTest3) {
                            if (valueCode3 == valueTest3) {
                                return true;
                            }
                        }
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        double valueCode4;
                        double valueTest4;
                        bool validCode4 = double.TryParse(codedvalueCode.Code, out valueCode4);
                        bool validTest4 = double.TryParse(test, out valueTest4);
                        if (validCode4 && validTest4) {
                            if (valueCode4 == valueTest4) {
                                return true;
                            }
                        }
                        break;
                    case esriFieldType.esriFieldTypeString:
                        if (codedvalueCode.Code == test) {
                            return true;
                        }
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                        DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.DateTimeFormat;
                        DateTime valueCode5;
                        DateTime valueTest5;
                        bool validCode5 = DateTime.TryParseExact(codedvalueCode.Code, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueCode5);
                        bool validTest5 = DateTime.TryParseExact(test, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueTest5);
                        if (validCode5 && validTest5) {
                            if (valueCode5 == valueTest5) {
                                return true;
                            }
                        }
                        break;
                }
            }

            message = string.Format("Value [{0}] not found in coded value domain [{1}]", test, this.Name);
            return false;
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.CodedValueDomainColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <CodedValues>
            writer.WriteStartElement("CodedValues");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfCodedValue");
            foreach (EsriTableRow row in this.Rows) {
                row.WriteXml(writer);
            }

            // </CodedValues>
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.GradientColor = ColorSettings.Default.CodedValueDomainColor;
            this.SubHeading = Resources.TEXT_CODED_VALUE_DOMAIN;
        }
    }
}
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
    public class DomainRangeRow : EsriTableRow {
        private string _max = string.Empty;
        private string _min = string.Empty;
        //
        // CONSTRUCTOR
        //
        public DomainRangeRow() : base() {
            this.UpdateText();
        }
        public DomainRangeRow(IXPathNavigable path): base(path) {
            XPathNavigator navigator = path.CreateNavigator();

            XPathNavigator navigatorMinValue = navigator.SelectSingleNode("MinValue");
            if (navigatorMinValue != null) {
                this._min = navigatorMinValue.Value;
            }

            XPathNavigator navigatorMaxValue = navigator.SelectSingleNode("MaxValue");
            if (navigatorMaxValue != null) {
                this._max = navigatorMaxValue.Value;
            }

            this.UpdateText();
        }
        public DomainRangeRow(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._max = info.GetString("max");
            this._min = info.GetString("min");
            this.UpdateText();
        }
        public DomainRangeRow(DomainRangeRow prototype) : base(prototype) {
            this._max = prototype.Max;
            this._min = prototype.Min;
            this.UpdateText();
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Domain Range Row")]
        [DefaultValue(null)]
        [Description("Maximum Range Value")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Max {
            get { return this._max; }
            set { this._max = value; this.UpdateText(); }
        }
        [Browsable(true)]
        [Category("Domain Range Row")]
        [DefaultValue(null)]
        [Description("Minimum Range Value")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Min {
            get { return this._min; }
            set { this._min = value; this.UpdateText(); }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("max", this._max);
            info.AddValue("min", this._min);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new DomainRangeRow(this);
        }
        public override void WriteXml(XmlWriter writer) {
            this.WriteInnerXml(writer);
        }
        public override void Errors(List<Error> list) {
            // Get Table
            DomainRange domain = (DomainRange)this.Table;

            // Validate Min and Max values
            switch (domain.FieldType) {
                case esriFieldType.esriFieldTypeSmallInteger: {
                        short valueMin;
                        short valueMax;
                        bool validMin = short.TryParse(this._min, out valueMin);
                        bool validMax = short.TryParse(this._max, out valueMax);

                        if (!validMin) {
                            string message = string.Format("Min '{0}' is not a valid short", this._min);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (!validMax) {
                            string message = string.Format("Max '{0}' is not a valid short", this._max);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (validMin && validMax) {
                            if (valueMin > valueMax) {
                                list.Add(new ErrorTableRow(this, "Min is greater than Max", ErrorType.Error));
                            }
                            else if (valueMin == valueMax) {
                                list.Add(new ErrorTableRow(this, "Min and Max are equal", ErrorType.Warning));
                            }
                        }
                        break;
                    }
                case esriFieldType.esriFieldTypeInteger: {
                        int valueMin;
                        int valueMax;
                        bool validMin = int.TryParse(this._min, out valueMin);
                        bool validMax = int.TryParse(this._max, out valueMax);

                        if (!validMin) {
                            string message = string.Format("Min '{0}' is not a valid int", this._min);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (!validMax) {
                            string message = string.Format("Max '{0}' is not a valid int", this._max);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (validMin && validMax) {
                            if (valueMin > valueMax) {
                                list.Add(new ErrorTableRow(this, "Min is greater than Max", ErrorType.Error));
                            }
                            else if (valueMin == valueMax) {
                                list.Add(new ErrorTableRow(this, "Min and Max are equal", ErrorType.Warning));
                            }
                        }
                        break;
                    }
                case esriFieldType.esriFieldTypeSingle: {
                        float valueMin;
                        float valueMax;
                        bool validMin = float.TryParse(this._min, out valueMin);
                        bool validMax = float.TryParse(this._max, out valueMax);

                        if (!validMin) {
                            string message = string.Format("Min '{0}' is not a valid float", this._min);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (!validMax) {
                            string message = string.Format("Max '{0}' is not a valid float", this._max);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (validMin && validMax) {
                            if (valueMin > valueMax) {
                                list.Add(new ErrorTableRow(this, "Min is greater than Max", ErrorType.Error));
                            }
                            else if (valueMin == valueMax) {
                                list.Add(new ErrorTableRow(this, "Min and Max are equal", ErrorType.Warning));
                            }
                        }
                        break;
                    }
                case esriFieldType.esriFieldTypeDouble: {
                        double valueMin;
                        double valueMax;
                        bool validMin = double.TryParse(this._min, out valueMin);
                        bool validMax = double.TryParse(this._max, out valueMax);

                        if (!validMin) {
                            string message = string.Format("Min '{0}' is not a valid double", this._min);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (!validMax) {
                            string message = string.Format("Max '{0}' is not a valid double", this._max);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (validMin && validMax) {
                            if (valueMin > valueMax) {
                                list.Add(new ErrorTableRow(this, "Min is greater than Max", ErrorType.Error));
                            }
                            else if (valueMin == valueMax) {
                                list.Add(new ErrorTableRow(this, "Min and Max are equal", ErrorType.Warning));
                            }
                        }
                        break;
                    }
                case esriFieldType.esriFieldTypeDate: {
                        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                        DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.DateTimeFormat;
                        DateTime valueMin;
                        DateTime valueMax;
                        bool validMin = DateTime.TryParseExact(this._min, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueMin);
                        bool validMax = DateTime.TryParseExact(this._max, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueMax);

                        if (!validMin) {
                            string message = string.Format("Min '{0}' is not a valid DateTime", this._min);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (!validMax) {
                            string message = string.Format("Max '{0}' is not a valid DateTime", this._max);
                            list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        }
                        if (validMin && validMax) {
                            if (valueMin > valueMax) {
                                list.Add(new ErrorTableRow(this, "Min is greater than Max", ErrorType.Error));
                            }
                            else if (valueMin == valueMax) {
                                list.Add(new ErrorTableRow(this, "Min and Max are equal", ErrorType.Warning));
                            }
                        }
                        break;
                    }
            }
        }
        public override void UpdateText() {
            // Text
            string min1 = this._min == null ? "?" : this._min.ToString();
            string max1 = this._max == null ? "?" : this._max.ToString();
            this.Text = string.Format("{0} - {1}", min1, max1);
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
                case esriFieldType.esriFieldTypeDate:
                    dataType = "xs:dateTime";
                    break;
            }

            // <MaxValue></MaxValue>
            writer.WriteStartElement("MaxValue");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, dataType);
            writer.WriteValue(this._max);
            writer.WriteEndElement();

            // <MinValue></MinValue>
            writer.WriteStartElement("MinValue");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, dataType);
            writer.WriteValue(this._min);
            writer.WriteEndElement();
        }
    }
}

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
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class DomainRange : Domain {
        //
        // CONSTRUCTOR
        //
        public DomainRange(IXPathNavigable path): base(path) {
            //
            XPathNavigator navigator = path.CreateNavigator();

            // Add Min/Max Row
            DomainRangeRow row = new DomainRangeRow(navigator);
            this.Rows.Add(row);
        }
        public DomainRange(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public DomainRange(DomainRange prototype) : base(prototype) { }
        //
        // PROPERTIES
        //
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new DomainRange(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Domain>
            writer.WriteStartElement("Domain");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._RANGEDOMAIN);

            //
            this.WriteInnerXml(writer);

            // </Domain>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            //
            base.Errors(list);

            // Check Field Type
            switch (this.FieldType) {
                case esriFieldType.esriFieldTypeSmallInteger:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeSingle:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeDate:
                    break;
                default:
                    list.Add(new ErrorTable(this, string.Format("Field Type Cannot be {0}", this.FieldType.ToString()), ErrorType.Error));
                    break;
            }

            // Add Range Domain Item Errors
            foreach (DomainRangeRow row in base.Rows) {
                row.Errors(list);
            }
        }
        public override bool IsValid(string test, out string message) {
            // Get Domain Range Row
            DomainRangeRow row = (DomainRangeRow)this.Rows[0];

            // 
            message = null;
            switch (this.FieldType) {
                case esriFieldType.esriFieldTypeSmallInteger: {
                        short valueMin;
                        short valueMax;
                        short valueTest;
                        bool validMin = short.TryParse(row.Min, out valueMin);
                        bool validMax = short.TryParse(row.Max, out valueMax);
                        bool validTest = short.TryParse(test, out valueTest);

                        if (validMin && validMax) {
                            if ((valueTest < valueMin) || (valueTest > valueMax)) {
                                message = "Value is outside the range domain";
                                return false;
                            }
                        }
                        return true;
                    }
                case esriFieldType.esriFieldTypeInteger: {
                        int valueMin;
                        int valueMax;
                        int valueTest;
                        bool validMin = int.TryParse(row.Min, out valueMin);
                        bool validMax = int.TryParse(row.Max, out valueMax);
                        bool validTest = int.TryParse(test, out valueTest);

                        if (validMin && validMax) {
                            if ((valueTest < valueMin) || (valueTest > valueMax)) {
                                message = "Value is outside the range domain";
                                return false;
                            }
                        }
                        return true;
                    }
                case esriFieldType.esriFieldTypeSingle: {
                        float valueMin;
                        float valueMax;
                        float valueTest;
                        bool validMin = float.TryParse(row.Min, out valueMin);
                        bool validMax = float.TryParse(row.Max, out valueMax);
                        bool validTest = float.TryParse(test, out valueTest);

                        if (validMin && validMax) {
                            if ((valueTest < valueMin) || (valueTest > valueMax)) {
                                message = "Value is outside the range domain";
                                return false;
                            }
                        }
                        return true;
                    }
                case esriFieldType.esriFieldTypeDouble: {
                        double valueMin;
                        double valueMax;
                        double valueTest;
                        bool validMin = double.TryParse(row.Min, out valueMin);
                        bool validMax = double.TryParse(row.Max, out valueMax);
                        bool validTest = double.TryParse(test, out valueTest);

                        if (validMin && validMax) {
                            if ((valueTest < valueMin) || (valueTest > valueMax)) {
                                message = "Value is outside the range domain";
                                return false;
                            }
                        }
                        return true;
                    }
                case esriFieldType.esriFieldTypeDate: {
                        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                        DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.DateTimeFormat;
                        DateTime valueMin;
                        DateTime valueMax;
                        DateTime valueTest;
                        bool validMin = DateTime.TryParseExact(row.Min, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueMin);
                        bool validMax = DateTime.TryParseExact(row.Max, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueMax);
                        bool validTest = DateTime.TryParseExact(test, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueTest);

                        if (validMin && validMax) {
                            if ((valueTest < valueMin) || (valueTest > valueMax)) {
                                message = "Value is outside the range domain";
                                return false;
                            }
                        }
                        return true;
                    }
                default:
                    return false;
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.RangeDomainColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void Initialize() {
            this.FieldType = esriFieldType.esriFieldTypeDouble;
            this.GradientColor = ColorSettings.Default.RangeDomainColor;
            this.SubHeading = Resources.TEXT_RANGE_DOMAIN; ;
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            //
            EsriTableRow row = (EsriTableRow)this.Rows[0];
            row.WriteXml(writer);
        }
     }
}

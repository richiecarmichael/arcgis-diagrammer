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
    /// ESRI Junction Connectivity Rule
    /// </summary>
    /// <remarks>
    /// Junction Connectivity Rule for Geometric Networks
    /// </remarks>
    [Serializable]
    public class EdgeConnectivityRule : ConnectivityRule {
        private int _fromClassID = -1;
        private int _fromEdgeSubtypeCode = -1;
        private int _toClassID = -1;
        private int _toEdgeSubtypeCode = -1;
        private int _defaultJunctionID = -1;
        private int _defaultJunctionSubtypeCode = -1;
        private List<JunctionSubtype> _junctionSubtypes = null;
        //
        // CONSTRUCTOR
        //
        public EdgeConnectivityRule() : base() {
            this._junctionSubtypes = new List<JunctionSubtype>();
        }
        public EdgeConnectivityRule(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <FromClassID></FromClassID>
            XPathNavigator navigatorFromClassID = navigator.SelectSingleNode("FromClassID");
            if (navigatorFromClassID != null) {
                this._fromClassID = navigatorFromClassID.ValueAsInt;
            }

            // <FromEdgeSubtypeCode></FromEdgeSubtypeCode> 
            XPathNavigator navigatorFromEdgeSubtypeCode = navigator.SelectSingleNode("FromEdgeSubtypeCode");
            if (navigatorFromEdgeSubtypeCode != null) {
                this._fromEdgeSubtypeCode = navigatorFromEdgeSubtypeCode.ValueAsInt;
            }

            // <ToClassID></ToClassID> 
            XPathNavigator navigatorToClassID = navigator.SelectSingleNode("ToClassID");
            if (navigatorToClassID != null) {
                this._toClassID = navigatorToClassID.ValueAsInt;
            }

            // <ToEdgeSubtypeCode></ToEdgeSubtypeCode> 
            XPathNavigator navigatorToEdgeSubtypeCode = navigator.SelectSingleNode("ToEdgeSubtypeCode");
            if (navigatorToEdgeSubtypeCode != null) {
                this._toEdgeSubtypeCode = navigatorToEdgeSubtypeCode.ValueAsInt;
            }

            // <DefaultJunctionID></DefaultJunctionID> 
            XPathNavigator navigatorDefaultJunctionID = navigator.SelectSingleNode("DefaultJunctionID");
            if (navigatorDefaultJunctionID != null) {
                this._defaultJunctionID = navigatorDefaultJunctionID.ValueAsInt;
            }

            // <DefaultJunctionSubtypeCode></DefaultJunctionSubtypeCode> 
            XPathNavigator navigatorDefaultJunctionSubtypeCode = navigator.SelectSingleNode("DefaultJunctionSubtypeCode");
            if (navigatorDefaultJunctionSubtypeCode != null) {
                this._defaultJunctionSubtypeCode = navigatorDefaultJunctionSubtypeCode.ValueAsInt;
            }

            // <JunctionSubtypes xsi:type="esri:ArrayOfJunctionSubtype">
            //    <JunctionSubtype xsi:type="esri:JunctionSubtype">
            //       <ClassID></ClassID> 
            //       <SubtypeCode></SubtypeCode> 
            //    </JunctionSubtype>
            // </JunctionSubtypes>

            //
            this._junctionSubtypes = new List<JunctionSubtype>();

            XPathNodeIterator interatorJunctionSubtype = navigator.Select("JunctionSubtypes/JunctionSubtype");
            while (interatorJunctionSubtype.MoveNext()) {
                // Get <JunctionSubtype>
                XPathNavigator navigatorJunctionSubtype = interatorJunctionSubtype.Current;

                // Create JunctionSubtype
                JunctionSubtype junctionSubtype = new JunctionSubtype(navigatorJunctionSubtype);

                // Add Rule to Collection
                this._junctionSubtypes.Add(junctionSubtype);
            }
        }
        public EdgeConnectivityRule(SerializationInfo info, StreamingContext context) : base(info, context) {
            // <FromClassID></FromClassID> 
            // <FromEdgeSubtypeCode></FromEdgeSubtypeCode> 
            // <ToClassID></ToClassID> 
            // <ToEdgeSubtypeCode></ToEdgeSubtypeCode> 
            // <DefaultJunctionID></DefaultJunctionID> 
            // <DefaultJunctionSubtypeCode></DefaultJunctionSubtypeCode>
            // <JunctionSubtypes></JunctionSubtypes>
            this._fromClassID = info.GetInt32("fromClassID");
            this._fromEdgeSubtypeCode = info.GetInt32("fromEdgeSubtypeCode");
            this._toClassID = info.GetInt32("toClassID");
            this._toEdgeSubtypeCode = info.GetInt32("toEdgeSubtypeCode");
            this._defaultJunctionID = info.GetInt32("defaultJunctionID");
            this._defaultJunctionSubtypeCode = info.GetInt32("defaultJunctionSubtypeCode");
            this._junctionSubtypes = (List<JunctionSubtype>)info.GetValue("junctionSubtypes", typeof(List<JunctionSubtype>));
        }
        public EdgeConnectivityRule(EdgeConnectivityRule prototype) : base(prototype) {
            this._fromClassID = prototype.FromClassID;
            this._fromEdgeSubtypeCode = prototype.FromEdgeSubtypeCode;
            this._toClassID = prototype.ToClassID;
            this._toEdgeSubtypeCode = prototype.ToEdgeSubtypeCode;
            this._defaultJunctionID = prototype.DefaultJunctionID;
            this._defaultJunctionSubtypeCode = prototype.DefaultJunctionSubtypeCode;
 
            // Clone Junction Subtypes
            this._junctionSubtypes = new List<JunctionSubtype>();
            foreach (JunctionSubtype junctionSubtype in prototype.JunctionSubtypes) {
                this._junctionSubtypes.Add((JunctionSubtype)junctionSubtype.Clone());
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The ID of the source NetworkEdge feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The ID of the source NetworkEdge feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int FromClassID {
            get { return this._fromClassID; }
            set { this._fromClassID = value; }
        }
        /// <summary>
        /// The subtype value of the source NetworkEdge feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The subtype value of the source NetworkEdge feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int FromEdgeSubtypeCode {
            get { return this._fromEdgeSubtypeCode; }
            set { this._fromEdgeSubtypeCode = value; }
        }
        /// <summary>
        /// The ID of the destination NetworkEdge feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The ID of the destination NetworkEdge feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int ToClassID {
            get { return this._toClassID; }
            set { this._toClassID = value; }
        }
        /// <summary>
        /// The subtype value of the target NetworkEdge feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The subtype value of the target NetworkEdge feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int ToEdgeSubtypeCode {
            get { return this._toEdgeSubtypeCode; }
            set { this._toEdgeSubtypeCode = value; }
        }
        /// <summary>
        /// The ID of the default junction feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The ID of the default junction feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int DefaultJunctionID {
            get { return this._defaultJunctionID; }
            set { this._defaultJunctionID = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The subtype value of the default junction feature class
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("The subtype value of the default junction feature class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int DefaultJunctionSubtypeCode {
            get { return this._defaultJunctionSubtypeCode; }
            set { this._defaultJunctionSubtypeCode = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Collection of Junction Subtypes
        /// </summary>
        [Browsable(true)]
        [Category("Edge Connectivity")]
        [DefaultValue(-1)]
        [Description("Collection of Junction Subtypes")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<JunctionSubtype> JunctionSubtypes {
            get { return this._junctionSubtypes; }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = string.Empty;

                if (this._junctionSubtypes.Count == 0) {
                    text += "(Not Set)";
                }
                else {
                    DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;

                    foreach (JunctionSubtype junctionSubtype in this._junctionSubtypes) {
                        ObjectClass objectClass = diagrammerEnvironment.SchemaModel.FindObjectClass(junctionSubtype.ClassID);
                        if (objectClass == null) { continue; }
                        Subtype subtype = objectClass.FindSubtype(junctionSubtype.SubtypeCode);
                        if (subtype == null && junctionSubtype.SubtypeCode != 0) { continue; }

                        // Add Newline if Text already exists
                        if (!string.IsNullOrEmpty(text)) {
                            text += Environment.NewLine;
                        }

                        // Add ObjectClass Name
                        text += objectClass.Name;

                        // Add Subtype Name
                        if (subtype != null) {
                            text += "::" + subtype.SubtypeName;
                        }

                        // Add "Default" text
                        if (junctionSubtype.ClassID == this._defaultJunctionID &&
                            junctionSubtype.SubtypeCode == this._defaultJunctionSubtypeCode) {
                            text += " (Default)";
                        }
                    }
                }

                return text;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("fromClassID", this._fromClassID);
            info.AddValue("fromEdgeSubtypeCode", this._fromEdgeSubtypeCode);
            info.AddValue("toClassID", this._toClassID);
            info.AddValue("toEdgeSubtypeCode", this._toEdgeSubtypeCode);
            info.AddValue("defaultJunctionID", this._defaultJunctionID);
            info.AddValue("defaultJunctionSubtypeCode", this._defaultJunctionSubtypeCode);
            info.AddValue("junctionSubtypes", this._junctionSubtypes, typeof(List<JunctionSubtype>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new EdgeConnectivityRule(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Write Base Errors
            base.Errors(list, table);

            // From Class ID and SubtypeCode
            EsriTable table2 = DiagrammerEnvironment.Default.SchemaModel.FindObjectClassOrSubtype(this._fromClassID, this._fromEdgeSubtypeCode);
            if (table2 == null) {
                string message = string.Format("Edge Connectivity Rule [{0}]: Can not identify the from edge [{1}::{0}]", this.RuleId.ToString(), this._fromClassID.ToString(), this._fromEdgeSubtypeCode.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }

            // To Class ID and SubtypeCode
            EsriTable table3 = DiagrammerEnvironment.Default.SchemaModel.FindObjectClassOrSubtype(this._toClassID, this._toEdgeSubtypeCode);
            if (table3 == null) {
                string message = string.Format("Edge Connectivity Rule [{0}]: Can not identify the to edge [{1}::{0}]", this.RuleId.ToString(), this._toClassID.ToString(), this._toEdgeSubtypeCode.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }

            // Default Class ID and SubtypeCode
            EsriTable table4 = DiagrammerEnvironment.Default.SchemaModel.FindObjectClassOrSubtype(this._defaultJunctionID, this._defaultJunctionSubtypeCode);
            if (table4 == null) {
                string message = string.Format("Edge Connectivity Rule [{0}]: Can not identify the default junction [{1}::{0}]", this.RuleId.ToString(), this._defaultJunctionID.ToString(), this._defaultJunctionSubtypeCode.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }
            else {
                bool found = false;
                foreach (JunctionSubtype junctionSubtype in this._junctionSubtypes) {
                    if (junctionSubtype.ClassID == this._defaultJunctionID &&
                        junctionSubtype.SubtypeCode == this._defaultJunctionSubtypeCode) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    string message = string.Format("Edge Connectivity Rule [{0}]: The default junction [{1}::{0}] in not contained in the junction list", this.RuleId.ToString(), this._defaultJunctionID.ToString(), this._defaultJunctionSubtypeCode.ToString());
                    list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                }
            }

            // Junction Subtype
            if (this._junctionSubtypes.Count == 0) {
                string message = string.Format("Edge Connectivity Rule [{0}]: The rule must at least one junction defined", this.RuleId.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }
            foreach (JunctionSubtype junctionSubtype in this._junctionSubtypes) {
                junctionSubtype.Errors(list, table);
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // <ConnectivityRule>
            writer.WriteStartElement("ConnectivityRule");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:EdgeConnectivityRule");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </ConnectivityRule>
            writer.WriteEndElement();
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <FromClassID></FromClassID> 
            writer.WriteStartElement("FromClassID");
            writer.WriteValue(this._fromClassID);
            writer.WriteEndElement();

            // <FromEdgeSubtypeCode></FromEdgeSubtypeCode>
            writer.WriteStartElement("FromEdgeSubtypeCode");
            writer.WriteValue(this._fromEdgeSubtypeCode);
            writer.WriteEndElement();

            // <ToClassID></ToClassID> 
            writer.WriteStartElement("ToClassID");
            writer.WriteValue(this._toClassID);
            writer.WriteEndElement();

            // <ToEdgeSubtypeCode></ToEdgeSubtypeCode> 
            writer.WriteStartElement("ToEdgeSubtypeCode");
            writer.WriteValue(this._toEdgeSubtypeCode);
            writer.WriteEndElement();

            // <DefaultJunctionID></DefaultJunctionID> 
            writer.WriteStartElement("DefaultJunctionID");
            writer.WriteValue(this._defaultJunctionID);
            writer.WriteEndElement();

            // <DefaultJunctionSubtypeCode></DefaultJunctionSubtypeCode>
            writer.WriteStartElement("DefaultJunctionSubtypeCode");
            writer.WriteValue(this._defaultJunctionSubtypeCode);
            writer.WriteEndElement();

            // <JunctionSubtypes>
            writer.WriteStartElement("JunctionSubtypes");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfJunctionSubtype");

            // Write Junction Subtype (if any)
            foreach (JunctionSubtype junctionSubtype in this._junctionSubtypes) {
                // <JunctionSubtype></JunctionSubtype>
                junctionSubtype.WriteXml(writer);
            }

            // </JunctionSubtypes>
            writer.WriteEndElement();
        }
    }
}

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
    /// ESRI Relationship Rule
    /// </summary>
    /// <remarks>
    /// Relationship rules define the cardinality between origin and destination objectclass and their substypes
    /// </remarks>
    [Serializable]
    public class RelationshipRule : Rule {
        private const string CATEGORY = "Relationship";
        private int _destinationClass = -1;
        private int _destinationSubtype = -1;
        private int _originClass = -1;
        private int _originSubtype = -1;
        private int _destinationMinimumCardinality = -1;
        private int _destinationMaximumCardinality = -1;
        private int _originMinimumCardinality = -1;
        private int _originMaximumCardinality = -1;
        //
        // CONSTRUCTOR
        //
        public RelationshipRule() : base() { }
        public RelationshipRule(IXPathNavigable path): base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <DestinationClassID></DestinationClassID>
            // <DestinationSubtypeCode></DestinationSubtypeCode>
            XPathNavigator navigatorDestinationClass = navigator.SelectSingleNode("DestinationClassID");
            XPathNavigator navigatorDestinationSubtype = navigator.SelectSingleNode("DestinationSubtypeCode");
            if (navigatorDestinationClass != null && navigatorDestinationSubtype != null) {
                this._destinationClass = navigatorDestinationClass.ValueAsInt;
                this._destinationSubtype = navigatorDestinationSubtype.ValueAsInt;
            }

            // <OriginClassID></OriginClassID>
            // <OriginSubtypeCode></OriginSubtypeCode>
            XPathNavigator navigatorOriginClass = navigator.SelectSingleNode("OriginClassID");
            XPathNavigator navigatorOriginSubtype = navigator.SelectSingleNode("OriginSubtypeCode");
            if (navigatorOriginClass != null && navigatorOriginSubtype != null) {
                this._originClass = navigatorOriginClass.ValueAsInt;
                this._originSubtype = navigatorOriginSubtype.ValueAsInt;
            }

            // <DestinationMinimumCardinality></DestinationMinimumCardinality> 
            XPathNavigator navigatorDestinationMinimumCardinality = navigator.SelectSingleNode("DestinationMinimumCardinality");
            if (navigatorDestinationMinimumCardinality != null) {
                this._destinationMinimumCardinality = navigatorDestinationMinimumCardinality.ValueAsInt;
            }

            // <DestinationMaximumCardinality></DestinationMaximumCardinality> 
            XPathNavigator navigatorDestinationMaximumCardinality = navigator.SelectSingleNode("DestinationMaximumCardinality");
            if (navigatorDestinationMaximumCardinality != null) {
                this._destinationMaximumCardinality = navigatorDestinationMaximumCardinality.ValueAsInt;
            }

            // <OriginMinimumCardinality></OriginMinimumCardinality> 
            XPathNavigator navigatorOriginMinimumCardinality = navigator.SelectSingleNode("OriginMinimumCardinality");
            if (navigatorOriginMinimumCardinality != null) {
                this._originMinimumCardinality = navigatorOriginMinimumCardinality.ValueAsInt;
            }

            // <OriginMaximumCardinality></OriginMaximumCardinality>
            XPathNavigator navigatorOriginMaximumCardinality = navigator.SelectSingleNode("OriginMaximumCardinality");
            if (navigatorOriginMaximumCardinality != null) {
                this._originMaximumCardinality = navigatorOriginMaximumCardinality.ValueAsInt;
            }
        }
        public RelationshipRule(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._destinationClass = info.GetInt32("destinationClass");
            this._destinationSubtype = info.GetInt32("destinationSubtype");
            this._originClass = info.GetInt32("originClass");
            this._originSubtype = info.GetInt32("originSubtype");
            this._destinationMinimumCardinality = info.GetInt32("destinationMinimumCardinality");
            this._destinationMaximumCardinality = info.GetInt32("destinationMaximumCardinality");
            this._originMinimumCardinality = info.GetInt32("originMinimumCardinality");
            this._originMaximumCardinality = info.GetInt32("originMaximumCardinality");
        }
        public RelationshipRule(RelationshipRule prototype): base(prototype) {
            this._destinationClass = prototype.DestinationClass;
            this._destinationSubtype = prototype.DestinationSubtype;
            this._originClass = prototype.OriginClass;
            this._originSubtype = prototype.OriginSubtype;
            this._destinationMinimumCardinality = prototype.DestinationMinimumCardinality;
            this._destinationMaximumCardinality = prototype.DestinationMaximumCardinality;
            this._originMinimumCardinality = prototype.OriginMinimumCardinality;
            this._originMaximumCardinality = prototype.OriginMaximumCardinality;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The name of the destination ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The name of the destination ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int DestinationClass {
            get { return this._destinationClass; }
            set { this._destinationClass = value; } // if (!this.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The subtype of the destination ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The subtype of the destination ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int DestinationSubtype {
            get { return this._destinationSubtype; }
            set { this._destinationSubtype = value; } // if (!this.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The name of the origin ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The name of the origin ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int OriginClass {
            get { return this._originClass; }
            set { this._originClass = value; } // if (!this.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The subtype of the origin ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The subtype of the origin ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int OriginSubtype {
            get { return this._originSubtype; }
            set { this._originSubtype = value; } // if (!this.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The minimum cardinality value of the destination ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The minimum cardinality value of the destination ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int DestinationMinimumCardinality {
            get { return this._destinationMinimumCardinality; }
            set { this._destinationMinimumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The maximum cardinality value of the destination ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The maximum cardinality value of the destination ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int DestinationMaximumCardinality {
            get { return this._destinationMaximumCardinality; }
            set { this._destinationMaximumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The minimum cardinality value of the origin ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The minimum cardinality value of the origin ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int OriginMinimumCardinality {
            get { return this._originMinimumCardinality; }
            set { this._originMinimumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The maximum cardinality value of the origin ObjectClass
        /// </summary>
        [Browsable(true)]
        [Category(RelationshipRule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The maximum cardinality value of the origin ObjectClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int OriginMaximumCardinality {
            get { return this._originMaximumCardinality; }
            set { this._originMaximumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = string.Empty;
                if (this._originMinimumCardinality == -1 && this._originMaximumCardinality == -1) {
                    text += string.Format("{0} (origin)", "default");
                }
                else {
                    text += string.Format("{0}-{1} (origin)",
                        this._originMinimumCardinality.ToString(),
                        this._originMaximumCardinality.ToString());
                }
                text += Environment.NewLine;
                if (this._destinationMinimumCardinality == -1 && this._destinationMaximumCardinality == -1) {
                    text += string.Format("{0} (dest)", "default");
                }
                else {
                    text += string.Format("{0}-{1} (dest)",
                        this._destinationMinimumCardinality.ToString(),
                        this._destinationMaximumCardinality.ToString());
                }
                return text;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("destinationClass", this._destinationClass);
            info.AddValue("destinationSubtype", this._destinationSubtype);
            info.AddValue("originClass", this._originClass);
            info.AddValue("originSubtype", this._originSubtype);
            info.AddValue("destinationMinimumCardinality", this._destinationMinimumCardinality);
            info.AddValue("destinationMaximumCardinality", this._destinationMaximumCardinality);
            info.AddValue("originMinimumCardinality", this._originMinimumCardinality);
            info.AddValue("originMaximumCardinality", this._originMaximumCardinality);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RelationshipRule(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Write Base Errors
            base.Errors(list, table);

            // TODO: If One-One then Origin Cardinality must be 0 or 1
            // TODO: If One-One then Origin Cardinality must be 0 or 1

            // TODO: If One-Many then Origin Cardinality must be 0 or 1
            // TODO: If One-Many then Origin Cardinality must be 0..M

            // TODO: If Many-Many then Origin Cardinality must be 0..M
            // TODO: If Many-Many then Origin Cardinality must be 0..M

            // TODO: Origin Cardinality. Min <= Max
            // TODO: Destination Cardinality. Min <= Max

            // TODO: Origin Cardinality. OK if Min and Max both -1. (Default Cardinatity)
            // TODO: Origin Destination. OK if Min and Max both -1. (Default Cardinatity)
        }
        public override void WriteXml(XmlWriter writer) {
            //  <RelationshipRule xsi:type="esri:RelationshipRule">
            //      <HelpString /> 
            //      <RuleID>1119</RuleID> 
            //      <DestinationClassID>41</DestinationClassID> 
            //      <DestinationSubtypeCode>2</DestinationSubtypeCode> 
            //      <OriginClassID>47</OriginClassID> 
            //      <OriginSubtypeCode>8</OriginSubtypeCode> 
            //      <DestinationMinimumCardinality>0</DestinationMinimumCardinality> 
            //      <DestinationMaximumCardinality>2</DestinationMaximumCardinality> 
            //      <OriginMinimumCardinality>0</OriginMinimumCardinality> 
            //      <OriginMaximumCardinality>1</OriginMaximumCardinality> 
            //  </RelationshipRule>

            // <RelationshipRule>
            writer.WriteStartElement("RelationshipRule");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:RelationshipRule");

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </RelationshipRule>
            writer.WriteEndElement();
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <DestinationClassID></DestinationClassID>
            writer.WriteStartElement("DestinationClassID");
            writer.WriteValue(this._destinationClass);
            writer.WriteEndElement();

            // <DestinationSubtypeCode></DestinationSubtypeCode>
            writer.WriteStartElement("DestinationSubtypeCode");
            writer.WriteValue(this._destinationSubtype);
            writer.WriteEndElement();

            // <OriginClassID></OriginClassID>
            writer.WriteStartElement("OriginClassID");
            writer.WriteValue(this._originClass);
            writer.WriteEndElement();

            // <OriginSubtypeCode></OriginSubtypeCode>
            writer.WriteStartElement("OriginSubtypeCode");
            writer.WriteValue(this._originSubtype);
            writer.WriteEndElement();

            // Destination Cardinality
            if (this._destinationMinimumCardinality != -1 && this._destinationMaximumCardinality != -1) {
                // <DestinationMinimumCardinality></DestinationMinimumCardinality>
                writer.WriteStartElement("DestinationMinimumCardinality");
                writer.WriteValue(this._destinationMinimumCardinality);
                writer.WriteEndElement();

                // <DestinationMaximumCardinality></DestinationMaximumCardinality>
                writer.WriteStartElement("DestinationMaximumCardinality");
                writer.WriteValue(this._destinationMaximumCardinality);
                writer.WriteEndElement();
            }

            // Origin Cardinality
            if (this._originMinimumCardinality != -1 && this._originMaximumCardinality != -1) {
                // <OriginMinimumCardinality></OriginMinimumCardinality>
                writer.WriteStartElement("OriginMinimumCardinality");
                writer.WriteValue(this._originMinimumCardinality);
                writer.WriteEndElement();

                // <OriginMaximumCardinality></OriginMaximumCardinality>
                writer.WriteStartElement("OriginMaximumCardinality");
                writer.WriteValue(this._originMaximumCardinality);
                writer.WriteEndElement();
            }
        }
    }
}

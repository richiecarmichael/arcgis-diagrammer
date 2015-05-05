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
    public class JunctionConnectivityRule : ConnectivityRule {
        private int _edgeClassId = -1;
        private int _edgeSubtypeCode = -1;
        private int _junctionClassId = -1;
        private int _subtypeCode = -1;
        private int _edgeMinimumCardinality = -1;
        private int _edgeMaximumCardinality = -1;
        private int _junctionMinimumCardinality = -1;
        private int _junctionMaximumCardinality = -1;
        private bool _isDefault = false;
        //
        // CONSTRUCTOR
        //
        public JunctionConnectivityRule() : base() { }
        public JunctionConnectivityRule(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <EdgeClassID></EdgeClassID>
            XPathNavigator navigatorEdgeClassID = navigator.SelectSingleNode("EdgeClassID");
            if (navigatorEdgeClassID != null) {
                this._edgeClassId = navigatorEdgeClassID.ValueAsInt;
            }

            // <EdgeSubtypeCode></EdgeSubtypeCode> 
            XPathNavigator navigatorEdgeSubtypeCode = navigator.SelectSingleNode("EdgeSubtypeCode");
            if (navigatorEdgeSubtypeCode != null) {
                this._edgeSubtypeCode = navigatorEdgeSubtypeCode.ValueAsInt;
            }

            // <JunctionClassID></JunctionClassID> 
            XPathNavigator navigatorJunctionClassID = navigator.SelectSingleNode("JunctionClassID");
            if (navigatorJunctionClassID != null) {
                this._junctionClassId = navigatorJunctionClassID.ValueAsInt;
            }

            // <SubtypeCode></SubtypeCode> 
            XPathNavigator navigatorSubtypeCode = navigator.SelectSingleNode("SubtypeCode");
            if (navigatorSubtypeCode != null) {
                this._subtypeCode = navigatorSubtypeCode.ValueAsInt;
            }

            // <EdgeMinimumCardinality></EdgeMinimumCardinality> 
            XPathNavigator navigatorEdgeMinimumCardinality = navigator.SelectSingleNode("EdgeMinimumCardinality");
            if (navigatorEdgeMinimumCardinality != null) {
                this._edgeMinimumCardinality = navigatorEdgeMinimumCardinality.ValueAsInt;
            }

            // <EdgeMaximumCardinality></EdgeMaximumCardinality> 
            XPathNavigator navigatorEdgeMaximumCardinality = navigator.SelectSingleNode("EdgeMaximumCardinality");
            if (navigatorEdgeMaximumCardinality != null) {
                this._edgeMaximumCardinality = navigatorEdgeMaximumCardinality.ValueAsInt;
            }

            // <JunctionMinimumCardinality></JunctionMinimumCardinality> 
            XPathNavigator navigatorJunctionMinimumCardinality = navigator.SelectSingleNode("JunctionMinimumCardinality");
            if (navigatorJunctionMinimumCardinality != null) {
                this._junctionMinimumCardinality = navigatorJunctionMinimumCardinality.ValueAsInt;
            }

            // <JunctionMaximumCardinality></JunctionMaximumCardinality> 
            XPathNavigator navigatorJunctionMaximumCardinality = navigator.SelectSingleNode("JunctionMaximumCardinality");
            if (navigatorJunctionMaximumCardinality != null) {
                this._junctionMaximumCardinality = navigatorJunctionMaximumCardinality.ValueAsInt;
            }

            // <IsDefault></IsDefault> 
            XPathNavigator navigatorIsDefault = navigator.SelectSingleNode("IsDefault");
            if (navigatorIsDefault != null) {
                this._isDefault = navigatorIsDefault.ValueAsBoolean;
            }
        }
        public JunctionConnectivityRule(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._edgeClassId = info.GetInt32("edgeClassId");
            this._edgeSubtypeCode = info.GetInt32("edgeSubtypeCode");
            this._junctionClassId = info.GetInt32("junctionClassId");
            this._subtypeCode = info.GetInt32("subtypeCode");
            this._edgeMinimumCardinality = info.GetInt32("edgeMinimumCardinality");
            this._edgeMaximumCardinality = info.GetInt32("edgeMaximumCardinality");
            this._junctionMinimumCardinality = info.GetInt32("junctionMinimumCardinality");
            this._junctionMaximumCardinality = info.GetInt32("junctionMaximumCardinality");
            this._isDefault = info.GetBoolean("isDefault");
        }
        public JunctionConnectivityRule(JunctionConnectivityRule prototype) : base(prototype) {
            this._edgeClassId = prototype.EdgeClassID;
            this._edgeSubtypeCode = prototype.EdgeSubtypeCode;
            this._junctionClassId = prototype.JunctionClassID;
            this._subtypeCode = prototype.SubtypeCode;
            this._edgeMinimumCardinality = prototype.EdgeMinimumCardinality;
            this._edgeMaximumCardinality = prototype.EdgeMaximumCardinality;
            this._junctionMinimumCardinality = prototype.JunctionMinimumCardinality;
            this._junctionMaximumCardinality = prototype.JunctionMaximumCardinality;
            this._isDefault = prototype.IsDefault;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The ID of the NetworkEdge FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The ID of the NetworkEdge FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int EdgeClassID {
            get { return this._edgeClassId; }
            set { this._edgeClassId = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The subtype value of the NetworkEdge FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The subtype value of the NetworkEdge FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int EdgeSubtypeCode {
            get { return this._edgeSubtypeCode; }
            set { this._edgeSubtypeCode = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The ID of the NetworkJunction FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The ID of the NetworkJunction FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int JunctionClassID {
            get { return this._junctionClassId; }
            set { this._junctionClassId = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The subtype value of the NetworkJunction FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The subtype value of the NetworkJunction FeatureClass")]
        [DisplayName("JunctionSubtypeCpde")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int SubtypeCode {
            get { return this._subtypeCode; }
            set { this._subtypeCode = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The minimum cardinality value of the NetworkEdge FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The minimum cardinality value of the NetworkEdge FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int EdgeMinimumCardinality {
            get { return this._edgeMinimumCardinality; }
            set { this._edgeMinimumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The maxnimum cardinality value of the NetworkEdge FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The maxnimum cardinality value of the NetworkEdge FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int EdgeMaximumCardinality {
            get { return this._edgeMaximumCardinality; }
            set { this._edgeMaximumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The minimum cardinality value of the NetworkJunction FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The minimum cardinality value of the NetworkJunction FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int JunctionMinimumCardinality {
            get { return this._junctionMinimumCardinality; }
            set { this._junctionMinimumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The maximum cardinality value of the NetworkJunction FeatureClass
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(-1)]
        [Description("The maximum cardinality value of the NetworkJunction FeatureClass")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int JunctionMaximumCardinality {
            get { return this._junctionMaximumCardinality; }
            set { this._junctionMaximumCardinality = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Indicates if the junction corresponds to the default junction
        /// </summary>
        [Browsable(true)]
        [Category("Junction Connectivity")]
        [DefaultValue(false)]
        [Description("Indicates if the junction corresponds to the default junction")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsDefault {
            get { return this._isDefault; }
            set { this._isDefault = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = string.Empty;

                // EJ
                text += string.Format("{0}: ", "EJ");
                if (this._edgeMinimumCardinality == -1 && this._edgeMaximumCardinality == -1) {
                    text += string.Format("{0}", "(Default)");
                }
                else {
                    text += string.Format("{0}-{1}", this._edgeMinimumCardinality.ToString(), this._edgeMaximumCardinality.ToString());
                }

                // JE
                text += Environment.NewLine;
                text += string.Format("{0}: ", "JE");
                if (this._junctionMinimumCardinality == -1 && this._junctionMaximumCardinality == -1) {
                    text += string.Format("{0}", "(Default)");
                }
                else {
                    text += string.Format("{0}-{1}", this._junctionMinimumCardinality.ToString(), this._junctionMaximumCardinality.ToString());
                }

                return text;
            }
        }
        [Browsable(false)]
        public override float BorderWidth {
            get {
                return this._isDefault ? 3f : 1f;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("edgeClassId", this._edgeClassId);
            info.AddValue("edgeSubtypeCode", this._edgeSubtypeCode);
            info.AddValue("junctionClassId", this._junctionClassId);
            info.AddValue("subtypeCode", this._subtypeCode);
            info.AddValue("edgeMinimumCardinality", this._edgeMinimumCardinality);
            info.AddValue("edgeMaximumCardinality", this._edgeMaximumCardinality);
            info.AddValue("junctionMinimumCardinality", this._junctionMinimumCardinality);
            info.AddValue("junctionMaximumCardinality", this._junctionMaximumCardinality);
            info.AddValue("isDefault", this._isDefault);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new JunctionConnectivityRule(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Write Base Errors
            base.Errors(list, table);
        }
        public override void WriteXml(XmlWriter writer) {
            // <ConnectivityRule>
            writer.WriteStartElement("ConnectivityRule");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:JunctionConnectivityRule");

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

            //<EdgeClassID></EdgeClassID> 
            writer.WriteStartElement("EdgeClassID");
            writer.WriteValue(this._edgeClassId);
            writer.WriteEndElement();

            //<EdgeSubtypeCode>1</EdgeSubtypeCode> 
            writer.WriteStartElement("EdgeSubtypeCode");
            writer.WriteValue(this._edgeSubtypeCode);
            writer.WriteEndElement();

            //<JunctionClassID>37</JunctionClassID> 
            writer.WriteStartElement("JunctionClassID");
            writer.WriteValue(this._junctionClassId);
            writer.WriteEndElement();

            //<SubtypeCode>1</SubtypeCode> 
            writer.WriteStartElement("SubtypeCode");
            writer.WriteValue(this._subtypeCode);
            writer.WriteEndElement();

            if (this._edgeMinimumCardinality != -1) {
                //<EdgeMinimumCardinality>1</EdgeMinimumCardinality> 
                writer.WriteStartElement("EdgeMinimumCardinality");
                writer.WriteValue(this._edgeMinimumCardinality);
                writer.WriteEndElement();
            }

            if (this._edgeMaximumCardinality != -1) {
                //<EdgeMaximumCardinality>2</EdgeMaximumCardinality> 
                writer.WriteStartElement("EdgeMaximumCardinality");
                writer.WriteValue(this._edgeMaximumCardinality);
                writer.WriteEndElement();
            }

            if (this._junctionMinimumCardinality != -1) {
                //<JunctionMinimumCardinality>1</JunctionMinimumCardinality> 
                writer.WriteStartElement("JunctionMinimumCardinality");
                writer.WriteValue(this._junctionMinimumCardinality);
                writer.WriteEndElement();
            }

            if (this._junctionMaximumCardinality != -1) {
                //<JunctionMaximumCardinality>3</JunctionMaximumCardinality> 
                writer.WriteStartElement("JunctionMaximumCardinality");
                writer.WriteValue(this._junctionMaximumCardinality);
                writer.WriteEndElement();
            }

            //<IsDefault>false</IsDefault> 
            writer.WriteStartElement("IsDefault");
            writer.WriteValue(this._isDefault);
            writer.WriteEndElement();
        }
    }
}

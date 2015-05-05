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
    [Serializable]
    public class GeometricNetworkControllerMembership : ControllerMembership {
        private string _geometricNetworkName = string.Empty;
        private string _enabledFieldName = string.Empty;
        private string _ancillaryRoleFieldName = string.Empty;
        private esriNetworkClassAncillaryRole _networkClassAncillaryRole = esriNetworkClassAncillaryRole.esriNCARNone;
        //
        // CONSTRUCTOR
        //
        public GeometricNetworkControllerMembership() : base() { }
        public GeometricNetworkControllerMembership(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <GeometricNetworkName>Empty_Net</GeometricNetworkName> 
            XPathNavigator navigatorGeometricNetworkName = navigator.SelectSingleNode("GeometricNetworkName");
            if (navigatorGeometricNetworkName != null) {
                this._geometricNetworkName = navigatorGeometricNetworkName.Value;
            }

            // <EnabledFieldName>Enabled</EnabledFieldName> 
            XPathNavigator navigatorEnabledFieldName = navigator.SelectSingleNode("EnabledFieldName");
            if (navigatorEnabledFieldName != null) {
                this._enabledFieldName = navigatorEnabledFieldName.Value;
            }

            // <AncillaryRoleFieldName /> 
            XPathNavigator navigatorAncillaryRoleFieldName = navigator.SelectSingleNode("AncillaryRoleFieldName");
            if (navigatorAncillaryRoleFieldName != null) {
                this._ancillaryRoleFieldName = navigatorAncillaryRoleFieldName.Value;
            }

            // <NetworkClassAncillaryRole>esriNCARNone</NetworkClassAncillaryRole> 
            XPathNavigator navigatorNetworkClassAncillaryRole = navigator.SelectSingleNode("NetworkClassAncillaryRole");
            if (navigatorNetworkClassAncillaryRole != null) {
                this._networkClassAncillaryRole = (esriNetworkClassAncillaryRole)Enum.Parse(typeof(esriNetworkClassAncillaryRole), navigatorNetworkClassAncillaryRole.Value, true);
            }
        }
        public GeometricNetworkControllerMembership(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._geometricNetworkName = info.GetString("geometricNetworkName");
            this._enabledFieldName = info.GetString("enabledFieldName");
            this._ancillaryRoleFieldName = info.GetString("ancillaryRoleFieldName");
            this._networkClassAncillaryRole = (esriNetworkClassAncillaryRole)Enum.Parse(typeof(esriNetworkClassAncillaryRole), info.GetString("networkClassAncillaryRole"), true);
        }
        public GeometricNetworkControllerMembership(GeometricNetworkControllerMembership prototype) : base(prototype) {
            this._geometricNetworkName = prototype.GeometricNetworkName;
            this._enabledFieldName = prototype.EnabledFieldName;
            this._ancillaryRoleFieldName = prototype.AncillaryRoleFieldName;
            this._networkClassAncillaryRole = prototype.NetworkClassAncillaryRole;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The geometric network in which this class participates
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network Controller")]
        [DefaultValue("")]
        [Description("The geometric network in which this class participates")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(true)]
        public string GeometricNetworkName {
            get { return this._geometricNetworkName; }
            set { this._geometricNetworkName = value; }
        }
        /// <summary>
        /// The name of the Enabled field for the class described by this class description
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network Controller")]
        [DefaultValue("")]
        [Description("The name of the Enabled field for the class described by this class description")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string EnabledFieldName {
            get { return this._enabledFieldName; }
            set { this._enabledFieldName = value; }
        }
        /// <summary>
        /// The name of the Ancillary Role field for the junction feature class described by this class description
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network Controller")]
        [DefaultValue("")]
        [Description("The name of the Ancillary Role field for the junction feature class described by this class description")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string AncillaryRoleFieldName {
            get { return this._ancillaryRoleFieldName; }
            set { this._ancillaryRoleFieldName = value; }
        }
        /// <summary>
        /// The possible network ancillary roles of the contained Features
        /// </summary>
        [Browsable(true)]
        [Category("Geometric Network Controller")]
        [DefaultValue(esriNetworkClassAncillaryRole.esriNCARNone)]
        [Description("The possible network ancillary roles of the contained Features")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkClassAncillaryRole NetworkClassAncillaryRole {
            get { return this._networkClassAncillaryRole; }
            set { this._networkClassAncillaryRole = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = string.Empty;
                switch (this._networkClassAncillaryRole) {
                    case esriNetworkClassAncillaryRole.esriNCARNone:
                        text += "Normal Role";
                        break;
                    case esriNetworkClassAncillaryRole.esriNCARSourceSink:
                        text += "Source/Sink";
                        break;
                    default:
                        break;
                }
                return text;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("geometricNetworkName", this._geometricNetworkName);
            info.AddValue("enabledFieldName", this._enabledFieldName);
            info.AddValue("ancillaryRoleFieldName", this._ancillaryRoleFieldName);
            info.AddValue("networkClassAncillaryRole", this._networkClassAncillaryRole.ToString("d"));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new GeometricNetworkControllerMembership(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // This controller must be assigned to a FeatureClass
            FeatureClass featureClass = table as FeatureClass;
            if (table.GetType() != typeof(FeatureClass)) {
                list.Add(new ErrorObject(this, table, "A geometric network controller is ONLY supported in FeatureClasses", ErrorType.Error));
            }

            // GeometricNetwork Name
            if (string.IsNullOrEmpty(this._geometricNetworkName)) {
                list.Add(new ErrorObject(this, table, "The controller property 'GeometricNetworkName' can not be empty", ErrorType.Error));
            }
            else {
                GeometricNetwork geometricNetwork = DiagrammerEnvironment.Default.SchemaModel.FindGeometricNetwork(this._geometricNetworkName);
                if (geometricNetwork == null) {
                    string message = string.Format("The geometric network [{0}] referenced in the controller does not exist", this._geometricNetworkName);
                    list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                }
            }

            // Enabled Field Cannot be NULL/Empty
            if (string.IsNullOrEmpty(this._enabledFieldName)) {
                list.Add(new ErrorObject(this, table, "The controller property 'EnabledFieldName' can not be empty", ErrorType.Error));
            }
            else {
                if (featureClass != null) {
                    Field field = featureClass.FindField(this._enabledFieldName);
                    if (field == null) {
                        string message = string.Format("The geometric network controller cannot find 'enabled field' [{0}] in the featureclass [{1}]", this._enabledFieldName, featureClass.Name);
                        list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                    }
                    else {
                        if (field.FieldType != esriFieldType.esriFieldTypeSmallInteger) {
                            string message = string.Format("The geometric network controller references an 'enabled field' [{0}] in the featureclass [{1}] that is not a 'Short Integer'", this._enabledFieldName, featureClass.Name);
                            list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                        }
                    }
                }
            }

            // Ancillary Role / Ancillary Role Field
            switch(this._networkClassAncillaryRole){
                case esriNetworkClassAncillaryRole.esriNCARNone:
                    if (!string.IsNullOrEmpty(this._ancillaryRoleFieldName)){
                        string message = string.Format("The ancillary role field name should be empty if the role (in a geometric network controller) is set to none.");
                        list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                    }
                    break;
                case esriNetworkClassAncillaryRole.esriNCARSourceSink:
                    if (string.IsNullOrEmpty(this._ancillaryRoleFieldName)) {
                        string message = string.Format("The ancillary role field name can not be empty for this role in a geometric network controller");
                        list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                    }
                    else {
                        if (featureClass != null) {
                            Field field = featureClass.FindField(this._ancillaryRoleFieldName);
                            if (field == null) {
                                string message = string.Format("The geometric network controller cannot find 'ancillary role field' [{0}] in the featureclass [{1}]", this._ancillaryRoleFieldName, featureClass.Name);
                                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                            }
                            else {
                                if (field.FieldType != esriFieldType.esriFieldTypeSmallInteger) {
                                    string message = string.Format("The geometric network controller references an 'ancillary role field' [{0}] in the featureclass [{1}] that is not a 'Short Integer'", this._ancillaryRoleFieldName, featureClass.Name);
                                    list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                                }
                            }
                        }
                    }
                    break;
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // <ControllerMembership>
            writer.WriteStartElement(Xml.CONTROLLERMEMBERSHIP);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._GEOMETRICNETWORKMEMBERSHIP);

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </ControllerMembership>
            writer.WriteEndElement();
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <GeometricNetworkName></GeometricNetworkName> 
            writer.WriteStartElement("GeometricNetworkName");
            writer.WriteValue(this._geometricNetworkName);
            writer.WriteEndElement();

            // <EnabledFieldName></EnabledFieldName> 
            writer.WriteStartElement("EnabledFieldName");
            writer.WriteValue(this._enabledFieldName);
            writer.WriteEndElement();

            // <AncillaryRoleFieldName></AncillaryRoleFieldName> 
            writer.WriteStartElement("AncillaryRoleFieldName");
            if (!string.IsNullOrEmpty(this._ancillaryRoleFieldName)) {
                writer.WriteValue(this._ancillaryRoleFieldName);
            }
            writer.WriteEndElement();

            // <NetworkClassAncillaryRole></NetworkClassAncillaryRole> 
            writer.WriteStartElement("NetworkClassAncillaryRole");
            writer.WriteValue(this._networkClassAncillaryRole.ToString());
            writer.WriteEndElement();
        }
    }
}

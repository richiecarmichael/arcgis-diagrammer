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

//  <ControllerMembership xsi:type="esri:TopologyMembership">
//    <TopologyName>Landbase_Topology</TopologyName> 
//    <Weight>5</Weight> 
//    <XYRank>1</XYRank> 
//    <ZRank>1</ZRank> 
//    <EventNotificationOnValidate>false</EventNotificationOnValidate> 
//  </ControllerMembership>

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class TopologyControllerMembership : ControllerMembership {
        private string _topologyName = string.Empty;
        private int _weight = -1;
        private int _xyRank = -1;
        private int _zRank = -1;
        private bool _eventNotificationOnValidate = false;
        //
        // CONSTRUCTOR
        //
        public TopologyControllerMembership() : base() { }
        public TopologyControllerMembership(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <TopologyName>Landbase_Topology</TopologyName> 
            XPathNavigator navigatorTopologyName = navigator.SelectSingleNode("TopologyName");
            if (navigatorTopologyName != null) {
                this._topologyName = navigatorTopologyName.Value;
            }

            //    <Weight>5</Weight> 
            XPathNavigator navigatorWeight = navigator.SelectSingleNode("Weight");
            if (navigatorWeight != null) {
                this._weight = navigatorWeight.ValueAsInt;
            }

            //    <XYRank>1</XYRank> 
            XPathNavigator navigatorXYRank = navigator.SelectSingleNode("XYRank");
            if (navigatorXYRank != null) {
                this._xyRank = navigatorXYRank.ValueAsInt;
            }

            //    <ZRank>1</ZRank> 
            XPathNavigator navigatorZRank = navigator.SelectSingleNode("ZRank");
            if (navigatorZRank != null) {
                this._zRank = navigatorZRank.ValueAsInt;
            }

            //    <EventNotificationOnValidate>false</EventNotificationOnValidate> 
            XPathNavigator navigatorEventNotificationOnValidate = navigator.SelectSingleNode("EventNotificationOnValidate");
            if (navigatorEventNotificationOnValidate != null) {
                this._eventNotificationOnValidate = navigatorEventNotificationOnValidate.ValueAsBoolean;
            }
        }
        public TopologyControllerMembership(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._topologyName = info.GetString("topologyName");
            this._weight = info.GetInt32("weight");
            this._xyRank = info.GetInt32("xyRank");
            this._zRank = info.GetInt32("zRank");
            this._eventNotificationOnValidate = info.GetBoolean("eventNotificationOnValidate");
        }
        public TopologyControllerMembership(TopologyControllerMembership prototype) : base(prototype) {
            this._topologyName = prototype.TopologyName;
            this._weight = prototype.Weight;
            this._xyRank = prototype.XYRank;
            this._zRank = prototype.ZRank;
            this._eventNotificationOnValidate = prototype.EventNotificationOnValidate;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The name of the Topology this feature class participates in
        /// </summary>
        [Browsable(true)]
        [Category("Topology Controller")]
        [DefaultValue("")]
        [Description("The name of the Topology this feature class participates in")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(true)]
        public string TopologyName {
            get { return this._topologyName; }
            set { this._topologyName = value; }
        }
        /// <summary>
        /// The weight of the feature class in the topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology Controller")]
        [DefaultValue(-1)]
        [Description("The weight of the feature class in the topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int Weight {
            get { return this._weight; }
            set { this._weight = value; }
        }
        /// <summary>
        /// The XYRank of the feature class in the topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology Controller")]
        [DefaultValue(-1)]
        [Description("The XYRank of the feature class in the topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int XYRank {
            get { return this._xyRank; }
            set { this._xyRank = value; }
        }
        /// <summary>
        /// The ZRank of the feature class in the topology
        /// </summary>
        [Browsable(true)]
        [Category("Topology Controller")]
        [DefaultValue(-1)]
        [Description("The ZRank of the feature class in the topology")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int ZRank {
            get { return this._zRank; }
            set { this._zRank = value; }
        }
        /// <summary>
        /// Indicates if event notification is fired on validate
        /// </summary>
        [Browsable(true)]
        [Category("Topology Controller")]
        [DefaultValue(false)]
        [Description("Indicates if event notification is fired on validate")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool EventNotificationOnValidate {
            get { return this._eventNotificationOnValidate; }
            set { this._eventNotificationOnValidate = value; }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = string.Empty;

                // Weight
                text += string.Format("Weight: {0}", this._weight.ToString());
                text += Environment.NewLine;

                // XYRank
                text += string.Format("XYRank: {0}", this._xyRank.ToString());
                text += Environment.NewLine;

                // ZRank
                text += string.Format("ZRank: {0}", this._zRank.ToString());
                text += Environment.NewLine;

                // Notification
                string notification = this._eventNotificationOnValidate ? "On" : "Off";
                text += string.Format("Notification: {0}", notification);

                // Return Text
                return text;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("topologyName", this._topologyName);
            info.AddValue("weight", this._weight);
            info.AddValue("xyRank", this._xyRank);
            info.AddValue("zRank", this._zRank);
            info.AddValue("eventNotificationOnValidate", this._eventNotificationOnValidate);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new TopologyControllerMembership(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO: Add Topology Controller Membership Errors
        }
        public override void WriteXml(XmlWriter writer) {
            // <ControllerMembership>
            writer.WriteStartElement(Xml.CONTROLLERMEMBERSHIP);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._TOPOLOGYMEMBERSHIP);

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

            // <TopologyName>Landbase_Topology</TopologyName> 
            writer.WriteStartElement("TopologyName");
            writer.WriteValue(this._topologyName);
            writer.WriteEndElement();

            // <Weight>5</Weight> 
            writer.WriteStartElement("Weight");
            writer.WriteValue(this._weight);
            writer.WriteEndElement();

            // <XYRank>1</XYRank> 
            writer.WriteStartElement("XYRank");
            writer.WriteValue(this._xyRank);
            writer.WriteEndElement();

            // <ZRank>1</ZRank> 
            writer.WriteStartElement("ZRank");
            writer.WriteValue(this._zRank);
            writer.WriteEndElement();

            // <EventNotificationOnValidate>false</EventNotificationOnValidate> 
            writer.WriteStartElement("EventNotificationOnValidate");
            writer.WriteValue(this._eventNotificationOnValidate);
            writer.WriteEndElement();
        }
    }
}

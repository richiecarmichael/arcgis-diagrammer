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
    public class NetWeight : EsriObject, IDiagramProperty {
        private int _weightID = -1;
        private string _weightName = string.Empty;
        private esriWeightType _weightType = esriWeightType.esriWTNull;
        private int _bitGateSize = -1;
        private bool _showLabels = false;
        //
        // CONSTRUCTOR
        //
        public NetWeight() : base() { }
        public NetWeight(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <WeightID>0</WeightID> 
            XPathNavigator navigatorWeightID = navigator.SelectSingleNode("WeightID");
            if (navigatorWeightID != null) {
                this._weightID = navigatorWeightID.ValueAsInt;
            }

            // <WeightName>MMElectricTraceWeight</WeightName> 
            XPathNavigator navigatorWeightName = navigator.SelectSingleNode("WeightName");
            if (navigatorWeightName != null) {
                this._weightName = navigatorWeightName.Value;
            }

            // <WeightType>esriWTInteger</WeightType> 
            XPathNavigator navigatorWeightType = navigator.SelectSingleNode("WeightType");
            if (navigatorWeightType != null) {
                this._weightType = (esriWeightType)Enum.Parse(typeof(esriWeightType), navigatorWeightType.Value, true);
            }

            // <BitGateSize>0</BitGateSize> 
            XPathNavigator navigatorBitGateSize = navigator.SelectSingleNode("BitGateSize");
            if (navigatorBitGateSize != null) {
                this._bitGateSize = navigatorBitGateSize.ValueAsInt;
            }
        }
        public NetWeight(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._weightID = info.GetInt32("weightID");
            this._weightName = info.GetString("weightName");
            this._weightType = (esriWeightType)Enum.Parse(typeof(esriWeightType), info.GetString("weightType"), true);
            this._bitGateSize = info.GetInt32("bitGateSize");
        }
        public NetWeight(NetWeight prototype) : base(prototype) {
            this._weightID = prototype.WeightID;
            this._weightName = prototype.WeightName;
            this._weightType = prototype.WeightType;
            this._bitGateSize = prototype.BitGateSize;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Internal ID of the network weight described by this NetWeight object
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight")]
        [DefaultValue("-1")]
        [Description("Internal ID of the network weight described by this NetWeight object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int WeightID {
            get { return this._weightID; }
            set { this._weightID = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Name of the network weight described by this NetWeight object
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight")]
        [DefaultValue("-1")]
        [Description("Name of the network weight described by this NetWeight object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string WeightName {
            get { return this._weightName; }
            set { this._weightName = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Type of network weight described by this NetWeight object
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight")]
        [DefaultValue(esriWeightType.esriWTNull)]
        [Description("Type of network weight described by this NetWeight object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriWeightType WeightType {
            get { return this._weightType; }
            set { this._weightType = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Bit gate size of the network weight described by this NetWeight object
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight")]
        [DefaultValue("-1")]
        [Description("Bit gate size of the network weight described by this NetWeight object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int BitGateSize {
            get { return this._bitGateSize; }
            set { this._bitGateSize = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Show/Hide Rule Text Labels
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight")]
        [DefaultValue(false)]
        [Description("Show/Hide Text Labels")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ShowLabels {
            get { return this._showLabels; }
            set { this._showLabels = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        [Browsable(false)]
        public string Label {
            get { return string.Format(this._weightName); }
        }
        [Browsable(false)]
        public virtual float BorderWidth {
            get { return 1f; }
        }
        //
        // PUBLIC METHODS
        // 
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("weightID", this._weightID);
            info.AddValue("weightName", this._weightName);
            info.AddValue("weightType", Convert.ToInt32(this._weightType).ToString());
            info.AddValue("bitGateSize", this._bitGateSize);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new NetWeight(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO : Add Errors!
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetWeight xsi:type="esri:NetWeight">
            //    <WeightID>0</WeightID> 
            //    <WeightName>MMElectricTraceWeight</WeightName> 
            //    <WeightType>esriWTInteger</WeightType> 
            //    <BitGateSize>0</BitGateSize> 
            //</NetWeight>

            // <NetWeight>
            writer.WriteStartElement("NetWeight");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetWeight");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </NetWeight>
            writer.WriteEndElement();
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <WeightID>0</WeightID> 
            writer.WriteStartElement("WeightID");
            writer.WriteValue(this._weightID);
            writer.WriteEndElement();

            // <WeightName>MMElectricTraceWeight</WeightName> 
            writer.WriteStartElement("WeightName");
            writer.WriteValue(this._weightName);
            writer.WriteEndElement();

            // <WeightType>esriWTInteger</WeightType> 
            writer.WriteStartElement("WeightType");
            writer.WriteValue(this._weightType.ToString());
            writer.WriteEndElement();

            // <BitGateSize>0</BitGateSize> 
            writer.WriteStartElement("BitGateSize");
            writer.WriteValue(this._bitGateSize);
            writer.WriteEndElement();
        }
    }
}

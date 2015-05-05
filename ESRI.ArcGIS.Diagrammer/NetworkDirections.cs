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
    /// <summary>
    /// ESRI Network Directions
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class NetworkDirections : EsriObject {
        private esriNetworkAttributeUnits _defaultOutputLengthUnits = esriNetworkAttributeUnits.esriNAUUnknown;
        private string _lengthAttributeName = string.Empty;
        private string _timeAttributeName = string.Empty;
        private string _roadClassAttributeName = string.Empty;
        private string _signpostFeatureClassName = string.Empty;
        private string _signpostStreetsTableName = string.Empty;
        //
        // CONSTRUCTOR
        //
        public NetworkDirections() : base() { }
        public NetworkDirections(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // DefaultOutputLengthUnits
            XPathNavigator navigatorDefaultOutputLengthUnits = navigator.SelectSingleNode("DefaultOutputLengthUnits");
            if (navigatorDefaultOutputLengthUnits != null) {
                this._defaultOutputLengthUnits = (esriNetworkAttributeUnits)Enum.Parse(typeof(esriNetworkAttributeUnits), navigatorDefaultOutputLengthUnits.Value, true);
            }

            // LengthAttributeName
            XPathNavigator navigatorLengthAttributeName = navigator.SelectSingleNode("LengthAttributeName");
            if (navigatorLengthAttributeName != null) {
                this._lengthAttributeName = navigatorLengthAttributeName.Value;
            }

            // TimeAttributeName
            XPathNavigator navigatorTimeAttributeName = navigator.SelectSingleNode("TimeAttributeName");
            if (navigatorTimeAttributeName != null) {
                this._timeAttributeName = navigatorTimeAttributeName.Value;
            }

            // RoadClassAttributeName
            XPathNavigator navigatorRoadClassAttributeName = navigator.SelectSingleNode("RoadClassAttributeName");
            if (navigatorRoadClassAttributeName != null) {
                this._roadClassAttributeName = navigatorRoadClassAttributeName.Value;
            }

            // SignpostFeatureClassName
            XPathNavigator navigatorSignpostFeatureClassName = navigator.SelectSingleNode("SignpostFeatureClassName");
            if (navigatorSignpostFeatureClassName != null) {
                this._signpostFeatureClassName = navigatorSignpostFeatureClassName.Value;
            }

            // SignpostStreetsTableName
            XPathNavigator navigatorSignpostStreetsTableName = navigator.SelectSingleNode("SignpostStreetsTableName");
            if (navigatorSignpostStreetsTableName != null) {
                this._signpostStreetsTableName = navigatorSignpostStreetsTableName.Value;
            }
        }
        public NetworkDirections(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._defaultOutputLengthUnits = (esriNetworkAttributeUnits)Enum.Parse(typeof(esriNetworkAttributeUnits), info.GetString("defaultOutputLengthUnits"), true);
            this._lengthAttributeName = info.GetString("lengthAttributeName");
            this._timeAttributeName = info.GetString("timeAttributeName");
            this._roadClassAttributeName = info.GetString("roadClassAttributeName");
            this._signpostFeatureClassName = info.GetString("signpostFeatureClassName");
            this._signpostStreetsTableName = info.GetString("signpostStreetsTableName");
        }
        public NetworkDirections(NetworkDirections prototype) : base(prototype) {
            this._defaultOutputLengthUnits = prototype.DefaultOutputLengthUnits;
            this._lengthAttributeName = prototype.LengthAttributeName;
            this._timeAttributeName = prototype.TimeAttributeName;
            this._roadClassAttributeName = prototype.RoadClassAttributeName;
            this._signpostFeatureClassName = prototype.SignpostFeatureClassName;
            this._signpostStreetsTableName = prototype.SignpostStreetsTableName;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The default length units that will be used for reporting distances in driving directions
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue(esriNetworkAttributeUnits.esriNAUUnknown)]
        [Description("The default length units that will be used for reporting distances in driving directions")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriNetworkAttributeUnits DefaultOutputLengthUnits {
            get { return this._defaultOutputLengthUnits; }
            set { this._defaultOutputLengthUnits = value; }
        }
        /// <summary>
        /// The name of the network attribute to be used for reporting travel distances
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue("")]
        [Description("The name of the network attribute to be used for reporting travel distances")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string LengthAttributeName {
            get { return this._lengthAttributeName; }
            set { this._lengthAttributeName = value; }
        }
        /// <summary>
        /// The name of the network attribute to be used for reporting travel time
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue("")]
        [Description("The name of the network attribute to be used for reporting travel time")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string TimeAttributeName {
            get { return this._timeAttributeName; }
            set { this._timeAttributeName = value; }
        }
        /// <summary>
        /// The name of the network attribute to be used for road classification
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue("")]
        [Description("The name of the network attribute to be used for road classification")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string RoadClassAttributeName {
            get { return this._roadClassAttributeName; }
            set { this._roadClassAttributeName = value; }
        }
        /// <summary>
        /// The name of the feature class containing the signposts
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue("")]
        [Description("The name of the feature class containing the signposts")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string SignpostFeatureClassName {
            get { return this._signpostFeatureClassName; }
            set { this._signpostFeatureClassName = value; }
        }
        /// <summary>
        /// The name of the indexed table of signpost street references
        /// </summary>
        [Browsable(true)]
        [Category("Network Directions")]
        [DefaultValue("")]
        [Description("The name of the indexed table of signpost street references")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string SignpostStreetsTableName {
            get { return this._signpostStreetsTableName; }
            set { this._signpostStreetsTableName = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("defaultOutputLengthUnits", Convert.ToInt32(this._defaultOutputLengthUnits).ToString());
            info.AddValue("lengthAttributeName", this._lengthAttributeName);
            info.AddValue("timeAttributeName", this._timeAttributeName);
            info.AddValue("roadClassAttributeName", this._roadClassAttributeName);
            info.AddValue("signpostFeatureClassName", this._signpostFeatureClassName);
            info.AddValue("signpostStreetsTableName", this._signpostStreetsTableName);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Direction Errors
        }
        public override object Clone() {
            return new NetworkDirections(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetworkDirections>
            writer.WriteStartElement("NetworkDirections");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetworkDirections");

            // <DefaultOutputLengthUnits></DefaultOutputLengthUnits>
            writer.WriteStartElement("DefaultOutputLengthUnits");
            writer.WriteValue(this._defaultOutputLengthUnits.ToString());
            writer.WriteEndElement();

            // <LengthAttributeName></LengthAttributeName>
            writer.WriteStartElement("LengthAttributeName");
            writer.WriteValue(this._lengthAttributeName);
            writer.WriteEndElement();

            // <TimeAttributeName></TimeAttributeName>
            writer.WriteStartElement("TimeAttributeName");
            writer.WriteValue(this._timeAttributeName);
            writer.WriteEndElement();

            // <RoadClassAttributeName></RoadClassAttributeName>
            writer.WriteStartElement("RoadClassAttributeName");
            writer.WriteValue(this._roadClassAttributeName);
            writer.WriteEndElement();

            // <SignpostFeatureClassName></SignpostFeatureClassName >
            writer.WriteStartElement("SignpostFeatureClassName");
            writer.WriteValue(this._signpostFeatureClassName);
            writer.WriteEndElement();

            // <SignpostStreetsTableName></SignpostStreetsTableName>
            writer.WriteStartElement("SignpostStreetsTableName");
            writer.WriteValue(this._signpostStreetsTableName);
            writer.WriteEndElement();

            // </NetworkDirections>
            writer.WriteEndElement();
        }
    }
}

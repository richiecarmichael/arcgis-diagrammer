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

// <ControllerMembership xsi:type="esri:NetworkDatasetMembership">
//   <NetworkDatasetName>ParisNet</NetworkDatasetName> 
// </ControllerMembership>

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class NetworkControllerMembership : ControllerMembership {
        private string _networkDatasetName = string.Empty;
        //
        // CONSTRUCTOR
        //
        public NetworkControllerMembership() : base() { }
        public NetworkControllerMembership(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <NetworkDatasetName>ParisNet</NetworkDatasetName> 
            XPathNavigator navigatorNetworkDatasetName = navigator.SelectSingleNode("NetworkDatasetName");
            if (navigatorNetworkDatasetName != null) {
                this._networkDatasetName = navigatorNetworkDatasetName.Value;
            }
        }
        public NetworkControllerMembership(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._networkDatasetName = info.GetString("networkDatasetName");
        }
        public NetworkControllerMembership(NetworkControllerMembership prototype) : base(prototype) {
            this._networkDatasetName = prototype.NetworkDatasetName;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The name of the Network this feature class participates in
        /// </summary>
        [Browsable(true)]
        [Category("Network Controller")]
        [DefaultValue("")]
        [Description("The name of the Network this feature class participates in")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(true)]
        public string NetworkDatasetName {
            get { return this._networkDatasetName; }
            set { this._networkDatasetName = value; }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("networkDatasetName", this._networkDatasetName);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new NetworkControllerMembership(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO: Add Topology Controller Membership Errors
        }
        public override void WriteXml(XmlWriter writer) {
            // <ControllerMembership>
            writer.WriteStartElement(Xml.CONTROLLERMEMBERSHIP);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._NETWORKDATASETMEMBERSHIP);

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </ControllerMembership>
            writer.WriteEndElement();
        }
        public override string Label {
            get { return string.Empty; }
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Innner Xml
            base.WriteInnerXml(writer);

            // <NetworkDatasetName></NetworkDatasetName> 
            writer.WriteStartElement("NetworkDatasetName");
            writer.WriteValue(this._networkDatasetName);
            writer.WriteEndElement();
        }
    }
}

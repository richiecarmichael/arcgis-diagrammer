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
    /// ESRI Network Source Directions
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableConverter))]
    public class NetworkSourceDirections : EsriObject {
        private string _adminAreaFieldName = string.Empty;
        private Shields _shields = null;
        private List<StreetNameFields> _streetNameFields = null;
        //
        // CONSTRUCTOR
        //
        //public NetworkSourceDirections() : base() { }
        public NetworkSourceDirections(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <AdminAreaFieldName>
            XPathNavigator navigatorAdminAreaFieldName = navigator.SelectSingleNode("AdminAreaFieldName");
            if (navigatorAdminAreaFieldName != null) {
                this._adminAreaFieldName = navigatorAdminAreaFieldName.Value;
            }

            // <Shields>
            XPathNavigator navigatorShields = navigator.SelectSingleNode("Shields");
            if (navigatorShields != null) {
                this._shields = new Shields(navigatorShields);
            }
            else {
                this._shields = new Shields();
            }

            // <StreetNameFields><StreetNameFields>
            this._streetNameFields = new List<StreetNameFields>();
            XPathNodeIterator interatorStreetNameFields = navigator.Select("StreetNameFields/StreetNameFields");
            while (interatorStreetNameFields.MoveNext()) {
                // Get <StreetNameFields>
                XPathNavigator navigatorStreetNameFields = interatorStreetNameFields.Current;

                // Add StreetNameFields
                StreetNameFields streetNameFields = new StreetNameFields(navigatorStreetNameFields);
                this._streetNameFields.Add(streetNameFields);
            }
        }
        public NetworkSourceDirections(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._adminAreaFieldName = info.GetString("adminAreaFieldName");
            this._shields = (Shields)info.GetValue("shields", typeof(Shields));
            this._streetNameFields = (List<StreetNameFields>)info.GetValue("streetNameFields", typeof(List<StreetNameFields>));
        }
        public NetworkSourceDirections(NetworkSourceDirections prototype) : base(prototype) {
            this._adminAreaFieldName = prototype.AdminAreaFieldName;
            this._shields = (Shields)prototype.Shields.Clone();
            this._streetNameFields = new List<StreetNameFields>();
            foreach (StreetNameFields streetNameFields in prototype.StreetNameFieldsCollection) {
                StreetNameFields streetNameFieldsClone = (StreetNameFields)streetNameFields.Clone();
                this._streetNameFields.Add(streetNameFieldsClone);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The default length units that will be used for reporting distances in driving directions
        /// </summary>
        [Browsable(true)]
        [Category("Network Source Directions")]
        [DefaultValue(esriNetworkAttributeUnits.esriNAUUnknown)]
        [Description("The default length units that will be used for reporting distances in driving directions")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string AdminAreaFieldName {
            get { return this._adminAreaFieldName; }
            set { this._adminAreaFieldName = value; }
        }
        /// <summary>
        /// The name of the network attribute to be used for reporting travel distances
        /// </summary>
        [Browsable(true)]
        [Category("Network Source Directions")]
        [DefaultValue("")]
        [Description("The name of the network attribute to be used for reporting travel distances")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Shields Shields {
            get { return this._shields; }
            set { this._shields = value; }
        }
        /// <summary>
        /// The name of the network attribute to be used for reporting travel time
        /// </summary>
        [Browsable(true)]
        [Category("Network Source Directions")]
        [DefaultValue("")]
        [Description("The name of the network attribute to be used for reporting travel time")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<StreetNameFields> StreetNameFieldsCollection {
            get { return this._streetNameFields; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("adminAreaFieldName", this._adminAreaFieldName);
            info.AddValue("shields", this._shields, typeof(Shields));
            info.AddValue("streetNameFields", this._streetNameFields, typeof(List<StreetNameFields>));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Network Source Direction Errors
        }
        public override object Clone() {
            return new NetworkSourceDirections(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetworkSourceDirections>
            writer.WriteStartElement("NetworkSourceDirections");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetworkSourceDirections");

            // <AdminAreaFieldName></AdminAreaFieldName>
            writer.WriteStartElement("AdminAreaFieldName");
            writer.WriteValue(this._adminAreaFieldName);
            writer.WriteEndElement();

            // <Shields></Shields>
            this._shields.WriteXml(writer);

            // <StreetNameFields> (Array)
            writer.WriteStartElement("StreetNameFields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfStreetNameFields");

            // <StreetNameFields></StreetNameFields>
            foreach (StreetNameFields streetNameFields in this._streetNameFields) {
                streetNameFields.WriteXml(writer);
            }

            // </StreetNameFields> (Array)
            writer.WriteEndElement();

            // </NetworkSourceDirections>
            writer.WriteEndElement();
        }
    }
}

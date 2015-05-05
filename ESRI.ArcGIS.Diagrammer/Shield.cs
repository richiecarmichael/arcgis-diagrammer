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
    /// ESRI Shield
    /// </summary>
    [Serializable]
    public class Shield : EsriObject {
        private string _shieldType = string.Empty;
        private string _shieldDescription = string.Empty;
        //
        // CONSTRUCTOR
        //
        public Shield() : base() { }
        public Shield(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <ShieldType>
            XPathNavigator navigatorShieldType = navigator.SelectSingleNode("ShieldType");
            if (navigatorShieldType != null) {
                this._shieldType = navigatorShieldType.Value;
            }

            // <ShieldDescription>
            XPathNavigator navigatorShieldDescription = navigator.SelectSingleNode("ShieldDescription");
            if (navigatorShieldDescription != null) {
                this._shieldDescription = navigatorShieldDescription.Value;
            }
        }
        public Shield(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._shieldType = info.GetString("shieldType");
            this._shieldDescription = info.GetString("shieldDescription");
        }
        public Shield(Shield prototype) : base(prototype) {
            this._shieldType = prototype.ShieldType;
            this._shieldDescription = prototype.ShieldDescription;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The shield description
        /// </summary>
        [Browsable(true)]
        [Category("Shield")]
        [DefaultValue("")]
        [Description("The shield description")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ShieldType {
            get { return this._shieldType; }
            set { this._shieldType = value; }
        }
        /// <summary>
        /// The shield type
        /// </summary>
        [Browsable(true)]
        [Category("Shield")]
        [DefaultValue("")]
        [Description("The shield type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ShieldDescription {
            get { return this._shieldDescription; }
            set { this._shieldDescription = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("shieldType", this._shieldType);
            info.AddValue("shieldDescription", this._shieldDescription);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Shield Errors
        }
        public override object Clone() {
            return new Shield(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Shield>
            writer.WriteStartElement("Shield");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Shield");

            // <ShieldType></ShieldType>
            writer.WriteStartElement("ShieldType");
            writer.WriteValue(this._shieldType);
            writer.WriteEndElement();

            // <ShieldDescription></ShieldDescription>
            writer.WriteStartElement("ShieldDescription");
            writer.WriteValue(this._shieldDescription);
            writer.WriteEndElement();

            // </Shield>
            writer.WriteEndElement();
        }
    }
}

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
    [Serializable]
    public class JunctionSubtype : EsriObject {
        //
        private const string CLASSID = "classID";
        private const string SUBTYPECODE = "subtypeCode";

        //
        private int _classID = -1;
        private int _subtypeCode = -1;
        //
        // CONSTRUCTOR
        //
        public JunctionSubtype() : base() { }
        public JunctionSubtype(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <ClassID></ClassID>
            XPathNavigator navigatorClassID = navigator.SelectSingleNode(Xml.CLASSID);
            if (navigatorClassID != null) {
                this._classID = navigatorClassID.ValueAsInt;
            }

            // <SubtypeCode></SubtypeCode>
            XPathNavigator navigatorSubtypeCode = navigator.SelectSingleNode(Xml.SUBTYPECODE);
            if (navigatorSubtypeCode != null) {
                this._subtypeCode = navigatorSubtypeCode.ValueAsInt;
            }
        }
        public JunctionSubtype(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._classID = info.GetInt32(JunctionSubtype.CLASSID);
            this._subtypeCode = info.GetInt32(JunctionSubtype.SUBTYPECODE);
        }
        public JunctionSubtype(JunctionSubtype prototype) : base(prototype) {
            this._classID = prototype.ClassID;
            this._subtypeCode = prototype.SubtypeCode;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Junction Subtype Class Id
        /// </summary>
        [Browsable(true)]
        [Category("Junction Subtype")]
        [DefaultValue(-1)]
        [Description("Junction Subtype Class Id")]
        [DisplayName("Dataset Id")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int ClassID {
            get { return this._classID; }
            set { this._classID = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Junction Subtype Code
        /// </summary>
        [Browsable(true)]
        [Category("Junction Subtype")]
        [DefaultValue(-1)]
        [Description("Junction Subtype Code")]
        [DisplayName("Subtype Code")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int SubtypeCode {
            get { return this._subtypeCode; }
            set { this._subtypeCode = value; if (!base.Suspended) { base.OnInvalidated(EventArgs.Empty); } }
        }
        //
        // PUBLIC METHODS
        // 
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(JunctionSubtype.CLASSID, this._classID);
            info.AddValue(JunctionSubtype.SUBTYPECODE, this._subtypeCode);
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new JunctionSubtype(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Check if class/subtype exists
            EsriTable table2 = DiagrammerEnvironment.Default.SchemaModel.FindObjectClassOrSubtype(this._classID, this._subtypeCode);
            if (table2 == null) {
                string message = string.Format("The junction subtype [{0}::{1}] does not exist", this._classID.ToString(), this._subtypeCode.ToString());
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }
        }
        public override void WriteXml(System.Xml.XmlWriter writer) {
             // <JunctionSubtype xsi:type="esri:JunctionSubtype">
            writer.WriteStartElement(Xml.JUNCTIONSUBTYPE);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._JUNCTIONSUBTYPE);

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </JunctionSubtype>
            writer.WriteEndElement();
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <ClassID></ClassID>
            writer.WriteStartElement(Xml.CLASSID);
            writer.WriteValue(this._classID);
            writer.WriteEndElement();

            // <SubtypeCode></SubtypeCode>
            writer.WriteStartElement(Xml.SUBTYPECODE);
            writer.WriteValue(this._subtypeCode);
            writer.WriteEndElement();
        }
    }
}

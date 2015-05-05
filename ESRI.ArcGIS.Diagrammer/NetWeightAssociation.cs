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
    public class NetWeightAssociation : EsriObject, IDiagramProperty {
        private int _weightID = -1;
        private string _tableName = string.Empty;
        private string _fieldName = string.Empty;
        private bool _showLabels = false;
        //
        // CONSTRUCTOR
        //
        public NetWeightAssociation() : base() {}
        public NetWeightAssociation(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <WeightID></WeightID>
            XPathNavigator navigatorWeightID = navigator.SelectSingleNode("WeightID");
            if (navigatorWeightID != null) {
                this._weightID = navigatorWeightID.ValueAsInt;
            }

            // <TableName></TableName> 
            XPathNavigator navigatorTableName = navigator.SelectSingleNode("TableName");
            if (navigatorTableName != null) {
                this._tableName = navigatorTableName.Value;
            }

            // <FieldName></FieldName> 
            XPathNavigator navigatorFieldName = navigator.SelectSingleNode("FieldName");
            if (navigatorFieldName != null) {
                this._fieldName = navigatorFieldName.Value;
            }
        }
        public NetWeightAssociation(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._weightID = info.GetInt32("weightID");
            this._tableName = info.GetString("tableName");
            this._fieldName = info.GetString("fieldName");
        }
        public NetWeightAssociation(NetWeightAssociation prototype) : base(prototype) {
            this._weightID = prototype.WeightID;
            this._tableName = prototype.TableName;
            this._fieldName = prototype.FieldName;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Internal ID of the network weight described by this NetWeight object
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight Association")]
        [DefaultValue("-1")]
        [Description("Internal ID of the network weight described by this NetWeight object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int WeightID {
            get { return this._weightID; }
            set { this._weightID = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Name of the table to which this weight is associated
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight Association")]
        [DefaultValue("")]
        [Description("Name of the table to which this weight is associated")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public string TableName {
            get { return this._tableName; }
            set { this._tableName = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
            /// Name of the field that contains the values for this weight
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight Association")]
        [DefaultValue("")]
        [Description("Name of the field that contains the values for this weight")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string FieldName {
            get { return this._fieldName; }
            set { this._fieldName = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Show/Hide Text Labels
        /// </summary>
        [Browsable(true)]
        [Category("Network Weight Association")]
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
            get {
                string text = string.Format("{0}::{1}", this._tableName, this._fieldName);
                return text;
            }
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
            info.AddValue("tableName", this._tableName);
            info.AddValue("fieldName", this._fieldName);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new NetWeightAssociation(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO : Add Errors!
        }
        public override void WriteXml(XmlWriter writer) {
            // <NetWeightAssociation xsi:type="esri:NetWeightAssociation">
            //    <WeightID>0</WeightID> 
            //    <TableName>BusBar</TableName> 
            //    <FieldName>ElectricTraceWeight</FieldName> 
            //</NetWeightAssociation>

            // <NetWeightAssociation>
            writer.WriteStartElement("NetWeightAssociation");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:NetWeightAssociation");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </NetWeightAssociation>
            writer.WriteEndElement();
        }
        //
        // PROTECTED
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Inner Xml
            base.WriteInnerXml(writer);

            // <WeightID>0</WeightID> 
            writer.WriteStartElement("WeightID");
            writer.WriteValue(this._weightID);
            writer.WriteEndElement();

            // <TableName>BusBar</TableName> 
            writer.WriteStartElement("TableName");
            writer.WriteValue(this._tableName);
            writer.WriteEndElement();

            // <FieldName>ElectricTraceWeight</FieldName> 
            writer.WriteStartElement("FieldName");
            writer.WriteValue(this._fieldName);
            writer.WriteEndElement();
        }
    }    
}
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
    /// ESRI Edge Feature Source
    /// </summary>
    [Serializable]
    public class EdgeFeatureSource : NetworkSource {
        private string _fromElevationFieldName = string.Empty;
        private string _toElevationFieldName = string.Empty;
        private List<Property> _connectivity = null;
        //
        // CONSTRUCTOR
        //
        //public EdgeFeatureSource() : base() { }
        public EdgeFeatureSource(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <FromElevationFieldName>
            XPathNavigator navigatorFromElevationFieldName = navigator.SelectSingleNode("FromElevationFieldName");
            if (navigatorFromElevationFieldName != null) {
                this._fromElevationFieldName = navigatorFromElevationFieldName.Value;
            }

            // <ToElevationFieldName>
            XPathNavigator navigatorToElevationFieldName = navigator.SelectSingleNode("ToElevationFieldName");
            if (navigatorToElevationFieldName != null) {
                this._toElevationFieldName = navigatorToElevationFieldName.Value;
            }

            // <Connectivity><PropertyArray><PropertySetProperty>
            this._connectivity = new List<Property>();
            XPathNodeIterator interatorProperty = navigator.Select("Connectivity/PropertyArray/PropertySetProperty");
            while (interatorProperty.MoveNext()) {
                // Get <Property>
                XPathNavigator navigatorProperty = interatorProperty.Current;

                // Add Property
                Property property = new Property(navigatorProperty);
                this._connectivity.Add(property);
            }
        }
        public EdgeFeatureSource(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._fromElevationFieldName = info.GetString("fromElevationFieldName");
            this._toElevationFieldName = info.GetString("toElevationFieldName");
            this._connectivity = (List<Property>)info.GetValue("connectivity", typeof(List<Property>));
        }
        public EdgeFeatureSource(EdgeFeatureSource prototype) : base(prototype) {
            this._fromElevationFieldName = prototype.FromElevationFieldName;
            this._toElevationFieldName = prototype.ToElevationFieldName;
            this._connectivity = new List<Property>();
            foreach (Property property in prototype.ConnectivityCollection) {
                Property propertyClone = (Property)property.Clone();
                this._connectivity.Add(property);
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The field name on the feature source to be used as the from elevation field when determining connectivity at coincident end vertices
        /// </summary>
        [Browsable(true)]
        [Category("Edge Feature Source")]
        [DefaultValue("")]
        [Description("The field name on the feature source to be used as the from elevation field when determining connectivity at coincident end vertices")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string FromElevationFieldName {
            get { return this._fromElevationFieldName; }
            set { this._fromElevationFieldName = value; }
        }
        /// <summary>
        /// The field name on the feature source to be used as the to elevation field when determining connectivity at coincident end vertices
        /// </summary>
        [Browsable(true)]
        [Category("Edge Feature Source")]
        [DefaultValue("")]
        [Description("The field name on the feature source to be used as the to elevation field when determining connectivity at coincident end vertices")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ToElevationFieldName {
            get { return this._toElevationFieldName; }
            set { this._toElevationFieldName = value; }
        }
        /// <summary>
        /// Collection of Network Connectivity Rules
        /// </summary>
        [Browsable(true)]
        [Category("Edge Feature Source")]
        [DefaultValue(null)]
        [Description("Collection of Network Connectivity Rules")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> ConnectivityCollection {
            get { return this._connectivity; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("fromElevationFieldName", this._fromElevationFieldName);
            info.AddValue("toElevationFieldName", this._toElevationFieldName);
            info.AddValue("connectivity", this._connectivity, typeof(List<Property>));

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO Edge Feature Source Errors
        }
        public override object Clone() {
            return new EdgeFeatureSource(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <EdgeFeatureSource>
            writer.WriteStartElement("EdgeFeatureSource");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:EdgeFeatureSource");

            // Writer Inner Xml
            this.WriteInnerXml(writer);

            // </EdgeFeatureSource>
            writer.WriteEndElement();
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Class
            base.WriteInnerXml(writer);

            // <FromElevationFieldName></FromElevationFieldName>
            writer.WriteStartElement("FromElevationFieldName");
            writer.WriteValue(this._fromElevationFieldName);
            writer.WriteEndElement();

            // <ToElevationFieldName></ToElevationFieldName>
            writer.WriteStartElement("ToElevationFieldName");
            writer.WriteValue(this._toElevationFieldName);
            writer.WriteEndElement();

            // <Connectivity>
            writer.WriteStartElement("Connectivity");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySet");

            // <PropertyArray>
            writer.WriteStartElement("PropertyArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfPropertySetProperty");

            // <PropertySetProperty>
            foreach (Property property in this._connectivity) {
                property.WriteXml(writer);
            }

            // </PropertyArray>
            writer.WriteEndElement();

            // </Connectivity>
            writer.WriteEndElement();
        }
    }
}

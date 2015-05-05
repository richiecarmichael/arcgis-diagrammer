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
    /// ESRI Index Field
    /// </summary>
    [Serializable]
    public class IndexField : EsriTableRow {
        private string _name = string.Empty;
        //
        // CONSTRUCTOR
        //
        public IndexField() : base() {
            this.UpdateText();
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public IndexField(IXPathNavigable path) : base(path) {
            XPathNavigator navigator = path.CreateNavigator();

            XPathNavigator navigatorName = navigator.SelectSingleNode("Name");
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // Initialize
            this.UpdateText();

            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public IndexField(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._name = info.GetString("name");

            // Initialize
            this.UpdateText();

            // Set Base
            this.Image = new Crainiate.ERM4.Image("Resource.publicfield.gif", "Crainiate.ERM4.Component");
        }
        public IndexField(IndexField prototype) : base(prototype) {
            this._name = prototype.Name;

            // Initialize
            this.UpdateText();
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Name of a Field belonging to the parent Index
        /// </summary>
        [Browsable(true)]
        [Category("IndexField")]
        [DefaultValue("")]
        [Description("Field Name")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string Name {
            get { return this._name; }
            set { this._name = value; this.UpdateText(); }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", this._name);
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new IndexField(this);
        }
        public override void WriteXml(XmlWriter writer) {
            Field field = null;
            if (this.Table is ObjectClass) {
                ObjectClass objectClass = (ObjectClass)this.Table;
                field = objectClass.FindField(this._name);
            }
            else if (this.Table is RasterBand) {
                RasterBand rasterBand = (RasterBand)this.Table;
                field = rasterBand.FindField(this._name);
            }
            if (field == null) { return; }
            field.WriteXml(writer);
        }
        public override void Errors(List<Error> list) {
            // Field Name
            if (string.IsNullOrEmpty(this._name)) {
                list.Add(new ErrorTableRow(this, "Index Field names can not be empty", ErrorType.Error));
            }
            else {
                // Get Parent Index
                Index index = (Index)base.Parent;

                // Get Sibling IndexFields
                List<IndexField> indexFields = index.GetIndexFields();

                // Check for duplicate IndexField names
                foreach (IndexField indexField in indexFields) {
                    if (indexField == this) { continue; }
                    if (indexField.Name == this._name) {
                        // Duplicate IndexField Name found
                        string message = string.Format(
                            "Index Field name '{0}' is duplicated in Index {1}",
                            this._name,
                            index.Name);
                        list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                        break;
                    }
                }

                // Find Field
                Field field = null;
                if (this.Table is ObjectClass) {
                    ObjectClass objectClass = (ObjectClass)this.Table;
                    field = objectClass.FindField(this._name);
                }
                else if (this.Table is RasterBand) {
                    RasterBand rasterBand = (RasterBand)this.Table;
                    field = rasterBand.FindField(this._name);
                }

                // Error if Field not found
                if (field == null) {
                    // Field cannot be found in parent ObjectClass/RasterBand
                    string message = string.Format("Index Field '{0}' is missing from parent dataset", this._name);
                    list.Add(new ErrorTableRow(this, message, ErrorType.Error));
                }
            }
        }
        public override void UpdateText() {
            this.Text = this._name;
        }
    }
}

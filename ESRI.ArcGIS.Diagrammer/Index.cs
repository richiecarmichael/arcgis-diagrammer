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
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using Crainiate.Diagramming;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Field Index
    /// </summary>
    [Serializable]
    public class Index : EsriTableGroup {
        private string _name = string.Empty;
        private bool _isUnique = true;
        private bool _isAscending = true;
        //
        // CONSTRUCTOR
        //
        public Index() : base() {
            // Suspend Events
            this.SuspendEvents = true;

            // Initialize Text
            this.UpdateText();

            // Collapse group
            this.Expanded = false;

            // Resume Events
            this.SuspendEvents = false;
        }
        public Index(IXPathNavigable path): base(path) {
            // Suspend Events
            this.SuspendEvents = true;

            // Get Naviagator
            XPathNavigator navigator = path.CreateNavigator();

            // <Name></Name>
            XPathNavigator navigatorIndexName = navigator.SelectSingleNode("Name");
            if (navigatorIndexName != null) {
                this._name = navigatorIndexName.Value;
            }

            // <IsUnique></IsUnique>
            XPathNavigator navigatorIsUnique = navigator.SelectSingleNode("IsUnique");
            if (navigatorIsUnique != null) {
                this._isUnique = navigatorIsUnique.ValueAsBoolean;
            }

            // <IsAscending></IsAscending>
            XPathNavigator navigatorIsAscending = navigator.SelectSingleNode("IsAscending");
            if (navigatorIsAscending != null) {
                this._isAscending = navigatorIsAscending.ValueAsBoolean;
            }

            // Initialize Text
            this.UpdateText();

            // Collapse group
            this.Expanded = false;

            // Resume Events
            this.SuspendEvents = false;
        }
        public Index(SerializationInfo info, StreamingContext context): base(info, context) {
            this._name = info.GetString("name");
            this._isUnique = info.GetBoolean("isUnique");
            this._isAscending = info.GetBoolean("isAscending");
            this.UpdateText();
        }
        public Index(Index prototype) : base(prototype) {
            this._name = prototype.Name;
            this._isUnique = prototype.IsUnique;
            this._isAscending = prototype.IsAscending;
            this.UpdateText();
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Name of the Index
        /// </summary>
        [Browsable(true)]
        [Category("Index")]
        [DefaultValue(null)]
        [Description("Index Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; this.UpdateText(); }
        }
        /// <summary>
        /// Indicates if the index is unique
        /// </summary>
        [Browsable(true)]
        [Category("Index")]
        [DefaultValue(true)]
        [Description("Indicates if the index is unique")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsUnique {
            get { return this._isUnique; }
            set { this._isUnique = value; this.UpdateText(); }
        }
        /// <summary>
        /// Indicates if the index is based on ascending order
        /// </summary>
        [Browsable(true)]
        [Category("Index")]
        [DefaultValue(true)]
        [Description("Indicates if the index is based on ascending order")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsAscending {
            get { return this._isAscending; }
            set { this._isAscending = value; this.UpdateText(); }
        }
        /// <summary>
        /// Collection of Index Fields
        /// </summary>
        [Browsable(true)]
        [Category("Index")]
        [DefaultValue(null)]
        [Description("Collection of Index Fields")]
        [Editor(typeof(IndexFieldCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems IndexFields {
            get { return this.Rows; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", this._name);
            info.AddValue("isUnique", this._isUnique);
            info.AddValue("isAscending", this._isAscending);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Index(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Index>
            writer.WriteStartElement("Index");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Index");

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </Index>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Index Fields
            List<IndexField> indexFields = this.GetIndexFields();

            // Add Index Field Errors
            foreach (IndexField indexField in indexFields) {
                indexField.Errors(list);
            }

            // Index Name
            if (string.IsNullOrEmpty(this._name)) {
                list.Add(new ErrorTableGroup(this, "Index names cannot be empty", ErrorType.Error));
            }
            else {
                // Get Validor
                Validator validator = WorkspaceValidator.Default.Validator;

                // Validate Index Name
                string message = null;
                if (!validator.ValidateFieldName(this._name, out message)) {
                    list.Add(new ErrorTableGroup(this, message, ErrorType.Error));
                }

                // Check Index Fields
                switch (indexFields.Count) {
                    case 0:
                        list.Add(new ErrorTableGroup(this, "Indexes must contain at least one field", ErrorType.Error));
                        break;
                    case 1:
                        // OK
                        break;
                    default:
                        list.Add(new ErrorTableGroup(this, "File Geodatabases do not support indexes with more than one field", ErrorType.Warning));
                        break;
                }
            }
        }
        public override void UpdateText() {
            this.Text = this._name;
        }
        public List<IndexField> GetIndexFields() {
            List<IndexField> list = new List<IndexField>();
            foreach (IndexField indexField in base.Rows) {
                list.Add(indexField);
            }
            return list;
        }
        public void AddIndexField(IndexField indexField) {
            this.Rows.Add(indexField);
            this.Table.SelectedItem = indexField;
        }
        public void RemoveIndexField(IndexField indexField) {
            // Get Index Field Index
            int i = base.Rows.IndexOf(indexField);
            if (i == -1) { return; }

            // Remove Index Field
            base.Rows.RemoveAt(i);

            // Select Next Coded Value
            if (base.Rows.Count == 0) {
                base.Table.SelectedItem = this;
            }
            else {
                if (i != base.Rows.Count) {
                    base.Table.SelectedItem = base.Rows[i];
                }
                else {
                    base.Table.SelectedItem = base.Rows[base.Rows.Count - 1];
                }
            }
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write base inner Xml
            base.WriteInnerXml(writer);

            // <Name></Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <IsUnique></IsUnique>
            writer.WriteStartElement("IsUnique");
            writer.WriteValue(this._isUnique);
            writer.WriteEndElement();

            // <IsAscending></IsAscending>
            writer.WriteStartElement("IsAscending");
            writer.WriteValue(this._isAscending);
            writer.WriteEndElement();

            // <Fields>
            writer.WriteStartElement("Fields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Fields");

            // <FieldArray>
            writer.WriteStartElement("FieldArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfField");

            // <FieldArray><Field>
            foreach (IndexField indexField in this.Rows) {
                //
                indexField.WriteXml(writer);
            }

            // </FieldArray>
            writer.WriteEndElement();

            // </Fields>
            writer.WriteEndElement();
        }  
    }
}

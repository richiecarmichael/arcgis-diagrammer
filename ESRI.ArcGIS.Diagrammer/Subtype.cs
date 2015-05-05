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
using Crainiate.ERM4.Navigation;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Subtype
    /// </summary>
    [DefaultPropertyAttribute("SubtypeName")]
    [Serializable]
    public class Subtype : EsriTable, IComparable {
        private string _subtypeName = string.Empty;
        private int _subtypeCode = -1;
        private bool _default = false;
        //
        // CONSTRUCTOR
        //
        public Subtype(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <SubtypeName>
            XPathNavigator navigatorSubtypeName = navigator.SelectSingleNode("SubtypeName");
            if (navigatorSubtypeName != null) {
                this._subtypeName = navigatorSubtypeName.Value;
            }

            // <SubtypeCode>
            XPathNavigator navigatorSubtypeCode = navigator.SelectSingleNode("SubtypeCode");
            if (navigatorSubtypeCode != null) {
                this._subtypeCode = navigatorSubtypeCode.ValueAsInt;
            }

            // <FieldInfos><SubtypeFieldInfo>
            XPathNodeIterator interatorSubtypeFieldInfo = navigator.Select("FieldInfos/SubtypeFieldInfo");
            while (interatorSubtypeFieldInfo.MoveNext()) {
                // Create Field
                XPathNavigator navigatorSubtypeFieldInfo = interatorSubtypeFieldInfo.Current;
                SubtypeField subtypeField = new SubtypeField(navigatorSubtypeFieldInfo);

                // Add Field To Group
                this.Rows.Add(subtypeField);
            }

            // Refresh
            this.Refresh();
        }
        public Subtype(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._subtypeName = info.GetString("subtypeName");
            this._subtypeCode = info.GetInt32("subtypeCode");
            this._default = info.GetBoolean("default");

            // Refresh
            this.Refresh();
        }
        public Subtype(Subtype prototype) : base(prototype) {
            this._subtypeName = prototype.SubtypeName;
            this._subtypeCode = prototype.SubtypeCode;
            this._default = prototype.Default;

            // Refresh
            this.Refresh();
        }
        //
        // Properties
        //
        /// <summary>
        /// Subtype Name
        /// </summary>
        [Browsable(true)]
        [Category("Subtype")]
        [DefaultValue(null)]
        [Description("Subtype Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string SubtypeName {
            get { return this._subtypeName; }
            set { this._subtypeName = value; this.Refresh(); }
        }
        /// <summary>
        /// Subtype Code
        /// </summary>
        [Browsable(true)]
        [Category("Subtype")]
        [DefaultValue(null)]
        [Description("Subtype Code")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int SubtypeCode {
            get { return this._subtypeCode; }
            set { this._subtypeCode = value; }
        }
        /// <summary>
        /// True if Default Subtype
        /// </summary>
        [Browsable(true)]
        [Category("Subtype")]
        [DefaultValue(false)]
        [Description("True if Default Subtype")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Default {
            get { return this._default; }
            set { this._default = value; this.Refresh(); }
        }
        /// <summary>
        /// Get Selected Subtype Field
        /// </summary>
        [Browsable(false)]
        [Category("Subtype")]
        [DefaultValue("")]
        [Description("Get Selected Subtype Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public SubtypeField SelectedSubtypeField {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                if (!(tableItem is SubtypeField)) { return null; }

                SubtypeField subtypeField = (SubtypeField)tableItem;
                return subtypeField;
            }
        }
        /// <summary>
        /// List of Subtype Fields
        /// </summary>
        [Browsable(true)]
        [Category("Subtype")]
        [DefaultValue(null)]
        [Description("Collection of Subtype Fields")]
        [Editor(typeof(SubtypeFieldCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems SubtypeFields {
            get { return this.Rows; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("subtypeName", this._subtypeName);
            info.AddValue("subtypeCode", this._subtypeCode);
            info.AddValue("default", this._default);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new Subtype(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <Subtype>
            writer.WriteStartElement("Subtype");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Subtype");

            // Write Inner XML
            this.WriteInnerXml(writer);

            // </Subtype>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Subypes Fields
            List<SubtypeField> subtypeFields = this.GetSubtypeFields();

            // Get Subtype Field Errors
            foreach (SubtypeField subtypeField in subtypeFields) {
                subtypeField.Errors(list);
            }

            // Subtype Name
            if (string.IsNullOrEmpty(this._subtypeName)) {
                // Name cannot be empty
                list.Add(new ErrorTable(this, "Subtype name cannot be empty", ErrorType.Error));
            }
            else {
                if (this._subtypeName.Length > 255){
                    list.Add(new ErrorTable(this, "Subtype name cannot be longer than 255 characters", ErrorType.Error));
                }
            }

            // Get Parent ObjectClass
            ObjectClass objectClass = this.GetParent();

            // Check parent objectclass
            if (objectClass == null){
                // Must have parent ObjectClass
                list.Add(new ErrorTable(this, "Subtype must be connected to Table or FeatureClass", ErrorType.Warning));
            }

            // [WARNING] subtype should have at least one subtype field
            if (subtypeFields.Count == 0){
                string message = string.Format("Subtype '{0}' should have at least one subtype field", this._subtypeName);
                list.Add(new ErrorTable(this, message, ErrorType.Warning));
            }
        }
        public override void Refresh() {
            base.Refresh();
            base.BorderWidth = this._default ? 3 : 1;
            base.Heading = this._subtypeName;
            base.Tooltip = this._subtypeName;
        }
        public void AddSubtypeField(SubtypeField subtypeField) {
            this.Rows.Add(subtypeField);
            this.SelectedItem = subtypeField;
        }
        public void RemoveSubtypeField(SubtypeField subtypeField) {
            // Get Field
            int i = this.Rows.IndexOf(subtypeField);
            if (i == -1) { return; }

            // Remove Field
            this.Rows.RemoveAt(i);

            // Select Next Coded Value
            if (this.Rows.Count == 0) { return; }
            if (i != this.Rows.Count) {
                this.SelectedItem = this.Rows[i];
            }
            else {
                this.SelectedItem = this.Rows[this.Rows.Count - 1];
            }
        }
        public List<SubtypeField> GetSubtypeFields() {
            List<SubtypeField> subtypeFields = new List<SubtypeField>();
            foreach (TableItem tableItem in this.Rows) {
                if (tableItem is SubtypeField) {
                    SubtypeField subtypeField = (SubtypeField)tableItem;
                    subtypeFields.Add(subtypeField);
                }
            }
            return subtypeFields;
        }
        public void SetDefault() {
            // Exit if already default
            if (this._default) { return; }

            // Set Default
            this._default = true;

            // Refresh Graphics
            this.Refresh();

            // Get Parent
            ObjectClass objectClass = this.GetParent();
            if (objectClass == null) { return; }

            // Get Siblings
            List<Subtype> subtypes = objectClass.GetSubtypes();

            // Make Siblings 
            foreach (Subtype subtype in subtypes) {
                if (subtype == this) { continue; }
                subtype.Default = false;
            }
        }
        public ObjectClass GetParent() { 
            // Get Model
            if (base.Container == null) { return null; }
            EsriModel model = (EsriModel)base.Container;

            // Get Parents
            Navigate navigate = model.Navigate;
            navigate.Start = this;
            Elements elementParents = navigate.Parents(1);

            // Objectclass to return
            ObjectClass objectClass = null;

            // Find Parent that are ObjectClasses
            foreach (Element elementParent in elementParents.Values) {
                if (elementParent is ObjectClass) {
                    objectClass = (ObjectClass)elementParent;
                    break;
                }
            }

            // Return parent objectclass
            return objectClass;
        }
        public int CompareTo(object obj) {
            if (!(obj is Subtype)) {
                throw new ArgumentException("object is not a Domain");
            }
            Subtype subtype = (Subtype)obj;
            return this._subtypeName.CompareTo(subtype.SubtypeName);
        }
        public override void RefreshColor() {
            base.GradientColor = ColorSettings.Default.SubtypeColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void Initialize() {
            base.GradientColor = ColorSettings.Default.SubtypeColor;
            base.SubHeading = Resources.TEXT_SUBTYPE;
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write base inner Xml
            base.WriteInnerXml(writer);

            // <SubtypeName></SubtypeName>
            writer.WriteStartElement("SubtypeName");
            writer.WriteValue(this._subtypeName);
            writer.WriteEndElement();

            // <SubtypeCode></SubtypeCode>
            writer.WriteStartElement("SubtypeCode");
            writer.WriteValue(this._subtypeCode);
            writer.WriteEndElement();

            // <FieldInfos>
            writer.WriteStartElement("FieldInfos");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfSubtypeFieldInfo");

            // <SubtypeFieldInfo></SubtypeFieldInfo>
            foreach (SubtypeField row in this.Rows) {
                row.WriteXml(writer);
            }

            // </FieldInfos>
            writer.WriteEndElement();
        }
    }
}

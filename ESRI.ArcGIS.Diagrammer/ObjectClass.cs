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
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI ObjectClass or "Table"
    /// </summary>
    [Serializable]
    public class ObjectClass : Dataset {
        private const string CATEGORY = "ObjectClass";
        private string _oidFieldName = string.Empty;
        protected string _clsid = string.Empty;
        private string _extClsid = string.Empty;
        private string _aliasName = string.Empty;
        private string _modelName = string.Empty;
        private string _globalIDFieldName = string.Empty;
        private string _rasterFieldName = string.Empty;
        private List<Property> _extensionProperties = null;
        private string _subtypeFieldName = string.Empty;
        private List<ControllerMembership> _controllerMemberships = null;
        //
        // CONSTRUCTOR
        //
        public ObjectClass(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <OIDFieldName></OIDFieldName>
            XPathNavigator navigatorOIDFieldName = navigator.SelectSingleNode("OIDFieldName");
            if (navigatorOIDFieldName != null) {
                this._oidFieldName = navigatorOIDFieldName.Value;
            }

            // <CLSID></CLSID>
            XPathNavigator navigatorCLSID = navigator.SelectSingleNode("CLSID");
            if (navigatorCLSID != null) {
                if (!string.IsNullOrEmpty(navigatorCLSID.Value)) {
                    this._clsid = navigatorCLSID.Value;
                }
            }

            // <EXTCLSID></EXTCLSID>
            XPathNavigator navigatorEXTCLSID = navigator.SelectSingleNode("EXTCLSID");
            if (navigatorEXTCLSID != null) {
                if (!string.IsNullOrEmpty(navigatorEXTCLSID.Value)) {
                    this._extClsid = navigatorEXTCLSID.Value;
                }
            }

            // <AliasName></AliasName>
            XPathNavigator navigatorAliasName = navigator.SelectSingleNode("AliasName");
            if (navigatorAliasName != null) {
                this._aliasName = navigatorAliasName.Value;
            }

            // <ModelName></ModelName>
            XPathNavigator navigatorModelName = navigator.SelectSingleNode("ModelName");
            if (navigatorModelName != null) {
                this._modelName = navigatorModelName.Value;
            }

            // <GlobalIDFieldName></GlobalIDFieldName>
            XPathNavigator navigatorGlobalIDFieldName = navigator.SelectSingleNode("GlobalIDFieldName");
            if (navigatorGlobalIDFieldName != null) {
                this._globalIDFieldName = navigatorGlobalIDFieldName.Value;
            }

            // <RasterFieldName></RasterFieldName>
            XPathNavigator navigatorRasterFieldName = navigator.SelectSingleNode("RasterFieldName");
            if (navigatorRasterFieldName != null) {
                this._rasterFieldName = navigatorRasterFieldName.Value;
            }

            // <PropertySetProperty></PropertySetProperty>
            this._extensionProperties = new List<Property>();
            XPathNodeIterator interatorPropertySetProperty = navigator.Select("ExtensionProperties/PropertyArray/PropertySetProperty");
            while (interatorPropertySetProperty.MoveNext()) {
                // Get Property Note
                XPathNavigator navigatorPropertySetProperty = interatorPropertySetProperty.Current;

                // Create Property Object
                Property property = new Property(navigatorPropertySetProperty);

                // Store Property Object in List
                this._extensionProperties.Add(property);
            }

            // <SubtypeFieldName></SubtypeFieldName>
            XPathNavigator navigatorSubtypeFieldName = navigator.SelectSingleNode("SubtypeFieldName");
            if (navigatorSubtypeFieldName != null) {
                this._subtypeFieldName = navigatorSubtypeFieldName.Value;
            }

            // Create ESRI Namespace Manager
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(navigator.NameTable);
            namespaceManager.AddNamespace(Xml._XSI, Xml.XMLSCHEMAINSTANCE);

            // <ControllerMemberships><ControllerMembership></ControllerMembership></ControllerMemberships>
            this._controllerMemberships = new List<ControllerMembership>();
            XPathNodeIterator interatorControllerMembership = navigator.Select("ControllerMemberships/ControllerMembership");
            while (interatorControllerMembership.MoveNext()) {
                // Get ControllerMembership
                XPathNavigator navigatorControllerMembership = interatorControllerMembership.Current;

                // Get Type
                XPathNavigator type = navigatorControllerMembership.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                switch (type.Value) {
                    case Xml._GEOMETRICNETWORKMEMBERSHIP:
                        GeometricNetworkControllerMembership geometricNetworkControllerMembership = new GeometricNetworkControllerMembership(navigatorControllerMembership);
                        this._controllerMemberships.Add(geometricNetworkControllerMembership);
                        break;
                    case Xml._TOPOLOGYMEMBERSHIP:
                        TopologyControllerMembership topologyControllerMembership = new TopologyControllerMembership(navigatorControllerMembership);
                        this._controllerMemberships.Add(topologyControllerMembership);
                        break;
                    case Xml._TERRAINMEMBERSHIP:
                        break;
                    case Xml._NETWORKDATASETMEMBERSHIP:
                        NetworkControllerMembership networkControllerMembership = new NetworkControllerMembership(navigatorControllerMembership);
                        this._controllerMemberships.Add(networkControllerMembership);
                        break;
                }
            }

            // Create Fields Group
            TableGroup tableGroupFields = new TableGroup();
            tableGroupFields.Expanded = true;
            tableGroupFields.Text = "Fields";
            this.Groups.Add(tableGroupFields);

            XPathNodeIterator interatorField = navigator.Select("Fields/FieldArray/Field");
            while (interatorField.MoveNext()) {
                // Create Field
                XPathNavigator navigatorField = interatorField.Current;
                Field field = new Field(navigatorField, this);

                // Add Field To Group
                tableGroupFields.Rows.Add(field);
            }

            // Create Indexes Group
            TableGroup tableGroupIndexes = new TableGroup();
            tableGroupIndexes.Expanded = true;
            tableGroupIndexes.Text = "Indexes";
            this.Groups.Add(tableGroupIndexes);

            XPathNodeIterator interatorIndex = navigator.Select("Indexes/IndexArray/Index");
            while (interatorIndex.MoveNext()) {
                // Add Index
                XPathNavigator navigatorIndex = interatorIndex.Current;
                Index index = new Index(navigatorIndex);
                tableGroupIndexes.Groups.Add(index);
                
                // Add Field Index
                XPathNodeIterator interatorIndexField = navigatorIndex.Select("Fields/FieldArray/Field");
                while (interatorIndexField.MoveNext()) {
                    XPathNavigator navigatorIndexField = interatorIndexField.Current;

                    IndexField indexField = new IndexField(navigatorIndexField);
                    index.Rows.Add(indexField);
                }
            }
        }
        public ObjectClass(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._oidFieldName = info.GetString("oidFieldName");
            this._clsid = info.GetString("clsid");
            this._extClsid = info.GetString("extClsid");
            this._aliasName = info.GetString("aliasName");
            this._modelName = info.GetString("modelName");
            this._globalIDFieldName = info.GetString("globalIDFieldName");
            this._rasterFieldName = info.GetString("rasterFieldName");
            this._extensionProperties = (List<Property>)info.GetValue("extensionProperties", typeof(List<Property>));
            this._subtypeFieldName = info.GetString("subtypeFieldName");
            this._controllerMemberships = (List<ControllerMembership>)info.GetValue("controllerMemberships", typeof(List<ControllerMembership>));
        }
        public ObjectClass(ObjectClass prototype) : base(prototype) {
            this._oidFieldName = prototype.OIDFieldName;
            this._clsid = prototype.CLSID;
            this._extClsid = prototype.EXTCLSID;
            this._aliasName = prototype.AliasName;
            this._modelName = prototype.ModelName;
            this._globalIDFieldName = prototype.GlobalIDFieldName;
            this._rasterFieldName = prototype.RasterFieldName;

            // Add Cloned Properites
            this._extensionProperties = new List<Property>();
            foreach (Property property in prototype.ExtensionProperties) {
                this._extensionProperties.Add((Property)property.Clone());
            }

            this._subtypeFieldName = prototype.SubtypeFieldName;

            // Add Cloned Controller Memberships
            this._controllerMemberships = new List<ControllerMembership>();
            foreach (ControllerMembership controllerMembership in prototype.ControllerMemberships) {
                this._controllerMemberships.Add((ControllerMembership)controllerMembership.Clone());
            }
        }
        //
        // Properties
        //
        /// <summary>
        /// The name of the OID Field
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("The name of the OID Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string OIDFieldName {
            get { return this._oidFieldName; }
            set { this._oidFieldName = value; }
        }
        /// <summary>
        /// Class Id
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(EsriRegistry.CLASS_TABLE)]
        [Description("Class Id")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(GuidConverter))]
        public string CLSID {
            get { return this._clsid; }
            set { this._clsid = value; }
        }
        /// <summary>
        /// Extension Class Id
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("Extension Class Id")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(GuidConverter))]
        public string EXTCLSID {
            get { return this._extClsid; }
            set { this._extClsid = value; }
        }
        /// <summary>
        /// The alias name for the table
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("The alias name for the table")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string AliasName {
            get { return this._aliasName; }
            set { this._aliasName = value; }
        }
        /// <summary>
        /// The model name for the table
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("The model name for the table")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ModelName {
            get { return this._modelName; }
            set { this._modelName = value; }
        }
        /// <summary>
        /// The name of the GlobalID Field
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("The name of the GlobalID Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string GlobalIDFieldName {
            get { return this._globalIDFieldName; }
            set { this._globalIDFieldName = value; }
        }
        /// <summary>
        /// The name of the raster field
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("The name of the raster field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string RasterFieldName {
            get { return this._rasterFieldName; }
            set { this._rasterFieldName = value; }
        }
        /// <summary>
        /// Collection of Properties
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("Collection of Properties")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<Property> ExtensionProperties {
            get { return this._extensionProperties; }
        }
        /// <summary>
        /// The name of the subtype field
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("The name of the subtype field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string SubtypeFieldName {
            get { return this._subtypeFieldName; }
            set { this._subtypeFieldName = value; }
        }
        /// <summary>
        /// Collection of Controller Memberships
        /// </summary>
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("Collection of Controller Memberships")]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<ControllerMembership> ControllerMemberships {
            get { return this._controllerMemberships; }
        }
        /// <summary>
        /// Get Selected Field
        /// </summary>
        [Browsable(false)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("Get Selected Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]        
        public Field SelectedField {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                Field field = tableItem as Field;
                return field;
            }
        }
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("Collection of Fields")]
        [Editor(typeof(FieldCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems Fields {
            get {
                TableGroup group = (TableGroup)this.Groups[0];
                TableItems tableItems = group.Rows;
                return tableItems;
            }
        }
        /// <summary>
        /// Get Selected Index
        /// </summary>
        [Browsable(false)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("Get Selected Index")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public Index SelectedIndex {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                Index index = tableItem as Index;
                return index;
            }
        }
        [Browsable(true)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue(null)]
        [Description("Collection of Indexes")]
        [Editor(typeof(IndexCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public TableItems Indexes {
            get {
                TableGroup group = (TableGroup)this.Groups[1];
                TableItems tableItems = group.Groups;
                return tableItems;
            }
        }
        /// <summary>
        /// Get Selected Index Field
        /// </summary>
        [Browsable(false)]
        [Category(ObjectClass.CATEGORY)]
        [DefaultValue("")]
        [Description("Get Selected Index Field")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public IndexField SelectedIndexField {
            get {
                // Get Selected Item
                TableItem tableItem = this.SelectedItem;
                if (tableItem == null) { return null; }
                IndexField indexField = tableItem as IndexField;
                return indexField;
            }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/OC=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("oidFieldName", this._oidFieldName);
            info.AddValue("clsid", this._clsid);
            info.AddValue("extClsid", this._extClsid);
            info.AddValue("aliasName", this._aliasName);
            info.AddValue("modelName", this._modelName);
            info.AddValue("globalIDFieldName", this._globalIDFieldName);
            info.AddValue("rasterFieldName", this._rasterFieldName);
            info.AddValue("extensionProperties", this._extensionProperties, typeof(List<Property>));
            info.AddValue("subtypeFieldName", this._subtypeFieldName);
            info.AddValue("controllerMemberships", this._controllerMemberships, typeof(List<ControllerMembership>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new ObjectClass(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DETABLE);

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Get Base List of Errors
            base.Errors(list);

            // Add Field Errors
            List<Field> fields = this.GetFields();
            foreach (Field field in fields) {
                field.Errors(list);
            }

            // Check if Field names duplicated. Domain names are not case sensitive.
            Dictionary<string, Field> dictionary = new Dictionary<string, Field>();
            foreach (Field field in fields) {
                if (string.IsNullOrEmpty(field.Name)) { continue; }
                Field f = null;
                if (dictionary.TryGetValue(field.Name.ToUpper(), out f)) {
                    string message1 = string.Format("Field name '{0}' is duplicated", field.Name);
                    string message2 = string.Format("Field name '{0}' is duplicated", f.Name);
                    list.Add(new ErrorTableRow(field, message1, ErrorType.Error));
                    list.Add(new ErrorTableRow(f, message2, ErrorType.Error));
                }
                else {
                    dictionary.Add(field.Name.ToUpper(), field);
                }
            }

            // Get Indexes
            List<Index> indexes = this.GetIndexes();

            // Add Index Errors
            foreach (Index index in indexes) {
                index.Errors(list);
            }

            // Check for duplicate Index Names
            Dictionary<string, Index> dictionary4 = new Dictionary<string, Index>();
            foreach (Index index in indexes) {
                if (string.IsNullOrEmpty(index.Name)) { continue; }
                Index i = null;
                if (dictionary4.TryGetValue(index.Name.ToUpper(), out i)) {
                    string message1 = string.Format("Subtype name '{0}' is duplicated", index.Name);
                    string message2 = string.Format("Subtype name '{0}' is duplicated", i.Name);
                    list.Add(new ErrorTable(this, message1, ErrorType.Error));
                    list.Add(new ErrorTable(this, message2, ErrorType.Error));
                }
                else {
                    dictionary4.Add(index.Name.ToUpper(), index);
                }
            }

            // Add Subtype Errors
            List<Subtype> subtypes = this.GetSubtypes();

            // Check if Subtype names duplicated. Subtype names are not case sensitive.
            Dictionary<string, Subtype> dictionary2 = new Dictionary<string, Subtype>();
            foreach (Subtype subtype in subtypes) {
                if (string.IsNullOrEmpty(subtype.SubtypeName)) { continue; }
                Subtype s = null;
                if (dictionary2.TryGetValue(subtype.SubtypeName.ToUpper(), out s)) {
                    string message1 = string.Format("Subtype name '{0}' is duplicated", subtype.SubtypeName);
                    string message2 = string.Format("Subtype name '{0}' is duplicated", s.SubtypeName);
                    list.Add(new ErrorTable(subtype, message1, ErrorType.Error));
                    list.Add(new ErrorTable(s, message2, ErrorType.Error));
                }
                else {
                    dictionary2.Add(subtype.SubtypeName.ToUpper(), subtype);
                }
            }

            // Check if Subtype codes duplicated
            Dictionary<int, Subtype> dictionary3 = new Dictionary<int, Subtype>();
            foreach (Subtype subtype in subtypes) {
                Subtype s = null;
                if (dictionary3.TryGetValue(subtype.SubtypeCode, out s)) {
                    string message1 = string.Format("Subtype code '{0}' is duplicated", subtype.SubtypeCode.ToString());
                    string message2 = string.Format("Subtype code '{0}' is duplicated", s.SubtypeCode.ToString());
                    list.Add(new ErrorTable(subtype, message1, ErrorType.Error));
                    list.Add(new ErrorTable(s, message2, ErrorType.Error));
                }
                else {
                    dictionary3.Add(subtype.SubtypeCode, subtype);
                }
            }

            // AliasName
            if (!string.IsNullOrEmpty(this._aliasName)) {
                if (this._aliasName.Length > 255) {
                    // Cannot be longer than 255 characters
                    list.Add(new ErrorTable(this, "Alias name cannot be longer than 255 characters", ErrorType.Error));
                }
            }

            // CLSID
            if (this.GetType() == typeof(ObjectClass)){
                if (string.IsNullOrEmpty(this._clsid)) {
                    // Cannot be empty
                    list.Add(new ErrorTable(this, "CLSID cannot be emtpy", ErrorType.Error));
                }
                else {
                    Guid guid = Guid.Empty;
                    try {
                        guid = new Guid(this._clsid);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                    if (guid == Guid.Empty) {
                        list.Add(new ErrorTable(this, "CLSID is not a valid guid {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}", ErrorType.Error));
                    }
                    else {
                        if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_TABLE) {
                            string x = string.Format("CLSID is valid but normally set to '{0}'", Resources.TEXT_TABLE);
                            list.Add(new ErrorTable(this, x, ErrorType.Warning));
                        }
                    }
                }
            }
            
            // EXTCLSID
            if (this.GetType() == typeof(ObjectClass)) {
                if (!string.IsNullOrEmpty(this._extClsid)) {
                    // Must be a valid GUID format "{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}"
                    Guid guid = Guid.Empty;
                    try {
                        guid = new Guid(this._extClsid);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                    if (guid == Guid.Empty) {
                        list.Add(new ErrorTable(this, "EXTCLSID is not a valid guid {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}", ErrorType.Error));
                    }
                }
            }

            // GlobalIDFieldName
            if (!string.IsNullOrEmpty(this._globalIDFieldName)) {
                Field field = this.FindField(this._globalIDFieldName);
                if (field == null) {
                    // GlobalIDFieldName does not exist
                    string message = string.Format("Global ID Field '{0}' does not exist", this._globalIDFieldName);
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
                else {
                    if (field.FieldType != esriFieldType.esriFieldTypeGlobalID) {
                        // Field is not of type "GlobalID"
                        string message = string.Format("Global ID Field '{0}' must be of type '{1}'", this._globalIDFieldName, esriFieldType.esriFieldTypeGlobalID.ToString());
                        list.Add(new ErrorTable(this, message, ErrorType.Error));
                    }
                }
            }

            // ModelName
            if (this._modelName.Length > 255) {
                // ModelName must be less than 255 characters long
                list.Add(new ErrorTable(this, "Model name exceeds maximum length of 255 characters", ErrorType.Error));
            }

            // OIDFieldName
            if (string.IsNullOrEmpty(this._oidFieldName)) {
                // OID Field is Empty
                if (this.GetType() == typeof(RelationshipClass)) {
                    // RelationshipClasses can have an empty OIDFieldName
                }
                else {
                    list.Add(new ErrorTable(this, "OIDFieldName cannot be empty", ErrorType.Error));
                }
            }
            else {
                Field field = this.FindField(this._oidFieldName);
                if (field == null) {
                    // OID Field does not exist
                    string message = string.Format("OIDFieldName '{0}' does not exist", this._oidFieldName);
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
                else {
                    if (field.FieldType != esriFieldType.esriFieldTypeOID) {
                        // OID Field is the correct field type
                        string message = string.Format("OIDFieldName '{0}' is not of type '{1}'", this._oidFieldName, esriFieldType.esriFieldTypeOID.ToString());
                        list.Add(new ErrorTable(this, message, ErrorType.Error));
                    }
                }
            }

            // Can only have one OID in ObjectClass
            int oidFieldCount = 0;
            foreach (Field field in fields) {
                if (field.FieldType == esriFieldType.esriFieldTypeOID) {
                    oidFieldCount++;
                }
            }
            switch (oidFieldCount) {
                case 0:
                    // ObjectClass does not contain any ObjectID Fields
                    if (this.GetType() == typeof(RelationshipClass)) {
                        // RelationshipClasses do not have to specify a OID Field
                    }
                    else {
                        list.Add(new ErrorTable(this, "Tables and FeatureClasses must have one OID Field", ErrorType.Error));
                    }
                    break;
                case 1:
                    // OK
                    break;
                default:
                    // Mulitple ObjectID Fields found
                    list.Add(new ErrorTable(this, "Only one ObjectID Fields is permitted per Table/FeatureClass", ErrorType.Error));
                    break;
            }

            // RasterFieldName
            if (!(string.IsNullOrEmpty(this._rasterFieldName))) {
                Field field = this.FindField(this._rasterFieldName);
                if (field == null) {
                    // Raster Field does not exist
                    string message = string.Format("Raster Field '{0}' does not exist", this._rasterFieldName);
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
                else {
                    if (field.FieldType != esriFieldType.esriFieldTypeRaster) {
                        // Raster Field is the correct field type
                        string message = string.Format("Raster Field '{0}' is not of type '{1}'", this._rasterFieldName, esriFieldType.esriFieldTypeRaster.ToString());
                        list.Add(new ErrorTable(this, message, ErrorType.Error));
                    }
                }
            }

            // ObjectClass can only participate in one Controller (eg GeometricNetwork, Topology, Network and Terrain?)
            switch (this._controllerMemberships.Count) {
                case 0:
                    // OK
                    break;
                case 1:
                    // OK. But must validate.
                    this._controllerMemberships[0].Errors(list, this);
                    break;
                default:
                    list.Add(new ErrorTable(this, "A table or feature class must only have one controller", ErrorType.Error));
                    break;
            }

            // SubtypeFieldName
            if (string.IsNullOrEmpty(this._subtypeFieldName)) {
                if (subtypes.Count > 0){
                    // Empty subtype field name but has one or more linked subtypes
                    string message = string.Format("Table/FeatureClass has no subtype field but has '{0}' subtypes linked", subtypes.Count.ToString());
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
            }
            else{
                Field field = this.FindField(this._subtypeFieldName);
                if (field == null) {
                    // Subtype Field does not exist
                    string message = string.Format("Subtype Field '{0}' does not exist", this._subtypeFieldName);
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
                else {
                    switch (field.FieldType) {
                        case esriFieldType.esriFieldTypeSmallInteger:
                        case esriFieldType.esriFieldTypeInteger:
                            // OK
                            break;
                        default:
                            // Invalid Subtype Field Type
                            string message = string.Format(
                                "Subtype Field '{0}' is not of type '{1}' or '{2}'",
                                this._subtypeFieldName,
                                esriFieldType.esriFieldTypeSmallInteger.ToString(),
                                esriFieldType.esriFieldTypeInteger.ToString());
                            list.Add(new ErrorTable(this, message, ErrorType.Error));
                            break;
                    }
                    if (subtypes.Count == 0) {
                        // Must have at least one Subtype
                        string message = string.Format("Table/FeatureClass has a subtype field but no linked subtypes");
                        list.Add(new ErrorTable(this, message, ErrorType.Error));
                    }
                    else {
                        // Check that only one subtype is the default
                        int defaultSubtypes = 0;
                        foreach (Subtype subtype in subtypes) {
                            if (subtype.Default) {
                                defaultSubtypes++;
                            }
                        }
                        switch (defaultSubtypes) {
                            case 0:
                                string message1 = string.Format("Table/FeatureClass must have a default subtype");
                                list.Add(new ErrorTable(this, message1, ErrorType.Error));
                                break;
                            case 1:
                                // OK
                                break;
                            default:
                                string message2 = string.Format("Table/FeatureClass cannot have more than one default subtype");
                                list.Add(new ErrorTable(this, message2, ErrorType.Error));
                                break;
                        }
                    }
                }
            }
        }
        public Field FindField(string name){
            Field field = null;
            TableGroup groupField = (TableGroup)this.Groups[0];
            foreach (Field fieldTest in groupField.Rows) {
                if (name.ToUpper() == fieldTest.Name.ToUpper()) {
                    field = fieldTest;
                    break;
                }
            }
            return field;
        }
        public Subtype FindSubtype(int code) {
            Subtype subtype = null;
            List<Subtype> subtypes = this.GetSubtypes();
            foreach (Subtype subtypeTest in subtypes) {
                if (subtypeTest.SubtypeCode == code) {
                    subtype = subtypeTest;
                    break;
                }
            }
            return subtype;
        }
        public Subtype FindSubtype(string name) {
            Subtype subtype = null;
            List<Subtype> subtypes = this.GetSubtypes();
            foreach (Subtype subtypeTest in subtypes) {
                if (subtypeTest.SubtypeName == name) {
                    subtype = subtypeTest;
                    break;
                }
            }
            return subtype;
        }
        public List<Field> GetFields() {
            List<Field> fields = new List<Field>();
            TableGroup groupField = (TableGroup)this.Groups[0];
            foreach (Field field in groupField.Rows) {
                fields.Add(field);
            }
            return fields;
        }
        public List<Index> GetIndexes() {
            List<Index> indexes = new List<Index>();
            TableGroup groupIndex = (TableGroup)this.Groups[1];
            foreach (Index index in groupIndex.Rows) {
                indexes.Add(index);
            }
            return indexes;
        }
        public List<Subtype> GetSubtypes() {
            // Create Subtype List
            List<Subtype> subtypes = new List<Subtype>();

            // Get Model
            if (base.Container == null) { return subtypes; }
            EsriModel model = (EsriModel)base.Container;

            // Get Navigator
            Navigate navigate = model.Navigate;
            navigate.Start = this;

            // Get Child Subtypes
            Elements elements = navigate.Children(1);
            foreach (Element element in elements.Values) {
                if (element is Subtype) {
                    Subtype subtype = (Subtype)element;
                    subtypes.Add(subtype);
                }
            }

            // Return Subtypes
            return subtypes;
        }
        public void AddField(Field field) {
            TableGroup groupField = (TableGroup)this.Groups[0];
            groupField.Rows.Add(field);
            this.SelectedItem = field;
        }
        public void AddIndex(Index index) {
            TableGroup groupIndex = (TableGroup)this.Groups[1];
            groupIndex.Groups.Add(index);
            this.SelectedItem = index;
        }
        public void RemoveField(Field field) {
            // Get Field Group
            TableGroup groupField = (TableGroup)this.Groups[0];

            // Get Field
            int i = groupField.Rows.IndexOf(field);
            if (i == -1) { return; }

            // Remove Field
            groupField.Rows.RemoveAt(i);

            // Select Next Coded Value
            if (groupField.Rows.Count == 0) {
                this.SelectedItem = groupField;
            }
            else {
                if (i != groupField.Rows.Count) {
                    this.SelectedItem = groupField.Rows[i];
                }
                else {
                    this.SelectedItem = groupField.Rows[groupField.Rows.Count - 1];
                }
            }
        }
        public void RemoveIndex(Index index) {
            // Get Index Grouo
            TableGroup groupIndex = (TableGroup)this.Groups[1];

            // Get Index
            int i = groupIndex.Groups.IndexOf(index);
            if (i == -1) { return; }

            // Remove Index
            groupIndex.Groups.RemoveAt(i);

            // Select Next Index
            if (groupIndex.Groups.Count == 0) {
                this.SelectedItem = groupIndex;
            }
            else {
                if (i != groupIndex.Groups.Count) {
                    this.SelectedItem = groupIndex.Groups[i];
                }
                else {
                    this.SelectedItem = groupIndex.Groups[groupIndex.Groups.Count - 1];
                }
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.ObjectClassColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void Initialize() {
            this.GradientColor = ColorSettings.Default.ObjectClassColor;
            this.SubHeading = Resources.TEXT_TABLE;
        }
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Inner Xml
            base.WriteInnerXml(writer);

            // Get Model
            SchemaModel model = (SchemaModel)base.Container;

            // <HasOID></HasOID>
            bool hasOID = !(string.IsNullOrEmpty(this._oidFieldName));
            writer.WriteStartElement("HasOID");
            writer.WriteValue(hasOID);
            writer.WriteEndElement();

            // <OIDFieldName></OIDFieldName>
            writer.WriteStartElement("OIDFieldName");
            if (hasOID) {
                writer.WriteValue(this._oidFieldName);
            }
            writer.WriteEndElement();

            // <Fields>
            TableGroup tableGroupField = (TableGroup)this.Groups[0];
            writer.WriteStartElement("Fields");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Fields");

            // <FieldArray>
            writer.WriteStartElement("FieldArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfField");

            // <Field></Field>
            foreach (Field field in tableGroupField.Rows) {
                field.WriteXml(writer);
            }

            // </FieldArray>
            writer.WriteEndElement();

            // </Fields>
            writer.WriteEndElement();

            // <Indexes>
            TableGroup tableGroupIndex = (TableGroup)this.Groups[1];
            writer.WriteStartElement("Indexes");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Indexes");

            // <IndexArray>
            writer.WriteStartElement("IndexArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfIndex");

            // <Index>
            foreach (Index index in tableGroupIndex.Groups) {
                index.WriteXml(writer);
            }

            // </IndexArray>
            writer.WriteEndElement();

            // </Indexes>
            writer.WriteEndElement();

            // <CLSID></CLSID>
            writer.WriteStartElement("CLSID");
            writer.WriteValue(this._clsid);
            writer.WriteEndElement();

            // <EXTCLSID></EXTCLSID>
            writer.WriteStartElement("EXTCLSID");
            if (!string.IsNullOrEmpty(this._extClsid)) {
                writer.WriteValue(this._extClsid);
            }
            writer.WriteEndElement();

            // <RelationshipClassNames>
            writer.WriteStartElement("RelationshipClassNames");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:Names");

            // <Name></Name>
            List<Dataset> datasets = model.GetDatasets(new Type[] { typeof(RelationshipClass) });
            foreach (RelationshipClass relationshipClass in datasets) {
                if (relationshipClass.OriginClassName == this.Name ||
                    relationshipClass.DestinationClassName == this.Name) {
                    // <Name></Name>
                    writer.WriteStartElement("Name");
                    writer.WriteValue(relationshipClass.Name);
                    writer.WriteEndElement();
                }
            }

            // </RelationshipClassNames>
            writer.WriteEndElement();

            // <AliasName></AliasName>
            writer.WriteStartElement("AliasName");
            writer.WriteValue(this._aliasName);
            writer.WriteEndElement();

            // <ModelName></ModelName>
            writer.WriteStartElement("ModelName");
            if (!string.IsNullOrEmpty(this._modelName)) {
                writer.WriteValue(this._modelName);
            }
            writer.WriteEndElement();

            // <HasGlobalID></HasGlobalID>
            bool hasGlobalID = !(string.IsNullOrEmpty(this._globalIDFieldName));
            writer.WriteStartElement("HasGlobalID");
            writer.WriteValue(hasGlobalID);
            writer.WriteEndElement();

            // <GlobalIDFieldName></GlobalIDFieldName>
            writer.WriteStartElement("GlobalIDFieldName");
            if (hasGlobalID) {
                writer.WriteValue(this._globalIDFieldName);
            }
            writer.WriteEndElement();

            // <RasterFieldName></RasterFieldName>
            writer.WriteStartElement("RasterFieldName");
            if (!string.IsNullOrEmpty(this._rasterFieldName)) {
                writer.WriteValue(this._rasterFieldName);
            }
            writer.WriteEndElement();

            // <ExtensionProperties>
            writer.WriteStartElement("ExtensionProperties");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:PropertySet");

            // <PropertyArray>
            writer.WriteStartElement("PropertyArray");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfPropertySetProperty");

            foreach (Property property in this._extensionProperties) {
                // <PropertySetProperty></PropertySetProperty>
                property.WriteXml(writer);
            }

            // </PropertyArray>
            writer.WriteEndElement();

            // </ExtensionProperties>
            writer.WriteEndElement();

            // Check if there are any subtypes
            List<Subtype> subtypes = this.GetSubtypes();

            // Validate Subtypes
            int defaultSubtypeCode = -1;
            foreach (Subtype subtype in subtypes) {
                if (subtype.Default) {
                    defaultSubtypeCode = subtype.SubtypeCode;
                }
            }

            // Write Subtypes
            if (subtypes.Count > 0) {
                // <SubtypeFieldName>
                writer.WriteStartElement("SubtypeFieldName");
                writer.WriteValue(this._subtypeFieldName);
                writer.WriteEndElement();

                // <DefaultSubtypeCode>
                writer.WriteStartElement("DefaultSubtypeCode");
                writer.WriteValue(defaultSubtypeCode);
                writer.WriteEndElement();

                // <Subtypes>
                writer.WriteStartElement("Subtypes");
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfSubtype");

                // <Subtype></Subtype>
                foreach (Subtype subtype in subtypes) {
                    subtype.WriteXml(writer);
                }

                // </Subtypes>
                writer.WriteEndElement();
            }

            // <ControllerMemberships>
            writer.WriteStartElement("ControllerMemberships");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:ArrayOfControllerMembership");

            // <ControllerMembership></ControllerMembership>
            foreach (ControllerMembership controllerMembership in this._controllerMemberships) {
                controllerMembership.WriteXml(writer);
            }

            // </ControllerMemberships>
            writer.WriteEndElement();
        }
        protected override void Rename(string oldName, string newName) {
            base.Rename(oldName, newName);

            // Rename references in Relationships
            List<Dataset> datasets = DiagrammerEnvironment.Default.SchemaModel.GetDatasets(new Type[] { typeof(RelationshipClass) });
            foreach (RelationshipClass relationshipClass in datasets) {
                if (relationshipClass.OriginClassName.ToUpper() == oldName.ToUpper()) {
                    relationshipClass.OriginClassName = newName;
                }
                if (relationshipClass.DestinationClassName.ToUpper() == oldName.ToUpper()) {
                    relationshipClass.DestinationClassName = newName;
                }
            }
        }
    }
}

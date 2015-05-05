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
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

// One to One (non-attributed), One to Manu (non-attributed)
// ----------------------------------------------------------
//<OriginClassKeys xsi:type="esri:ArrayOfRelationshipClassKey">
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>OriginID</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleOriginPrimary</KeyRole> 
//    </RelationshipClassKey>
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>DestID</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleOriginForeign</KeyRole> 
//    </RelationshipClassKey>
//</OriginClassKeys>

// One to One (attributed), One to Manu (attributed), Many to Many (attributed/non-attributed)
// -------------------------------------------------------------------------------------------
// Note: "Test1" and "Test2" are fields in the attributed relationship table
//<OriginClassKeys xsi:type="esri:ArrayOfRelationshipClassKey">
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>OriginID</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleOriginPrimary</KeyRole> 
//    </RelationshipClassKey>
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>Test1</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleOriginForeign</KeyRole> 
//    </RelationshipClassKey>
//</OriginClassKeys>
//<DestinationClassKeys xsi:type="esri:ArrayOfRelationshipClassKey">
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>DestID</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleDestinationPrimary</KeyRole> 
//    </RelationshipClassKey>
//    <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
//        <ObjectKeyName>Test2</ObjectKeyName> 
//        <ClassKeyName /> 
//        <KeyRole>esriRelKeyRoleDestinationForeign</KeyRole> 
//    </RelationshipClassKey>
//</DestinationClassKeys>

// Rules for field values
//-----------------------
// If attributed then "OriginPrimary" is a field from the Origin Table.
// If attributed then "OriginForeign" is a field from the Relationship Table.
// If attributed then "DestinationPrimary" is a field from the Destination Table.
// If attributed then "DestinationForeign" is a field from the Relationship Table.
// If not attributed then "OriginPrimary" is a field from Origin Table.
// If not attributed then "OriginForeign" is a field from Destination Table.
// If not attributed then "DestinationPrimary" is empty.
// If not attributed then "DestinationForeign" is empty.

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Relationship Class
    /// </summary>
    [Serializable]
    public class RelationshipClass : ObjectClass {
        private esriRelCardinality _cardinality = esriRelCardinality.esriRelCardinalityOneToMany;
        private esriRelNotification _notification = esriRelNotification.esriRelNotificationNone;
        private bool _isComposite = false;
        private string _originClassNames = string.Empty;
        private string _destinationClassNames = string.Empty;
        private esriRelKeyType _keyType = esriRelKeyType.esriRelKeyTypeSingle;
        private esriRelClassKey _classKey = esriRelClassKey.esriRelClassKeyUndefined;
        private string _forwardPathLabel = string.Empty;
        private string _backwardPathLabel = string.Empty;
        private bool _isReflexive = false;
        private string _originPrimary = string.Empty;
        private string _originForeign = string.Empty;
        private string _destinationPrimary = string.Empty;
        private string _destinationForeign = string.Empty;
        private List<RelationshipRule> _relationshipRules = null;

        private const string CARDINALITY = "cardinality";
        private const string NOTIFICATION = "notification";
        private const string ISCOMPOSITE = "isComposite";
        private const string ORIGINCLASSNAMES = "originClassNames";
        private const string DESTINATIONCLASSNAMES = "destinationClassNames";
        private const string KEYTYPE = "keyType";
        private const string CLASSKEY = "classKey";
        private const string FORWARDPATHLABEL = "forwardPathLabel";
        private const string BACKWARDPATHLABEL = "backwardPathLabel";
        private const string ISREFLEXIVE = "isReflexive";
        private const string ORIGINPRIMARY = "originPrimary";
        private const string ORIGINFOREIGN = "originForeign";
        private const string DESTINATIONPRIMARY = "destinationPrimary";
        private const string DESTINATIONFOREIGN = "destinationForeign";
        private const string RELATIONSHIPRULES = "relationshipRules";
        //
        // CONSTRUCTOR
        //
        public RelationshipClass(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // Get Model
            SchemaModel model = (SchemaModel)base.Container;

            // <Cardinality></Cardinality>
            XPathNavigator navigatorCardinality = navigator.SelectSingleNode(Xml.CARDINALITY);
            if (navigatorCardinality != null) {
                this._cardinality = (esriRelCardinality)Enum.Parse(typeof(esriRelCardinality), navigatorCardinality.Value, true);
            }

            // <Notification></Notification> 
            XPathNavigator navigatorNotification = navigator.SelectSingleNode(Xml.NOTIFICATION);
            if (navigatorNotification != null) {
                this._notification = (esriRelNotification)Enum.Parse(typeof(esriRelNotification), navigatorNotification.Value, true);
            }

            // <IsComposite>false</IsComposite> 
            XPathNavigator navigatorIsComposite = navigator.SelectSingleNode(Xml.ISCOMPOSITE);
            if (navigatorIsComposite != null) {
                this._isComposite = navigatorIsComposite.ValueAsBoolean;
            }

            // <OriginClassNames></OriginClassNames>
            XPathNavigator navigatorOriginClassNames = navigator.SelectSingleNode(string.Format("{0}/{1}", Xml.ORIGINCLASSNAMES, Xml.NAME)); //  "OriginClassNames/Name");
            if (navigatorOriginClassNames != null) {
                this._originClassNames = navigatorOriginClassNames.Value;
            }

            // <DestinationClassNames></DestinationClassNames>
            XPathNavigator navigatorDestinationClassNames = navigator.SelectSingleNode(string.Format("{0}/{1}", Xml.DESTINATIONCLASSNAMES, Xml.NAME)); //"DestinationClassNames/Name");
            if (navigatorDestinationClassNames != null) {
                this._destinationClassNames = navigatorDestinationClassNames.Value;
            }

            // <KeyType></KeyType> 
            XPathNavigator navigatorKeyType = navigator.SelectSingleNode(Xml.KEYTYPE);
            if (navigatorKeyType != null) {
                this._keyType = (esriRelKeyType)Enum.Parse(typeof(esriRelKeyType), navigatorKeyType.Value, true);
            }

            // <ClassKey></ClassKey> 
            XPathNavigator navigatorClassKey = navigator.SelectSingleNode(Xml.CLASSKEY);
            if (navigatorClassKey != null) {
                this._classKey = (esriRelClassKey)Enum.Parse(typeof(esriRelClassKey), navigatorClassKey.Value, true);
            }

            // <ForwardPathLabel></ForwardPathLabel> 
            XPathNavigator navigatorForwardPathLabel = navigator.SelectSingleNode(Xml.FORWARDPATHLABEL);
            if (navigatorForwardPathLabel != null) {
                this._forwardPathLabel = navigatorForwardPathLabel.Value;
            }

            // <BackwardPathLabel></BackwardPathLabel> 
            XPathNavigator navigatorBackwardPathLabel = navigator.SelectSingleNode(Xml.BACKWARDPATHLABEL);
            if (navigatorBackwardPathLabel != null) {
                this._backwardPathLabel = navigatorBackwardPathLabel.Value;
            }

            // <IsReflexive></IsReflexive>
            XPathNavigator navigatorIsReflexive = navigator.SelectSingleNode(Xml.ISREFLEXIVE);
            if (navigatorIsReflexive != null) {
                this._isReflexive = navigatorIsReflexive.ValueAsBoolean;
            }

            // <OriginClassKeys><RelationshipClassKey></RelationshipClassKey></OriginClassKeys>
            // <DestinationClassKeys><RelationshipClassKey></RelationshipClassKey></DestinationClassKeys>
            string xpath = string.Format("{0}/{2} | {1}/{2}", Xml.ORIGINCLASSKEYS, Xml.DESTINATIONCLASSKEYS, Xml.RELATIONSHIPCLASSKEY);
            XPathNodeIterator interatorClassKeys = navigator.Select(xpath); // "OriginClassKeys/RelationshipClassKey | DestinationClassKeys/RelationshipClassKey");
            while (interatorClassKeys.MoveNext()) {
                // Get <RelationshipClassKey>
                XPathNavigator navigatorClassKeys = interatorClassKeys.Current;

                // Get <KeyRole>
                XPathNavigator navigatorKeyRole = navigatorClassKeys.SelectSingleNode(Xml.KEYROLE);
                if (navigatorKeyRole == null){continue;}
                if (string.IsNullOrEmpty(navigatorKeyRole.Value)){continue;}
                string keyRole = navigatorKeyRole.Value;

                // Get <ObjectKeyName>
                XPathNavigator navigatorObjectKeyName = navigatorClassKeys.SelectSingleNode(Xml.OBJECTKEYNAME);
                if (navigatorObjectKeyName == null){continue;}

                // Set Relationship Keys
                if (keyRole == esriRelKeyRole.esriRelKeyRoleOriginPrimary.ToString()){
                    this._originPrimary = navigatorObjectKeyName.Value;
                }
                else if (keyRole == esriRelKeyRole.esriRelKeyRoleOriginForeign.ToString()){
                    this._originForeign = navigatorObjectKeyName.Value;
                }
                else if (keyRole == esriRelKeyRole.esriRelKeyRoleDestinationPrimary.ToString()){
                    this._destinationPrimary = navigatorObjectKeyName.Value;
                }
                else if (keyRole == esriRelKeyRole.esriRelKeyRoleDestinationForeign.ToString()){
                    this._destinationForeign = navigatorObjectKeyName.Value;
                }
            }

            // <RelationshipRules><RelationshipRule></RelationshipRule></RelationshipRules>
            this._relationshipRules = new List<RelationshipRule>();
            XPathNodeIterator interatorRelationshipRule = navigator.Select(string.Format("{0}/{1}", Xml.RELATIONSHIPRULES, Xml.RELATIONSHIPRULE)); // "RelationshipRules/RelationshipRule");
            while (interatorRelationshipRule.MoveNext()) {
                // Get <RelationshipRule>
                XPathNavigator navigatorRelationshipRule = interatorRelationshipRule.Current;

                // Create Relationship Rule
                RelationshipRule relationshipRule = new RelationshipRule(navigatorRelationshipRule);

                // Add Rule to Collection
                this._relationshipRules.Add(relationshipRule);
            }
        }
        public RelationshipClass(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._cardinality = (esriRelCardinality)Enum.Parse(typeof(esriRelCardinality), info.GetString(RelationshipClass.CARDINALITY), true);
            this._notification = (esriRelNotification)Enum.Parse(typeof(esriRelNotification), info.GetString(RelationshipClass.NOTIFICATION), true);
            this._isComposite = info.GetBoolean(RelationshipClass.ISCOMPOSITE);
            this._originClassNames = info.GetString(RelationshipClass.ORIGINCLASSNAMES);
            this._destinationClassNames = info.GetString(RelationshipClass.DESTINATIONCLASSNAMES);
            this._keyType = (esriRelKeyType)Enum.Parse(typeof(esriRelKeyType), info.GetString(RelationshipClass.KEYTYPE), true);
            this._classKey = (esriRelClassKey)Enum.Parse(typeof(esriRelClassKey), info.GetString(RelationshipClass.CLASSKEY), true);
            this._forwardPathLabel = info.GetString(RelationshipClass.FORWARDPATHLABEL);
            this._backwardPathLabel = info.GetString(RelationshipClass.BACKWARDPATHLABEL);
            this._isReflexive = info.GetBoolean(RelationshipClass.ISREFLEXIVE);
            this._originPrimary = info.GetString(RelationshipClass.ORIGINPRIMARY);
            this._originForeign = info.GetString(RelationshipClass.ORIGINFOREIGN);
            this._destinationPrimary = info.GetString(RelationshipClass.DESTINATIONPRIMARY);
            this._destinationForeign = info.GetString(RelationshipClass.DESTINATIONFOREIGN);
            this._relationshipRules = (List<RelationshipRule>)info.GetValue(RelationshipClass.RELATIONSHIPRULES, typeof(List<RelationshipRule>));
        }
        public RelationshipClass(RelationshipClass prototype) : base(prototype) {
            this._cardinality = prototype.Cardinality;
            this._notification = prototype.Notification;
            this._isComposite = prototype.IsComposite;
            this._originClassNames = prototype.OriginClassName;
            this._destinationClassNames = prototype.DestinationClassName;
            this._keyType = prototype.KeyType;
            this._classKey = prototype.ClassKey;
            this._forwardPathLabel = prototype.ForwardPathLabel;
            this._backwardPathLabel = prototype.BackwardPathLabel;
            this._isReflexive = prototype.IsReflexive;
            this._originPrimary = prototype.OriginPrimary;
            this._originForeign = prototype.OriginForeign;
            this._destinationPrimary = prototype.DestinationPrimary;
            this._destinationForeign = prototype.DestinationForeign;

            // Add Cloned Relationship Rules
            this._relationshipRules = new List<RelationshipRule>();
            foreach (RelationshipRule relationshipRule in prototype.RelationshipRules) {
                this._relationshipRules.Add((RelationshipRule)relationshipRule.Clone());
            }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The cardinality for the relationship class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(esriRelCardinality.esriRelCardinalityOneToMany)]
        [Description("The cardinality for the relationship class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRelCardinality Cardinality {
            get { return this._cardinality; }
            set { this._cardinality = value; }
        }
        /// <summary>
        /// The notification direction for the relationship class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(esriRelNotification.esriRelNotificationNone)]
        [Description("The notification direction for the relationship class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRelNotification Notification {
            get { return this._notification; }
            set { this._notification = value; }
        }
        /// <summary>
        /// Indicates if the relationship class represents a composite relationship in which the origin object class represents the composite object
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(false)]
        [Description("Indicates if the relationship class represents a composite relationship in which the origin object class represents the composite object")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsComposite {
            get { return this._isComposite; }
            set { this._isComposite = value; }
        }
        /// <summary>
        /// The origin object class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The origin object class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public string OriginClassName {
            get { return this._originClassNames; }
            set { this._originClassNames = value; }
        }
        /// <summary>
        /// The destination object class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The destination object class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public string DestinationClassName {
            get { return this._destinationClassNames; }
            set { this._destinationClassNames = value; }
        }
        /// <summary>
        /// Key type for the relationship class (Dual or Single)
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(esriRelKeyType.esriRelKeyTypeSingle)]
        [Description("Key type for the relationship class (Dual or Single)")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRelKeyType KeyType {
            get { return this._keyType; }
            set { this._keyType = value; }
        }
        /// <summary>
        /// Class key used for the relationship class (Undefined, ClassID or Class Code)
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(esriRelClassKey.esriRelClassKeyUndefined)]
        [Description("Class key used for the relationship class (Undefined, ClassID or Class Code)")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriRelClassKey ClassKey {
            get { return this._classKey; }
            set { this._classKey = value; }
        }
        /// <summary>
        /// The forward path label for the relationship class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The forward path label for the relationship class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string ForwardPathLabel {
            get { return this._forwardPathLabel; }
            set { this._forwardPathLabel = value; }
        }
        /// <summary>
        /// The backward path label for the relationship class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The backward path label for the relationship class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string BackwardPathLabel {
            get { return this._backwardPathLabel; }
            set { this._backwardPathLabel = value; }
        }
        /// <summary>
        /// Indicates if origin and destination sets intersect
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(false)]
        [Description("Indicates if origin and destination sets intersect")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool IsReflexive {
            get { return this._isReflexive; }
            set { this._isReflexive = value; }
        }
        /// <summary>
        /// The relationship origin primary Key
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(null)]
        [Description("The relationship origin primary Key")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string OriginPrimary {
            get { return this._originPrimary; }
            set { this._originPrimary = value; }
        }
        /// <summary>
        /// The relationship origin foreign Key
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(null)]
        [Description("The relationship origin foreign Key")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string OriginForeign {
            get { return this._originForeign; }
            set { this._originForeign = value; }
        }
        /// <summary>
        /// The relationship destination primary Key
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The relationship destination primary Key")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string DestinationPrimary {
            get { return this._destinationPrimary; }
            set { this._destinationPrimary = value; }
        }
        /// <summary>
        /// The relationship destination foreign Key
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue("")]
        [Description("The relationship destination foreign Key")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(FieldConverter))]
        public string DestinationForeign {
            get { return this._destinationForeign; }
            set { this._destinationForeign = value; }
        }
        /// <summary>
        /// The relationship rules that apply to this relationship class
        /// </summary>
        [Browsable(true)]
        [Category("Relationship Class")]
        [DefaultValue(null)]
        [Description("The relationship rules that apply to this relationship class")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public List<RelationshipRule> RelationshipRules {
            get { return this._relationshipRules; }
        }
        /// <summary>
        /// The relationship is attributed. This means that the relationship has an intermediated table.
        /// </summary>
        [Browsable(false)]
        public bool IsAttributed {
            get {
                if (this._cardinality == esriRelCardinality.esriRelCardinalityManyToMany) { return true; }
                if (this.GetFields().Count > 0) { return true; }
                return false;
            }
        }
        //
        // PUBLIC METHODS
        //
        public override string GetDatasetPath() {
            return "/RC=" + this.Name;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(RelationshipClass.CARDINALITY, Convert.ToInt32(this._cardinality).ToString());
            info.AddValue(RelationshipClass.NOTIFICATION, Convert.ToInt32(this._notification).ToString());
            info.AddValue(RelationshipClass.ISCOMPOSITE, this._isComposite);
            info.AddValue(RelationshipClass.ORIGINCLASSNAMES, this._originClassNames);
            info.AddValue(RelationshipClass.DESTINATIONCLASSNAMES, this._destinationClassNames);
            info.AddValue(RelationshipClass.KEYTYPE, Convert.ToInt32(this._keyType).ToString());
            info.AddValue(RelationshipClass.CLASSKEY, Convert.ToInt32(this._classKey).ToString());
            info.AddValue(RelationshipClass.FORWARDPATHLABEL, this._forwardPathLabel);
            info.AddValue(RelationshipClass.BACKWARDPATHLABEL, this._backwardPathLabel);
            info.AddValue(RelationshipClass.ISREFLEXIVE, this._isReflexive);
            info.AddValue(RelationshipClass.ORIGINPRIMARY, this._originPrimary);
            info.AddValue(RelationshipClass.ORIGINFOREIGN, this._originForeign);
            info.AddValue(RelationshipClass.DESTINATIONPRIMARY, this._destinationPrimary);
            info.AddValue(RelationshipClass.DESTINATIONFOREIGN, this._destinationForeign);
            info.AddValue(RelationshipClass.RELATIONSHIPRULES, this._relationshipRules, typeof(List<RelationshipRule>));

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new RelationshipClass(this);
        }
        public override void WriteXml(XmlWriter writer) {
            // <DataElement>
            writer.WriteStartElement(Xml.DATAELEMENT);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._DERELATIONSHIPCLASS);

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </DataElement>
            writer.WriteEndElement();
        }
        public override void Errors(List<Error> list) {
            // Write Base Errors
            base.Errors(list);

            // Origin Class Name
            ObjectClass origin = null;
            if (string.IsNullOrEmpty(this._originClassNames)) {
                list.Add(new ErrorTable(this, "The 'OriginClassName' field cannot be empty", ErrorType.Error));
            }
            else {
                origin = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(this._originClassNames);
                if (origin == null) {
                    list.Add(new ErrorTable(this, "The 'OriginClassName' is not a valid table or feature class", ErrorType.Error));
                }
            }

            // Destination Class Name
            ObjectClass destination = null;
            if (string.IsNullOrEmpty(this._destinationClassNames)) {
                list.Add(new ErrorTable(this, "The 'DestinationClassName' field cannot be empty", ErrorType.Error));
            }
            else {
                destination = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(this._destinationClassNames);
                if (destination == null) {
                    list.Add(new ErrorTable(this, "The 'DestinationClassName' is not a valid table or feature class", ErrorType.Error));
                }
            }

            // OriginPrimary
            if (string.IsNullOrEmpty(this._originPrimary)) {
                list.Add(new ErrorTable(this, "The 'OriginPrimary' field cannot be empty", ErrorType.Error));
            }
            else {
                if (origin != null) {
                    Field field = origin.FindField(this._originPrimary);
                    if (field == null) {
                        list.Add(new ErrorTable(this, "The 'OriginPrimary' does not exist in the origin table", ErrorType.Error));
                    }
                }
            }

            // Origin Foreign
            if (string.IsNullOrEmpty(this._originForeign)) {
                list.Add(new ErrorTable(this, "The 'OriginForeign' field cannot be empty", ErrorType.Error));
            }
            else {
                if (this.IsAttributed) {
                    Field field = this.FindField(this._originForeign);
                    if (field == null) {
                        list.Add(new ErrorTable(this, "The 'OriginForeign' does not exist in the relationship table", ErrorType.Error));
                    }
                }
                else {
                    if (destination != null) {
                        Field field = destination.FindField(this._originForeign);
                        if (field == null) {
                            list.Add(new ErrorTable(this, "The 'OriginForeign' does not exist in the destination table", ErrorType.Error));
                        }
                    }
                }
            }

            // Destination Primary
            if (string.IsNullOrEmpty(this._destinationPrimary)) {
                if (this.IsAttributed) {
                    list.Add(new ErrorTable(this, "The 'DestinationPrimary' field cannot be empty in attributed relationships", ErrorType.Error));
                }
                else {
                    // OK
                }
            }
            else {
                if (this.IsAttributed) {
                    if (destination != null) {
                        Field field = destination.FindField(this._destinationPrimary);
                        if (field == null) {
                            list.Add(new ErrorTable(this, "The 'DestinationPrimary' does not exist in the destination table", ErrorType.Error));
                        }
                    }
                }
                else {
                    list.Add(new ErrorTable(this, "The 'DestinationPrimary' must be empty in non-attributed relationships", ErrorType.Error));
                }
            }

            // Destination Foreign
            if (string.IsNullOrEmpty(this._destinationForeign)) {
                if (this.IsAttributed) {
                    list.Add(new ErrorTable(this, "The 'DestinationForeign' field cannot be empty in attributed relationships", ErrorType.Error));
                }
                else {
                    // OK
                }
            }
            else {
                if (this.IsAttributed) {
                    Field field = this.FindField(this._destinationForeign);
                    if (field == null) {
                        list.Add(new ErrorTable(this, "The 'DestinationForeign' does not exist in the relationship table", ErrorType.Error));
                    }
                }
                else {
                    list.Add(new ErrorTable(this, "The 'DestinationForeign' must be empty in non-attributed relationships", ErrorType.Error));
                }
            }
            
            // CLSID
            if (this.IsAttributed) {
                // CLSID must be 'EsriRegistry.CLSID_ATTRIBUTED_RELATIONSHIP' if attributed
                if (string.IsNullOrEmpty(this._clsid)) {
                    list.Add(new ErrorTable(this, string.Format("Attributed relationships must have a CLSID set to '{0}'.", EsriRegistry.CLASS_ATTRIBUTED_RELATIONSHIP), ErrorType.Error));
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
                        if (guid.ToString("B").ToUpper() != EsriRegistry.CLASS_ATTRIBUTED_RELATIONSHIP) {
                            list.Add(new ErrorTable(this, string.Format("Attributed relationships must have a CLSID set to '{0}'.", EsriRegistry.CLASS_ATTRIBUTED_RELATIONSHIP), ErrorType.Error));
                        }
                    }
                }
            }
            else {
                // CLSID must be blank if not attributed
                if (!string.IsNullOrEmpty(this._clsid)) {
                    list.Add(new ErrorTable(this, "Non-attributes relationships must have be empty CLSID", ErrorType.Error));
                }
            }

            // EXTCLSID
            if (!string.IsNullOrEmpty(this.EXTCLSID)) {
                list.Add(new ErrorTable(this, "EXTCLSID is must be empty for relationship classes", ErrorType.Error));
            }
        }
        public override void RefreshColor() {
            this.GradientColor = ColorSettings.Default.RelationshipColor;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Inner Xml
            base.WriteInnerXml(writer);

            // Get Model
            SchemaModel model = (SchemaModel)base.Container;

            // <Cardinality></Cardinality>
            writer.WriteStartElement(Xml.CARDINALITY);
            writer.WriteValue(this._cardinality.ToString());
            writer.WriteEndElement();

            // <Notification></Notification> 
            writer.WriteStartElement(Xml.NOTIFICATION);
            writer.WriteValue(this._notification.ToString());
            writer.WriteEndElement();

            // <IsAttributed></IsAttributed> 
            writer.WriteStartElement(Xml.ISATTRIBUTED);
            writer.WriteValue(this.IsAttributed);
            writer.WriteEndElement();

            // <IsComposite></IsComposite> 
            writer.WriteStartElement(Xml.ISCOMPOSITE);
            writer.WriteValue(this._isComposite);
            writer.WriteEndElement();

            // <OriginClassNames xsi:type="esri:Names">
            writer.WriteStartElement(Xml.ORIGINCLASSNAMES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._NAMES);

            // <Name></Name> 
            writer.WriteStartElement(Xml.NAME);
            writer.WriteValue(this._originClassNames);
            writer.WriteEndElement();

            // </OriginClassNames>
            writer.WriteEndElement();

            // <DestinationClassNames xsi:type="esri:Names">
            writer.WriteStartElement(Xml.DESTINATIONCLASSNAMES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._NAMES);

            // <Name>Fuse</Name>
            writer.WriteStartElement(Xml.NAME);
            writer.WriteValue(this._destinationClassNames);
            writer.WriteEndElement();

            // </DestinationClassNames>
            writer.WriteEndElement();

            // <KeyType></KeyType>
            writer.WriteStartElement(Xml.KEYTYPE);
            writer.WriteValue(this._keyType.ToString());
            writer.WriteEndElement();

            // <ClassKey></ClassKey> 
            writer.WriteStartElement(Xml.CLASSKEY);
            writer.WriteValue(this._classKey.ToString());
            writer.WriteEndElement();

            // <ForwardPathLabel></ForwardPathLabel> 
            writer.WriteStartElement(Xml.FORWARDPATHLABEL);
            writer.WriteValue(this._forwardPathLabel);
            writer.WriteEndElement();

            // <BackwardPathLabel></BackwardPathLabel> 
            writer.WriteStartElement(Xml.BACKWARDPATHLABEL);
            writer.WriteValue(this._backwardPathLabel);
            writer.WriteEndElement();

            // <IsReflexive></IsReflexive> 
            writer.WriteStartElement(Xml.ISREFLEXIVE);
            writer.WriteValue(this._isReflexive);
            writer.WriteEndElement();

            // <OriginClassKeys xsi:type="esri:ArrayOfRelationshipClassKey">
            writer.WriteStartElement(Xml.ORIGINCLASSKEYS);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFRELATIONSHIPCLASSKEY);

            // <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
            writer.WriteStartElement(Xml.RELATIONSHIPCLASSKEY);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._RELATIONSHIPCLASSKEY);

            // <ObjectKeyName></ObjectKeyName> 
            writer.WriteStartElement(Xml.OBJECTKEYNAME);
            writer.WriteValue(this._originPrimary);
            writer.WriteEndElement();

            // <ClassKeyName></ClassKeyName>
            writer.WriteStartElement(Xml.CLASSKEYNAME);
            writer.WriteEndElement();

            // <KeyRole>esriRelKeyRoleOriginPrimary</KeyRole>
            writer.WriteStartElement(Xml.KEYROLE);
            writer.WriteValue(esriRelKeyRole.esriRelKeyRoleOriginPrimary.ToString());
            writer.WriteEndElement();

            // </RelationshipClassKey>
            writer.WriteEndElement();

            // <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
            writer.WriteStartElement(Xml.RELATIONSHIPCLASSKEY);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._RELATIONSHIPCLASSKEY);

            // <ObjectKeyName></ObjectKeyName>
            writer.WriteStartElement(Xml.OBJECTKEYNAME);
            writer.WriteValue(this._originForeign);
            writer.WriteEndElement();

            // <ClassKeyName></ClassKeyName>
            writer.WriteStartElement(Xml.CLASSKEYNAME);
            writer.WriteEndElement();

            // <KeyRole></KeyRole>
            writer.WriteStartElement(Xml.KEYROLE);
            writer.WriteValue(esriRelKeyRole.esriRelKeyRoleOriginForeign.ToString());
            writer.WriteEndElement();

            // </RelationshipClassKey>
            writer.WriteEndElement();

            // </OriginClassKeys>
            writer.WriteEndElement();

            if (this.IsAttributed) {
                // <DestinationClassKeys xsi:type="esri:ArrayOfRelationshipClassKey">
                writer.WriteStartElement(Xml.DESTINATIONCLASSKEYS);
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFRELATIONSHIPCLASSKEY);

                // <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
                writer.WriteStartElement(Xml.RELATIONSHIPCLASSKEY);
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._RELATIONSHIPCLASSKEY);

                // <ObjectKeyName>OBJECTID</ObjectKeyName>
                writer.WriteStartElement(Xml.OBJECTKEYNAME);
                writer.WriteValue(this._destinationPrimary);
                writer.WriteEndElement();

                // <ClassKeyName></ClassKeyName>
                writer.WriteStartElement(Xml.CLASSKEYNAME);
                writer.WriteEndElement();

                // <KeyRole></KeyRole> 
                writer.WriteStartElement(Xml.KEYROLE);
                writer.WriteValue(esriRelKeyRole.esriRelKeyRoleDestinationPrimary.ToString());
                writer.WriteEndElement();

                // </RelationshipClassKey>
                writer.WriteEndElement();

                // <RelationshipClassKey xsi:type="esri:RelationshipClassKey">
                writer.WriteStartElement(Xml.RELATIONSHIPCLASSKEY);
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._RELATIONSHIPCLASSKEY);

                // <ObjectKeyName>DeviceObjectID</ObjectKeyName> 
                writer.WriteStartElement(Xml.OBJECTKEYNAME);
                writer.WriteValue(this._destinationForeign);
                writer.WriteEndElement();

                // <ClassKeyName></ClassKeyName>
                writer.WriteStartElement(Xml.CLASSKEYNAME);
                writer.WriteEndElement();

                // <KeyRole></KeyRole> 
                writer.WriteStartElement(Xml.KEYROLE);
                writer.WriteValue(esriRelKeyRole.esriRelKeyRoleDestinationForeign.ToString());
                writer.WriteEndElement();

                // </RelationshipClassKey>
                writer.WriteEndElement();

                // </DestinationClassKeys>
                writer.WriteEndElement();
            }

            // <RelationshipRules xsi:type="esri:ArrayOfRelationshipRule" /> 
            writer.WriteStartElement(Xml.RELATIONSHIPRULES);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFRELATIONSHIPRULE);

            // Write Relationship Rules (if any)
            foreach (RelationshipRule relationshipRule in this._relationshipRules) {
                // <RelationshipRule></RelationshipRule>
                relationshipRule.WriteXml(writer);
            }

            // </RelationshipRules>
            writer.WriteEndElement();
        }
        protected override void Initialize() {
            this.DrawExpand = true;
            this.GradientColor = ColorSettings.Default.RelationshipColor;
            this.SubHeading = Resources.TEXT_RELATIONSHIP;
        }
    }
}

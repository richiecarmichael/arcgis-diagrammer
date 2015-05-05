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
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    [DefaultPropertyAttribute("Name")]
    public abstract class Domain : EsriTable, IComparable {
        private const string NAME = "name";
        private const string FIELDTYPE = "fieldType";
        private const string MERGEPOLICYTYPE = "mergePolicyType";
        private const string SPLITPOLICYTYPE = "splitPolicyType";
        private const string DESCRIPTION = "description";
        private const string OWNER = "owner";
        //
        private string _name = string.Empty;
        private esriFieldType _fieldType = esriFieldType.esriFieldTypeString;
        private esriMergePolicyType _mergePolicy = esriMergePolicyType.esriMPTDefaultValue;
        private esriSplitPolicyType _splitPolicy = esriSplitPolicyType.esriSPTDefaultValue;
        private string _description = string.Empty;
        private string _owner = string.Empty;
        //
        // CONSTRUCTOR
        // 
        public Domain(IXPathNavigable path) : base(path) {
            // Suspect Events
            this.SuspendEvents = true;

            XPathNavigator navigator = path.CreateNavigator();
            XPathNodeIterator iterator = navigator.SelectChildren(XPathNodeType.Element);

            // <DomainName></DomainName>
            XPathNavigator navigatorDomainName = navigator.SelectSingleNode(Xml.DOMAINNAME);
            if (navigatorDomainName != null) {
                this._name = navigatorDomainName.Value;
            }

            // <FieldType></FieldType>
            XPathNavigator navigatorFieldType = navigator.SelectSingleNode(Xml.FIELDTYPE);
            if (navigatorFieldType != null) {
                this._fieldType = (esriFieldType)Enum.Parse(typeof(esriFieldType), navigatorFieldType.Value, true);
            }

            // <MergePolicy></MergePolicy>
            XPathNavigator navigatorMergePolicy = navigator.SelectSingleNode(Xml.MERGEPOLICY);
            if (navigatorMergePolicy != null) {
                this._mergePolicy = (esriMergePolicyType)Enum.Parse(typeof(esriMergePolicyType), navigatorMergePolicy.Value, true);
            }

            // <SplitPolicy></SplitPolicy>
            XPathNavigator navigatorSplitPolicy = navigator.SelectSingleNode(Xml.SPLITPOLICY);
            if (navigatorSplitPolicy != null) {
                this._splitPolicy = (esriSplitPolicyType)Enum.Parse(typeof(esriSplitPolicyType), navigatorSplitPolicy.Value, true);
            }

            // <Description></Description>
            XPathNavigator navigatorDescription = navigator.SelectSingleNode(Xml.DESCRIPTION);
            if (navigatorDescription != null) {
                this._description = navigatorDescription.Value;
            }

            // <Owner></Owner>
            XPathNavigator navigatorOwner = navigator.SelectSingleNode(Xml.OWNER);
            if (navigatorOwner != null) {
                this._owner = navigatorOwner.Value;
            }

            // Set Element
            this.Refresh();

            // Resume Events
            this.SuspendEvents = false;
        }
        public Domain(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._name = info.GetString(Domain.NAME);
            this._fieldType = (esriFieldType)Enum.Parse(typeof(esriFieldType), info.GetString(Domain.FIELDTYPE), true);
            this._mergePolicy = (esriMergePolicyType)Enum.Parse(typeof(esriMergePolicyType), info.GetString(Domain.MERGEPOLICYTYPE), true);
            this._splitPolicy = (esriSplitPolicyType)Enum.Parse(typeof(esriSplitPolicyType), info.GetString(Domain.SPLITPOLICYTYPE), true);
            this._description = info.GetString(Domain.DESCRIPTION);
            this._owner = info.GetString(Domain.OWNER);
        }
        public Domain(Domain prototype) : base(prototype) {
            this._name = prototype.Name;
            this._fieldType = prototype.FieldType;
            this._mergePolicy = prototype.MergePolicy;
            this._splitPolicy = prototype.SplitPolicy;
            this._description = prototype.Description;
            this._owner = prototype.Owner;
        }
        //
        // Properties
        //
        /// <summary>
        /// Domain Name
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(null)]
        [Description("Domain Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set {
                // Domain names cannot be empty
                if (string.IsNullOrEmpty(value)) {
                    MessageBox.Show(
                        "Domain names cannot be empty",
                        Resources.TEXT_APPLICATION,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                // Check if Domain names duplicated. Domain names are not case sensitive.
                List<Domain> domains = DiagrammerEnvironment.Default.SchemaModel.GetDomains();
                foreach (Domain domain in domains) {
                    if (domain == this) { continue; }
                    if (domain.Name.ToUpper() == value.ToUpper()) {
                        MessageBox.Show(
                            "A domain already exists with this name",
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }
                }

                // RenameDependencies
                this.Rename(this._name, value);

                // Store Name
                this._name = value;

                // Refresh Diagram Element Text
                this.Refresh();
            }
        }
        /// <summary>
        /// Field Type
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(esriFieldType.esriFieldTypeString)]
        [Description("Field Type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriFieldType FieldType {
            get { return this._fieldType; }
            set { this._fieldType = value; }
        }
        /// <summary>
        /// Merge Policy
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(esriMergePolicyType.esriMPTDefaultValue)]
        [Description("Merge Policy")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriMergePolicyType MergePolicy {
            get { return this._mergePolicy; }
            set { this._mergePolicy = value; }
        }
        /// <summary>
        /// Split Policy
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(esriSplitPolicyType.esriSPTDefaultValue)]
        [Description("Split Policy")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriSplitPolicyType SplitPolicy {
            get { return this._splitPolicy; }
            set { this._splitPolicy = value; }
        }
        /// <summary>
        /// Domain description
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(null)]
        [Description("Description")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Description {
            get { return this._description; }
            set { this._description = value; }
        }
        /// <summary>
        /// Domain owner
        /// </summary>
        [Browsable(true)]
        [Category("Domain")]
        [DefaultValue(null)]
        [Description("Owner")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Owner {
            get { return this._owner; }
            set { this._owner = value; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(Domain.NAME, this._name);
            info.AddValue(Domain.FIELDTYPE, this._fieldType.ToString("d"));
            info.AddValue(Domain.MERGEPOLICYTYPE, this._mergePolicy.ToString("d"));
            info.AddValue(Domain.SPLITPOLICYTYPE, this._splitPolicy.ToString("d"));
            info.AddValue(Domain.DESCRIPTION, this._description);
            info.AddValue(Domain.OWNER, this._owner);
            
            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list) {
            // Get Model
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;

            // Domain Name
            if (string.IsNullOrEmpty(this._name)) {
                // Domain name cannot be null or empty
                list.Add(new ErrorTable(this, "Domain name cannot be empty", ErrorType.Error));
            }
            else {
                // Get Validator
                Validator validator = WorkspaceValidator.Default.Validator;

                // Name must not contain invalid characters. Spaces are valid.
                string message = null;
                string name = this._name.Replace(" ", string.Empty);
                if (validator.ContainsInvalidCharacters(name, out message)) {
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }

                // Domain name must be less thab 255 characters.
                if (this._name.Length > 255) {
                    list.Add(new ErrorTable(this, "Domain name is more than 255 characters long", ErrorType.Error));
                }
            }

            // Owner
            if (!string.IsNullOrEmpty(this._owner)) {
                // Owner must be less than 255 characters.
                if (this._owner.Length > 255) {
                    list.Add(new ErrorTable(this, "Owner name is more than 255 characters long", ErrorType.Error));
                }

                // Owner cannot contain spaces.
                if (this._owner.IndexOf(" ") != -1) {
                    list.Add(new ErrorTable(this, "Owner name cannot contain spaces", ErrorType.Error));
                }
            }

            // Description must be less than 255 characters.
            if (!string.IsNullOrEmpty(this._description)) {
                if (this._description.Length > 255) {
                    list.Add(new ErrorTable(this, "Description is more than 255 characters long", ErrorType.Error));
                }
            }

            // Is this domain unused?
            if (!string.IsNullOrEmpty(this._name)) {
                bool used = false;
                 List<ObjectClass> objectClasses = schemaModel.GetObjectClasses();
                foreach (ObjectClass objectClass in objectClasses) {
                    List<Field> fields = objectClass.GetFields();
                    foreach (Field field in fields){
                        if (field.Domain == this._name) {
                            used = true;
                            break;
                        }
                    }
                    List<Subtype> subtypes = objectClass.GetSubtypes();
                    foreach (Subtype subtype in subtypes) {
                        List<SubtypeField> subtypeFields = subtype.GetSubtypeFields();
                        foreach (SubtypeField subtypeField in subtypeFields) {
                            if (subtypeField.DomainName == this._name) {
                                used = true;
                                break;
                            }
                        }
                        if (used) { break; }
                    }
                }
                if (!used) {
                    string message = string.Format("Domain [{0}] is not used by any Table or FeatureClass", this._name);
                    list.Add(new ErrorTable(this, message, ErrorType.Warning));
                }
            }
        }
        public override void Refresh() {
            base.Refresh();
            this.Heading = this._name;
            this.Tooltip = this._name;
        }
        public abstract bool IsValid(string test, out string message);
        public int CompareTo(object obj) {
            if (!(obj is Domain)) {
                throw new ArgumentException("object is not a Domain");
            }
            Domain domain = (Domain)obj;
            return this._name.CompareTo(domain.Name);
        }
        public override string ToString() {
            return this._name;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            //
            base.WriteInnerXml(writer);

            // <MinValue></MinValue>
            writer.WriteStartElement(Xml.DOMAINNAME);
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <FieldType></FieldType>
            writer.WriteStartElement(Xml.FIELDTYPE);
            writer.WriteValue(this._fieldType.ToString());
            writer.WriteEndElement();

            // <MergePolicy></MergePolicy>
            writer.WriteStartElement(Xml.MERGEPOLICY);
            writer.WriteValue(this._mergePolicy.ToString());
            writer.WriteEndElement();

            // <SplitPolicy></SplitPolicy>
            writer.WriteStartElement(Xml.SPLITPOLICY);
            writer.WriteValue(this._splitPolicy.ToString());
            writer.WriteEndElement();

            // <Description></Description>
            writer.WriteStartElement(Xml.DESCRIPTION);
            writer.WriteValue(this._description);
            writer.WriteEndElement();

            // <Owner></Owner>
            writer.WriteStartElement(Xml.OWNER);
            writer.WriteValue(this._owner);
            writer.WriteEndElement();
        }
        protected virtual void Rename(string oldName, string newName) {
            // Rename All References
            List<Dataset> datasets = DiagrammerEnvironment.Default.SchemaModel.GetDatasets(new Type[] { typeof(ObjectClass), typeof(FeatureClass) });
            foreach (ObjectClass objectClass in datasets) {
                foreach (Field field in objectClass.Fields) {
                    if (field.Domain.ToUpper() == oldName.ToUpper()) {
                        field.Domain = newName;
                    }
                }
                List<Subtype> subtypes = objectClass.GetSubtypes();
                foreach (Subtype subtype in subtypes) {
                    foreach (SubtypeField subtypeField in subtype.SubtypeFields) {
                        if (subtypeField.DomainName.ToUpper() == oldName.ToUpper()) {
                            subtypeField.DomainName = newName;
                        }
                    }
                }
            }
        }
    }
}

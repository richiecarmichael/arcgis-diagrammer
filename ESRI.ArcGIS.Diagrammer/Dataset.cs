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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Crainiate.Diagramming;
using Crainiate.ERM4.Navigation;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    [DefaultPropertyAttribute("Name")]
    public abstract class Dataset : EsriTable, IComparable {
        // Serialization Attributes
        private const string NAME_ = "name";
        private const string CHILDRENEXPANDED_ = "childrenExpanded";
        private const string FULLPROPSDEFAULT_ = "fullPropsDefault";
        private const string METADATARETRIEVED_ = "metadataRetrieved";
        private const string METADATA_ = "metadata";
        private const string DATASETTYPE_ = "datasetType";
        private const string DSID_ = "dsid";
        private const string VERSIONED_ = "versioned";
        private const string CANVERSION_ = "canVersion";

        //
        private string _name = string.Empty;
        private bool _childrenExpanded = true;
        private bool _fullPropsDefault = true;
        private bool _metadataRetrieved = false;
        private string _metadata = string.Empty;
        private esriDatasetType _datasetType = esriDatasetType.esriDTAny;
        private int _dsid = -1;
        private bool _versioned = false;
        private bool _canVersion = false;
        //
        // CONSTRUCTOR
        //
        public Dataset(IXPathNavigable path) : base(path) {
            // Suspend
            this.SuspendEvents = true;

            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Name>
            XPathNavigator navigatorName = navigator.SelectSingleNode(Xml.NAME);
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // <ChildrenExpanded>
            XPathNavigator navigatorChildrenExpanded = navigator.SelectSingleNode(Xml.CHILDRENEXPANDED);
            if (navigatorChildrenExpanded != null) {
                this._childrenExpanded = navigatorChildrenExpanded.ValueAsBoolean;
            }

            // <FullPropsRetrieved>
            XPathNavigator navigatorFullPropsRetrieved = navigator.SelectSingleNode(Xml.FULLPROPSRETRIEVED);
            if (navigatorFullPropsRetrieved != null) {
                this._fullPropsDefault = navigatorFullPropsRetrieved.ValueAsBoolean;
            }

            // <MetadataRetrieved>
            XPathNavigator navigatorMetadataRetrieved = navigator.SelectSingleNode(Xml.METADATARETRIEVED);
            if (navigatorMetadataRetrieved != null) {
                this._metadataRetrieved = navigatorMetadataRetrieved.ValueAsBoolean;
            }

            // <Metadata><XmlDoc>
            if (this._metadataRetrieved) {
                XPathNavigator navigatorMetadata = navigator.SelectSingleNode(string.Format("{0}/{1}", Xml.METADATA, Xml.XMLDOC));
                if (navigatorMetadata != null) {
                    this._metadata = navigatorMetadata.Value;
                }
            }

            // <DatasetType>
            XPathNavigator navigatorDatasetType = navigator.SelectSingleNode(Xml.DATASETTYPE);
            if (navigatorDatasetType != null) {
                this._datasetType = (esriDatasetType)Enum.Parse(typeof(esriDatasetType), navigatorDatasetType.Value, true);
            }

            // <DSID>
            if (this._fullPropsDefault) {
                XPathNavigator navigatorDSID = navigator.SelectSingleNode(Xml.DSID);
                if (navigatorDSID != null) {
                    this._dsid = navigatorDSID.ValueAsInt;
                }
            }

            // <Versioned>
            if (this._fullPropsDefault) {
                XPathNavigator navigatorVersioned = navigator.SelectSingleNode(Xml.VERSIONED);
                if (navigatorVersioned != null) {
                    this._versioned = navigatorVersioned.ValueAsBoolean;
                }
            }

            // <CanVersion>
            if (this._fullPropsDefault) {
                XPathNavigator navigatorCanVersion = navigator.SelectSingleNode(Xml.CANVERSION);
                if (navigatorCanVersion != null) {
                    this._canVersion = navigatorCanVersion.ValueAsBoolean;
                }
            }

            // Refresh Dataset
            this.Refresh();

            // Resume
            this.SuspendEvents = false;
        }
        public Dataset(SerializationInfo info, StreamingContext context) : base(info, context) {
            // Suspend
            this.SuspendEvents = true;

            this._name = info.GetString(Dataset.NAME_);
            this._childrenExpanded = info.GetBoolean(Dataset.CHILDRENEXPANDED_);
            this._fullPropsDefault = info.GetBoolean(Dataset.FULLPROPSDEFAULT_);
            this._metadataRetrieved = info.GetBoolean(Dataset.METADATARETRIEVED_);
            this._metadata = info.GetString(Dataset.METADATA_);
            this._datasetType = (esriDatasetType)Enum.Parse(typeof(esriDatasetType), info.GetString(Dataset.DATASETTYPE_), true);
            this._dsid = info.GetInt32(Dataset.DSID_);
            this._versioned = info.GetBoolean(Dataset.VERSIONED_);
            this._canVersion = info.GetBoolean(Dataset.CANVERSION_);

            // Resume
            this.SuspendEvents = false;
        }
        public Dataset(Dataset prototype) : base(prototype) {
            this._name = prototype.Name;
            this._childrenExpanded = prototype.ChildrenExpanded;
            this._fullPropsDefault = prototype.FullPropsDefault;
            this._metadataRetrieved = prototype.MetadataRetrieved;
            this._metadata = prototype.Metadata;
            this._datasetType = prototype.DatasetType;
            this._dsid = prototype.DSID;
            this._versioned = prototype.Versioned;
            this._canVersion = prototype.CanVersion;
        }
        //
        // Properties
        //
        /// <summary>
        /// Dataset Name
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(null)]
        [Description("Dataset Name")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set {
                // Dataset names cannot be empty
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

                // Rename Dependanices
                this.Rename(this._name, value);

                //
                this._name = value;

                // Refresh Diagram Element Text
                this.Refresh();
            }
        }
        /// <summary>
        /// Indicates if the children have been expanded
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(true)]
        [Description("Indicates if the children have been expanded")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ChildrenExpanded {
            get { return this._childrenExpanded; }
            set { this._childrenExpanded = value; }
        }
        /// <summary>
        /// Indicates if full properties have been retrieved
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(true)]
        [Description("Indicates if full properties have been retrieved")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool FullPropsDefault {
            get { return this._fullPropsDefault; }
            set { this._fullPropsDefault = value; }
        }
        /// <summary>
        /// Indicates if the metadata has been retrieved
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(false)]
        [Description("Indicates if the metadata has been retrieved")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool MetadataRetrieved {
            get { return this._metadataRetrieved; }
            set { this._metadataRetrieved = value; }
        }
        /// <summary>
        /// Metadata
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue("")]
        [Description("Metadata")]
        [EditorAttribute(typeof(MetadataEditor), typeof(UITypeEditor))]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverterAttribute(typeof(MetadataConverter))]
        public string Metadata {
            get { return this._metadata; }
            set { this._metadata = value; }
        }
        /// <summary>
        /// Dataset type
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(esriDatasetType.esriDTAny)]
        [Description("Dataset type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(true)]
        public esriDatasetType DatasetType {
            get { return this._datasetType; }
            set { this._datasetType = value; }
        }
        /// <summary>
        /// Dataset Id
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(-1)]
        [Description("Dataset Id")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public int DSID {
            get { return this._dsid; }
            set { this._dsid = value; }
        }
        /// <summary>
        /// Indicates if this dataset is versioned
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(false)]
        [Description("Indicates if this dataset is versioned")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool Versioned {
            get { return this._versioned; }
            set { this._versioned = value; }
        }
        /// <summary>
        /// Indicates if this dataset can be versioned
        /// </summary>
        [Browsable(true)]
        [Category("Dataset")]
        [DefaultValue(false)]
        [Description("Indicates if this dataset can be versioned")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool CanVersion {
            get { return this._canVersion; }
            set { this._canVersion = value; }
        }
        //
        // ABSTRACT METHODS
        //
        public abstract string GetDatasetPath();
        //
        // PUBLIC METHODS
        //
        public string GetCatalogPath() {
            //
            Dataset parent = this.GetParent();
            if (parent == null) {
                string version = string.Empty;
                DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
                SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
                switch (schemaModel.WorkspaceType) {
                    case esriWorkspaceType.esriFileSystemWorkspace:
                    case esriWorkspaceType.esriLocalDatabaseWorkspace:
                        break;
                    case esriWorkspaceType.esriRemoteDatabaseWorkspace:
                        if (string.IsNullOrEmpty(schemaModel.Version)) { break; }
                        version += string.Format("/V={0}", schemaModel.Version);
                        break;
                }
                return version + this.GetDatasetPath();
            }

            //
            return parent.GetCatalogPath() + this.GetDatasetPath();
        }
        public Dataset GetParent() {
            // Get Model
            EsriModel model = (EsriModel)base.Container;

            // Create Navigation
            Navigate navigate = model.Navigate;
            navigate.Start = this;

            // Get Parents
            Elements elements = navigate.Parents(1);
            if (elements.Count == 0) { return null; }

            // Get First Parent
            IEnumerator enumerator = elements.Values.GetEnumerator();
            bool ok = enumerator.MoveNext();
            if (!ok) { return null; }
            Element element = (Element)enumerator.Current;

            // Return Dataset
            if (!(element is Dataset)) { return null; }
            Dataset dataset = (Dataset)element;
            return dataset;
        }
        public virtual List<Dataset> GetChildren() {
            return new List<Dataset>();
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(Dataset.NAME_, this._name);
            info.AddValue(Dataset.CHILDRENEXPANDED_, this._childrenExpanded);
            info.AddValue(Dataset.FULLPROPSDEFAULT_, this._fullPropsDefault);
            info.AddValue(Dataset.METADATARETRIEVED_, this._metadataRetrieved);
            info.AddValue(Dataset.METADATA_, this._metadata);
            info.AddValue(Dataset.DATASETTYPE_, this._datasetType.ToString("d"));
            info.AddValue(Dataset.DSID_, this._dsid);
            info.AddValue(Dataset.VERSIONED_, this._versioned);
            info.AddValue(Dataset.CANVERSION_, this._canVersion);

            base.GetObjectData(info, context);
        }
        public override void WriteXml(XmlWriter writer) {
            this.WriteInnerXml(writer);
        }
        public override void Errors(List<Error> list) {
            // Get Validator
            Validator validator = WorkspaceValidator.Default.Validator;

            // Name cannot be empty
            if (string.IsNullOrEmpty(this._name)) {
                list.Add(new ErrorTable(this, "Name cannot be null or empty", ErrorType.Error));
            }

            // Name must be valid with the target workspace
            if (!string.IsNullOrEmpty(this._name)) {
                string message = null;
                if (!validator.ValidateTableName(this._name, out message)) {
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
            }

            // If Metadata is Empty then MetadataRetrieved must be FALSE
            if (this._metadataRetrieved) {
                if (string.IsNullOrEmpty(this._metadata)) {
                    string message = string.Format("[{0}] has MetadataRetrieved set to True but contains no metadata", this._name);
                    list.Add(new ErrorTable(this, message, ErrorType.Warning));
                }
            }
            else {
                if (!string.IsNullOrEmpty(this._metadata)) {
                    string message = string.Format("Metadata for [{0}] is defined, please change MetadataRetrieved to True.", this._name);
                    list.Add(new ErrorTable(this, message, ErrorType.Warning));
                }
            }

            // If Metadata not Emtpy then must be valid XML Document
            if (!string.IsNullOrEmpty(this._metadata)) {
                StringReader reader = new StringReader(this._metadata);
                XPathDocument path = null;
                try {
                    path = new XPathDocument(reader);
                }
                catch (XmlException xmlException) {
                    string message = string.Format("Dataset [{0}] Metadata is not a valid XML string. {1}.", this._name, xmlException.Message);
                    list.Add(new ErrorTable(this, message, ErrorType.Error));
                }
                finally {
                    reader.Close();
                    reader = null;
                    path = null;
                }
            }
        }
        public override void Refresh() {
            base.Refresh();
            this.Heading = this._name;
            this.Tooltip = this._name;
        }
        public int CompareTo(object obj) {
            if (!(obj is Dataset)) {
                throw new ArgumentException("object is not a Dataset");
            }
            Dataset dataset = (Dataset)obj;
            return this._name.CompareTo(dataset.Name);
        }
        public override string ToString() {
            return this._name;
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Xml
            base.WriteInnerXml(writer);

            // <CatalogPath></CatalogPath>
            writer.WriteStartElement(Xml.CATALOGPATH);
            writer.WriteValue(this.GetCatalogPath());
            writer.WriteEndElement();

            // <Name></Name>
            writer.WriteStartElement(Xml.NAME);
            writer.WriteValue(this._name);
            writer.WriteEndElement();

            // <ChildrenExpanded></ChildrenExpanded>
            if (!this._childrenExpanded) {
                writer.WriteStartElement(Xml.CHILDRENEXPANDED);
                writer.WriteValue(this._childrenExpanded);
                writer.WriteEndElement();
            }

            // <FullPropsRetrieved></FullPropsRetrieved>
            if (!this._fullPropsDefault) {
                writer.WriteStartElement(Xml.FULLPROPSRETRIEVED);
                writer.WriteValue(this._metadataRetrieved);
                writer.WriteEndElement();
            }

            // Metadata
            if (this._metadataRetrieved) {
                // <MetadataRetrieved></MetadataRetrieved>
                writer.WriteStartElement(Xml.METADATARETRIEVED);
                writer.WriteValue(this._metadataRetrieved);
                writer.WriteEndElement();

                // <Metadata>
                writer.WriteStartElement(Xml.METADATA);

                if (string.IsNullOrEmpty(this._metadata)) {
                    writer.WriteAttributeString(Xml._XSI, Xml._NIL, null, Xml._TRUE);
                }
                else {
                    writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._XMLPROPERTYSET);

                    // <XmlDoc><XmlDoc>
                    writer.WriteStartElement(Xml.XMLDOC);
                    writer.WriteValue(this._metadata);
                    writer.WriteEndElement();
                }

                // </Metadata>
                writer.WriteEndElement();
            }

            if (this.GetType() == typeof(FeatureDataset) ||
                this.GetType() == typeof(RasterDataset)) {
                // <Children>
                writer.WriteStartElement(Xml.CHILDREN);
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFDATAELEMENT);

                List<Dataset> datasets = this.GetChildren();

                // <Children><DataElement></DataElement>
                foreach (Dataset dataset in datasets) {
                    dataset.WriteXml(writer);
                }

                // </Children>
                writer.WriteEndElement();
            }

            // <DatasetType></DatasetType>
            writer.WriteStartElement(Xml.DATASETTYPE);
            writer.WriteValue(this._datasetType.ToString());
            writer.WriteEndElement();

            // <DSID></DSID>
            if (this._fullPropsDefault) {
                writer.WriteStartElement(Xml.DSID);
                writer.WriteValue(this._dsid);
                writer.WriteEndElement();
            }

            // <Versioned></Versioned>
            if (this._fullPropsDefault) {
                writer.WriteStartElement(Xml.VERSIONED);
                writer.WriteValue(this._versioned);
                writer.WriteEndElement();
            }

            // <CanVersion></CanVersion>
            if (this._fullPropsDefault) {
                writer.WriteStartElement(Xml.CANVERSION);
                writer.WriteValue(this._canVersion);
                writer.WriteEndElement();
            }
        }
        protected virtual void Rename(string oldName, string newName) { }
    }
}

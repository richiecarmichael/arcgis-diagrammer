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
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Crainiate.Diagramming;
using Crainiate.ERM4.Layouts;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Schema Model
    /// </summary>
    [Serializable]
    public partial class SchemaModel : EsriModel {
        private const string CATEGORY = "Schema Model";
        private esriWorkspaceType _workspaceType = esriWorkspaceType.esriLocalDatabaseWorkspace;
        private string _version = string.Empty;
        private string _document = string.Empty;
        private string _metadata = string.Empty;
        private Element _elementContext = null;
        private DiagramEncoding _encoding = DiagramEncoding.Unicode;
        private bool _indent = false;
        private bool _dirty = false;
        //
        // CONSTRUCTOR
        //
        public SchemaModel() {
            InitializeComponent();

            // Assign Runtime
            this.Runtime = new SchemaRuntime();
            this.ElementDoubleClick += new EventHandler(this.Model_ElementDoubleClick);
            this.AllowDrop = true;

            // Context Menu Text
            this.menuItemDomainCodedValue_AddCodedValue.Text = Resources.TEXT_ADD_CODED_VALUE;
            this.menuItemDomainCodedValue_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
            this.menuItemDomainCodedValue_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemDomainCodedValueItem_Delete.Text = Resources.TEXT_DELETE;
            this.menuItemDomainRange_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
            this.menuItemDomainRange_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemFeatureClass_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemFeatureClass_AddIndex.Text = Resources.TEXT_ADD_INDEX;
            this.menuItemFeatureClass_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemFeatureClass_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemFeatureDataset_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemFeatureDataset_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemField_AddToIndexes.Text = Resources.TEXT_ADD_TO_INDEXES;
            this.menuItemField_Delete.Text = Resources.TEXT_DELETE;
            this.menuItemGeometricNetwork_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
            this.menuItemGeometricNetwork_OpenEdgeEdgeDiagram.Text = Resources.TEXT_OPEN_EDGE_EDGE_DIAGRAM;
            this.menuItemGeometricNetwork_OpenEdgeJunctionDiagram.Text = Resources.TEXT_OPEN_EDGE_JUNCTION_DIAGRAM;
            this.menuItemGeometricNetwork_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemIndex_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemIndex_Delete.Text = Resources.TEXT_DELETE;
            this.menuItemIndexField_Delete.Text = Resources.TEXT_DELETE;
            this.menuItemObjectClass_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemObjectClass_AddIndex.Text = Resources.TEXT_ADD_INDEX;
            this.menuItemObjectClass_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemObjectClass_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemRasterBand_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemRasterBand_AddIndex.Text = Resources.TEXT_ADD_INDEX;
            this.menuItemRasterBand_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemRasterBand_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemRasterCatalog_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemRasterCatalog_AddIndex.Text = Resources.TEXT_ADD_INDEX;
            this.menuItemRasterCatalog_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemRasterCatalog_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemRasterDataset_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemRasterDataset_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemRelationship_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemRelationship_AddIndex.Text = Resources.TEXT_ADD_INDEX;
            this.menuItemRelationship_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
            this.menuItemRelationship_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemRelationship_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemSubtype_AddField.Text = Resources.TEXT_ADD_FIELD;
            this.menuItemSubtype_SetAsDefault.Text = Resources.TEXT_SET_AS_DEFAULT;
            this.menuItemSubtype_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemSubtypeField_Delete.Text = Resources.TEXT_DELETE;
            this.menuItemTopology_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
            this.menuItemTopology_OpenRuleDiagram.Text = Resources.TEXT_OPEN_RULE_DIAGRAM;
            this.menuItemTopology_Validate.Text = Resources.TEXT_VALIDATE;
            this.menuItemTopology_ViewMetadata.Text = Resources.TEXT_VIEW_METADATA;
            this.menuItemTerrain_OpenDiagram.Text = Resources.TEXT_OPEN_DIAGRAM;
        }
        //
        // EVENTS
        //
        public event EventHandler<DiagramEventArgs> DiagramRequest;
        //
        // PROPERTIES
        //
        /// <summary>
        /// Workspace Type
        /// </summary>
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
        [DefaultValue(esriWorkspaceType.esriLocalDatabaseWorkspace)]
        [Description("Workspace Type")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriWorkspaceType WorkspaceType {
            get { return this._workspaceType; }
            set { this._workspaceType = value; }
        }
        /// <summary>
        /// Database Version
        /// </summary>
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
        [DefaultValue("sde.DEFAULT")]
        [Description("Database Version")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Version {
            get { return this._version; }
            set { this._version = value; }
        }
        /// <summary>
        /// Schema Model Title
        /// </summary>
        [Browsable(false)]
        [Description("Diagram Title")]
        public string Title {
            get {
                if (string.IsNullOrEmpty(this._document)) {
                    return Resources.TEXT_UNTITLED;
                }
                string filename = System.IO.Path.GetFileNameWithoutExtension(this._document);
                if (string.IsNullOrEmpty(filename)) {
                    return Resources.TEXT_UNTITLED;
                }
                return filename;
            }
        }
        /// <summary>
        /// Indicates if the diagram has edits
        /// </summary>
        [Browsable(false)]
        [Description("Indicates if the diagram has edits")]
        public bool Dirty {
            get { return this._dirty; }
            set { this._dirty = value; }
        }
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
        [DefaultValue("")]
        [Description("ArcGIS Diagrammer document filename")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string Document {
            get { return this._document; }
            set { this._document = value; }
        }
        /// <summary>
        /// Metadata
        /// </summary>
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
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
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
        [DefaultValue(typeof(DiagramEncoding), "Unicode")]
        [Description("Encoding used when the diagram is saved to XML")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public DiagramEncoding XmlEncoding {
            get { return this._encoding; }
            set { this._encoding = value; }
        }
        [Browsable(true)]
        [Category(SchemaModel.CATEGORY)]
        [DefaultValue(false)]
        [Description("Specifies if indentation should be used when the diagrma is saved to XML")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool XmlIndent {
            get { return this._indent; }
            set { this._indent = value; }
        }
        [Browsable(false)]
        public List<Error> Errors {
            get {
                // Get DiagrammerEnvironment Singleton
                DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;

                // Remove invalid links
                diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs("Finding invalid links"));
                List<Line> invalidLines = new List<Line>();
                foreach (Line line in this.Lines.Values) {
                    if ((line.Start == null) || (line.End == null)) {
                        invalidLines.Add(line);
                        continue;
                    }
                    if (!line.Start.Docked || !line.End.Docked) {
                        invalidLines.Add(line);
                        continue;
                    }
                    if ((line.Start.DockedElement == null) || (line.End.DockedElement == null)) {
                        invalidLines.Add(line);
                        continue;
                    }
                    if (line.Start.DockedElement.Key == line.End.DockedElement.Key) {
                        invalidLines.Add(line);
                        continue;
                    }
                }
                for (int i = 0; i < invalidLines.Count; i++) {
                    string message = string.Format("Removing link {0}", i.ToString());
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    Line line = invalidLines[i];
                    this.Lines.Remove(line.Key);
                }

                // Remove duplicate links
                diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs("Removing overlapping links"));
                Dictionary<string, object> dictionaryLine = new Dictionary<string, object>();
                List<Line> overlappingLines = new List<Line>();
                foreach (Line line in this.Lines.Values) {
                    string key1 = string.Format("{0}|{1}", line.Start.DockedElement.Key, line.End.DockedElement.Key);
                    string key2 = string.Format("{1}|{0}", line.Start.DockedElement.Key, line.End.DockedElement.Key);
                    if (dictionaryLine.ContainsKey(key1) || dictionaryLine.ContainsKey(key2)) {
                        overlappingLines.Add(line);
                        continue;
                    }
                    dictionaryLine.Add(key1, null);
                }
                for (int i = 0; i < overlappingLines.Count; i++) {
                    string message = string.Format("Removing link {0}", i.ToString());
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    Line line = overlappingLines[i];
                    this.Lines.Remove(line.Key);
                }

                // Analyse each table in the diagram.
                List<Error> list = new List<Error>();
                foreach (Element element in this.Shapes.Values) {
                    if (element is EsriTable) {
                        // Get Table
                        EsriTable table = (EsriTable)element;

                        // Display Message
                        string message = string.Format("Scanning <{0}>", table.Heading);
                        diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                        // Get Table Errors
                        table.Errors(list);
                    }
                }

                // Display error is the wrong WorkspaceType is selected
                switch (this._workspaceType) {
                    case esriWorkspaceType.esriFileSystemWorkspace:
                        string message1 = string.Format("Diagrams with a WorkspaceType of [{0}] are not supported", this._workspaceType.ToString());
                        list.Add(new ErrorModel(message1, ErrorType.Error));
                        break;
                    case esriWorkspaceType.esriLocalDatabaseWorkspace:
                        if (!string.IsNullOrEmpty(this._version)) {
                            string message2 = string.Format("Diagrams with a WorkspaceType of [{0}] should have an empty 'Version'", this._workspaceType.ToString());
                            list.Add(new ErrorModel(message2, ErrorType.Error));
                        }
                        break;
                    case esriWorkspaceType.esriRemoteDatabaseWorkspace:
                        if (string.IsNullOrEmpty(this._version)) {
                            string message3 = string.Format("Diagrams with a WorkspaceType of [{0}] must have a version such as '{1}'", this._workspaceType.ToString(), "sde.DEFAULT");
                            list.Add(new ErrorModel(message3, ErrorType.Error));
                        }
                        break;
                    default:
                        break;
                }

                // Check if objectclass names duplicated. Objectclass names are not case sensitive.
                Dictionary<string, Dataset> dict = new Dictionary<string, Dataset>();
                List<Dataset> datasets = this.GetDatasets();
                foreach (Dataset dataset in datasets) {
                    if (dataset is ObjectClass ||
                        dataset is RasterDataset) {
                        if (string.IsNullOrEmpty(dataset.Name)) { continue; }

                        // Display Message
                        string message = string.Format("Checking for duplicate dataset names <{0}>", dataset.Name);
                        diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                        Dataset d = null;
                        if (dict.TryGetValue(dataset.Name.ToUpper(), out d)) {
                            string message1 = string.Format("Dataset name '{0}' is duplicated", dataset.Name);
                            string message2 = string.Format("Dataset name '{0}' is duplicated", d.Name);
                            list.Add(new ErrorTable(dataset, message1, ErrorType.Error));
                            list.Add(new ErrorTable(d, message2, ErrorType.Error));
                        }
                        else {
                            dict.Add(dataset.Name.ToUpper(), dataset);
                        }
                    }
                }

                // Check if dataset ids are duplicated.
                Dictionary<int, Dataset> dict4 = new Dictionary<int, Dataset>();
                List<Dataset> datasets4 = this.GetDatasets(new Type[] { typeof(ObjectClass), typeof(FeatureClass) });
                foreach (Dataset dataset in datasets4) {
                    // Display Message
                    string message = string.Format("Checked for duplicate dataset ids <{0}>", dataset.Name);
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    Dataset d = null;
                    if (dict4.TryGetValue(dataset.DSID, out d)) {
                        string message1 = string.Format("Dataset id '{0}' is duplicated, this may cause problems when adding topology, geometric network or network dataset rule", dataset.DSID.ToString());
                        string message2 = string.Format("Dataset id '{0}' is duplicated, this may cause problems when adding topology, geometric network or network dataset rule", d.DSID.ToString());
                        list.Add(new ErrorTable(dataset, message1, ErrorType.Warning));
                        list.Add(new ErrorTable(d, message2, ErrorType.Warning));
                    }
                    else {
                        dict4.Add(dataset.DSID, dataset);
                    }
                }

                // Check if Domain names duplicated. Domain names are not case sensitive.
                Dictionary<string, Domain> dict2 = new Dictionary<string, Domain>();
                List<Domain> domains2 = this.GetDomains();
                foreach (Domain domain in domains2) {
                    if (string.IsNullOrEmpty(domain.Name)) { continue; }

                    // Display Message
                    string message = string.Format("Checking for duplicated domain names <{0}>", domain.Name);
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    Domain d = null;
                    if (dict2.TryGetValue(domain.Name.ToUpper(), out d)) {
                        string message1 = string.Format("Domain name '{0}' is duplicated", domain.Name);
                        string message2 = string.Format("Domain name '{0}' is duplicated", d.Name);
                        list.Add(new ErrorTable(domain, message1, ErrorType.Error));
                        list.Add(new ErrorTable(d, message2, ErrorType.Error));
                    }
                    else {
                        dict2.Add(domain.Name.ToUpper(), domain);
                    }
                }

                // Check if FeatureDataset names duplicated. FeatureDataset names are not case sensitive.
                Dictionary<string, Dataset> dict3 = new Dictionary<string, Dataset>();
                List<Dataset> datasets3 = this.GetDatasets(new Type[] { typeof(FeatureDataset) });
                foreach (Dataset dataset in datasets3) {
                    if (string.IsNullOrEmpty(dataset.Name)) { continue; }

                    // Display Message
                    string message = string.Format("Checking for duplicate feature dataset names <{0}>", dataset.Name);
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));


                    Dataset d = null;
                    if (dict3.TryGetValue(dataset.Name.ToUpper(), out d)) {
                        string message1 = string.Format("Feature Dataset name '{0}' is duplicated", dataset.Name);
                        string message2 = string.Format("Feature Dataset name '{0}' is duplicated", d.Name);
                        list.Add(new ErrorTable(dataset, message1, ErrorType.Error));
                        list.Add(new ErrorTable(d, message2, ErrorType.Error));
                    }
                    else {
                        dict3.Add(dataset.Name.ToUpper(), dataset);
                    }
                }

                // Display Message
                diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(string.Empty));

                // Return list of errors (if any)
                return list;
            }
        }
        [Browsable(false)]
        public override Bitmap Icon {
            get { return Resources.BITMAP_GEODATABASE; }
        }
        //
        // PUBLIC METHODS
        //
        public override void OpenModel(string filename) {
            // Exit if FileName is invalid
            if (string.IsNullOrEmpty(filename)) { return; }

            // Exit if File does not exist
            if (!File.Exists(filename)) { return; }

            // Open XML Workspace
            XPathDocument document = null;
            try {
                document = new XPathDocument(filename, XmlSpace.None);
            }
            catch (XmlException ex) {
                MessageBox.Show(
                    "The XML file failed to load. Please select 'View > Exceptions' to view a detailed explanation.",
                    Resources.TEXT_APPLICATION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                // Add Exception
                ExceptionDialog.HandleException(ex);
            }
            if (document == null) { return; }

            // Get XPathNavigator (IXPathNavigable::CreateNavigator)
            XPathNavigator navigator = document.CreateNavigator();

            // Get <esri:Workspace>
            if (!navigator.MoveToFirstChild()) {
                MessageBox.Show(
                    "This file is not a valid ESRI xml workspace document",
                    Resources.TEXT_APPLICATION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            // Check Node Name
            if (navigator.Name != "esri:Workspace") {
                MessageBox.Show(
                    "This file is not a valid ESRI xml workspace document",
                    Resources.TEXT_APPLICATION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            // Create Xml Namespace Manager
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(navigator.NameTable);
            namespaceManager.AddNamespace(Xml._XSI, Xml.XMLSCHEMAINSTANCE);

            // Suspend Model
            this.Suspend();
            this.SuspendEvents = true;
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }

            // Select Domains
            XPathNodeIterator iteratorDomain = navigator.Select("WorkspaceDefinition/Domains/Domain");

            // Add Domains
            while (iteratorDomain.MoveNext()) {
                XPathNavigator domain = iteratorDomain.Current;
                XPathNavigator type = domain.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                switch (type.Value) {
                    case Xml._RANGEDOMAIN:
                        DomainRange domainRange = new DomainRange(domain);
                        this.Shapes.Add(this.Shapes.CreateKey(), domainRange);
                        break;
                    case Xml._CODEDVALUEDOMAIN:
                        DomainCodedValue domainCodedValue = new DomainCodedValue(domain);
                        this.Shapes.Add(this.Shapes.CreateKey(), domainCodedValue);
                        break;
                }
            }

            // Select Root DataElements
            XPathNodeIterator iteratorDataElement = navigator.Select("WorkspaceDefinition/DatasetDefinitions/DataElement");
            while (iteratorDataElement.MoveNext()) {
                XPathNavigator dataElement = iteratorDataElement.Current;
                XPathNavigator type = dataElement.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                switch (type.Value) {
                    case Xml._DEFEATUREDATASET:
                        // Create FeatureDataset
                        FeatureDataset featureDataset = new FeatureDataset(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), featureDataset);

                        // Loop for Child DataElements
                        XPathNodeIterator iteratorDataElement2 = dataElement.Select("Children/DataElement");
                        while (iteratorDataElement2.MoveNext()) {
                            XPathNavigator dataElement2 = iteratorDataElement2.Current;
                            XPathNavigator type2 = dataElement2.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                            switch (type2.Value) {
                                case Xml._DEFEATURECLASS:
                                    // Create FeatureClass
                                    FeatureClass featureClass2 = new FeatureClass(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), featureClass2);

                                    // Create Link to FeatureDataset
                                    Link link1 = new Link(featureDataset, featureClass2);
                                    link1.BorderColor = ModelSettings.Default.EnabledLines;
                                    link1.BorderStyle = DashStyle.Solid;
                                    link1.BorderWidth = 1f;
                                    link1.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link1);

                                    // Add Subtypes
                                    this.AddSubtypes(featureClass2, dataElement2);

                                    break;
                                case Xml._DEGEOMETRICNETWORK:
                                    // Create GeometricNetwork
                                    GeometricNetwork geometricNetwork = new GeometricNetwork(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), geometricNetwork);

                                    // Create Link to FeatureDataset
                                    Link link2 = new Link(featureDataset, geometricNetwork);
                                    link2.BorderColor = ModelSettings.Default.EnabledLines;
                                    link2.BorderStyle = DashStyle.Solid;
                                    link2.BorderWidth = 1f;
                                    link2.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link2);

                                    break;
                                case Xml._DERELATIONSHIPCLASS:
                                    // Create Relationship
                                    RelationshipClass relationshipClass = new RelationshipClass(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), relationshipClass);

                                    // Create Link to FeatureDataset
                                    Link link3 = new Link(featureDataset, relationshipClass);
                                    link3.BorderColor = ModelSettings.Default.EnabledLines;
                                    link3.BorderStyle = DashStyle.Solid;
                                    link3.BorderWidth = 1f;
                                    link3.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link3);

                                    break;
                                case Xml._DETOPOLOGY:
                                    // Create Topology
                                    Topology topology = new Topology(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), topology);

                                    // Create Link to FeatureDataset
                                    Link link4 = new Link(featureDataset, topology);
                                    link4.BorderColor = ModelSettings.Default.EnabledLines;
                                    link4.BorderStyle = DashStyle.Solid;
                                    link4.BorderWidth = 1f;
                                    link4.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link4);

                                    break;
                                case Xml._DENETWORKDATASET:
                                    // Create Network Dataset
                                    Network network = new Network(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), network);

                                    // Create Link to FeatureDataset
                                    Link link5 = new Link(featureDataset, network);
                                    link5.BorderColor = ModelSettings.Default.EnabledLines;
                                    link5.BorderStyle = DashStyle.Solid;
                                    link5.BorderWidth = 1f;
                                    link5.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link5);

                                    break;
                                case Xml._DETERRAIN:
                                    // Create Network Dataset
                                    Terrain terrain = new Terrain(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), terrain);

                                    // Create Link to FeatureDataset
                                    Link link6 = new Link(featureDataset, terrain);
                                    link6.BorderColor = ModelSettings.Default.EnabledLines;
                                    link6.BorderStyle = DashStyle.Solid;
                                    link6.BorderWidth = 1f;
                                    link6.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link6);

                                    break;
                            }
                        }

                        break;
                    case Xml._DEFEATURECLASS:
                        // Create FeatureClass
                        FeatureClass featureClass = new FeatureClass(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), featureClass);

                        // Add Subtypes
                        this.AddSubtypes(featureClass, dataElement);

                        break;
                    case Xml._DETABLE:
                        // Create Table
                        ObjectClass objectClass = new ObjectClass(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), objectClass);

                        // Add Subtypes
                        this.AddSubtypes(objectClass, dataElement);

                        break;
                    case Xml._DERASTERCATALOG:
                        // Create Table
                        RasterCatalog rasterCatalog = new RasterCatalog(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), rasterCatalog);

                        break;
                    case Xml._DERELATIONSHIPCLASS:
                        // Create Relationship
                        RelationshipClass relationshipClass2 = new RelationshipClass(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), relationshipClass2);

                        break;
                    case Xml._DERASTERDATASET:
                        // Create RasterDataset
                        RasterDataset rasterDataset = new RasterDataset(dataElement);
                        this.Shapes.Add(this.Shapes.CreateKey(), rasterDataset);

                        // Loop for Child DataElements
                        XPathNodeIterator iteratorDataElement3 = dataElement.Select("Children/DataElement");
                        while (iteratorDataElement3.MoveNext()) {
                            XPathNavigator dataElement2 = iteratorDataElement3.Current;
                            XPathNavigator type2 = dataElement2.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                            switch (type2.Value) {
                                case Xml._DERASTERBAND:
                                    // Create FeatureClass
                                    RasterBand rasterBand = new RasterBand(dataElement2);
                                    this.Shapes.Add(this.Shapes.CreateKey(), rasterBand);

                                    // Create Link to FeatureDataset
                                    Link link = new Link(rasterDataset, rasterBand);
                                    link.BorderColor = ModelSettings.Default.EnabledLines;
                                    link.BorderStyle = DashStyle.Solid;
                                    link.BorderWidth = 1f;
                                    link.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                                    this.Lines.Add(this.Lines.CreateKey(), link);

                                    break;
                            }
                        }

                        break;
                }
            }

            // <esri:Workspace><WorkspaceDefinition><Metadata><XmlDoc>
            XPathNavigator navigatorMetadata = navigator.SelectSingleNode("WorkspaceDefinition/Metadata/XmlDoc");
            if (navigatorMetadata != null) {
                this._metadata = navigatorMetadata.Value;
            }

            // <WorkspaceType>
            XPathNavigator navigatorWorkspaceType = navigator.SelectSingleNode("WorkspaceDefinition/WorkspaceType");
            if (navigatorWorkspaceType != null) {
                this._workspaceType = (esriWorkspaceType)Enum.Parse(typeof(esriWorkspaceType), navigatorWorkspaceType.Value, true);
            }

            // <Version>
            XPathNavigator navigatorVersion = navigator.SelectSingleNode("WorkspaceDefinition/Version");
            if (navigatorVersion != null) {
                this._version = navigatorVersion.Value;
            }

            // Close XML Document
            document = null;

            // Perform Layout
            this.ExecuteLayout(typeof(HierarchicalLayout), true);

            // Resume and Refresh Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Resume();
            }
            this.SuspendEvents = false;
            this.Resume();
            this.Refresh();

            // Make Dirty
            this._dirty = true;
        }
        public void SaveModel() {
            if (string.IsNullOrEmpty(this._document)) { return; }
            this.SaveModel(this._document);
        }
        public void SaveModel(string document) {
            // Suspend Model
            this.Suspend();
            this.SuspendEvents = true;
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            
            // Unselect all TableItems
            foreach (Element element in this.Shapes.Values) {
                if (element is EsriTable) {
                    EsriTable table = (EsriTable)element;
                    table.SelectedItem = null;
                }
            }

            // Resume and Refresh Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Resume();
            }
            this.SuspendEvents = false;
            this.Resume();
            this.Refresh();

            // Save Model
            this.Save(document, SaveFormat.Binary);
            this._document = document;
            this._dirty = false;
        }
        public void PublishModel(string document) {
            // Check Filename
            if (string.IsNullOrEmpty(document)) { return; }

            // Get Diagrammer Environment Singleton
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;

            // Specific XML Settings
            XmlWriterSettings settings = new XmlWriterSettings();
            switch (this._encoding) {
                case DiagramEncoding.ASCII:
                    settings.Encoding = Encoding.ASCII;
                    break;
                case DiagramEncoding.BigEndianUnicode:
                    settings.Encoding = Encoding.BigEndianUnicode;
                    break;
                case DiagramEncoding.Default:
                    settings.Encoding = Encoding.Default;
                    break;
                case DiagramEncoding.Unicode:
                    settings.Encoding = Encoding.Unicode;
                    break;
                case DiagramEncoding.UTF32:
                    settings.Encoding = Encoding.UTF32;
                    break;
                case DiagramEncoding.UTF7:
                    settings.Encoding = Encoding.UTF7;
                    break;
                case DiagramEncoding.UTF8:
                    settings.Encoding = Encoding.UTF8;
                    break;
                default:
                    settings.Encoding = Encoding.Default;
                    break;
            }
            settings.Indent = this._indent;
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = false;

            // Create the XmlWriter object and write some content.
            XmlWriter writer = XmlWriter.Create(document, settings);

            // <esri:Workspace>
            writer.WriteStartElement(Xml._ESRI, Xml.WORKSPACE, Xml.ESRISCHEME92);
            writer.WriteAttributeString(Xml._XMLNS, Xml._ESRI, null, Xml.ESRISCHEME92);
            writer.WriteAttributeString(Xml._XMLNS, Xml._XSI, null, Xml.XMLSCHEMAINSTANCE);
            writer.WriteAttributeString(Xml._XMLNS, Xml._XS, null, Xml.XMLSCHEMA);

            // <esri:Workspace><WorkspaceDefinition>
            writer.WriteStartElement(Xml.WORKSPACEDEFINITION);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._WORKSPACEDEFINITION);

            // <esri:Workspace><WorkspaceDefinition><WorkspaceType>
            writer.WriteStartElement(Xml.WORKSPACETYPE);
            writer.WriteValue(this._workspaceType.ToString());
            writer.WriteEndElement();

            // <esri:Workspace><WorkspaceDefinition><Version>
            writer.WriteStartElement(Xml.VERSION);
            writer.WriteValue(this._version);
            writer.WriteEndElement();

            // <esri:Workspace><WorkspaceDefinition><Domains>
            writer.WriteStartElement("Domains");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFDOMAIN);

            // <esri:Workspace><WorkspaceDefinition><Domains><Domain>
            foreach (Element element in this.Shapes.Values) {
                // Export Domains
                if (element is Domain) {
                    // Get Domain
                    Domain domain = (Domain)element;

                    // Display Message
                    string message = string.Format("Saving Domain <{0}>", domain.Name);
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    // Write Domain
                    domain.WriteXml(writer);
                }
            }

            // <esri:Workspace><WorkspaceDefinition></Domains>
            writer.WriteEndElement();

            // <esri:Workspace><WorkspaceDefinition><DatasetDefinitions>
            writer.WriteStartElement(Xml.DATASETDEFINITIONS);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._ARRAYOFDATAELEMENT);

            // Export FeatureDataset (and Child Objects)
            // <esri:Workspace><WorkspaceDefinition><DatasetDefinitions><DataElement>
            foreach (Element element in this.Shapes.Values) {
                if (element.GetType() == typeof(FeatureDataset)) {
                    // Get FeatureDataset
                    FeatureDataset featureDataset = (FeatureDataset)element;

                    // Display Message
                    string message = string.Format("Saving FeatureDataset <{0}>", featureDataset.Name);
                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                    // Write FeatureDataset
                    featureDataset.WriteXml(writer);
                }
            }

            // Export Stand Alone Objects
            // <esri:Workspace><WorkspaceDefinition><DatasetDefinitions><DataElement>
            foreach (Element element in this.Shapes.Values) {
                if (element.GetType() == typeof(FeatureClass)) {
                    FeatureClass featureClass = (FeatureClass)element;
                    Dataset dataset = featureClass.GetParent();
                    if (dataset == null) {
                        featureClass.WriteXml(writer);
                    }
                }
                else if (element.GetType() == typeof(ObjectClass)) {
                    ObjectClass objectClass = (ObjectClass)element;
                    objectClass.WriteXml(writer);
                }
                else if (element.GetType() == typeof(RasterCatalog)) {
                    RasterCatalog rasterCatalog = (RasterCatalog)element;
                    rasterCatalog.WriteXml(writer);
                }
                else if (element.GetType() == typeof(RasterDataset)) {
                    RasterDataset rasterDataset = (RasterDataset)element;
                    rasterDataset.WriteXml(writer);
                }
                else if (element.GetType() == typeof(RelationshipClass)) {
                    RelationshipClass relationshipClass = (RelationshipClass)element;
                    Dataset dataset = relationshipClass.GetParent();
                    if (dataset == null) {
                        relationshipClass.WriteXml(writer);
                    }
                }
            }

            // <esri:Workspace><WorkspaceDefinition></DatasetDefinitions>
            writer.WriteEndElement();

            // <esri:Workspace><WorkspaceDefinition><Metadata><XmlDoc>
            if (!string.IsNullOrEmpty(this._metadata)) {
                writer.WriteStartElement(Xml.METADATA);
                writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._XMLPROPERTYSET);
                writer.WriteStartElement(Xml.XMLDOC);
                writer.WriteValue(this._metadata);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            // <esri:Workspace></WorkspaceDefinition>
            writer.WriteEndElement();

            // <esri:Workspace><WorkspaceData>
            writer.WriteStartElement(Xml.WORKSPACEDATA);
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, Xml._WORKSPACEDATA);
            writer.WriteEndElement();

            // </esri:Workspace>
            writer.WriteEndElement();

            // Close Writer
            writer.Flush();
            writer.Close();

            // Display Message
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(string.Empty));
        }
        public Domain FindDomain(string name) {
            Domain domain = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is Domain) {
                    Domain domainTest = (Domain)element;
                    if (domainTest.Name.ToUpper() == name.ToUpper()) {
                        domain = domainTest;
                        break;
                    }
                }
            }
            return domain;
        }
        public GeometricNetwork FindGeometricNetwork(string name) {
            GeometricNetwork geometricNetwork = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is GeometricNetwork) {
                    GeometricNetwork geometricNetworkTest = (GeometricNetwork)element;
                    if (geometricNetworkTest.Name.ToUpper() == name.ToUpper()) {
                        geometricNetwork = geometricNetworkTest;
                        break;
                    }
                }
            }
            return geometricNetwork;
        }
        public Topology FindTopology(string name) {
            Topology topology = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is Topology) {
                    Topology topologyTest = (Topology)element;
                    if (topologyTest.Name.ToUpper() == name.ToUpper()) {
                        topology = topologyTest;
                        break;
                    }
                }
            }
            return topology;
        }
        public Network FindNetwork(string name) {
            Network network = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is Network) {
                    Network networkTest = (Network)element;
                    if (networkTest.Name.ToUpper() == name.ToUpper()) {
                        network = networkTest;
                        break;
                    }
                }
            }
            return network;
        }
        public List<ObjectClass> GetObjectClasses() {
            List<ObjectClass> objectClasses = new List<ObjectClass>();
            foreach (Element element in this.Shapes.Values) {
                if (element is ObjectClass) {
                    ObjectClass objectClass = (ObjectClass)element;
                    objectClasses.Add(objectClass);
                }
            }
            return objectClasses;
        }
        public List<Domain> GetDomains() {
            List<Domain> domains = new List<Domain>();
            foreach (Element element in this.Shapes.Values) {
                if (element is Domain) {
                    Domain domain = (Domain)element;
                    domains.Add(domain);
                }
            }
            return domains;
        }
        public List<Dataset> GetDatasets() {
            return this.GetDatasets(Type.EmptyTypes);
        }
        public List<Dataset> GetDatasets(Type[] types) {
            List<Dataset> datasets = new List<Dataset>();
            foreach (Element element in this.Shapes.Values) {
                if (element is Dataset) {
                    Dataset dataset = (Dataset)element;
                    if (types == Type.EmptyTypes) {
                        datasets.Add(dataset);
                    }
                    else {
                        foreach (Type type in types) {
                            if (dataset.GetType() == type) {
                                datasets.Add(dataset);
                                break;
                            }
                        }

                    }
                }
            }
            return datasets;
        }
        public ObjectClass FindObjectClass(int id) {
            ObjectClass objectClass = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is ObjectClass) {
                    ObjectClass objectClassTest = (ObjectClass)element;
                    if (objectClassTest.DSID == id) {
                        objectClass = objectClassTest;
                        break;
                    }
                }
            }
            return objectClass;
        }
        public ObjectClass FindObjectClass(string name) {
            ObjectClass objectClass = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is ObjectClass) {
                    ObjectClass objectClassTest = (ObjectClass)element;
                    if (objectClassTest.Name == name) {
                        objectClass = objectClassTest;
                        break;
                    }
                }
            }
            return objectClass;
        }
        public Subtype FindSubtype(int id, int code) {
            Subtype subtype = null;
            ObjectClass objectClass = this.FindObjectClass(id);
            if (objectClass != null) {
                subtype = objectClass.FindSubtype(code);
            }
            return subtype;
        }
        public Subtype FindSubtype(string name, string subtypeName) {
            Subtype subtype = null;
            ObjectClass objectClass = this.FindObjectClass(name);
            if (objectClass != null) {
                subtype = objectClass.FindSubtype(subtypeName);
            }
            return subtype;
        }
        public EsriTable FindObjectClassOrSubtype(int id, int code) {
            // Get ObjectClass
            ObjectClass objectClass = this.FindObjectClass(id);
            if (objectClass == null) { return null; }

            // Get Subtype (if any)
            Subtype subtype = objectClass.FindSubtype(code);
            if (subtype == null) {
                if (code == 0) {
                    return objectClass;
                }
                else {
                    return null;
                }
            }

            return subtype;
        }
        public ObjectClass FindParent(ControllerMembership controller) {
            ObjectClass objectClass = null;
            foreach (Element element in this.Shapes.Values) {
                if (element is ObjectClass) {
                    ObjectClass objectClassTest = (ObjectClass)element;
                    if (objectClassTest.ControllerMemberships.Contains(controller)) {
                        objectClass = objectClassTest;
                        break;
                    }
                }
            }
            return objectClass;
        }
        public Terrain FindParent(TerrainDataSource terrainDataSource) {
            foreach (Element element in this.Shapes.Values) {
                if (element is Terrain) {
                    Terrain terrain = (Terrain)element;
                    if (terrain.TerrainDataSources.Contains(terrainDataSource)) {
                        return terrain;
                    }
                }
            }
            return null;
        }
        //
        // PROTECTED METHODS
        //
        protected override void OnSerialize(IFormatter formatter, SurrogateSelector selector) {
            selector.AddSurrogate(typeof(SchemaModel), new StreamingContext(StreamingContextStates.All), new SchemaModelSerialize());
            base.OnSerialize(formatter, selector);
        }
        protected override void OnDeserialize(IFormatter formatter, SurrogateSelector selector) {
            SchemaModelSerialize surrogate = new SchemaModelSerialize();
            selector.AddSurrogate(typeof(SchemaModel), new StreamingContext(StreamingContextStates.All), surrogate);
            base.OnDeserialize(formatter, selector);
        }
        protected override void OnDeserializeComplete(object graph, IFormatter formatter, SurrogateSelector selector) {
            SchemaModelSerialize surrogate = (SchemaModelSerialize)Crainiate.ERM4.Serialization.Serialize.GetSurrogate(graph, selector);
            SchemaModel schemaModel = (SchemaModel)graph;

            // Apply Surrogate Settings
            this.SuspendEvents = true;
            this.Suspend();

            // Do Stuff
            this._workspaceType = schemaModel.WorkspaceType;
            this._version = schemaModel.Version;
            this._document = schemaModel.Document;
            this._metadata = schemaModel.Metadata;

            this.Resume();
            this.SuspendEvents = false;

            // Call Base Method
            base.OnDeserializeComplete(graph, formatter, selector);
        }
        protected virtual void OnDiagramRequest(DiagramEventArgs e) {
            EventHandler<DiagramEventArgs> handler = DiagramRequest;
            if (handler != null) {
                handler(this, e);
            }
        }
        protected override void OnDragEnter(DragEventArgs drgevent) {
            base.OnDragEnter(drgevent);
            this.ParseDraggedObject(drgevent);
        }
        protected override void OnDragOver(DragEventArgs drgevent) {
            base.OnDragOver(drgevent);
            this.ParseDraggedObject(drgevent);
        }
        protected override void OnElementInserted(Element element) {
            // Call base method
            base.OnElementInserted(element);

            // Fix: Force newly added table to have a Maximum size of 1000x1000. Model resets size to 320x320.
            if (!this.SuspendEvents) {
                if (element is EsriTable) {
                    EsriTable table = (EsriTable)element;
                    if (table.MaximumSize.Width != 1000f || table.MaximumSize.Height != 100000f) {
                        table.MaximumSize = new SizeF(1000f, 100000f);
                    }

                    // Select newly added element
                    if (!table.Selected) {
                        // Clear Model Selection
                        this.SelectElements(false);
                        table.Selected = true;
                    }
                }
            }

            // Add dependant objects. For example, annotation featureclass require domains.
            if (!this.SuspendEvents) {
                if (element.GetType() == typeof(FeatureClass)) {
                    FeatureClass featureClass = (FeatureClass)element;
                    List<Domain> domains = Factory.RequiredDomains(featureClass);
                    float y = 0;
                    foreach (Domain domain in domains) {
                        // Check if domain already added
                        if (this.FindDomain(domain.Name) != null) { continue; }

                        // Add domain with vertical offset
                        y += featureClass.Height;
                        domain.Location = new PointF(
                            featureClass.Location.X,
                            featureClass.Location.Y + y);
                        this.Shapes.Add(this.Shapes.CreateKey(), domain);
                    }
                }
            }

            // Reset Navigator
            this.Navigate.Reset();
        }
        protected override void OnElementInvalid(Element element) {
            base.OnElementInvalid(element);

            // Reset Navigator
            this.Navigate.Reset();

            // Set the dirty flag
            this._dirty = true;
        }
        protected override void OnElementRemoved(Element element) {
            base.OnElementRemoved(element);

            // Reset Navigator
            this.Navigate.Reset();
        }
        //
        // PRIVATE METHOD
        //
        private void Model_ElementMouseUp(object sender, MouseEventArgs e) {
            try {
                // Clear Context Element
                this._elementContext = null;

                // Exit if not right mouse button
                if (e.Button != MouseButtons.Right) { return; }

                // Get Element
                Element element = sender as Element;
                if (element == null) { return; }

                // Set Context Element
                this._elementContext = element;
                
                // Show Context Menu
                if (element.GetType() == typeof(DomainCodedValue)) {
                    DomainCodedValue domainCodedValue = (DomainCodedValue)element;
                    if (domainCodedValue.SelectedCodedValue != null) {
                        this.contextDomainCodedValueItem.Show(this, e.Location);
                    }
                    else {
                        this.contextDomainCodedValue.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(DomainRange)) {
                    this.contextDomainRange.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(FeatureClass)) {
                    FeatureClass featureClass = (FeatureClass)element;
                    if (featureClass.SelectedField != null) {
                        this.contextField.Show(this, e.Location);
                    }
                    else if (featureClass.SelectedIndex != null) {
                        this.contextIndex.Show(this, e.Location);
                    }
                    else if (featureClass.SelectedIndexField != null) {
                        this.contextIndexField.Show(this, e.Location);
                    }
                    else {
                        this.contextFeatureClass.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(FeatureDataset)) {
                    this.contextFeatureDataset.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(GeometricNetwork)) {
                    this.contextGeometricNetwork.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(Network)) {
                    this.contextNetwork.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(ObjectClass)) {
                    ObjectClass objectClass = (ObjectClass)element;
                    if (objectClass.SelectedField != null) {
                        this.contextField.Show(this, e.Location);
                    }
                    else if (objectClass.SelectedIndex != null) {
                        this.contextIndex.Show(this, e.Location);
                    }
                    else if (objectClass.SelectedIndexField != null) {
                        this.contextIndexField.Show(this, e.Location);
                    }
                    else {
                        this.contextObjectClass.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(RasterCatalog)) {
                    RasterCatalog rasterCatalog = (RasterCatalog)element;
                    if (rasterCatalog.SelectedField != null) {
                        this.contextField.Show(this, e.Location);
                    }
                    else if (rasterCatalog.SelectedIndex != null) {
                        this.contextIndex.Show(this, e.Location);
                    }
                    else if (rasterCatalog.SelectedIndexField != null) {
                        this.contextIndexField.Show(this, e.Location);
                    }
                    else {
                        this.contextRasterCatalog.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(RasterBand)) {
                    RasterBand rasterBand = (RasterBand)element;
                    if (rasterBand.SelectedField != null) {
                        this.contextField.Show(this, e.Location);
                    }
                    else if (rasterBand.SelectedIndex != null) {
                        this.contextIndex.Show(this, e.Location);
                    }
                    else if (rasterBand.SelectedIndexField != null) {
                        this.contextIndexField.Show(this, e.Location);
                    }
                    else {
                        this.contextRasterBand.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(RasterDataset)) {
                    this.contextRasterDataset.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(RelationshipClass)) {
                    RelationshipClass relationshipClass = (RelationshipClass)element;
                    if (relationshipClass.SelectedField != null) {
                        this.contextField.Show(this, e.Location);
                    }
                    else if (relationshipClass.SelectedIndex != null) {
                        this.contextIndex.Show(this, e.Location);
                    }
                    else if (relationshipClass.SelectedIndexField != null) {
                        this.contextIndexField.Show(this, e.Location);
                    }
                    else {
                        this.contextRelationship.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(Subtype)) {
                    Subtype subtype = (Subtype)element;
                    if (subtype.SelectedSubtypeField != null) {
                        this.contextSubtypeField.Show(this, e.Location);
                    }
                    else {
                        this.contextSubtype.Show(this, e.Location);
                    }
                }
                else if (element.GetType() == typeof(Terrain)) {
                    this.contextTerrain.Show(this, e.Location);
                }
                else if (element.GetType() == typeof(Topology)) {
                    this.contextTopology.Show(this, e.Location);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void Model_ElementDoubleClick(object sender, EventArgs e) {
            try {
                // Get Element
                Element element = sender as Element;
                if (element == null) { return; }
                this._elementContext = element;
                //
                if (element.GetType() == typeof(DomainCodedValue)){
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemDomainCodedValue_OpenDiagram, EventArgs.Empty });
                }
                else if (element.GetType() == typeof(DomainRange)) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemDomainRange_OpenDiagram, EventArgs.Empty });
                }
                else if (element.GetType() == typeof(RelationshipClass)) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemRelationship_OpenDiagram, EventArgs.Empty });
                }
                else if (element.GetType() == typeof(GeometricNetwork)) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemGeometricNetwork_OpenDiagram, EventArgs.Empty });
                }
                else if (element.GetType() == typeof(Topology)) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemTopology_OpenDiagram, EventArgs.Empty });
                }
                else if (element.GetType() == typeof(Terrain)) {
                    this.Invoke(new EventHandler(this.MenuItem_Activate), new object[] { this.menuItemTerrain_OpenDiagram, EventArgs.Empty });
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void AddSubtypes(ObjectClass objectClass, IXPathNavigable path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // Check if Subtypes Exist
            XPathNavigator navigatorSubtypeField = navigator.SelectSingleNode("SubtypeFieldName");
            if (navigatorSubtypeField != null) {
                // Get Default Subtype
                XPathNavigator navigatorDefaultSubtypeCode = navigator.SelectSingleNode("DefaultSubtypeCode");

                // Get Subtypes
                XPathNodeIterator iteratorSubtype = navigator.Select("Subtypes/Subtype");

                // Loop for each Subtype
                while (iteratorSubtype.MoveNext()) {
                    // Create Subtype
                    XPathNavigator navigatorSubtype = iteratorSubtype.Current;
                    Subtype subtype = new Subtype(navigatorSubtype);

                    // Update Default Subtype Property
                    subtype.Default = (subtype.SubtypeCode == navigatorDefaultSubtypeCode.ValueAsInt);

                    // Add Subtype to Model
                    this.Shapes.Add(this.Shapes.CreateKey(), subtype);

                    // Add Link From ObjectClass to Subtype
                    Link link = new Link(objectClass, subtype);
                    link.BorderColor = ModelSettings.Default.EnabledLines;
                    link.BorderStyle = DashStyle.Solid;
                    link.BorderWidth = 1f;
                    link.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
                    this.Lines.Add(this.Lines.CreateKey(), link);
                }
            }
        }
        private void ContextMenu_BeforePopup(object sender, TD.SandBar.MenuPopupEventArgs e) {
            try {
                // Exit if no Context Element
                if (this._elementContext == null) { return; }

                if (sender == this.contextDomainCodedValue) {
                    this.menuItemDomainCodedValue_OpenDiagram.Enabled = true;
                    this.menuItemDomainCodedValue_AddCodedValue.Enabled = true;
                    this.menuItemDomainCodedValue_Validate.Enabled = true;
                }
                else if (sender == this.contextDomainCodedValueItem) {
                    this.menuItemDomainCodedValueItem_Delete.Enabled = true;
                }
                else if (sender == this.contextDomainRange) {
                    this.menuItemDomainRange_OpenDiagram.Enabled = true;
                    this.menuItemDomainRange_Validate.Enabled = true;
                }
                else if (sender == this.contextFeatureClass) {
                    FeatureClass featureClass = (FeatureClass)this._elementContext;
                    this.menuItemFeatureClass_AddField.Enabled = true;
                    this.menuItemFeatureClass_AddIndex.Enabled = true;
                    this.menuItemFeatureClass_Validate.Enabled = true;
                    this.menuItemFeatureClass_ViewMetadata.Enabled = !string.IsNullOrEmpty(featureClass.Metadata);
                }
                else if (sender == this.contextFeatureDataset) {
                    FeatureDataset featureDataset = (FeatureDataset)this._elementContext;
                    this.menuItemFeatureDataset_Validate.Enabled = true;
                    this.menuItemFeatureDataset_ViewMetadata.Enabled = !string.IsNullOrEmpty(featureDataset.Metadata);
                }
                else if (sender == this.contextField) {
                    this.menuItemField_AddToIndexes.Enabled = true;
                    this.menuItemField_Delete.Enabled = true;
                }
                else if (sender == this.contextGeometricNetwork) {
                    GeometricNetwork geometricNetwork = (GeometricNetwork)this._elementContext;
                    this.menuItemGeometricNetwork_OpenDiagram.Enabled = true;
                    this.menuItemGeometricNetwork_OpenEdgeEdgeDiagram.Enabled = true;
                    this.menuItemGeometricNetwork_OpenEdgeJunctionDiagram.Enabled = true;
                    this.menuItemGeometricNetwork_ViewMetadata.Enabled = !string.IsNullOrEmpty(geometricNetwork.Metadata);
                }
                else if (sender == this.contextIndex) {
                    this.menuItemIndex_AddField.Enabled = true;
                    this.menuItemIndex_Delete.Enabled = true;
                }
                else if (sender == this.contextIndexField) {
                    this.menuItemIndexField_Delete.Enabled = true;
                }
                else if (sender == this.contextNetwork) {
                    // None
                }
                else if (sender == this.contextObjectClass) {
                    ObjectClass objectClass = (ObjectClass)this._elementContext;
                    this.menuItemObjectClass_AddField.Enabled = true;
                    this.menuItemObjectClass_AddIndex.Enabled = true;
                    this.menuItemObjectClass_Validate.Enabled = true;
                    this.menuItemObjectClass_ViewMetadata.Enabled = !string.IsNullOrEmpty(objectClass.Metadata);
                }
                else if (sender == this.contextRasterBand) {
                    RasterBand rasterBand = (RasterBand)this._elementContext;
                    this.menuItemRasterBand_AddField.Enabled = true;
                    this.menuItemRasterBand_AddIndex.Enabled = true;
                    this.menuItemRasterBand_Validate.Enabled = true;
                    this.menuItemRasterBand_ViewMetadata.Enabled = !string.IsNullOrEmpty(rasterBand.Metadata);
                }
                else if (sender == this.contextRasterCatalog) {
                    RasterCatalog rasterCatalog = (RasterCatalog)this._elementContext;
                    this.menuItemRasterCatalog_AddField.Enabled = true;
                    this.menuItemRasterCatalog_AddIndex.Enabled = true;
                    this.menuItemRasterCatalog_Validate.Enabled = true;
                    this.menuItemRasterCatalog_ViewMetadata.Enabled = !string.IsNullOrEmpty(rasterCatalog.Metadata);
                }
                else if (sender == this.contextRasterDataset) {
                    RasterDataset rasterDataset = (RasterDataset)this._elementContext;
                    this.menuItemRasterDataset_Validate.Enabled = true;
                    this.menuItemRasterDataset_ViewMetadata.Enabled = !string.IsNullOrEmpty(rasterDataset.Metadata);
                }
                else if (sender == this.contextRelationship) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    this.menuItemRelationship_AddField.Enabled = true;
                    this.menuItemRelationship_AddIndex.Enabled = true;
                    this.menuItemRelationship_OpenDiagram.Enabled = true;
                    this.menuItemRelationship_Validate.Enabled = true;
                    this.menuItemRelationship_ViewMetadata.Enabled = !string.IsNullOrEmpty(relationshipClass.Metadata);
                }
                else if (sender == this.contextSubtype) {
                    Subtype subtype = (Subtype)this._elementContext;
                    this.menuItemSubtype_AddField.Enabled = true;
                    this.menuItemSubtype_SetAsDefault.Enabled = !subtype.Default;
                    this.menuItemSubtype_Validate.Enabled = true;
                }
                else if (sender == this.contextSubtypeField) {
                    this.menuItemSubtypeField_Delete.Enabled = true;
                }
                else if (sender == this.contextTerrain) {
                    Terrain terrain = (Terrain)this._elementContext;
                    this.menuItemTerrain_OpenDiagram.Enabled = true;
                }
                else if (sender == this.contextTopology) {
                    Topology topology = (Topology)this._elementContext;
                    this.menuItemTopology_OpenDiagram.Enabled = true;
                    this.menuItemTopology_OpenRuleDiagram.Enabled = true;
                    this.menuItemTopology_Validate.Enabled = true;
                    this.menuItemTopology_ViewMetadata.Enabled = !string.IsNullOrEmpty(topology.Metadata);
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                // Exit if no Context Element
                if (this._elementContext == null) { return; }

                // Get Diagrammer Environment
                DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;

                if (sender == this.menuItemDomainCodedValue_AddCodedValue) {
                    DomainCodedValue domainCodedValue = (DomainCodedValue)this._elementContext;
                    domainCodedValue.AddCodedValue(new DomainCodedValueRow());
                }
                else if (sender == this.menuItemDomainCodedValue_OpenDiagram) {
                    DomainCodedValue domainCodedValue = (DomainCodedValue)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(domainCodedValue, typeof(DomainModel)));
                }
                else if (sender == this.menuItemDomainCodedValue_Validate) {
                    DomainCodedValue domainCodedValue = (DomainCodedValue)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(domainCodedValue));
                }
                else if (sender == this.menuItemDomainCodedValueItem_Delete) {
                    DomainCodedValue domainCodedValue = (DomainCodedValue)this._elementContext;
                    DomainCodedValueRow domainCodedValueRow = domainCodedValue.SelectedCodedValue;
                    if (domainCodedValueRow == null) { return; }
                    domainCodedValue.RemoveCodedValue(domainCodedValueRow);
                }
                else if (sender == this.menuItemDomainRange_OpenDiagram) {
                    DomainRange domainRange = (DomainRange)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(domainRange, typeof(DomainModel)));
                }
                else if (sender == this.menuItemDomainRange_Validate) {
                    DomainRange domainRange = (DomainRange)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(domainRange));
                }
                else if (sender == this.menuItemFeatureClass_AddField) {
                    FeatureClass featureClass = (FeatureClass)this._elementContext;
                    featureClass.AddField(new Field());
                }
                else if (sender == this.menuItemFeatureClass_AddIndex) {
                    FeatureClass featureClass = (FeatureClass)this._elementContext;
                    featureClass.AddIndex(new Index());
                }
                else if (sender == this.menuItemFeatureClass_Validate) {
                    FeatureClass featureClass = (FeatureClass)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(featureClass));
                }
                else if (sender == this.menuItemFeatureClass_ViewMetadata) {
                    FeatureClass featureClass = (FeatureClass)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(featureClass));
                }
                else if (sender == this.menuItemFeatureDataset_Validate) {
                    FeatureDataset featureDataset = (FeatureDataset)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(featureDataset));
                }
                else if (sender == this.menuItemFeatureDataset_ViewMetadata) {
                    FeatureDataset featureDataset = (FeatureDataset)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(featureDataset));
                }
                else if (sender == this.menuItemField_AddToIndexes) {
                    if (this._elementContext is ObjectClass) {
                        // Get Selected Field
                        ObjectClass objectClass = (ObjectClass)this._elementContext;
                        Field field = objectClass.SelectedField;

                        // Create New IndexField
                        IndexField indexField = new IndexField();
                        indexField.Name = field.Name;

                        // Create New Index
                        Index index = new Index();
                        index.AddIndexField(indexField);

                        // Add Index
                        objectClass.AddIndex(index);
                    }
                    else if (this._elementContext is RasterBand) {
                        // Get Selected Field
                        RasterBand rasterBand = (RasterBand)this._elementContext;
                        Field field = rasterBand.SelectedField;

                        // Create New IndexField
                        IndexField indexField = new IndexField();
                        indexField.Name = field.Name;

                        // Create New Index
                        Index index = new Index();
                        index.AddIndexField(indexField);

                        // Add Index
                        rasterBand.AddIndex(index);
                    }
                }
                else if (sender == this.menuItemField_Delete) {
                    if (this._elementContext is ObjectClass) {
                        // Get Selected Field
                        ObjectClass objectClass = (ObjectClass)this._elementContext;
                        Field field = objectClass.SelectedField;

                        // Remove Field
                        objectClass.RemoveField(field);
                    }
                    else if (this._elementContext is RasterBand) {
                        // Get Selected Field
                        RasterBand rasterBand = (RasterBand)this._elementContext;
                        Field field = rasterBand.SelectedField;

                        // Remove Field
                        rasterBand.RemoveField(field);
                    }
                }
                else if (sender == this.menuItemGeometricNetwork_OpenDiagram) {
                    GeometricNetwork geometricNetwork = (GeometricNetwork)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(geometricNetwork, typeof(GeometricNetworkModel)));
                }
                else if (sender == this.menuItemGeometricNetwork_OpenEdgeEdgeDiagram) {
                    GeometricNetwork geometricNetwork = (GeometricNetwork)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(geometricNetwork, typeof(GeometricNetworkModelEE)));
                }
                else if (sender == this.menuItemGeometricNetwork_OpenEdgeJunctionDiagram) {
                    GeometricNetwork geometricNetwork = (GeometricNetwork)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(geometricNetwork, typeof(GeometricNetworkModelEJ)));
                }
                else if (sender == this.menuItemGeometricNetwork_ViewMetadata) {
                    GeometricNetwork geometricNetwork = (GeometricNetwork)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(geometricNetwork));
                }
                else if (sender == this.menuItemIndex_AddField) {
                    if (this._elementContext is ObjectClass) {
                        // Get Selected Index
                        ObjectClass objectClass = (ObjectClass)this._elementContext;
                        Index index = objectClass.SelectedIndex;
                        
                        // Add Index Field
                        index.AddIndexField(new IndexField());
                    }
                    else if (this._elementContext is RasterBand) {
                        // Get Selected Index
                        RasterBand rasterBand = (RasterBand)this._elementContext;
                        Index index = rasterBand.SelectedIndex;

                        // Add Index Field
                        index.AddIndexField(new IndexField());
                    }
                }
                else if (sender == this.menuItemIndex_Delete) {
                    if (this._elementContext is ObjectClass) {
                        // Get Selected Index
                        ObjectClass objectClass = (ObjectClass)this._elementContext;
                        Index index = objectClass.SelectedIndex;

                        // Remove Index
                        objectClass.RemoveIndex(index);
                    }
                    else if (this._elementContext is RasterBand) {
                        // Get Selected Index
                        RasterBand rasterBand = (RasterBand)this._elementContext;
                        Index index = rasterBand.SelectedIndex;

                        // Remove Index
                        rasterBand.RemoveIndex(index);
                    }
                }
                else if (sender == this.menuItemIndexField_Delete) {
                    if (this._elementContext is ObjectClass) {
                        // Get Selected Index Field
                        ObjectClass objectClass = (ObjectClass)this._elementContext;
                        IndexField indexField = objectClass.SelectedIndexField;

                        // Get Parent Index
                        Index index = indexField.Parent as Index;
                        if (index == null) { return; }

                        // Remove Index Field
                        index.RemoveIndexField(indexField);
                    }
                    else if (this._elementContext is RasterBand) {
                        // Get Selected Index Field
                        ObjectClass rasterBand = (ObjectClass)this._elementContext;
                        IndexField indexField = rasterBand.SelectedIndexField;

                        // Get Parent Index
                        Index index = indexField.Parent as Index;
                        if (index == null) { return; }

                        // Remove Index Field
                        index.RemoveIndexField(indexField);
                    }
                }
                else if (sender == this.menuItemObjectClass_AddField) {
                    ObjectClass objectClass = (ObjectClass)this._elementContext;
                    objectClass.AddField(new Field());
                }
                else if (sender == this.menuItemObjectClass_AddIndex) {
                    ObjectClass objectClass = (ObjectClass)this._elementContext;
                    objectClass.AddIndex(new Index());
                }
                else if (sender == this.menuItemObjectClass_Validate) {
                    ObjectClass objectClass = (ObjectClass)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(objectClass));
                }
                else if (sender == this.menuItemObjectClass_ViewMetadata) {
                    ObjectClass objectClass = (ObjectClass)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(objectClass));
                }
                else if (sender == this.menuItemRasterBand_AddField) {
                    RasterBand rasterBand = (RasterBand)this._elementContext;
                    rasterBand.AddField(new Field());
                }
                else if (sender == this.menuItemRasterBand_AddIndex) {
                    RasterBand rasterBand = (RasterBand)this._elementContext;
                    rasterBand.AddIndex(new Index());
                }
                else if (sender == this.menuItemRasterBand_Validate) {
                    RasterBand rasterBand = (RasterBand)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(rasterBand));
                }
                else if (sender == this.menuItemRasterBand_ViewMetadata) {
                    RasterBand rasterBand = (RasterBand)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(rasterBand));
                }
                else if (sender == this.menuItemRasterCatalog_AddField) {
                    RasterCatalog rasterCatalog = (RasterCatalog)this._elementContext;
                    rasterCatalog.AddField(new Field());
                }
                else if (sender == this.menuItemRasterCatalog_AddIndex) {
                    RasterCatalog rasterCatalog = (RasterCatalog)this._elementContext;
                    rasterCatalog.AddIndex(new Index());
                }
                else if (sender == this.menuItemRasterCatalog_Validate) {
                    RasterCatalog rasterCatalog = (RasterCatalog)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(rasterCatalog));
                }
                else if (sender == this.menuItemRasterCatalog_ViewMetadata) {
                    RasterCatalog rasterCatalog = (RasterCatalog)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(rasterCatalog));
                }
                else if (sender == this.menuItemRasterDataset_Validate) {
                    RasterDataset rasterDataset = (RasterDataset)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(rasterDataset));
                }
                else if (sender == this.menuItemRasterDataset_ViewMetadata) {
                    RasterDataset rasterDataset = (RasterDataset)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(rasterDataset));
                }
                else if (sender == this.menuItemRelationship_AddField) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    relationshipClass.AddField(new Field());
                }
                else if (sender == this.menuItemRelationship_AddIndex) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    relationshipClass.AddIndex(new Index());
                }
                else if (sender == this.menuItemRelationship_OpenDiagram) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(relationshipClass, typeof(RelationshipModel)));
                }
                else if (sender == this.menuItemRelationship_Validate) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(relationshipClass));
                }
                else if (sender == this.menuItemRelationship_ViewMetadata) {
                    RelationshipClass relationshipClass = (RelationshipClass)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(relationshipClass));
                }
                else if (sender == this.menuItemSubtype_AddField) {
                    Subtype subtype = (Subtype)this._elementContext;
                    subtype.AddSubtypeField(new SubtypeField());
                }
                else if (sender == this.menuItemSubtype_SetAsDefault) {
                    Subtype subtype = (Subtype)this._elementContext;
                    subtype.Default = !subtype.Default;
                    if (subtype.Default) {
                        ObjectClass objectClass = subtype.GetParent();
                        if (objectClass == null) { return; }
                        foreach (Subtype subtype2 in objectClass.GetSubtypes()) {
                            if (subtype2 == subtype) { continue; }
                            if (subtype2.Default) {
                                subtype2.Default = false;
                            }
                        }
                    }
                }
                else if (sender == this.menuItemSubtype_Validate) {
                    Subtype subtype = (Subtype)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(subtype));
                }
                else if (sender == this.menuItemSubtypeField_Delete) {
                    Subtype subtype = (Subtype)this._elementContext;
                    SubtypeField subtypeField = subtype.SelectedSubtypeField;
                    subtype.RemoveSubtypeField(subtypeField);
                }
                else if (sender == this.menuItemTopology_OpenDiagram) {
                    Topology topology = (Topology)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(topology, typeof(TopologyModel)));
                }
                else if (sender == this.menuItemTopology_OpenRuleDiagram) {
                    Topology topology = (Topology)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(topology, typeof(TopologyRuleModel)));
                }
                else if (sender == this.menuItemTopology_Validate) {
                    Topology topology = (Topology)this._elementContext;
                    diagrammerEnvironment.OnTableValidationRequest(new TableEventArgs(topology));
                }
                else if (sender == this.menuItemTopology_ViewMetadata) {
                    Topology topology = (Topology)this._elementContext;
                    diagrammerEnvironment.OnMetadataViewerRequest(new DatasetEventArgs(topology));
                }
                else if (sender == this.menuItemTerrain_OpenDiagram) {
                    Terrain terrain = (Terrain)this._elementContext;
                    this.OnDiagramRequest(new DiagramEventArgs(terrain, typeof(TerrainModel)));
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void ParseDraggedObject(DragEventArgs e) {
            if (e.Effect == DragDropEffects.None) {
                if (e.Data.GetDataPresent(typeof(DomainCodedValue)) ||
                    e.Data.GetDataPresent(typeof(DomainRange)) ||
                    e.Data.GetDataPresent(typeof(FeatureClass)) ||
                    e.Data.GetDataPresent(typeof(FeatureDataset)) ||
                    e.Data.GetDataPresent(typeof(GeometricNetwork)) ||
                    e.Data.GetDataPresent(typeof(Network)) ||
                    e.Data.GetDataPresent(typeof(ObjectClass)) ||
                    e.Data.GetDataPresent(typeof(RasterCatalog)) ||
                    e.Data.GetDataPresent(typeof(RasterDataset)) ||
                    e.Data.GetDataPresent(typeof(RelationshipClass)) ||
                    e.Data.GetDataPresent(typeof(Subtype)) ||
                    e.Data.GetDataPresent(typeof(Terrain)) ||
                    e.Data.GetDataPresent(typeof(Topology))) {
                    e.Effect = DragDropEffects.All;
                }
            }
        }
    }
}

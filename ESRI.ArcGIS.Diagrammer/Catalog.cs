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
using System.Windows.Forms;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.ExceptionHandler;
using TD.SandBar;

namespace ESRI.ArcGIS.Diagrammer {
    public partial class Catalog : UserControl {
        // Image List Constants
        private const string FOLDER_CLOSED = "folder_closed";
        private const string FOLDER_OPENED = "folder_opened";
        private const string GEODATABASE = "geodatabase";
        private const string ANNOTATION_FEATURE_CLASS = "annotation_feature_class";
        private const string CODED_VALUE_DOMAIN = "coded_value_domain";
        private const string DIMENSION_FEATURE_CLASS = "dimension_feature_class";
        private const string FEATURE_DATASET = "feature_dataset";
        private const string FIELD = "field";
        private const string GEOMETRIC_NETWORK = "geometric_network";
        private const string MULTIPATCH_FEATURE_CLASS = "multipatch_feature_class";
        private const string NETWORK_DATASET = "network_dataset";
        private const string POINT_FEATURE_CLASS = "point_feature_class";
        private const string POLYGON_FEATURE_CLASS = "polygon_feature_class";
        private const string POLYLINE_FEATURE_CLASS = "polyline_feature_class";
        private const string RANGE_DOMAIN = "range_domain";
        private const string RASTER_BAND = "raster_band";
        private const string RASTER_CATALOG = "raster_catalog";
        private const string RASTER_DATASET = "raster_dataset";
        private const string RELATIONSHIP_CLASS = "relationship_class";
        private const string SUBTYPE = "subtype";
        private const string TABLE = "table";
        private const string TERRAIN = "terrain";
        private const string TOPOLOGY = "topology";

        private SchemaModel _model = null;
        private bool _suspend = false;
        //
        // CONSTRUCTOR
        //
        public Catalog() {
            InitializeComponent();

            //
            this.buttonItemCatalog.Image = Resources.BITMAP_CATALOG;
            this.buttonItemCategorized.Image = Resources.BITMAP_CATEGORIZED;
            this.buttonItemAlphabetical.Image = Resources.BITMAP_APHABETICAL;
            this.buttonItemRefresh.Image = Resources.BITMAP_REFRESH;

            this.buttonItemCatalog.ToolTipText = Resources.TEXT_CATALOG_VIEW;
            this.buttonItemCategorized.ToolTipText = Resources.TEXT_CATEGORIZED;
            this.buttonItemAlphabetical.ToolTipText = Resources.TEXT_ALPHABETICAL;
            this.buttonItemRefresh.ToolTipText = Resources.TEXT_REFRESH;

            // Load TreeView Image List
            this.treeView1.ImageList.Images.Add(Catalog.FOLDER_CLOSED, Resources.BITMAP_FOLDER_CLOSED);
            this.treeView1.ImageList.Images.Add(Catalog.FOLDER_OPENED, Resources.BITMAP_FOLDER_OPENED);
            this.treeView1.ImageList.Images.Add(Catalog.GEODATABASE, Resources.BITMAP_GEODATABASE);
            this.treeView1.ImageList.Images.Add(Catalog.ANNOTATION_FEATURE_CLASS, Resources.BITMAP_ANNOTATION_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.CODED_VALUE_DOMAIN, Resources.BITMAP_CODED_VALUE_DOMAIN);
            this.treeView1.ImageList.Images.Add(Catalog.DIMENSION_FEATURE_CLASS, Resources.BITMAP_DIMENSION_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.FEATURE_DATASET, Resources.BITMAP_FEATURE_DATASET);
            this.treeView1.ImageList.Images.Add(Catalog.FIELD, Resources.BITMAP_FIELD);
            this.treeView1.ImageList.Images.Add(Catalog.GEOMETRIC_NETWORK, Resources.BITMAP_GEOMETRIC_NETWORK);
            this.treeView1.ImageList.Images.Add(Catalog.MULTIPATCH_FEATURE_CLASS, Resources.BITMAP_MULTIPATCH_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.NETWORK_DATASET, Resources.BITMAP_NETWORK_DATASET);
            this.treeView1.ImageList.Images.Add(Catalog.POINT_FEATURE_CLASS, Resources.BITMAP_POINT_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.POLYGON_FEATURE_CLASS, Resources.BITMAP_POLYGON_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.POLYLINE_FEATURE_CLASS, Resources.BITMAP_POLYLINE_FEATURE_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.RANGE_DOMAIN, Resources.BITMAP_RANGE_DOMAIN);
            this.treeView1.ImageList.Images.Add(Catalog.RASTER_BAND, Resources.BITMAP_RASTER_BAND);
            this.treeView1.ImageList.Images.Add(Catalog.RASTER_CATALOG, Resources.BITMAP_RASTER_CATALOG);
            this.treeView1.ImageList.Images.Add(Catalog.RASTER_DATASET, Resources.BITMAP_RASTER_DATASET);
            this.treeView1.ImageList.Images.Add(Catalog.RELATIONSHIP_CLASS, Resources.BITMAP_RELATIONSHIP_CLASS);
            this.treeView1.ImageList.Images.Add(Catalog.SUBTYPE, Resources.BITMAP_SUBTYPE);
            this.treeView1.ImageList.Images.Add(Catalog.TABLE, Resources.BITMAP_TABLE);
            this.treeView1.ImageList.Images.Add(Catalog.TERRAIN, Resources.BITMAP_TERRAIN);
            this.treeView1.ImageList.Images.Add(Catalog.TOPOLOGY, Resources.BITMAP_TOPOLOGY);

            //
            this.menuButtonItemScroll.Text = Resources.TEXT_SCROLL_;
            this.menuButtonItemFlash.Text = Resources.TEXT_FLASH_;

            // Update Renderer
            ColorSchemeSettings.Default.PropertyChanged += new PropertyChangedEventHandler(this.ColorScheme_PropertyChanged);
            this.ColorScheme_PropertyChanged(null, null);
        }
        //
        // PROPERTIES
        //
        public SchemaModel SchemaModel {
            get { return this._model; }
            set { this._model = value; }
        }
        //
        // PUBLIC METHODS
        //
        public void Suspend() {
            this._suspend = true;
        }
        public void Resume() {
            this._suspend = false;
        }
        public void RefreshCatalog() {
            // Exit if Not Visible
            if (!base.Visible) { return; }

            // Exit if Control is Suspended
            if (this._suspend) { return; }

            // Exit if No Parent
            if (base.Parent == null) { return; }

            // Load Model
            this.LoadModel();
        }
        //
        // PRIVATE METHODS
        //
        private void ColorScheme_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            try {
                // Update SandBar
                Office2007Renderer sandBarRenderer = (Office2007Renderer)this.sandBarManager1.Renderer;
                if (sandBarRenderer.ColorScheme == ColorSchemeSettings.Default.ColorScheme) { return; }
                sandBarRenderer.ColorScheme = ColorSchemeSettings.Default.ColorScheme;
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void ContextMenu_BeforePopup(object sender, TD.SandBar.MenuPopupEventArgs e) {
            try {
                TreeNode treeNode = this.treeView1.SelectedNode;
                this.menuButtonItemScroll.Enabled = (treeNode is TreeNodeTable);
                this.menuButtonItemFlash.Enabled = (treeNode is TreeNodeTable);
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private TreeNode CreateCatalogNode(TreeNode node, EsriTable table) {
            if (table.GetType() == typeof(DomainCodedValue)) {
                // Get Coded Value Domain
                DomainCodedValue domainCodedValue = (DomainCodedValue)table;

                // Add Coded Value Domain
                TreeNodeTable treeNode = new TreeNodeTable(domainCodedValue);
                treeNode.ImageKey = Catalog.CODED_VALUE_DOMAIN;
                treeNode.SelectedImageKey = Catalog.CODED_VALUE_DOMAIN;
                treeNode.Text = domainCodedValue.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(DomainRange)) {
                // Get Range Domain
                DomainRange domainRange = (DomainRange)table;

                // Add Range Domain
                TreeNodeTable treeNode = new TreeNodeTable(domainRange);
                treeNode.ImageKey = Catalog.RANGE_DOMAIN;
                treeNode.SelectedImageKey = Catalog.RANGE_DOMAIN;
                treeNode.Text = domainRange.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(FeatureClass)) {
                // Get FeatureClass
                FeatureClass featureClass = (FeatureClass)table;

                // Add FeatureClass Node
                string imageKey = null;
                switch (featureClass.FeatureType) {
                    case ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTAnnotation:
                        imageKey = Catalog.ANNOTATION_FEATURE_CLASS;
                        break;
                    case ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTDimension:
                        imageKey = Catalog.DIMENSION_FEATURE_CLASS;
                        break;
                    case ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple:
                        switch (featureClass.ShapeType) {
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                                imageKey = Catalog.POINT_FEATURE_CLASS;
                                break;
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                                imageKey = Catalog.POLYGON_FEATURE_CLASS;
                                break;
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                                imageKey = Catalog.POLYLINE_FEATURE_CLASS;
                                break;
                            case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultiPatch:
                                imageKey = Catalog.MULTIPATCH_FEATURE_CLASS;
                                break;
                            default:
                                imageKey = Catalog.POINT_FEATURE_CLASS;
                                break;
                        }
                        break;
                    default:
                        imageKey = POINT_FEATURE_CLASS;
                        break;
                }
                TreeNodeTable treeNode = new TreeNodeTable(featureClass);
                treeNode.ImageKey = imageKey;
                treeNode.SelectedImageKey = imageKey;
                treeNode.Text = featureClass.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(FeatureDataset)) {
                // Get FeatureDataset
                FeatureDataset featureDataset = (FeatureDataset)table;

                // Add FeatureDataset Node
                TreeNodeTable treeNode = new TreeNodeTable(featureDataset);
                treeNode.ImageKey = Catalog.FEATURE_DATASET;
                treeNode.SelectedImageKey = Catalog.FEATURE_DATASET;
                treeNode.Text = featureDataset.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(GeometricNetwork)) {
                // Get GeometricNetwork
                GeometricNetwork geometricNetwork = (GeometricNetwork)table;

                // Add GeometricNetwork Node
                TreeNodeTable treeNode = new TreeNodeTable(geometricNetwork);
                treeNode.ImageKey = Catalog.GEOMETRIC_NETWORK;
                treeNode.SelectedImageKey = Catalog.GEOMETRIC_NETWORK;
                treeNode.Text = geometricNetwork.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(Network)) {
                // Get Network
                Network network = (Network)table;

                // Add Network TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(network);
                treeNode.ImageKey = Catalog.NETWORK_DATASET;
                treeNode.SelectedImageKey = Catalog.NETWORK_DATASET;
                treeNode.Text = network.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(ObjectClass)) {
                // Get ObjectClass
                ObjectClass objectClass = (ObjectClass)table;

                // Add ObjectClass TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(objectClass);
                treeNode.ImageKey = Catalog.TABLE;
                treeNode.SelectedImageKey = Catalog.TABLE;
                treeNode.Text = objectClass.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(RasterBand)) {
                // Get RasterBand
                RasterBand rasterBand = (RasterBand)table;

                // Add RasterBand TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(rasterBand);
                treeNode.ImageKey = Catalog.RASTER_BAND;
                treeNode.SelectedImageKey = Catalog.RASTER_BAND;
                treeNode.Text = rasterBand.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(RasterCatalog)) {
                // Get RasterCatalog
                RasterCatalog rasterCatalog = (RasterCatalog)table;

                // Add RasterCatalog TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(rasterCatalog);
                treeNode.ImageKey = Catalog.RASTER_CATALOG;
                treeNode.SelectedImageKey = Catalog.RASTER_CATALOG;
                treeNode.Text = rasterCatalog.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(RasterDataset)) {
                // Get RasterDataset
                RasterDataset rasterDataset = (RasterDataset)table;

                // Add RasterDataset TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(rasterDataset);
                treeNode.ImageKey = Catalog.RASTER_DATASET;
                treeNode.SelectedImageKey = Catalog.RASTER_DATASET;
                treeNode.Text = rasterDataset.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(RelationshipClass)) {
                // Get RelationshipClass
                RelationshipClass relationshipClass = (RelationshipClass)table;

                // Add RelationshipClass TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(relationshipClass);
                treeNode.ImageKey = Catalog.RELATIONSHIP_CLASS;
                treeNode.SelectedImageKey = Catalog.RELATIONSHIP_CLASS;
                treeNode.Text = relationshipClass.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(Subtype)) {
                // Get Subtype
                Subtype subtype = (Subtype)table;

                // Add Subtype TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(subtype);
                treeNode.ImageKey = Catalog.SUBTYPE;
                treeNode.SelectedImageKey = Catalog.SUBTYPE;
                treeNode.Text = subtype.SubtypeName;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(Terrain)) {
                // Get Terrain
                Terrain terrain = (Terrain)table;

                // Add Terrain TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(terrain);
                treeNode.ImageKey = Catalog.TERRAIN;
                treeNode.SelectedImageKey = Catalog.TERRAIN;
                treeNode.Text = terrain.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            if (table.GetType() == typeof(Topology)) {
                // Get Topology
                Topology topology = (Topology)table;

                // Add Topology TreeNode
                TreeNodeTable treeNode = new TreeNodeTable(topology);
                treeNode.ImageKey = Catalog.TOPOLOGY;
                treeNode.SelectedImageKey = Catalog.TOPOLOGY;
                treeNode.Text = topology.Name;
                node.Nodes.Add(treeNode);

                return treeNode;
            }

            return null;
        }
        private TreeNode FindNode(TreeNode treeNode, EsriTable table) {
            //
            if (treeNode is TreeNodeTable) {
                TreeNodeTable treeNodeTable = (TreeNodeTable)treeNode;
                if (treeNodeTable.Table == table) {
                    return treeNodeTable;
                }
            }

            //
            foreach (TreeNode treeNodeChild in treeNode.Nodes) {
                TreeNode treeNode2 = this.FindNode(treeNodeChild, table);
                if (treeNode2 != null) {
                    return treeNode2;
                }
            }

            return null;
        }
        private void MenuItem_Activate(object sender, EventArgs e) {
            try {
                TreeNode treeNode = this.treeView1.SelectedNode;
                if (treeNode == null) { return; }
                TreeNodeTable treeNodeTable = treeNode as TreeNodeTable;
                if (treeNodeTable == null) { return; }
                EsriTable table = treeNodeTable.Table;
                if (table == null) { return; }

                if (sender == this.menuButtonItemScroll) {
                    this._model.ScrollToElement(table);
                }
                else if (sender == this.menuButtonItemFlash) {
                    table.Flash();
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void LoadModel() {
            // Store Selected EsriTable (if any)
            EsriTable table = null;
            if (this.treeView1.SelectedNode != null) {
                TreeNodeTable treeNodeTable = this.treeView1.SelectedNode as TreeNodeTable;
                if (treeNodeTable != null) {
                    if (treeNodeTable.Table != null) {
                        table = treeNodeTable.Table;
                    }
                }
            }

            // Clear TreeView
            this.treeView1.Nodes.Clear();

            // Exit if No Model
            if (this._model == null) { return; }

            // Get Datasets
            List<Dataset> datasets = this._model.GetDatasets();

            // Get Domains
            List<Domain> domains = this._model.GetDomains();

            // Start TreeView Update
            this.treeView1.BeginUpdate();
            this.treeView1.Sorted = false;

            // Add Geodatabase Node
            TreeNodeGeodatabase treeNode = new TreeNodeGeodatabase(this._model);
            treeNode.ImageKey = Catalog.GEODATABASE;
            treeNode.SelectedImageKey = Catalog.GEODATABASE;
            treeNode.Text = this._model.Title;
            this.treeView1.Nodes.Add(treeNode);

            if (this.buttonItemCatalog.Checked) {
                // Sort Datasets
                datasets.Sort();

                // Loop Throught Datasets
                foreach (Dataset dataset in datasets) {
                    if (dataset is FeatureDataset) {
                        // Get FeatureDataset
                        FeatureDataset featureDataset = (FeatureDataset)dataset;

                        // Add FeatureDataset Node
                        TreeNode treeNode2 = this.CreateCatalogNode(treeNode, featureDataset);

                        // Get Child Datasets
                        List<Dataset> datasets2 = featureDataset.GetChildren();
                        datasets2.Sort();

                        foreach (Dataset dataset2 in datasets2) {
                            TreeNode treeNode3 = this.CreateCatalogNode(treeNode2, dataset2);

                            // Add Subtypes if ObjectClass
                            if (dataset2 is ObjectClass) {
                                // Get ObjectClass
                                ObjectClass objectClass = (ObjectClass)dataset2;

                                // Get Subtypes
                                List<Subtype> subtypes = objectClass.GetSubtypes();
                                subtypes.Sort();

                                // Add Subtypes Nodes
                                foreach (Subtype subtype in subtypes) {
                                    TreeNode treeNodeSubtype = this.CreateCatalogNode(treeNode3, subtype);
                                }
                            }
                        }
                    }
                }

                // Add Everything Else
                foreach (Dataset dataset in datasets) {
                    // Skip FeatureDataset and FeatureDataset Objects
                    if (dataset is FeatureDataset ||
                        dataset is GeometricNetwork ||
                        dataset is Network ||
                        dataset is RasterBand ||
                        dataset is Terrain ||
                        dataset is Topology) {
                        continue;
                    }

                    // Skip Objects that Belong to a FeatureDataset
                    if (dataset is FeatureClass ||
                        dataset is RelationshipClass) {
                        Dataset parent = dataset.GetParent();
                        if (parent != null) {
                            continue;
                        }
                    }

                    // Create Node
                    TreeNode treeNode2 = this.CreateCatalogNode(treeNode, dataset);

                    // Add Subtypes if ObjectClass
                    if (dataset is ObjectClass) {
                        // Get ObjectClass
                        ObjectClass objectClass = (ObjectClass)dataset;

                        // Get Subtypes
                        List<Subtype> subtypes = objectClass.GetSubtypes();
                        subtypes.Sort();

                        // Add Subtypes Nodes
                        foreach (Subtype subtype in subtypes) {
                            TreeNode treeNodeSubtype = this.CreateCatalogNode(treeNode2, subtype);
                        }
                    }

                    // Add Raster Bands
                    if (dataset.GetType() == typeof(RasterDataset)) {
                        // Get RasterDataset
                        RasterDataset rasterDataset = (RasterDataset)dataset;

                        // Get RasterBands
                        List<Dataset> rasterBands = rasterDataset.GetChildren();

                        // Add RasterBands
                        foreach (Dataset datasetRasterBand in rasterBands) {
                            if (datasetRasterBand.GetType() == typeof(RasterBand)) {
                                RasterBand rasterBand = (RasterBand)datasetRasterBand;
                                TreeNode treeNodeRasterBand = this.CreateCatalogNode(treeNode2, rasterBand);
                            }
                        }
                    }
                }

                // Sort Domains
                domains.Sort();

                // Add Domains
                foreach (Domain domain in domains) {
                    TreeNode treeNodeDomain = this.CreateCatalogNode(treeNode, domain);
                }

                // Expand Geodatabase Node
                treeNode.Expand();
            }
            else if (this.buttonItemCategorized.Checked) {
                // Loop for each Dataset
                foreach (Dataset dataset in datasets) {
                    // Get Group Node Name
                    string key = string.Empty;
                    if (dataset.GetType() == typeof(FeatureDataset)) {
                        key = Resources.TEXT_FEATURE_DATASET;
                    }
                    else if (dataset.GetType() == typeof(FeatureClass)) {
                        key = Resources.TEXT_FEATURE_CLASS;
                    }
                    else if (dataset.GetType() == typeof(GeometricNetwork)) {
                        key = Resources.TEXT_GEOMETRIC_NETWORK;
                    }
                    else if (dataset.GetType() == typeof(ObjectClass)) {
                        key = Resources.TEXT_TABLE;
                    }
                    else if (dataset.GetType() == typeof(RasterBand)) {
                        key = Resources.TEXT_RASTER_BAND;
                    }
                    else if (dataset.GetType() == typeof(RasterCatalog)) {
                        key = Resources.TEXT_RASTER_CATALOG;
                    }
                    else if (dataset.GetType() == typeof(RasterDataset)) {
                        key = Resources.TEXT_RASTER_DATASET;
                    }
                    else if (dataset.GetType() == typeof(RelationshipClass)) {
                        key = Resources.TEXT_RELATIONSHIP;
                    }
                    else if (dataset.GetType() == typeof(Terrain)) {
                        key = Resources.TEXT_TERRAIN;
                    }
                    else if (dataset.GetType() == typeof(Topology)) {
                        key = Resources.TEXT_TOPOLOGY;
                    }
                    if (string.IsNullOrEmpty(key)) { continue; }

                    // Get Group Node (create if it does not exist)
                    TreeNodeGroup treeNodeGroup = null;
                    foreach (TreeNodeGroup group in treeNode.Nodes) {
                        if (group.Type == dataset.GetType()) {
                            treeNodeGroup = group;
                            break;
                        }
                    }
                    if (treeNodeGroup == null) {
                        treeNodeGroup = new TreeNodeGroup(dataset.GetType());
                        treeNodeGroup.ImageKey = Catalog.FOLDER_CLOSED;
                        treeNodeGroup.SelectedImageKey = Catalog.FOLDER_CLOSED;
                        treeNodeGroup.Text = key;
                        treeNode.Nodes.Add(treeNodeGroup);
                    }

                    // Create New Dataset Node
                    TreeNode treeNodeDataset = this.CreateCatalogNode(treeNodeGroup, dataset);
                }

                // Append Subtypes Nodes
                foreach (Dataset dataset in datasets) {
                    // Is ObjectClass?
                    if (dataset is ObjectClass) {
                        // Cast to ObjectClass
                        ObjectClass objectClass = (ObjectClass)dataset;

                        // Get Subtypes
                        List<Subtype> subtypes = objectClass.GetSubtypes();
                        if (subtypes.Count == 0) { continue; }

                        // Find Subtype Group Node
                        TreeNodeGroup treeNodeGroup = null;
                        foreach (TreeNodeGroup group in treeNode.Nodes) {
                            if (group.Type == typeof(Subtype)) {
                                treeNodeGroup = group;
                                break;
                            }
                        }
                        if (treeNodeGroup == null) {
                            treeNodeGroup = new TreeNodeGroup(typeof(Subtype));
                            treeNodeGroup.ImageKey = Catalog.FOLDER_CLOSED;
                            treeNodeGroup.SelectedImageKey = Catalog.FOLDER_CLOSED;
                            treeNodeGroup.Text = Resources.TEXT_SUBTYPE;
                            treeNode.Nodes.Add(treeNodeGroup);
                        }

                        // Add Each Subtype
                        foreach (Subtype subtype in subtypes) {
                            TreeNode treeNodeSubtype = this.CreateCatalogNode(treeNodeGroup, subtype);
                        }
                    }
                }

                // Loop for each Domain
                foreach (Domain domain in domains) {
                    // Get Group Node Name
                    string key = string.Empty;
                    if (domain.GetType() == typeof(DomainCodedValue)) {
                        key = Resources.TEXT_CODED_VALUE;
                    }
                    else if (domain.GetType() == typeof(DomainRange)) {
                        key = Resources.TEXT_RANGE_DOMAIN;
                    }
                    if (string.IsNullOrEmpty(key)) { continue; }

                    // Get Group Node (create if it does not exist)
                    TreeNodeGroup treeNodeGroup = null;
                    foreach (TreeNodeGroup group in treeNode.Nodes) {
                        if (group.Type == domain.GetType()) {
                            treeNodeGroup = group;
                            break;
                        }
                    }
                    if (treeNodeGroup == null) {
                        treeNodeGroup = new TreeNodeGroup(domain.GetType());
                        treeNodeGroup.ImageKey = Catalog.FOLDER_CLOSED;
                        treeNodeGroup.SelectedImageKey = Catalog.FOLDER_CLOSED;
                        treeNodeGroup.Text = key;
                        treeNode.Nodes.Add(treeNodeGroup);
                    }

                    // Create New Dataset Node
                    TreeNode treeNodeDomain = this.CreateCatalogNode(treeNodeGroup, domain);
                }

                // Expand Geodatabase Node
                treeNode.Expand();

                // Traditional Text Sort
                this.treeView1.Sort();
            }
            else if (this.buttonItemAlphabetical.Checked) {
                // Loop for each Dataset
                foreach (Dataset dataset in datasets) {
                    // Create New Dataset Node
                    TreeNode treeNodeDataset = this.CreateCatalogNode(treeNode, dataset);

                    if (dataset is ObjectClass) {
                        // Cast to ObjectClass
                        ObjectClass objectClass = (ObjectClass)dataset;

                        // Get and Add Subtypes
                        List<Subtype> subtypes = objectClass.GetSubtypes();
                        foreach (Subtype subtype in subtypes) {
                            TreeNode treeNodeSubtype = this.CreateCatalogNode(treeNode, subtype);
                        }
                    }
                }

                // Loop for each Domain
                foreach (Domain domain in domains) {
                    TreeNode treeNodeDomain = this.CreateCatalogNode(treeNode, domain);
                }

                // Expand Geodatabase Node
                treeNode.Expand();

                // Traditional Text Sort
                this.treeView1.Sort();
            }

            // Reselect Previous EsriTable (if any)
            if (table != null) {
                if (this.treeView1.Nodes.Count == 1) {
                    //TreeNode treeNode = this.treeView1.Nodes[0];
                    TreeNode treeNodeFind = this.FindNode(treeNode, table);
                    if (treeNodeFind != null) {
                        treeNodeFind.EnsureVisible();
                        treeNodeFind.TreeView.SelectedNode = treeNodeFind;
                    }
                }
            }

            // End TreeView Update
            this.treeView1.EndUpdate();
        }
        private void ToolBar_ButtonClick(object sender, ToolBarItemEventArgs e) {
            try {
                if (e.Item == this.buttonItemCatalog ||
                    e.Item == this.buttonItemCategorized ||
                    e.Item == this.buttonItemAlphabetical) {
                    ButtonItem item = (ButtonItem)e.Item;
                    if (item.Checked) { return; }
                    this.buttonItemCatalog.Checked = (e.Item == this.buttonItemCatalog);
                    this.buttonItemCategorized.Checked = (e.Item == this.buttonItemCategorized);
                    this.buttonItemAlphabetical.Checked = (e.Item == this.buttonItemAlphabetical);
                    this.LoadModel();
                }
                else if (e.Item == this.buttonItemRefresh) {
                    this.LoadModel();
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void TreeView_AfterCollapse(object sender, TreeViewEventArgs e) {
            try {
                if (e.Node is TreeNodeGroup) {
                    e.Node.ImageKey = Catalog.FOLDER_CLOSED;
                    e.Node.SelectedImageKey = Catalog.FOLDER_CLOSED;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void TreeView_AfterExpand(object sender, TreeViewEventArgs e) {
            try {
                if (e.Node is TreeNodeGroup) {
                    e.Node.ImageKey = Catalog.FOLDER_OPENED;
                    e.Node.SelectedImageKey = Catalog.FOLDER_OPENED;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e) {
            try {
                if (e.Node is TreeNodeGeodatabase) {
                    TreeNodeGeodatabase treeNodeGeodatabase = (TreeNodeGeodatabase)e.Node;
                    EsriModel model = treeNodeGeodatabase.Model;
                    model.SelectElements(false);
                }
                else if (e.Node is TreeNodeTable) {
                    TreeNodeTable treeNodeTable = (TreeNodeTable)e.Node;
                    EsriTable table = treeNodeTable.Table;
                    EsriModel model = (EsriModel)table.Container;
                    model.SelectElements(false);
                    table.Selected = true;
                }
                else if (e.Node is TreeNodeGroup) { }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void TreeView_KeyDown(object sender, KeyEventArgs e) {
            try {
                switch (e.KeyData) {
                    case Keys.Enter:
                        // Handle Event
                        e.Handled = true;

                        // Get Table
                        TreeNode treeNode = this.treeView1.SelectedNode;
                        if (treeNode == null) { return; }
                        TreeNodeTable treeNodeTable = treeNode as TreeNodeTable;
                        if (treeNodeTable == null) { return; }
                        EsriTable table = treeNodeTable.Table;
                        if (table == null) { return; }

                        // Scroll to Table
                        this._model.ScrollToElement(table);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            try {
                // Get Double Clicked Node (and Table)
                if (e == null) { return; }
                TreeNode treeNode = e.Node;
                if (treeNode == null) { return; }
                TreeNodeTable treeNodeTable = treeNode as TreeNodeTable;
                if (treeNodeTable == null) { return; }
                EsriTable table = treeNodeTable.Table;
                if (table == null) { return; }

                // Scroll to Table
                this._model.ScrollToElement(table);
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        //
        // CLASSES
        // 
        private class TreeNodeGeodatabase : TreeNode {
            private readonly EsriModel m_model = null;
            //
            // CONSTRUCTOR
            //
            public TreeNodeGeodatabase(EsriModel model) : base() {
                this.m_model = model;
            }
            //
            // PROPERTIES
            //
            public EsriModel Model {
                get { return this.m_model; }
            }
        }
        private class TreeNodeTable : TreeNode {
            private readonly EsriTable m_table = null;
            //
            // CONSTRUCTOR
            //
            public TreeNodeTable(EsriTable table) : base() {
                this.m_table = table;
            }
            //
            // PROPERTIES
            //
            public EsriTable Table {
                get { return this.m_table; }
            }
        }
        private class TreeNodeGroup : TreeNode {
            private readonly Type m_type = null;
            //
            // CONSTRUCTOR
            //
            public TreeNodeGroup(Type type) : base() {
                this.m_type = type;
            }
            //
            // PROPERTIES
            //
            public Type Type {
                get { return this.m_type; }
            }
        }
    }
}

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
using System.Drawing;
using System.Windows.Forms;
using Crainiate.Diagramming;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.Geometry;

namespace ESRI.ArcGIS.Diagrammer {
    public partial class Palette : UserControl {
        private const string ANNOTATION_FEATURE_CLASS = "annotation_feature_class";
        private const string CODED_VALUE_DOMAIN = "coded_value_domain";
        private const string DIMENSION_FEATURE_CLASS = "dimension_feature_class";
        private const string FEATURE_DATASET = "feature_dataset";
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
        //
        // CONSTRUCTOR
        //
        public Palette() {
            //
            InitializeComponent();

            // Load Palette Image LIst
            this.listViewPalette.SmallImageList.Images.Add(Palette.ANNOTATION_FEATURE_CLASS, Resources.BITMAP_ANNOTATION_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.CODED_VALUE_DOMAIN, Resources.BITMAP_CODED_VALUE_DOMAIN);
            this.listViewPalette.SmallImageList.Images.Add(Palette.DIMENSION_FEATURE_CLASS, Resources.BITMAP_DIMENSION_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.FEATURE_DATASET, Resources.BITMAP_FEATURE_DATASET);
            this.listViewPalette.SmallImageList.Images.Add(Palette.GEOMETRIC_NETWORK, Resources.BITMAP_GEOMETRIC_NETWORK);
            this.listViewPalette.SmallImageList.Images.Add(Palette.MULTIPATCH_FEATURE_CLASS, Resources.BITMAP_MULTIPATCH_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.NETWORK_DATASET, Resources.BITMAP_NETWORK_DATASET);
            this.listViewPalette.SmallImageList.Images.Add(Palette.POINT_FEATURE_CLASS, Resources.BITMAP_POINT_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.POLYGON_FEATURE_CLASS, Resources.BITMAP_POLYGON_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.POLYLINE_FEATURE_CLASS, Resources.BITMAP_POLYLINE_FEATURE_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.RANGE_DOMAIN, Resources.BITMAP_RANGE_DOMAIN);
            this.listViewPalette.SmallImageList.Images.Add(Palette.RASTER_BAND, Resources.BITMAP_RASTER_BAND);
            this.listViewPalette.SmallImageList.Images.Add(Palette.RASTER_CATALOG, Resources.BITMAP_RASTER_CATALOG);
            this.listViewPalette.SmallImageList.Images.Add(Palette.RASTER_DATASET, Resources.BITMAP_RASTER_DATASET);
            this.listViewPalette.SmallImageList.Images.Add(Palette.RELATIONSHIP_CLASS, Resources.BITMAP_RELATIONSHIP_CLASS);
            this.listViewPalette.SmallImageList.Images.Add(Palette.SUBTYPE, Resources.BITMAP_SUBTYPE);
            this.listViewPalette.SmallImageList.Images.Add(Palette.TABLE, Resources.BITMAP_TABLE);
            this.listViewPalette.SmallImageList.Images.Add(Palette.TERRAIN, Resources.BITMAP_TERRAIN);
            this.listViewPalette.SmallImageList.Images.Add(Palette.TOPOLOGY, Resources.BITMAP_TOPOLOGY);

            // Create Palette Group
            ListViewGroup listViewGroupDomains = new ListViewGroup(Resources.TEXT_DOMAINS, HorizontalAlignment.Left);
            ListViewGroup listViewGroupFeatureClasses = new ListViewGroup(Resources.TEXT_FEATURECLASSES, HorizontalAlignment.Left);
            ListViewGroup listViewGroupRasters = new ListViewGroup(Resources.TEXT_RASTERS, HorizontalAlignment.Left);
            ListViewGroup listViewGroupDatasets = new ListViewGroup(Resources.TEXT_DATASETS, HorizontalAlignment.Left);
            ListViewGroup listViewGroupOther = new ListViewGroup(Resources.TEXT_OTHER, HorizontalAlignment.Left);
            this.listViewPalette.Groups.Add(listViewGroupDomains);
            this.listViewPalette.Groups.Add(listViewGroupFeatureClasses);
            this.listViewPalette.Groups.Add(listViewGroupRasters);
            this.listViewPalette.Groups.Add(listViewGroupDatasets);
            this.listViewPalette.Groups.Add(listViewGroupOther);

            // Add Column
            this.listViewPalette.Columns.Add(string.Empty, -2, HorizontalAlignment.Left);

            // Create Unsupport Dataset Font
            Font fontUnsupport = new Font(this.Font, FontStyle.Strikeout);

            // Add Palette Items
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_ANNOTATION }, Palette.ANNOTATION_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_CODED_VALUE }, Palette.CODED_VALUE_DOMAIN, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDomains));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_DIMENSION }, Palette.DIMENSION_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_FEATURE_DATASET }, Palette.FEATURE_DATASET, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDatasets));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_GEOMETRIC_NETWORK }, Palette.GEOMETRIC_NETWORK, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDatasets));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_MULTIPATCH }, Palette.MULTIPATCH_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_NETWORK }, Palette.NETWORK_DATASET, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDatasets));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_POINT }, Palette.POINT_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_POLYGON }, Palette.POLYGON_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_POLYLINE }, Palette.POLYLINE_FEATURE_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupFeatureClasses));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_RANGE }, Palette.RANGE_DOMAIN, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDomains));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_RASTER_BAND }, Palette.RASTER_BAND, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupRasters));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_RASTER_CATALOG }, Palette.RASTER_CATALOG, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupRasters));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_RASTER_DATASET }, Palette.RASTER_DATASET, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupRasters));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_RELATIONSHIP }, Palette.RELATIONSHIP_CLASS, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupOther));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_SUBTYPE }, Palette.SUBTYPE, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupOther));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_TABLE }, Palette.TABLE, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupOther));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_TERRAIN }, Palette.TERRAIN, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDatasets));
            this.listViewPalette.Items.Add(new ListViewItem(new string[] { Resources.TEXT_TOPOLOGY }, Palette.TOPOLOGY, SystemColors.ControlText, SystemColors.Window, this.Font, listViewGroupDatasets));

            // Adjust Column Width
            foreach (ColumnHeader column in this.listViewPalette.Columns) {
                column.Width = -1;
            }
        }
        //
        // PRIVATE METHODS
        //
        private void ListView_ItemDrag(object sender, ItemDragEventArgs e) {
            try {
                // Exit if nothing selected
                if (this.listViewPalette.SelectedItems.Count == 0) { return; }

                // 
                Element element = null;

                switch (this.listViewPalette.SelectedItems[0].ImageKey) {
                    case Palette.ANNOTATION_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTAnnotation);
                        break;
                    case Palette.CODED_VALUE_DOMAIN:
                        element = Factory.CreateDomain(typeof(DomainCodedValue));
                        break;
                    case Palette.DIMENSION_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTDimension);
                        break;
                    case Palette.FEATURE_DATASET:
                        element = Factory.CreateFeatureDataset();
                        break;
                    case Palette.GEOMETRIC_NETWORK:
                        element = Factory.CreateGeometricNetwork();
                        break;
                    case Palette.MULTIPATCH_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryMultiPatch);
                        break;
                    case Palette.NETWORK_DATASET:
                        element = Factory.CreateNetwork();
                        break;
                    case Palette.POINT_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPoint);
                        break;
                    case Palette.POLYGON_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPolygon);
                        break;
                    case Palette.POLYLINE_FEATURE_CLASS:
                        element = Factory.CreateFeatureClass(ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPolyline);
                        break;
                    case Palette.RANGE_DOMAIN:
                        element = Factory.CreateDomain(typeof(DomainRange));
                        break;
                    case Palette.RASTER_BAND:
                        element = Factory.CreateRasterBand();
                        break;
                    case Palette.RASTER_CATALOG:
                        element = Factory.CreateRasterCatalog();
                        break;
                    case Palette.RASTER_DATASET:
                        element = Factory.CreateRasterDataset();
                        break;
                    case Palette.RELATIONSHIP_CLASS:
                        element = Factory.CreateRelationshipClass();
                        break;
                    case Palette.SUBTYPE:
                        element = Factory.CreateSubtype();
                        break;
                    case Palette.TABLE:
                        element = Factory.CreateObjectClass();
                        break;
                    case Palette.TERRAIN:
                        element = Factory.CreateTerrain();
                        break;
                    case Palette.TOPOLOGY:
                        element = Factory.CreateTopology();
                        break;
                    default:
                        break;
                }
                if (element == null) { return; }

                // Add Element to Drag and Drop DataObject
                this.listViewPalette.DoDragDrop(element, DragDropEffects.All);
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
        private void ListView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e) {
            try {
                string title = e.Item.Text;
                string toolTip = string.Empty;

                switch (e.Item.ImageKey) {
                    case Palette.ANNOTATION_FEATURE_CLASS:
                        toolTip = Resources.TEXT_ANNOTATION_DESCRIPTION;
                        break;
                    case Palette.CODED_VALUE_DOMAIN:
                        toolTip = Resources.TEXT_CODED_VALUE_DESCRIPTION;
                        break;
                    case Palette.DIMENSION_FEATURE_CLASS:
                        toolTip = Resources.TEXT_DIMENSION_DESCRIPTION;
                        break;
                    case Palette.FEATURE_DATASET:
                        toolTip = Resources.TEXT_FEATURE_DATASET_DESCRIPTION;
                        break;
                    case Palette.GEOMETRIC_NETWORK:
                        toolTip = Resources.TEXT_GEOMETRIC_NETWORK_DESCRIPTION;
                        break;
                    case Palette.MULTIPATCH_FEATURE_CLASS:
                        toolTip = Resources.TEXT_MULTIPATCH_DESCRIPTION;
                        break;
                    case Palette.NETWORK_DATASET:
                        toolTip = Resources.TEXT_NETWORK_DESCRIPTION;
                        break;
                    case Palette.POINT_FEATURE_CLASS:
                        toolTip = Resources.TEXT_POINT_DESCRIPTION;
                        break;
                    case Palette.POLYGON_FEATURE_CLASS:
                        toolTip = Resources.TEXT_POLYGON_DESCRIPTION;
                        break;
                    case Palette.POLYLINE_FEATURE_CLASS:
                        toolTip = Resources.TEXT_POLYLINE_DESCRIPTION;
                        break;
                    case Palette.RANGE_DOMAIN:
                        toolTip = Resources.TEXT_RANGE_DESCRIPTION;
                        break;
                    case Palette.RASTER_BAND:
                        toolTip = Resources.TEXT_RASTER_BAND_DESCRIPTION;
                        break;
                    case Palette.RASTER_CATALOG:
                        toolTip = Resources.TEXT_RASTER_CATALOG_DESCRIPTION;
                        break;
                    case Palette.RASTER_DATASET:
                        toolTip = Resources.TEXT_RASTER_DATASET_DESCRIPTION;
                        break;
                    case Palette.RELATIONSHIP_CLASS:
                        toolTip = Resources.TEXT_RELATIONSHIP_DESCRIPTION;
                        break;
                    case Palette.SUBTYPE:
                        toolTip = Resources.TEXT_SUBTYPE_DESCRIPTION;
                        break;
                    case Palette.TABLE:
                        toolTip = Resources.TEXT_TABLE_DESCRIPTION;
                        break;
                    case Palette.TERRAIN:
                        toolTip = Resources.TEXT_TERRAIN_DESCRIPTION;
                        break;
                    case Palette.TOPOLOGY:
                        toolTip = Resources.TEXT_TOPOLOGY_DESCRIPTION;
                        break;
                    default:
                        break;
                }

                string toolTip2 = string.Empty;
                bool needNewLine = false;
                for (int i = 0; i < toolTip.Length; i++) {
                    string x = toolTip.Substring(i, 1);
                    if (needNewLine) {
                        if (x == " ") {
                            toolTip2 += Environment.NewLine;
                            needNewLine = false;
                            continue;
                        }
                    }
                    else {
                        if ((i != 0) && (i % 80 == 0)) {
                            needNewLine = true;
                        }
                    }
                    toolTip2 += x;
                }

                // Update Tooltip
                if (this.toolTip1.GetToolTip(this.listViewPalette) != toolTip2) {
                    this.toolTip1.SetToolTip(this.listViewPalette, toolTip2);
                }
                if (this.toolTip1.ToolTipTitle != title) {
                    this.toolTip1.ToolTipTitle = title;
                }
            }
            catch (Exception ex) {
                ExceptionDialog.HandleException(ex);
            }
        }
    }
}

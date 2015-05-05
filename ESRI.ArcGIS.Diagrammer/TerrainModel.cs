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
using System.ComponentModel;
using System.Drawing;
using Crainiate.Diagramming;
using Crainiate.ERM4.Layouts;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public partial class TerrainModel : EsriModel {
        private Terrain _terrain = null;
        //
        // CONSTRUCTOR
        // 
        public TerrainModel() {
            InitializeComponent();

            this.AllowDrop = false;
            this.Runtime = new TerrainRuntime();
            this.ElementRemoved += new ElementsEventHandler(this.Model_ElementRemoved);
            this.ElementInvalid += new ElementEventHandler(this.Model_ElementInvalid);
            this.ElementInserted += new ElementsEventHandler(this.Model_ElementInserted);
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public Terrain Terrain {
            get { return this._terrain; }
            set { this._terrain = value; }
        }
        [Browsable(false)]
        public override bool CanCut {
            get { return false; }
        }
        [Browsable(false)]
        public override bool CanCopy {
            get { return false; }
        }
        [Browsable(false)]
        public override bool CanPaste {
            get { return false; }
        }
        [Browsable(false)]
        public override Bitmap Icon {
            get { return Resources.BITMAP_TERRAIN; }
        }
        //
        // PUBLIC METHODS
        //
        public override void OpenModel() {
            // Exit if invalid
            if (this._terrain == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            this.Suspend();
            this.SuspendEvents = true;

            // Add GeometricNetwork Shape
            EsriShape<Terrain> shapeTerrain = new EsriShape<Terrain>(this._terrain);
            this.Shapes.Add(this.Shapes.CreateKey(), shapeTerrain);

            // Get Parent FeatureDataset
            Dataset parent = this._terrain.GetParent();
            if (parent != null) {
                FeatureDataset featureDataset = parent as FeatureDataset;
                if (featureDataset != null) {

                    // Add FeatureDataset Shape
                    EsriShape<FeatureDataset> shapeFeatureDataset = new EsriShape<FeatureDataset>(featureDataset);
                    this.Shapes.Add(this.Shapes.CreateKey(), shapeFeatureDataset);

                    // Add all Child FeatureClasses
                    foreach (Dataset dataset in featureDataset.GetChildren()) {
                        if (dataset.GetType() == typeof(FeatureClass)) {
                            // Add FetaureClass Shape
                            FeatureClass featureClass = (FeatureClass)dataset;
                            EsriShape<FeatureClass> shapeFeatureClass = new EsriShape<FeatureClass>(featureClass);
                            this.Shapes.Add(this.Shapes.CreateKey(), shapeFeatureClass);

                            // Add Link 
                            Arrow arrow = new Arrow();
                            arrow.BorderColor = ModelSettings.Default.DisabledLined;
                            arrow.DrawBackground = false;
                            Line line = new Line(shapeFeatureClass, shapeFeatureDataset);
                            line.BorderColor = ModelSettings.Default.DisabledLined;
                            line.End.AllowMove = false;
                            line.Start.Marker = arrow;
                            line.Start.AllowMove = false;
                            this.Lines.Add(this.Lines.CreateKey(), line);

                            foreach (TerrainDataSource terrainDataSource in this._terrain.TerrainDataSources) {
                                if (terrainDataSource.FeatureClassName == dataset.Name) {
                                    EsriLine<TerrainDataSource> line2 = new EsriLine<TerrainDataSource>(terrainDataSource, shapeTerrain, shapeFeatureClass);
                                    this.Lines.Add(this.Lines.CreateKey(), line2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Perform Layout
            this.ExecuteLayout(typeof(HierarchicalLayout), true);

            // Resume and Refresh Model
            this.SuspendEvents = false;
            this.Resume();
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Resume();
            }
            this.Refresh();
        }
        //
        // PRIVATE METHODS
        //
        private void Model_ElementInserted(object sender, ElementsEventArgs e) { }
        private void Model_ElementInvalid(object sender, ElementEventArgs e) {
            // Exit if GeometricNetwork Does not Exist
            if (this._terrain == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<TerrainDataSource> line = element as EsriLine<TerrainDataSource>;
            if (line == null) { return; }

            // Get Controller
            TerrainDataSource terrainDataSource = line.Parent;

            // Suspend Rule Events
            terrainDataSource.Suspend();

            // Get Start and End Elements
            Element elementStart = line.Start.Shape;
            Element elementEnd = line.End.Shape;

            // Update FeatureClass Name Property
            if (elementEnd == null) {
                terrainDataSource.FeatureClassName = string.Empty;
            }
            else if (elementEnd is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)elementEnd;
                FeatureClass featureClass = shape.Parent;
                terrainDataSource.FeatureClassName = featureClass.Name;
            }

            // Add if missing
            if (!this._terrain.TerrainDataSources.Contains(terrainDataSource)) {
                this._terrain.TerrainDataSources.Add(terrainDataSource);
            }

            // Resume Controller Events
            terrainDataSource.Resume();
        }
        private void Model_ElementRemoved(object sender, ElementsEventArgs e) {
            // Check Terrain
            if (this._terrain == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<TerrainDataSource> line = element as EsriLine<TerrainDataSource>;
            if (line == null) { return; }

            // Get Rule
            TerrainDataSource terrainDataSource = line.Parent;

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
            Terrain terrain = schemaModel.FindParent(terrainDataSource);
            if (terrain != null) {
                terrain.TerrainDataSources.Remove(terrainDataSource);
            }
        }
    }
}

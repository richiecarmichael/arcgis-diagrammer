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
    public partial class TopologyModel : EsriModel {
        private Topology _topology = null;
        //
        // CONSTRUCTOR
        // 
        public TopologyModel() {
            InitializeComponent();

            this.AllowDrop = false;
            this.Runtime = new TopologyRuntime();
            this.ElementRemoved += new ElementsEventHandler(this.Model_ElementRemoved);
            this.ElementInvalid += new ElementEventHandler(this.Model_ElementInvalid);
            this.ElementInserted += new ElementsEventHandler(this.Model_ElementInserted);
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public Topology Topology {
            get { return this._topology; }
            set { this._topology = value; }
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
            get { return Resources.BITMAP_TOPOLOGY; }
        }
        //
        // PUBLIC METHODS
        //
        public override void OpenModel() {
            // Exit if invalid
            if (this._topology == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            this.Suspend();
            this.SuspendEvents = true;

            // Add GeometricNetwork Shape
            EsriShape<Topology> shapeTopology = new EsriShape<Topology>(this._topology);
            this.Shapes.Add(base.Shapes.CreateKey(), shapeTopology);

            // Get Parent FeatureDataset
            Dataset parent = this._topology.GetParent();
            if (parent == null) { return; }
            FeatureDataset featureDataset = parent as FeatureDataset;
            if (featureDataset == null) { return; }

            // Add FeatureDataset Shape
            EsriShape<FeatureDataset> shapeFeatureDataset = new EsriShape<FeatureDataset>(featureDataset);
            this.Shapes.Add(base.Shapes.CreateKey(), shapeFeatureDataset);

            // Add all Child FeatureClasses
            foreach (Dataset dataset in featureDataset.GetChildren()) {
                if (dataset.GetType() == typeof(FeatureClass)) {
                    // Add FetaureClass Shape
                    FeatureClass featureClass = (FeatureClass)dataset;
                    EsriShape<FeatureClass> shapeFeatureClass = new EsriShape<FeatureClass>(featureClass);
                    this.Shapes.Add(base.Shapes.CreateKey(), shapeFeatureClass);

                    // Add Link 
                    Arrow arrow = new Arrow();
                    arrow.BorderColor = ModelSettings.Default.DisabledLined;
                    arrow.DrawBackground = false;
                    Line line = new Line(shapeFeatureClass, shapeFeatureDataset);
                    line.BorderColor = ModelSettings.Default.DisabledLined;
                    line.End.AllowMove = false;
                    line.Start.Marker = arrow;
                    line.Start.AllowMove = false;
                    this.Lines.Add(base.Lines.CreateKey(), line);

                    foreach (ControllerMembership controllerMembership in featureClass.ControllerMemberships) {
                        if (controllerMembership is TopologyControllerMembership) {
                            TopologyControllerMembership topologyControllerMembership = (TopologyControllerMembership)controllerMembership;
                            if (topologyControllerMembership.TopologyName == this._topology.Name) {
                                EsriLine<TopologyControllerMembership> line2 = new EsriLine<TopologyControllerMembership>(topologyControllerMembership, shapeTopology, shapeFeatureClass);
                                this.Lines.Add(base.Lines.CreateKey(), line2);
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
            if (this._topology == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<TopologyControllerMembership> line = element as EsriLine<TopologyControllerMembership>;
            if (line == null) { return; }

            // Get Controller
            TopologyControllerMembership controller = line.Parent;

            // Suspend Rule Events
            controller.Suspend();

            // Get Start and End Elements
            Element elementStart = line.Start.Shape;
            Element elementEnd = line.End.Shape;

            // Update GeometricNetwork Name Property
            if (elementStart == null) {
                controller.TopologyName = string.Empty;
            }
            else if (elementStart is EsriShape<Topology>) {
                EsriShape<Topology> shape = (EsriShape<Topology>)elementStart;
                Topology topology = shape.Parent;
                controller.TopologyName = topology.Name;
            }

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
            ObjectClass objectClass = schemaModel.FindParent(controller);
            if (objectClass != null) {
                objectClass.ControllerMemberships.Remove(controller);
            }

            if (elementEnd == null) { }
            else if (elementEnd is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> es = (EsriShape<FeatureClass>)elementEnd;
                FeatureClass featureClass = es.Parent;
                featureClass.ControllerMemberships.Add(controller);
            }

            // Resume Controller Events
            controller.Resume();
        }
        private void Model_ElementRemoved(object sender, ElementsEventArgs e) {
            // Check Topology
            if (this._topology == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<TopologyControllerMembership> line = element as EsriLine<TopologyControllerMembership>;
            if (line == null) { return; }

            // Get Rule
            TopologyControllerMembership controller = line.Parent;

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
            ObjectClass objectClass = schemaModel.FindParent(controller);
            if (objectClass != null) {
                objectClass.ControllerMemberships.Remove(controller);
            }
        }
    }
}

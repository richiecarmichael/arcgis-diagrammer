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
    public partial class GeometricNetworkModel : EsriModel {
        private GeometricNetwork _geometricNetwork = null;
        //
        // CONSTRUCTOR
        // 
        public GeometricNetworkModel() {
            InitializeComponent();
            this.AllowDrop = false;
            this.Runtime = new GeometricNetworkRuntime();
            this.ElementRemoved += new ElementsEventHandler(this.Model_ElementRemoved);
            this.ElementInvalid += new ElementEventHandler(this.Model_ElementInvalid);
            this.ElementInserted += new ElementsEventHandler(this.Model_ElementInserted);
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public GeometricNetwork GeometricNetwork {
            get { return this._geometricNetwork; }
            set { this._geometricNetwork = value; }
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
            get { return Resources.BITMAP_GEOMETRIC_NETWORK; }
        }
        //
        // PUBLIC METHODS
        //
        public override void OpenModel() {
            // Exit if invalid
            if (this._geometricNetwork == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                base.UndoList.Suspend();
            }
            base.Suspend();
            base.SuspendEvents = true;

            // Add GeometricNetwork Shape
            EsriShape<GeometricNetwork> shapeGN = new EsriShape<GeometricNetwork>(this._geometricNetwork);
            base.Shapes.Add(base.Shapes.CreateKey(), shapeGN);

            // Get Parent FeatureDataset
            Dataset parent = this._geometricNetwork.GetParent();
            if (parent == null) { return; }
            FeatureDataset featureDataset = parent as FeatureDataset;
            if (featureDataset == null) { return; }

            // Add FeatureDataset Shape
            EsriShape<FeatureDataset> shapeFD = new EsriShape<FeatureDataset>(featureDataset);
            base.Shapes.Add(base.Shapes.CreateKey(), shapeFD);

            // Add all Child FeatureClasses
            foreach (Dataset dataset in featureDataset.GetChildren()) {
                if (dataset.GetType() == typeof(FeatureClass)) {
                    // Add FetaureClass Shape
                    FeatureClass featureClass = (FeatureClass)dataset;
                    EsriShape<FeatureClass> shapeFC = new EsriShape<FeatureClass>(featureClass);
                    base.Shapes.Add(base.Shapes.CreateKey(), shapeFC);

                    // Add Link 
                    Arrow arrow = new Arrow();
                    arrow.BorderColor = ModelSettings.Default.DisabledLined;
                    arrow.DrawBackground = false;
                    Line line = new Line(shapeFC, shapeFD);
                    line.BorderColor = ModelSettings.Default.DisabledLined;
                    line.End.AllowMove = false;
                    line.Start.Marker = arrow;
                    line.Start.AllowMove = false;
                    base.Lines.Add(base.Lines.CreateKey(), line);

                    foreach (ControllerMembership controllerMembership in featureClass.ControllerMemberships) {
                        if (controllerMembership is GeometricNetworkControllerMembership) {
                            GeometricNetworkControllerMembership geometricNetworkControllerMembership = (GeometricNetworkControllerMembership)controllerMembership;
                            if (geometricNetworkControllerMembership.GeometricNetworkName == this._geometricNetwork.Name) {
                                EsriLine<GeometricNetworkControllerMembership> line2 = new EsriLine<GeometricNetworkControllerMembership>(geometricNetworkControllerMembership, shapeGN, shapeFC);
                                base.Lines.Add(base.Lines.CreateKey(), line2);
                            }
                        }
                    }
                }
            }

            // Perform Layout
            base.ExecuteLayout(typeof(HierarchicalLayout), true);

            // Resume and Refresh Model
            base.SuspendEvents = false;
            base.Resume();
            if (ModelSettings.Default.EnableUndoRedo) {
                base.UndoList.Resume();
            }
            base.Refresh();
        }
        //
        // PRIVATE METHODS
        //
        private void Model_ElementInserted(object sender, ElementsEventArgs e) { }
        private void Model_ElementInvalid(object sender, ElementEventArgs e) {
            // Exit if GeometricNetwork Does not Exist
            if (this._geometricNetwork == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<GeometricNetworkControllerMembership> line = element as EsriLine<GeometricNetworkControllerMembership>;
            if (line == null) { return; }

            // Get Controller
            GeometricNetworkControllerMembership controller = line.Parent;

            // Suspend Rule Events
            controller.Suspend();

            // Get Start and End Elements
            Element elementStart = line.Start.Shape;
            Element elementEnd = line.End.Shape;

            // Update GeometricNetwork Name Property
            if (elementStart == null) {
                controller.GeometricNetworkName = string.Empty;
            }
            else if (elementStart is EsriShape<GeometricNetwork>) {
                EsriShape<GeometricNetwork> shape = (EsriShape<GeometricNetwork>)elementStart;
                GeometricNetwork geometricNetwork = shape.Parent;
                controller.GeometricNetworkName = geometricNetwork.Name;
            }

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
            ObjectClass objectClass = schemaModel.FindParent(controller);
            if (objectClass != null) {
                objectClass.ControllerMemberships.Remove(controller);
            }

            if (elementEnd == null) {
                controller.AncillaryRoleFieldName = string.Empty;
                controller.EnabledFieldName = string.Empty;
            }
            else if (elementEnd is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> es = (EsriShape<FeatureClass>)elementEnd;
                FeatureClass featureClass = es.Parent;
                featureClass.ControllerMemberships.Add(controller);
            }

            // Resume Controller Events
            controller.Resume();
        }
        private void Model_ElementRemoved(object sender, ElementsEventArgs e) {
            // Check Relationship
            if (this._geometricNetwork == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<GeometricNetworkControllerMembership> line = element as EsriLine<GeometricNetworkControllerMembership>;
            if (line == null) { return; }

            // Get Rule
            GeometricNetworkControllerMembership controller = line.Parent;

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

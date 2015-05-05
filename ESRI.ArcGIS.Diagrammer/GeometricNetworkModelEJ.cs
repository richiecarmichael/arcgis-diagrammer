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
using Crainiate.Diagramming;
using Crainiate.ERM4.Layouts;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public partial class GeometricNetworkModelEJ : EsriModel {
        private GeometricNetwork _geometricNetwork = null;
        //
        // CONSTRUCTOR
        // 
        public GeometricNetworkModelEJ() {
            InitializeComponent();

            this.AllowDrop = false;
            this.Runtime = new GeometricNetworkRuntimeEJ();
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
            List<EsriShape<FeatureClass>> listFeatureClassFrom = new List<EsriShape<FeatureClass>>();
            List<EsriShape<FeatureClass>> listFeatureClassTo = new List<EsriShape<FeatureClass>>();
            List<EsriShape<Subtype>> listSubtypeFrom = new List<EsriShape<Subtype>>();
            List<EsriShape<Subtype>> listSubtypeTo = new List<EsriShape<Subtype>>();

            // Exit if invalid
            if (this._geometricNetwork == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            this.Suspend();
            this.SuspendEvents = true;

            // Get SchemaModel
            SchemaModel schemaModel = (SchemaModel)this._geometricNetwork.Container;

            // Get Parent FeatureDataset
            Dataset parent = this._geometricNetwork.GetParent();
            if (parent == null) { return; }
            FeatureDataset featureDataset = parent as FeatureDataset;
            if (featureDataset == null) { return; }

            // Add From and To FeatureDatasets
            EsriShape<FeatureDataset> featureDataset1 = new EsriShape<FeatureDataset>(featureDataset);
            EsriShape<FeatureDataset> featureDataset2 = new EsriShape<FeatureDataset>(featureDataset);
            this.Shapes.Add(this.Shapes.CreateKey(), featureDataset1);
            this.Shapes.Add(this.Shapes.CreateKey(), featureDataset2);

            // Add all Child FeatureClasses
            foreach (Dataset dataset in featureDataset.GetChildren()) {
                if (dataset.GetType() != typeof(FeatureClass)) { continue; }

                // Get FeatureClass
                FeatureClass featureClass = (FeatureClass)dataset;

                // Only allow Simle and Complex Edges
                switch (featureClass.FeatureType) {
                    case esriFeatureType.esriFTSimpleEdge:
                    case esriFeatureType.esriFTComplexEdge:
                    case esriFeatureType.esriFTSimpleJunction:
                        break;
                    default:
                        continue;
                }

                // Only continue if FeatureClass belongs to the GeometricNetwork
                bool participate = false;
                foreach (ControllerMembership controller in featureClass.ControllerMemberships) {
                    if (controller is GeometricNetworkControllerMembership) {
                        GeometricNetworkControllerMembership geometricNetworkControllerMembership = (GeometricNetworkControllerMembership)controller;
                        if (geometricNetworkControllerMembership.GeometricNetworkName == this._geometricNetwork.Name) {
                            participate = true;
                            break;
                        }
                    }
                }
                if (!participate) { continue; }

                // Get Subtypes
                List<Subtype> subtypes = featureClass.GetSubtypes();

                switch (featureClass.FeatureType){ //  (featureClass.CLSID) {
                    case esriFeatureType.esriFTSimpleEdge: //  EsriRegistry.CLASS_SIMPLEEDGE:
                    case esriFeatureType.esriFTComplexEdge: //  EsriRegistry.CLASS_COMPLEXEDGE:
                        // Add From FetaureClasses and Subtypes
                        EsriShape<FeatureClass> featureClass1 = new EsriShape<FeatureClass>(featureClass);
                        this.Shapes.Add(this.Shapes.CreateKey(), featureClass1);
                        listFeatureClassFrom.Add(featureClass1);

                        // Add Line from FeatureDataset to FeatureClass
                        Arrow arrow1 = new Arrow();
                        arrow1.BorderColor = ModelSettings.Default.DisabledLined;
                        arrow1.DrawBackground = false;
                        Line line1 = new Line(featureDataset1, featureClass1);
                        line1.End.AllowMove = false;
                        line1.End.Marker = arrow1;
                        line1.Start.AllowMove = false;
                        line1.BorderColor = ModelSettings.Default.DisabledLined;
                        this.Lines.Add(this.Lines.CreateKey(), line1);

                        // Add Subtypes and Link to FeatureClass
                        foreach (Subtype subtype in subtypes) {
                            EsriShape<Subtype> sub = new EsriShape<Subtype>(subtype);
                            this.Shapes.Add(this.Shapes.CreateKey(), sub);
                            listSubtypeFrom.Add(sub);
                            Arrow arrow3 = new Arrow();
                            arrow3.BorderColor = ModelSettings.Default.DisabledLined;
                            arrow3.DrawBackground = false;
                            Line line = new Line(featureClass1, sub);
                            line.End.AllowMove = false;
                            line.End.Marker = arrow3;
                            line.Start.AllowMove = false;
                            line.BorderColor = ModelSettings.Default.DisabledLined;
                            this.Lines.Add(this.Lines.CreateKey(), line);
                        }
                        break;
                    case esriFeatureType.esriFTSimpleJunction: // EsriRegistry.CLASS_SIMPLEJUNCTION:
                        // Add To FetaureClasses and Subtypes
                        EsriShape<FeatureClass> featureClass2 = new EsriShape<FeatureClass>(featureClass);
                        this.Shapes.Add(this.Shapes.CreateKey(), featureClass2);
                        listFeatureClassTo.Add(featureClass2);

                        // Add Line from FeatureDataset to FeatureClass
                        Arrow arrow2 = new Arrow();
                        arrow2.BorderColor = ModelSettings.Default.DisabledLined;
                        arrow2.DrawBackground = false;
                        Line line2 = new Line(featureClass2, featureDataset2);
                        line2.End.AllowMove = false;
                        line2.Start.AllowMove = false;
                        line2.Start.Marker = arrow2;
                        line2.BorderColor = ModelSettings.Default.DisabledLined;
                        this.Lines.Add(this.Lines.CreateKey(), line2);

                        // Add Subtyes and Link to FeatureClasses
                        foreach (Subtype subtype in subtypes) {
                            EsriShape<Subtype> sub = new EsriShape<Subtype>(subtype);
                            this.Shapes.Add(this.Shapes.CreateKey(), sub);
                            listSubtypeTo.Add(sub);
                            Arrow arrow4 = new Arrow();
                            arrow4.BorderColor = ModelSettings.Default.DisabledLined;
                            arrow4.DrawBackground = false;
                            Line line = new Line(sub, featureClass2);
                            line.End.AllowMove = false;
                            line.Start.Marker = arrow4;
                            line.Start.AllowMove = false;
                            line.BorderColor = ModelSettings.Default.DisabledLined;
                            this.Lines.Add(this.Lines.CreateKey(), line);
                        }
                        break;
                }
            }

            // Loop Through All Connectivity Rules
            foreach (ConnectivityRule connectivityRule in this._geometricNetwork.ConnectivityRules) {
                // Continue only if Edge Connectivity Rule
                if (!(connectivityRule is JunctionConnectivityRule)) { continue; }

                // Get Edge Connectivity Rule
                JunctionConnectivityRule junctionConnectivityRule = (JunctionConnectivityRule)connectivityRule;

                // Origin Table
                EsriTable origin = schemaModel.FindObjectClassOrSubtype(
                    junctionConnectivityRule.EdgeClassID,
                    junctionConnectivityRule.EdgeSubtypeCode);

                // Destination Table
                EsriTable destination = schemaModel.FindObjectClassOrSubtype(
                    junctionConnectivityRule.JunctionClassID,
                    junctionConnectivityRule.SubtypeCode);

                // Origin and Destination Shapes
                Shape shapeOrigin = null;
                Shape shapeDestiantion = null;

                // Find Origin Shape in Diagram
                foreach (EsriShape<FeatureClass> f in listFeatureClassFrom) {
                    if (f.Parent == origin) {
                        shapeOrigin = f;
                        break;
                    }
                }
                if (shapeOrigin == null) {
                    foreach (EsriShape<Subtype> s in listSubtypeFrom) {
                        if (s.Parent == origin) {
                            shapeOrigin = s;
                            break;
                        }
                    }
                }

                // Find Destination Shape in Diagram
                foreach (EsriShape<FeatureClass> f in listFeatureClassTo) {
                    if (f.Parent == destination) {
                        shapeDestiantion = f;
                        break;
                    }
                }
                if (shapeDestiantion == null) {
                    foreach (EsriShape<Subtype> s in listSubtypeTo) {
                        if (s.Parent == destination) {
                            shapeDestiantion = s;
                            break;
                        }
                    }
                }

                // Skip if Origin and Destination Shapes not found
                if (shapeOrigin == null || shapeDestiantion == null) { continue; }

                EsriLine<JunctionConnectivityRule> line2 = new EsriLine<JunctionConnectivityRule>(junctionConnectivityRule, shapeOrigin, shapeDestiantion);
                this.Lines.Add(this.Lines.CreateKey(), line2);
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
        private void Model_ElementInserted(object sender, ElementsEventArgs e) {
            // Check GeometricNetwork
            if (this._geometricNetwork == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<JunctionConnectivityRule> line = element as EsriLine<JunctionConnectivityRule>;
            if (line == null) { return; }

            // Get Rule
            JunctionConnectivityRule rule = line.Parent;

            // Add Rule
            this._geometricNetwork.ConnectivityRules.Add(rule);
        }
        private void Model_ElementInvalid(object sender, ElementEventArgs e) {
            // Exit if GeometricNetwork Does not Exist
            if (this._geometricNetwork == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<JunctionConnectivityRule> line = element as EsriLine<JunctionConnectivityRule>;
            if (line == null) { return; }

            // Get EdgeConnectivityRule
            JunctionConnectivityRule rule = line.Parent;

            // Suspend Rule Events
            rule.Suspend();

            // Get Start and End Elements
            Element elementStart = line.Start.Shape;
            Element elementEnd = line.End.Shape;

            // Update Start Class/Subtype
            if (elementStart == null) {
                rule.EdgeClassID = -1;
                rule.EdgeSubtypeCode = -1;
            }
            else if (elementStart is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)elementStart;
                ObjectClass objectClass = shape.Parent;
                rule.EdgeClassID = objectClass.DSID;
                rule.EdgeSubtypeCode = 0;
            }
            else if (elementStart is EsriShape<Subtype>) {
                EsriShape<Subtype> shape = (EsriShape<Subtype>)elementStart;
                Subtype subtype = shape.Parent;
                ObjectClass objectClass = subtype.GetParent();
                if (objectClass == null) {
                    rule.EdgeClassID = -1;
                    rule.EdgeSubtypeCode = -1;
                }
                else {
                    rule.EdgeClassID = objectClass.DSID;
                    rule.EdgeSubtypeCode = subtype.SubtypeCode;
                }
            }

            // Update End Class/Subtype
            if (elementEnd == null) {
                rule.JunctionClassID = -1;
                rule.SubtypeCode = -1;
            }
            else if (elementEnd is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)elementEnd;
                ObjectClass objectClass = shape.Parent;
                rule.JunctionClassID = objectClass.DSID;
                rule.SubtypeCode = 0;
            }
            else if (elementEnd is EsriShape<Subtype>) {
                EsriShape<Subtype> shape = (EsriShape<Subtype>)elementEnd;
                Subtype subtype = shape.Parent;
                ObjectClass objectClass = subtype.GetParent();
                if (objectClass == null) {
                    rule.JunctionClassID = -1;
                    rule.SubtypeCode = -1;
                }
                else {
                    rule.JunctionClassID = objectClass.DSID;
                    rule.SubtypeCode = subtype.SubtypeCode;
                }
            }

            // Resume Rule Events
            rule.Resume();
        }
        private void Model_ElementRemoved(object sender, ElementsEventArgs e) {
            // Check GeometricNetwork
            if (this._geometricNetwork == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<JunctionConnectivityRule> line = element as EsriLine<JunctionConnectivityRule>;
            if (line == null) { return; }

            // Get Rule
            JunctionConnectivityRule rule = line.Parent;

            // Remove rule from relationship
            this._geometricNetwork.ConnectivityRules.Remove(rule);
        }
    }
}

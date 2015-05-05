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

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public partial class RelationshipModel : EsriModel {
        private RelationshipClass _relationshipClass = null;
        //
        // CONSTRUCTOR
        // 
        public RelationshipModel() {
            InitializeComponent();

            this.AllowDrop = false;
            this.Runtime = new RelationshipRuntime();
            this.ElementRemoved += new ElementsEventHandler(this.Model_ElementRemoved);
            this.ElementInvalid += new ElementEventHandler(this.Model_ElementInvalid);
            this.ElementInserted += new ElementsEventHandler(this.Model_ElementInserted);
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public RelationshipClass RelationshipClass {
            get { return this._relationshipClass; }
            set { this._relationshipClass = value; }
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
            get { return Resources.BITMAP_RELATIONSHIP_CLASS; }
        }
        //
        // PUBLIC METHODS
        //
        public override void OpenModel() {
            EsriShape<ObjectClass> orig = null;
            EsriShape<ObjectClass> dest = null;
            List<EsriShape<Subtype>> originSubtypes = new List<EsriShape<Subtype>>();
            List<EsriShape<Subtype>> destinationSubtypes = new List<EsriShape<Subtype>>();

            // Exit if invalid
            if (this._relationshipClass == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            this.Suspend();
            this.SuspendEvents = true;

            // Get Schema Model
            SchemaModel schemaModel = (SchemaModel)this._relationshipClass.Container;

            // Get Origin and Destination ObjectClasses
            ObjectClass objectClassOrig = schemaModel.FindObjectClass(this._relationshipClass.OriginClassName);
            ObjectClass objectClassDest = schemaModel.FindObjectClass(this._relationshipClass.DestinationClassName);

            // Add ObjectClasses
            orig = new EsriShape<ObjectClass>(objectClassOrig);
            dest = new EsriShape<ObjectClass>(objectClassDest);
            this.Shapes.Add(this.Shapes.CreateKey(), orig);
            this.Shapes.Add(this.Shapes.CreateKey(), dest);

            // Get Subtypes
            List<Subtype> subtypesOrig = objectClassOrig.GetSubtypes();
            List<Subtype> subtypesDest = objectClassDest.GetSubtypes();

            // Add Subtypes
            foreach (Subtype subtype in subtypesOrig) {
                EsriShape<Subtype> sub = new EsriShape<Subtype>(subtype);
                this.Shapes.Add(this.Shapes.CreateKey(), sub);

                Arrow arrow = new Arrow();
                arrow.BorderColor = ModelSettings.Default.DisabledLined;
                arrow.DrawBackground = false;

                Line line = new Line(orig, sub);
                line.BorderColor = ModelSettings.Default.DisabledLined;
                line.End.AllowMove = false;
                line.End.Marker = arrow;
                line.Start.AllowMove = false;
                this.Lines.Add(this.Lines.CreateKey(), line);

                originSubtypes.Add(sub);
            }
            foreach (Subtype subtype in subtypesDest) {
                EsriShape<Subtype> sub = new EsriShape<Subtype>(subtype);
                this.Shapes.Add(this.Shapes.CreateKey(), sub);

                Arrow arrow = new Arrow();
                arrow.BorderColor = ModelSettings.Default.DisabledLined;
                arrow.DrawBackground = false;

                Line line = new Line(sub, dest);
                line.BorderColor = ModelSettings.Default.DisabledLined;
                line.End.AllowMove = false;
                line.Start.AllowMove = false;
                line.Start.Marker = arrow;
                this.Lines.Add(this.Lines.CreateKey(), line);

                destinationSubtypes.Add(sub);
            }

            // Get Rules
            List<RelationshipRule> rules = this._relationshipClass.RelationshipRules;
            foreach (RelationshipRule rule in rules) {
                Shape shapeOrign = null;
                if (subtypesOrig.Count == 0) {
                    shapeOrign = orig;
                }
                else {
                    foreach (EsriShape<Subtype> subtype in originSubtypes) {
                        if (subtype.Parent.SubtypeCode == rule.OriginSubtype) {
                            shapeOrign = subtype;
                            break;
                        }
                    }
                }

                Shape shapeDestination = null;
                if (subtypesDest.Count == 0) {
                    shapeDestination = dest;
                }
                else {
                    foreach (EsriShape<Subtype> subtype in destinationSubtypes) {
                        if (subtype.Parent.SubtypeCode == rule.DestinationSubtype) {
                            shapeDestination = subtype;
                            break;
                        }
                    }
                }
                if (shapeOrign == null || shapeDestination == null) { continue; }
                //
                EsriLine<RelationshipRule> line = new EsriLine<RelationshipRule>(rule, shapeOrign, shapeDestination);
                this.Lines.Add(this.Lines.CreateKey(), line);
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
            // Check Relationship
            if (this._relationshipClass == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<RelationshipRule> line = element as EsriLine<RelationshipRule>;
            if (line == null) { return; }

            // Get Rule
            RelationshipRule rule = line.Parent;

            // Add Rule
            this._relationshipClass.RelationshipRules.Add(rule);
        }
        private void Model_ElementInvalid(object sender, ElementEventArgs e) {
            // Check Relationship
            if (this._relationshipClass == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<RelationshipRule> line = element as EsriLine<RelationshipRule>;
            if (line == null) { return; }

            // Get Rule
            RelationshipRule rule = line.Parent;

            // Suspend Rule Events
            rule.Suspend();

            //
            Element elementStart = line.Start.Shape;
            Element elementEnd = line.End.Shape;

            // Update Origin Class/Subtype
            if (elementStart == null) {
                rule.OriginClass = -1;
                rule.OriginSubtype = -1;
            }
            else if (elementStart is EsriShape<ObjectClass>) {
                EsriShape<ObjectClass> shape = (EsriShape<ObjectClass>)elementStart;
                ObjectClass objectClass = shape.Parent;
                rule.OriginClass = objectClass.DSID;
                rule.OriginSubtype = 0;
            }
            else if (elementStart is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)elementStart;
                FeatureClass featureClass = shape.Parent;
                rule.OriginClass = featureClass.DSID;
                rule.OriginSubtype = 0;
            }
            else if (elementStart is EsriShape<Subtype>) {
                EsriShape<Subtype> shape = (EsriShape<Subtype>)elementStart;
                Subtype subtype = shape.Parent;
                ObjectClass objectClass = subtype.GetParent();
                if (objectClass != null) {
                    rule.OriginClass = objectClass.DSID;
                    rule.OriginSubtype = subtype.SubtypeCode;
                }
            }

            // Update Destination Class/Subtype
            if (elementEnd == null) {
                rule.DestinationClass = -1;
                rule.DestinationSubtype = -1;
            }
            else if (elementEnd is EsriShape<ObjectClass>) {
                EsriShape<ObjectClass> shape = (EsriShape<ObjectClass>)elementEnd;
                ObjectClass objectClass = shape.Parent;
                rule.DestinationClass = objectClass.DSID;
                rule.DestinationSubtype = 0;
            }
            else if (elementEnd is EsriShape<FeatureClass>) {
                EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)elementEnd;
                FeatureClass featureClass = shape.Parent;
                rule.DestinationClass = featureClass.DSID;
                rule.DestinationSubtype = 0;
            }
            else if (elementEnd is EsriShape<Subtype>) {
                EsriShape<Subtype> shape = (EsriShape<Subtype>)elementEnd;
                Subtype subtype = shape.Parent;
                ObjectClass objectClass = subtype.GetParent();
                if (objectClass != null) {
                    rule.DestinationClass = objectClass.DSID;
                    rule.DestinationSubtype = subtype.SubtypeCode;
                }
            }

            // Resume Rule Events
            rule.Resume();
        }
        private void Model_ElementRemoved(object sender, ElementsEventArgs e) {
            // Check Relationship
            if (this._relationshipClass == null) { return; }

            // Get Element
            Element element = e.Value;
            if (element == null) { return; }
            EsriLine<RelationshipRule> line = element as EsriLine<RelationshipRule>;
            if (line == null) { return; }

            // Get Rule
            RelationshipRule rule = line.Parent;

            // Remove rule from relationship
            this._relationshipClass.RelationshipRules.Remove(rule);
        }
    }
}

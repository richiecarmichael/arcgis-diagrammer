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
using System.Drawing.Drawing2D;

using Crainiate.Diagramming;
using Crainiate.ERM4.Layouts;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Domain Model
    /// </summary>
    /// <remarks>
    /// Domain models represent a stylized domain-centric diagram. Edits are currently not supported.
    /// </remarks>
    [Serializable]
    public partial class DomainModel : EsriModel {
        private Domain m_domain = null;
        public DomainModel() {
            InitializeComponent();
            this.AllowDrop = false;
            this.Runtime = new DomainRuntime();
        }
        //
        // PROPERTIES
        //
        [Browsable(false)]
        public Domain Domain {
            get { return this.m_domain; }
            set { this.m_domain = value; }
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
        public override bool CanDelete {
            get { return false; }
        }
        [Browsable(false)]
        public override Bitmap Icon {
            get {
                if (this.m_domain is DomainCodedValue) {
                    return Resources.BITMAP_RANGE_DOMAIN;
                }
                else if (this.m_domain is DomainRange) {
                    return Resources.BITMAP_CODED_VALUE_DOMAIN;
                }
                return null;
            }
        }
        //
        // PRIVATE METHODS
        //
        public override void OpenModel() {
            // Exit if invalid
            if (this.m_domain == null) { return; }

            // Suspend Model
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Suspend();
            }
            this.Suspend();
            this.SuspendEvents = true;

            // Get Schema Model
            SchemaModel model = (SchemaModel)this.m_domain.Container;

            // Add ObjectClasses that use the Domain
            List<ObjectClass> objectClasses = model.GetObjectClasses();
            foreach (ObjectClass objectClass in objectClasses) {
                // Add Fields
                List<Field> fields = objectClass.GetFields();
                List<Subtype> subtypes = objectClass.GetSubtypes();

                // Check if Domain Used
                bool domainUsed = false;
                foreach (Field field in fields) {
                    if (field.Domain == this.m_domain.Name) {
                        domainUsed = true;
                        break;
                    }
                }

                if (!domainUsed) {
                    foreach (Subtype subtype in subtypes) {
                        // Loop for each subtype field
                        List<SubtypeField> subtypeFields = subtype.GetSubtypeFields();
                        foreach (SubtypeField subtypeField in subtypeFields) {
                            if (subtypeField.DomainName == this.m_domain.Name) {
                                domainUsed = true;
                                break;
                            }
                        }
                    }
                }

                // Skip this ObjectClass if Domain not used
                if (!domainUsed) { continue; }

                // Add ObjectClass
                Shape shapeObjectClass = null;
                if (objectClass.GetType() == typeof(ObjectClass)) {
                    shapeObjectClass = new EsriShape<ObjectClass>(objectClass);
                }
                else if (objectClass.GetType() == typeof(FeatureClass)) {
                    shapeObjectClass = new EsriShape<FeatureClass>((FeatureClass)objectClass);
                }
                this.Shapes.Add(this.Shapes.CreateKey(), shapeObjectClass);

                // Add Fields
                foreach (Field field in fields) {
                    if (field.Domain == this.m_domain.Name) {
                        // Create Field
                        EsriShape<Field> shapeField = new EsriShape<Field>(field);
                        this.Shapes.Add(this.Shapes.CreateKey(), shapeField);

                        // Create ObjectClass Link
                        Arrow arrow = new Arrow();
                        arrow.BorderColor = ModelSettings.Default.DisabledLined;
                        arrow.BorderStyle = DashStyle.Solid;
                        arrow.BorderWidth = 1f;
                        arrow.DrawBackground = false;

                        Line line = new Line(shapeObjectClass, shapeField);
                        line.BorderColor = ModelSettings.Default.DisabledLined;
                        line.BorderStyle = DashStyle.Solid;
                        line.BorderWidth = 1f;
                        line.Start.AllowMove = false;
                        line.End.AllowMove = false;
                        line.End.Marker = arrow;
                        this.Lines.Add(this.Lines.CreateKey(), line);
                    }
                }

                // Add Subtypes
                foreach (Subtype subtype in subtypes) {
                    //
                    EsriShape<Subtype> shapeSubtype = null;

                    List<SubtypeField> subtypeFields = subtype.GetSubtypeFields();
                    foreach (SubtypeField subtypeField in subtypeFields) {
                        if (subtypeField.DomainName == this.m_domain.Name) {
                            // Add Subtype
                            if (shapeSubtype == null) {
                                // Add Subtype Table
                                shapeSubtype = new EsriShape<Subtype>(subtype);
                                this.Shapes.Add(this.Shapes.CreateKey(), shapeSubtype);

                                // Add ObjectClass to Subtype Line
                                Arrow arrow = new Arrow();
                                arrow.BorderColor = ModelSettings.Default.DisabledLined;
                                arrow.BorderStyle = DashStyle.Solid;
                                arrow.BorderWidth = 1f;
                                arrow.DrawBackground = false;

                                Line line = new Line(shapeObjectClass, shapeSubtype);
                                line.BorderColor = ModelSettings.Default.DisabledLined;
                                line.BorderStyle = DashStyle.Solid;
                                line.BorderWidth = 1f;
                                line.Start.AllowMove = false;
                                line.End.AllowMove = false;
                                line.End.Marker = arrow;
                                this.Lines.Add(this.Lines.CreateKey(), line);
                            }

                            // Create SubtypeField
                            EsriShape<SubtypeField> shapeSubtypeField = new EsriShape<SubtypeField>(subtypeField);
                            this.Shapes.Add(this.Shapes.CreateKey(), shapeSubtypeField);

                            // Create SubtypeField Link
                            Arrow arrow2 = new Arrow();
                            arrow2.BorderColor = ModelSettings.Default.DisabledLined;
                            arrow2.BorderStyle = DashStyle.Solid;
                            arrow2.BorderWidth = 1f;
                            arrow2.DrawBackground = false;

                            Line line2 = new Line(shapeSubtype, shapeSubtypeField);
                            line2.BorderColor = ModelSettings.Default.DisabledLined;
                            line2.BorderStyle = DashStyle.Solid;
                            line2.BorderWidth = 1f;
                            line2.Start.AllowMove = false;
                            line2.End.AllowMove = false;
                            line2.End.Marker = arrow2;
                            this.Lines.Add(this.Lines.CreateKey(), line2);

                            break;
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
    }
}

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
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Crainiate.Diagramming;

namespace ESRI.ArcGIS.Diagrammer {
    public abstract class EsriCollectionEditor : CollectionEditor {
        //
        // CONSTRUCTOR
        //
        public EsriCollectionEditor(Type type) : base(type) { }
        //
        // PROPERTIES
        //
        public abstract string Title { get;}
        //
        // PROTECTED METHODS
        //
        protected override CollectionForm CreateCollectionForm() {
            // Create default editor form. Update title and listen for closing form closing event.
            CollectionForm collectionForm = base.CreateCollectionForm();
            collectionForm.Text = this.Title;
            collectionForm.Closing += new CancelEventHandler(this.CollectionForm_Closing);

            // Find PropertyGrid control. Update filter.
            PropertyGrid propertyGrid = this.FindPropertyGrid(collectionForm);
            propertyGrid.HelpBackColor = SystemColors.Window;
            propertyGrid.HelpVisible = true;
            propertyGrid.LineColor = System.Drawing.Color.Silver;
            propertyGrid.ToolbarVisible = false;

            // Return editor dialog.
            return collectionForm;
        }
        //
        // PRIVATE METHODS
        //
        private PropertyGrid FindPropertyGrid(Control control) {
            // Loops through all controls until the PropertyGrid is located.
            if (control is PropertyGrid) {
                return (PropertyGrid)control;
            }
            if (control.HasChildren) {
                foreach (Control control2 in control.Controls) {
                    PropertyGrid propertyGrid = this.FindPropertyGrid(control2);
                    if (propertyGrid != null) {
                        return propertyGrid;
                    }
                }
            }
            return null;
        }
        private void CollectionForm_Closing(object sender, CancelEventArgs e) {
            // Get Collection Form
            CollectionForm collectionForm = (CollectionForm)sender;

            // Get edited collection object
            object editValue = collectionForm.EditValue;

            // Check if collection edits have been OK'ed or Canceled
            DialogResult dialogResult = collectionForm.DialogResult;
            switch (dialogResult) {
                case DialogResult.OK:
                    // Get Schema Model
                    DiagrammerEnvironment d = DiagrammerEnvironment.Default;
                    SchemaModel schemaModel = d.SchemaModel;

                    // Suspect Model Draws
                    schemaModel.Suspend();

                    // Get TableItems Collection
                    TableItems tableItems = (TableItems)editValue;

                    // Store items in new List
                    List<TableItem> list = new List<TableItem>();
                    foreach (TableItem tableItem in tableItems) {
                        list.Add(tableItem);
                    }

                    // Clear TableItems Collection
                    tableItems.Clear();

                    // Re-add all items.
                    foreach (TableItem tableItem in list) {
                        // If the Table property is not set then use TableItems::Add to append the item.
                        // This will ensure that Table, GradientColor, Indent properties are correctly set.
                        // This will also reset the height of the TableItems collection.
                        if (tableItem.Indent == 0f) {
                            tableItems.Add(tableItem);
                        }
                        else {
                            ((IList)tableItems).Add(tableItem);
                        }
                    }

                    // Refresh Model
                    schemaModel.Resume();
                    schemaModel.Refresh();

                    break;
                case DialogResult.Cancel:
                default:
                    // Collection edits are canceled. No need to refresh.
                    break;
            }
        }
    }
}

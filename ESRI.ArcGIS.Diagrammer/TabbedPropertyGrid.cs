/*=============================================================================
 * 
 * Copyright © 2009 ESRI. All rights reserved. 
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
using System.Drawing;
using System.Windows.Forms;

namespace ESRI.ArcGIS.Diagrammer {
    public class TabbedPropertyGrid : PropertyGrid {
        public TabbedPropertyGrid(): base() {
            // Assign display properties
            this.HelpBackColor = SystemColors.Window;
            this.LineColor = System.Drawing.Color.Silver;
            this.ToolbarVisible = false;
        }
        public void SetParent(Form form) {
            // Catch null arguments
            if (form == null) {
                throw new ArgumentNullException("form");
            }

            // Set this property to intercept all events
            form.KeyPreview = true;

            // Listen for keydown event
            form.KeyDown += new KeyEventHandler(this.Form_KeyDown);
        }
        private void Form_KeyDown(object sender, KeyEventArgs e) {
            // Exit if cursor not in control
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position)) { return; }

            // Handle tab key
            if (e.KeyCode != Keys.Tab) { return; }
            e.Handled = true;
            e.SuppressKeyPress = true;

            // Get selected griditem
            GridItem gridItem = this.SelectedGridItem;
            if (gridItem == null) { return; }

            // Create a collection all visible child gridytems in propertygrid
            GridItem root = gridItem;
            while (root.GridItemType != GridItemType.Root) {
                root = root.Parent;
            }
            List<GridItem> gridItems = new List<GridItem>();
            this.FindItems(root, gridItems);

            // Get position of selected griditem in collection
            int index = gridItems.IndexOf(gridItem);

            // Select next griditem in collection
            this.SelectedGridItem = gridItems[++index];
        }
        private void FindItems(GridItem item, List<GridItem> gridItems) {
            switch (item.GridItemType) {
                case GridItemType.Root:
                case GridItemType.Category:
                    foreach (GridItem i in item.GridItems) {
                        this.FindItems(i, gridItems);
                    }
                    break;
                case GridItemType.Property:
                    gridItems.Add(item);
                    if (item.Expanded) {
                        foreach (GridItem i in item.GridItems) {
                            this.FindItems(i, gridItems);
                        }
                    }
                    break;
                case GridItemType.ArrayValue:
                    break;
            }
        }
    }
}

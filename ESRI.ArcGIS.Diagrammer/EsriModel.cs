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
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Crainiate.Diagramming;
using Crainiate.ERM4.Layouts;
using Crainiate.ERM4.Svg;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Model
    /// </summary>
    /// <remarks>
    /// The sub-classed model has been extended to support ESRI Workspace Documents as a data source
    /// </remarks>
    public partial class EsriModel : Model {
        private string _printerName = string.Empty;
        private string _printerPaperSize = string.Empty;
        private System.Drawing.Point _mouseStart;
        private System.Drawing.Point _scrollStart;
        private bool _panning = false;
        //
        // CONSTRUCTOR
        //
        public EsriModel() {
            InitializeComponent();

            // Suspend Layout
            this.SuspendLayout();

            // Change Settings
            this.AllowDrop = false;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(200, 200);
            this.DiagramSize = new Size(1000, 1000);
            this.Paged = false;

            // TODO Remove this eventually. It is probably called when any element is added.
            this.ElementInserted += new ElementsEventHandler(this.EsriModel_ElementInserted);

            // Resume Layout
            this.ResumeLayout(false);

            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Resume();
            }

            // Get Notified if Diagram Colors or Model Settings Changed 
            ColorSettings.Default.SettingsSaving += new SettingsSavingEventHandler(this.DiagramColorsChanged);
            ModelSettings.Default.SettingsSaving += new SettingsSavingEventHandler(this.ModelSettingsChanged);
        }
        //
        // PROPERTIES
        // 
        [Browsable(false)]
        public virtual bool CanCut {
            get { return this.GetCommand("cut"); }
        }
        [Browsable(false)]
        public virtual bool CanCopy {
            get { return this.GetCommand("copy"); }
        }
        [Browsable(false)]
        public virtual bool CanPaste {
            get {
                IDataObject dataObject = Clipboard.GetDataObject();
                return dataObject.GetDataPresent(typeof(Elements));
            }
        }
        [Browsable(false)]
        public virtual bool CanDelete {
            get { return this.GetCommand("delete"); }
        }
        [Browsable(false)]
        public virtual bool CanUndo {
            get { return this.GetCommand("undo"); }
        }
        [Browsable(false)]
        public virtual bool CanRedo {
            get { return this.GetCommand("redo"); }
        }
        [Browsable(false)]
        public virtual bool CanSelectAll {
            get { return this.Shapes.Count > 0; }
        }
        [Browsable(false)]
        public virtual Bitmap Icon {
            get { return null; }
        }
        //
        // PUBLIC METHODS
        //
        public virtual void OpenModel() { }
        public virtual void OpenModel(string filename) { }
        public void ExportModel(string filename, SaveFormat format) {
            if (format == SaveFormat.Svg) {
                // Suspend Model
                this.Suspend();
                this.SuspendEvents = true;
                if (ModelSettings.Default.EnableUndoRedo) {
                    this.UndoList.Suspend();
                }

                // Fix: Adjust size of table to be slightly bigger than the "HeadingHeight"
                // TODO: Remove when SVG Export Bug is fixed
                foreach (Element element in this.Shapes.Values) {
                    if (element is EsriTable) {
                        EsriTable table = (EsriTable)element;
                        if (table.Height <= table.HeadingHeight) {
                            table.Height = table.HeadingHeight + 1;
                        }
                    }
                }

                // Resume and Refresh Model
                if (ModelSettings.Default.EnableUndoRedo) {
                    this.UndoList.Resume();
                }
                this.SuspendEvents = false;
                this.Resume();
                this.Refresh();

                // Export Model to SVG
                SvgDocument document = new SvgDocument();
                foreach (Element element in this.Shapes.Values) {
                    Type type = element.GetType();
                    if (!document.Formatters.ContainsKey(type)) {
                        if (type.IsSubclassOf(typeof(EsriTable))) {
                            document.RegisterFormatter(type, typeof(TableFormatter));
                        }
                        else if (type.IsSubclassOf(typeof(Shape))) {
                            document.RegisterFormatter(type, typeof(ShapeFormatter));
                        }
                    }
                }
                foreach (Element element in this.Lines.Values) {
                    Type type = element.GetType();
                    if (!document.Formatters.ContainsKey(type)) {
                        if (type.IsSubclassOf(typeof(Link))) {
                            document.RegisterFormatter(type, typeof(LinkFormatter));
                        }
                    }
                }
                document.AddDiagram(this);
                document.Save(filename);
            }
            else {
                this.Save(filename, format);
            }
        }
        public void ExecuteLayout(Type type, bool resize) {
            bool suspend = this.Suspended;
            if (!suspend) {
                this.SuspendEvents = true;
                this.Suspend();
            }

            // Create Layout Graph
            Graph graph = new Graph();
            graph.AddDiagram(this);

            //
            Layout layout = null;

            if (type == typeof(TreeLayout)) {
                TreeLayout treeLayout = new TreeLayout();
                treeLayout.Direction = TreeLayoutSettings.Default.Direction;
                treeLayout.LevelDistance = TreeLayoutSettings.Default.LevelDistance;
                treeLayout.OrthogonalLayoutStyle = TreeLayoutSettings.Default.OrthogonalLayoutStyle;
                treeLayout.RootSelection = TreeLayoutSettings.Default.RootSelection;
                treeLayout.SiblingDistance = TreeLayoutSettings.Default.SiblingDistance;
                treeLayout.SubtreeDistance = TreeLayoutSettings.Default.SubtreeDistance;
                treeLayout.TreeDistance = TreeLayoutSettings.Default.TreeDistance;
                layout = treeLayout;
            }
            else if (type == typeof(OrthogonalLayout)) {
                OrthogonalLayout orthogonalLayout = new OrthogonalLayout();
                orthogonalLayout.ConnectedComponentDistance = OrthogonalLayoutSettings.Default.ConnectedComponentDistance;
                orthogonalLayout.CrossingQuality = OrthogonalLayoutSettings.Default.CrossingQuality;
                orthogonalLayout.Distance = OrthogonalLayoutSettings.Default.Distance;
                orthogonalLayout.Overhang = OrthogonalLayoutSettings.Default.Overhang;
                layout = orthogonalLayout;
            }
            else if (type == typeof(HierarchicalLayout)) {
                HierarchicalLayout hierarchicalLayout = new HierarchicalLayout();
                hierarchicalLayout.ConnectedComponentDistance = HierarchicalLayoutSettings.Default.ConnectedComponentDistance;
                hierarchicalLayout.Direction = HierarchicalLayoutSettings.Default.Direction;
                hierarchicalLayout.LayerDistance = HierarchicalLayoutSettings.Default.LayerDistance;
                hierarchicalLayout.ObjectDistance = HierarchicalLayoutSettings.Default.ObjectDistance;
                hierarchicalLayout.RespectLayer = HierarchicalLayoutSettings.Default.RespectLayer;
                layout = hierarchicalLayout;
            }
            else if (type == typeof(ForceDirectedLayout)) {
                ForceDirectedLayout forceDirectedLayout = new ForceDirectedLayout();
                forceDirectedLayout.ConnectedComponentDistance = ForcedDirectLayoutSettings.Default.ConnectedComponentDistance;
                forceDirectedLayout.Quality = ForcedDirectLayoutSettings.Default.Quality;
                forceDirectedLayout.Tuning = ForcedDirectLayoutSettings.Default.TuningType;
                layout = forceDirectedLayout;
            }
            else if (type == typeof(CircularLayout)) {
                CircularLayout circularLayout = new CircularLayout();
                circularLayout.CircleDistance = CircularLayoutSettings.Default.CircleDistance;
                circularLayout.ConnectedComponentDistance = CircularLayoutSettings.Default.ConnectedComponentDistance;
                circularLayout.LayerDistance = CircularLayoutSettings.Default.LayerDistance;
                circularLayout.ObjectDistance = CircularLayoutSettings.Default.ObjectDistance;
                layout = circularLayout;
            }

            //
            if (layout != null) {
                // Do Layout
                layout.DoLayout(graph);

                // Apply if Layout Succeeded
                if (layout.Status == LayoutStatus.Success) {
                    // Resize Diagram
                    RectangleF rectangle = graph.TotalArea();
                    rectangle.Inflate(50, 50);
                    if (resize) {
                        this.DiagramSize = rectangle.Size.ToSize();
                    }
                    graph.ScaleToFit(this.DiagramSize);

                    // Apply Layout
                    graph.Apply(this);
                }
            }

            if (!suspend) {
                this.Resume();
                this.SuspendEvents = false;
                this.Refresh();
            }
        }
        public void ZoomFullExtent() {
            float scaleX = 100f * base.Width / base.DiagramSize.Width;
            float scaleY = 100f * base.Height / base.DiagramSize.Height;
            float scale = 0.9f * Math.Min(scaleX, scaleY);
            this.Zoom = scale;
        }
        public void ZoomModel(float zoom) {
            this.SuspendEvents = true;
            this.Suspend();
            this.Zoom = zoom;
            this.Resume();
            this.SuspendEvents = false;
            this.Refresh();
        }
        //
        // PROTECTED EVENTS
        //
        private void DiagramColorsChanged(object sender, CancelEventArgs e) {
            // Suspend Model
            this.Suspend();

            // Get All Elements from Model
            foreach (Element element in base.Shapes.Values) {
                if (element is EsriTable) {
                    EsriTable table = (EsriTable)element;
                    table.RefreshColor();
                }
                else if (element is EsriShape<FeatureClass>) {
                    EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)element;
                    shape.BorderColor = ColorSettings.Default.FeatureClassColor;
                    shape.GradientColor = ColorSettings.Default.FeatureClassColor;
                }
                else if (element is EsriShape<GeometricNetwork>) {
                    EsriShape<GeometricNetwork> shape = (EsriShape<GeometricNetwork>)element;
                    shape.BorderColor = ColorSettings.Default.GeometricNetworkColor;
                    shape.GradientColor = ColorSettings.Default.GeometricNetworkColor;
                }
                else if (element is EsriShape<FeatureDataset>) {
                    EsriShape<FeatureDataset> shape = (EsriShape<FeatureDataset>)element;
                    shape.BorderColor = ColorSettings.Default.FeatureDatasetColor;
                    shape.GradientColor = ColorSettings.Default.FeatureDatasetColor;
                }
                else if (element.GetType() == typeof(EsriShape<DomainCodedValue>)) {
                    EsriShape<DomainCodedValue> shape = (EsriShape<DomainCodedValue>)element;
                    shape.BorderColor = ColorSettings.Default.CodedValueDomainColor;
                    shape.GradientColor = ColorSettings.Default.CodedValueDomainColor;
                }
                else if (element.GetType() == typeof(EsriShape<DomainRange>)) {
                    EsriShape<DomainRange> shape = (EsriShape<DomainRange>)element;
                    shape.BorderColor = ColorSettings.Default.RangeDomainColor;
                    shape.GradientColor = ColorSettings.Default.RangeDomainColor;
                }
                else if (element.GetType() == typeof(EsriShape<FeatureClass>)) {
                    EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)element;
                    shape.BorderColor = ColorSettings.Default.FeatureClassColor;
                    shape.GradientColor = ColorSettings.Default.FeatureClassColor;
                }
                else if (element.GetType() == typeof(EsriShape<ObjectClass>)) {
                    EsriShape<ObjectClass> shape = (EsriShape<ObjectClass>)element;
                    shape.BorderColor = ColorSettings.Default.ObjectClassColor;
                    shape.GradientColor = ColorSettings.Default.ObjectClassColor;
                }
                else if (element.GetType() == typeof(EsriShape<Subtype>)) {
                    EsriShape<Subtype> shape = (EsriShape<Subtype>)element;
                    shape.BorderColor = ColorSettings.Default.SubtypeColor;
                    shape.GradientColor = ColorSettings.Default.SubtypeColor;
                }
                else if (element.GetType() == typeof(EsriShape<Field>)) {
                    EsriShape<Field> shape = (EsriShape<Field>)element;
                    shape.BorderColor = ColorSettings.Default.FieldColor;
                    shape.GradientColor = ColorSettings.Default.FieldColor;
                }
                else if (element.GetType() == typeof(EsriShape<SubtypeField>)) {
                    EsriShape<SubtypeField> shape = (EsriShape<SubtypeField>)element;
                    shape.BorderColor = ColorSettings.Default.SubtypeFieldColor;
                    shape.GradientColor = ColorSettings.Default.SubtypeFieldColor;
                }
                else if (element is EsriShape<FeatureClass>) {
                    EsriShape<FeatureClass> shape = (EsriShape<FeatureClass>)element;
                    shape.BorderColor = ColorSettings.Default.FeatureClassColor;
                    shape.GradientColor = ColorSettings.Default.FeatureClassColor;
                }
                else if (element is EsriShape<GeometricNetwork>) {
                    EsriShape<GeometricNetwork> shape = (EsriShape<GeometricNetwork>)element;
                    shape.BorderColor = ColorSettings.Default.GeometricNetworkColor;
                    shape.GradientColor = ColorSettings.Default.GeometricNetworkColor;
                }
                else if (element is EsriShape<FeatureDataset>) {
                    EsriShape<FeatureDataset> shape = (EsriShape<FeatureDataset>)element;
                    shape.BorderColor = ColorSettings.Default.FeatureDatasetColor;
                    shape.GradientColor = ColorSettings.Default.FeatureDatasetColor;
                }
            }

            // Resume and Refresh Model
            this.Resume();
            this.Refresh();
        }
        //
        // PRIVATE METHODS
        //
        private void ModelSettingsChanged(object sender, CancelEventArgs e) {
            if (ModelSettings.Default.EnableUndoRedo) {
                this.UndoList.Resume();
            }
            else {
                this.UndoList.Suspend();
            }
        }
        private void EsriModel_ElementInserted(object sender, ElementsEventArgs e) {
            if (e == null) { return; }
            if (e.Value == null) { return; }
            Element element = e.Value;
            if (element is Table) {
                Table table = (Table)element;
                foreach (TableRow row in table.Rows) {
                    this.ResetTable(row, table);
                }
                foreach (TableGroup group in table.Groups) {
                    this.ResetTable(group, table);
                }
            }
        }
        private void ResetTable(TableItem item, Table table) {
            if (item is TableRow) {
                TableRow tableRow = (TableRow)item;
                if (tableRow is EsriTableRow) {
                    ((EsriTableRow)tableRow).SetTable2(table);
                }
            }
            else if (item is TableGroup) {
                TableGroup tableGroup = (TableGroup)item;
                if (tableGroup is EsriTableGroup) {
                    ((EsriTableGroup)tableGroup).SetTable2(table);
                }
                foreach (TableItem item2 in tableGroup.Groups) {
                    this.ResetTable(item2, table);
                }
                foreach (TableItem item3 in tableGroup.Rows) {
                    this.ResetTable(item3, table);
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Middle) {
                if (!this._panning) { return; }
                if (this.CurrentMouseElements.MouseStartElement == null) {
                    int x = this._scrollStart.X + (this._mouseStart.X - e.X);
                    int y = this._scrollStart.Y + (this._mouseStart.Y - e.Y);
                    if (x < 0) { x = 0; }
                    if (y < 0) { y = 0; }
                    this.AutoScrollPosition = new System.Drawing.Point(x, y);
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Middle) {
                if (this.CurrentMouseElements.MouseStartElement == null) {
                    this._panning = true;
                    this._mouseStart = e.Location;
                    this._scrollStart = new System.Drawing.Point(
                        Math.Abs(this.AutoScrollPosition.X),
                        Math.Abs(this.AutoScrollPosition.Y)
                        );
                    this.Cursor = Cursors.Hand;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Middle) {
                this._panning = false;
                this.Cursor = Cursors.Default;
            }
        }
    }
}

namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentTopology {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.topologyModel1 = new ESRI.ArcGIS.Diagrammer.TopologyModel();
            this.SuspendLayout();
            // 
            // topologyModel1
            // 
            this.topologyModel1.AutoScroll = true;
            this.topologyModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.topologyModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.topologyModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topologyModel1.DragElement = null;
            this.topologyModel1.GridColor = System.Drawing.Color.Silver;
            this.topologyModel1.GridSize = new System.Drawing.Size(20, 20);
            this.topologyModel1.Location = new System.Drawing.Point(0, 0);
            this.topologyModel1.Name = "topologyModel1";
            this.topologyModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.topologyModel1.Size = new System.Drawing.Size(298, 246);
            this.topologyModel1.TabIndex = 0;
            this.topologyModel1.Topology = null;
            this.topologyModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.topologyModel1.Zoom = 100F;
            // 
            // TabbedDocumentTopology
            // 
            this.Controls.Add(this.topologyModel1);
            this.Name = "TabbedDocumentTopology";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.TopologyModel topologyModel1;





    }
}

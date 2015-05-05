namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentTerrain {
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
            this.terrainModel1 = new ESRI.ArcGIS.Diagrammer.TerrainModel();
            this.SuspendLayout();
            // 
            // terrainModel1
            // 
            this.terrainModel1.AutoScroll = true;
            this.terrainModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.terrainModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.terrainModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terrainModel1.DragElement = null;
            this.terrainModel1.GridColor = System.Drawing.Color.Silver;
            this.terrainModel1.GridSize = new System.Drawing.Size(20, 20);
            this.terrainModel1.Location = new System.Drawing.Point(0, 0);
            this.terrainModel1.Name = "terrainModel1";
            this.terrainModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.terrainModel1.Size = new System.Drawing.Size(298, 246);
            this.terrainModel1.TabIndex = 0;
            this.terrainModel1.Terrain = null;
            this.terrainModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.terrainModel1.Zoom = 100F;
            // 
            // TabbedDocumentTerrain
            // 
            this.Controls.Add(this.terrainModel1);
            this.Name = "TabbedDocumentTerrain";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private Diagrammer.TerrainModel terrainModel1;





    }
}

namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentGeometricNetworkEJ {
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
            this.geometricNetworkModelEJ1 = new ESRI.ArcGIS.Diagrammer.GeometricNetworkModelEJ();
            this.SuspendLayout();
            // 
            // geometricNetworkModelEJ1
            // 
            this.geometricNetworkModelEJ1.AutoScroll = true;
            this.geometricNetworkModelEJ1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModelEJ1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModelEJ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geometricNetworkModelEJ1.DragElement = null;
            this.geometricNetworkModelEJ1.GeometricNetwork = null;
            this.geometricNetworkModelEJ1.GridColor = System.Drawing.Color.Silver;
            this.geometricNetworkModelEJ1.GridSize = new System.Drawing.Size(20, 20);
            this.geometricNetworkModelEJ1.Location = new System.Drawing.Point(0, 0);
            this.geometricNetworkModelEJ1.Name = "geometricNetworkModelEJ1";
            this.geometricNetworkModelEJ1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.geometricNetworkModelEJ1.Size = new System.Drawing.Size(298, 246);
            this.geometricNetworkModelEJ1.TabIndex = 0;
            this.geometricNetworkModelEJ1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.geometricNetworkModelEJ1.Zoom = 100F;
            // 
            // TabbedDocumentGeometricNetworkEJ
            // 
            this.Controls.Add(this.geometricNetworkModelEJ1);
            this.Name = "TabbedDocumentGeometricNetworkEJ";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.GeometricNetworkModelEJ geometricNetworkModelEJ1;

    }
}

namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentGeometricNetworkEE {
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
            this.geometricNetworkModelEE1 = new ESRI.ArcGIS.Diagrammer.GeometricNetworkModelEE();
            this.SuspendLayout();
            // 
            // geometricNetworkModelEE1
            // 
            this.geometricNetworkModelEE1.AutoScroll = true;
            this.geometricNetworkModelEE1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModelEE1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModelEE1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geometricNetworkModelEE1.DragElement = null;
            this.geometricNetworkModelEE1.GeometricNetwork = null;
            this.geometricNetworkModelEE1.GridColor = System.Drawing.Color.Silver;
            this.geometricNetworkModelEE1.GridSize = new System.Drawing.Size(20, 20);
            this.geometricNetworkModelEE1.Location = new System.Drawing.Point(0, 0);
            this.geometricNetworkModelEE1.Name = "geometricNetworkModelEE1";
            this.geometricNetworkModelEE1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.geometricNetworkModelEE1.Size = new System.Drawing.Size(298, 246);
            this.geometricNetworkModelEE1.TabIndex = 0;
            this.geometricNetworkModelEE1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.geometricNetworkModelEE1.Zoom = 100F;
            // 
            // TabbedDocumentGeometricNetworkEE
            // 
            this.Controls.Add(this.geometricNetworkModelEE1);
            this.Name = "TabbedDocumentGeometricNetworkEE";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.GeometricNetworkModelEE geometricNetworkModelEE1;

    }
}

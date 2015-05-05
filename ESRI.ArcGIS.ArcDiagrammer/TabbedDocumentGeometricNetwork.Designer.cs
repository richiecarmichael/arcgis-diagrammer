namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentGeometricNetwork {
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
            this.geometricNetworkModel1 = new ESRI.ArcGIS.Diagrammer.GeometricNetworkModel();
            this.SuspendLayout();
            // 
            // geometricNetworkModel1
            // 
            this.geometricNetworkModel1.AutoScroll = true;
            this.geometricNetworkModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.geometricNetworkModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geometricNetworkModel1.DragElement = null;
            this.geometricNetworkModel1.GeometricNetwork = null;
            this.geometricNetworkModel1.GridColor = System.Drawing.Color.Silver;
            this.geometricNetworkModel1.GridSize = new System.Drawing.Size(20, 20);
            this.geometricNetworkModel1.Location = new System.Drawing.Point(0, 0);
            this.geometricNetworkModel1.Name = "geometricNetworkModel1";
            this.geometricNetworkModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.geometricNetworkModel1.Size = new System.Drawing.Size(298, 246);
            this.geometricNetworkModel1.TabIndex = 0;
            this.geometricNetworkModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.geometricNetworkModel1.Zoom = 100F;
            // 
            // TabbedDocumentGeometricNetwork
            // 
            this.Controls.Add(this.geometricNetworkModel1);
            this.Name = "TabbedDocumentGeometricNetwork";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.GeometricNetworkModel geometricNetworkModel1;




    }
}

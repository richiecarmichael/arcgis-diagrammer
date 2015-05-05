namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentDomain {
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
            this.domainModel1 = new ESRI.ArcGIS.Diagrammer.DomainModel();
            this.SuspendLayout();
            // 
            // domainModel1
            // 
            this.domainModel1.AutoScroll = true;
            this.domainModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.domainModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.domainModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.domainModel1.Domain = null;
            this.domainModel1.DragElement = null;
            this.domainModel1.GridColor = System.Drawing.Color.Silver;
            this.domainModel1.GridSize = new System.Drawing.Size(20, 20);
            this.domainModel1.Location = new System.Drawing.Point(0, 0);
            this.domainModel1.Name = "domainModel1";
            this.domainModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.domainModel1.Size = new System.Drawing.Size(298, 246);
            this.domainModel1.TabIndex = 0;
            this.domainModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.domainModel1.Zoom = 100F;
            // 
            // TabbedDocumentDomain
            // 
            this.Controls.Add(this.domainModel1);
            this.Name = "TabbedDocumentDomain";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.DomainModel domainModel1;



    }
}

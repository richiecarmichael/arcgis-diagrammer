namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentRelationship {
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
            this.relationshipModel1 = new ESRI.ArcGIS.Diagrammer.RelationshipModel();
            this.SuspendLayout();
            // 
            // relationshipModel1
            // 
            this.relationshipModel1.AutoScroll = true;
            this.relationshipModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.relationshipModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.relationshipModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.relationshipModel1.DragElement = null;
            this.relationshipModel1.GridColor = System.Drawing.Color.Silver;
            this.relationshipModel1.GridSize = new System.Drawing.Size(20, 20);
            this.relationshipModel1.Location = new System.Drawing.Point(0, 0);
            this.relationshipModel1.Name = "relationshipModel1";
            this.relationshipModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.relationshipModel1.RelationshipClass = null;
            this.relationshipModel1.Size = new System.Drawing.Size(218, 197);
            this.relationshipModel1.TabIndex = 0;
            this.relationshipModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.relationshipModel1.Zoom = 100F;
            // 
            // TabbedDocumentRelationship
            // 
            this.Controls.Add(this.relationshipModel1);
            this.Name = "TabbedDocumentRelationship";
            this.Size = new System.Drawing.Size(218, 197);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.RelationshipModel relationshipModel1;

    }
}

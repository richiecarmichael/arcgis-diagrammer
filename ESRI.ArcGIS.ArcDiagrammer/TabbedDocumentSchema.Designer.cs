namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentSchema {
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
            this.schemaModel1 = new ESRI.ArcGIS.Diagrammer.SchemaModel();
            this.SuspendLayout();
            // 
            // schemaModel1
            // 
            this.schemaModel1.AllowDrop = true;
            this.schemaModel1.AutoScroll = true;
            this.schemaModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.schemaModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.schemaModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaModel1.DragElement = null;
            this.schemaModel1.GridColor = System.Drawing.Color.Silver;
            this.schemaModel1.GridSize = new System.Drawing.Size(20, 20);
            this.schemaModel1.Location = new System.Drawing.Point(0, 0);
            this.schemaModel1.Name = "schemaModel1";
            this.schemaModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.schemaModel1.Size = new System.Drawing.Size(241, 168);
            this.schemaModel1.TabIndex = 0;
            this.schemaModel1.Version = "";
            this.schemaModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.schemaModel1.Zoom = 100F;
            // 
            // TabbedDocumentSchema
            // 
            this.AllowClose = false;
            this.Controls.Add(this.schemaModel1);
            this.Name = "TabbedDocumentSchema";
            this.Size = new System.Drawing.Size(241, 168);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.SchemaModel schemaModel1;

    }
}

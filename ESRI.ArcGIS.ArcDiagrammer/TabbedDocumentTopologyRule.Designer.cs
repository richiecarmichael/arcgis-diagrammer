namespace ESRI.ArcGIS.ArcDiagrammer {
    partial class TabbedDocumentTopologyRule {
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
            this.topologyRuleModel1 = new ESRI.ArcGIS.Diagrammer.TopologyRuleModel();
            this.SuspendLayout();
            // 
            // topologyRuleModel1
            // 
            this.topologyRuleModel1.AutoScroll = true;
            this.topologyRuleModel1.AutoScrollMinSize = new System.Drawing.Size(1000, 1000);
            this.topologyRuleModel1.DiagramSize = new System.Drawing.Size(1000, 1000);
            this.topologyRuleModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topologyRuleModel1.DragElement = null;
            this.topologyRuleModel1.GridColor = System.Drawing.Color.Silver;
            this.topologyRuleModel1.GridSize = new System.Drawing.Size(20, 20);
            this.topologyRuleModel1.Location = new System.Drawing.Point(0, 0);
            this.topologyRuleModel1.Name = "topologyRuleModel1";
            this.topologyRuleModel1.PageLineSize = new System.Drawing.SizeF(0F, 0F);
            this.topologyRuleModel1.Size = new System.Drawing.Size(298, 246);
            this.topologyRuleModel1.TabIndex = 0;
            this.topologyRuleModel1.Topology = null;
            this.topologyRuleModel1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.topologyRuleModel1.Zoom = 100F;
            // 
            // TabbedDocumentTopologyRule
            // 
            this.Controls.Add(this.topologyRuleModel1);
            this.Name = "TabbedDocumentTopologyRule";
            this.Size = new System.Drawing.Size(298, 246);
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Diagrammer.TopologyRuleModel topologyRuleModel1;






    }
}

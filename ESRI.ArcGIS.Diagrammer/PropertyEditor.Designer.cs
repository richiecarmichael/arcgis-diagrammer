namespace ESRI.ArcGIS.Diagrammer {
    partial class PropertyEditor {
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
            this.tabbedPropertyGrid1 = new ESRI.ArcGIS.Diagrammer.TabbedPropertyGrid();
            this.SuspendLayout();
            // 
            // tabbedPropertyGrid1
            // 
            this.tabbedPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedPropertyGrid1.HelpBackColor = System.Drawing.SystemColors.Window;
            this.tabbedPropertyGrid1.LineColor = System.Drawing.Color.Silver;
            this.tabbedPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.tabbedPropertyGrid1.Name = "tabbedPropertyGrid1";
            this.tabbedPropertyGrid1.Size = new System.Drawing.Size(239, 285);
            this.tabbedPropertyGrid1.TabIndex = 0;
            this.tabbedPropertyGrid1.ToolbarVisible = false;
            // 
            // PropertyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabbedPropertyGrid1);
            this.Name = "PropertyEditor";
            this.Size = new System.Drawing.Size(239, 285);
            this.ResumeLayout(false);

        }

        #endregion

        private TabbedPropertyGrid tabbedPropertyGrid1;


    }
}

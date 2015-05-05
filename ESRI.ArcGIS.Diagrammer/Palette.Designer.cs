namespace ESRI.ArcGIS.Diagrammer {
    partial class Palette {
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
            this.components = new System.ComponentModel.Container();
            this.listViewPalette = new System.Windows.Forms.ListView();
            this.imageListGeneral = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // listViewPalette
            // 
            this.listViewPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPalette.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewPalette.Location = new System.Drawing.Point(0, 0);
            this.listViewPalette.MultiSelect = false;
            this.listViewPalette.Name = "listViewPalette";
            this.listViewPalette.Size = new System.Drawing.Size(318, 442);
            this.listViewPalette.SmallImageList = this.imageListGeneral;
            this.listViewPalette.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewPalette.TabIndex = 2;
            this.listViewPalette.UseCompatibleStateImageBehavior = false;
            this.listViewPalette.View = System.Windows.Forms.View.Details;
            this.listViewPalette.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.ListView_ItemMouseHover);
            this.listViewPalette.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ListView_ItemDrag);
            // 
            // imageListGeneral
            // 
            this.imageListGeneral.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListGeneral.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListGeneral.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 500;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // Palette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewPalette);
            this.Name = "Palette";
            this.Size = new System.Drawing.Size(318, 442);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewPalette;
        private System.Windows.Forms.ImageList imageListGeneral;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

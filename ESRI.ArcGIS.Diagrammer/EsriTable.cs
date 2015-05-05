/*=============================================================================
 * 
 * Copyright © 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished—all rights reserved.
 * Use of this ESRI commercial Software, Data, and Documentation is limited to
 * the ESRI License Agreement. In no event shall the Government acquire greater
 * than Restricted/Limited Rights. At a minimum Government rights to use,
 * duplicate, or disclose is subject to restrictions as set for in FAR 12.211,
 * FAR 12.212, and FAR 52.227-19 (June 1987), FAR 52.227-14 (ALT I, II, and III)
 * (June 1987), DFARS 227.7202, DFARS 252.227-7015 (NOV 1995).
 * Contractor/Manufacturer is ESRI, 380 New York Street, Redlands,
 * CA 92373-8100, USA.
 * 
 * SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE, ARE DISCLAIMED.  IN NO EVENT SHALL ESRI OR CONTRIBUTORS
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT; STRICT LIABILITY; OR TORT ARISING
 * IN ANY WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE TO THE FULL EXTENT ALLOWED BY APPLICABLE LAW.
 * 
 * =============================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using Crainiate.Diagramming;

namespace ESRI.ArcGIS.Diagrammer {
    public abstract class EsriTable : Table {
        private bool m_flash = false;
        //
        // CONSTRUCTOR
        // 
        public EsriTable(IXPathNavigable path) : base() {
            // Check if path is valid
            if (path == null) { throw new NullReferenceException("<path> argument cannot be Null"); }

            // Suspend
            this.SuspendEvents = true;

            // Set Element
            this.AllowMove = true;
            this.BackColor = Color.White;
            this.ContractedSize = new SizeF(120f, 41f);
            this.DrawExpand = true;
            this.Expanded = false;
            this.HeadingHeight = 40f;
            this.Height = 41f;
            this.MaximumSize = new SizeF(100000f, 1000f);

            // Call Initializer
            this.Initialize();

            // Resume
            this.SuspendEvents = false;
        }
        public EsriTable(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public EsriTable(EsriTable prototype) : base(prototype) { }
        //
        // PROPERTIES
        //
        //
        // PROTECTED METHODS
        //
        protected virtual void WriteInnerXml(XmlWriter writer) { }
        protected override void OnSelectedItemChanged() {
            base.OnSelectedItemChanged();
            base.OnSelectedChanged();
        }
        protected override void Render(Graphics graphics, IRender render) {
            if (this.m_flash) {
                // Get Path
                GraphicsPath path = base.GetPath();

                // Draw Fill
                LinearGradientBrush brush = new LinearGradientBrush(
                    new RectangleF(0f, 0f, base.Rectangle.Width, base.Rectangle.Height),
                    base.BackColor,
                    Color.Yellow,
                    base.GradientMode);
                graphics.FillPath(brush, path);

                // Draw Outline
                Pen pen = new Pen(base.BorderColor, base.BorderWidth);
                pen.DashStyle = DashStyle.Solid;
                pen.Color = Color.Yellow;
                graphics.DrawPath(pen, path);
            }
            else {
                base.Render(graphics, render);
            }
        }
        protected abstract void Initialize();
        //
        // PUBLIC METHODS
        //
        public abstract void WriteXml(XmlWriter writer);
        public abstract void Errors(List<Error> list);
        public virtual void Refresh() { }
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }
        public override bool OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                TableItem tableItem = this.GetTableItem(e);
                this.SelectedItem = tableItem;
            }
            return base.OnMouseDown(e);
        }
        public void Flash() {
            FlashElement flashElement = new FlashElement();
            flashElement.Flash += new EventHandler<FlashEventArgs>(this.FlashElement_Flash);
            Thread thread = new Thread(new ThreadStart(flashElement.Execute));
            thread.Start();
        }
        public abstract void RefreshColor();
        //
        // PRIVATE
        //
        private void FlashElement_Flash(object sender, FlashEventArgs e) {
            Model model = (Model)base.Container;
            if (model.InvokeRequired) {
                model.Invoke(new EventHandler<FlashEventArgs>(this.FlashElement_Flash), new object[] { sender, e });
            }
            else {
                this.m_flash = e.Flash;
                this.Invalidate();
            }
        }
    }
}

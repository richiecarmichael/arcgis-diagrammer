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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Text;

using Crainiate.Diagramming;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class EsriLine<T> : Link, IParentObject where T : EsriObject {
        private readonly T m_parent = default(T);
        //
        // CONSTRUCTOR
        //
        public EsriLine(T parent) : base() {
            this.m_parent = parent;
            this.Initialize();
        }
        public EsriLine(T parent, PointF start, PointF end) : base(start, end) {
            this.m_parent = parent;
            this.Initialize();
        }
        public EsriLine(T parent, Shape start, Shape end) : base(start, end) {
            this.m_parent = parent;
            this.Initialize();
        }
        public EsriLine(SerializationInfo info, StreamingContext context) : base(info, context) {
            this.m_parent = (T)info.GetValue("parent", typeof(T));
        }
        public EsriLine(EsriLine<T> prototype) : base(prototype) {
            this.m_parent = prototype.Parent;
        }
        //
        // PROPERTY
        //
        [Browsable(false)]
        public T Parent {
            get { return this.m_parent; }
        }
        [Browsable(false)]
        public object ParentObject {
            get { return this.m_parent; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("parent", this.m_parent, typeof(T));
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new EsriLine<T>(this);
        }
        //
        // PRIVATE METHODS
        //
        private void Initialize() {
            // Suspend Events
            this.SuspendEvents = true;

            // Create Text Label
            TextLabel textLabel = new TextLabel();
            textLabel.Color = ModelSettings.Default.TextColor;
            textLabel.Bold = false;

            // Default Settings
            this.BorderColor = ModelSettings.Default.EnabledLines;
            this.BorderStyle = DashStyle.Solid;
            this.BorderWidth = 1f;
            this.End.Marker.BorderColor = ModelSettings.Default.EnabledLines;
            this.Label = textLabel;
            this.Opacity = 100;
            this.SmoothingMode = SmoothingMode.HighQuality;
            this.Tooltip = string.Empty;

            // Set Invalidation Listener
            EsriObject esriObject = (EsriObject)this.m_parent;
            esriObject.Invalidated += new EventHandler<EventArgs>(this.Parent_Invalidated);

            // Run Invalidation Delegate to Update Label and Tooltip
            this.Parent_Invalidated(null, EventArgs.Empty);

            // Resume Events
            this.SuspendEvents = false;
        }
        private void Parent_Invalidated(object sender, EventArgs e) {
            if (this.m_parent is IDiagramProperty) {
                IDiagramProperty diagramText = (IDiagramProperty)this.m_parent;
                this.SuspendEvents = true;
                this.Label.Text = diagramText.ShowLabels ? diagramText.Label : string.Empty;
                this.Tooltip = diagramText.Label;
                this.BorderWidth = diagramText.BorderWidth;
                this.End.Marker.BorderWidth = diagramText.BorderWidth;
                this.SuspendEvents = false;
                this.Invalidate();
            }
        }
    }
}

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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming;

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    public class EsriShape<T> : Shape, IParentObject {
        private readonly T _parent = default(T);
        //
        // CONSTRUCTOR
        //
        public EsriShape(T parent) : base(){
            this._parent = parent;

            //
            this.SuspendEvents = true;

            // Get a stencil containing some basic shapes
            BasicStencil stencil = (BasicStencil)Crainiate.ERM4.Component.Instance.GetStencil(typeof(BasicStencil));

            this.BorderWidth = 2f;
            this.Size = new Size(100, 40);
            this.SmoothingMode = SmoothingMode.HighQuality;
            this.StencilItem = stencil[BasicStencilType.RoundedRectangle];
            this.Label = new TextLabel();
            this.Label.Color = ModelSettings.Default.TextColor;

            if (typeof(T) == typeof(DomainCodedValue)) {
                DomainCodedValue domainCodedValue = this._parent as DomainCodedValue;
                this.BorderColor = ColorSettings.Default.CodedValueDomainColor;
                this.GradientColor = ColorSettings.Default.CodedValueDomainColor;
                this.Label.Text = domainCodedValue.Name;
                this.Tooltip = domainCodedValue.Name;
            }
            else if (typeof(T) == typeof(DomainRange)) {
                DomainRange domainRange = this._parent as DomainRange;
                this.BorderColor = ColorSettings.Default.RangeDomainColor;
                this.GradientColor = ColorSettings.Default.RangeDomainColor;
                this.Label.Text = domainRange.Name;
                this.Tooltip = domainRange.Name;
            }
            else if (typeof(T) == typeof(FeatureClass)) {
                FeatureClass featureClass = this._parent as FeatureClass;
                this.BorderColor = ColorSettings.Default.FeatureClassColor;
                this.GradientColor = ColorSettings.Default.FeatureClassColor;
                this.Label.Text = featureClass.Name;
                this.Tooltip = featureClass.Name;
            }
            else if (typeof(T) == typeof(ObjectClass)) {
                ObjectClass objectClass = this._parent as ObjectClass;
                this.BorderColor = ColorSettings.Default.ObjectClassColor;
                this.GradientColor = ColorSettings.Default.ObjectClassColor;
                this.Label.Text = objectClass.Name;
                this.Tooltip = objectClass.Name;
            }
            else if (typeof(T) == typeof(RelationshipClass)) {
                RelationshipClass relationship = this._parent as RelationshipClass;
                this.BorderColor = ColorSettings.Default.RelationshipColor;
                this.GradientColor = ColorSettings.Default.RelationshipColor;
                this.Label.Text = relationship.Name;
                this.Tooltip = relationship.Name;
            }
            else if (typeof(T) == typeof(Subtype)) {
                Subtype subtype = this._parent as Subtype;
                this.BorderColor = ColorSettings.Default.SubtypeColor;
                this.GradientColor = ColorSettings.Default.SubtypeColor;
                this.Label.Text = subtype.SubtypeName;
                this.Tooltip = subtype.SubtypeName;
            }
            else if (typeof(T) == typeof(Field)) {
                Field field = this._parent as Field;
                this.BorderColor = ColorSettings.Default.FieldColor;
                this.GradientColor = ColorSettings.Default.FieldColor;
                this.Label.Text = field.Name;
                this.Tooltip = field.Name;
            }
            else if (typeof(T) == typeof(SubtypeField)) {
                SubtypeField subtypeField = this._parent as SubtypeField;
                this.BorderColor = ColorSettings.Default.SubtypeFieldColor;
                this.GradientColor = ColorSettings.Default.SubtypeFieldColor;
                this.Label.Text = subtypeField.FieldName;
                this.Tooltip = subtypeField.FieldName;
            }
            else if (typeof(T) == typeof(FeatureDataset)) {
                FeatureDataset featureDataset = this._parent as FeatureDataset;
                this.BorderColor = ColorSettings.Default.FeatureDatasetColor;
                this.GradientColor = ColorSettings.Default.FeatureDatasetColor;
                this.Label.Text = featureDataset.Name;
                this.Tooltip = featureDataset.Name;
            }
            else if (typeof(T) == typeof(GeometricNetwork)) {
                GeometricNetwork geometricNetwork = this._parent as GeometricNetwork;
                this.BorderColor = ColorSettings.Default.GeometricNetworkColor;
                this.GradientColor = ColorSettings.Default.GeometricNetworkColor;
                this.Label.Text = geometricNetwork.Name;
                this.Tooltip = geometricNetwork.Name;
            }
            else if (typeof(T) == typeof(Network)) {
                Network network = this._parent as Network;
                this.BorderColor = ColorSettings.Default.NetworkColor;
                this.GradientColor = ColorSettings.Default.NetworkColor;
                this.Label.Text = network.Name;
                this.Tooltip = network.Name;
            }
            else if (typeof(T) == typeof(Topology)) {
                Topology topology = this._parent as Topology;
                this.BorderColor = ColorSettings.Default.TopologyColor;
                this.GradientColor = ColorSettings.Default.TopologyColor;
                this.Label.Text = topology.Name;
                this.Tooltip = topology.Name;
            }
            else if (typeof(T) == typeof(Terrain)) {
                Terrain terrain = this._parent as Terrain;
                this.BorderColor = ColorSettings.Default.TerrainColor;
                this.GradientColor = ColorSettings.Default.TerrainColor;
                this.Label.Text = terrain.Name;
                this.Tooltip = terrain.Name;
            }

            //
            this.SuspendEvents = false;
        }
        public EsriShape(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._parent = (T)info.GetValue("parent", typeof(T));
        }
        public EsriShape(EsriShape<T> prototype) : base(prototype) {
            this._parent = prototype.Parent;
        }
        //
        // PROPERTY
        //
        [Browsable(false)]
        public T Parent {
            get { return this._parent; }
        }
        [Browsable(false)]
        public object ParentObject {
            get { return this._parent; }
        }
        //
        // PUBLIC METHODS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("parent", this._parent);
            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new EsriShape<T>(this);
        }
    }
}

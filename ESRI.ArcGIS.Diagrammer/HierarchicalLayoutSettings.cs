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

using System.ComponentModel;
using System.Configuration;
using Crainiate.ERM4.Layouts;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class HierarchicalLayoutSettings : ApplicationSettingsBase {
        private static HierarchicalLayoutSettings defaultInstance = ((HierarchicalLayoutSettings)(ApplicationSettingsBase.Synchronized(new HierarchicalLayoutSettings())));
        public static HierarchicalLayoutSettings Default {
            get { return defaultInstance; }
        }
        [Browsable(true)]
        [Category("SettingsHierarchicalLayout")]
        [DefaultValue(40f)]
        [DefaultSettingValue("40")]
        [Description("Minimum distance that connected components in a graph must have")]
        [DisplayName("Connected Component Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float ConnectedComponentDistance{
            get { return ((float)(this["ConnectedComponentDistance"])); }
            set { this["ConnectedComponentDistance"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsHierarchicalLayout")]
        [DefaultValue(HierarchicalDirection.TopToBottom)]
        [DefaultSettingValue("TopToBottom")]
        [Description("Get or sets the orientation of the layout")]
        [DisplayName("Layout Direction")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public HierarchicalDirection Direction {
            get { return ((HierarchicalDirection)(this["Direction"])); }
            set { this["Direction"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsHierarchicalLayout")]
        [DefaultValue(40f)]
        [DefaultSettingValue("40")]
        [Description("Defines the minimal distance between consecutive layers (levels)")]
        [DisplayName("Layer Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float LayerDistance {
            get { return ((float)(this["LayerDistance"])); }
            set { this["LayerDistance"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsHierarchicalLayout")]
        [DefaultValue(40f)]
        [DefaultSettingValue("40")]
        [Description("Defines the minimal distance between nodes on the same layer (level)")]
        [DisplayName("Object Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float ObjectDistance {
            get { return ((float)(this["ObjectDistance"])); }
            set { this["ObjectDistance"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsHierarchicalLayout")]
        [DefaultValue(false)]
        [DefaultSettingValue("false")]
        [Description("A boolean value determining if the layout layer should be respected")]
        [DisplayName("Respect Layer")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool RespectLayer {
            get { return ((bool)(this["RespectLayer"])); }
            set { this["RespectLayer"] = value; }
        }
    }
}

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
    public sealed class TreeLayoutSettings : ApplicationSettingsBase {
        private static TreeLayoutSettings defaultInstance = ((TreeLayoutSettings)(ApplicationSettingsBase.Synchronized(new TreeLayoutSettings())));
        public static TreeLayoutSettings Default {
            get { return defaultInstance; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(TreeDirection.TopToBottom)]
        [DefaultSettingValue("TopToBottom")]
        [Description("Get or sets the orientation of the layout")]
        [DisplayName("Direction")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public TreeDirection Direction {
            get { return ((TreeDirection)(this["Direction"])); }
            set { this["Direction"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(typeof(float), "50")]
        [DefaultSettingValue("50")]
        [Description("Defines the distance between the consecutive levels")]
        [DisplayName("Level Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float LevelDistance {
            get { return ((float)(this["LevelDistance"])); }
            set { this["LevelDistance"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(false)]
        [DefaultSettingValue("false")]
        [Description("Determines how connecting lines are drawn in the layout. When set to true, edges are drawn orthogonal (a sequence of horizontal and vertical segments). Otherwise, edges are drawn straight line.")]
        [DisplayName("Orthogonal Layout Style")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool OrthogonalLayoutStyle {
            get { return ((bool)(this["OrthogonalLayoutStyle"])); }
            set { this["OrthogonalLayoutStyle"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(RootSelection.Source)]
        [DefaultSettingValue("Source")]
        [Description("This property describes the selection method for the root(s)")]
        [DisplayName("Root Selection")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public RootSelection RootSelection {
            get { return ((RootSelection)(this["RootSelection"])); }
            set { this["RootSelection"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(typeof(float), "50")]
        [DefaultSettingValue("50")]
        [Description("Defines the minimal distance between consecutive children of a node")]
        [DisplayName("Sibling Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float SiblingDistance {
            get { return ((float)(this["SiblingDistance"])); }
            set { this["SiblingDistance"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(typeof(float), "50")]
        [DefaultSettingValue("50")]
        [Description("Defines the minimal distance between the consecutive subtrees of a node")]
        [DisplayName("Subtree Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float SubtreeDistance {
            get { return ((float)(this["SubtreeDistance"])); }
            set { this["SubtreeDistance"] = value; }
        }
        [Browsable(true)]
        [Category("Settings Tree Layout")]
        [DefaultValue(typeof(float), "50")]
        [DefaultSettingValue("50")]
        [Description("Defines the distance between different trees in the diagram layout")]
        [DisplayName("Tree Distance")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public float TreeDistance {
            get { return ((float)(this["TreeDistance"])); }
            set { this["TreeDistance"] = value; }
        }
    }
}

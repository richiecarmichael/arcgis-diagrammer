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
using System.Drawing;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class ModelSettings : ApplicationSettingsBase {
        private static ModelSettings defaultInstance = ((ModelSettings)(ApplicationSettingsBase.Synchronized(new ModelSettings())));
        public static ModelSettings Default {
            get { return defaultInstance; }
        }
        [Browsable(true)]
        [Category("SettingsModel")]
        [DefaultValue(true)]
        [DefaultSettingValue("true")]
        [Description("Set to TRUE to enable undo/redo operations")]
        [DisplayName("Enable Undo and Redo")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool EnableUndoRedo {
            get { return ((bool)(this["EnableUndoRedo"])); }
            set { this["EnableUndoRedo"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsModel")]
        [DefaultValue(typeof(Color), "CornflowerBlue")]
        [DefaultSettingValue("CornflowerBlue")]
        [Description("Color used to display lines that are editable")]
        [DisplayName("Enabled Lines")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color EnabledLines {
            get { return ((Color)(this["EnabledLines"])); }
            set { this["EnabledLines"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsModel")]
        [DefaultValue(typeof(Color), "Gray")]
        [DefaultSettingValue("Gray")]
        [Description("Color used to display lines that are not editable")]
        [DisplayName("Disabled Lined")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color DisabledLined {
            get { return ((Color)(this["DisabledLined"])); }
            set { this["DisabledLined"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsModel")]
        [DefaultValue(typeof(Color), "Black")]
        [DefaultSettingValue("Black")]
        [Description("Color used to display label text in diagrams")]
        [DisplayName("Text Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color TextColor {
            get { return ((Color)(this["TextColor"])); }
            set { this["TextColor"] = value; }
        }
        //[Browsable(true)]
        //[Category("SettingsModel")]
        //[DefaultValue(MetadataEditorType.ISO)]
        //[DefaultSettingValue("ISO")]
        //[Description("Editor used to create or modify metadata or a dataset or workspace")]
        //[DisplayName("Metadata Editor")]
        //[ParenthesizePropertyName(false)]
        //[UserScopedSetting()]
        //[ReadOnly(true)]
        //public MetadataEditorType MetadataEditorType {
        //    get { return ((MetadataEditorType)(this["MetadataEditorType"])); }
        //    set { this["MetadataEditorType"] = value; }
        //}
    }
}

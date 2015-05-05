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
using System.Drawing.Design;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class SchemaReportSettings : ApplicationSettingsBase {
        private const string CATEGORY = "SettingsSchemaReport";
        private static SchemaReportSettings defaultInstance = ((SchemaReportSettings)(ApplicationSettingsBase.Synchronized(new SchemaReportSettings())));

        public static SchemaReportSettings Default {
            get { return defaultInstance; }
        }

        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue("Tahoma")]
        [DefaultSettingValue("Tahoma")]
        [Description("Font used for schema and data reports")]
        [DisplayName("Font Name")]
        [ParenthesizePropertyName(false)]
        [TypeConverter(typeof(FontConverter.FontNameConverter))]
        [Editor("System.Drawing.Design.FontNameEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UserScopedSetting()]
        public string FontName {
            get { return ((string)(this["FontName"])); }
            set { this["FontName"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(Color), "White")]
        [DefaultSettingValue("White")]
        [Description("Background color used in schema and data reports")]
        [DisplayName("Back Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color BackColor {
            get { return ((Color)(this["BackColor"])); }
            set { this["BackColor"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(Color), "Black")]
        [DefaultSettingValue("Black")]
        [Description("Text color used in schema and data reports")]
        [DisplayName("Fore Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color ForeColor {
            get { return ((Color)(this["ForeColor"])); }
            set { this["ForeColor"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(ushort), "1")]
        [DefaultSettingValue("1")]
        [Description("Report Font Size (Level 1)")]
        [DisplayName("Font Size Level 1")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort Size1 {
            get { return ((ushort)(this["Size1"])); }
            set { this["Size1"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(ushort), "2")]
        [DefaultSettingValue("2")]
        [Description("Report Font Size (Level 2)")]
        [DisplayName("Font Size Level 2")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort Size2 {
            get { return ((ushort)(this["Size2"])); }
            set { this["Size2"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(ushort), "3")]
        [DefaultSettingValue("3")]
        [Description("Report Font Size (Level 3)")]
        [DisplayName("Font Size Level 3")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort Size3 {
            get { return ((ushort)(this["Size3"])); }
            set { this["Size3"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(ushort), "4")]
        [DefaultSettingValue("4")]
        [Description("Report Font Size (Level 4)")]
        [DisplayName("Font Size Level 4")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort Size4 {
            get { return ((ushort)(this["Size4"])); }
            set { this["Size4"] = value; }
        }
        [Browsable(true)]
        [Category(CATEGORY)]
        [DefaultValue(typeof(ushort), "5")]
        [DefaultSettingValue("5")]
        [Description("Report Font Size (Level 5)")]
        [DisplayName("Font Size Level 5")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort Size5 {
            get { return ((ushort)(this["Size5"])); }
            set { this["Size5"] = value; }
        }
    }
}

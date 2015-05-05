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

using ESRI.ArcGIS.Carto;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class DataReportSettings : ApplicationSettingsBase {
        private static DataReportSettings defaultInstance = ((DataReportSettings)(ApplicationSettingsBase.Synchronized(new DataReportSettings())));
        public static DataReportSettings Default {
            get { return defaultInstance; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Category("SettingsDataReport")]
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
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(true)]
        [DefaultSettingValue("true")]
        [Description("Displays a small thumbnail image in the data report")]
        [DisplayName("Show Small Image")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool ShowSmallImage {
            get { return ((bool)(this["ShowSmallImage"])); }
            set { this["ShowSmallImage"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(esriImageFormat.esriImageJPG)]
        [DefaultSettingValue("esriImageJPG")]
        [Description("Small Image Format")]
        [DisplayName("Small Image Type")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public esriImageFormat SmallImageType {
            get { return ((esriImageFormat)(this["SmallImageType"])); }
            set { this["SmallImageType"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(Size), "50, 50")]
        [DefaultSettingValue("50, 50")]
        [Description("Small Image Size")]
        [DisplayName("Small Image Size")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Size SmallImageSize {
            get { return ((Size)(this["SmallImageSize"])); }
            set { this["SmallImageSize"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(ushort), "96")]
        [DefaultSettingValue("96")]
        [Description("Small Image Resolution")]
        [DisplayName("Small Image Resolution")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort SmallImageResolution {
            get { return ((ushort)(this["SmallImageResolution"])); }
            set { this["SmallImageResolution"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(Color), "White")]
        [DefaultSettingValue("White")]
        [Description("Small Image Background Color")]
        [DisplayName("Small Image Background Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color SmallImageBackgroundColor {
            get { return ((Color)(this["SmallImageBackgroundColor"])); }
            set { this["SmallImageBackgroundColor"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(true)]
        [DefaultSettingValue("true")]
        [Description("Displays a Large thumbnail image in the data report")]
        [DisplayName("Show Large Image")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool ShowLargeImage {
            get { return ((bool)(this["ShowLargeImage"])); }
            set { this["ShowLargeImage"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(esriImageFormat.esriImageJPG)]
        [DefaultSettingValue("esriImageJPG")]
        [Description("Large Image Format")]
        [DisplayName("Large Image Type")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public esriImageFormat LargeImageType {
            get { return ((esriImageFormat)(this["LargeImageType"])); }
            set { this["LargeImageType"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(Size), "500, 500")]
        [DefaultSettingValue("500, 500")]
        [Description("Large Image Size")]
        [DisplayName("Large Image Size")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Size LargeImageSize {
            get { return ((Size)(this["LargeImageSize"])); }
            set { this["LargeImageSize"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(ushort), "96")]
        [DefaultSettingValue("96")]
        [Description("Large Image Resolution")]
        [DisplayName("Large Image Resolution")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public ushort LargeImageResolution {
            get { return ((ushort)(this["LargeImageResolution"])); }
            set { this["LargeImageResolution"] = value; }
        }
        [Browsable(true)]
        [Category("SettingsDataReport")]
        [DefaultValue(typeof(Color), "White")]
        [DefaultSettingValue("White")]
        [Description("Large Image Background Color")]
        [DisplayName("Large Image Background Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color LargeImageBackgroundColor {
            get { return ((Color)(this["LargeImageBackgroundColor"])); }
            set { this["LargeImageBackgroundColor"] = value; }
        }
    }
}

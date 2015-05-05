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
using System.Windows.Forms;

namespace ESRI.ArcGIS.ArcDiagrammer {
    public sealed class SettingsWindow : ApplicationSettingsBase {
        private static SettingsWindow defaultInstance = ((SettingsWindow)(ApplicationSettingsBase.Synchronized(new SettingsWindow())));
        public static SettingsWindow Default {
            get { return defaultInstance; }
        }
        [Browsable(false)]
        [DefaultValue("")]
        [UserScopedSetting()]
        public string SandbarLayout {
            get { return ((string)(this["SandbarLayout"])); }
            set { this["SandbarLayout"] = value; }
        }
        [Browsable(false)]
        [DefaultValue("")]
        [UserScopedSetting()]
        public string SanddockLayout {
            get { return ((string)(this["SanddockLayout"])); }
            set { this["SanddockLayout"] = value; }
        }
        [Browsable(false)]
        [DefaultValue(typeof(Point), "0, 0")]
        [DefaultSettingValue("0, 0")]
        [UserScopedSetting()]
        public Point WindowLocation {
            get { return ((Point)(this["WindowLocation"])); }
            set { this["WindowLocation"] = value; }
        }
        [Browsable(false)]
        [DefaultValue(typeof(Size), "0, 0")]
        [DefaultSettingValue("0, 0")]
        [UserScopedSetting()]
        public Size WindowSize {
            get { return ((Size)(this["WindowSize"])); }
            set { this["WindowSize"] = value; }
        }
        [Browsable(true)]
        [Category("Data")]
        [DefaultValue(true)]
        [DefaultSettingValue("true")]
        [Description("Saves the ArcDiagrammer window position on exit")]
        [DisplayName("Save Window Position")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool SaveWindowPosition {
            get { return ((bool)(this["SaveWindowPosition"])); }
            set { this["SaveWindowPosition"] = value; }
        }
        [Browsable(true)]
        [Category("Data")]
        [DefaultValue(false)]
        [DefaultSettingValue("false")]
        [Description("Displays a window whenever an exception occurs")]
        [DisplayName("Show Exception Window")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool ShowExceptionWindow {
            get { return ((bool)(this["ShowExceptionWindow"])); }
            set { this["ShowExceptionWindow"] = value; }
        }
        [Browsable(false)]
        [DefaultValue(FormWindowState.Normal)]
        [DefaultSettingValue("FormWindowState.Normal")]
        [UserScopedSetting()]
        public FormWindowState WindowState {
            get { return ((FormWindowState)(this["WindowState"])); }
            set { this["WindowState"] = value; }
        }
    }
}

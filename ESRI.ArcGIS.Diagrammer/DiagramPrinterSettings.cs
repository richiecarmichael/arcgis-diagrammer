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
using System.Drawing.Printing;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class DiagramPrinterSettings : ApplicationSettingsBase {
        private static DiagramPrinterSettings defaultInstance = ((DiagramPrinterSettings)(ApplicationSettingsBase.Synchronized(new DiagramPrinterSettings())));
        public static DiagramPrinterSettings Default {
            get { return defaultInstance; }
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Printer used to print diagrams (not reports)
        /// </summary>
        [Browsable(true)]
        [Category("Printer Settings")]
        [DefaultSettingValue("")]
        [DefaultValue("")]
        [Description("Printer used to print diagrams (not reports)")]
        [DisplayName("Printer Name")]
        [ParenthesizePropertyName(false)]
        [TypeConverter(typeof(PrinterNameConverter))]
        [UserScopedSetting()]
        public string PrinterName {
            get { return ((string)(this["PrinterName"])); }
            set { this["PrinterName"] = value; }
        }
        /// <summary>
        /// Paper size used to print diagrams (not reports)
        /// </summary>
        [Browsable(true)]
        [Category("Printer Settings")]
        [DefaultSettingValue("")]
        [DefaultValue("")]
        [Description("Paper size used to print diagrams (not reports)")]
        [DisplayName("Printer Paper Size")]
        [ParenthesizePropertyName(false)]
        [TypeConverter(typeof(PrinterPaperSizeConverter))]
        [UserScopedSetting()]
        public string PrinterPaperSize {
            get { return ((string)(this["PrinterPaperSize"])); }
            set { this["PrinterPaperSize"] = value; }
        }
        /// <summary>
        /// Use Landscape Printer Orientation
        /// </summary>
        [Browsable(true)]
        [Category("Printer Settings")]
        [DefaultSettingValue("false")]
        [DefaultValue(false)]
        [Description("Use Landscape Printer Orientation")]
        [DisplayName("Landscape")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public bool Landscape {
            get { return ((bool)(this["Landscape"])); }
            set { this["Landscape"] = value; }
        }
        /// <summary>
        /// Printer Margins
        /// </summary>
        [Browsable(true)]
        [Category("Printer Settings")]
        [DefaultSettingValue("100, 100, 100, 100")]
        [DefaultValue(typeof(Margins), "100, 100, 100, 100")]
        [Description("Printer Margins in 100th of an inch")]
        [DisplayName("Margins")]
        [ParenthesizePropertyName(false)]
        [TypeConverter(typeof(MarginsConverter))]
        [UserScopedSetting()]
        public Margins Margins {
            get { return ((Margins)(this["Margins"])); }
            set { this["Margins"] = value; }
        }
        //
        // PROTECTED METHODS
        //
        protected override void OnSettingChanging(object sender, SettingChangingEventArgs e) {
            base.OnSettingChanging(sender, e);
            if (e.SettingName == "PrinterName") {
                this["PrinterPaperSize"] = string.Empty;
            }
        }
    }
}

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
using System.Configuration;
using System.Drawing;
using System.Text;

namespace ESRI.ArcGIS.Diagrammer {
    public sealed class ColorSettings : ApplicationSettingsBase {
        private static ColorSettings defaultInstance = ((ColorSettings)(ApplicationSettingsBase.Synchronized(new ColorSettings())));
        public static ColorSettings Default {
            get { return defaultInstance; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Orchid")]
        [DefaultSettingValue("Orchid")]
        [Description("Color of Coded Value Domains")]
        [DisplayName("Coded Value Domain Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color CodedValueDomainColor {
            get { return ((Color)(this["CodedValueDomainColor"])); }
            set { this["CodedValueDomainColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "LightBlue")]
        [DefaultSettingValue("LightBlue")]
        [Description("Color of Range Domains")]
        [DisplayName("Range Domain Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color RangeDomainColor {
            get { return ((Color)(this["RangeDomainColor"])); }
            set { this["RangeDomainColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Khaki")]
        [DefaultSettingValue("Khaki")]
        [Description("Color of Tables")]
        [DisplayName("Table Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color ObjectClassColor {
            get { return ((Color)(this["ObjectClassColor"])); }
            set { this["ObjectClassColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "DarkKhaki")]
        [DefaultSettingValue("DarkKhaki")]
        [Description("Color of Feature Classes")]
        [DisplayName("Feature Class Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color FeatureClassColor {
            get { return ((Color)(this["FeatureClassColor"])); }
            set { this["FeatureClassColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "AliceBlue")]
        [DefaultSettingValue("AliceBlue")]
        [Description("Color of Raster Catalog")]
        [DisplayName("Raster Catalog Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color RasterCatalogColor {
            get { return ((Color)(this["RasterCatalogColor"])); }
            set { this["RasterCatalogColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "DeepPink")]
        [DefaultSettingValue("DeepPink")]
        [Description("Color of Raster Dataset")]
        [DisplayName("Raster Dataset Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color RasterDatasetColor {
            get { return ((Color)(this["RasterDatasetColor"])); }
            set { this["RasterDatasetColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Peru")]
        [DefaultSettingValue("Peru")]
        [Description("Color of Feature Datasets")]
        [DisplayName("Feature Dataset Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color FeatureDatasetColor {
            get { return ((Color)(this["FeatureDatasetColor"])); }
            set { this["FeatureDatasetColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "PaleGreen")]
        [DefaultSettingValue("PaleGreen")]
        [Description("Color of Subtypes")]
        [DisplayName("Subtype Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color SubtypeColor {
            get { return ((Color)(this["SubtypeColor"])); }
            set { this["SubtypeColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Gold")]
        [DefaultSettingValue("Gold")]
        [Description("Color of Geometric Networks")]
        [DisplayName("Geometric Network Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color GeometricNetworkColor {
            get { return ((Color)(this["GeometricNetworkColor"])); }
            set { this["GeometricNetworkColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "LightPink")]
        [DefaultSettingValue("LightPink")]
        [Description("Color of Topology Datasets")]
        [DisplayName("Topology Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color TopologyColor {
            get { return ((Color)(this["TopologyColor"])); }
            set { this["TopologyColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "ForestGreen")]
        [DefaultSettingValue("ForestGreen")]
        [Description("Color of Network Datasets")]
        [DisplayName("Network Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color NetworkColor {
            get { return ((Color)(this["NetworkColor"])); }
            set { this["NetworkColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Silver")]
        [DefaultSettingValue("Silver")]
        [Description("Color of Relationship")]
        [DisplayName("RelationshipColor Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color RelationshipColor {
            get { return ((Color)(this["RelationshipColor"])); }
            set { this["RelationshipColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "Firebrick")]
        [DefaultSettingValue("Firebrick")]
        [Description("Color of Field")]
        [DisplayName("Field Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color FieldColor {
            get { return ((Color)(this["FieldColor"])); }
            set { this["FieldColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "DarkSalmon")]
        [DefaultSettingValue("DarkSalmon")]
        [Description("Color of Subtpe Field")]
        [DisplayName("Subtype Field Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color SubtypeFieldColor {
            get { return ((Color)(this["SubtypeFieldColor"])); }
            set { this["SubtypeFieldColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [DefaultSettingValue("DodgerBlue")]
        [Description("Color of Raster Band")]
        [DisplayName("Raster Band Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color RasterBandColor {
            get { return ((Color)(this["RasterBandColor"])); }
            set { this["RasterBandColor"] = value; }
        }
        [Browsable(true)]
        [Category("Color Settings")]
        [DefaultValue(typeof(Color), "LightCyan")]
        [DefaultSettingValue("LightCyan")]
        [Description("Color of Terrain")]
        [DisplayName("Terrain Color")]
        [ParenthesizePropertyName(false)]
        [UserScopedSetting()]
        public Color TerrainColor {
            get { return ((Color)(this["TerrainColor"])); }
            set { this["TerrainColor"] = value; }
        }
    }
}

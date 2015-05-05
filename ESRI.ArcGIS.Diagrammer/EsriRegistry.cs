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
using System.Runtime.InteropServices;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// Collection of Registry Tools
    /// </summary>
    [ComVisible(false)]
    public static class EsriRegistry {
        // ESRI Classes
        public const string CLASS_APPLICATION = "{E1740EC5-9513-11D2-A2DF-0000F8774FB5}";
        public const string CLASS_ESRIEDITOR_EDITOR = "{F8842F20-BB23-11D0-802B-0000F8037368}";

        // General
        public const string COMPONENT_LAYER_PROPERTY_PAGE = "{1476C782-6F57-11D2-A2C6-080009B6F22B}";
        public const string COMPONENT_LAYER_FACTORY = "{34C20001-4D3C-11D0-92D8-00805F7C28B0}";
        public const string COMPONENT_RENDERER_PROPERTY_PAGE = "{962BD9A9-1EC8-11D3-9F4D-00C04F6BC709}";

        // Geoprocessing
        public const string COMPONENT_GP_FUNCTION_FACTORY = "{FD939A4A-955D-4094-B440-77083E410F41}";
        public const string COMPONENT_GP_DATATYPE_FACTORY = "{CDA62C78-1221-4246-A622-61C354091657}";
        public const string COMPONENT_GP_TOOLEXTENSION_FACTORY = "{02079C92-481B-4C37-8CD6-F530109D6016}";
        public const string COMPONENT_GP_TOOLPROPERTYPAGES = "{49F484F5-C1D5-4D89-B586-D23B5DF54E03}";

        // ArcCatalog
        public const string COMPONENT_GX_EXTENSION = "{4531C69D-DC07-11D2-9F2F-00C04F6BC69E}";
        public const string COMPONENT_GX_JITEXTENSION = "{E72DB9D2-D842-4AA5-9D7F-1B70EA2F0A10}";

        // ArcMap
        public const string COMPONENT_MX_COMMAND = "{B56A7C42-83D4-11D2-A2E9-080009B6F22B}";
        public const string COMPONENT_MX_TOOLBAR = "{B56A7C4A-83D4-11D2-A2E9-080009B6F22B}";
        public const string COMPONENT_MX_DOCKABLEWINDOW = "{117623B5-F9D1-11D3-A67F-0008C7DF97B9}";
        public const string COMPONENT_MX_TOCVIEW = "{089874FC-CC18-11D2-9F39-00C04F6BC78E}";
        public const string COMPONENT_MX_EXTENSION = "{B56A7C45-83D4-11D2-A2E9-080009B6F22B}";
        public const string COMPONENT_MX_JITEXTENSION = "{B8C1C6CD-BE34-4EED-BAE9-8584F7A61B07}";
        public const string COMPONENT_MX_CONTEXTANALYSERS = "{0C452011-84CB-11D2-AE68-080009EC732A}";
        public const string COMPONENT_MX_CONTEXTMENU_FEATURELAYER = "{BF643199-9062-11D2-AE71-080009EC732A}";

        // ArcScene
        public const string COMPONENT_SX_COMMAND = "{F27D8292-A383-11D3-8206-0080C7597E71}";
        public const string COMPONENT_SX_TOOLBAR = "{F27D8291-A383-11D3-8206-0080C7597E71}";

        // ArcGlobe
        public const string COMPONENT_GMX_COMMAND = "{720E21D4-2199-11D6-B2B3-00508BCDDE28}";
        public const string COMPONENT_GMX_TOOLBAR = "{720E21D3-2199-11D6-B2B3-00508BCDDE28}";
        public const string COMPONENT_GMX_DOCKABLEWINDOW = "{720E21D8-2199-11D6-B2B3-00508BCDDE28}";
        public const string COMPONENT_GMX_EXTENSION = "{720E21D6-2199-11D6-B2B3-00508BCDDE28}";
        public const string COMPONENT_GMX_JITEXTENSION = "{99A23410-D290-41C8-83D8-123D37A1B67B}";
        public const string COMPONENT_GMX_CONTEXTMENU_FEATURELAYER = "{720E21E1-2199-11D6-B2B3-00508BCDDE28}";
        public const string COMPONENT_GMX_ANIMATION_TYPE = "{350DE633-42A4-458D-B07B-35D3B10E12B6}";

        // esriCore.Feature
        public const string ESRICORE_FEATURE = "{52353152-891A-11D0-BEC6-00805F7C4268}";

        // Geodatabase Workspace Factory Codes
        public const string GEODATABASE_FILE = "{71FE75F0-EA0C-4406-873E-B7D53748AE7E}";
        public const string GEODATABASE_PERSONAL = "{DD48C96A-D92A-11D1-AA81-00C04FA33A15}";
        public const string GEODATABASE_SDE = "{D9B4FA40-D6D9-11D1-AA81-00C04FA33A15}";

        // ObjectClass CLSID and EXTCLSID guids
        public const string CLASS_FEATURECLASS = "{52353152-891A-11D0-BEC6-00805F7C4268}";
        public const string CLASS_TABLE = "{7A566981-C114-11D2-8A28-006097AFF44E}";
        public const string CLASS_ANNOTATION = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
        public const string CLASS_ANNOTATION_EXTENSION = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
        public const string CLASS_DIMENSION = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
        public const string CLASS_DIMENSION_EXTENSION = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
        public const string CLASS_SIMPLEJUNCTION = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
        public const string CLASS_SIMPLEEDGE = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
        public const string CLASS_COMPLEXEDGE = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
        public const string CLASS_RASTERCATALOG = "{3EAA2478-5332-40F8-8FA8-62382390A3BA}";
        public const string CLASS_ATTRIBUTED_RELATIONSHIP = "{A07E9CB1-9A95-11D2-891A-0000F877762D}";
    }
}

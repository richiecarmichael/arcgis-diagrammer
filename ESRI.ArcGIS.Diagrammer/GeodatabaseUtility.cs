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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseDistributed;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;

namespace ESRI.ArcGIS.Diagrammer {
    public static class GeodatabaseUtility {
        public static string ProcessDataObject(DragEventArgs e) {
            // If the dropped object is from ArcCatalog then export the geodatabase
            // to XML and return pathname.
            if (e.Data.GetDataPresent("ESRI Names")) {
                return GeodatabaseUtility.ExportXml(e);
            }

            // If a use dropped one or more files then get first file and verify that it
            // has a "xml" or "diagram" extension.
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                object drop = e.Data.GetData(DataFormats.FileDrop);
                if (drop == null) { return null; }

                string[] files = drop as string[];
                if (files == null) { return null; }
                if (files.Length != 1) { return null; }

                string file = files[0];
                string extension = System.IO.Path.GetExtension(file);
                switch (extension.ToUpper()) {
                    case ".XML":
                    case ".DIAGRAM":
                        return file;
                    default:
                        return null;
                }
            }

            // Invalid DataObject. Return Null.
            return null;
        }
        public static bool ValidDataObject(DragEventArgs e) {
            if (e.Data.GetDataPresent("ESRI Names")) { return true; }
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) { return true; }
            return false;
        }
        public static string ExportXml(DragEventArgs e) {
            // Get dropped object
            if (!e.Data.GetDataPresent("ESRI Names")) { return null; }
            object drop = e.Data.GetData("ESRI Names");

            // Convert to byte array
            MemoryStream memoryStream = (MemoryStream)drop;
            byte[] bytes = memoryStream.ToArray();
            object byteArray = (object)bytes;

            // Get First WorkpaceName
            INameFactory nameFactory = new NameFactoryClass();
            IEnumName enumName = nameFactory.UnpackageNames(ref byteArray);
            IName name = enumName.Next();
            IWorkspaceName workspaceName = name as IWorkspaceName;
            if (workspaceName != null){
                return GeodatabaseUtility.ExportWorkspaceXml(workspaceName);
            };

            MessageBox.Show(
                Resources.TEXT_DROPPED_OBJECT_NOT_VALID_GEODATABASE,
                Resources.TEXT_APPLICATION,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            return null;
        }
        private static string ExportWorkspaceXml(IWorkspaceName workspaceName) {
            // Exclude FileSystemWorkspaces
            switch (workspaceName.Type) {
                case esriWorkspaceType.esriLocalDatabaseWorkspace:
                case esriWorkspaceType.esriRemoteDatabaseWorkspace:
                    break;
                case esriWorkspaceType.esriFileSystemWorkspace:
                default:
                    MessageBox.Show(
                        Resources.TEXT_DROPPED_OBJECT_NOT_GEODATABASE,
                        Resources.TEXT_APPLICATION,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    return null;
            }

            // Get Workspace
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();

            // Create Temporary File
            string path = System.IO.Path.GetTempPath();
            string file = string.Format("{0}.{1}", Guid.NewGuid().ToString("N").ToUpper(), "xml");
            string outfile = System.IO.Path.Combine(path, file);
  
            //
            IGdbXmlExport gdbXmlExport = new GdbExporterClass();
            try {
                gdbXmlExport.ExportWorkspaceSchema(workspace, outfile, false, true);
            }
            catch (COMException ex) {
                // Handle Exception
                ExceptionDialog.HandleException(ex);

                // Display Informative Error Message
                switch (ex.ErrorCode) {
                    case -2147220967: // 0x80040219
                        // Invalid network weight association.
                        // Connection to ESRI OLE DB provider is invalid.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_1A + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_1B,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147220735: // 0x80040301
                        // The dataset was not found.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_2,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147220655: // 0x80040351
                        // The table was not found
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_3,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147220473: // 0x80040407
                        // The feature class' default subtype code cannot be retrieved or is invalid.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_4,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147216558: // 0x80041352
                        // Unable to instantiate object class extension COM component.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_5,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147216100: // 0x8004151C
                        // You have insufficient permissions to access one or more datasets.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_6,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case -2147216086: // 0x8004152A
                        // Specified attribute column doesn't exist.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_7,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    default:
                        // Display a general error message.
                        MessageBox.Show(
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_8A + Environment.NewLine +
                            Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_8B,
                            Resources.TEXT_APPLICATION,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                }

                // Exit Method
                return null;
            }
            catch (Exception ex) {
                // Handle Exception
                ExceptionDialog.HandleException(ex);

                // Display a general error message.
                MessageBox.Show(
                    Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED + Environment.NewLine +
                    Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_9A + Environment.NewLine +
                    Resources.TEXT_GEODATABASE_SCHEMA_EXPORT_FAILED_9B,
                    Resources.TEXT_APPLICATION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                // Exit Method
                return null;
            }

            // Clear Geodatabase Exporter
            gdbXmlExport = null;

            // Return pathname to Xml File
            return outfile;
        }
        //private static string ExportGeometricNetworkXml(IGeometricNetworkName geometricNetworkName) {
        //    // Get Workspace
        //    IName name = (IName)geometricNetworkName;
        //    //IGeometricNetwork geometricNetwork = (IGeometricNetwork)name.Open();

        //    // Create Temporary File
        //    string path = System.IO.Path.GetTempPath();
        //    string file = string.Format("{0}.{1}", Guid.NewGuid().ToString("N").ToUpper(), "xml");
        //    string outfile = System.IO.Path.Combine(path, file);

        //    //
        //    IEnumName enumName = new NamesEnumeratorClass();
        //    IEnumNameEdit enumNameEdit = (IEnumNameEdit)enumName;
        //    enumNameEdit.Add(name);

        //    IScratchWorkspaceFactory scratchWorkspaceFactory = new ScratchWorkspaceFactoryClass();
        //    IWorkspace workspaceScratch = scratchWorkspaceFactory.CreateNewScratchWorkspace();
        //    IDataset datasetScratch = (IDataset)workspaceScratch;
        //    IName nameScratch = datasetScratch.FullName;

        //    IEnumNameMapping enumNameMapping = null;

        //    IGeoDBDataTransfer geoDBDataTransfer = new GeoDBDataTransferClass();
        //    bool boolHaveConflicts = geoDBDataTransfer.GenerateNameMapping(enumName, nameScratch, out enumNameMapping);
        //    if (boolHaveConflicts) {
        //        MessageBox.Show(
        //                "Problems encountered generating 'name mapping' for geomeric network.",
        //                Resources.TEXT_APPLICATION,
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Error,
        //                MessageBoxDefaultButton.Button1,
        //                MessageBoxOptions.DefaultDesktopOnly);
        //        return null;
        //    }

        //    //
        //    IGdbXmlExport gdbXmlExport = new GdbExporterClass();
        //    try {
        //        gdbXmlExport.ExportDatasetsSchema(enumNameMapping, outfile, false, true);
        //    }
        //    catch (COMException ex) {
        //        // Handle Exception
        //        ExceptionDialog.HandleException(ex);
        //    }

        //    // Clear Geodatabase Exporter
        //    gdbXmlExport = null;

        //    // Return pathname to Xml File
        //    return outfile;
        //}
        public static string GetDescription(esriNetworkAttributeUnits units) {
            switch (units) {
                case esriNetworkAttributeUnits.esriNAUCentimeters:
                    return "Centimeters";
                case esriNetworkAttributeUnits.esriNAUDays:
                    return "Days";
                case esriNetworkAttributeUnits.esriNAUDecimalDegrees:
                    return "DecimalDegrees";
                case esriNetworkAttributeUnits.esriNAUDecimeters:
                    return "Decimeters";
                case esriNetworkAttributeUnits.esriNAUFeet:
                    return "Feet";
                case esriNetworkAttributeUnits.esriNAUHours:
                    return "Hours";
                case esriNetworkAttributeUnits.esriNAUInches:
                    return "Inches";
                case esriNetworkAttributeUnits.esriNAUKilometers:
                    return "Kilometers";
                case esriNetworkAttributeUnits.esriNAUMeters:
                    return "Meters";
                case esriNetworkAttributeUnits.esriNAUMiles:
                    return "Miles";
                case esriNetworkAttributeUnits.esriNAUMillimeters:
                    return "Millimeters";
                case esriNetworkAttributeUnits.esriNAUMinutes:
                    return "Minutes";
                case esriNetworkAttributeUnits.esriNAUNauticalMiles:
                    return "NauticalMiles";
                case esriNetworkAttributeUnits.esriNAUSeconds:
                    return "Seconds";
                case esriNetworkAttributeUnits.esriNAUYards:
                    return "Yards";
                case esriNetworkAttributeUnits.esriNAUUnknown:
                default:
                    return "Unknown";
            }
        }
        public static string GetDescription(esriTopologyRuleType rule) {
            switch (rule) {
                case esriTopologyRuleType.esriTRTAny:
                    return "Any Rule";
                case esriTopologyRuleType.esriTRTFeatureLargerThanClusterTolerance:
                    return "The rule is a feature to be deleted is smaller than the cluster tolerance rule";
                case esriTopologyRuleType.esriTRTAreaNoGaps:
                    return "Must not have gaps";
                case esriTopologyRuleType.esriTRTAreaNoOverlap:
                    return "Must not overlap";
                case esriTopologyRuleType.esriTRTAreaCoveredByAreaClass:
                    return "Must be covered by feature class of";
                case esriTopologyRuleType.esriTRTAreaAreaCoverEachOther:
                    return "Must cover each other";
                case esriTopologyRuleType.esriTRTAreaCoveredByArea:
                    return "Must be covered by";
                case esriTopologyRuleType.esriTRTAreaNoOverlapArea:
                    return "Must not overlap with";
                case esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary:
                    return "Must be covered by boundary of";
                case esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary:
                    return "Must be covered by boundary of";
                case esriTopologyRuleType.esriTRTPointProperlyInsideArea:
                    return "Must be properly inside polygons";
                case esriTopologyRuleType.esriTRTLineNoOverlap:
                    return "Must not overlap";
                case esriTopologyRuleType.esriTRTLineNoIntersection:
                    return "Must not intersect";
                case esriTopologyRuleType.esriTRTLineNoDangles:
                    return "Must not have dangles";
                case esriTopologyRuleType.esriTRTLineNoPseudos:
                    return "Must not have pseudo-nodes";
                case esriTopologyRuleType.esriTRTLineCoveredByLineClass:
                    return "Must be covered by feature class of";
                case esriTopologyRuleType.esriTRTLineNoOverlapLine:
                    return "Must not overlap with";
                case esriTopologyRuleType.esriTRTPointCoveredByLine:
                    return "Point must be covered by line";
                case esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint:
                    return "Must be covered by endpoint of";
                case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine:
                    return "Boundary must be covered by";
                case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary:
                    return "Area boundary must be covered by boundary of";
                case esriTopologyRuleType.esriTRTLineNoSelfOverlap:
                    return "Must not self overlap";
                case esriTopologyRuleType.esriTRTLineNoSelfIntersect:
                    return "Must not self intersect";
                case esriTopologyRuleType.esriTRTLineNoIntersectOrInteriorTouch:
                    return "Must not intersect or touch interior";
                case esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint:
                    return "Endpoint must be covered by";
                case esriTopologyRuleType.esriTRTAreaContainPoint:
                    return "Contains point";
                case esriTopologyRuleType.esriTRTLineNoMultipart:
                    return "Must be single part";
                default:
                    return "Unknown";
            }
        }
        public static string GetDescription(rstPixelType type) {
            switch (type) {
                case rstPixelType.PT_U1: return "U1";
                case rstPixelType.PT_U2: return "U2";
                case rstPixelType.PT_U4: return "U4";
                case rstPixelType.PT_UCHAR: return "U8";
                case rstPixelType.PT_CHAR: return "S8";
                case rstPixelType.PT_USHORT: return "U16";
                case rstPixelType.PT_SHORT: return "S16";
                case rstPixelType.PT_ULONG: return "U32";
                case rstPixelType.PT_LONG: return "S32";
                case rstPixelType.PT_FLOAT: return "F32";
                case rstPixelType.PT_DOUBLE: return "F64";
                case rstPixelType.PT_COMPLEX: return "C64";
                case rstPixelType.PT_DCOMPLEX: return "C128";
                case rstPixelType.PT_UNKNOWN:
                default:
                    return "UNKNOWN";
            }
        }
        public static string GetDescription(esriGeometryType type) {
            switch (type) {
                case esriGeometryType.esriGeometryAny: return "Any";
                case esriGeometryType.esriGeometryBag: return "Bag";
                case esriGeometryType.esriGeometryBezier3Curve: return "Bezier Curve";
                case esriGeometryType.esriGeometryCircularArc: return "Circular Arc";
                case esriGeometryType.esriGeometryEllipticArc: return "Elliptic Arc";
                case esriGeometryType.esriGeometryEnvelope: return "Envelope";
                case esriGeometryType.esriGeometryLine: return "Line";
                case esriGeometryType.esriGeometryMultiPatch: return "MultiPatch";
                case esriGeometryType.esriGeometryMultipoint: return "Multipoint";
                case esriGeometryType.esriGeometryNull: return "Null";
                case esriGeometryType.esriGeometryPath: return "Path";
                case esriGeometryType.esriGeometryPoint: return "Point";
                case esriGeometryType.esriGeometryPolygon: return "Polygon";
                case esriGeometryType.esriGeometryPolyline: return "Polyline";
                case esriGeometryType.esriGeometryRay: return "Ray";
                case esriGeometryType.esriGeometryRing: return "Ring";
                case esriGeometryType.esriGeometrySphere: return "Sphere";
                case esriGeometryType.esriGeometryTriangleFan: return "Triangle Fan";
                case esriGeometryType.esriGeometryTriangles: return "Triangles";
                case esriGeometryType.esriGeometryTriangleStrip: return "Triangle Strip";
                default:
                    return "UNKNOWN";
            }
        }
        public static string GetDescription(esriFeatureType type) {
            switch (type) {
                case esriFeatureType.esriFTAnnotation: return "Annotation";
                case esriFeatureType.esriFTComplexEdge: return "Complex Edge";
                case esriFeatureType.esriFTComplexJunction: return "Complex Junction";
                case esriFeatureType.esriFTCoverageAnnotation: return "Coverage Annotation";
                case esriFeatureType.esriFTDimension: return "Dimension";
                case esriFeatureType.esriFTRasterCatalogItem: return "Raster Catalog";
                case esriFeatureType.esriFTSimple: return "Simple";
                case esriFeatureType.esriFTSimpleEdge: return "Simple Edge";
                case esriFeatureType.esriFTSimpleJunction: return "Simple Junction";
                default:
                    return "UNKNOWN";
            }
        }
        public static string GetDescription(esriDatasetType type) {
            switch (type) {
                case esriDatasetType.esriDTAny: return "Any";
                case esriDatasetType.esriDTCadastralFabric: return "Cadastral Fabric";
                case esriDatasetType.esriDTCadDrawing: return "Cad Drawing";
                case esriDatasetType.esriDTContainer: return "Container";
                case esriDatasetType.esriDTFeatureClass: return "Feature Class";
                case esriDatasetType.esriDTFeatureDataset: return "Feature Dataset";
                case esriDatasetType.esriDTGeo: return "Geo";
                case esriDatasetType.esriDTGeometricNetwork: return "Geometric Network";
                case esriDatasetType.esriDTLocator: return "Locator";
                case esriDatasetType.esriDTNetworkDataset: return "Network Dataset";
                case esriDatasetType.esriDTPlanarGraph: return "Planar Graph";
                case esriDatasetType.esriDTRasterBand: return "Raster Band";
                case esriDatasetType.esriDTRasterCatalog: return "Raster Catalog";
                case esriDatasetType.esriDTRasterDataset: return "Raster Dataset";
                case esriDatasetType.esriDTRelationshipClass: return "Relationship Class";
                case esriDatasetType.esriDTRepresentationClass: return "Representation Class";
                case esriDatasetType.esriDTSchematicDataset: return "Schematic Dataset";
                case esriDatasetType.esriDTTable: return "Table";
                case esriDatasetType.esriDTTerrain: return "Terrain";
                case esriDatasetType.esriDTText: return "Text";
                case esriDatasetType.esriDTTin: return "Tin";
                case esriDatasetType.esriDTTool: return "Tool";
                case esriDatasetType.esriDTToolbox: return "Toolbox";
                case esriDatasetType.esriDTTopology: return "Topology";
                default:
                    return "UNKNOWN";
            }
        }
        public static string GetDescription(esriTinSurfaceType type) {
            switch (type) { 
                case esriTinSurfaceType.esriTinContour: return "Inputs as contour lines.";
                case esriTinSurfaceType.esriTinHardLine: return "Inputs as hard break lines.";
                case esriTinSurfaceType.esriTinHardClip: return "Inputs as hard clipping polygons.";
                case esriTinSurfaceType.esriTinHardErase: return "Inputs as hard erase polygons.";
                case esriTinSurfaceType.esriTinHardReplace: return "Inputs as hard replace polygons.";
                case esriTinSurfaceType.esriTinHardValueFill: return "Inputs as hard value polygons.";
                case esriTinSurfaceType.esriTinZLessHardLine: return "Inputs as Z-less hard break lines.";
                case esriTinSurfaceType.esriTinZLessHardClip: return "Inputs as Z-less hard clipping polygons.";
                case esriTinSurfaceType.esriTinZLessHardErase: return "Inputs as Z-less hard erase polygons.";
                case esriTinSurfaceType.esriTinSoftLine: return "Inputs as soft break lines."; 
                case esriTinSurfaceType.esriTinSoftClip: return "Inputs as soft clipping polygons.";
                case esriTinSurfaceType.esriTinSoftErase: return "Inputs as soft erase polygons.";
                case esriTinSurfaceType.esriTinSoftReplace: return "Inputs as soft replace polygons.";
                case esriTinSurfaceType.esriTinSoftValueFill: return "Inputs as soft value polygons.";
                case esriTinSurfaceType.esriTinZLessSoftLine: return "Inputs as Z-less soft break lines."; 
                case esriTinSurfaceType.esriTinZLessContour: return "Inputs as Z-less soft contour lines.";
                case esriTinSurfaceType.esriTinZLessSoftClip: return "Inputs as Z-less soft clipping polygons."; 
                case esriTinSurfaceType.esriTinZLessSoftErase: return "Inputs as Z-less soft erase polygons.";
                case esriTinSurfaceType.esriTinMassPoint: return "Inputs as mass points.";
                default:
                    return "UNKNOWN";
            }
        }
        public static esriNetworkAttributeUnits GetNetworkAttributeUnits(string description) {
            switch (description) {
                case "Centimeters":
                    return esriNetworkAttributeUnits.esriNAUCentimeters;
                case "Days":
                    return esriNetworkAttributeUnits.esriNAUDays;
                case "DecimalDegrees":
                    return esriNetworkAttributeUnits.esriNAUDecimalDegrees;
                case "Decimeters":
                    return esriNetworkAttributeUnits.esriNAUDecimeters;
                case "Feet":
                    return esriNetworkAttributeUnits.esriNAUFeet;
                case "Hours":
                    return esriNetworkAttributeUnits.esriNAUHours;
                case "Inches":
                    return esriNetworkAttributeUnits.esriNAUInches;
                case "Kilometers":
                    return esriNetworkAttributeUnits.esriNAUKilometers;
                case "Meters":
                    return esriNetworkAttributeUnits.esriNAUMeters;
                case "Miles":
                    return esriNetworkAttributeUnits.esriNAUMiles;
                case "Millimeters":
                    return esriNetworkAttributeUnits.esriNAUMillimeters;
                case "Minutes":
                    return esriNetworkAttributeUnits.esriNAUMinutes;
                case "NauticalMiles":
                    return esriNetworkAttributeUnits.esriNAUNauticalMiles;
                case "Seconds":
                    return esriNetworkAttributeUnits.esriNAUSeconds;
                case "Yards":
                    return esriNetworkAttributeUnits.esriNAUYards;
                case "Unknown":
                default:
                    return esriNetworkAttributeUnits.esriNAUUnknown;
            }
        }
        public static rstPixelType GetPixelType(string description) {
            switch (description) {
                case "U1": return rstPixelType.PT_U1;
                case "U2": return rstPixelType.PT_U2;
                case "U4": return rstPixelType.PT_U4;
                case "U8": return rstPixelType.PT_UCHAR;
                case "S8": return rstPixelType.PT_CHAR;
                case "U16": return rstPixelType.PT_USHORT;
                case "S16": return rstPixelType.PT_SHORT;
                case "U32": return rstPixelType.PT_ULONG;
                case "S32": return rstPixelType.PT_LONG;
                case "F32": return rstPixelType.PT_FLOAT;
                case "F64": return rstPixelType.PT_DOUBLE;
                case "C64": return rstPixelType.PT_COMPLEX;
                case "C128": return rstPixelType.PT_DCOMPLEX;
                default:
                    return rstPixelType.PT_UNKNOWN;
            }
        }
        public static string GetTableName(string name){
            if (string.IsNullOrEmpty(name)) { return name; }
            string[] split = name.Split(new char[] { char.Parse(".") });
            string sub = split[split.Length - 1];
            return sub;
        }
        public static bool IsValidateValue(esriFieldType fieldType, string value, out string message) {
            if (string.IsNullOrEmpty(value)) {
                message = "is null or empty";
                return false;
            }

            switch (fieldType) {
                case esriFieldType.esriFieldTypeSmallInteger:
                    short valueTest1;
                    bool validTest1 = short.TryParse(value, out valueTest1);
                    message = validTest1 ? string.Empty : string.Format("is not a valid small integer");
                    return validTest1;
                case esriFieldType.esriFieldTypeInteger:
                    int valueTest2;
                    bool validTest2 = int.TryParse(value, out valueTest2);
                    message = validTest2 ? string.Empty : string.Format("is not a valid integer");
                    return validTest2;
                case esriFieldType.esriFieldTypeSingle:
                    float valueTest3;
                    bool validTest3 = float.TryParse(value, out valueTest3);
                    message = validTest3 ? string.Empty : string.Format("is not a valid single/float");
                    return validTest3;
                case esriFieldType.esriFieldTypeDouble:
                    double valueTest4;
                    bool validTest4 = double.TryParse(value, out valueTest4);
                    message = validTest4 ? string.Empty : string.Format("is not a valid double");
                    return validTest4;
                case esriFieldType.esriFieldTypeString:
                    message = string.Empty;
                    return true;
                case esriFieldType.esriFieldTypeDate:
                    CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                    DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.DateTimeFormat;
                    DateTime valueTest5;
                    bool validTest5 = DateTime.TryParseExact(value, dateTimeFormatInfo.SortableDateTimePattern, cultureInfo, DateTimeStyles.None, out valueTest5);
                    message = validTest5 ? string.Empty : string.Format("is not a valid date");
                    return validTest5;
                default:
                    message = string.Format("field type [{0}] does not support default values", fieldType.ToString());
                    return false;
            }
        }
        /// <summary>
        /// Convert from .Net Color to ESRI Color
        /// </summary>
        /// <param name="netColor">.NET Color</param>
        /// <returns>ESRI Color</returns>
        public static IColor ToESRIColor(Color netColor) {
            IRgbColor esriColor = new RgbColorClass() {
                Red = Convert.ToInt32(netColor.R),
                Green = Convert.ToInt32(netColor.G),
                Blue = Convert.ToInt32(netColor.B),
                Transparency = netColor.A
            };
            return (IColor)esriColor;
        }
        /// <summary>
        /// Create Bitmap From ESRI Dataset
        /// </summary>
        /// <param name="dataset">Dataset to generate an image from</param>
        /// <param name="imageFormat">Output image format</param>
        /// <param name="size">Size of output image</param>
        /// <param name="resolution">Resolution of output image (dpi)</param>
        /// <param name="background">Background color</param>
        /// <param name="filename">Ouput filename</param>
        public static void CreateBitmap(IDataset dataset, esriImageFormat imageFormat, Size size, ushort resolution, Color background, string filename) {
            ILayer layer = null;

            switch (dataset.Type) {
                case esriDatasetType.esriDTFeatureClass:
                    IFeatureClass featureClass = (IFeatureClass)dataset;
                    switch (featureClass.FeatureType) {
                        case esriFeatureType.esriFTDimension:
                            layer = new DimensionLayerClass();
                            break;
                        case esriFeatureType.esriFTAnnotation:
                            layer = new FeatureLayerClass();
                            IGeoFeatureLayer geoFeaureLayer = (IGeoFeatureLayer)layer;
                            geoFeaureLayer.DisplayAnnotation = true;
                            break;
                        case esriFeatureType.esriFTComplexEdge:
                        case esriFeatureType.esriFTComplexJunction:
                        case esriFeatureType.esriFTSimple:
                        case esriFeatureType.esriFTSimpleEdge:
                        case esriFeatureType.esriFTSimpleJunction:
                            layer = new FeatureLayerClass();
                            break;
                    }
                    if (layer == null) { return; }

                    IFeatureLayer featureLayer = (IFeatureLayer)layer;
                    featureLayer.FeatureClass = featureClass;

                    break;
                case esriDatasetType.esriDTRasterDataset:
                    layer = new RasterLayerClass();
                    IRasterLayer rasterLayer = (IRasterLayer)layer;
                    rasterLayer.CreateFromDataset((IRasterDataset)dataset);
                    break;
                default:
                    string message = string.Format("[{0}] is not supported", dataset.Type.ToString());
                    throw new Exception(message);
            }
            if (layer == null) { return; }

            // Create In-memory Map
            IMap map = new MapClass();
            map.AddLayer(layer);
            IActiveView activeView = (IActiveView)map;
            IExport export = null;
            tagRECT rect = new tagRECT();

            // Set Format Specific Properties
            switch (imageFormat) {
                case esriImageFormat.esriImageJPG:
                    export = new ExportJPEGClass();
                    IExportJPEG exportJpeg = (IExportJPEG)export;
                    exportJpeg.ProgressiveMode = false;
                    exportJpeg.Quality = 100;

                    break;
                default:
                    throw new Exception("[" + imageFormat.ToString() + "] is not supported");
            }
            if (export == null) {
                throw new Exception("Failed to Created Exporter");
            }

            // Set Background
            if ((export is IExportBMP) ||
                (export is IExportGIF) ||
                (export is IExportJPEG) ||
                (export is IExportPNG) ||
                (export is IExportTIFF)) {
                IExportImage exportImage = (IExportImage)export;
                exportImage.ImageType = esriExportImageType.esriExportImageTypeTrueColor;
                exportImage.BackgroundColor = GeodatabaseUtility.ToESRIColor(background);
            }

            // Set Export Frame
            rect = activeView.ExportFrame;
            rect.left = 0;
            rect.top = 0;
            rect.right = size.Width;
            rect.bottom = size.Height;

            // Set Output Extent
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(rect.left, rect.top, rect.right, rect.bottom);
            export.PixelBounds = envelope;
            export.Resolution = resolution;
            export.ExportFileName = filename;

            // Export map to image
            int intHdc = export.StartExporting();
            activeView.Output(intHdc, resolution, ref rect, null, null);
            export.FinishExporting();
            export.Cleanup();

            // Clear Layers
            map.ClearLayers();

            // Release COM Objects
            GeodatabaseUtility.ReleaseComObject(layer);
            GeodatabaseUtility.ReleaseComObject(envelope);
            GeodatabaseUtility.ReleaseComObject(map);
            GeodatabaseUtility.ReleaseComObject(activeView);
            GeodatabaseUtility.ReleaseComObject(export);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public static void ReleaseComObject(object o) {
            if (o == null) { return; }
            if (!Marshal.IsComObject(o)) { return; }
            while (Marshal.ReleaseComObject(o) > 0) {
                continue;
            }
            o = null;
        }
    }
}

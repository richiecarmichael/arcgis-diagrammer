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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Diagrammer.Properties;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// Class that creates template geodatabase objects
    /// </summary>
    public static class Factory {
        private static XPathNodeIterator LoadXml(string fileName, string path) {
            // Create Navigator
            StringReader reader = new StringReader(fileName);
            XPathDocument document = new XPathDocument(reader);
            reader.Close();
            XPathNavigator navigator = document.CreateNavigator();

            // Get <esri:Workspace>
            bool ok = navigator.MoveToFirstChild();

            //
            XmlNamespaceManager namesp = new XmlNamespaceManager(navigator.NameTable);
            namesp.AddNamespace(Xml._XSI, Xml.XMLSCHEMAINSTANCE);
            namesp.AddNamespace(Xml._ESRI, Xml.ESRISCHEME92);

            // Select Data Element
            XPathNodeIterator iterator = navigator.Select(path, namesp);

            // Return Navigator
            return iterator;
        }
        public static Domain CreateDomain(Type type) {
            Domain domain = null;

            // Load XML Workspace Document Text
            if (type == typeof(DomainCodedValue)) {
                XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_DOMAIN_CODEDVALUE, "WorkspaceDefinition/Domains/Domain");
                if (iterator.MoveNext()) {
                    domain = new DomainCodedValue(iterator.Current);
                }
            }
            else if (type == typeof(DomainRange)) {
                XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_DOMAIN_RANGE, "WorkspaceDefinition/Domains/Domain");
                if (iterator.MoveNext()) {
                    domain = new DomainRange(iterator.Current);
                }
            }

            return domain;
        }
        public static FeatureClass CreateFeatureClass() {
            return Factory.CreateFeatureClass(esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryNull);
        }
        public static FeatureClass CreateFeatureClass(esriFeatureType featureType) {
            return Factory.CreateFeatureClass(featureType, esriGeometryType.esriGeometryNull);
        }
        public static FeatureClass CreateFeatureClass(esriFeatureType featureType, esriGeometryType geometryType) {
            XPathNodeIterator iterator = null;
            switch (featureType) {
                case esriFeatureType.esriFTSimple:
                    switch (geometryType) {
                        case esriGeometryType.esriGeometryMultiPatch:
                            iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_MULTIPATCH, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                            break;
                        case esriGeometryType.esriGeometryPoint:
                            iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_POINT, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_POLYGON, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                            break;
                        case esriGeometryType.esriGeometryPolyline:
                            iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_POLYLINE, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                            break;
                    }
                    break;
                case esriFeatureType.esriFTAnnotation:
                    iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_ANNOTATION, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                    break;
                case esriFeatureType.esriFTDimension:
                    iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_DIMENSION, "WorkspaceDefinition/DatasetDefinitions/DataElement");
                    break;
            }
            if (iterator == null) { return null; }
            if (!iterator.MoveNext()) { return null; }

            XPathNavigator navigator = iterator.Current;
            FeatureClass featureClass = new FeatureClass(navigator); ;
            return featureClass;
        }
        public static FeatureDataset CreateFeatureDataset() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_FEATUREDATASET, "WorkspaceDefinition/DatasetDefinitions/DataElement");
            if (!iterator.MoveNext()) { return null; }
            return new FeatureDataset(iterator.Current);
        }
        public static GeometricNetwork CreateGeometricNetwork() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_GEOMETRIC_NETWORK, "WorkspaceDefinition/DatasetDefinitions/DataElement/Children/DataElement[@xsi:type='esri:DEGeometricNetwork']");
            if (!iterator.MoveNext()) { return null; }
            return new GeometricNetwork(iterator.Current);
        }
        public static Network CreateNetwork() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_NETWORK, "WorkspaceDefinition/DatasetDefinitions/DataElement/Children/DataElement[@xsi:type='esri:DENetworkDataset']");
            if (!iterator.MoveNext()) { return null; }
            return new Network(iterator.Current);
        }
        public static ObjectClass CreateObjectClass() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_TABLE, "WorkspaceDefinition/DatasetDefinitions/DataElement");
            if (!iterator.MoveNext()) { return null; }
            return new ObjectClass(iterator.Current);
        }
        public static RasterBand CreateRasterBand() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_RASTERBAND, "WorkspaceDefinition/DatasetDefinitions/DataElement/Children/DataElement");
            if (!iterator.MoveNext()) { return null; }
            return new RasterBand(iterator.Current);
        }
        public static RasterCatalog CreateRasterCatalog() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_RASTERCATALOG, "WorkspaceDefinition/DatasetDefinitions/DataElement");
            if (!iterator.MoveNext()) { return null; }
            return new RasterCatalog(iterator.Current);
        }
        public static RasterDataset CreateRasterDataset() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_RASTERDATASET, "WorkspaceDefinition/DatasetDefinitions/DataElement");
            if (!iterator.MoveNext()) { return null; }
            return new RasterDataset(iterator.Current);
        }
        public static RelationshipClass CreateRelationshipClass() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_RELATIONSHIPCLASS, "WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DERelationshipClass']");
            if (!iterator.MoveNext()) { return null; }
            return new RelationshipClass(iterator.Current);
        }
        public static Subtype CreateSubtype() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_SUBTYPE, "WorkspaceDefinition/DatasetDefinitions/DataElement/Subtypes/Subtype");
            if (!iterator.MoveNext()) { return null; }
            return new Subtype(iterator.Current);
        }
        public static Terrain CreateTerrain() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_TERRAIN, "WorkspaceDefinition/DatasetDefinitions/DataElement/Children/DataElement[@xsi:type='esri:DETerrain']");
            if (!iterator.MoveNext()) { return null; }
            return new Terrain(iterator.Current);
        }
        public static Topology CreateTopology() {
            XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_TOPOLOGY, "WorkspaceDefinition/DatasetDefinitions/DataElement/Children/DataElement[@xsi:type='esri:DETopology']");
            if (!iterator.MoveNext()) { return null; }
            return new Topology(iterator.Current);
        }
        public static List<Domain> RequiredDomains(FeatureClass featureClass) {
            List<Domain> domains = new List<Domain>();
            switch (featureClass.FeatureType) {
                case esriFeatureType.esriFTSimple:
                    switch (featureClass.ShapeType) {
                        case esriGeometryType.esriGeometryMultiPatch:
                        case esriGeometryType.esriGeometryPoint:
                        case esriGeometryType.esriGeometryPolygon:
                        case esriGeometryType.esriGeometryPolyline:
                            break;
                    }
                    break;
                case esriFeatureType.esriFTAnnotation:
                    XPathNodeIterator iterator = Factory.LoadXml(Resources.FILE_FEATURECLASS_ANNOTATION, "WorkspaceDefinition/Domains/Domain");

                    // Add Domains
                    while (iterator.MoveNext()) {
                        // Get Domain
                        XPathNavigator domain = iterator.Current;

                        // Create Namespace
                        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(domain.NameTable);
                        namespaceManager.AddNamespace(Xml._XSI, Xml.XMLSCHEMAINSTANCE);

                        // Create Domain
                        XPathNavigator type = domain.SelectSingleNode(Xml._XSITYPE, namespaceManager);
                        switch (type.Value) {
                            case Xml._RANGEDOMAIN:
                                domains.Add(new DomainRange(domain));
                                break;
                            case Xml._CODEDVALUEDOMAIN:
                                domains.Add(new DomainCodedValue(domain));
                                break;
                        }
                    }
                    break;
                case esriFeatureType.esriFTDimension:
                    break;
            }
            return domains;
        }
    }
}

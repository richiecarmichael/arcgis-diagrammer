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

namespace ESRI.ArcGIS.Diagrammer {
    public static class Xml {
        // Namespaces
        public const string XMLSCHEMA = "http://www.w3.org/2001/XMLSchema";
        public const string XMLSCHEMAINSTANCE = "http://www.w3.org/2001/XMLSchema-instance";
        public const string ESRISCHEME92 = "http://www.esri.com/schemas/ArcGIS/9.2";
        public const string ESRISCHEME93 = "http://www.esri.com/schemas/ArcGIS/9.3";

        // ESRI Attributes
        public const string _ARRAYOFCONNECTIVITYRULE = "esri:ArrayOfConnectivityRule";
        public const string _ARRAYOFDATAELEMENT = "esri:ArrayOfDataElement";
        public const string _ARRAYOFDOMAIN = "esri:ArrayOfDomain";
        public const string _ARRAYOFNETWEIGHT = "esri:ArrayOfNetWeight";
        public const string _ARRAYOFNETWEIGHTASSOCIATION = "esri:ArrayOfNetWeightAssociation";
        public const string _ARRAYOFRELATIONSHIPCLASSKEY = "esri:ArrayOfRelationshipClassKey";
        public const string _ARRAYOFRELATIONSHIPRULE = "esri:ArrayOfRelationshipRule";
        public const string _ARRAYOFSTRING = "esri:ArrayOfString";
        public const string _ARRAYOFTERRAINDATASOURCE = "esri:ArrayOfTerrainDataSource";
        public const string _ARRAYOFTERRAINPYRAMIDLEVELWINDOWSIZE = "esri:ArrayOfTerrainPyramidLevelWindowSize";
        public const string _ARRAYOFTERRAINPYRAMIDLEVELZTOL = "esri:ArrayOfTerrainPyramidLevelZTol";
        public const string _ARRAYOFTOPOLOGYRULE = "esri:ArrayOfTopologyRule";
        public const string _CODEDVALUEDOMAIN = "esri:CodedValueDomain";
        public const string _DEFEATURECLASS = "esri:DEFeatureClass";
        public const string _DEFEATUREDATASET = "esri:DEFeatureDataset";
        public const string _DEGEOMETRICNETWORK = "esri:DEGeometricNetwork";
        public const string _DENETWORKDATASET = "esri:DENetworkDataset";
        public const string _DERASTERBAND = "esri:DERasterBand";
        public const string _DERASTERCATALOG = "esri:DERasterCatalog";
        public const string _DERASTERDATASET = "esri:DERasterDataset";
        public const string _DERELATIONSHIPCLASS = "esri:DERelationshipClass";
        public const string _DETABLE = "esri:DETable";
        public const string _DETERRAIN = "esri:DETerrain";
        public const string _DETOPOLOGY = "esri:DETopology";
        public const string _JUNCTIONSUBTYPE = "esri:JunctionSubtype";
        public const string _GEOMETRICNETWORKMEMBERSHIP = "esri:GeometricNetworkMembership";
        public const string _NAMES = "esri:Names";
        public const string _NETWORKDATASETMEMBERSHIP = "esri:NetworkDatasetMembership";
        public const string _RANGEDOMAIN = "esri:RangeDomain";
        public const string _RELATIONSHIPCLASSKEY = "esri:RelationshipClassKey";
        public const string _TERRAINMEMBERSHIP = "esri:TerrainMembership";
        public const string _TERRAINDATASOURCE = "esri:TerrainDataSource";
        public const string _TERRAINPYRAMIDLEVELWINDOWSIZE = "esri:TerrainPyramidLevelWindowSize";
        public const string _TERRAINPYRAMIDLEVELZTOL = "esri:TerrainPyramidLevelZTol";
        public const string _TOPOLOGYMEMBERSHIP = "esri:TopologyMembership";
        public const string _WORKSPACEDATA = "esri:WorkspaceData";
        public const string _WORKSPACEDEFINITION = "esri:WorkspaceDefinition";
        public const string _XMLPROPERTYSET = "esri:XmlPropertySet";

        // XML Attributes
        public const string _NIL = "nil";
        public const string _TRUE = "true";
        public const string _TYPE = "type";
        public const string _XSI = "xsi";
        public const string _XS = "xs";
        public const string _ESRI = "esri";
        public const string _XSITYPE = "@xsi:type";
        public const string _XMLNS = "xmlns";
        public const string _SHORT = "xs:short";
        public const string _INT = "xs:int";
        public const string _FLOAT = "xs:float";
        public const string _DOUBLE = "xs:double";
        public const string _STRING = "xs:string";
        public const string _DATETIME = "xs:dateTime";

        // Node Names
        public const string AVGNUMPOINTS = "AvgNumPoints";
        public const string GEOMETRYTYPE = "GeometryType";
        public const string HASM = "HasM";
        public const string HASZ = "HasZ";
        //public const string SPATIALREFERENCE = "SpatialReference";
        public const string GRIDSIZE0 = "GridSize0";
        public const string GRIDSIZE1 = "GridSize1";
        public const string GRIDSIZE2 = "GridSize2";

        public const string BACKWARDPATHLABEL = "BackwardPathLabel";
        public const string CANVERSION = "CanVersion";
        public const string CARDINALITY = "Cardinality";
        public const string CATALOGPATH = "CatalogPath";
        public const string CHILDREN = "Children";
        public const string CHILDRENEXPANDED = "ChildrenExpanded";
        public const string CLASSID = "ClassID";
        public const string CLASSKEY = "ClassKey";
        public const string CLASSKEYNAME = "ClassKeyName";
        public const string CLUSTERTOLERANCE = "ClusterTolerance";
        public const string CODE = "Code";
        public const string CONNECTIVITYRULES = "ConnectivityRules";
        public const string CONTROLLERMEMBERSHIP = "ControllerMembership";
        public const string DATAELEMENT = "DataElement";
        public const string DATASETDEFINITIONS = "DatasetDefinitions";
        public const string DATASETTYPE = "DatasetType";
        public const string DESCRIPTION = "Description";
        public const string DESTINATIONCLASSKEYS = "DestinationClassKeys";
        public const string DESTINATIONCLASSNAMES = "DestinationClassNames";
        public const string DOMAINNAME = "DomainName";
        public const string DSID = "DSID";
        public const string EXTENT = "Extent";
        public const string FEATURECLASSNAMES = "FeatureClassNames";
        public const string FIELDTYPE = "FieldType";
        public const string FORWARDPATHLABEL = "ForwardPathLabel";    
        public const string FULLPROPSRETRIEVED = "FullPropsRetrieved";
        public const string ISATTRIBUTED = "IsAttributed";
        public const string ISCOMPOSITE = "IsComposite";
        public const string ISREFLEXIVE = "IsReflexive";
        public const string JUNCTIONSUBTYPE = "JunctionSubtype";
        public const string KEYROLE = "KeyRole";
        public const string KEYTYPE = "KeyType";
        public const string MAXGENERATEDERRORCOUNT = "MaxGeneratedErrorCount";
        public const string MERGEPOLICY = "MergePolicy";
        public const string METADATA = "Metadata";
        public const string METADATARETRIEVED = "MetadataRetrieved";
        public const string NAME = "Name";
        public const string NETWORKWEIGHTS = "NetworkWeights";
        public const string NETWORKTYPE = "NetworkType";
        public const string NOTIFICATION = "Notification";
        public const string OBJECTKEYNAME = "ObjectKeyName";
        public const string ORIGINCLASSKEYS = "OriginClassKeys";
        public const string ORIGINCLASSNAMES = "OriginClassNames";
        public const string ORPHANJUNCTIONFEATURECLASSNAME = "OrphanJunctionFeatureClassName";
        public const string OWNER = "Owner";
        public const string RELATIONSHIPCLASSKEY = "RelationshipClassKey";
        public const string RELATIONSHIPRULE = "RelationshipRule";
        public const string RELATIONSHIPRULES = "RelationshipRules";
        public const string SPATIALREFERENCE = "SpatialReference";
        public const string SPLITPOLICY = "SplitPolicy";
        public const string SUBTYPECODE = "SubtypeCode";
        public const string TERRAINDATASOURCE = "TerrainDataSource";
        public const string TERRAINDATASOURCES = "TerrainDataSources";
        public const string TERRAINPYRAMIDLEVELWINDOWSIZE = "TerrainPyramidLevelWindowSize";
        public const string TERRAINPYRAMIDLEVELWINDOWSIZES = "TerrainPyramidLevelWindowSizes";
        public const string TERRAINPYRAMIDLEVELZTOL = "TerrainPyramidLevelZTol";
        public const string TERRAINPYRAMIDLEVELZTOLS = "TerrainPyramidLevelZTols";
        public const string TOPOLOGYRULE = "TopologyRule";
        public const string TOPOLOGYRULES = "TopologyRules";
        public const string VERSION = "Version";
        public const string VERSIONED = "Versioned";
        public const string WEIGHTASSOCIATIONS = "WeightAssociations";
        public const string WORKSPACE = "Workspace";
        public const string WORKSPACEDATA = "WorkspaceData";
        public const string WORKSPACETYPE = "WorkspaceType";
        public const string WORKSPACEDEFINITION = "WorkspaceDefinition";
        public const string XMLDOC = "XmlDoc";
        public const string ZCLUSTERTOLERANCE = "ZClusterTolerance";
    }
}

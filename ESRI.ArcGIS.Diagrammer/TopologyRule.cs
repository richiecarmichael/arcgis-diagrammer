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
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

//------------
// SAMPLE XML
//------------
//  <TopologyRule xsi:type="esri:TopologyRule">
//    <HelpString /> 
//    <RuleID>1</RuleID> 
//    <Name /> 
//    <GUID>C27042FF-A8D0-4A18-9C55-21EAD4495196</GUID> 
//    <TopologyRuleType>esriTRTAreaNoOverlap</TopologyRuleType> 
//    <OriginClassID>5</OriginClassID> 
//    <OriginSubtype>0</OriginSubtype> 
//    <DestinationClassID>5</DestinationClassID> 
//    <DestinationSubtype>0</DestinationSubtype> 
//    <TriggerErrorEvents>false</TriggerErrorEvents> 
//    <AllOriginSubtypes>true</AllOriginSubtypes> 
//    <AllDestinationSubtypes>true</AllDestinationSubtypes> 
//  </TopologyRule>

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// ESRI Topology Rule
    /// </summary>
    /// <remarks>
    /// The TopologyRule class is used to define the permissible spatial relationships between features within a topology.  Topology rules can be defined with a single feature class or between two feature classes.  Topology rules can also be defined to the subtype level of a feature class.  Topology rules have an origin and a destination feature class, either of which can be set to the subtype level.  Depending on the type of topology rule that is implemented, the destination feature class properties may be irrelevant.
    /// </remarks>
    [Serializable]
    public class TopologyRule : Rule {
        private string _name = string.Empty;
        private string _guid = string.Empty;
        private esriTopologyRuleType _topologyRuleType = esriTopologyRuleType.esriTRTAny;
        private int _originClassId = -1;
        private int _originSubtype = -1;
        private int _destinationClassId = -1;
        private int _destinationSubtype = -1;
        private bool _triggerErrorEvents = false;
        private bool _allOriginSubtypes = false;
        private bool _allDestinationSubtypes = false;
        //
        // CONSTRUCTOR
        //
        public TopologyRule() : base() {
            this._guid = System.Guid.NewGuid().ToString("D");
        }
        public TopologyRule(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <Name /> 
            XPathNavigator navigatorName = navigator.SelectSingleNode("Name");
            if (navigatorName != null) {
                this._name = navigatorName.Value;
            }

            // <GUID>C27042FF-A8D0-4A18-9C55-21EAD4495196</GUID> 
            XPathNavigator navigatorGUID = navigator.SelectSingleNode("GUID");
            if (navigatorGUID != null) {
                this._guid = navigatorGUID.Value;
            }

            // <TopologyRuleType>esriTRTAreaNoOverlap</TopologyRuleType> 
            XPathNavigator navigatorTopologyRuleType = navigator.SelectSingleNode("TopologyRuleType");
            if (navigatorTopologyRuleType != null) {
                this._topologyRuleType = (esriTopologyRuleType)Enum.Parse(typeof(esriTopologyRuleType), navigatorTopologyRuleType.Value, true);
            }

            // <OriginClassID>5</OriginClassID> 
            XPathNavigator navigatorOriginClassID = navigator.SelectSingleNode("OriginClassID");
            if (navigatorOriginClassID != null) {
                this._originClassId = navigatorOriginClassID.ValueAsInt;
            }

            // <OriginSubtype>0</OriginSubtype> 
            XPathNavigator navigatorOriginSubtype = navigator.SelectSingleNode("OriginSubtype");
            if (navigatorOriginSubtype != null) {
                this._originSubtype = navigatorOriginSubtype.ValueAsInt;
            }

            // <DestinationClassID>5</DestinationClassID> 
            XPathNavigator navigatorDestinationClassID = navigator.SelectSingleNode("DestinationClassID");
            if (navigatorDestinationClassID != null) {
                this._destinationClassId = navigatorDestinationClassID.ValueAsInt;
            }

            // <DestinationSubtype>0</DestinationSubtype> 
            XPathNavigator navigatorDestinationSubtype = navigator.SelectSingleNode("DestinationSubtype");
            if (navigatorDestinationSubtype != null) {
                this._destinationSubtype = navigatorDestinationSubtype.ValueAsInt;
            }

            // <TriggerErrorEvents>false</TriggerErrorEvents> 
            XPathNavigator navigatorTriggerErrorEvents = navigator.SelectSingleNode("TriggerErrorEvents");
            if (navigatorTriggerErrorEvents != null) {
                this._triggerErrorEvents = navigatorTriggerErrorEvents.ValueAsBoolean;
            }

            // <AllOriginSubtypes>true</AllOriginSubtypes> 
            XPathNavigator navigatorAllOriginSubtypes = navigator.SelectSingleNode("AllOriginSubtypes");
            if (navigatorName != null) {
                this._allOriginSubtypes = navigatorAllOriginSubtypes.ValueAsBoolean;
            }

            // <AllDestinationSubtypes>true</AllDestinationSubtypes> 
            XPathNavigator navigatorAllDestinationSubtypes = navigator.SelectSingleNode("AllDestinationSubtypes");
            if (navigatorAllDestinationSubtypes != null) {
                this._allDestinationSubtypes = navigatorAllDestinationSubtypes.ValueAsBoolean;
            }
        }
        public TopologyRule(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._name = info.GetString("name");
            this._guid = info.GetString("guid");
            this._topologyRuleType = (esriTopologyRuleType)Enum.Parse(typeof(esriTopologyRuleType), info.GetString("topologyRuleType"), true);
            this._originClassId = info.GetInt32("originClassId");
            this._originSubtype = info.GetInt32("originSubtype");
            this._destinationClassId = info.GetInt32("destinationClassId");
            this._destinationSubtype = info.GetInt32("destinationSubtype");
            this._triggerErrorEvents = info.GetBoolean("triggerErrorEvents");
            this._allOriginSubtypes = info.GetBoolean("allOriginSubtypes");
            this._allDestinationSubtypes = info.GetBoolean("allDestinationSubtypes");
        }
        public TopologyRule(TopologyRule prototype) : base(prototype) {
            this._name = prototype.Name;
            this._guid = prototype.Guid;
            this._topologyRuleType = prototype.TopologyRuleType;
            this._originClassId = prototype.OriginClassId;
            this._originSubtype = prototype.OriginSubtype;
            this._destinationClassId = prototype.DestinationClassId;
            this._destinationSubtype = prototype.DestinationSubtype;
            this._triggerErrorEvents = prototype.TriggerErrorEvents;
            this._allOriginSubtypes = prototype.AllOriginSubtypes;
            this._allDestinationSubtypes = prototype.AllDestinationSubtypes;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// Name of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue("")]
        [Description("Name of the topology rule")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// GUID of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue("")]
        [Description("GUID of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(GuidConverter))]
        public string Guid {
            get { return this._guid; }
            set { this._guid = value; }
        }
        /// <summary>
        /// Topology rule type of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(esriTopologyRuleType.esriTRTAny)]
        [Description("Topology rule type of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public esriTopologyRuleType TopologyRuleType {
            get { return this._topologyRuleType; }
            set { this._topologyRuleType = value; }
        }
        /// <summary>
        /// Origin ClassID of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(-1)]
        [Description("Origin ClassID of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int OriginClassId {
            get { return this._originClassId; }
            set { this._originClassId = value; }
        }
        /// <summary>
        /// Origin subtype of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(-1)]
        [Description("Origin subtype of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int OriginSubtype {
            get { return this._originSubtype; }
            set { this._originSubtype = value; }
        }
        /// <summary>
        /// Destination ClassID of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(-1)]
        [Description("Destination ClassID of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ObjectClassConverter))]
        public int DestinationClassId {
            get { return this._destinationClassId; }
            set { this._destinationClassId = value; }
        }
        /// <summary>
        /// Destination subtype of the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(-1)]
        [Description("Destination subtype of the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(SubtypeConverter))]
        public int DestinationSubtype {
            get { return this._destinationSubtype; }
            set { this._destinationSubtype = value; }
        }
        /// <summary>
        /// Indicates if error events are triggered for the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(false)]
        [Description("Indicates if error events are triggered for the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool TriggerErrorEvents {
            get { return this._triggerErrorEvents; }
            set { this._triggerErrorEvents = value; }
        }
        /// <summary>
        /// Indicates if all origin subtypes are specified for the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(false)]
        [Description("Indicates if all origin subtypes are specified for the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool AllOriginSubtypes {
            get { return this._allOriginSubtypes; }
            set { this._allOriginSubtypes = value; }
        }
        /// <summary>
        /// Indicates if all destination subtypes are specified for the topology rule
        /// </summary>
        [Browsable(true)]
        [Category("Topology Rule")]
        [DefaultValue(false)]
        [Description("Indicates if all destination subtypes are specified for the topology rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool AllDestinationSubtypes {
            get { return this._allDestinationSubtypes; }
            set { this._allDestinationSubtypes = value; }
        }
        [Browsable(false)]
        public override string Label {
            get {
                string text = GeodatabaseUtility.GetDescription(this._topologyRuleType);
                return text;
            }
        }
        //
        // PUBLIC METHDOS
        //
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", this._name);
            info.AddValue("guid", this._guid);
            info.AddValue("topologyRuleType", Convert.ToInt32(this._topologyRuleType).ToString());
            info.AddValue("originClassId", this._originClassId);
            info.AddValue("originSubtype", this._originSubtype);
            info.AddValue("destinationClassId", this._destinationClassId);
            info.AddValue("destinationSubtype", this._destinationSubtype);
            info.AddValue("triggerErrorEvents", this._triggerErrorEvents);
            info.AddValue("allOriginSubtypes", this._allOriginSubtypes);
            info.AddValue("allDestinationSubtypes", this._allDestinationSubtypes);

            base.GetObjectData(info, context);
        }
        public override object Clone() {
            return new TopologyRule(this);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // Write Base Errors
            base.Errors(list, table);

            // Get Topology
            Topology topology = (Topology)table;

            // Check for duplicate rules
            bool duplicate = false;
            foreach (TopologyRule topologyRule in topology.TopologyRules) {
                if (topologyRule == this) { continue; }
                if (this._allOriginSubtypes == topologyRule.AllOriginSubtypes &&
                    this._originClassId == topologyRule.OriginClassId &&
                    this._originSubtype == topologyRule.OriginSubtype &&
                    this._allDestinationSubtypes == topologyRule.AllDestinationSubtypes &&
                    this._destinationClassId == topologyRule.DestinationClassId &&
                    this._destinationSubtype == topologyRule.DestinationSubtype &&
                    this._topologyRuleType == topologyRule.TopologyRuleType) {
                    duplicate = true;
                    break;
                }
            }
            if (duplicate) {
                string message = string.Format("Duplicate topology rules found");
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }

            // Duplicate Guids
            bool duplicateGuid = false;
            foreach (TopologyRule topologyRule in topology.TopologyRules) {
                if (topologyRule == this) { continue; }
                if (this._guid == topologyRule.Guid) {
                    duplicateGuid = true;
                    break;
                }
            }
            if (duplicateGuid) {
                string message = string.Format("Topology Rule Guid [{0}] is not unique", this._guid);
                list.Add(new ErrorObject(this, table, message, ErrorType.Error));
            }

            // Check if rule conflict with rules using "All Subtypes" - Origin
            if (this._allOriginSubtypes) {
                bool originSubtypeConflict = false;
                foreach (TopologyRule topologyRule in topology.TopologyRules) {
                    if (topologyRule == this) { continue; }
                    if (topologyRule.AllOriginSubtypes == false &&
                        this._originClassId == topologyRule.OriginClassId &&
                        this._allDestinationSubtypes == topologyRule.AllDestinationSubtypes &&
                        this._destinationClassId == topologyRule.DestinationClassId &&
                        this._destinationSubtype == topologyRule.DestinationSubtype &&
                        this._topologyRuleType == topologyRule.TopologyRuleType) {
                        originSubtypeConflict = true;
                        break;
                    }
                }
                if (originSubtypeConflict) {
                    string message = string.Format("The Topology Rule [{0}] with an Origin 'AllSubtypes' setting conflicts another rule", this._guid);
                    list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                }
            }

            // Check if rule conflict with rules using "All Subtypes" - Destination
            if (this._allDestinationSubtypes) {
                bool destinationSubtypeConflict = false;
                foreach (TopologyRule topologyRule in topology.TopologyRules) {
                    if (topologyRule == this) { continue; }
                    if (topologyRule.AllDestinationSubtypes == false &&
                        this._destinationClassId == topologyRule.DestinationClassId &&
                        this._allOriginSubtypes == topologyRule.AllOriginSubtypes &&
                        this._originClassId == topologyRule.OriginClassId &&
                        this._originSubtype == topologyRule.OriginSubtype &&
                        this._topologyRuleType == topologyRule.TopologyRuleType) {
                        destinationSubtypeConflict = true;
                        break;
                    }
                }
                if (destinationSubtypeConflict) {
                    string message = string.Format("The Topology Rule [{0}] with an Destination 'AllSubtypes' setting conflicts another rule", this._guid);
                    list.Add(new ErrorObject(this, table, message, ErrorType.Error));
                }
            }

            // Valid rules for Origin/Destination Shape Types
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;

            // Get Origin and Destination FeatureClasses
            ObjectClass objectClassOrig = schemaModel.FindObjectClass(this._originClassId);
            ObjectClass objectClassDest = schemaModel.FindObjectClass(this._destinationClassId);

            // Check if Origin and Destination ObjectClasses Exist
            if (objectClassOrig == null) {
                list.Add(new ErrorObject(this, table, "Origin FeatureClass Does Not Exist", ErrorType.Error));
            }
            if (objectClassDest == null) {
                list.Add(new ErrorObject(this, table, "Destination FeatureClass Does Not Exist", ErrorType.Error));
            }

            // Check if Origin and Destination Subypes Exist
            EsriTable tableOrig = schemaModel.FindObjectClassOrSubtype(this._originClassId, this._originSubtype);
            EsriTable tableDest = schemaModel.FindObjectClassOrSubtype(this._destinationClassId, this._destinationSubtype);
            if (tableOrig == null) {
                list.Add(new ErrorObject(this, table, "Origin FeatureClass and/or Subtype Does Not Exist", ErrorType.Error));
            }
            if (tableDest == null) {
                list.Add(new ErrorObject(this, table, "Destination FeatureClass and/or Subtype Does Not Exist", ErrorType.Error));
            }

            // Get Origin and Destination FeatureClasses
            FeatureClass featureClassOrig = objectClassOrig as FeatureClass;
            FeatureClass featureClassDest = objectClassDest as FeatureClass;

            // These rules MUST have the same origin and destination class/subtype
            if (featureClassOrig != null && featureClassDest != null && tableOrig != null && tableDest != null) {
                switch (this._topologyRuleType) {
                    case esriTopologyRuleType.esriTRTLineNoOverlap:
                    case esriTopologyRuleType.esriTRTLineNoIntersection:
                    case esriTopologyRuleType.esriTRTLineNoDangles:
                    case esriTopologyRuleType.esriTRTLineNoPseudos:
                    case esriTopologyRuleType.esriTRTLineNoSelfOverlap:
                    case esriTopologyRuleType.esriTRTLineNoSelfIntersect:
                    case esriTopologyRuleType.esriTRTLineNoIntersectOrInteriorTouch:
                    case esriTopologyRuleType.esriTRTLineNoMultipart:
                    case esriTopologyRuleType.esriTRTAreaNoGaps:
                    case esriTopologyRuleType.esriTRTAreaNoOverlap:
                        if (this._originClassId != this._destinationClassId ||
                            this._originSubtype != this._destinationSubtype) {
                            list.Add(new ErrorObject(this, table, "This rule must have the same origin and destination FeatureClass and Subtype", ErrorType.Error));
                        }
                        break;
                    default:
                        break;
                }
            }

            // Examine Rule Based on Origin and Destination GeometryType
            bool ok = false;
            if (featureClassOrig != null && featureClassDest != null) {
                switch (featureClassOrig.ShapeType) {
                    case esriGeometryType.esriGeometryPoint:
                        switch (featureClassDest.ShapeType) {
                            case esriGeometryType.esriGeometryPoint:
                                // POINT > POINT
                                switch (this._topologyRuleType) {
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolyline:
                                // POINT > POLYLINE
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTPointCoveredByLine:
                                    case esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                // POINT > POLYGON
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary:
                                    case esriTopologyRuleType.esriTRTPointProperlyInsideArea:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                        }
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        switch (featureClassDest.ShapeType) {
                            case esriGeometryType.esriGeometryPoint:
                                // POLYLINE > POINT
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolyline:
                                // POLYLINE > POLYLINE
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTLineNoOverlap:
                                    case esriTopologyRuleType.esriTRTLineNoIntersection:
                                    case esriTopologyRuleType.esriTRTLineNoDangles:
                                    case esriTopologyRuleType.esriTRTLineNoPseudos:
                                    case esriTopologyRuleType.esriTRTLineCoveredByLineClass:
                                    case esriTopologyRuleType.esriTRTLineNoOverlapLine:
                                    case esriTopologyRuleType.esriTRTLineNoSelfOverlap:
                                    case esriTopologyRuleType.esriTRTLineNoSelfIntersect:
                                    case esriTopologyRuleType.esriTRTLineNoIntersectOrInteriorTouch:
                                    case esriTopologyRuleType.esriTRTLineNoMultipart:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                // POLYLINE > POLYGON
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                        }
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        switch (featureClassDest.ShapeType) {
                            case esriGeometryType.esriGeometryPoint:
                                // POLYGON > POINT
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTAreaContainPoint:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolyline:
                                // POLYGON > POLYLINE
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                // POLYGON > POLYGON
                                switch (this._topologyRuleType) {
                                    case esriTopologyRuleType.esriTRTAreaNoGaps:
                                    case esriTopologyRuleType.esriTRTAreaNoOverlap:
                                    case esriTopologyRuleType.esriTRTAreaCoveredByAreaClass:
                                    case esriTopologyRuleType.esriTRTAreaAreaCoverEachOther:
                                    case esriTopologyRuleType.esriTRTAreaCoveredByArea:
                                    case esriTopologyRuleType.esriTRTAreaNoOverlapArea:
                                    case esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary:
                                        ok = true;
                                        break;
                                    default:
                                        ok = false;
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            if (!ok) {
                list.Add(new ErrorObject(this, table, "This rule cannot be added between these FeatureClass geometric types", ErrorType.Error));
            }
        }
        public override void WriteXml(XmlWriter writer) {
            // <RelationshipRule>
            writer.WriteStartElement("TopologyRule");
            writer.WriteAttributeString(Xml._XSI, Xml._TYPE, null, "esri:TopologyRule");

            // Write Inner Xml
            this.WriteInnerXml(writer);

            // </RelationshipRule>
            writer.WriteEndElement();
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Inner Xml
            base.WriteInnerXml(writer);

            //    <Name /> 
            writer.WriteStartElement("Name");
            if (!string.IsNullOrEmpty(this._name)) {
                writer.WriteValue(this._name);
            }
            writer.WriteEndElement();

            //    <GUID>C27042FF-A8D0-4A18-9C55-21EAD4495196</GUID> 
            writer.WriteStartElement("GUID");
            writer.WriteValue(this._guid);
            writer.WriteEndElement();

            //    <TopologyRuleType>esriTRTAreaNoOverlap</TopologyRuleType> 
            writer.WriteStartElement("TopologyRuleType");
            writer.WriteValue(this._topologyRuleType.ToString());
            writer.WriteEndElement();

            //    <OriginClassID>5</OriginClassID> 
            writer.WriteStartElement("OriginClassID");
            writer.WriteValue(this._originClassId);
            writer.WriteEndElement();

            //    <OriginSubtype>0</OriginSubtype> 
            if (this._originSubtype != -1) {
                writer.WriteStartElement("OriginSubtype");
                writer.WriteValue(this._originSubtype);
                writer.WriteEndElement();
            }

            //    <DestinationClassID>5</DestinationClassID> 
            writer.WriteStartElement("DestinationClassID");
            writer.WriteValue(this._destinationClassId);
            writer.WriteEndElement();

            //    <DestinationSubtype>0</DestinationSubtype> 
            if (this._destinationSubtype != -1) {
                writer.WriteStartElement("DestinationSubtype");
                writer.WriteValue(this._destinationSubtype);
                writer.WriteEndElement();
            }

            //    <TriggerErrorEvents>false</TriggerErrorEvents> 
            writer.WriteStartElement("TriggerErrorEvents");
            writer.WriteValue(this._triggerErrorEvents);
            writer.WriteEndElement();

            //    <AllOriginSubtypes>true</AllOriginSubtypes> 
            writer.WriteStartElement("AllOriginSubtypes");
            writer.WriteValue(this._allOriginSubtypes);
            writer.WriteEndElement();

            //    <AllDestinationSubtypes>true</AllDestinationSubtypes> 
            writer.WriteStartElement("AllDestinationSubtypes");
            writer.WriteValue(this._allDestinationSubtypes);
            writer.WriteEndElement();
        }
    }
}

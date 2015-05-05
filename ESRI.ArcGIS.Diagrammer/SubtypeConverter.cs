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
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// Subtype Converter
    /// </summary>
    public class SubtypeConverter : TypeConverter {
        public SubtypeConverter() { }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            // Get Model
            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
            if (de == null) { return null; }
            SchemaModel model = de.SchemaModel;
            if (model == null) { return null; }

            // Create List
            List<string> list = new List<string>();

            // In some cases do not display list of subtypes
            bool skip = false;
            if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                // If "All Subtypes" then do not show dropdown
                TopologyRule topologyRule = (TopologyRule)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "OriginSubtype":
                        skip = topologyRule.AllOriginSubtypes;
                        break;
                    case "DestinationSubtype":
                        skip = topologyRule.AllDestinationSubtypes;
                        break;
                }
            }

            //
            if (!skip) {
                ObjectClass objectClass = this.GetObjectClass(context);
                if (objectClass != null) {
                    List<Subtype> subtypes = objectClass.GetSubtypes();
                    foreach (Subtype subtype in subtypes) {
                        list.Add(subtype.SubtypeName);
                    }
                    if (subtypes.Count == 0) {
                        list.Add(Resources.TEXT_CLASS_BR);
                    }
                }
            }

            // Sort List
            list.Sort();
            list.Insert(0, Resources.TEXT_NONE_BR);
            
            // Return List
            StandardValuesCollection svc = new StandardValuesCollection(list);
            return svc;
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            if (value.GetType() == typeof(string)) {
                string text = Convert.ToString(value);

                // "" or "(None)"
                if (text == string.Empty || text == Resources.TEXT_NONE_BR) {
                    if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                        TopologyRule topologyRule = (TopologyRule)context.Instance;
                        switch (context.PropertyDescriptor.Name) {
                            case "OriginSubtype":
                                return topologyRule.AllOriginSubtypes ? 0 : -1;
                            case "DestinationSubtype":
                                return topologyRule.AllDestinationSubtypes ? 0 : -1;
                            default:
                                return -1;
                        }
                    }
                    return -1;
                }

                // "(Class)"
                if (text == Resources.TEXT_CLASS_BR) {
                    return 0;
                }

                // Subtype Name
                ObjectClass objectClass = this.GetObjectClass(context);
                if (objectClass == null) { return -1; }
                Subtype subtype = objectClass.FindSubtype(text);
                if (subtype == null) { return -1; }
                return subtype.SubtypeCode;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }
            if (destinationType == null) {
                throw new ArgumentNullException("destinationType");
            }

            if (destinationType == typeof(string)) {
                if (value.GetType() == typeof(int)) {
                    int id = Convert.ToInt32(value);
                    ObjectClass objectClass = this.GetObjectClass(context);
                    if (objectClass == null) { return Resources.TEXT_NONE_BR; }
                    List<Subtype> subtypes = objectClass.GetSubtypes();
                    if (subtypes.Count == 0) {
                        if (id == -1) {
                            return Resources.TEXT_NONE_BR;
                        }
                        return Resources.TEXT_CLASS_BR;
                    }

                    if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                        TopologyRule topologyRule = (TopologyRule)context.Instance;
                        switch (context.PropertyDescriptor.Name) {
                            case "OriginSubtype":
                                if (topologyRule.OriginSubtype == 0 && topologyRule.AllOriginSubtypes) {
                                    return Resources.TEXT_NONE_BR;
                                }
                                break;
                            case "DestinationSubtype":
                                if (topologyRule.DestinationSubtype == 0 && topologyRule.AllDestinationSubtypes) {
                                    return Resources.TEXT_NONE_BR;
                                }
                                break;
                        }
                    }

                    Subtype subtype = objectClass.FindSubtype(id);
                    if (subtype == null) { return Resources.TEXT_NONE_BR; }
                    return subtype.SubtypeName;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return true;
        }
        private ObjectClass GetObjectClass(ITypeDescriptorContext context) {
            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
            SchemaModel model = de.SchemaModel;
            ObjectClass objectClass = null;
            if (context.PropertyDescriptor.ComponentType == typeof(EdgeConnectivityRule)) {
                EdgeConnectivityRule edgeConnectivityRule = (EdgeConnectivityRule)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "FromEdgeSubtypeCode":
                        objectClass = model.FindObjectClass(edgeConnectivityRule.FromClassID);
                        break;
                    case "ToEdgeSubtypeCode":
                        objectClass = model.FindObjectClass(edgeConnectivityRule.ToClassID);
                        break;
                    case "DefaultJunctionSubtypeCode":
                        objectClass = model.FindObjectClass(edgeConnectivityRule.DefaultJunctionID);
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionSubtype)) {
                JunctionSubtype junctionSubtype = (JunctionSubtype)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "SubtypeCode":
                        objectClass = model.FindObjectClass(junctionSubtype.ClassID);
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(RelationshipRule)) {
                RelationshipRule relationshipRule = (RelationshipRule)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "OriginSubtype":
                        objectClass = model.FindObjectClass(relationshipRule.OriginClass);
                        break;
                    case "DestinationSubtype":
                        objectClass = model.FindObjectClass(relationshipRule.DestinationClass);
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionConnectivityRule)) {
                JunctionConnectivityRule junctionConnectivityRule = (JunctionConnectivityRule)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "EdgeSubtypeCode":
                        objectClass = model.FindObjectClass(junctionConnectivityRule.EdgeClassID);
                        break;
                    case "SubtypeCode":
                        objectClass = model.FindObjectClass(junctionConnectivityRule.JunctionClassID);
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                TopologyRule topologyRule = (TopologyRule)context.Instance;
                switch (context.PropertyDescriptor.Name) {
                    case "OriginSubtype":
                        objectClass = model.FindObjectClass(topologyRule.OriginClassId);
                        break;
                    case "DestinationSubtype":
                        objectClass = model.FindObjectClass(topologyRule.DestinationClassId);
                        break;
                }
            }
            return objectClass;
        }
    }
}

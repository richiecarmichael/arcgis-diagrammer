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
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    public class ObjectClassConverter : TypeConverter {
        public ObjectClassConverter() { }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            // RelationshipClass:        OriginClassName, DestinationClassName
            // EdgeConnectivityRule:     DefaultJunctionID, FromClassID, ToClassID
            // JunctionSubtype:          ClassID
            // RelationshipRule:         OriginClass, DestinationClass
            // JunctionConnectivityRule: EdgeClassID, JunctionClassID
            // TopologyRule:             OriginClassId, DestinationClassId
            // NetWeightAssociation:     TableName
            // TerrainDataSource:        FeatureClassName

            // Get Model
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            if (diagrammerEnvironment == null) { return null; }
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;
            if (schemaModel == null) { return null; }

            // Get ObjectClasses
            List<ObjectClass> objectClasses = schemaModel.GetObjectClasses();

            // Create List
            List<string> list = new List<string>();

            if (context.PropertyDescriptor.ComponentType == typeof(RelationshipClass)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClassName":
                    case "DestinationClassName":
                        foreach (ObjectClass objectClass in objectClasses) {
                            if (objectClass.GetType() == typeof(ObjectClass) ||
                                objectClass.GetType() == typeof(FeatureClass) ||
                                objectClass.GetType() == typeof(RasterCatalog)) {
                                list.Add(objectClass.Name);
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(EdgeConnectivityRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "DefaultJunctionID":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType){             //  (objectClass.CLSID) {
                                case esriFeatureType.esriFTSimpleJunction: // EsriRegistry.CLASS_SIMPLEJUNCTION:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                    case "FromClassID":
                    case "ToClassID":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType) {         // (objectClass.CLSID) {
                                case esriFeatureType.esriFTComplexEdge: // EsriRegistry.CLASS_COMPLEXEDGE:
                                case esriFeatureType.esriFTSimpleEdge:  // EsriRegistry.CLASS_SIMPLEEDGE:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionSubtype)) {
                switch (context.PropertyDescriptor.Name) {
                    case "ClassID":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType) {            // (objectClass.CLSID) {
                                case esriFeatureType.esriFTSimpleJunction: // EsriRegistry.CLASS_SIMPLEJUNCTION:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(RelationshipRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClass":
                    case "DestinationClass":
                        foreach (ObjectClass objectClass in objectClasses) {
                            if (objectClass.GetType() == typeof(ObjectClass) ||
                                objectClass.GetType() == typeof(FeatureClass)) {
                                list.Add(objectClass.Name);
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionConnectivityRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "EdgeClassID":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType) {         // (objectClass.CLSID) {
                                case esriFeatureType.esriFTComplexEdge: // EsriRegistry.CLASS_COMPLEXEDGE:
                                case esriFeatureType.esriFTSimpleEdge:  // EsriRegistry.CLASS_SIMPLEEDGE:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                    case "JunctionClassID":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType) {            // (objectClass.CLSID) {
                                case esriFeatureType.esriFTSimpleJunction: // EsriRegistry.CLASS_SIMPLEJUNCTION:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClassId":
                    case "DestinationClassId":
                        foreach (ObjectClass objectClass in objectClasses) {
                            if (objectClass.GetType() == typeof(FeatureClass)) {
                                list.Add(objectClass.Name);
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(NetWeightAssociation)) {
                switch (context.PropertyDescriptor.Name) {
                    case "TableName":
                        foreach (ObjectClass objectClass in objectClasses) {
                            FeatureClass featureClass = objectClass as FeatureClass;
                            if (featureClass == null) { continue; }
                            switch (featureClass.FeatureType) {            // (objectClass.CLSID) {
                                case esriFeatureType.esriFTComplexEdge:    // EsriRegistry.CLASS_COMPLEXEDGE:
                                case esriFeatureType.esriFTSimpleEdge:     // EsriRegistry.CLASS_SIMPLEEDGE:
                                case esriFeatureType.esriFTSimpleJunction: // EsriRegistry.CLASS_SIMPLEJUNCTION:
                                    list.Add(objectClass.Name);
                                    break;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TerrainDataSource)) {
                switch (context.PropertyDescriptor.Name) {
                    case "FeatureClassName":
                        TerrainDataSource terrainDataSource = (TerrainDataSource)context.Instance;
                        SchemaModel model = DiagrammerEnvironment.Default.SchemaModel;
                        if (model == null) { break; }
                        Terrain terrain = model.FindParent(terrainDataSource);
                        FeatureDataset featureDataset = terrain.GetParent() as FeatureDataset;
                        if (featureDataset == null) { break; }
                        foreach(Dataset dataset in featureDataset.GetChildren()){
                            if (dataset.DatasetType == esriDatasetType.esriDTFeatureClass){
                                list.Add(dataset.Name);
                            }
                        }
                        break;
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

                if (context.PropertyDescriptor.ComponentType == typeof(RelationshipClass)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "OriginClassName":
                        case "DestinationClassName":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return string.Empty;
                            }
                            return text;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(EdgeConnectivityRule)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "DefaultJunctionID":
                        case "FromClassID":
                        case "ToClassID":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return -1;
                            }
                            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
                            SchemaModel model = de.SchemaModel;
                            ObjectClass objectClass = model.FindObjectClass(text);
                            if (objectClass == null) { return -1; }
                            return objectClass.DSID;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(JunctionSubtype)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "ClassID":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return -1;
                            }
                            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
                            SchemaModel model = de.SchemaModel;
                            ObjectClass objectClass = model.FindObjectClass(text);
                            if (objectClass == null) { return -1; }
                            return objectClass.DSID;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(RelationshipRule)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "OriginClass":
                        case "DestinationClass":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return -1;
                            }
                            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
                            SchemaModel model = de.SchemaModel;
                            ObjectClass objectClass = model.FindObjectClass(text);
                            if (objectClass == null) { return -1; }
                            return objectClass.DSID;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(JunctionConnectivityRule)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "EdgeClassID":
                        case "JunctionClassID":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return -1;
                            }
                            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
                            SchemaModel model = de.SchemaModel;
                            ObjectClass objectClass = model.FindObjectClass(text);
                            if (objectClass == null) { return -1; }
                            return objectClass.DSID;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "OriginClassId":
                        case "DestinationClassId":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return -1;
                            }
                            DiagrammerEnvironment de = DiagrammerEnvironment.Default;
                            SchemaModel model = de.SchemaModel;
                            ObjectClass objectClass = model.FindObjectClass(text);
                            if (objectClass == null) { return -1; }
                            return objectClass.DSID;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(NetWeightAssociation)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "TableName":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return string.Empty;
                            }
                            return text;
                    }
                }
                else if (context.PropertyDescriptor.ComponentType == typeof(TerrainDataSource)) {
                    switch (context.PropertyDescriptor.Name) {
                        case "FeatureClassName":
                            if (text == string.Empty ||
                                text == Resources.TEXT_NONE_BR) {
                                return string.Empty;
                            }
                            return text;
                    }
                }
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

            if (context.PropertyDescriptor.ComponentType == typeof(RelationshipClass)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClassName":
                    case "DestinationClassName":
                        if (value.GetType() == typeof(string)) {
                            string text = Convert.ToString(value);
                            if (string.IsNullOrEmpty(text)) {
                                return Resources.TEXT_NONE_BR;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(EdgeConnectivityRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "DefaultJunctionID":
                    case "FromClassID":
                    case "ToClassID":
                        if (value.GetType() == typeof(int)) {
                            int id = Convert.ToInt32(value);
                            if (id == -1) { return Resources.TEXT_NONE_BR; }
                            ObjectClass objectClasss = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(id);
                            if (objectClasss == null) { return string.Empty; }
                            return objectClasss.Name;
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionSubtype)) {
                switch (context.PropertyDescriptor.Name) {
                    case "ClassID":
                        if (value.GetType() == typeof(int)) {
                            int id = Convert.ToInt32(value);
                            if (id == -1) { return Resources.TEXT_NONE_BR; }
                            ObjectClass objectClasss = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(id);
                            if (objectClasss == null) { return string.Empty; }
                            return objectClasss.Name;
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(RelationshipRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClass":
                    case "DestinationClass":
                        if (value.GetType() == typeof(int)) {
                            int id = Convert.ToInt32(value);
                            if (id == -1) { return Resources.TEXT_NONE_BR; }
                            ObjectClass objectClasss = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(id);
                            if (objectClasss == null) { return string.Empty; }
                            return objectClasss.Name;
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(JunctionConnectivityRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "EdgeClassID":
                    case "JunctionClassID":
                        if (value.GetType() == typeof(int)) {
                            int id = Convert.ToInt32(value);
                            if (id == -1) { return Resources.TEXT_NONE_BR; }
                            ObjectClass objectClasss = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(id);
                            if (objectClasss == null) { return string.Empty; }
                            return objectClasss.Name;
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TopologyRule)) {
                switch (context.PropertyDescriptor.Name) {
                    case "OriginClassId":
                    case "DestinationClassId":
                        if (value.GetType() == typeof(int)) {
                            int id = Convert.ToInt32(value);
                            if (id == -1) { return Resources.TEXT_NONE_BR; }
                            ObjectClass objectClasss = DiagrammerEnvironment.Default.SchemaModel.FindObjectClass(id);
                            if (objectClasss == null) { return string.Empty; }
                            return objectClasss.Name;
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(NetWeightAssociation)) {
                switch (context.PropertyDescriptor.Name) {
                    case "TableName":
                        if (value.GetType() == typeof(string)) {
                            string text = Convert.ToString(value);
                            if (string.IsNullOrEmpty(text)) {
                                return Resources.TEXT_NONE_BR;
                            }
                        }
                        break;
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TerrainDataSource)) {
                switch (context.PropertyDescriptor.Name) {
                    case "FeatureClassName":
                        if (value.GetType() == typeof(string)) {
                            string text = Convert.ToString(value);
                            if (string.IsNullOrEmpty(text)) {
                                return Resources.TEXT_NONE_BR;
                            }
                        }
                        break;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return true;
        }
    }
}

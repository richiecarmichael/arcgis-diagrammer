/*=============================================================================
 * 
 * Copyright � 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished�all rights reserved.
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
    public class FieldConverter : TypeConverter {
        public FieldConverter() { }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            // FeatureClass::ShapeFieldName, AreaFieldName, LengthFieldName, ShapeFieldName
            // GeometricNetworkControllerMembership::EnabledFieldName, AncillaryRoleFieldName
            // IndexField::Name
            // ObjectClass::OIDFieldName, GlobalIDFieldName, RasterFieldName, SubtypeFieldName, OIDFieldName, GlobalIDFieldName
            // RelationshipClass::OriginPrimary, OriginForeign, DestinationPrimary, DestinationForeign
            // SubtypeField::FieldName
            // NetWeightAssociation::FieldName
            // TerrainDataSource::HeightField, TagValueField

            // Create List
            List<string> list = new List<string>();

            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            SchemaModel schemaModel = diagrammerEnvironment.SchemaModel;

            if (context.PropertyDescriptor.ComponentType == typeof(IndexField)) {
                IndexField indexField = (IndexField)context.Instance;
                ObjectClass objectClass = (ObjectClass)indexField.Table;
                foreach (Field field in objectClass.GetFields()) {
                    list.Add(field.Name);
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(GeometricNetworkControllerMembership)) {
                GeometricNetworkControllerMembership geometricNetworkControllerMembership = (GeometricNetworkControllerMembership)context.Instance;
                ObjectClass objectClass = schemaModel.FindParent(geometricNetworkControllerMembership);
                foreach (Field field in objectClass.GetFields()) {
                    list.Add(field.Name);
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(SubtypeField)) {
                SubtypeField subtypeField = (SubtypeField)context.Instance;
                Subtype subtype = (Subtype)subtypeField.Table;
                ObjectClass objectClass = subtype.GetParent();
                foreach (Field field in objectClass.GetFields()) {
                    list.Add(field.Name);
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(ObjectClass)) {
                ObjectClass objectClass = (ObjectClass)context.Instance;
                foreach (Field field in objectClass.GetFields()) {
                    list.Add(field.Name);
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(RelationshipClass)) {
                RelationshipClass relationshipClass = (RelationshipClass)context.Instance;
                if (relationshipClass.IsAttributed) {
                    switch (context.PropertyDescriptor.Name) {
                        case "OIDFieldName":
                        case "GlobalIDFieldName":
                        case "RasterFieldName":
                        case "SubtypeFieldName":
                            foreach (Field field in relationshipClass.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "OriginPrimary":
                            ObjectClass objectClass1 = schemaModel.FindObjectClass(relationshipClass.OriginClassName);
                            foreach (Field field in objectClass1.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "OriginForeign":
                        case "DestinationForeign":
                            foreach (Field field in relationshipClass.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "DestinationPrimary":
                            ObjectClass objectClass2 = schemaModel.FindObjectClass(relationshipClass.DestinationClassName);
                            foreach (Field field in objectClass2.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                    }
                }
                else {
                    switch (context.PropertyDescriptor.Name) {
                        case "OIDFieldName":
                        case "GlobalIDFieldName":
                        case "RasterFieldName":
                        case "SubtypeFieldName":
                            foreach (Field field in relationshipClass.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "OriginPrimary":
                            ObjectClass objectClass1 = schemaModel.FindObjectClass(relationshipClass.OriginClassName);
                            foreach (Field field in objectClass1.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "OriginForeign":
                            ObjectClass objectClass2 = schemaModel.FindObjectClass(relationshipClass.DestinationClassName);
                            foreach (Field field in objectClass2.GetFields()) {
                                list.Add(field.Name);
                            }
                            break;
                        case "DestinationPrimary":
                        case "DestinationForeign":
                            break;
                    }
                }
            }
            else if (
                context.PropertyDescriptor.ComponentType == typeof(FeatureClass) ||
                context.PropertyDescriptor.ComponentType == typeof(RasterCatalog)) {
                FeatureClass featureClass = (FeatureClass)context.Instance;
                foreach (Field field in featureClass.GetFields()) {
                    list.Add(field.Name);
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(NetWeightAssociation)) {
                NetWeightAssociation netWeightAssociation = (NetWeightAssociation)context.Instance;
                if (netWeightAssociation != null) {
                    if (!string.IsNullOrEmpty(netWeightAssociation.TableName)) {
                        ObjectClass objectClass = schemaModel.FindObjectClass(netWeightAssociation.TableName);
                        if (objectClass != null) {
                            foreach (Field field in objectClass.GetFields()) {
                                list.Add(field.Name);
                            }
                        }
                    }
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(TerrainDataSource)) {
                TerrainDataSource terrainDataSource = (TerrainDataSource)context.Instance;
                if (terrainDataSource != null) {
                    if (!string.IsNullOrEmpty(terrainDataSource.FeatureClassName)) {
                        ObjectClass objectClass = schemaModel.FindObjectClass(terrainDataSource.FeatureClassName);
                        if (objectClass != null) {
                            foreach (Field field in objectClass.GetFields()) {
                                list.Add(field.Name);
                            }
                        }
                    }
                }
            }

            // Sort field name list and insert "None" item
            list.Sort();
            list.Insert(0, Resources.TEXT_NONE_BR);

            // Return sort field name list
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
                if (text == Resources.TEXT_NONE_BR) {
                    return string.Empty;
                }
                return text;
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
                if (value.GetType() == typeof(string)) {
                    string text = Convert.ToString(value);
                    if (string.IsNullOrEmpty(text)) {
                        return Resources.TEXT_NONE_BR;
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return true;
        }
    }
}

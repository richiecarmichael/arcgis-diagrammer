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
    public class GuidConverter : TypeConverter {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            //
            List<string> list = new List<string>();

            if (context.Instance.GetType() == typeof(ObjectClass) ||
                context.Instance.GetType() == typeof(FeatureClass) ||
                context.Instance.GetType() == typeof(RasterCatalog) ||
                context.Instance.GetType() == typeof(RelationshipClass)) {
                if (context.PropertyDescriptor.Name == "CLSID") {
                    list.Add(Resources.TEXT_FEATURE_CLASS);
                    list.Add(Resources.TEXT_TABLE);
                    list.Add(Resources.TEXT_ANNOTATION);
                    list.Add(Resources.TEXT_DIMENSION);
                    list.Add(Resources.TEXT_SIMPLE_JUNCTION);
                    list.Add(Resources.TEXT_SIMPLE_EDGE);
                    list.Add(Resources.TEXT_COMPLEX_EDGE);
                    list.Add(Resources.TEXT_RASTER_CATALOG);
                    list.Add(Resources.TEXT_ATTRIBUTED_RELATIONSHIP);
                }
                else if (context.PropertyDescriptor.Name == "EXTCLSID") {
                    list.Add(Resources.TEXT_ANNOTATION);
                    list.Add(Resources.TEXT_DIMENSION);
                }
            }
            else if (context.Instance.GetType() == typeof(TopologyRule)) {
                if (context.PropertyDescriptor.Name == "Guid") { }
            }

            // Sort List
            list.Sort();
            list.Insert(0, Resources.TEXT_NONE_BR);

            // Passes the local integer array.
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
                string text = (string)value;

                switch (context.PropertyDescriptor.Name) {
                    case "CLSID":
                        if (text == Resources.TEXT_NONE_BR) {
                            return string.Empty;
                        }
                        else if (text == Resources.TEXT_FEATURE_CLASS) {
                            return EsriRegistry.CLASS_FEATURECLASS;
                        }
                        else if (text == Resources.TEXT_TABLE) {
                            return EsriRegistry.CLASS_TABLE;
                        }
                        else if (text == Resources.TEXT_ANNOTATION) {
                            return EsriRegistry.CLASS_ANNOTATION;
                        }
                        else if (text == Resources.TEXT_DIMENSION) {
                            return EsriRegistry.CLASS_DIMENSION;
                        }
                        else if (text == Resources.TEXT_SIMPLE_JUNCTION) {
                            return EsriRegistry.CLASS_SIMPLEJUNCTION;
                        }
                        else if (text == Resources.TEXT_SIMPLE_EDGE) {
                            return EsriRegistry.CLASS_SIMPLEEDGE;
                        }
                        else if (text == Resources.TEXT_COMPLEX_EDGE) {
                            return EsriRegistry.CLASS_COMPLEXEDGE;
                        }
                        else if (text == Resources.TEXT_RASTER_CATALOG) {
                            return EsriRegistry.CLASS_RASTERCATALOG;
                        }
                        else if (text == Resources.TEXT_ATTRIBUTED_RELATIONSHIP) {
                            return EsriRegistry.CLASS_ATTRIBUTED_RELATIONSHIP;
                        }
                        Guid g1 = new Guid(text);
                        return text;
                    case "EXTCLSID":
                        if (text == Resources.TEXT_NONE_BR) {
                            return string.Empty;
                        }
                        else if (text == Resources.TEXT_ANNOTATION) {
                            return EsriRegistry.CLASS_ANNOTATION_EXTENSION;
                        }
                        else if (text == Resources.TEXT_DIMENSION) {
                            return EsriRegistry.CLASS_DIMENSION_EXTENSION;
                        }
                        Guid g2 = new Guid(text);
                        return text;
                    case "Guid":
                        if (text == Resources.TEXT_NONE_BR) {
                            return string.Empty;
                        }
                        Guid g3 = new Guid(text);
                        return text;
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

            if (destinationType == typeof(string)) {
                if (value.GetType() == typeof(string)) {
                    string text = Convert.ToString(value);
                    switch (context.PropertyDescriptor.Name) {
                        case "CLSID":
                            switch (text) {
                                case "":
                                    return Resources.TEXT_NONE_BR;
                                case EsriRegistry.CLASS_FEATURECLASS:
                                    return Resources.TEXT_FEATURE_CLASS;
                                case EsriRegistry.CLASS_TABLE:
                                    return Resources.TEXT_TABLE;
                                case EsriRegistry.CLASS_ANNOTATION:
                                    return Resources.TEXT_ANNOTATION;
                                case EsriRegistry.CLASS_DIMENSION:
                                    return Resources.TEXT_DIMENSION;
                                case EsriRegistry.CLASS_SIMPLEJUNCTION:
                                    return Resources.TEXT_SIMPLE_JUNCTION;
                                case EsriRegistry.CLASS_SIMPLEEDGE:
                                    return Resources.TEXT_SIMPLE_EDGE;
                                case EsriRegistry.CLASS_COMPLEXEDGE:
                                    return Resources.TEXT_COMPLEX_EDGE;
                                case EsriRegistry.CLASS_RASTERCATALOG:
                                    return Resources.TEXT_RASTER_CATALOG;
                                case EsriRegistry.CLASS_ATTRIBUTED_RELATIONSHIP:
                                    return Resources.TEXT_ATTRIBUTED_RELATIONSHIP;
                                default:
                                    return text;
                            }
                        case "EXTCLSID":
                            switch (text) {
                                case "":
                                    return Resources.TEXT_NONE_BR;
                                case EsriRegistry.CLASS_ANNOTATION_EXTENSION:
                                    return Resources.TEXT_ANNOTATION;
                                case EsriRegistry.CLASS_DIMENSION_EXTENSION:
                                    return Resources.TEXT_DIMENSION;
                                default:
                                    return text;
                            }
                        case "Guid":
                            switch (text) {
                                case "":
                                    return Resources.TEXT_NONE_BR;
                                default:
                                    return text;
                            }
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return false;
        }
    }
}

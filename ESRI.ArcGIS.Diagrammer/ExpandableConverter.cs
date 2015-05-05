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
    /// Expandable Type Converter for Diagrammer Classes
    /// </summary>
    public class ExpandableConverter : ExpandableObjectConverter {
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
                if (value == null) {
                    return Resources.TEXT_NONE_BR;
                }
                else if (value.GetType() == typeof(StorageDef)) {
                    return Resources.TEXT_STORAGE_DEFINITION_BR;
                }
                else if (value.GetType() == typeof(RasterDef)) {
                    return Resources.TEXT_RASTER_DEFINITION_BR;
                }
                else if (value.GetType() == typeof(SpatialReference)) {
                    return Resources.TEXT_SPATIAL_REFERENCE_BR;
                }
                else if (value.GetType() == typeof(Point)) {
                    return Resources.TEXT_POINT_BR;
                }
                else if (value.GetType() == typeof(GeometryDef)) {
                    return Resources.TEXT_GEOMETRY_DEFINITION_BR;
                }
                else if (value.GetType() == typeof(Extent)) {
                    return Resources.TEXT_EXTENT_BR;
                }
                else if (value.GetType() == typeof(NetworkDirections)) {
                    return Resources.TEXT_NETWORK_DIRECTIONS_BR;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool IsValid(ITypeDescriptorContext context, object value) {
            if (context == null) { return false; }
            if (context.Instance == null) { return false; }
            return (value != null);
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
                    return null;
                }
                else if (text == Resources.TEXT_STORAGE_DEFINITION_BR) {
                    return new StorageDef();
                }
                else if (text == Resources.TEXT_RASTER_DEFINITION_BR) {
                    return new RasterDef();
                }
                else if (text == Resources.TEXT_SPATIAL_REFERENCE_BR) {
                    return new SpatialReference();
                }
                else if (text == Resources.TEXT_POINT_BR) {
                    return new Point();
                }
                else if (text == Resources.TEXT_GEOMETRY_DEFINITION_BR) {
                    return new GeometryDef();
                }
                else if (text == Resources.TEXT_EXTENT_BR) {
                    return new Extent();
                }
                else if (text == Resources.TEXT_NETWORK_DIRECTIONS_BR) {
                    return new NetworkDirections();
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            // Create List
            List<string> list = new List<string>();

            // Add None
            list.Add(Resources.TEXT_NONE_BR);

            if (context.PropertyDescriptor.PropertyType == typeof(StorageDef)) {
                list.Add(Resources.TEXT_STORAGE_DEFINITION_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(RasterDef)) {
                list.Add(Resources.TEXT_RASTER_DEFINITION_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(SpatialReference)) {
                list.Add(Resources.TEXT_SPATIAL_REFERENCE_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(Point)) {
                list.Add(Resources.TEXT_POINT_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(GeometryDef)) {
                list.Add(Resources.TEXT_GEOMETRY_DEFINITION_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(Extent)) {
                list.Add(Resources.TEXT_EXTENT_BR);
            }
            else if (context.PropertyDescriptor.PropertyType == typeof(NetworkDirections)) {
                list.Add(Resources.TEXT_NETWORK_DIRECTIONS_BR);
            }

            // Passes the local integer array.
            StandardValuesCollection svc = new StandardValuesCollection(list);
            return svc;
        }
    }
}

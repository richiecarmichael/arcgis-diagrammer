///*=============================================================================
// * 
// * Copyright 2007 ESRI. All rights reserved. 
// * 
// * Use subject to ESRI license agreement.
// * 
// * Unpublished—all rights reserved.
// * Use of this ESRI commercial Software, Data, and Documentation is limited to
// * the ESRI License Agreement. In no event shall the Government acquire greater
// * than Restricted/Limited Rights. At a minimum Government rights to use,
// * duplicate, or disclose is subject to restrictions as set for in FAR 12.211,
// * FAR 12.212, and FAR 52.227-19 (June 1987), FAR 52.227-14 (ALT I, II, and III)
// * (June 1987), DFARS 227.7202, DFARS 252.227-7015 (NOV 1995).
// * Contractor/Manufacturer is ESRI, 380 New York Street, Redlands,
// * CA 92373-8100, USA.
// * 
// * SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// * INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// * PARTICULAR PURPOSE, ARE DISCLAIMED.  IN NO EVENT SHALL ESRI OR CONTRIBUTORS
// * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// * INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY
// * THEORY OF LIABILITY, WHETHER IN CONTRACT; STRICT LIABILITY; OR TORT ARISING
// * IN ANY WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE
// * POSSIBILITY OF SUCH DAMAGE TO THE FULL EXTENT ALLOWED BY APPLICABLE LAW.
// * 
// * =============================================================================*/

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using System.Globalization;
//using System.Collections;
//using System.ComponentModel.Design.Serialization;
//using System.Reflection;

//namespace ESRI.ArcGIS.Diagrammer {
//    public class DiagramSizeConverter : TypeConverter {
//        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
//            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
//        }
//        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
//            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
//        }
//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
//            string text = value as string;
//            if (text == null) {
//                return base.ConvertFrom(context, culture, value);
//            }
//            string text2 = text.Trim();
//            if (text2.Length == 0) {
//                return null;
//            }
//            if (culture == null) {
//                culture = CultureInfo.CurrentCulture;
//            }
//            char ch = culture.TextInfo.ListSeparator[0];
//            string[] textArray = text2.Split(new char[] { ch });
//            int[] numArray = new int[textArray.Length];
//            TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
//            for (int i = 0; i < numArray.Length; i++) {
//                numArray[i] = (int)converter.ConvertFromString(context, culture, textArray[i]);
//            }
//            if (numArray.Length != 2) {
//                throw new ArgumentException("TextParseFailedFormat");
//            }
//            return new DiagramSize(numArray[0], numArray[1]);
//        }
//        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
//            if (destinationType == null) {
//                throw new ArgumentNullException("destinationType");
//            }
//            if (value is DiagramSize) {
//                if (destinationType == typeof(string)) {
//                    DiagramSize size = (DiagramSize)value;
//                    if (culture == null) {
//                        culture = CultureInfo.CurrentCulture;
//                    }
//                    string separator = culture.TextInfo.ListSeparator + " ";
//                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
//                    string[] textArray = new string[2];
//                    int num = 0;
//                    textArray[num++] = converter.ConvertToString(context, culture, size.Width);
//                    textArray[num++] = converter.ConvertToString(context, culture, size.Height);
//                    return string.Join(separator, textArray);
//                }
//                if (destinationType == typeof(InstanceDescriptor)) {
//                    DiagramSize size2 = (DiagramSize)value;
//                    ConstructorInfo member = typeof(DiagramSize).GetConstructor(new Type[] { typeof(int), typeof(int) });
//                    if (member != null) {
//                        return new InstanceDescriptor(member, new object[] { size2.Width, size2.Height });
//                    }
//                }
//            }
//            return base.ConvertTo(context, culture, value, destinationType);
//        }
//        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
//            if (propertyValues == null) {
//                throw new ArgumentNullException("propertyValues");
//            }
//            object obj2 = propertyValues["Width"];
//            object obj3 = propertyValues["Height"];
//            if (((obj2 == null) || (obj3 == null)) || (!(obj2 is int) || !(obj3 is int))) {
//                throw new ArgumentException("PropertyValueInvalidEntry");
//            }
//            return new DiagramSize((int)obj2, (int)obj3);
//        }
//        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
//            return true;
//        }
//        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
//            return TypeDescriptor.GetProperties(typeof(DiagramSize), attributes).Sort(new string[] { "Width", "Height" });
//        }
//        public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
//            return true;
//        }
//    }
//}

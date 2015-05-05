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
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ExceptionHandler;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public class DataReport : Report {
        private IWorkspaceName m_workspaceName = null;
        //
        // CONSTRUCTOR
        //
        public DataReport() : base() {
            this.Xsl = Resources.FILE_DATA_REPORT;
            DataReportSettings.Default.SettingsSaving += new SettingsSavingEventHandler(this.SettingsSaving);
        }
        //
        // PROPERTIES
        //
        [Browsable(true)]
        [Category("Data Report")]
        [DefaultValue(null)]
        [Description("Personal/File Geodatabase or ArcSDE Connection")]
        [DisplayName("Workspace")]
        [EditorAttribute(typeof(WorkspaceNameEditor), typeof(UITypeEditor))]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        [TypeConverter(typeof(WorkspaceNameConverter))]
        public IWorkspaceName WorkspaceName {
            get { return this.m_workspaceName; }
            set {
                try {
                    // Store New WorkspaceName
                    this.m_workspaceName = value;

                    // Create Report
                    this.CreateReport();
                }
                catch(Exception ex){
                    ExceptionDialog.HandleException(ex);
                }
            }
        }
        //
        // PROTECTED METHODS
        //
        protected override XsltArgumentList GetArgumentList() {
            XsltArgumentList argsList = base.GetArgumentList();

            argsList.AddParam("font", string.Empty, DataReportSettings.Default.FontName);
            argsList.AddParam("backcolor", string.Empty, ColorTranslator.ToHtml(DataReportSettings.Default.BackColor));
            argsList.AddParam("forecolor", string.Empty, ColorTranslator.ToHtml(DataReportSettings.Default.ForeColor));
            argsList.AddParam("size1", string.Empty, DataReportSettings.Default.Size1.ToString());
            argsList.AddParam("size2", string.Empty, DataReportSettings.Default.Size2.ToString());
            argsList.AddParam("size3", string.Empty, DataReportSettings.Default.Size3.ToString());
            argsList.AddParam("size4", string.Empty, DataReportSettings.Default.Size4.ToString());
            argsList.AddParam("size5", string.Empty, DataReportSettings.Default.Size5.ToString());
            argsList.AddParam("pathName", string.Empty, this.m_workspaceName.PathName);
            argsList.AddParam("category", string.Empty, this.m_workspaceName.Category);

            return argsList;
        }
        //
        // PRIVATE METHODS
        //
        private void CreateReport() {
            // Exit if Workspace is NULL
            if (this.m_workspaceName == null){ return; }

            //
            DiagrammerEnvironment diagrammerEnvironment = DiagrammerEnvironment.Default;
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs("Creating Report"));

            // Get IWorkspace
            IName name = (IName)this.m_workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();

            // Get Temporary File
            string filename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N").ToUpper() + ".xml");

            // Specific XML Settings
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8; //  Encoding.Default;
            settings.Indent = false;
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = false;

            // Create the XmlWriter object and write some content.
            XmlWriter writer = XmlWriter.Create(filename, settings);

            // <DataReport>
            writer.WriteStartElement("DataReport");

            //
            IEnumDatasetName enumDatasetName1 = workspace.get_DatasetNames(esriDatasetType.esriDTAny);
            IDatasetName datasetName1 = enumDatasetName1.Next();
            while (datasetName1 != null) {
                switch (datasetName1.Type) {
                    case esriDatasetType.esriDTFeatureDataset:
                        // <DataReport><FeatureDataset>
                        writer.WriteStartElement("FeatureDataset");

                        // <DataReport><FeatureDataset><Name>
                        writer.WriteStartElement("Name");
                        writer.WriteValue(datasetName1.Name);
                        writer.WriteEndElement();

                        IEnumDatasetName enumDatasetName2 = datasetName1.SubsetNames;
                        IDatasetName datasetName2 = enumDatasetName2.Next();
                        while (datasetName2 != null) {
                            switch (datasetName2.Type) {
                                case esriDatasetType.esriDTFeatureClass:
                                    // Display Message
                                    string message = string.Format("Adding <{0}>", datasetName2.Name);
                                    diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message));

                                    // Add Dataset
                                    this.AddDataset(writer, datasetName2);
                                    break;
                                default:
                                    break;
                            }
                            datasetName2 = enumDatasetName2.Next();
                        }

                        // <DataReport></FeatureDataset>
                        writer.WriteEndElement();

                        break;
                    case esriDatasetType.esriDTFeatureClass:
                    case esriDatasetType.esriDTTable:
                        // Display Message
                        string message2 = string.Format("Adding <{0}>", datasetName1.Name);
                        diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(message2));

                        // Add Dataset
                        this.AddDataset(writer, datasetName1);
                        break;
                    default:
                        break;
                }

                datasetName1 = enumDatasetName1.Next();
            }

            // </DataReport>
            writer.WriteEndElement();

            // Close Writer
            writer.Close();

            // Set Source XML
            this.Xml = filename;

            // Fire Invalidate Event so that the Report Tabbed Document can Reload
            this.OnInvalidated(new EventArgs());

            // Clear Messages
            diagrammerEnvironment.OnProgressChanged(new ProgressEventArgs(string.Empty));
        }
        private void AddDataset(XmlWriter writer, IDatasetName datasetName) {
            // Check Parameters
            if (writer == null) { return; }
            if (datasetName == null) { return; }

            // Open Dataset
            IName name = datasetName as IName;
            if (name == null) { return; }
            object o = null;
            try {
                o = name.Open();
            }
            catch { }
            if (o == null) { return; }
            IDataset dataset = o as IDataset;
            if (dataset == null) { return; }
            IFeatureClass featureClass = dataset as IFeatureClass;

            // <Dataset>
            writer.WriteStartElement("Dataset");

            // <Dataset><Name>
            writer.WriteStartElement("Name");
            writer.WriteValue(dataset.Name);
            writer.WriteEndElement();

            // <Dataset><Type>
            string type = string.Empty;
            switch(dataset.Type){
                case esriDatasetType.esriDTFeatureClass:
                    if (featureClass == null) {
                        type += "Unknown";
                        break;
                    }
                    switch (featureClass.FeatureType) {
                        case esriFeatureType.esriFTAnnotation:
                        case esriFeatureType.esriFTDimension:
                        case esriFeatureType.esriFTSimple:
                            type += GeodatabaseUtility.GetDescription(esriDatasetType.esriDTFeatureClass);
                            break;
                        default:
                            type += GeodatabaseUtility.GetDescription(featureClass.FeatureType);
                            break;
                    }          
                    break;
                default:
                    type += GeodatabaseUtility.GetDescription(dataset.Type);
                    break;
            }
            writer.WriteStartElement("Type");
            writer.WriteValue(type);
            writer.WriteEndElement();

            // <Dataset><Geometry>
            string geometry = "-";
            if (featureClass != null) {
                switch (featureClass.FeatureType) {
                    case esriFeatureType.esriFTAnnotation:
                    case esriFeatureType.esriFTDimension:
                        geometry = GeodatabaseUtility.GetDescription(featureClass.FeatureType);
                        break;
                    default:
                        geometry = GeodatabaseUtility.GetDescription(featureClass.ShapeType);
                        break;
                }
            }
            writer.WriteStartElement("Geometry");
            writer.WriteValue(geometry);
            writer.WriteEndElement();

            //if (dataset is IFeatureClass) {
            //    IFeatureClass featureClass = (IFeatureClass)dataset;

            //    // <Dataset><FeatureType>
            //    writer.WriteStartElement("FeatureType");
            //    writer.WriteValue(GeodatabaseUtility.GetDescription(featureClass.FeatureType));
            //    writer.WriteEndElement();

            //    // Add ...<Dataset><ShapeType>
            //    switch (featureClass.FeatureType) {
            //        case esriFeatureType.esriFTAnnotation:
            //        case esriFeatureType.esriFTDimension:
            //            break;
            //        default:
            //            writer.WriteStartElement("ShapeType");
            //            writer.WriteValue(GeodatabaseUtility.GetDescription(featureClass.ShapeType));
            //            writer.WriteEndElement();
            //            break;
            //    }
            //}

            // Get Row Count
            ITable table = dataset as ITable;
            int intRowCount = -1;
            if (table != null) {
                try {
                    intRowCount = table.RowCount(null);
                }
                catch { }
            }

            // <Dataset><RowCount>
            writer.WriteStartElement("RowCount");
            switch(intRowCount){
                case -1:
                    writer.WriteValue("Error");
                    break;
                case 0:
                default:
                    writer.WriteValue(intRowCount.ToString());
                    break;
            }
            writer.WriteEndElement();

            if (intRowCount > 0) {
                if (dataset is IGeoDataset) {
                    IGeoDataset geoDataset = (IGeoDataset)dataset;
                    IEnvelope envelope = geoDataset.Extent;
                    if (envelope != null && !envelope.IsEmpty) {
                        // <Dataset><Extent>
                        writer.WriteStartElement("Extent");

                        // <Dataset><Extent><XMax>
                        writer.WriteStartElement("XMax");
                        writer.WriteValue(envelope.XMax.ToString());
                        writer.WriteEndElement();

                        // <Dataset><Extent><XMin>
                        writer.WriteStartElement("XMin");
                        writer.WriteValue(envelope.XMin.ToString());
                        writer.WriteEndElement();

                        // <Dataset><Extent><YMax>
                        writer.WriteStartElement("YMax");
                        writer.WriteValue(envelope.YMax.ToString());
                        writer.WriteEndElement();

                        // <Dataset><Extent><YMin>
                        writer.WriteStartElement("YMin");
                        writer.WriteValue(envelope.YMin.ToString());
                        writer.WriteEndElement();

                        // <Dataset></Extent>
                        writer.WriteEndElement();

                        // <Dataset><SmallImage>
                        if (DataReportSettings.Default.ShowSmallImage) {
                            string smallImage = System.IO.Path.GetTempFileName();
                            writer.WriteStartElement("SmallImage");
                            writer.WriteValue(smallImage);
                            writer.WriteEndElement();
                            GeodatabaseUtility.CreateBitmap(
                                dataset,
                                DataReportSettings.Default.SmallImageType,
                                DataReportSettings.Default.SmallImageSize,
                                DataReportSettings.Default.SmallImageResolution,
                                DataReportSettings.Default.SmallImageBackgroundColor,
                                smallImage);
                        }

                        // <Dataset><LargeImage>
                        if (DataReportSettings.Default.ShowLargeImage) {
                            string largeImage = System.IO.Path.GetTempFileName();
                            writer.WriteStartElement("LargeImage");
                            writer.WriteValue(largeImage);
                            writer.WriteEndElement();
                            GeodatabaseUtility.CreateBitmap(
                                dataset,
                                DataReportSettings.Default.LargeImageType,
                                DataReportSettings.Default.LargeImageSize,
                                DataReportSettings.Default.LargeImageResolution,
                                DataReportSettings.Default.LargeImageBackgroundColor,
                                largeImage);
                        }
                    }
                }
            }

            ISubtypes subtypes = dataset as ISubtypes;
            if (subtypes != null && subtypes.HasSubtype) {
                int subtypeCode = 0;
                IEnumSubtype enumSubtype = subtypes.Subtypes;
                string subtypeName = enumSubtype.Next(out subtypeCode);
                while (subtypeName != null) {
                    // <Dataset><Sybtype>
                    writer.WriteStartElement("Subtype");

                    // <Dataset><Sybtype><Name>
                    writer.WriteStartElement("Name");
                    writer.WriteValue(subtypeName);
                    writer.WriteEndElement();

                    // Get Row Count
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = string.Format("{0} = {1}", subtypes.SubtypeFieldName, subtypeCode.ToString());
                    int rowCount = table.RowCount(queryFilter);

                    // <Dataset><Sybtype><RowCount>
                    writer.WriteStartElement("RowCount");
                    writer.WriteValue(rowCount.ToString());
                    writer.WriteEndElement();

                    // <Dataset></Sybtype>
                    writer.WriteEndElement();

                    // Get Next Subtype
                    subtypeName = enumSubtype.Next(out subtypeCode);
                }
            }

            // </Dataset>
            writer.WriteEndElement();
        }
        private void SettingsSaving(object sender, CancelEventArgs e) {
            this.OnInvalidated(EventArgs.Empty);
        }
    }
}

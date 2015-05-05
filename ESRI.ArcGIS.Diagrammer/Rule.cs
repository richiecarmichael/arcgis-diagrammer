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

namespace ESRI.ArcGIS.Diagrammer {
    [Serializable]
    [DefaultProperty("RuleId")]
    public abstract class Rule : EsriObject, IDiagramProperty {
        private const string CATEGORY = "Rule";
        private string _helpString = string.Empty;
        private int _ruleId = -1;
        private int _category = -1;
        private bool _showLabels = false;
        //
        // CONSTRUCTOR
        //
        public Rule() : base() { }
        public Rule(IXPathNavigable path) : base(path) {
            // Get Navigator
            XPathNavigator navigator = path.CreateNavigator();

            // <HelpString></HelpString>
            XPathNavigator navigatorHelpString = navigator.SelectSingleNode("HelpString");
            if (navigatorHelpString != null) {
                this._helpString = navigatorHelpString.Value;
            }

            // <RuleID></RuleID>
            XPathNavigator navigatorRuleID = navigator.SelectSingleNode("RuleID");
            if (navigatorRuleID != null) {
                this._ruleId = navigatorRuleID.ValueAsInt;
            }

            // <Category></Category>
            XPathNavigator navigatorCategory = navigator.SelectSingleNode("Category");
            if (navigatorCategory != null) {
                this._category = navigatorCategory.ValueAsInt;
            }
        }
        public Rule(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._helpString = info.GetString("helpString");
            this._ruleId = info.GetInt32("ruleId");
            this._category = info.GetInt32("category");
        }
        public Rule(Rule prototype) : base(prototype) {
            this._helpString = prototype.HelpString;
            this._ruleId = prototype.RuleId;
            this._category = prototype.Category;
        }
        //
        // PROPERTIES
        //
        /// <summary>
        /// The helpstring associated with the validation rule
        /// </summary>
        [Browsable(true)]
        [Category(Rule.CATEGORY)]
        [DefaultValue("")]
        [Description("The helpstring associated with the validation rule")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public string HelpString {
            get { return this._helpString; }
            set { this._helpString = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The ID of the validation rule
        /// </summary>
        [Browsable(true)]
        [Category(Rule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The ID of the validation rule")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(true)]
        public int RuleId {
            get { return this._ruleId; }
            set { this._ruleId = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// The name associated with the validation rule
        /// </summary>
        [Browsable(true)]
        [Category(Rule.CATEGORY)]
        [DefaultValue(-1)]
        [Description("The name associated with the validation rule")]
        [ParenthesizePropertyName(true)]
        [ReadOnly(false)]
        public int Category {
            get { return this._category; }
            set { this._category = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        /// <summary>
        /// Show/Hide Rule Text Labels
        /// </summary>
        [Browsable(true)]
        [Category(Rule.CATEGORY)]
        [DefaultValue(false)]
        [Description("Show/Hide Text Labels")]
        [ParenthesizePropertyName(false)]
        [ReadOnly(false)]
        public bool ShowLabels {
            get { return this._showLabels; }
            set { this._showLabels = value; if (!this.Suspended) { this.OnInvalidated(EventArgs.Empty); } }
        }
        [Browsable(false)]
        public abstract string Label {
            get;
        }
        [Browsable(false)]
        public virtual float BorderWidth {
            get { return 1f; }
        }
        //
        // PUBLIC METHODS
        // 
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("helpString", this._helpString);
            info.AddValue("ruleId", this._ruleId);
            info.AddValue("category", this._ruleId);

            base.GetObjectData(info, context);
        }
        public override void Errors(List<Error> list, EsriTable table) {
            // TODO : HelpString contains invalid characters
            // TODO : Rule Id less than -1
        }
        //
        // PROTECTED METHODS
        //
        protected override void WriteInnerXml(XmlWriter writer) {
            // Write Base Inner Xml
            base.WriteInnerXml(writer);

            // <HelpString></HelpString>
            writer.WriteStartElement("HelpString");
            if (!(string.IsNullOrEmpty(this._helpString))) {
                writer.WriteValue(this._helpString);
            }
            writer.WriteEndElement();

            // <RuleID></RuleID>
            writer.WriteStartElement("RuleID");
            writer.WriteValue(this._ruleId);
            writer.WriteEndElement();

            // <Category></Category>
            if (this._category != -1) {
                writer.WriteStartElement("Category");
                writer.WriteValue(this._category);
                writer.WriteEndElement();
            }
        }
    }
}

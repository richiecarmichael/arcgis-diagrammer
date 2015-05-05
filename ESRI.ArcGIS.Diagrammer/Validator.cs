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
using ESRI.ArcGIS.Geodatabase;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// A class for validating table and field names
    /// </summary>
    public abstract class Validator {
        private IWorkspace _workspace = null;
        //
        // CONSTRUCTOR
        //
        public Validator() { }
        public Validator(IWorkspace workspace) {
            this.SetWorkspace(workspace);
        }
        //
        // PROPERTIES
        //
        public IWorkspace Workspace {
            get { return this._workspace; }
        }
        //
        // STATIC PROPERTY
        //
        /// <summary>
        /// Returns the default validator
        /// </summary>
        public static Validator Default {
            get { return new FileGeodatabaseValidator(); }
        }
        //
        // PROTECTED
        //
        protected void SetWorkspace(IWorkspace workspace) {
            this._workspace = workspace;
        }
        //
        // PUBLIC METHODS 
        //
        /// <summary>
        /// Validates a table name
        /// </summary>
        /// <param name="name">The table name to validate</param>
        /// <param name="message">If table name fails then this variable will contain a description</param>
        /// <returns>True if passed. False if failed.</returns>
        public bool ValidateTableName(string name, out string message) {
            // Check Arguments
            if (string.IsNullOrEmpty(name)) {
                throw new NullReferenceException("<name> argument cannot be null");
            }

            // Get Table Name
            string tableName = GeodatabaseUtility.GetTableName(name);

            // Validate Table Name
            IFieldChecker fieldChecker = new FieldCheckerClass();
            fieldChecker.ValidateWorkspace = this._workspace;
            string newName = null;
            int error = fieldChecker.ValidateTableName(tableName, out newName);

            // Create Message
            switch (error) {
                case 0:
                    message = null;
                    break;
                case 1:
                    message = string.Format("Table name [{0}] contains a reserved word.", name);
                    break;
                case 2:
                    message = string.Format("Table name [{0}] contains an invalid character.", name);
                    break;
                case 4:
                    message = string.Format("Table name [{0}] has invalid starting character.", name);
                    break;
                default:
                    message = string.Format("Table name [{0}] is invalid.", name);
                    break;
            }

            // Append Recommended Name
            switch (error) {
                case 0:
                    return true;
                default:
                    if (!(string.IsNullOrEmpty(newName))) {
                        if (newName != name) {
                            message += string.Format(" Try [{0}].", newName);
                        }
                    }
                    return false;
            }
        }
        /// <summary>
        /// Validates a field name
        /// </summary>
        /// <param name="name">The field name to validate</param>
        /// <param name="message">If field name fails then this variable will contain a description</param>
        /// <returns>True if passed. False if failed.</returns>
        public bool ValidateFieldName(string name, out string message) {
            // Validate Input Arguments
            if (string.IsNullOrEmpty(name)) {
                throw new NullReferenceException("<name> argument cannot be null");
            }

            // Create Field Checker
            IFieldChecker fieldChecker = new FieldCheckerClass();
            fieldChecker.ValidateWorkspace = this._workspace;

            // Create Field
            IFieldEdit fieldEdit = new FieldClass();
            fieldEdit.Name_2 = name;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

            // Create Fields
            IFieldsEdit fieldsEdit = new FieldsClass();
            fieldsEdit.AddField((IField)fieldEdit);

            // Create Field Error Enumerator
            IEnumFieldError enumFieldError = null;
            IFields fieldsFixed = null;

            // Validate Field Name
            fieldChecker.ValidateField(0, (IFields)fieldsEdit, out enumFieldError, out fieldsFixed);

            // Get Validated Name (if any)
            string newName = null;
            if (fieldsFixed != null) {
                IField fieldFixed = fieldsFixed.get_Field(0);
                newName = fieldFixed.Name;
            }

            // Get Errors (if any)
            if (enumFieldError != null) {
                enumFieldError.Reset();
                IFieldError fieldError = enumFieldError.Next();
                if (fieldError != null) {
                    switch (fieldError.FieldError) {
                        case esriFieldNameErrorType.esriDuplicatedFieldName:
                            message = "is duplicated";
                            return false;
                        case esriFieldNameErrorType.esriInvalidCharacter:
                            message = "contains one or more invalid characters";
                            return false;
                        case esriFieldNameErrorType.esriInvalidFieldNameLength:
                            message = "is too long";
                            return false;
                        case esriFieldNameErrorType.esriNoFieldError:
                            message = "has no field error";
                            return false;
                        case esriFieldNameErrorType.esriSQLReservedWord:
                            message = "contains one or more reserved word";
                            return false;
                        default:
                            message = "contains an unknown error";
                            return false;
                    }
                }
            }

            // The FieldChecker may have renamed the field without returning an error.
            if (newName != null) {
                if (newName != name) {
                    message = "has an invalid field name";
                    return false;
                }
            }

            // No Errors
            message = null;
            return true;
        }
        /// <summary>
        /// Checks if the input string contains database specific invalid characters
        /// </summary>
        /// <param name="name">string to test for invalid characters</param>
        /// <param name="message">If name fails then this variable will contain a description</param>
        /// <returns>False if passed. True if failed.</returns>
        public bool ContainsInvalidCharacters(string name, out string message) {
            // Validate Input Arguments
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name", "Name argument cannot be null or empty");
            }

            // Get array of invalid characters
            ISQLSyntax sqlSyntax = (ISQLSyntax) this._workspace;
            string invalidCharacters = sqlSyntax.GetInvalidCharacters();
            char[] invalidCharacters2 = invalidCharacters.ToCharArray();

            // Get index of first occuring invalid character
            int index = name.IndexOfAny(invalidCharacters2);

            if (index != -1){
                message = string.Format(
                    "The text '{0}' contains one or more of the following invalid characters '{1}'",
                    name,
                    invalidCharacters);
                return true;
            }

            // No Errors
            message = null;

            // Return false (Name does NOT contain any invalid characters)
            return false;
        }
    }
}

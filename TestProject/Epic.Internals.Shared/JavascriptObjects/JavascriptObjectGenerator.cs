using Epic.Internals.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Epic.Internals.Shared.JavascriptObjects
{
    /// <summary>
    /// This class finds any object marked with the JavascriptOjectAttribute and extracts them
    /// to a single javscript file that can be used for intellisense.  This is useful when you 
    /// have complex custom types used both on the client and server (via ajax calls and such)
    /// and would like to have the intellisense work for those cases.
    /// </summary>
    public class JavascriptObjectGenerator : JavascriptGenerator<JavascriptObjectAttribute>
    {

        /// <summary>
        /// Creates a new javascript object generator for the given assemblies
        /// </summary>
        /// <param name="inAssemblies">The list of assemblies to search</param>
        /// <param name="defaultNamespace">Specify the default namespace for all enums. this parameter is required.
        ///     Example: "TripReimbursement.Shared.ServerObjects"</param>
        /// <param name="regNamespace">The function call needed to register the namespace.  It will get the parameters appended onto it.
        ///     This parameter is required
        ///     Example: "TripReimbursement.Shared.Namespace.regNamespace"</param>
        public JavascriptObjectGenerator(List<Assembly> inAssemblies, string defaultNamespace, string regNamespace)
            : base(inAssemblies, defaultNamespace, regNamespace)
        {
            
        }

        /// <summary>
        /// Processes the list of found type-attr pairs
        /// </summary>
        /// <param name="foundTypes">the list of type-attr pairs found</param>
        protected override void GenerateJavascriptForNamespace(List<TypeAttributePair<JavascriptObjectAttribute>> foundTypes)
        {
            foreach (TypeAttributePair<JavascriptObjectAttribute> typePair in foundTypes)
            {
                Type extractType = typePair.TargetType;
                ProcessType(extractType);
                AddLine(0, "");  // add blank line
            }
        }

        /// <summary>
        /// Processes a single type into the following structure:
        /// function typeName() {
        ///     summary
        ///     
        ///     this.Prop1 = init;
        ///     this.Prop2 = init;
        /// 
        /// }
        /// </summary>
        /// <param name="type">The type to process</param>
        private void ProcessType(Type type)
        {
            int indent = 1;
            // name: function name() {
			AddFormatLine(indent, "{0}: function {0}() {{", TypeUtils.GetTypeName(type));  // need to trim ` out of generic types

            // Some sort of documentation... I can't get comments, but could consider having a
            // description property of the attribute.  But probably unecessary
            indent++;
            AddLine(indent, "/// <summary>");
            AddFormatLine(indent, "/// {0}", GetMemberDescription(type));
            AddFormatLine(indent, "/// Generated from {0}", type.FullName);
            AddLine(indent, "/// </summary>");

            // Create the field descriptions.  In order for these to work, they need to come directly after the summary
            PropertyInfo[] publicProps = type.GetProperties();
            foreach (PropertyInfo prop in publicProps)
            {
                string jsTypeName = GetJavascriptTypeName(prop.PropertyType);
                string propDescription = GetMemberDescription(prop.DeclaringType, prop);

                // gives: /// <field name='Name' type='type'></field>
                AddFormatLine(indent, "/// <field name='{0}' type='{1}'{3}>{2}</field>", prop.Name, jsTypeName, propDescription, TryGetArrayTypeName(prop.PropertyType));
            }

            // Now actually create properties
            foreach (PropertyInfo prop in publicProps)
            {
                string initVal = GetInitializeVal(prop);

                //format:  this.PropName = InitValue
                AddFormatLine(indent, "this.{0} = {1};", prop.Name, initVal);
            }

            // close function
            indent--;
            AddLine(indent, "},");
        }

        /// <summary>
        /// Tries to get the element type for an array type.  If the given type
        /// is not an array type, it returns an empty string
        /// </summary>
        /// <param name="propType">The property type</param>
        /// <returns>The element type for an array, or null if the type is not an array type</returns>
        private string TryGetArrayTypeName(Type propType)
        {
            if (TypeUtils.IsEnumerable(propType))
            {
                Type[] genericTypes = propType.GetGenericArguments();
                if (genericTypes.Length > 0)
                {
                    return " elementType='" + GetJavascriptTypeName(genericTypes[0]) + "'";
                }
                
            }

            return "";
        }

        /// <summary>
        /// Gets the javascript type name for the given type.
        /// </summary>
        /// <param name="propType">The property Type</param>
        /// <returns>The type name for that property type</returns>
        private string GetJavascriptTypeName(Type propType)
        {
            TypeCode code = Type.GetTypeCode(propType);

			string typeName;
            if (TypeUtils.IsNumericType(propType))
            {
                typeName = "Number";
            }
            else if (code == TypeCode.String)
            {
                typeName = "String";
            }
            else if (code == TypeCode.Boolean)
            {
                typeName = "Boolean";
            }
            else if (TypeUtils.IsEnumerable(propType))
            {
                typeName = "Array";
            }
            else
            {
                typeName = ClientScriptGenHelper.TryFindExtractedType(this, propType);
				if (string.IsNullOrEmpty(typeName))
				{
					typeName = TypeUtils.GetTypeName(propType);
				}
            }

			return typeName;
        }

        /// <summary>
        /// In order to properly reflect the type in javascript, we need
        /// to initialize the property to something meaningful.  This function
        /// will retrieve the value to initialize the javascript property to
        /// </summary>
        /// <param name="prop">The property to initialize</param>
        /// <returns>A string contianing the value that the javascript property should initialize to</returns>
        private string GetInitializeVal(PropertyInfo prop)
        {
            Type propType = prop.PropertyType;
            TypeCode code = Type.GetTypeCode(propType);

            //Check for string
            if (code == TypeCode.String)
            {
                return "\"\"";  //empty string for strings
            }

            //DateTime
            if (code == TypeCode.DateTime)
            {
                return "\"\"";  // i don't know what to do for datetime yet.. just empty string again
            }

            //Numeric
            if (TypeUtils.IsNumericType(propType))
            {
                return "0";  // initialize it to 0, just to show it is a number
            }

            //Boolean
            if (code == TypeCode.Boolean)
            {
                return "false";  //initialize to false
            }

            //Check for array of objects (no way i know of to strongly type these)
            if (TypeUtils.IsEnumerable(propType))
            {
                return "[]";  // return array initializer for enumerable types
            }

            return "null";   //Last resort, just return null
        }
		
    }
}

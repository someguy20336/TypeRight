using Epic.Internals.Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Epic.Internals.Shared.JavascriptObjects
{
    /// <summary>
    /// Represents a class that generates javascript information for
    /// types decorated with TAttributeType.  Generally, this is used 
    /// to make the javascript a bit more organized and provide better
    /// intellisense
    /// </summary>
    /// <typeparam name="TAttributeType">The Attribute type to look for</typeparam>
    public abstract class JavascriptGenerator<TAttributeType> : ClientScriptGenerator<TAttributeType> where TAttributeType : JavascriptExtractedAttribute
    {

        /// <summary>
        /// The javascript function used to register the namespaces.  Generally something like
        /// TripReimbursement.Shared.Namespace.regNamespace
        /// </summary>
        private string _regNamespace;
        
        /// <summary>
        /// Creates a new Javascript generator for the given assemblies
        /// </summary>
        /// <param name="assemblies">The list of assemblies to search</param>
        /// <param name="defaultNamespace">Specify the default namespace for all enums. this parameter is required.
        ///     Example: "TripReimbursement.Shared.ServerEnums"</param>
        /// <param name="regNamespace">The function call needed to register the namespace.  It will get the parameters appended onto it.
        ///     This parameter is required
        ///     Example: "TripReimbursement.Shared.Namespace.regNamespace"</param>
        public JavascriptGenerator(List<Assembly> assemblies, string defaultNamespace, string regNamespace)
			: base(assemblies, defaultNamespace)
        {


            if (string.IsNullOrEmpty(regNamespace))
            {
                throw new ArgumentNullException("regNamespace must be specified.");
            }

            _regNamespace = regNamespace;
        }

		/// <summary>
		/// Writes the javascript
		/// </summary>
		protected override void Write()
		{
			foreach (string oneNamepace in NamespaceGroups.Keys)
			{
				//Register the namespace
				AddFormatLine(0, _regNamespace + "(\"{0}\");", oneNamepace);
				AddLine(0, "");

				//Add the namespace
				AddFormatLine(0, "{0} = {{", oneNamepace);
				AddLine(0, "");

				//Generate the JS for that namespace
				List<TypeAttributePair<TAttributeType>> typesInNS = NamespaceGroups[oneNamepace];
				GenerateJavascriptForNamespace(typesInNS);

				//Close the namespace
				AddLine(0, "}");
			}
		}

        /// <summary>
        /// This abstract method will process the types that were found to have <typeparamref name="TAttributeType"/>
        /// </summary>
        /// <param name="foundTypes">the list of TypeAttributePair objects found</param>
        protected abstract void GenerateJavascriptForNamespace(List<TypeAttributePair<TAttributeType>> foundTypes);
        
    }


}

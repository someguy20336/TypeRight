using Epic.Internals.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.JavascriptObjects
{
	/// <summary>
	/// Generates Typescript objects for the client to use
	/// </summary>
	public class TypescriptObjectGenerator : TypescriptGenerator<JavascriptObjectAttribute>
	{
		/// <summary>
		/// Creates a new typescript object generator
		/// </summary>
		/// <param name="assemblies">The assemblies to search</param>
		/// <param name="defaultNamespace">The defualt namespace for the objects</param>
		public TypescriptObjectGenerator(List<Assembly> assemblies, string defaultNamespace)
			: base(assemblies, defaultNamespace)
		{ }

		/// <summary>
		/// Generates the members of a given type.  By this point, the class piece has been generated.
		/// </summary>
		/// <param name="typeAttr">The type/attr pair</param>
		protected override void GenerateTypescriptForTypeMembers(TypeAttributePair<JavascriptObjectAttribute> typeAttr)
		{
			Type type = typeAttr.TargetType;

			// Create the field descriptions.  In order for these to work, they need to come directly after the summary
			PropertyInfo[] publicProps = type.GetProperties();

			// Now actually create properties
			foreach (PropertyInfo prop in publicProps)
			{
				string name = prop.Name;

				string tsTypeName = GetTypescriptTypeName(prop.PropertyType);
				string propDescription = GetMemberDescription(prop.DeclaringType, prop);

				// format: /** description */
				AddFormatLine(2, "/** {0} */", propDescription);
				// format:  this.PropName: type;
				AddFormatLine(2, "{0}: {1};", prop.Name, tsTypeName);
			}
		}


		/// <summary>
		/// Gets the Typescript object type name for the given type.
		/// 
		/// TODO: this could be better written and needs to support dictionaries
		/// </summary>
		/// <param name="propType">The property Type</param>
		/// <returns>The type name for that property type</returns>
		private string GetTypescriptTypeName(Type propType)
		{
			Type underlyingType = propType;

			if (propType.IsGenericType)
			{
				if (propType.GetGenericTypeDefinition() == typeof(Nullable<>)
					|| propType.GetGenericTypeDefinition() == typeof(List<>))
				{
					underlyingType = propType.GetGenericArguments()[0];
				}
			}
			TypeCode code = Type.GetTypeCode(underlyingType);
			string typeName;

			if (TypeUtils.IsNumericType(underlyingType))
			{
				typeName = "number";
			}
			else if (code == TypeCode.String || code == TypeCode.DateTime)
			{
				typeName = "string";
			}
			else if (code == TypeCode.Boolean)
			{
				typeName = "boolean";
			}
			else
			{
				typeName = ClientScriptGenHelper.TryFindExtractedType(this, underlyingType);
				if (string.IsNullOrEmpty(typeName))
				{
					typeName = "any";
				}
			}

			if (TypeUtils.IsEnumerable(propType))
			{
				typeName += "[]";
			}

			return typeName;
		}
	}
}

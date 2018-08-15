using Epic.Internals.Shared.Enums;
using Epic.Internals.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.ClientScriptGeneration
{
	/// <summary>
	/// Generates Server Enums in Typescript
	/// </summary>
	public class TypescriptEnumGenerator : TypescriptGenerator<ClientScriptEnumAttribute>
	{
		/// <summary>
		/// Creates a new typescript enum generator
		/// </summary>
		/// <param name="assemblies">The assemblies to search</param>
		/// <param name="defaultNamespace">The default namespace</param>
		public TypescriptEnumGenerator(List<Assembly> assemblies, string defaultNamespace)
			: base(assemblies, defaultNamespace)
		{ }

		/// <summary>
		/// Adds the definition for the type
		/// </summary>
		/// <param name="type">The type to make the definition for</param>
		protected override void GenerateTypeDefLine(Type type)
		{
			AddFormatLine(1, "export var {0} = {{", TypeUtils.GetTypeName(type));
		}

		/// <summary>
		/// Generates the typescript for a given enum's members
		/// </summary>
		/// <param name="typeAttrPair">The type/attr pair to generate</param>
		protected override void GenerateTypescriptForTypeMembers(TypeAttributePair<ClientScriptEnumAttribute> typeAttrPair)
		{
			Type type = typeAttrPair.TargetType;
			ClientScriptEnumAttribute attr = typeAttrPair.AttributeValue;

			Array enumVals = type.GetEnumValues();

			for (int i = 0; i < enumVals.Length; i++)
			{
				//I couldn't figure out how to get these two pieces
				//of information in one object, so i just cast them to both
				//The underlying type may just be dynamic (based on the Enum)
				int intVal = (int)enumVals.GetValue(i);
				Enum enumVal = enumVals.GetValue(i) as Enum;
				AddEnumField(enumVal, intVal);
			}
		}

		/// <summary>
		/// Given an Enum field, it adds it to the generated typescript
		/// </summary>
		/// <param name="enumVal">the enum val object</param>
		/// <param name="intVal">the integer value of the enum</param>
		private void AddEnumField(Enum enumVal, int intVal)
		{
			int indent = 2;
			//Add the documentation for the enum
			AddFormatLine(indent, "/** {0} */",
							GetMemberDescription(enumVal.GetType(),
									enumVal.GetType().GetField(enumVal.ToString())));
			AddFormatLine(indent, "{0}: {{", enumVal.ToString());

			EnumDisplayNameAttribute attr = EnumUtils.GetEnumDispNameAttribute(enumVal);
			AddFormatLine(indent + 1, "id: {0},", intVal);
			AddFormatLine(indent + 1, "name: \"{0}\",", attr.DisplayName);
			AddFormatLine(indent + 1, "abbrev: \"{0}\",", attr.Abbreviation);

			string extraData = GetExtraData(enumVal);
			if (!string.IsNullOrEmpty(extraData))
			{
				AddLine(indent + 1, extraData);
			}

			AddLine(indent, "},");
		}

		/// <summary>
		/// Determines whether the enum has extra data and returns it if so
		/// </summary>
		/// <param name="enumVal">The enum value</param>
		/// <returns>Extra data if it exits</returns>
		private string GetExtraData(Enum enumVal)
		{
			FieldInfo enumInfo = enumVal.GetType().GetField(enumVal.ToString());
			JavascriptEnumFieldDataAttribute fieldData = enumInfo.GetCustomAttribute<JavascriptEnumFieldDataAttribute>();

			if (fieldData != null)
			{
				return string.Join(",", fieldData.ExtraData);
			}
			return "";
		}
	}
}

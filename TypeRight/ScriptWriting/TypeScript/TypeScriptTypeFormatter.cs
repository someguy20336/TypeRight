using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{

	/// <summary>
	/// Standard type formatter for typescript objects
	/// </summary>
	public class TypeScriptTypeFormatter : TypeFormatter 
	{
		/// <summary>
		/// Index of names for the collection an how many times they show up
		/// </summary>
		protected Dictionary<string, int> NameCountIndex { get; private set; }
		
		/// <summary>
		/// Creates a new type formatter for typescript
		/// </summary>
		/// <param name="typeCollection"></param>
		public TypeScriptTypeFormatter(ExtractedTypeCollection typeCollection)
		{
			// Cache an index of the count of each name.  
			// This will help for cases when a type has the same name, but a different number of type params because typescript doesn't allow it
			NameCountIndex = typeCollection.GetReferenceTypes().GroupBy(refType => GetIndexName(refType)).ToDictionary(grp => grp.Key, grp => grp.Count());
		}

		/// <summary>
		/// Formats the anon type
		/// </summary>
		/// <param name="anonymousType"></param>
		/// <returns></returns>
		public override string FormatAnonymousType(AnonymousTypeDescriptor anonymousType)
		{
			return TypeScriptHelper.BuildAnonymousType(anonymousType.Properties.ToDictionary(
						pr => pr.Name,
						pr => pr.Type.FormatType(this))
						);
		}

		/// <summary>
		/// formats an array type
		/// </summary>
		/// <param name="arrayType"></param>
		/// <returns></returns>
		public override string FormatArrayType(ArrayTypeDescriptor arrayType)
		{
			return $"{arrayType.ElementType.FormatType(this)}[]";
		}

		/// <summary>
		/// Formats a boolean type
		/// </summary>
		/// <param name="booleanType"></param>
		/// <returns></returns>
		public override string FormatBooleanType(BooleanTypeDescriptor booleanType)
		{
			return TypeScriptHelper.BooleanTypeName;
		}

		/// <summary>
		/// Formats a date time type
		/// </summary>
		/// <param name="dateTimeType"></param>
		/// <returns></returns>
		public override string FormatDateTimeType(DateTimeTypeDescriptor dateTimeType)
		{
			return TypeScriptHelper.StringTypeName;
		}

		/// <summary>
		/// formats a dictionary type
		/// </summary>
		/// <param name="dictionaryType"></param>
		/// <returns></returns>
		public override string FormatDictionaryType(DictionaryTypeDescriptor dictionaryType)
		{
			return TypeScriptHelper.FormatDictionaryType(dictionaryType.Key.FormatType(this), dictionaryType.Value.FormatType(this));
		}

		/// <summary>
		/// formats a extracted enum type
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public override string FormatExtractedEnumType(ExtractedEnumTypeDescriptor enumType)
		{
			if (enumType.UseExtendedSyntax)
			{
				return TypeScriptHelper.NumericTypeName;
			}
			else
			{
				return $"{GetNamespaceWithDot(enumType)}{enumType.Name}";
			}
		}

		/// <summary>
		/// Formats a list type
		/// </summary>
		/// <param name="listType"></param>
		/// <returns></returns>
		public override string FormatListType(ListTypeDescriptor listType)
		{
			return $"{listType.TypeArg.FormatType(this)}[]";
		}

		/// <summary>
		/// formats a named reference type 
		/// </summary>
		/// <param name="namedReferenceType"></param>
		/// <returns></returns>
		public override string FormatNamedReferenceType(NamedReferenceTypeDescriptor namedReferenceType)
		{
			// Create the type name appending the number of type arguments if there are duplicate names
			string typeName;
			if (NameCountIndex[GetIndexName(namedReferenceType)] > 1 && namedReferenceType.TypeArguments.Count > 0)
			{
				typeName = $"{namedReferenceType.Name}_{namedReferenceType.TypeArguments.Count}";
			}
			else
			{
				typeName = namedReferenceType.Name;
			}

			// Append type arguments
			if (namedReferenceType.TypeArguments.Count > 0)
			{
				IEnumerable<string> typeArgs = namedReferenceType.TypeArguments.Select(csType => csType.FormatType(this));
				typeName += $"<{string.Join(", ", typeArgs)}>";
			}

			return $"{GetNamespaceWithDot(namedReferenceType)}{typeName}";
		}

		/// <summary>
		/// Gets the namespace with the trailing dot for the given type
		/// </summary>
		/// <param name="typeDescriptor"></param>
		/// <returns></returns>
		private string GetNamespaceWithDot(ExtractedTypeDescriptor typeDescriptor)
		{
			string typeNs = GetTypeNamespace(typeDescriptor);
			if (!string.IsNullOrEmpty(typeNs))
			{
				typeNs += ".";
			}
			return typeNs;
		}

		/// <summary>
		/// Gets the namespace (or prefix) of the given type
		/// </summary>
		/// <param name="typeDescriptor">The type descriptor</param>
		/// <returns>The namespace or prefix, or null if not applicable</returns>
		protected virtual string GetTypeNamespace(ExtractedTypeDescriptor typeDescriptor)
		{
			return typeDescriptor.Namespace;
		}

		/// <summary>
		/// Formats an enum that isn't extracted
		/// </summary>
		/// <param name="enumTypeDescriptor"></param>
		/// <returns></returns>
		public override string FormatNonExtractedEnum(NonExtractedEnumTypeDescriptor enumTypeDescriptor)
		{
			return TypeScriptHelper.NumericTypeName;
		}

		/// <summary>
		/// Formats nullable type
		/// </summary>
		/// <param name="nullableType"></param>
		/// <returns></returns>
		public override string FormatNullableType(NullableTypeDescriptor nullableType)
		{
			return nullableType.TypeArgument.FormatType(this);
		}

		/// <summary>
		/// Formats numeric type
		/// </summary>
		/// <param name="numericType"></param>
		/// <returns></returns>
		public override string FormatNumericType(NumericTypeDescriptor numericType)
		{
			return TypeScriptHelper.NumericTypeName;
		}

		/// <summary>
		/// Formats string type
		/// </summary>
		/// <param name="stringType"></param>
		/// <returns></returns>
		public override string FormatStringType(StringTypeDescriptor stringType)
		{
			return TypeScriptHelper.StringTypeName;
		}

		/// <summary>
		/// Formats a type param
		/// </summary>
		/// <param name="typeParameter"></param>
		/// <returns></returns>
		public override string FormatTypeParameter(TypeParameterDescriptor typeParameter)
		{
			return typeParameter.Type.Name;
		}

		/// <summary>
		/// Formats an unknown type
		/// </summary>
		/// <param name="unknownType"></param>
		/// <returns></returns>
		public override string FormatUnknownType(UnknownTypeDescriptor unknownType)
		{
			return TypeScriptHelper.AnyTypeName;
		}

		/// <summary>
		/// Formats a name declaration
		/// </summary>
		/// <param name="refType"></param>
		/// <returns></returns>
		public override string FormatNamedTypeDeclaration(ExtractedReferenceType refType)
		{

			// Create the type name appending the number of type arguments if there are duplicate names
			string name;
			if (NameCountIndex[GetIndexName(refType)] > 1 && refType.NamedType.TypeArguments.Count > 0)
			{
				name = $"{refType.Name}_{refType.NamedType.TypeArguments.Count}";
			}
			else
			{
				name = refType.Name;
			}


			if (refType.NamedType.TypeArguments.Count > 0)
			{
				name += $"<{string.Join(", ", refType.NamedType.TypeArguments.Select(arg => arg.Name))}>";
			}
			return name;
		}

		private string GetIndexName(ExtractedReferenceType refType) => $"{refType.Namespace}.{refType.Name}";
		private string GetIndexName(NamedReferenceTypeDescriptor refType) => $"{refType.Namespace}.{refType.Name}";
	}
}

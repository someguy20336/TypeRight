using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// A class that formats a type for the emitter
	/// </summary>
	public abstract class TypeFormatter
	{
		/// <summary>
		/// Formats an extracted reference type declaration
		/// </summary>
		/// <param name="refType"></param>
		/// <returns></returns>
		public abstract string FormatNamedTypeDeclaration(ExtractedReferenceType refType);

		/// <summary>
		/// Formats an anonymous type
		/// </summary>
		/// <param name="anonymousType"></param>
		/// <returns></returns>
		public abstract string FormatAnonymousType(AnonymousTypeDescriptor anonymousType);

		/// <summary>
		/// Formats an array type
		/// </summary>
		/// <param name="arrayType"></param>
		/// <returns></returns>
		public abstract string FormatArrayType(ArrayTypeDescriptor arrayType);

		/// <summary>
		/// Formats a dictionary type
		/// </summary>
		/// <param name="dictionaryType"></param>
		/// <returns></returns>
		public abstract string FormatDictionaryType(DictionaryTypeDescriptor dictionaryType);

		/// <summary>
		/// Formats an extracted enum type
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public abstract string FormatExtractedEnumType(ExtractedEnumTypeDescriptor enumType);
		
		/// <summary>
		/// Formats a list type
		/// </summary>
		/// <param name="listType"></param>
		/// <returns></returns>
		public abstract string FormatListType(ListTypeDescriptor listType);

		/// <summary>
		/// Formats a extracted referene type
		/// </summary>
		/// <param name="namedReferenceType"></param>
		/// <returns></returns>
		public abstract string FormatNamedReferenceType(NamedReferenceTypeDescriptor namedReferenceType);

		/// <summary>
		/// Formats a non extracted enum type
		/// </summary>
		/// <param name="enumTypeDescriptor"></param>
		/// <returns></returns>
		public abstract string FormatNonExtractedEnum(NonExtractedEnumTypeDescriptor enumTypeDescriptor);

		/// <summary>
		/// Formats a nullable type
		/// </summary>
		/// <param name="nullableType"></param>
		/// <returns></returns>
		public abstract string FormatNullableType(NullableTypeDescriptor nullableType);

		/// <summary>
		/// Formats a string type
		/// </summary>
		/// <param name="stringType"></param>
		/// <returns></returns>
		public abstract string FormatStringType(StringTypeDescriptor stringType);

		/// <summary>
		/// Formats a numeric type
		/// </summary>
		/// <param name="numericType"></param>
		/// <returns></returns>
		public abstract string FormatNumericType(NumericTypeDescriptor numericType);

		/// <summary>
		/// Formats a boolean type
		/// </summary>
		/// <param name="booleanType"></param>
		/// <returns></returns>
		public abstract string FormatBooleanType(BooleanTypeDescriptor booleanType);

		/// <summary>
		/// Formats a date time type
		/// </summary>
		/// <param name="dateTimeType"></param>
		/// <returns></returns>
		public abstract string FormatDateTimeType(DateTimeTypeDescriptor dateTimeType);

		/// <summary>
		/// Formats a type parameter
		/// </summary>
		/// <param name="typeParameter"></param>
		/// <returns></returns>
		public abstract string FormatTypeParameter(TypeParameterDescriptor typeParameter);

		/// <summary>
		/// Formats an unknown type
		/// </summary>
		/// <param name="unknownType"></param>
		/// <returns></returns>
		public abstract string FormatUnknownType(UnknownTypeDescriptor unknownType);
	}
}

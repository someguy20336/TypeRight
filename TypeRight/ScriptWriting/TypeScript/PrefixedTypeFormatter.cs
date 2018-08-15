using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// A type formatter that appends a static prefix to types
	/// </summary>
	public class PrefixedTypeFormatter : TypeScriptTypeFormatter
	{
		private readonly string _typePrefix;
		private readonly string _enumPrefix;

		/// <summary>
		/// Creates a prefixed type formatter
		/// </summary>
		/// <param name="typeCollection">The collection of all types</param>
		/// <param name="typePrefix">The prefix to use for reference types</param>
		/// <param name="enumPrefix">The prefix to use for enums</param>
		public PrefixedTypeFormatter(ExtractedTypeCollection typeCollection, string typePrefix, string enumPrefix) : base(typeCollection)
		{
			_typePrefix = typePrefix;
			_enumPrefix = enumPrefix;
		}

		/// <summary>
		/// Gets the type namespace
		/// </summary>
		/// <param name="extractedType"></param>
		/// <returns></returns>
		protected override string GetTypeNamespace(ExtractedTypeDescriptor extractedType)
		{
			if (extractedType is ExtractedEnumTypeDescriptor enumType)
			{
				return _enumPrefix;
			} 

			return _typePrefix;
		}
	}
}

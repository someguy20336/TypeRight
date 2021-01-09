using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	
	/// <summary>
	/// An object that resolves prefixes to types
	/// </summary>
	public interface ITypePrefixResolver
	{
		/// <summary>
		/// Gets the prefix for the given type
		/// </summary>
		/// <param name="typeDescriptor">The type descriptor</param>
		/// <returns>The prefix, if any</returns>
		string GetPrefix(ExtractedTypeDescriptor typeDescriptor);

		/// <summary>
		/// Gets the prefix for a given extracted type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns></returns>
		string GetPrefix(ExtractedType type);
	}
	
	/// <summary>
	/// A prefix resolver that accounts for an item being imported
	/// </summary>
	public class ModuleTypePrefixResolver : ITypePrefixResolver
	{
		private ImportManager _imports;

		/// <summary>
		/// Creates a new resolver from the import statements
		/// </summary>
		/// <param name="imports"></param>
		public ModuleTypePrefixResolver(ImportManager imports)
		{
			_imports = imports;
		}

		/// <summary>
		/// Gets the type namespace
		/// </summary>
		/// <param name="extractedType"></param>
		/// <returns></returns>
		public string GetPrefix(ExtractedTypeDescriptor extractedType)
		{
			if (_imports.ContainsImportPath(extractedType.TargetPath))
			{
				return _imports.GetImportAtPath(extractedType.TargetPath).ImportAlias;
			}
			return "";
		}

		/// <summary>
		/// Gets the prefix for a given extracted type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns></returns>
		public string GetPrefix(ExtractedType type)
		{
			if (_imports.ContainsImportPath(type.TargetPath))
			{
				return _imports.GetImportAtPath(type.TargetPath).ImportAlias;
			}
			return "";
		}
	}
}

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
	/// Prefix resolver for using namespaces
	/// </summary>
	public class NamespacedTypePrefixResolver : ITypePrefixResolver
	{
		private readonly string _enumNs;
		private readonly string _classNs;

		public NamespacedTypePrefixResolver(string enumNamespse, string classNamespace)
		{
			_enumNs = enumNamespse;
			_classNs = classNamespace;
		}

		/// <summary>
		/// Gets the prefix for the given type
		/// </summary>
		/// <param name="typeDescriptor">The type descriptor</param>
		/// <returns>The prefix, if any</returns>
		public string GetPrefix(ExtractedTypeDescriptor typeDescriptor)
		{
			if (typeDescriptor is ExtractedEnumTypeDescriptor)
			{
				return _enumNs;
			}
			else
			{
				return _classNs;
			}
		}

		/// <summary>
		/// Gets the prefix for a given extracted type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns></returns>
		public string GetPrefix(ExtractedType type)
		{
			if (type is ExtractedEnumType)
			{
				return _enumNs;
			}
			else
			{
				return _classNs;
			}
		}
	}

	/// <summary>
	/// A prefix resolver that accounts for an item being imported
	/// </summary>
	public class ModuleTypePrefixResolver : ITypePrefixResolver
	{
		private Dictionary<string, ImportStatement> _imports;

		/// <summary>
		/// Creates a new resolver from the import statements
		/// </summary>
		/// <param name="imports"></param>
		public ModuleTypePrefixResolver(Dictionary<string, ImportStatement> imports)
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
			if (_imports.ContainsKey(extractedType.TargetPath))
			{
				return _imports[extractedType.TargetPath].ImportAlias;
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
			if (_imports.ContainsKey(type.TargetPath))
			{
				return _imports[type.TargetPath].ImportAlias;
			}
			return "";
		}
	}
}

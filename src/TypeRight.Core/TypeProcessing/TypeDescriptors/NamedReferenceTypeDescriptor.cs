using TypeRight.CodeModel;
using TypeRight.ScriptWriting;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Descriptor for an extracted class or interface
	/// </summary>
	public class NamedReferenceTypeDescriptor : ExtractedTypeDescriptor
	{

		private TypeTable _typeTable;
		private IReadOnlyList<TypeDescriptor> _typeArgs;

		/// <summary>
		/// Gets the type arguments for this type
		/// </summary>
		public IReadOnlyList<TypeDescriptor> TypeArguments => GetOrCreateTypeArguments();

		/// <summary>
		/// Creates the named reference type descriptor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="table"></param>
		/// <param name="targetPath">The target path of the type</param>
		internal NamedReferenceTypeDescriptor(INamedType type, TypeTable table, string targetPath) : base(type, targetPath)
		{
			_typeTable = table;
		}

		private IReadOnlyList<TypeDescriptor> GetOrCreateTypeArguments()
		{
			if (_typeArgs == null)
			{
				_typeArgs = NamedType.TypeArguments.Select(arg => _typeTable.LookupType(arg)).ToList().AsReadOnly();
			}
			return _typeArgs;
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatNamedReferenceType(this);
		}
	}
}

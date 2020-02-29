using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A type descriptor for a nullable type
	/// </summary>
	public class NullableTypeDescriptor : TypeDescriptor
	{
		/// <summary>
		/// The type arg
		/// </summary>
		private TypeDescriptor _typeArg;

		/// <summary>
		/// The type table
		/// </summary>
		private TypeTable _typeTable;

		/// <summary>
		/// The named type
		/// </summary>
		private INamedType _namedType;

		/// <summary>
		/// Gets the type argument descriptor
		/// </summary>
		public TypeDescriptor TypeArgument => GetOrCreateTypeArg();

		/// <summary>
		/// Creates a nullable type descriptor
		/// </summary>
		/// <param name="namedType"></param>
		/// <param name="typeTable"></param>
		internal NullableTypeDescriptor(INamedType namedType, TypeTable typeTable) : base(namedType)
		{
			_namedType = namedType;
			_typeTable = typeTable;
		}

		/// <summary>
		/// Gets or creates the type argument
		/// </summary>
		/// <returns></returns>
		private TypeDescriptor GetOrCreateTypeArg()
		{
			if (_typeArg == null)
			{
				_typeArg = _typeTable.LookupType(_namedType.TypeArguments[0]);
			}
			return _typeArg;
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatNullableType(this);
		}
	}
}

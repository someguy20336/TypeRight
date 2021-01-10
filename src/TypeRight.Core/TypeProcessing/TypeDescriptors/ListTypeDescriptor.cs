using System;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A descriptor for a list type
	/// </summary>
	public class ListTypeDescriptor : TypeDescriptor
	{
		private INamedType _listType;
		private TypeTable _typeTable;
		private TypeDescriptor _typeArg;

		/// <summary>
		/// Gets the type argument
		/// </summary>
		public TypeDescriptor TypeArg => GetOrCreateTypeArg();

		/// <summary>
		/// Creates a new list type descriptor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="typeTable"></param>
		internal ListTypeDescriptor(INamedType type, TypeTable typeTable) : base(type)
		{
			_listType = type;
			_typeTable = typeTable;
		}

		private TypeDescriptor GetOrCreateTypeArg()
		{
			if (_typeArg == null)
			{
				if (_listType.TypeArguments.Count > 0)
				{
					_typeArg = _typeTable.LookupType(_listType.TypeArguments[0]);
				}
				else
				{
					_typeArg = new UnknownTypeDescriptor();
				}				
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
			return formatter.FormatListType(this);
		}
	}
}

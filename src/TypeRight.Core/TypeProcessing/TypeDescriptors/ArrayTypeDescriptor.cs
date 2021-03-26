using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A descriptor for an array type
	/// </summary>
	public class ArrayTypeDescriptor : TypeDescriptor
	{
		/// <summary>
		/// The array type
		/// </summary>
		private IArrayType _arrayType;

		/// <summary>
		/// The type table
		/// </summary>
		private TypeFactory _typeTable;

		/// <summary>
		/// the type arg
		/// </summary>
		private TypeDescriptor _elementType;

		/// <summary>
		/// Gets the type argument for the array
		/// </summary>
		public TypeDescriptor ElementType => GetOrCreateElementType();


		/// <summary>
		/// Creates a new array type descriptor
		/// </summary>
		/// <param name="type">The type</param>
		/// <param name="typeTable">The type table</param>
		internal ArrayTypeDescriptor(IArrayType type, TypeFactory typeTable) : base(type)
		{
			_arrayType = type;
			_typeTable = typeTable;
		}

		/// <summary>
		/// Gets or creates the element type
		/// </summary>
		/// <returns></returns>
		private TypeDescriptor GetOrCreateElementType()
		{
			if (_elementType == null)
			{
				_elementType = _typeTable.LookupType(_arrayType.ElementType);
			}
			return _elementType;
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatArrayType(this);
		}
	}
}

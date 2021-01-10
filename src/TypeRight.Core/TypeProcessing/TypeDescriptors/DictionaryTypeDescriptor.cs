using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A dictionary type descriptor
	/// </summary>
	public class DictionaryTypeDescriptor : TypeDescriptor
	{

		/// <summary>
		/// The type table
		/// </summary>
		private TypeTable _typeTable;

		private INamedType _dictType;

		private TypeDescriptor _key;

		private TypeDescriptor _value;

		/// <summary>
		/// Gets the descriptor for the key type
		/// </summary>
		public TypeDescriptor Key => GetOrCreateKey();

		/// <summary>
		/// Gets the descriptor for the value type
		/// </summary>
		public TypeDescriptor Value => GetOrCreateValue();

		/// <summary>
		/// Creates a dictionary type descriptor
		/// </summary>
		/// <param name="type">The type</param>
		/// <param name="table">The type table</param>
		internal DictionaryTypeDescriptor(INamedType type, TypeTable table) : base(type)
		{
			_typeTable = table;
			_dictType = type;
		}

		private TypeDescriptor GetOrCreateKey()
		{
			if (_key == null)
			{
				_key = _typeTable.LookupType(_dictType.TypeArguments[0]);
			}
			return _key;
		}

		private TypeDescriptor GetOrCreateValue()
		{
			if (_value == null)
			{
				_value = _typeTable.LookupType(_dictType.TypeArguments[1]);
			}
			return _value;
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatDictionaryType(this);
		}
	}
}

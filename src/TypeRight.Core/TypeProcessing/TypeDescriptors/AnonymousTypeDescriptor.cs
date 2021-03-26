using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A type descriptor for anonymous types
	/// </summary>
	public class AnonymousTypeDescriptor : TypeDescriptor
	{

		private TypeFactory _typeFactory;
		private IReadOnlyList<ExtractedProperty> _props;
		private INamedType _namedType;

		/// <summary>
		/// Gets the list of anonymous properties
		/// </summary>
		public IReadOnlyList<ExtractedProperty> Properties => GetOrCreateProperties();

		internal AnonymousTypeDescriptor(INamedType type, TypeFactory typeFactory) : base(type)
		{
			_namedType = type;
			_typeFactory = typeFactory;
		}

		private IReadOnlyList<ExtractedProperty> GetOrCreateProperties()
		{
			if (_props == null)
			{
				_props = _namedType.Properties.Select(pr => _typeFactory.CreateExtractedProperty(pr)).ToList().AsReadOnly();
			}
			return _props;
		}

		/// <summary>
		/// Formats the anonymous type
		/// </summary>
		/// <param name="formatter"></param>
		/// <returns></returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatAnonymousType(this);
		}
	}
}

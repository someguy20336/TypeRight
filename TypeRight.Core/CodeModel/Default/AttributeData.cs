using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class AttributeData : IAttributeData
	{
		public INamedType AttributeType { get; }

		public IReadOnlyDictionary<string, object> NamedArguments { get; }

		public IReadOnlyList<object> ConstructorArguments { get; }

		public AttributeData(INamedType attributeType, IReadOnlyDictionary<string, object> namedArguments = null, IReadOnlyList<object> constructorArguments = null)
		{
			AttributeType = attributeType;
			NamedArguments = namedArguments ?? new Dictionary<string, object>();
			ConstructorArguments = constructorArguments ?? new List<object>();
		}
	}
}

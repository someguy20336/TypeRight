using System;
using System.Collections.Generic;

namespace TypeRight.CodeModel.Default
{
	public class Property : IProperty
	{
		public string Name { get; }

		public IType PropertyType { get; }

		public string Comments { get; }

		public IEnumerable<IAttributeData> Attributes { get; }

		public Property(string name, IType propertyType, string comments = "", IEnumerable<IAttributeData> attrs = null)
		{
			Name = name;
			PropertyType = propertyType;
			Comments = comments;
			Attributes = attrs ?? Array.Empty<IAttributeData>();
		}
	}
}

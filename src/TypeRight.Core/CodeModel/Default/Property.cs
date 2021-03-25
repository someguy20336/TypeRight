using System;
using System.Collections.Generic;

namespace TypeRight.CodeModel.Default
{
	public class Property : IProperty
	{
		public string Name { get; }

		public IType PropertyType { get; }

		public string Comments { get; }

		public IEnumerable<IAttributeData> Attributes => Array.Empty<IAttributeData>();

		public Property(string name, IType propertyType, string comments = "")
		{
			Name = name;
			PropertyType = propertyType;
			Comments = comments;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class Field : IField
	{
		public string Name { get; }

		public string Comments { get; }

		public object Value { get; }

		public IReadOnlyList<IAttributeData> Attributes { get; }

		public Field(string name, object value, string comments = "", IReadOnlyList<IAttributeData> attributes = null)
		{
			Name = name;
			Value = value;
			Comments = comments;
			Attributes = attributes ?? new List<IAttributeData>();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class MethodParameter : IMethodParameter
	{
		public string Name { get; }

		public string Comments { get; }

		public IType ParameterType { get; }

		public IEnumerable<IAttributeData> Attributes { get; }

		public bool IsOptional { get; }

		public MethodParameter(string name, IType parameterType, bool optional = false, string comments = "", IEnumerable<IAttributeData> attributes = null)
		{
			Name = name;
			ParameterType = parameterType;
			Comments = comments;
			Attributes = attributes ?? new List<AttributeData>();
			IsOptional = optional;
		}
	}
}

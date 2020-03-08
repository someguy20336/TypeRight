using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class Method : IMethod
	{
		public string Name { get; }

		public string SummaryComments { get; }

		public string ReturnsComments { get; }

		public IType ReturnType { get; }

		public IReadOnlyList<IMethodParameter> Parameters { get; }

		public IReadOnlyList<IAttributeData> Attributes { get; }

		public Method(string name, 
			IType returnType, 
			IReadOnlyList<IMethodParameter> parameters = null, 
			IReadOnlyList<IAttributeData> attributes = null, 
			string summaryComments = "", 
			string returnsComments = "")
		{
			Name = name;
			ReturnType = returnType;
			Parameters = parameters ?? new List<IMethodParameter>();
			Attributes = attributes ?? new List<IAttributeData>();
			SummaryComments = summaryComments;
			ReturnsComments = returnsComments;
		}
	}
}

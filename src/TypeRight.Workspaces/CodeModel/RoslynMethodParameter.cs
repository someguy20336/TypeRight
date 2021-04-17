using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;

namespace TypeRight.Workspaces.CodeModel
{
	internal class RoslynMethodParameter : IMethodParameter
	{
		/// <summary>
		/// Gets the name of the parameter
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the comments for this parameter
		/// </summary>
		public string Comments { get; }

		/// <summary>
		/// Gets the type of the parameter
		/// </summary>
		public IType ParameterType { get; }

		/// <summary>
		/// Gets the attribues for this parameter
		/// </summary>
		public IEnumerable<IAttributeData> Attributes { get; }

		/// <summary>
		/// Gets whether this parameter is an optional parameter
		/// </summary>
		public bool IsOptional { get; }

		public RoslynMethodParameter(IParameterSymbol parameter, string comments, ParseContext context)
		{
			Name = parameter.Name;
			Comments = comments;
			ParameterType = RoslynType.CreateType(parameter.Type, context);
			Attributes = RoslynAttributeData.FromSymbol(parameter, context);
			IsOptional = parameter.IsOptional;
		}

		public override string ToString()
		{
			return $"{ParameterType.Name} {Name}";
		}
	}
}

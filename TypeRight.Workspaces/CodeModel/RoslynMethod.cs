using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynMethod : IMethod
	{

		private Lazy<IReadOnlyList<IAttributeData>> _attrs;

		private Lazy<IType> _returnType;

		private List<IMethodParameter> _parameters = new List<IMethodParameter>();

		public string Name { get; }

		public string SummaryComments { get; }

		public string ReturnsComments { get; }

		public IType ReturnType => _returnType.Value;

		public IReadOnlyList<IMethodParameter> Parameters => _parameters;

		public IReadOnlyList<IAttributeData> Attributes => _attrs.Value;

		public RoslynMethod(IMethodSymbol methodSymbol, ParseContext context)
		{
			Name = methodSymbol.Name;

			// Documentation
			XmlDocumentation doc = context.DocumentationProvider.GetDocumentationForSymbol(methodSymbol);
			SummaryComments = doc.Summary;
			ReturnsComments = doc.Returns;

			// Attributes
			_attrs = new Lazy<IReadOnlyList<IAttributeData>>(() => RoslynAttributeData.FromSymbol(methodSymbol, context));

			// Return Type
			_returnType = new Lazy<IType>(() =>
			{
				MethodReturnTypeHandler handler = context.GetMethodReturnTypeHandler(methodSymbol);
				IType returnType = handler.GetReturnType(context, methodSymbol);
				return returnType;
			});

			// Parameters
			_parameters = new List<IMethodParameter>();
			foreach (IParameterSymbol oneParam in methodSymbol.Parameters)
			{
				string comments = "";
				if (doc.Parameters.ContainsKey(oneParam.Name))
				{
					comments = doc.Parameters[oneParam.Name];
				}
				RoslynMethodParameter csParam = new RoslynMethodParameter(
					oneParam,
					comments,
					context);

				_parameters.Add(csParam);
			}
		}
	}
}

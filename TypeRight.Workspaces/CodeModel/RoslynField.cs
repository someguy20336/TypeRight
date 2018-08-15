using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynField : IField
	{

		private Lazy<IReadOnlyList<IAttributeData>> _attrs;

		public string Name { get; private set; }

		public string Comments { get; private set; }

		public object Value { get; private set; }

		public IReadOnlyList<IAttributeData> Attributes => _attrs.Value;

		public RoslynField(IFieldSymbol fieldSymbol, ParseContext context)
		{
			Name = fieldSymbol.Name;
			Comments = context.DocumentationProvider.GetDocumentationForSymbol(fieldSymbol).Summary;
			Value = fieldSymbol.ConstantValue;

			_attrs = new Lazy<IReadOnlyList<IAttributeData>>(() =>
			{
				return RoslynAttributeData.FromSymbol(fieldSymbol, context);
			});
		}
	}
}

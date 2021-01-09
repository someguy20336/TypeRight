using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynProperty : IProperty
	{
		private Lazy<IType> _propType;

		/// <summary>
		/// Gets the name of the property
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the type of this property
		/// </summary>
		public IType PropertyType => _propType.Value;

		/// <summary>
		/// Gets the comments for this property
		/// </summary>
		public string Comments { get; }

		public RoslynProperty(IPropertySymbol propSymbol, ParseContext context)
		{
			Name = propSymbol.Name;

			_propType = new Lazy<IType>(() =>
			{
				return RoslynType.CreateType(propSymbol.Type, context);
			});
			Comments = context.DocumentationProvider.GetDocumentationForSymbol(propSymbol).Summary;
		}

		public override string ToString()
		{
			return $"{PropertyType.ToString()} {Name}";
		}
	}
}

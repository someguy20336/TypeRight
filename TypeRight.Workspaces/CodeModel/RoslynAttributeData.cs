using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynAttributeData : IAttributeData
	{
		private Dictionary<string, object> _namedArgs = new Dictionary<string, object>();

		private List<object> _ctorArg = new List<object>();

		public INamedType AttributeType { get; }

		public IReadOnlyDictionary<string, object> NamedArguments => _namedArgs;

		public IReadOnlyList<object> ConstructorArguments => _ctorArg;

		public RoslynAttributeData(AttributeData attrData, ParseContext context)
		{
			AttributeType = new RoslynNamedType(attrData.AttributeClass, context);

			// Named args
			foreach (KeyValuePair<string, TypedConstant> namedArg in attrData.NamedArguments)
			{
				
				_namedArgs.Add(namedArg.Key, GetTypedConstantValue(namedArg.Value, context));
			}

			// Constructor Args
			foreach (TypedConstant arg in attrData.ConstructorArguments)
			{
				_ctorArg.Add(GetTypedConstantValue(arg, context));
			}
		}

		private object GetTypedConstantValue(TypedConstant typedConstant, ParseContext context)
		{
			object value;
			switch (typedConstant.Kind)
			{
				case TypedConstantKind.Primitive:
					value = typedConstant.Value;
					break;
				case TypedConstantKind.Type:
					value = new RoslynNamedType(typedConstant.Value as INamedTypeSymbol, context);
					break;

				case TypedConstantKind.Error:
				case TypedConstantKind.Enum:
				//TODO what does this give?
				case TypedConstantKind.Array:
				// TODO: what about array args? Currently don't need it
				default:
					value = null;
					break;
			}
			return value;
		}

		public static IReadOnlyList<RoslynAttributeData> FromSymbol(ISymbol symbol, ParseContext context)
		{
			return symbol.GetAttributes().Select(attr => new RoslynAttributeData(attr, context)).ToList();
		}
	}
}

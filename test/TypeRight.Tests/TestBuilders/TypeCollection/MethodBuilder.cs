using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Attributes;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	internal class MethodBuilder : IAttributable
	{
		private NamedTypeBuilder _parent;
		private IType _returnType;
		private string _name;
		public TypeCollectionBuilder TypeCollectionBuilder => _parent.TypeCollectionBuilder;
		
		private List<IMethodParameter> _parameters = new List<IMethodParameter>();

		public List<IAttributeData> Attributes { get; } = new List<IAttributeData>();

		public MethodBuilder(NamedTypeBuilder parent, string name, IType returnType)
		{
			_parent = parent;
			_name = name;
			_returnType = returnType;
		}

		public MethodBuilder AddParameter(string paramName, IType type, bool optional = false)
		{
			_parameters.Add(new MethodParameter(paramName, type, optional));
			return this;
		}

		public MethodBuilder AddParameter(string paramName, Type returnType, bool optional = false)
		{
			return AddParameter(paramName, TypeCollectionBuilder.GetNamedType(returnType), optional);
		}

		public MethodBuilder AddScriptActionAttribute()
		{
			Attributes.Add(new AttributeData(
				new NamedType(typeof(ScriptActionAttribute).Name, typeof(ScriptActionAttribute).FullName)
				));
			return this;
		}

		public NamedTypeBuilder Commit()
		{
			Method method = new Method(
				_name,
				_returnType,
				_parameters,
				Attributes);
			
			_parent.Methods.Add(method);
			return _parent;
		}
	}
}

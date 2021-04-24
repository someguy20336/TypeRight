using System.Collections.Generic;

namespace TypeRight.Tests.TestBuilders
{
	public class TestAttributeBuilder<T> where T : IAttributable
	{
		private T _parent;

		private string _name = "";

		private List<string> _ctorArgs = new List<string>();

		private Dictionary<string, string> _namedArgs = new Dictionary<string, string>();
		
		public TestAttributeBuilder(T parent, string name)
		{
			_parent = parent;
			_name = name;
		}

		public TestAttributeBuilder<T> AddConstructorArg(string value)
		{
			_ctorArgs.Add(value);
			return this;
		}

		public TestAttributeBuilder<T> AddStringConstructorArg(string value)
		{
			_ctorArgs.Add($"\"{value}\"");
			return this;
		}

		public TestAttributeBuilder<T> AddNamedArg(string name, string value)
		{
			_namedArgs.Add(name, value);
			return this;
		}

		public TestAttributeBuilder<T> AddStringNamedArg(string name, string value)
		{
			_namedArgs.Add(name, $"\"{value}\"");
			return this;
		}

		public T Commit()
		{
			AttributeInfo attr = new AttributeInfo()
			{
				AttributeTypeName = _name,
				CtorArguments = _ctorArgs,
				NamedArguments = _namedArgs,
			};
			_parent.Attributes.Add(attr);
			return _parent;
		}
	}
}

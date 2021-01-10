using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class TypeBase : IType
	{
		public string Name { get; }

		public TypeBase(string name)
		{
			Name = name;
		}
	}
}

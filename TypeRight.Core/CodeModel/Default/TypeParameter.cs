using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class TypeParameter : ITypeParameter
	{
		public string Name { get; }

		public TypeParameter(string name)
		{
			Name = name;
		}

	}
}

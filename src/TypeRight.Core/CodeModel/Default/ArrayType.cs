using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class ArrayType : IArrayType
	{
		public IType ElementType { get; }

		public string Name { get; }

		public ArrayType(IType elementType, string name)
		{
			ElementType = elementType;
			Name = name;
		}
	}
}

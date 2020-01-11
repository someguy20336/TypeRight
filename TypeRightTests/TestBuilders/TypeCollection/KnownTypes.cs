using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRightTests.TestBuilders.TypeCollection
{
	public static class KnownTypes
	{
		public static readonly INamedType StringType = GetNamedTypeForType(typeof(string));
		public static readonly INamedType IntType = GetNamedTypeForType(typeof(int));

		public static INamedType TryResolveKnownType(string name)
		{
			switch (name)
			{
				case "string": return StringType;
				case "int": return IntType;
				default:
					return null;
			}
		}

		private static NamedType GetNamedTypeForType(Type type) => new NamedType(type.Name, type.FullName);
	}
}

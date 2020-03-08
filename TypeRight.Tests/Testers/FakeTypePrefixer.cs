using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.Testers
{
	class FakeTypePrefixer : ITypePrefixResolver
	{
		public const string Prefix = "TestPrefix";

		public string GetPrefix(ExtractedTypeDescriptor typeDescriptor)
		{
			return Prefix;
		}

		public string GetPrefix(ExtractedType type)
		{
			return Prefix;
		}
	}
}

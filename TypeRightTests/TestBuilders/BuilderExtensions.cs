using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Attributes;

namespace TypeRightTests.TestBuilders
{
	internal static class BuilderExtensions
	{
		public static T AddScriptObjectAttribute<T>(this T builder) where T : IAttributable
		{
			AddAttribute(builder, typeof(ScriptObjectAttribute).FullName).Commit();
			return builder;
		}

		public static T AddScriptEnumAttribute<T>(this T builder) where T : IAttributable
		{
			AddAttribute(builder, typeof(ScriptEnumAttribute).FullName).Commit();
			return builder;
		}

		public static TestAttributeBuilder<T> AddAttribute<T>(this T builder, string name) where T : IAttributable
		{
			return new TestAttributeBuilder<T>(builder, name);
		}


		public static T AddProperty<T>(this T builder, string name, string type, string comments = "") where T : ITypeWithProperties
		{
			builder.Properties.Add(new SymbolInfo() { Name = name, Type = type, Comments = comments });
			return builder;
		}

		public static string GetAttributeText<T>(this T builder) where T : IAttributable
		{
			if (builder.Attributes.Count == 0)
			{
				return "";
			}
			return $"[{ string.Join(",", builder.Attributes.Select(attr => attr.ToFormattedString())) }]";
		}


	}
}

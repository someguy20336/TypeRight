using System;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	internal static class TypeCollectionBuilderExtensions
	{

		public static TypeCollectionBuilder AddAspNetCoreTypes(this TypeCollectionBuilder builder)
		{
			return builder.RegisterExternalType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.RouteAttributeName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.AreaAttribute, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.HttpGetAttributeName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.HttpPostAttributeName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetCoreNamespace);
		}

		public static TypeCollectionBuilder AddAspNetTypes(this TypeCollectionBuilder builder)
		{
			return builder.RegisterExternalType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.RouteAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.RouteAreaAttribute, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpGetAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpPostAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetNamespace)
			;
		}

		public static T AddAttribute<T>(this T attributable, string fullTypeName, params object[] ctorArgs) where T : IAttributable
		{
			attributable.Attributes.Add(new AttributeData(attributable.TypeCollectionBuilder.GetNamedType(fullTypeName), null, ctorArgs));
			return attributable;
		}

		public static T AddAttribute<T>(this T attributable, Type type, params object[] ctorArgs) where T : IAttributable
		{
			
			attributable.Attributes.Add(new AttributeData(attributable.TypeCollectionBuilder.GetNamedType(type), null, ctorArgs));
			return attributable;
		}

		public static T AddAttribute<T>(this T attributable, INamedType attrType, params object[] ctorArgs) where T : IAttributable
		{
			attributable.Attributes.Add(new AttributeData(attrType, null, ctorArgs));
			return attributable;
		}

		public static TypeCollectionTester BuildAndTest(this TypeCollectionBuilder builder) => new TypeCollectionTester(builder.Build());
	}
}

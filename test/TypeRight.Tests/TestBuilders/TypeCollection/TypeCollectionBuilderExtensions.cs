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
				.RegisterExternalType(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.HttpPatchAttributeName, MvcConstants.AspNetCoreNamespace)
				.RegisterExternalType(MvcConstants.ApiVersionAttributeFullName_AspNetCore)
				;
		}

		public static TypeCollectionBuilder AddAspNetTypes(this TypeCollectionBuilder builder)
		{
			return builder.RegisterExternalType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.RouteAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.RouteAreaAttribute, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpGetAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpPostAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.HttpPatchAttributeName, MvcConstants.AspNetNamespace)
				.RegisterExternalType(MvcConstants.ApiVersionAttributeFullName_AspNet)
			;
		}

		public static T AddAttribute<T>(this T attributable, string fullTypeName, params object[] ctorArgs) where T : IAttributable
		{
			attributable.Attributes.Add(new AttributeData(attributable.TypeCollectionBuilder.GetNamedType(fullTypeName), null, ctorArgs));
			return attributable;
		}

	}
}

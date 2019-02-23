using NetStandardLib;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

[assembly: ExternalScriptObject(
	typeof(NetStandardClass),
	typeof(GenericModel<>),
	typeof(TestTwoTypeParams<,>)
	)]
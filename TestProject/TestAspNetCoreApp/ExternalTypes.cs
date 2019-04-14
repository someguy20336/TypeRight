using NetStandardLib;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

[assembly: ExternalScriptObject(
	typeof(NetStandardClass),
	typeof(GenericModel<>),
	typeof(TestTwoTypeParams<,>)
	)]

[assembly: ExternalScriptObject("./Scripts/CustomGroup.ts",
	typeof(CustomGroupObject1),
	typeof(CustomGroupObj2)
	)]
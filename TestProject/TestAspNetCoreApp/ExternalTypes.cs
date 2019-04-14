using NetStandardLib;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

[assembly: ScriptObjects(
	typeof(NetStandardClass),
	typeof(GenericModel<>),
	typeof(TestTwoTypeParams<,>)
	)]

[assembly: ScriptObjects("./Scripts/CustomGroup.ts",
	typeof(CustomGroupObject1),
	typeof(CustomGroupObj2),
	typeof(CustomGroupObj2<>),
	typeof(ASimpleModel<,>)
	)]
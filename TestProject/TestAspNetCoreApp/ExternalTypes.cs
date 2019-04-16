using NetStandardLib;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

[assembly: ScriptObjects(
	typeof(NetStandardClass),
	typeof(GenericModel<>),
	typeof(TestTwoTypeParams<,>),
	typeof(NetStandardEnum)
	)]

[assembly: ScriptObjects("./Scripts/CustomGroup.ts",
	typeof(CustomGroupObject1),
	typeof(CustomGroupObj2),
	typeof(CustomGroupObj2<>),
	typeof(ASimpleModel<,>),
	typeof(ASimpleEnum)
	)]

[assembly: ScriptObjects("./Scripts/Home/Models.ts",
	typeof(CustomGroupObj3)
	)]
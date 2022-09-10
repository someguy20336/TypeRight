using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

[assembly: ScriptObjects(
    typeof(GenericModel<>),
    typeof(TestTwoTypeParams<,>)
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
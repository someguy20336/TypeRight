using TypeRight.Attributes;

namespace TestWebApp.TestClasses
{
    [ScriptEnum]
    public enum ExampleEnum
    {
        [DefaultEnumDisplayNameProvider(DisplayName = "UNO")]
        One,
        [DefaultEnumDisplayNameProvider(DisplayName = "Dos")]
        Two,
        Three
    }
}
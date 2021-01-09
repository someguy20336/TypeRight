using TypeRight.Attributes;

namespace TestWebApp.TestClasses
{
    [ScriptEnum]
    public enum ExampleEnum
    {
        [EnumDisplayName(DisplayName = "UNO")]
        One,
        [EnumDisplayName(DisplayName = "Dos")]
        Two,
        Three
    }
}
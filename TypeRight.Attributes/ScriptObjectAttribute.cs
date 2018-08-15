using System;

namespace TypeRight.Attributes
{
    /// <summary>
    /// Attribute used to mark an object for extraction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ScriptObjectAttribute : Attribute
	{
    }
}

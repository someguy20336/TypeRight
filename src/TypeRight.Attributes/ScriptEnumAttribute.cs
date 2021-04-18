using System;

namespace TypeRight.Attributes
{
    /// <summary>
    /// Marks an Enum for extraction
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class ScriptEnumAttribute : Attribute
    {
		/// <summary>
		/// Gets or sets whether the extended enum syntax should be used.  This syntax
		/// will allow you to use display names for enums if you intend to use them in code
		/// </summary>
		public bool UseExtendedSyntax { get; set; }
	}
}

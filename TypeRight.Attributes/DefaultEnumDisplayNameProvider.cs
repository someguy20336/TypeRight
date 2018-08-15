using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Default implementation of IEnumDisplayNameProvider
	/// </summary>
	public class DefaultEnumDisplayNameProvider: Attribute, IEnumDisplayNameProvider
    {
        /// <summary>
        /// Gets or sets the display name to use for this enum
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation to use
        /// </summary>
        public string Abbreviation { get; set; }
    }
}

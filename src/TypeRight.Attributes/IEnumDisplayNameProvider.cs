namespace TypeRight.Attributes
{
	/// <summary>
	/// Implemented by an attribute to provide a display name/abbreviation for an enum member. <br /><br />
	/// 
	/// You attribute must implement this interface.  You can use it in one of several ways:<br />
	///		1. Use the DisplayName and Abbreviation properties as named parameters <br />
	///		2. Use a constructor with a single string input.  This will set both the display and abbrev <br />
	///		3. Use a constructor with two (or more) string inputs.  The first input will be the display name and the second will be the abbreviation <br />
	/// </summary>
	public interface IEnumDisplayNameProvider
    {
        /// <summary>
        /// Gets the display name of the enum
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the abbreviation of the enum
        /// </summary>
        string Abbreviation { get; }
    }
}

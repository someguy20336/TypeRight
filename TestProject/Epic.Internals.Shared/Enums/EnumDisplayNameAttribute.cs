using System;

namespace Epic.Internals.Shared.Enums
{
	/// <summary>
	/// Attribute used for adding a description to an Enum
	/// </summary>
	public class EnumDisplayNameAttribute : Attribute
	{
		/// <summary>
		/// Gets the Display Name
		/// </summary>
		public string DisplayName { get; private set; }

		/// <summary>
		/// Gets the abbreviation
		/// </summary>
		public string Abbreviation { get; private set; }

		/// <summary>
		/// Creates a new Enum disp name with the given display name
		/// </summary>
		/// <param name="dispName">the disp name</param>
		public EnumDisplayNameAttribute(string dispName)
			: this(dispName, dispName)
		{
		}

		/// <summary>
		/// Creates a new Enum disp name with the given display name and
		/// an abbrev
		/// </summary>
		/// <param name="dispName">the disp name</param>
		/// <param name="abbrev">the abbreviation</param>
		public EnumDisplayNameAttribute(string dispName, string abbrev)
		{
			DisplayName = dispName;
			Abbreviation = abbrev;
		}
	}
}

using TypeRight.Attributes;
using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// An enum member that will be extracted into a script
	/// </summary>
	public class EnumMemberInfo
	{
		/// <summary>
		/// Gets the field for this member
		/// </summary>
		public IField Field { get; private set; }

		/// <summary>
		/// Gets the name of the enum member
		/// </summary>
		public string Name => Field.Name;

		/// <summary>
		/// Gets the display name of the enum
		/// </summary>
		public string DisplayName { get; private set; }

		/// <summary>
		/// Gets the abbreviation of this enum
		/// </summary>
		public string Abbreviation { get; private set; }

		/// <summary>
		/// Gets the comments for this enum
		/// </summary>
		public string Comments => Field.Comments;

		/// <summary>
		/// Gets the underlying value of the enum
		/// </summary>
		public int Value => (int)Field.Value;

		/// <summary>
		/// Creates a new enum member info object
		/// </summary>
		/// <param name="field">The field</param>
		/// <param name="dispNameParseFilter">The display name partse filter</param>
		public EnumMemberInfo(IField field, TypeFilter dispNameParseFilter)
		{
			Field = field;

			// Default all to be the same as name
			DisplayName = field.Name;
			Abbreviation = field.Name;

			IAttributeData dispNameAttr = GetDisplayNameAttr(dispNameParseFilter);

			if (dispNameAttr != null)
			{
				ProcessDisplayNameAttribute(dispNameAttr);
			}

		}

		private IAttributeData GetDisplayNameAttr(TypeFilter dispNameParseFilter)
		{
			string dispNameProvider = typeof(IEnumDisplayNameProvider).FullName;
			foreach (IAttributeData attr in Field.Attributes)
			{
				if (dispNameParseFilter.Matches(attr.AttributeType))
				{
					return attr;
				}
			}
			return null;
		}

		private void ProcessDisplayNameAttribute(IAttributeData attr)
		{
			string abbrev = null;
			string dispName = null;
			foreach (KeyValuePair<string, object> namedArgs in attr.NamedArguments)
			{
				if (namedArgs.Key == nameof(IEnumDisplayNameProvider.Abbreviation))
				{
					abbrev = (string)namedArgs.Value;
				}
				else if (namedArgs.Key == nameof(IEnumDisplayNameProvider.DisplayName))
				{
					dispName = (string)namedArgs.Value;
				}
			}

			// If it doesn't have a display name yet, try checking the constructor arguments
			// First arg is assumed to be display name, second arg assumed to be abbrev
			if (string.IsNullOrEmpty(dispName) && attr.ConstructorArguments.Count >= 1)
			{
				if (attr.ConstructorArguments.Count >= 1)
				{
					dispName = attr.ConstructorArguments[0].ToString();
				}
				if (attr.ConstructorArguments.Count == 1)
				{
					abbrev = dispName;
				}
				else
				{
					abbrev = attr.ConstructorArguments[1].ToString();
				}
			}

			if (!string.IsNullOrEmpty(abbrev))
			{
				Abbreviation = abbrev;
			}
			if (!string.IsNullOrEmpty(dispName))
			{
				DisplayName = dispName;
			}
		}
	}
}
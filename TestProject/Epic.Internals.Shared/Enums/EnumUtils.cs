using System;
using System.Reflection;

namespace Epic.Internals.Shared.Enums
{
	/// <summary>
	/// Provides a collection of useful enum utilities
	/// </summary>
	public static class EnumUtils
	{
		/// <summary>
		/// Converts an int to a <typeparamref name="TEnumType"/>
		/// </summary>
		/// <typeparam name="TEnumType">The type of enum to convert the int to </typeparam>
		/// <param name="number">the int number</param>
		/// <returns>the corresponding <typeparamref name="TEnumType"/> value</returns>
		public static TEnumType ToEnum<TEnumType>(this int number)
		{
			return (TEnumType)Enum.Parse(typeof(TEnumType), number.ToString());
		}

		/// <summary>
		/// Given an Enum, it returns a display name for it, given that it has the EnumDisplayNameAttribute.
		/// Otherwise, it just uses the name of the enum
		/// </summary>
		/// <param name="oneValue">The enum to find the display name for</param>
		/// <returns>The display name associated with the num</returns>
		public static string GetEnumDispName(this Enum oneValue)
		{
			return GetEnumDispNameAttribute(oneValue).DisplayName;
		}

		/// <summary>
		/// Given an Enum, it returns an abbreviation for it, given that it has the EnumDisplayNameAttribute.
		/// Otherwise, it just uses the name of the enum
		/// </summary>
		/// <param name="oneValue">The enum to find the abbrev for</param>
		/// <returns>The abbrev associated with the num</returns>
		public static string GetEnumAbbrev(this Enum oneValue)
		{
			return GetEnumDispNameAttribute(oneValue).Abbreviation;
		}

		/// <summary>
		/// Retrieves the enum display name attribute for the associated enum
		/// </summary>
		/// <param name="oneValue">the enum to get the attribute for</param>
		/// <returns>An enum display name attr</returns>
		public static EnumDisplayNameAttribute GetEnumDispNameAttribute(this Enum oneValue)
		{
			if (oneValue == null)
			{
				return new EnumDisplayNameAttribute("");
			}
			FieldInfo enumInfo = oneValue.GetType().GetField(oneValue.ToString());

			if (enumInfo == null)
			{
				return new EnumDisplayNameAttribute(oneValue.ToString());
			}

			EnumDisplayNameAttribute[] EnumAttributes = (EnumDisplayNameAttribute[])enumInfo.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
			if (EnumAttributes.Length > 0)
			{
				return EnumAttributes[0];
			}
			else
			{
				return new EnumDisplayNameAttribute(oneValue.ToString());  //just fake it
			}
		}
	}
}

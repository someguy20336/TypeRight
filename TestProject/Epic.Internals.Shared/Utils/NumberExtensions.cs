using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
	/// <summary>
	/// Contains a series of extension for numbers
	/// </summary>
	public static class NumberExtensions
	{
		/// <summary>
		/// Truncates a number after a given decimal place
		/// </summary>
		/// <param name="num">The number to truncate</param>
		/// <param name="decimalPlaces">The number of decimal places</param>
		/// <returns>The truncated number</returns>
		public static decimal TruncateToDecimal(this decimal num, int decimalPlaces)
		{
			decimal powTen = (decimal)Math.Pow(10, decimalPlaces);
			return Math.Truncate((num * powTen)) / powTen;
		}
	}
}

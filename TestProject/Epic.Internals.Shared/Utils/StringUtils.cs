using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
	/// <summary>
	/// Contains functions for string utilities
	/// </summary>
	public class StringUtils
	{
		/// <summary>
		/// Join a list of strings for display
		/// </summary>
		/// <param name="lst">The original list of strings</param>
		/// <param name="intermediate">the separator between elements</param>
		/// <param name="last">the separator just before last element</param>
		/// <returns>the combined string</returns>
		public static string Join(List<string> lst, string intermediate, string last)
		{
			if(lst == null || lst.Count == 0)
			{
				return "";
			}

			if(lst.Count == 1)
			{
				return lst[0];
			}

			string resp = lst[0];

			for(int i=1;i<lst.Count-1;++i)
			{
				resp += intermediate + lst[i];
			}

			return resp + last + lst[lst.Count - 1];
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
	/// <summary>
	/// execption extension functions
	/// </summary>
	public static class ExceptionUtils
	{
		/// <summary>
		/// Gets the first line of the exception message
		/// </summary>
		/// <param name="ex">This exception</param>
		/// <returns>The first line of the exception message</returns>
		public static string GetMessageFirstLineOnly(this Exception ex)
		{
			int index = ex.Message.IndexOf("\r\n");
			if (index < 0)
			{
				return ex.Message;
			}
			else
			{
				return ex.Message.Substring(0, index);
			}
		}

		/// <summary>
		/// Gets exception path
		/// </summary>
		/// <param name="ex">This exception</param>
		/// <param name="level">Exception level</param>
		/// <returns>Exception type names and messages</returns>
		private static string GetExceptionPath(this Exception ex, int level)
		{
			StringBuilder sb = new StringBuilder("");
			sb.Append('-', level);
			sb.Append(ex.GetType().FullName).Append(": ").Append(ex.Message);
			if (ex.InnerException != null)
			{
				sb.AppendLine().Append(ex.InnerException.GetExceptionPath(level + 1));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Gets the information (exception type and message, indented with dashes by level) 
		/// about the exception and its inner exceptions on the path.
		/// </summary>
		/// <param name="ex">This exception</param>
		/// <returns>Exception type names and messages</returns>
		public static string GetExceptionPath(this Exception ex)
		{
			return ex.GetExceptionPath(0);
		}


		/// <summary>
		/// Gets verbose exception message
		/// </summary>
		/// <param name="ex">This exception</param>
		/// <param name="includeAssemblyName">Include assembly name?</param>
		/// <param name="includeExceptionPath">Include exception path?</param>
		/// <param name="includeStackTrace">Include stack trace?</param>
		/// <returns>Verbose exception message</returns>
		public static string GetMessageVerbose(this Exception ex, bool includeAssemblyName, bool includeExceptionPath, bool includeStackTrace)
		{
			StringBuilder sb = new StringBuilder(ex.GetMessageFirstLineOnly());
			sb.AppendLine();
			sb.AppendLine();
			if (ex.Data != null)
			{
				foreach (DictionaryEntry de in ex.Data)
				{
					if (de.Key.ToString().StartsWith(ex.GetType().GUID.ToString())) continue;
					sb.Append(de.Key.ToString());
					sb.AppendLine(":");
					sb.AppendLine(de.Value == null ? "[NULL]" : de.Value.ToString());
					sb.AppendLine();
				}
			}

			if (includeAssemblyName)
			{
				sb.AppendLine("Assembly:")
					.AppendLine(ex.GetType().Assembly.FullName)
					.AppendLine();
			};

			if (includeExceptionPath)
			{
				sb.AppendLine("Exception path:")
					.AppendLine(ex.GetExceptionPath())
					.AppendLine();
			};

			if (includeStackTrace && !string.IsNullOrEmpty(ex.StackTrace))
			{
				sb.AppendLine("Stack trace:")
					.AppendLine(ex.StackTrace)
					.AppendLine();
			}

			return sb.ToString();
		}
	}
}

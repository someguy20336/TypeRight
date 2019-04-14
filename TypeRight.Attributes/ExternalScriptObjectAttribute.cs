using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Use this attribute to force extract a type that is not
	/// in the current assembly (or even solution).
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class ExternalScriptObjectAttribute : Attribute
	{
		/// <summary>
		/// Gets the external type to extract, though who cares about this prop
		/// </summary>
		public Type[] ExternalTypes { get; private set; }

		/// <summary>
		/// Creates a new External type extract attribute
		/// </summary>
		/// <param name="externalTypes">The external types to extract</param>
		public ExternalScriptObjectAttribute(params Type[] externalTypes)
		{
			ExternalTypes = externalTypes;
		}

		/// <summary>
		/// Creates a new External type extract attribute
		/// </summary>
		/// <param name="path">The path, relative to the root of the project, to save the result file to</param>
		/// <param name="externalTypes">The external types to extract</param>
		public ExternalScriptObjectAttribute(string path, params Type[] externalTypes)
		{
			ExternalTypes = externalTypes;
		}
	}
}

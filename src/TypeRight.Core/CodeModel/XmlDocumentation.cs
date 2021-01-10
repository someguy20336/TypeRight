using System.Collections.Generic;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Member XML documentation
	/// </summary>
	public abstract class XmlDocumentation
	{
		
		/// <summary>
		/// The list of parameters, param name and description
		/// </summary>
		protected Dictionary<string, string> ParamListProtected { get; } = new Dictionary<string, string>();

		/// <summary>
		/// Gets the XML summary for the symbol
		/// </summary>
		public string Summary { get; protected set; } = "";

		/// <summary>
		/// Gets the XML parameter documentation, if any.  The dictionary
		/// is indexed by parameter name, with a value of the parameter description
		/// </summary>
		public IReadOnlyDictionary<string, string> Parameters => ParamListProtected;

		/// <summary>
		/// Gets the returns comments
		/// </summary>
		public string Returns { get; protected set; } = "";
		
	}
}

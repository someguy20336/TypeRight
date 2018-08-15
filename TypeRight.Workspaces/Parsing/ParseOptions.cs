using System.Collections.Generic;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// The parse options for a given parse
	/// </summary>
	public class ParseOptions
	{
		/// <summary>
		/// Gets the method return type handlers
		/// </summary>
		public List<MethodReturnTypeHandler> MethodReturnTypeHandlers { get; } = new List<MethodReturnTypeHandler>();

		/// <summary>
		/// Gets the default parse options
		/// </summary>
		/// <returns>The default parse options</returns>
		public static ParseOptions GetDefault()
		{
			MethodReturnTypeHandler jsonHandler = new ParseSyntaxForTypeMethodHandler(
				"System.Web.Mvc.JsonResult",
				new InvocationReturnForwardFilter("Json", 0)
				);
			MethodReturnTypeHandler actionHandler = new ParseSyntaxForTypeMethodHandler(
				"System.Web.Mvc.ActionResult",
				new InvocationReturnForwardFilter("Json", 0)
				);
			ParseOptions options = new ParseOptions();
			options.MethodReturnTypeHandlers.Add(jsonHandler);
			options.MethodReturnTypeHandlers.Add(actionHandler);
			return options;
		}
	}
}

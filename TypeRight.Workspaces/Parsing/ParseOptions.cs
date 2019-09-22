using System.Collections.Generic;
using TypeRight.TypeProcessing;

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
			ParseOptions options = new ParseOptions();
			options.MethodReturnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetFullName("JsonResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			options.MethodReturnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetFullName("ActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			options.MethodReturnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("JsonResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			options.MethodReturnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("ActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			options.MethodReturnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("IActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			return options;
		}
	}
}

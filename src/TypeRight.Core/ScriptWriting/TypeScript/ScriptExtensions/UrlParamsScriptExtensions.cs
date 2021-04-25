using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;

namespace TypeRight.ScriptWriting.TypeScript.ScriptExtensions
{
	/// <summary>
	/// Initializes the URL parameters
	/// </summary>
	internal class InitUrlParamsScriptExtensions : IScriptExtension
	{
		public const string UrlParamsVarName = "urlParams";
		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"let {UrlParamsVarName} = new URLSearchParams();");
		}
	}

	/// <summary>
	/// Adds the string URL code
	/// </summary>
	internal class AddStringUrlParamsScriptExtension : IScriptExtension
	{
		public const string UrlQueryStringVarName = "queryString";

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"let {UrlQueryStringVarName} = \"\";");
			writer.WriteLine($"if ({InitUrlParamsScriptExtensions.UrlParamsVarName}.getAll().length > 0) {{");
			writer.PushIndent();
			writer.WriteLine($"{UrlQueryStringVarName} = \"?\" + {InitUrlParamsScriptExtensions.UrlParamsVarName}.toString();");
			writer.PopIndent();
			writer.WriteLine("}");
		}
	}

	/// <summary>
	/// Adds a simple key/value parameter to the query string
	/// </summary>
	internal class AddSimpleParameterToQueryStringScriptExtension : IScriptExtension
	{
		private string _paramName;

		public AddSimpleParameterToQueryStringScriptExtension(string paramName)
		{
			_paramName = paramName;
		}

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"{QueryParameterHelperFunctions.TryAppendKeyValueFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName}, \"{_paramName}\", {_paramName});");
		}
	}

	internal class AddComplexParameterToQueryStringScriptExtension : IScriptExtension
	{
		private string _paramName;

		public AddComplexParameterToQueryStringScriptExtension(string paramName)
		{
			_paramName = paramName;
		}

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"{QueryParameterHelperFunctions.TryAppendObjectFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName}, {_paramName});");
		}
	}
}

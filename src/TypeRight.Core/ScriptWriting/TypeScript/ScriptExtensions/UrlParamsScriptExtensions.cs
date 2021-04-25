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

	internal class AddKeyValueToQueryStringScriptExtension : IScriptExtension
	{
		private readonly string _key;
		private readonly string _value;

		public AddKeyValueToQueryStringScriptExtension(string key, string value)
		{
			_key = key;
			_value = value;
		}

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"{QueryParameterHelperFunctions.TryAppendKeyValueFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName}, \"{_key}\", \"{_value}\");");
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

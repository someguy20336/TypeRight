using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.TypeProcessing;

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
			writer.WriteLine($"const {UrlParamsVarName} = new URLSearchParams();");
		}
	}

	/// <summary>
	/// Adds a simple key/value parameter to the query string
	/// </summary>
	internal class AddSimpleParameterToQueryStringScriptExtension : IScriptExtension
	{
		private readonly MvcActionParameter _param;

		public AddSimpleParameterToQueryStringScriptExtension(MvcActionParameter param)
		{
			_param = param;
		}

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"{QueryParameterHelperFunctions.TryAppendKeyValueFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName}, \"{_param.ApiName}\", {_param.Name});");
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
		private readonly MvcActionParameter _param;

		public AddComplexParameterToQueryStringScriptExtension(MvcActionParameter param)
		{
			_param = param;
		}

		public void Write(IScriptWriter writer)
		{
			writer.WriteLine($"{QueryParameterHelperFunctions.TryAppendObjectFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName}, {_param.Name});");
		}
	}
}

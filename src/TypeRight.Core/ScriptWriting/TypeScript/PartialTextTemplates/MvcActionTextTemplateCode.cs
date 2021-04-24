using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	partial class MvcActionTextTemplate
	{
		private MvcAction _action;
		private FetchFunctionDescriptor _fetchFunc;
		private TypeFormatter _formatter;

		public MvcActionTextTemplate(MvcAction action, FetchFunctionDescriptor fetchFunc, TypeFormatter formatter)
		{
			_action = action;
			_formatter = formatter;
			_fetchFunc = fetchFunc;
		}

		/// <summary>
		/// Builds the fetch function name, including the return keyword if necessary
		/// </summary>
		/// <param name="actionInfo"></param>
		/// <returns></returns>
		private string BuildFetchFunctionName()
		{
			if (_fetchFunc.ReturnType == "void")
			{
				return _fetchFunc.FunctionName;
			}
			else
			{
				return "return " + _fetchFunc.FunctionName;
			}
		}

		/// <summary>
		/// Builds the action signature
		/// </summary>
		/// <param name="action">The action to build the signature for</param>
		/// <returns>The string signature</returns>
		private string BuildActionSignature()
		{
			List<string> actionParams = new List<string>();

			var nonIgnoredParams = _action.Parameters.Where(p => p.BindingType != ActionParameterSourceType.Ignored);

			var methodRequiredParameters = nonIgnoredParams.Where(p => !p.IsOptional).Select(FormatMethodParameter);
			var userRequiredParameters = _fetchFunc.AdditionalParameters.Where(p => !p.Optional).Select(FormatUserParameter);
			var methodOptionalParameters = nonIgnoredParams.Where(p => p.IsOptional).Select(FormatMethodParameter);
			var userOptionalParameters = _fetchFunc.AdditionalParameters.Where(p => p.Optional).Select(FormatUserParameter);

			actionParams.AddRange(
				methodRequiredParameters
				.Union(userRequiredParameters)
				.Union(methodOptionalParameters)
				.Union(userOptionalParameters)
				);

			return $"{_action.ScriptName}({string.Join(", ", actionParams)}): { ReplaceTokens(_fetchFunc.ReturnType) }";
		}

		private string FormatMethodParameter(MvcActionParameter oneParam)
		{
			string paramTypes = string.Join(" | ", oneParam.Types.Select(t => t.FormatType(_formatter)));
			return $"{oneParam.Name}{ (oneParam.IsOptional ? "?" : "") }: {paramTypes}";
		}

		private string FormatUserParameter(ActionParameter userParam)
		{
			string paramType = ReplaceTokens(userParam.Type);
			return $"{userParam.Name}{ (userParam.Optional ? "?" : "") }: {paramType}";
		}

		private string BuildFetchParameters()
		{
			var allParams = _fetchFunc.FetchParameterResolvers.Select(pr => pr.ResolveParameter(_action));
			return string.Join(", ", allParams);
		}

		/// <summary>
		/// Gets the key value pairs of parameters and comments for this action
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns></returns>
		private IEnumerable<KeyValuePair<string, string>> GetParameterComments()
		{
			// Get the params that should actually be written
			HashSet<string> allParams = new HashSet<string>(_action.Parameters
				.Where(p => p.BindingType != ActionParameterSourceType.Ignored)
				.Select(p => p.Name)
				);

			return _action.ParameterComments.Where(kv => allParams.Contains(kv.Key));
		}

		private string ReplaceTokens(string typeStr)
		{
			return typeStr.Replace("$returnType$", _action.ReturnType.FormatType(_formatter));
		}
	}
}

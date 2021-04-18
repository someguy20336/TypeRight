﻿using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	partial class MvcActionTextTemplate
	{
		private MvcActionInfo _curAction;
		private FetchFunctionDescriptor _curFetchFunc;
		private TypeFormatter _formatter;

		protected ControllerContext Context { get; private set; }

		public MvcActionTextTemplate(ControllerContext context, ImportManager imports)
		{
			_formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(imports));
			Context = context;
		}

		public string WriteAction(MvcActionInfo action)
		{
			_curAction = action;
			_curFetchFunc = Context.FetchFunctionResolver.Resolve(_curAction.RequestMethod.Name);

			GenerationEnvironment.Clear();
			return TransformText();
		}

		/// <summary>
		/// Builds the fetch function name, including the return keyword if necessary
		/// </summary>
		/// <param name="actionInfo"></param>
		/// <returns></returns>
		private string BuildFetchFunctionName()
		{
			if (_curFetchFunc.ReturnType == "void")
			{
				return _curFetchFunc.FunctionName;
			}
			else
			{
				return "return " + _curFetchFunc.FunctionName;
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

			var nonIgnoredParams = _curAction.Parameters.Where(p => p.BindingType != ActionParameterSourceType.Ignored);

			var methodRequiredParameters = nonIgnoredParams.Where(p => !p.IsOptional).Select(FormatMethodParameter);
			var userRequiredParameters = _curFetchFunc.AdditionalParameters.Where(p => !p.Optional).Select(FormatUserParameter);
			var methodOptionalParameters = nonIgnoredParams.Where(p => p.IsOptional).Select(FormatMethodParameter);
			var userOptionalParameters = _curFetchFunc.AdditionalParameters.Where(p => p.Optional).Select(FormatUserParameter);

			actionParams.AddRange(
				methodRequiredParameters
				.Union(userRequiredParameters)
				.Union(methodOptionalParameters)
				.Union(userOptionalParameters)
				);

			return $"{_curAction.Name}({string.Join(", ", actionParams)}): { ReplaceTokens(_curFetchFunc.ReturnType) }";
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
			var allParams = _curFetchFunc.FetchParameterResolvers.Select(pr => pr.ResolveParameter(_curAction));
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
			HashSet<string> allParams = new HashSet<string>(_curAction.Parameters
				.Where(p => p.BindingType != ActionParameterSourceType.Ignored)
				.Select(p => p.Name)
				);

			return _curAction.ParameterComments.Where(kv => allParams.Contains(kv.Key));
		}

		private string ReplaceTokens(string typeStr)
		{
			return typeStr.Replace("$returnType$", _curAction.ReturnType.FormatType(_formatter));
		}
	}
}

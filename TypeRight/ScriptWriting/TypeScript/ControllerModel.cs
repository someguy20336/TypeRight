﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.ScriptWriting.TypeScript
{
	public enum ActionParameterSourceType
	{
		Url,
		Body,
		Fetch,
		Ignored
	}

	public class ControllerModel
	{
		public string Name { get; set; }
		public IEnumerable<ImportStatement> Imports { get; set; }

		public IEnumerable<ControllerActionModel> Actions { get; set; }
	}

	public class ControllerActionModel
	{
		public string Name { get; set; }

		public string SummaryComments { get; set; }

		public string ReturnsComments { get; set; }

		/// <summary>
		/// Gets the parameter comments in an index of parameter name description
		/// </summary>
		public IReadOnlyDictionary<string, string> ParameterComments { get; set; }

		public IEnumerable<ActionParameterModel> Parameters { get; set; }

		public string BaseUrl { get; set; }

		public string ReturnType { get; set; }

		public string FetchFunctionName { get; set; }
	}

	public class ActionParameterModel
	{
		public ActionParameterSourceType ActionParameterSourceType { get; set; }

		public string Name { get; set; }

		public string Comments { get; set; }

		public string ParameterType { get; set; }

		public bool IsOptional { get; set; }
	}
}
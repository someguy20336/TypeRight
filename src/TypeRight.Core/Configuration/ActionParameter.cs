using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.Configuration.Json;

namespace TypeRight.Configuration
{

	public enum ParameterKind
	{
		Custom,
		RequestMethod,
		Url,
		Body
	}

	/// <summary>
	/// An action parameter
	/// </summary>
	[JsonConverter(typeof(ActionParameterJsonConverter))]
	public class ActionParameter
	{
		public static readonly ActionParameter Url = new ActionParameter() { Kind = ParameterKind.Url };
		public static readonly ActionParameter RequestMethod = new ActionParameter() { Kind = ParameterKind.RequestMethod };
		public static readonly ActionParameter Body = new ActionParameter() { Kind = ParameterKind.Body };


		public ParameterKind Kind { get; private set; }

		public string Name { get; private set; }

		public string Type { get; private set; }

		public bool Optional { get; private set; }

		private ActionParameter() { }

		public ActionParameter(string name, string type, bool isOptional)
		{
			Kind = ParameterKind.Custom;
			Name = name;
			Type = type;
			Optional = isOptional;
		}

		public static ActionParameter FromName(string name)
		{
			if (Enum.TryParse<ParameterKind>(name, true, out var kind))
			{

				switch (kind)
				{
					case ParameterKind.RequestMethod:
						return RequestMethod;
					case ParameterKind.Url:
						return Url;
					case ParameterKind.Body:
						return Body;
					default:
						break;
				}

			}
			return new ActionParameter("ERROR", "ERROR", false);

		}
	}
}

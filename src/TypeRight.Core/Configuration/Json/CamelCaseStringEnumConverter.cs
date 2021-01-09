using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace TypeRight.Configuration.Json
{
	/// <summary>
	/// Converts an enum with camel casing
	/// </summary>
	class CamelCaseStringEnumConverter : StringEnumConverter
	{
		public CamelCaseStringEnumConverter()
		{
			NamingStrategy = new CamelCaseNamingStrategy();
		}
	}
}

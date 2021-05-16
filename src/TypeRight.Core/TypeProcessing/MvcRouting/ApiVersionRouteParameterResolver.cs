using System.Linq;
using System.Text.RegularExpressions;

namespace TypeRight.TypeProcessing.MvcRouting
{
	internal class ApiVersionRouteParameterResolver : RouteParameterResolver
	{
		private static Regex s_apiVersionToken = new Regex("{([a-zA-Z]+)(:apiVersion)}");

		public static ApiVersionRouteParameterResolver AspNetCore = new ApiVersionRouteParameterResolver(MvcConstants.ApiVersionAttributeFullName_AspNetCore);

		private readonly string _attrTypeFullName;

		private ApiVersionRouteParameterResolver(string attrTypeFullName)
		{
			_attrTypeFullName = attrTypeFullName;
		}

		public override string TryResolve(string currentRoute, MvcController controller, MvcAction action)
		{
			var apiVersionAttr = controller.NamedType.Attributes.LastOrDefault(attr => attr.AttributeType.FullName == _attrTypeFullName);
			if (apiVersionAttr == null)
			{
				return currentRoute;
			}

			// TODO: can override on action... but eh for now

			string version = apiVersionAttr.ConstructorArguments[0].ToString();
			return s_apiVersionToken.Replace(currentRoute, version);
		}
	}
}

namespace TypeRight.TypeProcessing.MvcRouting
{
	public abstract class RouteParameterResolver
	{
		public abstract string TryResolve(string currentRoute, MvcControllerInfo controller, MvcActionInfo action);
	}


	internal class DelegateRouteParameterResolver : RouteParameterResolver
	{
		private readonly ResolverFunc _func;

		public delegate string ResolverFunc(string currentRoute, MvcControllerInfo controller, MvcActionInfo action);

		public DelegateRouteParameterResolver(ResolverFunc func)
		{
			_func = func;
		}

		public override string TryResolve(string currentRoute, MvcControllerInfo controller, MvcActionInfo action) => _func(currentRoute, controller, action);
	}

}

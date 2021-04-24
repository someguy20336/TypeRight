namespace TypeRight.TypeProcessing.MvcRouting
{
	public abstract class RouteParameterResolver
	{
		public abstract string TryResolve(string currentRoute, MvcController controller, MvcAction action);
	}


	internal class DelegateRouteParameterResolver : RouteParameterResolver
	{
		private readonly ResolverFunc _func;

		public delegate string ResolverFunc(string currentRoute, MvcController controller, MvcAction action);

		public DelegateRouteParameterResolver(ResolverFunc func)
		{
			_func = func;
		}

		public override string TryResolve(string currentRoute, MvcController controller, MvcAction action) => _func(currentRoute, controller, action);
	}

}

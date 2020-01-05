namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// For a given method, this filter will forward the "return type" to the argument at the given index
	/// </summary>
	public class InvocationReturnForwardFilter
	{
		/// <summary>
		/// Gets the method name this filter applies to
		/// </summary>
		public string AppliesToMethodWithName { get; private set; }

		/// <summary>
		/// Gets the index of the argument the return type should be forwarded to
		/// </summary>
		public int UseArgumentIndex { get; private set; }

		/// <summary>
		/// Creates a new filter
		/// </summary>
		/// <param name="methodName">The method name</param>
		/// <param name="argIndex">The argument index</param>
		public InvocationReturnForwardFilter(string methodName, int argIndex)
		{
			AppliesToMethodWithName = methodName;
			UseArgumentIndex = argIndex;
		}
	}
}

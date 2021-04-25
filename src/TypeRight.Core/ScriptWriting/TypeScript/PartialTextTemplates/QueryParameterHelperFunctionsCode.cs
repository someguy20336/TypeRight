namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	/// <summary>
	/// Note - this template is internal and is only public to unit testing purposes.  
	/// This could change at any time
	/// </summary>
	public partial class QueryParameterHelperFunctions
	{
		public const string TryAppendObjectFuncName = "tryAppendObjectValuesToUrl";
		public const string TryAppendKeyValueFuncName = "tryAppendKeyValueToUrl";
		private readonly bool _needsAppendObjectFunc;

		public QueryParameterHelperFunctions(bool needsAppendObjectFunc)
		{
			_needsAppendObjectFunc = needsAppendObjectFunc;
		}
	}
}

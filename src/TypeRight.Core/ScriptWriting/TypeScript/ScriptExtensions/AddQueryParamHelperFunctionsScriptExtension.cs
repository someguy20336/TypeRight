using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;

namespace TypeRight.ScriptWriting.TypeScript.ScriptExtensions
{
	internal class AddQueryParamHelperFunctionsScriptExtension : IScriptExtension
	{
		private readonly bool _needsObjHelperFunc;

		public AddQueryParamHelperFunctionsScriptExtension(bool needsObjHelperFunc)
		{
			_needsObjHelperFunc = needsObjHelperFunc;
		}

		public void Write(IScriptWriter writer)
		{
			string text = new QueryParameterHelperFunctions(_needsObjHelperFunc).TransformText();
			writer.Write(text);
		}
	}
}

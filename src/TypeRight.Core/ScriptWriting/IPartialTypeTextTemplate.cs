using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// An partial text template responsible for generating text for a specfic type
	/// </summary>
	internal interface IPartialTypeTextTemplate
	{
		string GetText(ExtractedType type);

		void PushIndent(string indent);
	}
}

using TypeRight.CodeModel;

namespace TypeRight
{
	public class ScriptGenerationParameters
	{
		public string ProjectPath { get; set; }

		public ITypeIterator TypeIterator { get; set; }

		public bool Force { get; set; }
	}
}

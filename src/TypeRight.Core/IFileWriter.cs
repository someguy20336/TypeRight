using System.IO;

namespace TypeRight
{
	public interface IFileWriter
	{
		void WriteFile(string path, string contents);
	}

	public class FileSystemWriter : IFileWriter
	{
		public void WriteFile(string path, string contents)
		{
			File.WriteAllText(path, contents);
		}
	}
}

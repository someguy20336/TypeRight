using System.Collections.Generic;

namespace TypeRight.Configuration
{
	/// <summary>
	/// The import definiition
	/// </summary>
	public class ImportDefinition
	{
		public List<string> Items { get; set; } = new List<string>();

		public bool UseAlias { get; set; } = false;

		public string Path { get; set; }
	}
}

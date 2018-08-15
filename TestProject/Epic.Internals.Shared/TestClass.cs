using ClientScriptGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared
{
	/// <summary>
	/// These Are class XML
	/// </summary>
	[ClientScriptObject]
	class TestClass
	{
		/// <summary>
		/// String prop XML
		/// Two lines
		/// Heyoooo, lezgo 3
		/// </summary>
		public string StringProperty { get; set; }

		/// <summary>
		/// Int Prop XML
		/// </summary>
		public int IntProperty { get; set; }

        public int[] IntArray { get; set; }

        public List<string> StringList { get; set; }

        public Dictionary<string, int> DictionaryStringInt { get; set; }

        public AnExtractedClass<int> ExtractedClass { get; set; }

		public NotExtractedClass NotExtracted { get; set; }
	}


    [ClientScriptObject]
	class AnExtractedClass<T>
	{
        public T GenericType { get; set; }

        public List<T> GenericList { get; set; }
    }

	class NotExtractedClass
	{

	}
}

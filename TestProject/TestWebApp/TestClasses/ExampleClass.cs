using TypeRight.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DependentLibrary;
using System.IO;

[assembly: ExternalTypeExtract(typeof(Class1), typeof(FileInfo))]

namespace TestWebApp.TestClasses
{
	[ScriptObject]
	public class ExampleClass
	{
        /// <summary>
        /// Doc for int - i did it yea more comments more
        /// </summary>
		public int IntProperty { get; set; }

        /// <summary>
        /// String list! more! no more ya hey hi no
        /// </summary>
		public List<string> StringList { get; set; }

		/// <summary>
		/// Adding documentation for this class hey, yo hola
		/// </summary>
        public int AnotherPropertyYea { get; set; }

        /// <summary>
        /// Another
        /// </summary>
        public bool BooleanProp { get; set; }

        /// <summary>
        /// new thing again
        /// </summary>
        public int Hey { get; private set; }

        public int AddedProp { get; private set; }
    }

    [ScriptObject]
    public class Extends : ExampleClass
    {
        public int OneMoreProp { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeLocation;

namespace TypeRight
{
	public class ScriptGenerationParameters
	{
		public string ProjectPath { get; set; }

		public ITypeIterator TypeIterator { get; set; }
	}
}

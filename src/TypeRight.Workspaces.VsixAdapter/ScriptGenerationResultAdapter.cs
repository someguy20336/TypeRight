using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.VsixContract;

namespace TypeRight.Workspaces.VsixAdapter
{
	class ScriptGenerationResultAdapter : IScriptGenerationResult
	{
		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

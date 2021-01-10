using System;
using TypeRight.Attributes;

namespace AspNetCoreTargetNetFramework.Models
{
	[ScriptObject]
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
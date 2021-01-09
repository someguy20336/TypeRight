using System;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Models
{
	/// <summary>
	/// Test summ
	/// </summary>
	[ScriptObject]
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
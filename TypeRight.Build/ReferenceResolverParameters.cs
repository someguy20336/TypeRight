using System;

namespace TypeRight.Build
{
	/// <summary>
	/// An object that can be used to provide parameters to the ReferenceResolver
	/// </summary>
	public class ReferenceResolverParameters : MarshalByRefObject
	{
		/// <summary>
		/// Gets or sets the search paths
		/// </summary>
		public string[] SearchPaths { get; set; }

		/// <summary>
		/// Gets or sets the target framework directories
		/// </summary>
		public string[] TargetFrameworkDirectories { get; set; }

		/// <summary>
		/// Gets or sets the full framework folders
		/// </summary>
		public string[] FullFrameworkFolders { get; set; }

		/// <summary>
		/// Gets or sets the full framework subset names
		/// </summary>
		public string[] FullTargetFrameworkSubsetNames { get; set; }

		/// <summary>
		/// Gets or sets the allowed assembly extensions
		/// </summary>
		public string[] AllowedAssemblyExtensions { get; set; }

		/// <summary>
		/// Gets or sets the allowed related file extensions
		/// </summary>
		public string[] AllowedRelatedFileExtensions { get; set; }

		/// <summary>
		/// Gets or sets the target processor architechure
		/// </summary>
		public string TargetProcessorArchitecture { get; set; }

		/// <summary>
		/// Gets or sets the target framework moniker
		/// </summary>
		public string TargetFrameworkMoniker { get; set; }

		/// <summary>
		/// Gets or sets the target framework version in the form "vX.Y"
		/// </summary>
		public string TargetFrameworkVersion { get; set; }

		/// <summary>
		/// Gets or sets the targeted runtime version
		/// </summary>
		public string TargetedRuntimeVersion { get; set; }
	}
}

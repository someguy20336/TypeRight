using TypeRight.Build;
using TypeRight.Workspaces;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace TypeRightTests.Tests
{
	/// <summary>
	/// Default parameter provider that uses this assembly to create the parameters
	/// </summary>
	public class TestRefResolverParams : ReferenceResolverParameters
	{
		/// <summary>
		/// Creates a new default reference resolver param provider
		/// </summary>
		public TestRefResolverParams()
		{

			Assembly thisAssembly = GetType().Assembly;
			AssemblyName assemblyName = thisAssembly.GetName();
			TargetFrameworkAttribute targetFrameworkAttr = thisAssembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), false)?[0] as TargetFrameworkAttribute;

			// Target Framework
			FrameworkName name = new FrameworkName(targetFrameworkAttr.FrameworkName);
			TargetFrameworkMoniker = name.FullName;
			TargetFrameworkVersion = $"v{name.Version.Major}.{name.Version.Minor}";  // i.e. v4.6

			string progFilesFolder;
			if (assemblyName.ProcessorArchitecture == System.Reflection.ProcessorArchitecture.X86)
			{
				progFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
				TargetProcessorArchitecture = "x86";
			}
			else
			{
				progFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				TargetProcessorArchitecture = assemblyName.ProcessorArchitecture.ToString();  // ??
			}

			// Framework directories
			string targetPath = Path.Combine(progFilesFolder, @"Reference Assemblies\Microsoft\Framework\.NETFramework\", TargetFrameworkVersion);
			TargetFrameworkDirectories = new string[]
			{
				targetPath,
				Path.Combine(targetPath, "Facades")
			};

			TargetedRuntimeVersion = $"v{Environment.Version.Major}.{Environment.Version.Minor}.{Environment.Version.Build}"; // i.e. "v4.0.30319";

			SearchPaths = new string[]{
				"{CandidateAssemblyFiles}",
				"{HintPathFromItem}",
				"{TargetFrameworkDirectory}",
				@"{Registry: Software\Microsoft\.NETFramework,v4.6,AssemblyFoldersEx}",
				"{AssemblyFolders}",
				"{GAC}",
				"{RawFileName}",
			};

			AllowedAssemblyExtensions = new string[]
			{
				".winmd",
				".dll",
				".exe"
			};

			AllowedRelatedFileExtensions = new string[] { ".pdb", ".xml", ".pri" };
			FullFrameworkFolders = new string[] { @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\" };
			FullTargetFrameworkSubsetNames = new string[] { "Full" };
		}
	}
}

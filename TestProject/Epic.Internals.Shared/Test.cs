using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared
{
	public class Test
	{
		public async static void TestThis()
		{
			MSBuildWorkspace workspace = MSBuildWorkspace.Create();
			string path = @"C:\Users\Dave\Documents\Visual Studio 2015\Projects\TestWebApp\TestWebApp.sln";
			//string path = @"C:\Users\Dave\Documents\Visual Studio 2015\Projects\TestProject\TestProject.sln";
			Solution sol = await workspace.OpenSolutionAsync(path);
			Project pro = await workspace.OpenProjectAsync(@"C:\Users\Dave\Documents\Visual Studio 2015\Projects\TestWebApp\TestWebApp\TestWebApp.csproj");


			ProjectDependencyGraph graph = sol.GetProjectDependencyGraph();
			foreach (Project proj in sol.Projects)
			{
				Console.WriteLine(proj.Name);
			} 
			

		}
	}
}

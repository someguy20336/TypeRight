using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeRight.Workspaces.Parsing;

namespace TypeRight.Build;

public class ScriptGenerationTask : Task
{
    [Required]
    public ITaskItem[] ReferencePath { get; set; }

    [Required]
    public ITaskItem[] Compile { get; set; }

    [Required]
    public string ProjectPath { get; set; }

    public override bool Execute()
    {
        Log.LogMessage(MessageImportance.Normal, "Generating Scripts for {0}", ProjectPath);

        // Format the command line with the minimal info needed for Roslyn to create a workspace.
        var commandLineForProject = string.Format("/reference:{0} {1}",
            ReferencePath.Select(i => i.ItemSpec).ToSingleString(",", "\"", "\""),
            Compile.Select(i => i.ItemSpec).ToSingleString(" ", "\"", "\""));

        // Create the Roslyn workspace.
        string dir = Path.GetDirectoryName(ProjectPath);
        string name = Path.GetFileNameWithoutExtension(ProjectPath);
        using AdhocWorkspace workspace = new();
        var proj = CommandLineProject.CreateProjectInfo(name, LanguageNames.CSharp, commandLineForProject, dir);
        proj = proj.WithParseOptions(proj.ParseOptions.WithDocumentationMode(DocumentationMode.Parse));
        workspace.AddProject(proj);

        ProjectId mainProjId = workspace.CurrentSolution.Projects.First().Id;
        ProjectParser parser = new(workspace, mainProjId);
        ScriptGenEngine engine = new();
        var result = engine.GenerateScripts(new ScriptGenerationParameters()
        {
            ProjectPath = ProjectPath,
            TypeIterator = parser,
            Force = false
        });

        Log.LogMessage(MessageImportance.Normal, "Completed script generation");
        return true;
    }
}

public static class IEnumerableExtension
{
    public static string ToSingleString<T>(this IEnumerable<T> collection, string separator, string leftWrapper, string rightWrapper)
    {
        var stringBuilder = new StringBuilder();

        foreach (var item in collection)
        {
            if (stringBuilder.Length > 0)
            {
                if (!string.IsNullOrEmpty(separator))
                    stringBuilder.Append(separator);
            }

            if (!string.IsNullOrEmpty(leftWrapper))
                stringBuilder.Append(leftWrapper);

            stringBuilder.Append(item.ToString());

            if (!string.IsNullOrEmpty(rightWrapper))
                stringBuilder.Append(rightWrapper);
        }

        return stringBuilder.ToString();
    }
}


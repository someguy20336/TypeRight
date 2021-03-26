using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using System;
using System.IO;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight
{
	/// <summary>
	/// Base class to use for script generation engines
	/// </summary>
	public class ScriptGenEngine
	{
		
		/// <summary>
		/// Generates the scripts
		/// </summary>
		/// <returns>The script generation result</returns>
		public ScriptGenerationResult GenerateScripts(ScriptGenerationParameters parameters)
		{
			string projectPath = parameters.ProjectPath;
			ConfigOptions configOptions = ConfigParser.GetForProject(parameters.ProjectPath);

			if (parameters.TypeIterator == null)
			{
				return new ScriptGenerationResult(false, $"A {typeof(ITypeIterator).Name} was not provided");
			}

			if (configOptions == null || (!configOptions.Enabled && !parameters.Force))
			{
				return new ScriptGenerationResult(false, $"Script generation is disabled in the configuration options.");
			}

			if (string.IsNullOrEmpty(configOptions.ServerObjectsResultFilepath))
			{
				return new ScriptGenerationResult(false, "ResultFilePath is not specified in the configuration options.");
			}

			Uri projUri = new Uri(projectPath);

			Uri resultRelative;
			try
			{
				resultRelative = new Uri(configOptions.ServerObjectsResultFilepath, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException)
			{
				return new ScriptGenerationResult(false, "ResultFilePath is not in a valid format.");
			}

			Uri resultAbsolute = resultRelative.IsAbsoluteUri ? resultRelative : new Uri(projUri, resultRelative);
			FileInfo fi = new FileInfo(resultAbsolute.LocalPath);
			if (!fi.Directory.Exists)
			{
				return new ScriptGenerationResult(false, $"The directory in ResultFilePath of the config file ({fi.Directory.FullName}) does not exist.");
			}

			ProcessorSettings processorSettings = new ProcessorSettings()
			{
				DefaultResultPath = resultAbsolute.LocalPath,
				ProjectPath = projUri.LocalPath,
				NamingStrategy = PropertyNamingStrategy.Create(configOptions.PropNameCasingConverter)
			};

			// At this point we are good
			TypeVisitor visitor = new TypeVisitor(processorSettings);
			parameters.TypeIterator.IterateTypes(visitor);

			ExtractedTypeCollection typeCollection = visitor.TypeCollection;
			IScriptTemplate scriptGen = ScriptTemplateFactory.GetTemplate();

			// Write the object script text
			foreach (var typeGroup in typeCollection.GroupBy(t => t.TargetPath))
			{
				TypeWriteContext scriptContext = new TypeWriteContext(
					typeGroup,
					typeCollection,
					typeGroup.Key
					);
				string scriptText = scriptGen.CreateTypeTemplate().GetText(scriptContext);
				File.WriteAllText(typeGroup.Key, scriptText);
			}

			// Write MVC controllers
			FetchFunctionResolver fetchResolver = new FetchFunctionResolver(projUri, configOptions.ActionConfigurations);

			foreach (MvcControllerInfo controller in typeCollection.GetMvcControllers())
			{
				ControllerContext context = new ControllerContext(
					controller,
					controller.GetControllerResultPath(),
					typeCollection,
					new Uri(resultAbsolute.LocalPath),
					fetchResolver,
					configOptions.BaseUrl,
					configOptions.QueryParams
					);

				string controllerScript = scriptGen.CreateControllerTextTemplate().GetText(context);
				File.WriteAllText(context.OutputPath, controllerScript);
			}

			return new ScriptGenerationResult(true, null);
		}
		
	}
}

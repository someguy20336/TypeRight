using TypeRight.Configuration;
using TypeRight.TypeLocation;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TypeRight.ScriptGeneration
{
	/// <summary>
	/// Base class to use for script generation engines
	/// </summary>
	public class ScriptGenEngine : IScriptGenEngine
	{
		private ITypeIterator _typeIterator;

		/// <summary>
		/// Gets the path of the project we are generating scripts for
		/// </summary>
		public string ProjectPath { get; }

		/// <summary>
		/// Gets the config options
		/// </summary>
		public ConfigOptions ConfigurationOptions { get; set; }


		/// <summary>
		/// Creates a new script generation engine
		/// </summary>
		/// <param name="projPath">The path to the project</param>
		/// <param name="typeIterator">The type iterator</param>
		public ScriptGenEngine(string projPath, ITypeIterator typeIterator)
			: this(projPath, typeIterator, ConfigParser.GetForProject(projPath))
		{
		}

		/// <summary>
		/// Creates a new script generation engine
		/// </summary>
		/// <param name="projPath"></param>
		/// <param name="typeIterator"></param>
		/// <param name="options"></param>
		public ScriptGenEngine(string projPath, ITypeIterator typeIterator, ConfigOptions options)  // Need some sort of options provider?
		{
			ProjectPath = projPath;
			_typeIterator = typeIterator;
			ConfigurationOptions = options;
		}

		/// <summary>
		/// Generates the scripts
		/// </summary>
		/// <returns>The script generation result</returns>
		public IScriptGenerationResult GenerateScripts()
		{

			if (ConfigurationOptions == null || !ConfigurationOptions.Enabled)
			{
				return new ScriptGenerationResult(false, $"Script generation is disabled in the configuration options.");
			}

			if (string.IsNullOrEmpty(ConfigurationOptions.ServerObjectsResultFilepath))
			{
				return new ScriptGenerationResult(false, "ResultFilePath is not specified in the configuration options.");
			}

			Uri projUri = new Uri(ProjectPath);

			Uri resultRelative;
			try
			{
				resultRelative = new Uri(ConfigurationOptions.ServerObjectsResultFilepath, UriKind.RelativeOrAbsolute);
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
				ProjectPath = projUri.LocalPath
			};

			if (!string.IsNullOrEmpty(ConfigurationOptions.MvcActionAttributeName))
			{
				processorSettings.MvcActionFilter = new IsOfTypeFilter(ConfigurationOptions.MvcActionAttributeName);
			}


			// At this point we are good
			TypeVisitor visitor = new TypeVisitor(processorSettings);
			_typeIterator.IterateTypes(visitor);

			ExtractedTypeCollection typeCollection = visitor.TypeCollection;
			IScriptTemplate scriptGen = ScriptTemplateFactory.GetTemplate(ConfigurationOptions.TemplateType);

			// Write the object script text
			foreach (var typeGroup in typeCollection.GroupBy(t => t.TargetPath))
			{
				TypeWriteContext scriptContext = new TypeWriteContext()
				{
					IncludedTypes = typeGroup,
					OutputPath = typeGroup.Key,
					TypeCollection = typeCollection,

					TypeNamespace = ConfigurationOptions.ClassNamespace,
					EnumNamespace = ConfigurationOptions.EnumNamespace,
				};
				string scriptText = scriptGen.CreateTypeTemplate().GetText(scriptContext);
				File.WriteAllText(typeGroup.Key, scriptText);
			}

			// Write MVC controllers
			FetchFunctionResolver fetchResolver = new FetchFunctionResolver(projUri, ConfigurationOptions.ActionConfig);

			foreach (MvcControllerInfo controller in typeCollection.GetMvcControllers())
			{
				string outputPath = GetControllerResultPath(controller);
				ControllerContext context = new ControllerContext()
				{
					OutputPath = outputPath,
					ServerObjectsResultFilepath = new Uri(resultAbsolute.LocalPath),
					TypeCollection = typeCollection,
					ModelBinding = ConfigurationOptions.ModelBindingType,

					// Fetch Function
					FetchFunctionResolver = fetchResolver,

					// Things I don't want to support anymore
					WebMethodNamespace = ConfigurationOptions.WebMethodNamespace,
					TypeNamespace = ConfigurationOptions.ClassNamespace,
					EnumNamespace = ConfigurationOptions.EnumNamespace,
				};

				string controllerScript = scriptGen.CreateControllerTextTemplate().GetText(controller, context);
				File.WriteAllText(outputPath, controllerScript);
			}

			return new ScriptGenerationResult(true, null);
		}


		/// <summary>
		/// Gets the result path for a controller
		/// </summary>
		/// <param name="controller">The controller</param>
		/// <returns>The result path</returns>
		private string GetControllerResultPath(MvcControllerInfo controller)
		{
			FileInfo fileInfo = new FileInfo(controller.FilePath);
			DirectoryInfo controllerDir = fileInfo.Directory;

			// Calculate the result
			string controllerName = controller.Name.Substring(0, controller.Name.Length - "Controller".Length);
			string resultPath = Path.Combine(controllerDir.FullName, $"..\\Scripts\\{controllerName}", controllerName + "Actions.ts");
			return Path.GetFullPath(resultPath);
		}

	}
}

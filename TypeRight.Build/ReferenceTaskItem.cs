using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Build.Utilities;
using Microsoft.Build.Evaluation;
using System.IO;

namespace TypeRight.Build
{
	/// <summary>
	/// Task item used by a reference.  the only difference is that this resolves the hint path
	/// </summary>
	class ReferenceTaskItem : ITaskItem
	{
		private TaskItem _backingTaskItem;

		public string ItemSpec
		{
			get
			{
				return _backingTaskItem.ItemSpec;
			}

			set
			{
				_backingTaskItem.ItemSpec = value;
			}
		}

		public int MetadataCount
		{
			get
			{
				return _backingTaskItem.MetadataCount;
			}
		}

		public ICollection MetadataNames
		{
			get
			{
				return _backingTaskItem.MetadataNames;
			}
		}

		public IDictionary CloneCustomMetadata()
		{
			return _backingTaskItem.CloneCustomMetadata();
		}

		public void CopyMetadataTo(ITaskItem destinationItem)
		{
			_backingTaskItem.CopyMetadataTo(destinationItem);
		}

		public string GetMetadata(string metadataName)
		{
			return _backingTaskItem.GetMetadata(metadataName);
		}

		public void RemoveMetadata(string metadataName)
		{
			_backingTaskItem.RemoveMetadata(metadataName);
		}

		public void SetMetadata(string metadataName, string metadataValue)
		{
			_backingTaskItem.SetMetadata(metadataName, metadataValue);
		}

		/// <summary>
		/// Creates a new reference task item from the project item
		/// </summary>
		/// <param name="projItem">The project item</param>
		public ReferenceTaskItem(ProjectItem projItem)
		{
			string projDir = projItem.Project.DirectoryPath;
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			foreach (ProjectMetadata md in projItem.Metadata)
			{
				string name = md.Name;
				string evaluatedVal = md.EvaluatedValue;

				// Need to eval the hint path to an absolute path
				if (name == "HintPath" && !string.IsNullOrEmpty(evaluatedVal))
				{
					evaluatedVal = EvalHintPath(projDir, evaluatedVal);
				}

				metadata.Add(name, evaluatedVal);
				
			}

			_backingTaskItem = new TaskItem(projItem.EvaluatedInclude, metadata);
		}

		/// <summary>
		/// Evaluates the hint path to an absolute path
		/// </summary>
		/// <param name="projDir">The project directory</param>
		/// <param name="hintPath">The hind path</param>
		/// <returns>The absolute path</returns>
		private string EvalHintPath(string projDir, string hintPath)
		{
			if (Path.IsPathRooted(hintPath))
			{
				return hintPath;
			}
			return (new Uri(Path.Combine(projDir, hintPath))).LocalPath;
		}
	}
}

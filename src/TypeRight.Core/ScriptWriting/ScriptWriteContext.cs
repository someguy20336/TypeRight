﻿using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Base class for writing scripts
	/// </summary>
	public abstract class ScriptWriteContext
	{
		/// <summary>
		/// Gets or sets the output path
		/// </summary>
		public string OutputPath { get; private set; }

		/// <summary>
		/// Gets or sets the type collection
		/// </summary>
		public ExtractedTypeCollection TypeCollection { get; private set; }

		public ScriptWriteContext(
			ExtractedTypeCollection extractedTypes,
			string outputPath
			)
		{
			OutputPath = outputPath;
			TypeCollection = extractedTypes;
		}
	}
}

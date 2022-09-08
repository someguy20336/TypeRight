using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class MiscHelperTests
	{
		[TestMethod]
		public void ImportStatement_MakesRelativePath_UpOneDirectory()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderC\OtherFile.ts";

			ImportStatement statement = new(from, to, true, ImportModuleNameStyle.Extensionless);
			Assert.AreEqual("../FolderC/OtherFile", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_MakesRelativePath_SubDirectory()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new(from, to, true, ImportModuleNameStyle.Extensionless);
			Assert.AreEqual("./FolderC/OtherFile", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_ReplaceWithJs_GetsJsExtension()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new(from, to, true, ImportModuleNameStyle.ReplaceWithJs);
			Assert.AreEqual("./FolderC/OtherFile.js", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_Extensionless_NoExtension_DoesNothing()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile";

			ImportStatement statement = new(from, to, true, ImportModuleNameStyle.ReplaceWithJs);
			Assert.AreEqual("./FolderC/OtherFile", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_ToImportStatement_ListsTypes()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new(from, to, false, ImportModuleNameStyle.Extensionless);
			statement.AddItem("Type1");
			statement.AddItem("Type2");

			Assert.AreEqual($"import {{ Type1, Type2 }} from \"{ statement.FromRelativePath }\";", statement.ToImportStatement());
		}

		[TestMethod]
		public void ImportStatement_Statement_AsteriskNotation()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new(from, to, true, ImportModuleNameStyle.Extensionless);
			statement.AddItem("Type1");
			statement.AddItem("Type2");

			Assert.AreEqual($"import * as OtherFile from \"{ statement.FromRelativePath }\";", statement.ToImportStatement());
		}


	}
}

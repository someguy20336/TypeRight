using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class MiscHelperTests
	{
		[TestMethod]
		public void ImportStatement_MakesRelativePath_UpOneDirectory()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderC\OtherFile.ts";

			ImportStatement statement = new ImportStatement(from, to, true);
			Assert.AreEqual("../FolderC/OtherFile", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_MakesRelativePath_SubDirectory()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new ImportStatement(from, to, true);
			Assert.AreEqual("./FolderC/OtherFile", statement.FromRelativePath);
		}

		[TestMethod]
		public void ImportStatement_ToImportStatement_ListsTypes()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new ImportStatement(from, to, false);
			statement.AddItem("Type1");
			statement.AddItem("Type2");

			Assert.AreEqual($"import {{ Type1, Type2 }} from \"{ statement.FromRelativePath }\";", statement.ToImportStatement());
		}

		[TestMethod]
		public void ImportStatement_Statement_AsteriskNotation()
		{
			string from = @"C:\FolderA\FolderB\File.ts";
			string to = @"C:\FolderA\FolderB\FolderC\OtherFile.ts";

			ImportStatement statement = new ImportStatement(from, to, true);
			statement.AddItem("Type1");
			statement.AddItem("Type2");

			Assert.AreEqual($"import * as OtherFile from \"{ statement.FromRelativePath }\";", statement.ToImportStatement());
		}
	}
}

using TypeRight.TypeLocation;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.Testers
{
	class EnumTester
	{
		private ExtractedEnumType _enumInfo;

		public const string TestNamespace = "DefaultEnumNamespace";

		public EnumTester(ExtractedEnumType enumInfo)
		{
			_enumInfo = enumInfo;
		}

		public EnumTester HasMemberWithName(string name)
		{
			Assert.IsTrue(_enumInfo.Members.Any(mem => mem.Name == name));
			return this;
		}

		public EnumTester MemberHasIntValue(string name, int value)
		{
			Assert.AreEqual(value, _enumInfo.Members.Where(mem => mem.Name == name).First().Value);
			return this;
		}

		public EnumTester MemberDisplayNameIs(string memberName, string dispName)
		{
			Assert.AreEqual(dispName, _enumInfo.Members.Where(mem => mem.Name == memberName).First().DisplayName);
			return this;
		}

		public EnumTester MemberAbbrevNameIs(string memberName, string abbr)
		{
			Assert.AreEqual(abbr, _enumInfo.Members.Where(mem => mem.Name == memberName).First().Abbreviation);
			return this;
		}
	}
}

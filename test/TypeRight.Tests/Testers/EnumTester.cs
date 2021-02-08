using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TypeRight.Tests.Testers
{
	public class EnumTester
	{
		private ExtractedEnumType _enumInfo;

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

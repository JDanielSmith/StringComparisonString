using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StringOrdinalIgnoreCase = JDanielSmith.System.StringComparisonString<JDanielSmith.System.OrdinalIgnoreCase>;

namespace StringComparisonStringUnitTest;

[TestClass]
public class GetHashCodeUnitTest
{
	[TestMethod]
	public void StringOrdinalIgnoreCaseGetHashCode()
	{
		var abc = new StringOrdinalIgnoreCase("abc");
		var ABC = new StringOrdinalIgnoreCase("ABC");
		Assert.AreEqual(abc.GetHashCode(), ABC.GetHashCode());
		Assert.IsTrue(ABC.GetHashCode() == abc.GetHashCode());
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseDictionary()
	{
		var d = new Dictionary<StringOrdinalIgnoreCase, int>() { { "abc", 1 }, { "def", 2 } };
		Assert.IsTrue(d.ContainsKey("ABC"));

		try
		{
			d.Add("DEF", 2);
			Assert.Fail();
		}
		catch (ArgumentException) { }
	}
}

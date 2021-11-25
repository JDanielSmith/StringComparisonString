using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StringOrdinalIgnoreCase = JDanielSmith.System.StringComparisonString<JDanielSmith.System.OrdinalIgnoreCase>;
namespace StringComparisonStringUnitTest;

[TestClass]
public class CtorUnitTest
{
	[TestMethod]
	public void StringCtor()
	{
		var s = new String("abc");
		Assert.IsNotNull(s);

		s = null;
		s = new String(s);
		Assert.IsNotNull(s);
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseCtor()
	{
		var s = new StringOrdinalIgnoreCase("abc");
		Assert.IsNotNull(s);

		s = null;
		s = new StringOrdinalIgnoreCase(s);
		Assert.IsNotNull(s);
	}
}

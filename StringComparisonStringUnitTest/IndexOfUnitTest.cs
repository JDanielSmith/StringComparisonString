﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using JDanielSmith.System;

namespace StringComparisonStringUnitTest;

[TestClass]
public class IndexOfUnitTest
{
	static readonly StringComparisonString<StringComparison.OrdinalIgnoreCase> abcABC = new("abcABC");

	[TestMethod]
	public void TestIndexOf()
	{
		Assert.AreEqual(1, abcABC.IndexOf("B"));
	}

	[TestMethod]
	public void TestLastIndexOf()
	{
		Assert.AreEqual(4, abcABC.LastIndexOf("b"));
	}

	[TestMethod]
	public void TestStartsWith()
	{
		Assert.IsTrue(abcABC.StartsWith("A"));
	}

	[TestMethod]
	public void TestEndsWith()
	{
		Assert.IsTrue(abcABC.EndsWith("c"));
	}
}

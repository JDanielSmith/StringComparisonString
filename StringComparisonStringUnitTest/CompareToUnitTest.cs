using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StringOrdinalIgnoreCase = JDanielSmith.System.StringComparisonString<JDanielSmith.System.OrdinalIgnoreCase>;

namespace StringComparisonStringUnitTest;

[TestClass]
public class CompareToUnitTest
{
	static void CompareTo<T>(int expected, T s1, T s2, Func<T, string> cast) where T : class, IComparable, IComparable<T>, IComparable<string>
	{
		Assert.AreEqual(expected, s1.CompareTo((object)s2));
		Assert.AreEqual(expected, s1.CompareTo(cast(s2)));
		Assert.AreEqual(expected, s1.CompareTo(s2));
	}

	static void CompareTo<T>(T Null, T Empty, T a, T b, Func<T, string> cast) where T : class, IComparable, IComparable<T>, IComparable<string>
	{
		CompareTo(0, Null, Null, cast);
		CompareTo(0, Empty, Null, cast);
		CompareTo(0, Null, Empty, cast);
		CompareTo(0, Empty, Empty, cast);

		CompareTo(0, a, a, cast);
		CompareTo(1, a, Null, cast);
		CompareTo(1, a, Empty, cast);
		CompareTo(-1, Null, a, cast);
		CompareTo(-1, Empty, a, cast);

		CompareTo(0, b, b, cast);
		CompareTo(-1, a, b, cast);
		CompareTo(1, b, a, cast);
	}

	static void CompareTo_Null<T>(T Null, T Empty, T a, Func<T, string> cast) where T : class, IComparable, IComparable<T>, IComparable<string>
	{
		CompareTo(1, Null, null, cast);
		CompareTo(1, Empty, null, cast);
		CompareTo(1, a, null, cast);

		string sNull = null;
		Assert.AreEqual(1, Null.CompareTo(sNull));
		Assert.AreEqual(1, Empty.CompareTo(sNull));
		Assert.AreEqual(1, a.CompareTo(sNull));
	}

	class Foo { };
	static readonly Foo s_foo = new Foo();
	static void CompareTo_ArgumentException<T>(T Null, T Empty) where T : class, IComparable, IComparable<T>
	{
		try
		{
			Assert.AreEqual(1, Null.CompareTo((object)s_foo));
			Assert.Fail();
		}
		catch (ArgumentException) { }

		try
		{
			Assert.AreEqual(1, Empty.CompareTo((object)s_foo));
			Assert.Fail();
		}
		catch (ArgumentException) { }
	}

	static readonly String String_Null = new String((char[])null);
	static readonly String String_Empty = String.Empty;

	[TestMethod]
	public void StringCompareTo()
	{
		CompareTo(String_Null, String_Empty, "a", "b", s => (string)s);
	}

	[TestMethod]
	public void StringCompareTo_Null()
	{
		CompareTo_Null(String_Null, String_Empty, "a", s => (string)s);
	}

	[TestMethod]
	public void StringCompareTo_ArgumentException()
	{
		CompareTo_ArgumentException(String_Null, String_Empty);
	}

	static readonly StringOrdinalIgnoreCase StringOrdinalIgnoreCase_Null = new StringOrdinalIgnoreCase(null);
	static readonly StringOrdinalIgnoreCase StringOrdinalIgnoreCase_Empty = String.Empty;

	[TestMethod]
	public void StringOrdinalIgnoreCaseCompareTo_ArgumentException()
	{
		CompareTo_ArgumentException(StringOrdinalIgnoreCase_Null, StringOrdinalIgnoreCase_Empty);
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseCompareTo_Null()
	{
		CompareTo_Null(StringOrdinalIgnoreCase_Null, StringOrdinalIgnoreCase_Empty, (StringOrdinalIgnoreCase)"a",
			s => (string)s);
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseCompareTo()
	{
		StringOrdinalIgnoreCase a = "a";
		StringOrdinalIgnoreCase b = "b";

		CompareTo(StringOrdinalIgnoreCase_Null, StringOrdinalIgnoreCase_Empty, a, b, s => (string)s);

		StringOrdinalIgnoreCase A = "A";
		CompareTo(0, a, a, s => (string)s);
		CompareTo(0, a, A, s => (string)s);
		CompareTo(0, A, a, s => (string)s);
		CompareTo(0, A, A, s => (string)s);

		Assert.IsTrue(a == A);
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseComparisionOperators()
	{
		StringOrdinalIgnoreCase a = "a";
		StringOrdinalIgnoreCase b = "b";
		StringOrdinalIgnoreCase A = "A";
	
		Assert.IsTrue(a == A);
		Assert.IsTrue(A == a);

		Assert.IsTrue(a < b);
		Assert.IsTrue(A < b);
		Assert.IsTrue(a <= b);
		Assert.IsTrue(A <= b);

		Assert.IsFalse(b < a);
		Assert.IsFalse(b < A);
		Assert.IsFalse(b <= a);
		Assert.IsFalse(b <= A);

		Assert.IsTrue(b > a);
		Assert.IsTrue(b > A);
		Assert.IsTrue(b >= a);
		Assert.IsTrue(b >= A);

		Assert.IsFalse(a > b);
		Assert.IsFalse(A > b);
		Assert.IsFalse(a >= b);
		Assert.IsFalse(A >= b);
	}

	[TestMethod]
	public void StringOrdinalIgnoreCaseComparisionOperatorsNull()
	{
		StringOrdinalIgnoreCase a = "a";
		StringOrdinalIgnoreCase b = null;
		StringOrdinalIgnoreCase A = null;

		Assert.IsFalse(a == A);
		Assert.IsFalse(A == a);

		Assert.IsFalse(a < b);
		Assert.IsFalse(A < b);
		Assert.IsFalse(a <= b);
		Assert.IsTrue(A <= b);

		Assert.IsTrue(b < a);
		Assert.IsFalse(b < A);
		Assert.IsTrue(b <= a);
		Assert.IsTrue(b <= A);

		Assert.IsFalse(b > a);
		Assert.IsFalse(b > A);
		Assert.IsFalse(b >= a);
		Assert.IsTrue(b >= A);

		Assert.IsTrue(a > b);
		Assert.IsFalse(A > b);
		Assert.IsTrue(a >= b);
		Assert.IsTrue(A >= b);
	}
}

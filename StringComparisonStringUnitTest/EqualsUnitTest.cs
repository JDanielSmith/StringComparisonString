using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StringOrdinalIgnoreCase = JDanielSmith.System.StringComparisonString<JDanielSmith.System.OrdinalIgnoreCase>;

namespace CaseInsenstiveStringUnitTest
{
	[TestClass]
	public class EqualsUnitTest
	{
		static void Equals_<T>(T Null, T Empty, bool reallyEmpty) where T : class, IEquatable<T>, IEquatable<string>
		{
			Assert.IsTrue(Null != null);
			Assert.IsTrue(Empty != null);
			Assert.IsTrue(null != Null);
			Assert.IsTrue(null != Empty);

			Assert.IsTrue(Null.Equals(Null));
			Assert.AreEqual(Null, Null);

			Assert.IsTrue(Empty.Equals(Empty));
			Assert.AreEqual(Empty, Empty);

			Assert.IsTrue(Null.Equals(Empty));
			Assert.AreEqual(Null, Empty);

			Assert.IsTrue(Empty.Equals(Null));
			Assert.AreEqual(Empty, Null);

			if (reallyEmpty)
			{
				Assert.IsTrue(Empty.Equals(String.Empty));
				Assert.AreEqual(Empty, String.Empty);

				Assert.IsTrue(Null.Equals(String.Empty));
				Assert.AreEqual(Null, String.Empty);
			}

			Assert.IsFalse(Null.Equals("&"));
			Assert.AreNotEqual(Null, "&");

			Assert.IsFalse(Empty.Equals("&"));
			Assert.AreNotEqual(Empty, "&");
		}
		static void Equals<T>(T Null, T Empty) where T : class, IEquatable<T>, IEquatable<string>
		{
			Equals_(Null, Empty, reallyEmpty: true);
			Equals_("a", "a", reallyEmpty: false);
			Equals_("A", "A", reallyEmpty: false);
		}

		static void Equals_Null<T>(T Null, T Empty) where T : class, IEquatable<T>, IEquatable<string>
		{
			object oNull = null;
			Assert.IsFalse(Null.Equals(oNull));
			Assert.IsFalse(Empty.Equals(oNull));

			string sNull = null;
			Assert.IsFalse(Null.Equals(sNull));
			Assert.IsFalse(Empty.Equals(sNull));

			T tNull = null;
			Assert.IsFalse(Null.Equals(tNull));
			Assert.IsFalse(Empty.Equals(tNull));
		}

		static readonly String String_Null = new String((char[])null);
		static readonly String String_Empty = String.Empty;

		[TestMethod]
		public void StringEquals()
		{
			Equals(String_Null, String_Empty);

			Assert.IsTrue(String_Null == String_Empty);
			Assert.IsTrue(String_Empty == String_Null);
			Assert.IsTrue(String_Empty == String.Empty);
			Assert.IsTrue(String.Empty == String_Empty);
			Assert.IsTrue(String_Null == String.Empty);
			Assert.IsTrue(String.Empty == String_Null);

			Assert.IsTrue(String_Null != null);
			Assert.IsTrue(String_Empty != null);
			Assert.IsTrue(null != String_Empty);
			Assert.IsTrue(null != String_Null);

			Assert.IsFalse(String_Null == "a");
			Assert.IsFalse(String_Empty == "a");
			Assert.IsTrue("a" != String_Empty);
			Assert.IsTrue("a" != String_Null);
		}

		[TestMethod]
		public void StringEquals_Null()
		{
			Equals_Null(String_Null, String_Empty);
		}

		static readonly StringOrdinalIgnoreCase StringOrdinalIgnoreCase_Null = new StringOrdinalIgnoreCase(null);
		static readonly StringOrdinalIgnoreCase StringOrdinalIgnoreCase_Empty = String.Empty;

		[TestMethod]
		public void StringOrdinalIgnoreCaseEquals()
		{
			Equals(StringOrdinalIgnoreCase_Null, StringOrdinalIgnoreCase_Empty);

			StringOrdinalIgnoreCase a = "a";
			StringOrdinalIgnoreCase A = "A";
			Equals_(a, A, reallyEmpty: false);
			Equals_(A, a, reallyEmpty: false);

			Assert.IsTrue(StringOrdinalIgnoreCase_Null == StringOrdinalIgnoreCase_Empty);
			Assert.IsTrue(StringOrdinalIgnoreCase_Empty == StringOrdinalIgnoreCase_Null);
			Assert.IsTrue(StringOrdinalIgnoreCase_Empty == String.Empty);
			Assert.IsTrue(String.Empty == StringOrdinalIgnoreCase_Empty);
			Assert.IsTrue(StringOrdinalIgnoreCase_Null == String.Empty);
			Assert.IsTrue(String.Empty == StringOrdinalIgnoreCase_Null);

			string sNull = null;
			Assert.IsTrue(StringOrdinalIgnoreCase_Null != sNull);
			Assert.IsTrue(StringOrdinalIgnoreCase_Empty != sNull);
			Assert.IsTrue(sNull != StringOrdinalIgnoreCase_Empty);
			Assert.IsTrue(sNull != StringOrdinalIgnoreCase_Null);

			StringOrdinalIgnoreCase soicNull = null;
			Assert.IsTrue(StringOrdinalIgnoreCase_Null != soicNull);
			Assert.IsTrue(StringOrdinalIgnoreCase_Empty != soicNull);
			Assert.IsTrue(soicNull != StringOrdinalIgnoreCase_Empty);
			Assert.IsTrue(soicNull != StringOrdinalIgnoreCase_Null);

			Assert.IsFalse(StringOrdinalIgnoreCase_Null == "a");
			Assert.IsFalse(StringOrdinalIgnoreCase_Empty == "a");
			Assert.IsFalse(soicNull == "a");
			Assert.IsTrue("a" != StringOrdinalIgnoreCase_Empty);
			Assert.IsTrue("a" != StringOrdinalIgnoreCase_Null);
			Assert.IsTrue("a" != soicNull);
		}

		[TestMethod]
		public void StringOrdinalIgnoreCaseEquals_Null()
		{
			Equals_Null(StringOrdinalIgnoreCase_Null, StringOrdinalIgnoreCase_Empty);
		}
	}
}

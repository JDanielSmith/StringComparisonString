using System;
using SystemStringComparer = System.StringComparer;
using SystemStringComparison = System.StringComparison;

using CodeAnalysis = System.Diagnostics.CodeAnalysis;

namespace JDanielSmith.System
{
	/// <summary>
	/// Provide a case-insensitive wrapper around System.String.
	/// 
	/// This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
	/// it can be easy to forget to pass an IEqualityComparer<> in the constructor.
	/// 
	/// Some hints from: http://stackoverflow.com/questions/33039324/how-can-system-string-be-properly-wrapped-for-case-insensitivy
	/// </summary>
	[CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "String")]
	[CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes")]
	public sealed class String<TStringComparison> : IComparable, ICloneable,
		IComparable<String<TStringComparison>>, IEquatable<String<TStringComparison>>,
		IComparable<string>, IEquatable<string>
		where TStringComparison : StringComparison, new()
	{
		// Can't do TStringComparison.Comparer as in C++.  But since the new() constraint was provided
		// create a new instance and extract the information.  This only needs to be done once per type.
		static readonly StringComparison _instance = new TStringComparison();
		static readonly SystemStringComparer _comparer = _instance.Comparer;

		public string Value { get; }

		public String(string value)
		{
			// matching the behavior of System.String is more straight-forward if "Value" is never null
			Value = value ?? string.Empty;
		}

		// easily convert to/from System.String
		public static implicit operator String<TStringComparison>(string value)
		{
			return new String<TStringComparison>(value);
		}
		public static implicit operator string(String<TStringComparison> value)
		{
			return value?.Value;
		}

		#region Equals, IEquatable
		public override bool Equals(object obj)
		{
			if (Object.ReferenceEquals(obj, null))
				return false; // this != null

			var other = obj as String<TStringComparison>;
			if (!Object.ReferenceEquals(other, null))
				return Equals(other); // call Equals(String<TStringComparerAndComparison>)

			var s_other = obj as string;
			if (!Object.ReferenceEquals(s_other, null))
				return Equals(s_other); // call Equals(string)

			return _comparer.Equals(obj);
		}
		public bool Equals(String<TStringComparison> other)
		{
			if (Object.ReferenceEquals(other, null))
				return false; // this != null
			return Equals(other.Value); // call Equals(string)
		}
		public bool Equals(String<TStringComparison> other, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return Equals(other);
		}
		public bool Equals(string other)
		{
			return _comparer.Equals(Value, other);
		}
		public bool Equals(string other, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return _comparer.Equals(Value, other);
		}

		public override int GetHashCode()
		{
			return _comparer.GetHashCode(Value);
		}
		#endregion

		public override string ToString()
		{
			return Value;
		}

		public object Clone()
		{
			return new String<TStringComparison>(Value);
		}

		#region IComparable
		public int CompareTo(object obj)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(obj, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			// obj must be either StringOrdinalIgnoreCase or String
			var other = obj as String<TStringComparison>;
			if (Object.ReferenceEquals(other, null))
			{
				var s_other = obj as string;
				if (Object.ReferenceEquals(s_other, null))
					throw new ArgumentException("Object must be of type " + nameof(String<TStringComparison>) + " or String.");

				return CompareTo(s_other); // call CompareTo(string)
			}

			return CompareTo(other); // call CompareTo(StringOrdinalIgnoreCase)
		}
		public int CompareTo(String<TStringComparison> other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			if (Object.ReferenceEquals(Value, other.Value))
				return 0;

			return CompareTo(other.Value); // call CompareTo(string)
		}
		public int CompareTo(string other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			return _comparer.Compare(Value, other);
		}

		public static bool operator ==(String<TStringComparison> left, String<TStringComparison> right)
		{
			if (Object.ReferenceEquals(left, null))
				return Object.ReferenceEquals(right, null); // null == null, null != something
			return left.Equals(right); // know left != null
		}
		public static bool operator ==(String<TStringComparison> left, string right)
		{
			if (Object.ReferenceEquals(left, null))
				return Object.ReferenceEquals(right, null); // null == null, null != something
			return left.Equals(right); // know x != null
		}
		public static bool operator ==(string left, String<TStringComparison> right)
		{
			return right == left; // == is commutative, left == right
		}

		public static bool operator !=(String<TStringComparison> left, String<TStringComparison> right)
		{
			return !(left == right);
		}
		public static bool operator !=(string left, String<TStringComparison> right)
		{
			return !(left == right);
		}
		public static bool operator !=(String<TStringComparison> left, string right)
		{
			return !(left == right);
		}
		#endregion

		#region IndexOf, LastIndexOf, StartsWith, EndsWith
		static readonly SystemStringComparison _comparisonType = _instance.Comparison;

		public bool EndsWith(string value)
		{
			return Value.EndsWith(value, _comparisonType);
		}
		public bool EndsWith(string value, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return EndsWith(value);
		}

		public int IndexOf(string value)
		{
			return Value.IndexOf(value, _comparisonType);
		}
		public int IndexOf(string value, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return IndexOf(value);
		}

		public int IndexOf(string value, int startIndex)
		{
			return Value.IndexOf(value, startIndex, _comparisonType);
		}
		public int IndexOf(string value, int startIndex, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return IndexOf(value, startIndex);
		}

		public int IndexOf(string value, int startIndex, int count)
		{
			return Value.IndexOf(value, startIndex, count, _comparisonType);
		}
		public int IndexOf(string value, int startIndex, int count, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return IndexOf(value, startIndex, count);
		}

		public int LastIndexOf(string value)
		{
			return Value.LastIndexOf(value, _comparisonType);
		}
		public int LastIndexOf(string value, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return LastIndexOf(value);
		}

		public int LastIndexOf(string value, int startIndex)
		{
			return Value.LastIndexOf(value, startIndex, _comparisonType);
		}
		public int LastIndexOf(string value, int startIndex, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return LastIndexOf(value, startIndex);
		}

		public int LastIndexOf(string value, int startIndex, int count)
		{
			return Value.LastIndexOf(value, startIndex, count, _comparisonType);
		}
		public int LastIndexOf(string value, int startIndex, int count, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));
			return LastIndexOf(value, startIndex, count);
		}
		
		public bool StartsWith(string value)
		{
			return Value.StartsWith(value, _comparisonType);
		}
		public bool StartsWith(string value, SystemStringComparison comparisonType) // for easier use with existing code
		{
			if (comparisonType != _comparisonType)
				throw new ArgumentOutOfRangeException(nameof(comparisonType));

			return StartsWith(value);
		}
		#endregion

	}
}

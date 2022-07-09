using SStringComparison = global::System.StringComparison;

namespace JDanielSmith.System;

public static class StringComparison
{
	/// <summary>
	/// "Convert" System.StringComparison values into types so they can be used as a generic argument
	/// </summary>
	public interface IStringComparison
	{
		static abstract SStringComparison Comparison { get; }
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the current culture.
	/// </summary>
	public struct CurrentCulture : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.CurrentCulture;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the current culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public struct CurrentCultureIgnoreCase : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.CurrentCultureIgnoreCase;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the invariant culture.
	/// </summary>
	public struct InvariantCulture : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.InvariantCulture;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the invariant culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public struct InvariantCultureIgnoreCase : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.InvariantCultureIgnoreCase;
	}

	/// <summary>
	/// Compare strings using ordinal sort rules.
	/// </summary>
	public struct Ordinal : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.Ordinal;
	}

	/// <summary>
	/// Compare strings using ordinal sort rules and ignoring the case of the strings
	/// being compared.
	/// </summary>
	public struct OrdinalIgnoreCase : IStringComparison
	{
		public static SStringComparison Comparison { get; } = SStringComparison.OrdinalIgnoreCase;
	}
}
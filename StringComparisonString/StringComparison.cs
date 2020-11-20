namespace JDanielSmith.System
{
	/// <summary>
	/// "Convert" System.StringComparison values into types so they can be used as a generic argument
	/// </summary>
	public abstract class StringComparison
	{
		public global::System.StringComparison Comparison { get; }
		internal StringComparison(global::System.StringComparison comparison)
		{
			Comparison = comparison;
		}
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the current culture.
	/// </summary>
	public sealed class CurrentCulture : StringComparison
	{
		public CurrentCulture() : base(global::System.StringComparison.CurrentCulture) { }
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the current culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public sealed class CurrentCultureIgnoreCase : StringComparison
	{
		public CurrentCultureIgnoreCase() : base(global::System.StringComparison.CurrentCultureIgnoreCase) { }
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the invariant culture.
	/// </summary>
	public sealed class InvariantCulture : StringComparison
	{
		public InvariantCulture() : base(global::System.StringComparison.InvariantCulture) { }
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the invariant culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public sealed class InvariantCultureIgnoreCase : StringComparison
	{
		public InvariantCultureIgnoreCase() : base(global::System.StringComparison.InvariantCultureIgnoreCase) { }
	}

	/// <summary>
	/// Compare strings using ordinal sort rules.
	/// </summary>
	public sealed class Ordinal : StringComparison
	{
		public Ordinal() : base(global::System.StringComparison.Ordinal) { }
	}

	/// <summary>
	/// Compare strings using ordinal sort rules and ignoring the case of the strings
	/// being compared.
	/// </summary>
	public sealed class OrdinalIgnoreCase : StringComparison
	{
		public OrdinalIgnoreCase() : base(global::System.StringComparison.OrdinalIgnoreCase) { }
	}
}

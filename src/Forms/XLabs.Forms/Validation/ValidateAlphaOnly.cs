using System.Text.RegularExpressions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidateAlphaOnly.
	/// </summary>
	internal class ValidateAlphaOnly : ValidatorPredicate
	{
		#region Static Fields

		/// <summary>
		/// The alpha only
		/// </summary>
		private static readonly Regex AlphaOnly = new Regex(@"^[\p{L}]*$");

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateAlphaOnly"/> class.
		/// </summary>
		public ValidateAlphaOnly() : base(Validators.AlphaOnly,PredicatePriority.Low, IsAlphaOnly) { }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether [is alpha only] [the specified rule].
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is alpha only] [the specified rule]; otherwise, <c>false</c>.</returns>
		private static bool IsAlphaOnly(Rule rule, string value)
		{
			return string.IsNullOrEmpty(value) || AlphaOnly.IsMatch(value);
		}

		#endregion
	}
}
using System.Text.RegularExpressions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidateNumericOnly.
	/// </summary>
	internal class ValidateNumericOnly : ValidatorPredicate
	{
		#region Static Fields

		/// <summary>
		/// The numeric
		/// </summary>
		private static readonly Regex Numeric = new Regex(@"^[\p{N}\.,]*$");

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateNumericOnly"/> class.
		/// </summary>
		public ValidateNumericOnly() : base(Validators.NumericOnly, PredicatePriority.Low, IsAlphaNumeric) { }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether [is alpha numeric] [the specified rule].
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is alpha numeric] [the specified rule]; otherwise, <c>false</c>.</returns>
		private static bool IsAlphaNumeric(Rule rule, string value)
		{
			return string.IsNullOrEmpty(value) || Numeric.IsMatch(value);
		}

		#endregion
	}
}

using System;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class UserValidator.
	/// </summary>
	internal class UserValidator : ValidatorPredicate
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidatorPredicate" /> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="eval">The eval.</param>
		public UserValidator(Validators id, PredicatePriority priority, Func<Rule, string, bool> eval) : base(id, priority, eval) {
		}

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { get; set; }
	}
}
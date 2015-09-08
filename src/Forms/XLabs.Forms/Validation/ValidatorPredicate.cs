using System;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidatorPredicate.
	/// </summary>
	internal class ValidatorPredicate
	{
		#region Fields

		/// <summary>
		/// The _evaluator
		/// </summary>
		private readonly Func<Rule, string, bool> _evaluator;
		/// <summary>
		/// The _id
		/// </summary>
		private readonly Validators _id;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidatorPredicate"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="eval">The eval.</param>
		public ValidatorPredicate(Validators id, PredicatePriority priority, Func<Rule, string, bool> eval)
		{
			_id = id;
			_evaluator = eval;
			Priority = priority;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the predicate.
		/// </summary>
		/// <value>The predicate.</value>
		public Func<Rule, string, bool> Predicate { get { return _evaluator; } }
		/// <summary>
		/// Gets the type of the validator.
		/// </summary>
		/// <value>The type of the validator.</value>
		public Validators ValidatorType { get { return _id; } }
		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		public PredicatePriority Priority { get; private set; }
		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Determines whether the specified identifier is a.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <returns><c>true</c> if the specified identifier is a; otherwise, <c>false</c>.</returns>
		public bool IsA(Validators identifier) { return (identifier & _id) == _id; }

		#endregion
	}
}
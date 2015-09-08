using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms.Validation
{
	/// <summary>
	///     A set of validation elements
	///     When all of the contained Rules are
	///     satisified the RuleSet signals Valid via the
	///     <see cref="IsValid" /> bindable property />
	/// </summary>
	/// Element created at 07/11/2014,3:08 AM by Charles
	public class RuleSet : BindableObject
	{
		#region Static Fields

		/// <summary>Property Defintion for <see cref="Actions" /></summary>
		/// Element created at 07/11/2014,6:12 AM by Charles
		public static BindableProperty ActionsProperty =
			BindableProperty.Create<RuleSet, Actions>(x => x.Actions,
				default(Actions));

		/// <summary>Property Definition for <see cref="IsValid" /></summary>
		/// Element created at 07/11/2014,6:12 AM by Charles
		public static BindableProperty IsValidProperty =
			BindableProperty.Create<RuleSet, bool>(x => x.IsValid,
				default(bool),
				BindingMode.TwoWay);

		/// <summary>Property Definition for <see cref="Rules" /></summary>
		/// Element created at 07/11/2014,6:13 AM by Charles
		public static BindableProperty RulesProperty =
			BindableProperty.Create<RuleSet, Rules>(x => x.Rules,
				default(Rules),
				BindingMode.OneWay,
				null,
				(bo, o, n) => ((RuleSet)bo).RulesChanged(o, n));

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="RuleSet" /> class.
		/// </summary>
		/// Element created at 07/11/2014,6:13 AM by Charles
		public RuleSet()
		{
			Actions = new Actions();
			Rules = new Rules();
			Rules.CollectionChanged += RulesCollectionChanged;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the actions.</summary>
		/// <value>The actions.</value>
		/// Element created at 07/11/2014,6:14 AM by Charles
		public Actions Actions
		{
			get { return (Actions)GetValue(ActionsProperty); }
			set { SetValue(ActionsProperty, value); }
		}

		/// <summary>
		///     Gets or sets a value indicating whether this RuleSet is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		/// Element created at 07/11/2014,6:13 AM by Charles
		public bool IsValid
		{
			get { return (bool)GetValue(IsValidProperty); }
			set { SetValue(IsValidProperty, value); }
		}

		/// <summary>Gets or sets the rules.</summary>
		/// <value>The rules.</value>
		/// Element created at 07/11/2014,6:14 AM by Charles
		public Rules Rules
		{
			get { return (Rules)GetValue(RulesProperty); }
			set { SetValue(RulesProperty, value); }
		}

		#endregion

		#region Methods

		/// <summary>
		///     Checks the state on each property change.
		///     Actions are applied from the least specific
		///     to the most specific:
		///     Generic Validation set actions
		///     Targeted Validation set actions
		///     Actions owned by a rule
		/// </summary>
		/// Element created at 08/11/2014,3:02 AM by Charles
		internal void CheckState()
		{
			// No rules or actions? no need to validate
			if (Actions == null || Actions.Count == 0 || Rules == null || Rules.Count == 0)
			{
				return;
			}

			//Gather up our results
			var results = Rules.Select(x => new
				{
					State = x.IsSatisfied(),
					Source = x.Element,
					Passed = x.LastResult.HasValue && x.LastResult.Value,
					x.Actions
				}).ToList();

			IsValid = results.All(x => x.Passed);

			//Apply generic Validation set actions
			foreach (var a in Actions.Where(x => x.Element == null))
			{
				foreach (var e in Rules)
					a.ApplyResult(e.LastResult.HasValue && e.LastResult.Value, e.Element);
			}

			//Apply Targeted Validation set actions 
			foreach (var a in Actions.Where(a => a.Element != null))
				a.ApplyResult(IsValid);

			//Apply Rule based actions
			foreach (var r in results.Where(x => x.State != RuleResult.ValidationNoChange && x.Actions.Any()))
			{
				foreach (var va in r.Actions)
					va.ApplyResult(r.Passed, va.Element==null ? r.Source : null);
			}

		}

		private void RulesChanged(
			ObservableCollection<Rule> oldvalue,
			ObservableCollection<Rule> newvalue)
		{
			if (oldvalue != null)
			{
				foreach (Rule rule in oldvalue)
				{
					rule.Disconnect();
				}
				oldvalue.CollectionChanged -= RulesCollectionChanged;
			}

			newvalue.CollectionChanged += RulesCollectionChanged;
			foreach (Rule rule in newvalue)
			{
				rule.Connect(this);
			}
		}

		private void RulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action == NotifyCollectionChangedAction.Reset)
			{
				foreach (Rule r in Rules)
				{
					r.Disconnect();
				}
			}
			if (args.Action == NotifyCollectionChangedAction.Add)
			{
				Rules[args.NewStartingIndex].Disconnect();
				Rules[args.NewStartingIndex].Connect(this);
			}
			if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				var r = args.OldItems[0] as Rule;
				if (r != null)
				{
					r.Disconnect();
				}
			}
		}

		#endregion
	}
}
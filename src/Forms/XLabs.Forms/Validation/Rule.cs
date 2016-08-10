// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Rule.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using XLabs.Exceptions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Defines a single validation rule
	/// A validation rule consists of an Element, a property on that element(must be a bindable property)
	/// and a set of validation rules
	/// </summary>
	/// Element created at 07/11/2014,3:06 AM by Charles
	public class Rule : BindableObject
	{
		#region Static Fields

		/// <summary>
		/// Definition for the <see cref="UserDefined" /> property
		/// </summary>
		/// Element created at 08/11/2014,2:54 PM by Charles
		public static BindableProperty UserDefinedProperty =
			BindableProperty.Create<Rule, string>(x => x.UserDefined, default(string));

		/// <summary>
		/// Actions to run when this Validation rule evaluates
		/// </summary>
		/// Element created at 08/11/2014,12:43 PM by Charles
		public static BindableProperty ActionsProperty =
			BindableProperty.Create<Rule, Actions>(x => x.Actions,
				default(Actions));

		/// <summary>
		/// Property definition for <see cref="Callback" />
		/// </summary>
		/// Element created at 07/11/2014,10:48 AM by Charles
		public static BindableProperty CallbackProperty =
			BindableProperty.Create<Rule, Predicate<string>>(x => x.Callback,
				default(Predicate<string>));

		/// <summary>
		/// Property Definition for <see cref="Element" />
		/// </summary>
		/// Element created at 07/11/2014,2:54 AM by Charles
		public static BindableProperty ElementProperty =
			BindableProperty.Create<Rule, BindableObject>(x => x.Element,
				default(BindableObject));

		/// <summary>
		/// The maximum length property
		/// </summary>
		/// Element created at 08/11/2014,2:46 AM by Charles
		public static BindableProperty MaximumLengthProperty =
			BindableProperty.Create<Rule, int>(x => x.MaximumLength, default(int));

		/// <summary>
		/// Property Definition for <see cref="Maximum" />
		/// </summary>
		/// Element created at 07/11/2014,3:02 AM by Charles
		public static BindableProperty MaximumProperty =
			BindableProperty.Create<Rule, double>(x => x.Maximum, default(double));

		/// <summary>
		/// The minimum length property
		/// </summary>
		/// Element created at 07/11/2014,4:00 PM by Charles
		public static BindableProperty MinimumLengthProperty =
			BindableProperty.Create<Rule, int>(x => x.MinimumLength, default(int));

		/// <summary>
		/// Property Definition for <see cref="Minimum" />
		/// </summary>
		/// Element created at 07/11/2014,3:01 AM by Charles
		public static BindableProperty MinimumProperty =
			BindableProperty.Create<Rule, double>(x => x.Minimum, default(double));

		/// <summary>
		/// Property definition for <see cref="Property" />
		/// </summary>
		/// Element created at 07/11/2014,4:45 AM by Charles
		public static BindableProperty PropertyProperty =
			BindableProperty.Create<Rule, string>(x => x.Property, default(string));

		/// <summary>
		/// Property Definition for <see cref="Regex" />
		/// </summary>
		/// Element created at 07/11/2014,3:03 AM by Charles
		public static BindableProperty RegexProperty =
			BindableProperty.Create<Rule, string>(x => x.Regex, default(string));

		/// <summary>
		/// Property definition for <see cref="ResultCallback" />
		/// </summary>
		/// Element created at 07/11/2014,11:49 AM by Charles
		public static BindableProperty ResultCallbackProperty =
			BindableProperty.Create<Rule, Action<BindableObject, string, RuleResult>>(
				x => x.ResultCallback,
				default(Action<BindableObject, string, RuleResult>));

		/// <summary>
		/// Property definition for <see cref="RuleName" />
		/// </summary>
		/// Element created at 07/11/2014,11:49 AM by Charles
		public static BindableProperty RuleNameProperty =
			BindableProperty.Create<Rule, string>(x => x.RuleName, default(string));

		/// <summary>
		/// Property Definition for <see cref="Validators" />
		/// </summary>
		/// Element created at 07/11/2014,2:54 AM by Charles
		public static BindableProperty ValidatorsProperty =
			BindableProperty.Create<Rule, Validators>(x => x.Validators,
				default(Validators));

		/// <summary>
		/// The user validator predicates
		/// </summary>
		private static readonly List<UserValidator> UserValidatorPredicates = new List<UserValidator>();

		/// <summary>
		/// The available predicates
		/// </summary>
		private static readonly ValidatorPredicate[] AvailablePredicates =
			{
				new ValidatorPredicate(Validators.Required,PredicatePriority.High,  (rule, val) => !string.IsNullOrEmpty(val)),
				new ValidatorPredicate(Validators.GreaterThan,PredicatePriority.Medium, 
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							double d;
							if (double.TryParse(val, out d))
							{
								return rule.Minimum <= d;
							}
							return false;
						}),
				new ValidatorPredicate(Validators.LessThan,PredicatePriority.Medium,
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							double d;
							if (double.TryParse(val, out d))
							{
								return rule.Maximum >= d;
							}
							return false;
						}),
				new ValidateEmailAddress(),
				new ValidatorPredicate(Validators.Pattern,PredicatePriority.User,
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							try
							{
								var regex = new Regex(rule.Regex);
								return regex.IsMatch(val);
							}
							catch (Exception)
							{
								return false;
							}
						}),
				new ValidatorPredicate(Validators.Between,PredicatePriority.Medium,
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							double value;
							return double.TryParse(val, out value) 
									&& value >= rule.Minimum
									&& value <= rule.Maximum;
						}),
				new ValidatorPredicate(Validators.Predicate,PredicatePriority.User,
					(rule, val) => string.IsNullOrEmpty(val) || (rule.Callback == null || rule.Callback(val))),
				new ValidateDateTime(),
				new ValidatorPredicate(Validators.Numeric,PredicatePriority.Medium,
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							DateTime d;
							return DateTime.TryParse(val, out d);
						}),
				new ValidatorPredicate(Validators.Integer,PredicatePriority.Medium,
					(rule, val) =>
						{
							if (string.IsNullOrEmpty(val)) return true;
							long i;
							return long.TryParse(val, out i);
						}),
				new ValidatorPredicate(Validators.MinLength,PredicatePriority.Medium,
					(rule, val) => string.IsNullOrEmpty(val) || val.Length >= rule.MinimumLength),
				new ValidatorPredicate(Validators.MaxLength,PredicatePriority.Medium,
					(rule, val) => string.IsNullOrEmpty(val) || val.Length <= rule.MaximumLength),
				new ValidateAlphaOnly(),
				new ValidateAlphaNumeric() ,
				new ValidateNumericOnly()
			};

		#endregion

		#region Fields

		/// <summary>
		/// The _predicates
		/// </summary>
		private List<Func<Rule, string, bool>> _predicates =
			new List<Func<Rule, string, bool>>();
		/// <summary>
		/// The _pi
		/// </summary>
		private PropertyInfo _pi;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Rule" /> class.
		/// </summary>
		/// Element created at 08/11/2014,1:11 PM by Charles
		public Rule()
		{
			Actions=new Actions();
		}
		#region Public Properties

		/// <summary>
		/// Gets or sets the user defined validators to use.
		/// If there is more than one use a comma separated list
		/// </summary>
		/// <value>The user defined validators.</value>
		/// Element created at 08/11/2014,2:55 PM by Charles
		public string UserDefined
		{
			get { return (string)GetValue(UserDefinedProperty); }
			set { SetValue(UserDefinedProperty,value);}
		}
		/// <summary>
		/// Gets or sets the actions to run when this rule is evaluated.
		/// </summary>
		/// <value>The actions.</value>
		/// Element created at 08/11/2014,12:44 PM by Charles
		public Actions Actions
		{
			get { return (Actions)GetValue(ActionsProperty); }
			set { SetValue(ActionsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the user predicate.
		/// </summary>
		/// <value>The predicate.</value>
		/// Element created at 07/11/2014,10:48 AM by Charles
		public Predicate<string> Callback
		{
			get { return (Predicate<string>)GetValue(CallbackProperty); }
			set { SetValue(CallbackProperty, value); }
		}

		/// <summary>
		/// Gets or sets the element the element to be validated.
		/// The validated element must be a bindable object
		/// </summary>
		/// <value>The element.</value>
		/// Element created at 07/11/2014,2:54 AM by Charles
		public BindableObject Element
		{
			get { return (BindableObject)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum value for numeric comparsions.</value>
		/// Element created at 07/11/2014,6:18 AM by Charles
		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum string length.
		/// </summary>
		/// <value>The maximum length.</value>
		/// Element created at 07/11/2014,4:01 PM by Charles
		public int MaximumLength
		{
			get { return (int)GetValue(MaximumLengthProperty); }
			set { SetValue(MaximumLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum value for numeric comparsions.</value>
		/// Element created at 07/11/2014,6:18 AM by Charles
		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Gets or sets the minimum string length.
		/// </summary>
		/// <value>The minimum length.</value>
		/// Element created at 07/11/2014,4:01 PM by Charles
		public int MinimumLength
		{
			get { return (int)GetValue(MinimumLengthProperty); }
			set { SetValue(MinimumLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the property.
		/// </summary>
		/// <value>The property whose value is being validated.</value>
		/// Element created at 07/11/2014,6:19 AM by Charles
		public string Property
		{
			get { return (string)GetValue(PropertyProperty); }
			set { SetValue(PropertyProperty, value); }
		}

		/// <summary>
		/// Gets or sets the regular expression to match against
		/// </summary>
		/// <value>The regular expression <see cref="Regex" /></value>
		/// Element created at 07/11/2014,3:04 AM by Charles
		public string Regex
		{
			get { return (string)GetValue(RegexProperty); }
			set { SetValue(RegexProperty, value); }
		}

		/// <summary>
		/// Gets or sets the result callback.
		/// The result callback, if present, is called everytime this rule is evaluated
		/// </summary>
		/// <value>The result callback.</value>
		/// Element created at 07/11/2014,11:50 AM by Charles
		public Action<BindableObject, string, RuleResult> ResultCallback
		{
			get { return (Action<BindableObject, string, RuleResult>)GetValue(ResultCallbackProperty); }
			set { SetValue(ResultCallbackProperty, value); }
		}

		/// <summary>
		/// Gets or sets the name of the rule, this value is passed into the ResultCallback.
		/// </summary>
		/// <value>The name of the rule.</value>
		/// Element created at 07/11/2014,11:50 AM by Charles
		public string RuleName
		{
			get { return (string)GetValue(RuleNameProperty); }
			set { SetValue(RuleNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the set of validators to exectue.
		/// </summary>
		/// <value><see cref="Validators" /></value>
		/// Element created at 07/11/2014,2:55 AM by Charles
		public Validators Validators
		{
			get { return (Validators)GetValue(ValidatorsProperty); }
			set { SetValue(ValidatorsProperty, value); }
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the property information.
		/// </summary>
		/// <value>The property information.</value>
		/// <exception cref="PropertyNotFoundException"></exception>
		/// <exception cref="PropertyNotFoundException">Thrown if the specified property cannot be found</exception>
		/// Element created at 07/11/2014,12:01 PM by Charles
		protected virtual PropertyInfo PropertyInfo
		{
			get
			{
				if (_pi == null)
				{
					Type type = Element.GetType();
					List<PropertyInfo> allprops = type.GetRuntimeProperties().ToList();
					_pi =
						allprops.FirstOrDefault(
							x =>
								string.Compare(x.Name, Property, StringComparison.OrdinalIgnoreCase)
								== 0);
					if (_pi == null)
					{
						throw new PropertyNotFoundException(type,
							Property,
							allprops.Select(x => x.Name));
					}
				}
				return _pi;
			}
		}

		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value>The host.</value>
		private RuleSet Host { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [last result].
		/// </summary>
		/// <value><c>null</c> if [last result] contains no value, <c>true</c> if [last result]; otherwise, <c>false</c>.</value>
		internal bool? LastResult { get; set; }
		#endregion

		#region Methods

		/// <summary>
		/// Adds a user supplied validator to the list of availble validators.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="predicate">The predicate.</param>
		/// Element created at 08/11/2014,2:47 PM by Charles
		public static void AddValidator(string name, Func<Rule, string, bool> predicate)
		{
			var v = new UserValidator(Validators.UserSupplied,
				PredicatePriority.User,
				predicate){UserName = name};
			UserValidatorPredicates.RemoveAll(x => string.Compare(x.UserName, name, StringComparison.OrdinalIgnoreCase) == 0);
			UserValidatorPredicates.Add(v);
		}

		/// <summary>
		/// Connects this Rule to it's property.
		/// </summary>
		/// <param name="vs">The parent <see cref="RuleSet" />.</param>
		/// Element created at 07/11/2014,6:19 AM by Charles
		internal void Connect(RuleSet vs)
		{
			Host = vs;
			UserDefined = UserDefined ?? string.Empty;
			_predicates =
				AvailablePredicates.Where(x => x.IsA(Validators))
								   .Union(
									   UserValidatorPredicates.Where(
										   x => UserDefined.ToLower().Contains(x.UserName.ToLower())))
								   .OrderBy(y => y.Priority)
								   .Select(z => z.Predicate).ToList();
			Element.PropertyChanged += ElementPropertyChanged;
		}

		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		internal void Disconnect()
		{
			_predicates.Clear();
			Element.PropertyChanged -= ElementPropertyChanged;
		}

		/// <summary>
		/// Determines whether this instance is satisfied.
		/// </summary>
		internal RuleResult IsSatisfied()
		{
			var val = PropertyInfo.GetValue(Element) ?? string.Empty;
			var thisresult = _predicates.All(x => x(this, val.ToString()));
			RuleResult result;
			if (LastResult.HasValue)
			{
				result = LastResult.Value == thisresult
					? RuleResult.ValidationNoChange
					: thisresult
						? RuleResult.ValidationSuccess
						: RuleResult.ValidationFailure;                
			}
			else
			{
				result = thisresult
					? RuleResult.ValidationSuccess
					: RuleResult.ValidationFailure;                
			}
			LastResult = thisresult;
			if (ResultCallback != null)
			{
				ResultCallback(Element, RuleName, result);
			}

			return result;
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Property && Host != null)
			{
				Host.CheckState();
			}
		}

		#endregion
	}
}
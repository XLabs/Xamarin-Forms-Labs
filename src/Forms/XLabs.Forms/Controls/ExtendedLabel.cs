using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedLabel.
	/// </summary>
	public class ExtendedLabel : Label
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedLabel"/> class.
		/// </summary>
		public ExtendedLabel()
		{
		}

		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsUnderlineProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsUnderline, false);

		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsUnderline
		{
			get
			{
				return (bool)GetValue(IsUnderlineProperty);
			}
			set
			{
				SetValue(IsUnderlineProperty, value);
			}
		}

		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsStrikeThroughProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsStrikeThrough, false);

		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsStrikeThrough
		{
			get
			{
				return (bool)GetValue(IsStrikeThroughProperty);
			}
			set
			{
				SetValue(IsStrikeThroughProperty, value);
			}
		}

		/// <summary>
		/// This is the drop shadow property
		/// </summary>
		public static readonly BindableProperty IsDropShadowProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsDropShadow, false);

		public bool IsDropShadow 
		{
			get 
			{
				return (bool)GetValue (IsDropShadowProperty);
			}
			set 
			{
				SetValue (IsDropShadowProperty, value);
			}
		}

		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty = 
			BindableProperty.Create<ExtendedLabel, string>(p => p.Placeholder, default(string));

		/// <summary>
		/// Gets or sets the string value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder string.</value>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set 
			{
				SetValue(FormattedPlaceholderProperty, null);
				SetValue(PlaceholderProperty, value); 
			}
		}

		/// <summary>
		/// The formatted placeholder property.
		/// </summary>
		public static readonly BindableProperty FormattedPlaceholderProperty = 
			BindableProperty.Create<ExtendedLabel, FormattedString>(p => p.FormattedPlaceholder, default(FormattedString));

		/// <summary>
		/// Gets or sets the FormattedString value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder FormattedString.</value>
		public FormattedString FormattedPlaceholder
		{
			get { return (FormattedString)GetValue(FormattedPlaceholderProperty); }
			set 
			{  
				SetValue(PlaceholderProperty, null);
				SetValue(FormattedPlaceholderProperty, value);
			}
		}

	}
}
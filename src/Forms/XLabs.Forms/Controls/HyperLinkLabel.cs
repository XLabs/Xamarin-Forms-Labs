using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class HyperLinkLabel.
	/// </summary>
	public class HyperLinkLabel : Label
	{
		/// <summary>
		/// The subject property
		/// </summary>
		public static readonly BindableProperty SubjectProperty;
		/// <summary>
		/// The navigate URI property
		/// </summary>
		public static readonly BindableProperty NavigateUriProperty;

		/// <summary>
		/// Initializes static members of the <see cref="HyperLinkLabel"/> class.
		/// </summary>
		static HyperLinkLabel()
		{
			SubjectProperty = BindableProperty.Create("Subject", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);
			NavigateUriProperty = BindableProperty.Create("NavigateUri", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);          
		}
		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject
		{
			get { return (string)base.GetValue(SubjectProperty); }
			set { base.SetValue(SubjectProperty, value); }
		}

		/// <summary>
		/// Gets or sets the navigate URI.
		/// </summary>
		/// <value>The navigate URI.</value>
		public string NavigateUri
		{
			get { return (string)base.GetValue(NavigateUriProperty); }
			set { base.SetValue(NavigateUriProperty, value); }
		}
	}
}

namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Forms.Validation;

	/// <summary>
	/// Class ValidatorSample.
	/// </summary>
	public partial class ValidatorSample : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidatorSample"/> class.
		/// </summary>
		public ValidatorSample()
		{
			//// User defined validators must be added before
			//// the xaml is parsed
			Rule.AddValidator("EndInCom", MustEndInCom);
			InitializeComponent();
		}

		/// <summary>
		/// Musts the end in COM.
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="val">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool MustEndInCom(Rule rule, string val)
		{
			return string.IsNullOrEmpty(val) || val.EndsWith("com");
		}
	}
}

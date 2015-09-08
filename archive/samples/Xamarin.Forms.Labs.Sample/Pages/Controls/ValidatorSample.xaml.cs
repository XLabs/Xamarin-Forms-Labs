namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    using Xamarin.Forms.Labs.Behaviors;
    using Xamarin.Forms.Labs.Validation;

    public partial class ValidatorSample : ContentPage
    {
        public ValidatorSample()
        {
            //// User defined validators must be added before
            //// the xaml is parsed
            Rule.AddValidator("EndInCom", MustEndInCom);
            InitializeComponent();
        }

        private bool MustEndInCom(Rule rule, string val)
        {
            return string.IsNullOrEmpty(val) || val.EndsWith("com");
        }
    }
}

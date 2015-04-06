namespace XLabs.Sample.Pages.Controls
{
    using System;
    using Data;
    using Xamarin.Forms;

    using XLabs.Forms.Controls;

    /// <summary>
    /// Class ButtonPage.
    /// </summary>
    public partial class ButtonPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPage"/> class.
        /// </summary>
        public ButtonPage()
        {
            InitializeComponent();

            TwitterButton.Clicked += ButtonClick;
            FacebookButton.Clicked += ButtonClick;
            //Showing custom font in image button
            FacebookButton.Font = Font.OfSize("Open 24 Display St", 20);
            GoogleButton.Clicked += ButtonClick;
            MicrosoftButton.Clicked += ButtonClick;

            this.BindingContext = new ButtonPageViewModel();
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ButtonClick(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            this.DisplayAlert("Button Pressed", string.Format("The {0} button was pressed.", button.Text), "OK",
                "Cancel");
        }
    }

    public class ButtonPageViewModel : ObservableObject
    {
        private bool buttonEnabled;

        public bool ButtonEnabled
        {
            get
            {
                return this.buttonEnabled;
            }
            set
            {
                if (this.SetProperty(ref this.buttonEnabled, value))
                {
                    this.NotifyPropertyChanged("EnabledButtonTitle");
                }
            }
        }

        public string EnabledButtonTitle
        {
            get
            {
                return this.buttonEnabled ? "Enabled Image" : "Disabled image";
            }
        }
    }
}

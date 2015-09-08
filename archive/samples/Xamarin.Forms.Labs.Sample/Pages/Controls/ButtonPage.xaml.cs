using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class ButtonPage : ContentPage
    {
        public ButtonPage()
        {
            InitializeComponent();

            TwitterButton.Clicked += Button_Click;
            FacebookButton.Clicked += Button_Click;
            //Showing custom font in image button
            FacebookButton.Font = Font.OfSize("Open 24 Display St", 20);
            GoogleButton.Clicked += Button_Click;
            MicrosoftButton.Clicked += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            this.DisplayAlert("Button Pressed", string.Format("The {0} button was pressed.", button.Text), "OK",
                "Cancel");
        }
    }
}

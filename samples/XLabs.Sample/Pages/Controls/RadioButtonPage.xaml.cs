using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Sample.Pages.Controls
{
    public partial class RadioButtonPage
    {
        public RadioButtonPage()
        {
            InitializeComponent();

            BackgroundColor = Color.Black;

            ansPicker.ItemsSource = new[]
            {
                "Red",
                "Blue",
                "Green",
                "Yellow",
                "Orange"
            };

            ansPicker.CheckedChanged += ansPicker_CheckedChanged;
        }

        private void ansPicker_CheckedChanged(object sender, int e)
        {
            var radio = sender as CustomRadioButton;

            if (radio == null || radio.Id == -1)
            {
                return;
            }

            DisplayAlert("Info", radio.Text, "OK");
        }
    }

}

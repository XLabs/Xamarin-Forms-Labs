namespace XLabs.Sample.Pages.Controls
{
	using System;
	using System.Linq;

	using Xamarin.Forms;

	public partial class CheckBoxPage : ContentPage
    {    
        public CheckBoxPage ()
        {
            InitializeComponent ();

            ListView.ItemsSource = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().Select(c => c.ToString());
        }
    }
}


namespace XLabs.Sample.Pages.Controls
{
	using System;

	using Xamarin.Forms;

	using XLabs.Forms.Controls;

	public partial class PopupPage
    {
        public PopupPage()
        {
            InitializeComponent();

            this.OpenButton.Clicked += OpenButtonClicked;
        }

        void OpenButtonClicked(object sender, EventArgs e)
        {
            var popupLayout = this.Content as PopupLayout;

            if (popupLayout.IsPopupActive)
            {
                popupLayout.DismissPopup();
            }
            else
            {
                var list = new ListView()
                {
                    BackgroundColor = Color.White,
                    ItemsSource = new[] { "1", "2", "3" },
                    HeightRequest = this.Height * .5,
                    WidthRequest = this.Width * .8
                };

                list.ItemSelected += (s, args) => 
                    popupLayout.DismissPopup();

                popupLayout.ShowPopup(list);
            }
        }
    }
}

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class ImageGalleryPage : ContentPage
    {
        public ImageGalleryPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        protected override void OnAppearing()
        {
           (BindingContext as MainViewModel).AddImages();

            base.OnAppearing();
        }
    }
}


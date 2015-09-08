namespace XLabs.Sample.Pages.Mvvm
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using ViewModel;
    using Xamarin.Forms;

    public partial class CanvasWebHybrid
    {
        private bool loaded;

        public CanvasWebHybrid ()
        {
            InitializeComponent ();

            this.NativeList.HeightRequest = Device.OnPlatform(250, 320, 150);
            this.hybridWebView.HeightRequest = Device.OnPlatform(300, 300, 400);

            this.hybridWebView.RegisterCallback("dataCallback", t =>
                Debug.WriteLine(t)
            );

            this.hybridWebView.RegisterCallback("chartUpdated", t =>
                Debug.WriteLine(t)
            );

            var model = ChartViewModel.Dummy;


            this.BindingContext = model;

            model.PropertyChanged += HandlePropertyChanged;

            model.DataPoints.CollectionChanged += HandleCollectionChanged;

            foreach (var datapoint in model.DataPoints)
            {
                datapoint.PropertyChanged += HandlePropertyChanged;
            }

            this.hybridWebView.LoadFinished += (s, e) =>
            {
                this.loaded = true;
                this.hybridWebView.CallJsFunction ("onViewModelData", this.BindingContext);
            };

            this.hybridWebView.LeftSwipe += (s, e) =>
                Debug.WriteLine("Left swipe from HybridWebView");

            this.hybridWebView.RightSwipe += (s, e) =>
                Debug.WriteLine("Right swipe from HybridWebView");
        }

        void HandleCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var datapoint in e.NewItems.OfType<DataPoint>())
            {
                datapoint.PropertyChanged += HandlePropertyChanged;
            }

            foreach (var datapoint in e.OldItems.OfType<DataPoint>())
            {
                datapoint.PropertyChanged -= HandlePropertyChanged;
            }
        }

        void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.loaded)
            {
                this.hybridWebView.CallJsFunction("onViewModelData", this.BindingContext);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.hybridWebView.LoadFromContent("HTML/home.html");
        }
    }
}


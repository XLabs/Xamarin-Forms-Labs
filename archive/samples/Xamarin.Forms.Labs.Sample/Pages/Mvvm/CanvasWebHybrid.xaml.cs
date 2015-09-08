using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample
{	
    public partial class CanvasWebHybrid : BaseView
    {	
        public CanvasWebHybrid ()
        {
            InitializeComponent ();

            this.hybridWebView.RegisterCallback("dataCallback", t =>
                System.Diagnostics.Debug.WriteLine(t)
            );

            this.hybridWebView.RegisterCallback("chartUpdated", t =>
                System.Diagnostics.Debug.WriteLine(t)
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
                this.hybridWebView.CallJsFunction ("onViewModelData", this.BindingContext);
            };

            this.hybridWebView.LeftSwipe += (s, e) =>
                System.Diagnostics.Debug.WriteLine("Left swipe from HybridWebView");

            this.hybridWebView.RightSwipe += (s, e) =>
                System.Diagnostics.Debug.WriteLine("Right swipe from HybridWebView");
        }

        void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.hybridWebView.CallJsFunction ("onViewModelData", this.BindingContext);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.hybridWebView.LoadFromContent("HTML/home.html");
        }
    }
}


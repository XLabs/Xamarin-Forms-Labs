using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Mvvm;
using System.Linq;

namespace XForms.Toolkit.Sample
{	
    public partial class CanvasWebHybrid : BaseView
	{	
		public CanvasWebHybrid ()
		{
			InitializeComponent ();

            this.hybridWebView.RegisterCallback("dataCallback", t =>
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

            this.hybridWebView.Uri = new Uri ("https://raw.githubusercontent.com/sami1971/SimplyMobile/master/iOS/Tests/WebClientTests/Content/home.html");
		}

        void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
//            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
//            {
                foreach (var datapoint in e.NewItems.OfType<DataPoint>())
                {
                    datapoint.PropertyChanged += HandlePropertyChanged;
                }
//            } 
//            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
//            {
                foreach (var datapoint in e.OldItems.OfType<DataPoint>())
                {
                    datapoint.PropertyChanged -= HandlePropertyChanged;
                }
//            }
            
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.hybridWebView.CallJsFunction ("onViewModelData", this.BindingContext);
        }
	}
}


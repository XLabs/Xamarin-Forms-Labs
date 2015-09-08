using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms.Labs.Data;

namespace Xamarin.Forms.Labs.Sample
{
    public class ChartViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
    {
        public ChartViewModel()
        {
            this.DataPoints = new ObservableCollection<DataPoint>();
        }

        public static ChartViewModel Dummy
        {
            get
            {
                var model = new ChartViewModel()
                {
                    Title = "Dummy model"
                };

                model.DataPoints.Add(new DataPoint() { Label = "Banana", Y = 18, Max = 100 });
                model.DataPoints.Add(new DataPoint() { Label = "Orange", Y = 29, Max = 100 });
                model.DataPoints.Add(new DataPoint() { Label = "Apple", Y = 40, Max = 100 });

                return model;
            }
        }

        string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                this.SetProperty(ref title, value);
            }
        }

        ObservableCollection<DataPoint> dataPoints;
        public ObservableCollection<DataPoint> DataPoints
        {
            get
            {
                return dataPoints;
            }
            set
            {
                dataPoints = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    public class DataPoint : ObservableObject
    {
        private string label;
        private double y;
        private double maximum = 100;

        public string Label 
        {
            get { return this.label; }
            set { this.SetProperty (ref label, value); }
        }

        public double Y 
        {
            get { return this.y; }
            set { this.SetProperty (ref y, value); }
        }

        public double Max
        {
            get { return this.maximum; }
            set { this.SetProperty (ref this.maximum, value); }
        }

        public override string ToString()
        {
            return string.Format("Label: {0}, Y: {1}", this.Label, this.Y);
        }
    }
}

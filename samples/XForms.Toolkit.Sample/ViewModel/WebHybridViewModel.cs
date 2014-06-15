using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Mvvm;
using System.Collections.ObjectModel;

namespace XForms.Toolkit.Sample
{
    public class ChartViewModel : ViewModel
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
                this.ChangeAndNotify(ref title, value);
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
                this.NotifyPropertyChanged ();
            }
        }
    }

    public class DataPoint : ViewModel
    {
        private string label;
        private double y;
        private double maximum = 100;

        public string Label 
        {
            get { return this.label; }
            set { this.ChangeAndNotify (ref label, value); }
        }

        public double Y 
        {
            get { return this.y; }
            set { this.ChangeAndNotify (ref y, value); }
        }

        public double Max
        {
            get { return this.maximum; }
            set { this.ChangeAndNotify (ref this.maximum, value); }
        }

        public override string ToString()
        {
            return string.Format("Label: {0}, Y: {1}", this.Label, this.Y);
        }
    }
}

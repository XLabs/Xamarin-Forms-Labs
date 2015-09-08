namespace XLabs.Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using XLabs.Data;

    [DataContract]
    /// <summary>
    /// Class ChartViewModel.
    /// </summary>
    public class ChartViewModel : Forms.Mvvm.ViewModel
    {
        /// <summary>
        /// The data points
        /// </summary>
        ObservableCollection<DataPoint> _dataPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartViewModel"/> class.
        /// </summary>
        public ChartViewModel()
        {
            DataPoints = new ObservableCollection<DataPoint>();
        }

        /// <summary>
        /// Gets the dummy.
        /// </summary>
        /// <value>The dummy.</value>
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

        /// <summary>
        /// The title
        /// </summary>
        string _title;

        [DataMember(Name = "title")]
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        [DataMember(Name = "dataPoints")]
        /// <summary>
        /// Gets or sets the data points.
        /// </summary>
        /// <value>The data points.</value>
        public ObservableCollection<DataPoint> DataPoints
        {
            get
            {
                return _dataPoints;
            }
            set
            {
                _dataPoints = value;
                NotifyPropertyChanged();
            }
        }
    }

    [DataContract]
    /// <summary>
    /// Class DataPoint.
    /// </summary>
    public class DataPoint : ObservableObject
    {
        /// <summary>
        /// The _label
        /// </summary>
        private string _label;
        /// <summary>
        /// The _y
        /// </summary>
        private double _y;
        /// <summary>
        /// The _maximum
        /// </summary>
        private double _maximum = 100;

        [DataMember(Name = "label")]
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label 
        {
            get { return _label; }
            set { SetProperty (ref _label, value); }
        }

        [DataMember(Name = "y")]
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y 
        {
            get { return _y; }
            set { SetProperty (ref _y, value); }
        }

        [DataMember(Name = "max")]
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Max
        {
            get { return _maximum; }
            set { SetProperty (ref _maximum, value); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Label: {0}, Y: {1}", Label, Y);
        }
    }
}

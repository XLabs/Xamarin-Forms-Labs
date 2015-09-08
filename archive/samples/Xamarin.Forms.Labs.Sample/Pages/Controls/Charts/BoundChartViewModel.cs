using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Labs.Charting.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.Charts
{
    public class BoundChartViewModel : INotifyPropertyChanged
    {
        #region commands
        public ICommand ChangeColorYellowCommand { get; set; }
        public ICommand ChangeColorGreenCommand { get; set; }
        public ICommand ChangeColorWhiteCommand { get; set; }
        public ICommand ChangeSourceCommand { get; set; }
        #endregion

        public BoundChartViewModel()
        {
            #region commands
            ChangeColorYellowCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.Yellow;
            },
            (color) =>
            {
                return Color != Color.Yellow;
            });
            ChangeColorGreenCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.Green;
            },
            (color) =>
            {
                return Color != Color.Green;
            });
            ChangeColorWhiteCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.White;
            },
            (color) =>
            {
                return Color != Color.White;
            });

            ChangeSourceCommand = new Command(() =>
            {
                this.ChartData = getChartData();
            },
            () =>
            {
                return true;
            });
            #endregion

            Color = Color.Yellow;

            this.ChartData = getChartData();

        }

        private List<List<Tuple<string, double>>> getChartData()
        {
            Random rnd = new Random();

            var seriesValues1 = new List<Tuple<string, double>>()
            {
                new Tuple<string, double>("Jan",   rnd.Next(0,100)),
                new Tuple<string, double>("Feb",   rnd.Next(0,100)),
                new Tuple<string, double>("March", rnd.Next(0,100))
            };
            var seriesValues2 = new List<Tuple<string, double>>()
            {
                new Tuple<string, double>("Jan",   rnd.Next(0,100)),
                new Tuple<string, double>("Feb",   rnd.Next(0,100)),
                new Tuple<string, double>("March", rnd.Next(0,100))
            };
            var seriesValues3 = new List<Tuple<string, double>>()
            {
                new Tuple<string, double>("Jan",   rnd.Next(0,100)),
                new Tuple<string, double>("Feb",   rnd.Next(0,100)),
                new Tuple<string, double>("March", rnd.Next(0,100))
            };

            return new List<List<Tuple<string, double>>>()
            {
                seriesValues1, seriesValues2, seriesValues3
            };
        }

        private Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    ((Command)this.ChangeColorYellowCommand).ChangeCanExecute();
                    ((Command)this.ChangeColorGreenCommand).ChangeCanExecute();
                    ((Command)this.ChangeColorWhiteCommand).ChangeCanExecute();
                    OnPropertyChanged("Color");
                }
            }
        }

        private List<List<Tuple<string, double>>> _chartData;
        public List<List<Tuple<string, double>>> ChartData
        {
            get
            {
                return _chartData;
            }
            set
            {
                _chartData = value;
                OnPropertyChanged("ChartData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }
    }
}

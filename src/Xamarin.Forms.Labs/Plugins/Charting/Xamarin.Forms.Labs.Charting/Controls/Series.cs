using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class Series : Element
    {
        public static readonly BindableProperty PointsProperty = BindableProperty.Create("Points", typeof(DataPointCollection), typeof(Series), default(DataPointCollection), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Series), Color.Blue, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty TypeProperty = BindableProperty.Create("Type", typeof(ChartType), typeof(Series), ChartType.Bar, BindingMode.OneWay, null, null, null, null);

        public Color Color
        {
            get
            {
                return (Color)base.GetValue(Series.ColorProperty);
            }
            set
            {
                base.SetValue(Series.ColorProperty, value);
            }
        }

        public DataPointCollection Points
        {
            get
            {
                return (DataPointCollection)base.GetValue(Series.PointsProperty);
            }
            set
            {
                base.SetValue(Series.PointsProperty, value);
            }
        }

        public ChartType Type
        {
            get
            {
                return (ChartType)base.GetValue(Series.TypeProperty);
            }
            set
            {
                base.SetValue(Series.TypeProperty, value);
            }
        }

        public Series()
        {
            Points = new DataPointCollection();
        }

        public Series(Color color)
        {
            Points = new DataPointCollection();
            this.Color = color;
        }
    }
}

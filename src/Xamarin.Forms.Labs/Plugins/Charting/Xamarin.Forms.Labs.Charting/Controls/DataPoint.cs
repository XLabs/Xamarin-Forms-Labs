using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class DataPoint : Element
    {
        public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(DataPoint), String.Empty, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(float), typeof(DataPoint), 0.0F, BindingMode.OneWay, null, null, null, null);

        public string Label
        {
            get
            {
                return (string)base.GetValue(DataPoint.LabelProperty);
            }
            set
            {
                base.SetValue(DataPoint.LabelProperty, value);
            }
        }

        public float Value
        {
            get
            {
                return (float)base.GetValue(DataPoint.ValueProperty);
            }
            set
            {
                base.SetValue(DataPoint.ValueProperty, value);
            }
        }
    }
}

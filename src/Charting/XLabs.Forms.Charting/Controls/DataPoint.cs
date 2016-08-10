namespace XLabs.Forms.Charting.Controls
{
	using System;

	using Xamarin.Forms;

	/// <summary>
	/// Contains the logic for drawing a point.
	/// </summary>
	public class DataPoint : Element
	{
		/// <summary>
		/// The label property
		/// </summary>
		public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(DataPoint), String.Empty, BindingMode.OneWay, null, null, null, null);
		/// <summary>
		/// The value property
		/// </summary>
		public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(DataPoint), 0.0, BindingMode.OneWay, null, null, null, null);
		/// <summary>
		/// The color property
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(DataPoint), Color.Blue, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// X-axis label. Only the labels of the first series will be rendered.
		/// </summary>
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

		/// <summary>
		/// Value of the point, used to be drawn.
		/// </summary>
		public double Value
		{
			get
			{
				return (double)base.GetValue(DataPoint.ValueProperty);
			}
			set
			{
				base.SetValue(DataPoint.ValueProperty, value);
			}
		}

		/// <summary>
		/// Color of the point, used to be drawn as pie-chart slice.
		/// </summary>
		public Color Color
		{
			get
			{
				return (Color)base.GetValue(DataPoint.ColorProperty);
			}
			set
			{
				base.SetValue(DataPoint.ColorProperty, value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPoint"/> class.
		/// </summary>
		public DataPoint()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPoint"/> class.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="value">The value.</param>
		public DataPoint(string label, double value)
		{
			Label = label;
			Value = value;
		}
	}
}

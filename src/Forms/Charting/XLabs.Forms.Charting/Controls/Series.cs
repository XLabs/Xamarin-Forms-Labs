namespace XLabs.Forms.Charting.Controls
{
	using Xamarin.Forms;

	/// <summary>
	/// Represents a single series to be drawn in a chart.
	/// </summary>
	public class Series : Element
	{
		public static readonly BindableProperty PointsProperty = BindableProperty.Create("Points", typeof(DataPointCollection), typeof(Series), default(DataPointCollection), BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Series), Color.Blue, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty TypeProperty = BindableProperty.Create("Type", typeof(ChartType), typeof(Series), ChartType.Bar, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// Color of the Series.
		/// </summary>
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

		/// <summary>
		/// DataPoints containing Y-axis values and X-axis labels.
		/// </summary>
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

		/// <summary>
		/// Type of the series. Possible values: Bar & Line.
		/// </summary>
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
	}
}

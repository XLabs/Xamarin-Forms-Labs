using System.Runtime.CompilerServices;

namespace XLabs.Forms.Charting.Controls
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Xamarin.Forms;

	using XLabs.Forms.Charting.Events;

	/// <summary>
	/// Contains charting algorithms and is able to draw and render a chart.
	/// </summary>
	public class Chart : View
	{
		#region Constants
		private const int PADDING_LEFT = 50;
		private const int PADDING_TOP = 20;
		#endregion

		#region BindableProperties
		public static readonly BindableProperty DataSourceProperty = BindableProperty.Create("DataSource", typeof(IEnumerable<IEnumerable<object>>), typeof(Chart), default(IEnumerable<IEnumerable<object>>), BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Chart), Color.White, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty SeriesProperty = BindableProperty.Create("Series", typeof(SeriesCollection), typeof(Chart), default(SeriesCollection), BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(Chart), 5.0, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty GridProperty = BindableProperty.Create("Grid", typeof(bool), typeof(Chart), true, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty XPathProperty = BindableProperty.Create("XPath", typeof(string), typeof(Chart), null, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty YPathProperty = BindableProperty.Create("YPath", typeof(string), typeof(Chart), null, BindingMode.OneWay, null, null, null, null);
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the datasource of the chart's data to display.
		/// </summary>
		public IEnumerable<IEnumerable<object>> DataSource
		{
			get
			{
				return (IEnumerable<IEnumerable<object>>)base.GetValue(Chart.DataSourceProperty);
			}
			set
			{
				base.SetValue(Chart.DataSourceProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the color of the grid and border of the chart element.
		/// </summary>
		public Color Color
		{
			get
			{
				return (Color)base.GetValue(Chart.ColorProperty);
			}
			set
			{
				base.SetValue(Chart.ColorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the series which should be displayed inside the chart element.
		/// </summary>
		public SeriesCollection Series
		{
			get
			{
				return (SeriesCollection)base.GetValue(Chart.SeriesProperty);
			}
			set
			{
				base.SetValue(Chart.SeriesProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the desired spacing between series inside chart element.
		/// </summary>
		public double Spacing
		{
			get
			{
				return (double)base.GetValue(Chart.SpacingProperty);
			}
			set
			{
				base.SetValue(Chart.SpacingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this element contains horizontal grid lines.
		/// </summary>
		public bool Grid
		{
			get
			{
				return (bool)base.GetValue(Chart.GridProperty);
			}
			set
			{
				base.SetValue(Chart.GridProperty, value);
			}
		}
		/// <summary>
		/// Determines the property on the object that contains the X-values of the chart.
		/// </summary>
		/// <remarks>
		/// Will be used if the DataSource property is set.
		/// </remarks>
		public string XPath
		{
			get
			{
				return (string)base.GetValue(Chart.XPathProperty);
			}
			set
			{
				base.SetValue(Chart.XPathProperty, value);
			}
		}
		/// <summary>
		/// Determines the property on the object that contains the Y-values of the chart.
		/// </summary>
		/// <remarks>
		/// Will be used if the DataSource property is set.
		/// </remarks>
		public string YPath
		{
			get
			{
				return (string)base.GetValue(Chart.YPathProperty);
			}
			set
			{
				base.SetValue(Chart.YPathProperty, value);
			}
		}
		#endregion

		public Chart()
		{
			Series = new SeriesCollection();
		}

		/// <summary>
		/// Event args for the Chart events.
		/// </summary>
		/// <typeparam name="T">A DrawingData class, which is put into the Data property.</typeparam>
		internal class DrawEventArgs<T> : EventArgs
		{
			/// <summary>
			/// The event coming from the Chart
			/// </summary>
			public T Data { get; set; }
		}

		#region Events
		/// <summary>
		/// Fires when canvas needs to draw text
		/// </summary>
		internal event EventHandler<DrawEventArgs<TextDrawingData>> OnDrawText;

		/// <summary>
		/// Fires when canvas needs to draw a grid line
		/// </summary>
		internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawGridLine;

		/// <summary>
		/// Fires when canvas needs to draw a bar
		/// </summary>
		internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawBar;

		/// <summary>
		/// Fires when canvas needs to draw a line
		/// </summary>
		internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawLine;

		/// <summary>
		/// Fires when canvas needs to draw a circle
		/// </summary>
		internal event EventHandler<DrawEventArgs<SingleDrawingData>> OnDrawCircle;

		/// <summary>
		/// Fires when canvas needs to draw a pie
		/// </summary>
		internal event EventHandler<DrawEventArgs<PieDrawingData>> OnDrawPie;
		#endregion

		/// <summary>
		/// Draw the chart using the specified handlers.
		/// </summary>
		/// <remarks>
		/// Set the events before calling DrawChart.
		/// </remarks>
		public void DrawChart()
		{
			int noOfBars = 0;
			double highestValue = 0;

			if (DataSource != default(IEnumerable<IEnumerable>))
			{
				while (Series.Count < DataSource.Count())
					Series.Add(new Series());

				for (int i = 0; i < DataSource.Count(); i++)
				{
					IEnumerable<object> seriesValues = DataSource.ElementAt(i);
					// There should be at least one value to access its type and properties.
					if (seriesValues.Count() > 1)
					{
						Type type = seriesValues.ElementAt(1).GetType();
						IEnumerable<PropertyInfo> properties = type.GetTypeInfo().DeclaredProperties;
						// There should be at least two accessible properties on the type.
						if (properties.Count() >= 2)
						{
							Series series = Series[i];
							series.Points = new DataPointCollection();

							foreach (var val in seriesValues)
							{
								// Currently DataPoint expects a string (X) and a double (Y).
								string xValue = "";
								if (XPath != null)
								{
									// Get value from property using reflection
									PropertyInfo info = properties.FirstOrDefault(p => p.Name == XPath);
									if (info != null)
										xValue = info.GetValue(val).ToString();
								}
								else
									// Get value from property using the first property it can find on the object
									xValue = properties.ElementAt(0).GetValue(val, null).ToString();

								double yValue = 0;
								if (YPath != null)
								{
									// Get value from property using reflection
									PropertyInfo info = properties.FirstOrDefault(p => p.Name == YPath);
									if (info != null)
										yValue = Convert.ToDouble(info.GetValue(val).ToString());
								}
								else
									// Get value from property using the second property it can find on the object
									yValue = Convert.ToDouble(properties.ElementAt(1).GetValue(val, null));

								series.Points.Add(new DataPoint(xValue, yValue));
							}
						}
					}
				}
			}


			foreach (Series series in Series)
			{
				if (series.Type == ChartType.Bar)
					noOfBars += series.Points.Count();

				double tempHighestValue = series.Points.Max(p => p.Value);
				if (highestValue < tempHighestValue)
				{
					highestValue = tempHighestValue;
				}
			}

			if (Series.FirstOrDefault(s => s.Type == ChartType.Pie) == null)
				highestValue = DrawGrid(highestValue);

			// If there are no bars, fake them
			if (noOfBars == 0)
				noOfBars = Series[0].Points.Count;

			double widthPerBar = ((WidthRequest - PADDING_LEFT) - (Spacing * (noOfBars - 1))) / noOfBars;

			if (Series.FirstOrDefault(s => s.Type == ChartType.Pie) == null)
			{
				OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, PADDING_TOP, PADDING_LEFT, HeightRequest, 0) });  //Y-axis
				OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, HeightRequest + 1, WidthRequest, HeightRequest + 1, 0) });      //X-axis
				DrawLabels(highestValue, widthPerBar, Series[0].Points);
				for (int i = 0; i < Series.Count; i++)
				{
					Series series = Series[i];

					switch (series.Type)
					{
						case ChartType.Bar:
							DrawBarChart(highestValue, widthPerBar, i, series.Points);
							break;
						case ChartType.Line:
							DrawLineChart(highestValue, widthPerBar, i, series.Points);
							break;
					}
				}
			}
			else
			{
				Series series = Series.First(s => s.Type == ChartType.Pie);
				DrawPieChart(series.Points, Series.IndexOf(series));
			}
		}

		#region Private Drawing Functions
		/// <summary>
		/// Draw the horizontal grid lines and Y-axis labels.
		/// </summary>
		/// <param name="highestValue">Highest Y-value within the series.</param>
		/// <returns>New highest value.</returns>
		private float DrawGrid(double highestValue)
		{
			int noOfHorizontalLines = 4;
			double quarterValue = highestValue / 4;
			double valueOfPart = ((int)Math.Round(quarterValue / 10.0)) * 10;
			if (valueOfPart < quarterValue)
				noOfHorizontalLines = 5;
			double quarterHeight = (HeightRequest - PADDING_TOP) / noOfHorizontalLines;

			// Horizontal lines and Y-value labels
			OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData((valueOfPart * noOfHorizontalLines).ToString(), 10, PADDING_TOP + 5) });
			for (int i = 1; i <= noOfHorizontalLines; i++)
			{
				if (Grid)
				{
					OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, PADDING_TOP + (quarterHeight * i), WidthRequest, PADDING_TOP + (quarterHeight * i), 0) });
				}
				double currentValue = (valueOfPart * noOfHorizontalLines) - (valueOfPart * i);
				OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData(currentValue.ToString(), 10, PADDING_TOP + (quarterHeight * i) + 5) });
			}

			return (float)valueOfPart * noOfHorizontalLines;
		}

		/// <summary>
		/// Draw a bar.
		/// </summary>
		/// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
		/// <param name="widthPerBar">Width of a single bar.</param>
		/// <param name="barNo">The number of the series</param>
		/// <param name="points">Specified points in the series.</param>
		private void DrawBarChart(double highestValue, double widthPerBar, int barNo, DataPointCollection points)
		{
			double widthIterator = 2 + (barNo * widthPerBar) + PADDING_LEFT;

			foreach (DataPoint point in points)
			{
				double heightOfBar = ((HeightRequest - PADDING_TOP) / highestValue) * point.Value;

				OnDrawBar(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(widthIterator + 1, ((HeightRequest - PADDING_TOP) - heightOfBar) + PADDING_TOP, (widthIterator + widthPerBar) - 1, HeightRequest, barNo) });

				widthIterator += widthPerBar * Series.Count(s => s.Type == ChartType.Bar) + Spacing;
			}
		}

		/// <summary>
		/// Draw a line.
		/// </summary>
		/// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
		/// <param name="widthPerBar">Width of a single bar.</param>
		/// <param name="lineNo">The number of the series</param>
		/// <param name="points">Specified points in the series.</param>
		private void DrawLineChart(double highestValue, double widthPerBar, int lineNo, DataPointCollection points)
		{
			int noOfBarSeries = Series.Count(s => s.Type == ChartType.Bar);
			if (noOfBarSeries == 0)
				noOfBarSeries = 1;
			double widthOfAllBars = noOfBarSeries * widthPerBar;
			double widthIterator = 2 + PADDING_LEFT;

			List<double> pointsList = new List<double>();
			double[] previousPoints = new double[2];
			for (int i = 0; i < points.Count; i++)
			{
				double heightOfLine = ((HeightRequest - PADDING_TOP) / highestValue) * points[i].Value;

				double x = widthIterator + (widthOfAllBars / 2);
				double y = ((HeightRequest - PADDING_TOP) - heightOfLine) + PADDING_TOP;

				if (i != 0)
				{
					OnDrawLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(previousPoints[0], previousPoints[1], x, y, lineNo) });
				}

				previousPoints[0] = x;
				previousPoints[1] = y;

				OnDrawCircle(this, new DrawEventArgs<SingleDrawingData>() { Data = new SingleDrawingData(widthIterator + (widthOfAllBars / 2), ((HeightRequest - PADDING_TOP) - heightOfLine) + PADDING_TOP, lineNo) });

				widthIterator += widthPerBar * noOfBarSeries + Spacing;
			}
		}

		/// <summary>
		/// Draw a pie chart.
		/// </summary>
		/// <param name="points">Specified points in the series.</param>
		private void DrawPieChart(DataPointCollection points, int pieNo)
		{
			double sizeOfCircle = ((WidthRequest > HeightRequest) ? HeightRequest / 2 : WidthRequest / 2);
			double[] values = points.Select(p => p.Value).ToArray();
			double degreesPerValue = 360 / values.Sum();

			for (int i = 0; i < values.Length; i++)
			{
				values[i] = values[i] * degreesPerValue;
			}

			OnDrawPie(this, new DrawEventArgs<PieDrawingData> { Data = new PieDrawingData(WidthRequest / 2, HeightRequest / 2, pieNo, sizeOfCircle, values) });
		}

		/// <summary>
		/// Draw text.
		/// </summary>
		/// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
		/// <param name="widthPerBar">Width of a single bar.</param>
		/// <param name="points">Specified points in the series.</param>
		private void DrawLabels(double highestValue, double widthPerBar, DataPointCollection points)
		{
			int noOfBarSeries = Series.Count(s => s.Type == ChartType.Bar);
			if (noOfBarSeries == 0)
				noOfBarSeries = 1;
			double widthOfAllBars = noOfBarSeries * widthPerBar;
			double widthIterator = 2 + PADDING_LEFT;

			for (int i = 0; i < points.Count; i++)
			{
				OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData(points[i].Label, (widthIterator + widthOfAllBars / 2) - (points[i].Label.Length * 4), HeightRequest + 25) });
				widthIterator += widthPerBar * noOfBarSeries + Spacing;
			}
		}
		#endregion
	}
}

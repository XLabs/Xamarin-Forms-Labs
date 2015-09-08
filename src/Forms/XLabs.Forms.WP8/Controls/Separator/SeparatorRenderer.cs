using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]

namespace XLabs.Forms.Controls
{
	using System.Windows.Media;
	using System.Windows.Shapes;

	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	/// Class SeparatorRenderer.
	/// </summary>
	public class SeparatorRenderer : ViewRenderer<Separator, Path>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				return;
			}

			if (Control == null)
			{
				Background = Xamarin.Forms.Color.Transparent.ToBrush();
				SetNativeControl(new Path());
			}

			SetProperties(Control);
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetProperties(Control);
		}

		/// <summary>
		/// Sets the properties.
		/// </summary>
		/// <param name="line">The line.</param>
		private void SetProperties(Path line)
		{
			var myLineSegment = new LineSegment()
			{
				Point = new System.Windows.Point(Element.Width, 0)
			};

			var myPathSegmentCollection = new PathSegmentCollection { myLineSegment };

			var myPathFigureCollection = new PathFigureCollection
				                             {
					                             new PathFigure
						                             {
							                             StartPoint = new System.Windows.Point(0, 0),
							                             Segments = myPathSegmentCollection
						                             }
				                             };

			line.Stroke = Element.Color.ToBrush();
			line.StrokeDashArray = new System.Windows.Media.DoubleCollection();

			if (Element.StrokeType != StrokeType.Solid)
			{
				if (Element.StrokeType == StrokeType.Dashed)
				{
					line.StrokeDashArray.Add(10);
				}
				line.StrokeDashArray.Add(2);
			}

			line.Data = new PathGeometry { Figures = myPathFigureCollection };

			line.StrokeThickness = Element.Thickness;
		}
	}
}

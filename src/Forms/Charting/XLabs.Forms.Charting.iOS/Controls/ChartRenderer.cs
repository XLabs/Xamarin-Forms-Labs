using Xamarin.Forms;

using XLabs.Forms.Charting.Controls;

[assembly: ExportRenderer(typeof(Chart), typeof(ChartRenderer))]
namespace XLabs.Forms.Charting.Controls
{
	using System.Linq;

	using UIKit;

	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ChartRenderer.
	/// </summary>
	public class ChartRenderer : ViewRenderer<Chart, ChartSurface>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Chart> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || Element == null)
			{
				return;
			}

			// Use color specified at DataPoints if it is a Pie Chart
			UIColor[] colors;
			
			var pieSeries = Element.Series.FirstOrDefault(s => s.Type == ChartType.Pie);
			
			if (pieSeries != null)
			{
				colors = new UIColor[pieSeries.Points.Count];
				for (int i = 0; i < pieSeries.Points.Count; i++)
				{
					colors[i] = pieSeries.Points[i].Color.ToUIColor();
				}
			}
			else
			{
				colors = new UIColor[Element.Series.Count];
				for (var i = 0; i < Element.Series.Count; i++)
				{
					colors[i] = Element.Series[i].Color.ToUIColor();
				}
			}

			var surfaceView = new ChartSurface(Element, Element.Color.ToUIColor(), colors);
			SetNativeControl(surfaceView);
		}
	}
}
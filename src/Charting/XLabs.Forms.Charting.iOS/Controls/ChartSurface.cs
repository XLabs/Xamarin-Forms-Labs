using OpenTK;

namespace XLabs.Forms.Charting.Controls
{
	using CoreGraphics;
	using Events;
	using Foundation;
	using System;
	using UIKit;

	/// <summary>
	/// Class ChartSurface.
	/// </summary>
	public class ChartSurface : UIView
    {
        const float START_ANGLE = -((float)Math.PI / 2);
        const float END_ANGLE = ((2 * (float)Math.PI) + START_ANGLE);

        /// <summary>
        /// The chart
        /// </summary>
        internal Chart Chart;

        /// <summary>
        /// The color
        /// </summary>
        internal UIColor Color;

        /// <summary>
        /// The colors
        /// </summary>
        internal UIColor[] Colors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSurface"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="color">The color.</param>
        /// <param name="colors">The colors.</param>
        public ChartSurface(Chart chart, UIColor color, UIColor[] colors)
        {
            Chart = chart;
            Color = color;
            Colors = colors;

            Chart.OnDrawBar -= _chart_OnDrawBar;
            Chart.OnDrawBar += _chart_OnDrawBar;
            Chart.OnDrawCircle -= _chart_OnDrawCircle;
            Chart.OnDrawCircle += _chart_OnDrawCircle;
            Chart.OnDrawGridLine -= _chart_OnDrawGridLine;
            Chart.OnDrawGridLine += _chart_OnDrawGridLine;
            Chart.OnDrawLine -= _chart_OnDrawLine;
            Chart.OnDrawLine += _chart_OnDrawLine;
            Chart.OnDrawText -= _chart_OnDrawText;
            Chart.OnDrawText += _chart_OnDrawText;
            Chart.OnDrawPie -= _chart_OnDrawPie;
            Chart.OnDrawPie += _chart_OnDrawPie;
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            Chart.DrawChart();
        }

        /// <summary>
        /// _chart_s the on draw bar.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(1);
                Colors[e.Data.SeriesNo].SetFill();
                Colors[e.Data.SeriesNo].SetStroke();

                var rect = new CGRect((float)e.Data.XFrom, (float)e.Data.YFrom, (float)(e.Data.XTo - e.Data.XFrom), (float)(e.Data.YTo - e.Data.YFrom));
                g.AddRect(rect);

                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        /// <summary>
        /// _chart_s the on draw circle.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<SingleDrawingData> e)
        {
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(2);
                Colors[e.Data.SeriesNo].SetFill();
                Colors[e.Data.SeriesNo].SetStroke();
                g.AddArc((float)e.Data.X, (float)e.Data.Y, (float)e.Data.Size, START_ANGLE, END_ANGLE, true);
                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        /// <summary>
        /// _chart_s the on draw grid line.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(2);
                Color.SetFill();
                Color.SetStroke();

                g.MoveTo((float)e.Data.XFrom, (float)e.Data.YFrom);
                g.AddLineToPoint((float)e.Data.XTo, (float)e.Data.YTo);

                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        /// <summary>
        /// _chart_s the on draw line.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _chart_OnDrawLine(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(2.5F);
                Colors[e.Data.SeriesNo].SetFill();
                Colors[e.Data.SeriesNo].SetStroke();

                g.MoveTo((float)e.Data.XFrom, (float)e.Data.YFrom);
                g.AddLineToPoint((float)e.Data.XTo, (float)e.Data.YTo);

                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        /// <summary>
        /// _chart_s the on draw pie.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _chart_OnDrawPie(object sender, Chart.DrawEventArgs<PieDrawingData> e)
        {
            double totalDegrees = 0;
            for (var i = 0; i < e.Data.Percentages.Length; i++)
            {
                var degrees = e.Data.Percentages[i];
                using (var g = UIGraphics.GetCurrentContext())
                {
                    g.SetLineWidth(2);
                    Colors[i].SetFill();
                    Colors[i].SetStroke();
                    g.MoveTo((float)e.Data.X, (float)e.Data.Y);
                    g.AddArc((float)e.Data.X, (float)e.Data.Y, (float)e.Data.Size,
                             (float)MathHelper.Deg2Rad(360 - totalDegrees - degrees), (float)MathHelper.Deg2Rad(360 - totalDegrees), false);
                    g.DrawPath(CGPathDrawingMode.FillStroke);
                }

                totalDegrees += degrees;
            }
        }

        /// <summary>
        /// _chart_s the on draw text.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void _chart_OnDrawText(object sender, Chart.DrawEventArgs<TextDrawingData> e)
        {
            var str = new NSString(e.Data.Text);
            str.DrawString(new CGPoint((float)e.Data.X, (float)e.Data.Y), UIFont.SystemFontOfSize(12));
        }
    }
}
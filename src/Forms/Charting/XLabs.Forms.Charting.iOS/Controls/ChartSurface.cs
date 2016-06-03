namespace XLabs.Forms.Charting.Controls
{
    using System;
    using CoreGraphics;
    using Events;
    using Foundation;
    using UIKit;

    /// <summary>
    /// Class ChartSurface.
    /// </summary>
    public class ChartSurface : UIView
    {
        const float StartAngle = -((float)Math.PI / 2);
        const float EndAngle = ((2 * (float)Math.PI) + StartAngle);

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
            this.Chart = chart;
            this.Color = color;
            this.Colors = colors;

            this.Chart.OnDrawBar -= _chart_OnDrawBar;
            this.Chart.OnDrawBar += _chart_OnDrawBar;
            this.Chart.OnDrawCircle -= _chart_OnDrawCircle;
            this.Chart.OnDrawCircle += _chart_OnDrawCircle;
            this.Chart.OnDrawGridLine -= _chart_OnDrawGridLine;
            this.Chart.OnDrawGridLine += _chart_OnDrawGridLine;
            this.Chart.OnDrawLine -= _chart_OnDrawLine;
            this.Chart.OnDrawLine += _chart_OnDrawLine;
            this.Chart.OnDrawText -= _chart_OnDrawText;
            this.Chart.OnDrawText += _chart_OnDrawText;
            this.Chart.OnDrawPie -= _chart_OnDrawPie;
            this.Chart.OnDrawPie += _chart_OnDrawPie;
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            this.Chart.DrawChart();
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
                this.Colors[e.Data.SeriesNo].SetFill();
                this.Colors[e.Data.SeriesNo].SetStroke();

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
                this.Colors[e.Data.SeriesNo].SetFill();
                this.Colors[e.Data.SeriesNo].SetStroke();
                g.AddArc((float)e.Data.X, (float)e.Data.Y, (float)e.Data.Size, StartAngle, EndAngle, true);
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
                this.Color.SetFill();
                this.Color.SetStroke();

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
                this.Colors[e.Data.SeriesNo].SetFill();
                this.Colors[e.Data.SeriesNo].SetStroke();

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
                    this.Colors[i].SetFill();
                    this.Colors[i].SetStroke();
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
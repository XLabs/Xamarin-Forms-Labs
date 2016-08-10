using WPColor = System.Windows.Media.Color;
using WPPoint = System.Windows.Point;
using WPSize = System.Windows.Size;
using WPShapes = System.Windows.Shapes;

namespace XLabs.Forms.Charting.Controls
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media;

    using WPShapes;

    using XLabs.Forms.Charting.Events;

    /// <summary>
    /// Class ChartSurface.
    /// </summary>
    public class ChartSurface : Canvas
    {
        /// <summary>
        /// The chart
        /// </summary>
        public Chart Chart;
        /// <summary>
        /// The brush
        /// </summary>
        public SolidColorBrush Brush;
        /// <summary>
        /// The colors
        /// </summary>
        public WPColor[] Colors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSurface"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="color">The color.</param>
        /// <param name="colors">The colors.</param>
        public ChartSurface(Chart chart, WPColor color, WPColor[] colors)
            : base()
        {
            Chart = chart;
            Brush = new SolidColorBrush(color);
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

            Redraw();
        }

        /// <summary>
        /// Redraws this instance.
        /// </summary>
        public void Redraw()
        {
            Chart.DrawChart();
        }

        /// <summary>
        /// _chart_s the on draw bar.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            WPShapes.Rectangle rectangle = new WPShapes.Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors[e.Data.SeriesNo]);
            rectangle.Width = e.Data.XTo - e.Data.XFrom;
            rectangle.Height = e.Data.YTo - e.Data.YFrom;

            Canvas.SetLeft(rectangle, e.Data.XFrom);
            Canvas.SetTop(rectangle, e.Data.YFrom);

            this.Children.Add(rectangle);
        }

        /// <summary>
        /// _chart_s the on draw circle.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<SingleDrawingData> e)
        {
            WPShapes.Ellipse ellipse = new WPShapes.Ellipse
            {
                Fill = new SolidColorBrush(Colors[e.Data.SeriesNo]),
                Width = e.Data.Size,
                Height = e.Data.Size
            };

            Canvas.SetLeft(ellipse, e.Data.X - (e.Data.Size / 2));
            Canvas.SetTop(ellipse, e.Data.Y - (e.Data.Size / 2));

            this.Children.Add(ellipse);
        }

        /// <summary>
        /// _chart_s the on draw grid line.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            WPShapes.Line line = new WPShapes.Line
            {
                Stroke = Brush,
                StrokeThickness = 2,
                X1 = e.Data.XFrom,
                Y1 = e.Data.YFrom,
                X2 = e.Data.XTo,
                Y2 = e.Data.YTo
            };


            this.Children.Add(line);
        }

        /// <summary>
        /// _chart_s the on draw line.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawLine(object sender, Chart.DrawEventArgs<DoubleDrawingData> e)
        {
            WPShapes.Line line = new WPShapes.Line
            {
                Stroke = new SolidColorBrush(Colors[e.Data.SeriesNo]),
                StrokeThickness = 2,
                X1 = e.Data.XFrom,
                Y1 = e.Data.YFrom,
                X2 = e.Data.XTo,
                Y2 = e.Data.YTo
            };


            this.Children.Add(line);
        }

        /// <summary>
        /// _chart_s the on draw text.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawText(object sender, Chart.DrawEventArgs<TextDrawingData> e)
        {
            TextBlock textBlock = new TextBlock { Foreground = Brush, Text = e.Data.Text };

            Canvas.SetLeft(textBlock, e.Data.X);
            Canvas.SetTop(textBlock, e.Data.Y);

            this.Children.Add(textBlock);
        }
        /// <summary>
        /// _chart_s the on draw pie.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _chart_OnDrawPie(object sender, Chart.DrawEventArgs<PieDrawingData> e)
        {
            double size = ((e.Data.X > e.Data.Y) ? e.Data.Y * 2 : e.Data.X * 2);
            double halfSize = size / 2;
            var previousPoint = new WPPoint(size, halfSize); // initial perimetric point (at 0 degrees)

            double totalDegrees = 0;
            for (int i = 0; i < e.Data.Percentages.Length; i++)
            {
                var path = new Path();

                var pathFigure = new PathFigure();
                pathFigure.IsClosed = true;
                pathFigure.StartPoint = new WPPoint(halfSize, halfSize);

                var degrees = e.Data.Percentages[i];
                totalDegrees += degrees;
                // calculate perimetric point at totalDegrees
                double coordinateX = halfSize * Math.Cos(MathHelper.Deg2Rad(360 - totalDegrees));
                double coordinateY = halfSize * Math.Sin(MathHelper.Deg2Rad(360 - totalDegrees));

                var lineSegment = new LineSegment();
                lineSegment.Point = previousPoint;
                pathFigure.Segments.Add(lineSegment);

                previousPoint = new WPPoint(coordinateX + halfSize, coordinateY + halfSize); // actual perimetric point (will be the previous in the next iteration)

                var arcSegment = new ArcSegment();
                arcSegment.Size = new WPSize(halfSize, halfSize);
                arcSegment.Point = previousPoint;
                arcSegment.RotationAngle = 0;
                arcSegment.IsLargeArc = degrees > 180 ? true : false;
                arcSegment.SweepDirection = SweepDirection.Counterclockwise;
                pathFigure.Segments.Add(arcSegment);

                var pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection();

                pathGeometry.Figures.Add(pathFigure);

                path.Data = pathGeometry;
                path.Fill = new SolidColorBrush(Colors[i]);

                Children.Add(path);
            }
        }
    }
}

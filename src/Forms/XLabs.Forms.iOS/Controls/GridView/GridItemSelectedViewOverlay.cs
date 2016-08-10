namespace XLabs.Forms.Controls
{
    using CoreGraphics;
    using UIKit;

    /// <summary>
    /// Class GridItemSelectedViewOverlay.
    /// </summary>
    public class GridItemSelectedViewOverlay : UIView
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GridItemSelectedViewOverlay"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public GridItemSelectedViewOverlay (CGRect frame) : base (frame)
        {
            this.BackgroundColor = UIColor.Clear;
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw (CGRect rect)
        {
            using (var g = UIGraphics.GetCurrentContext ()) 
            {
                g.SetLineWidth (10);
                UIColor.FromRGB (64, 30, 168).SetStroke ();
                UIColor.Clear.SetFill ();

                //create geometry
                var path = new CGPath ();
                path.AddRect (rect);
                path.CloseSubpath ();

                //add geometry to graphics context and draw it
                g.AddPath (path);
                g.DrawPath (CGPathDrawingMode.Stroke);
            }
        }
    }
}
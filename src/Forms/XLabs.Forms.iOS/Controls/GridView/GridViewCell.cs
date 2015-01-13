namespace XLabs.Forms.Controls
{
	using System.ComponentModel;
	using CoreGraphics;

	using CoreGraphics;
	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class GridViewCell.
	/// </summary>
	public class GridViewCell : UICollectionViewCell
	{
		/// <summary>
		/// The key
		/// </summary>
		public const string Key = "GridViewCell";

		/// <summary>
		/// The _view cell
		/// </summary>
		private ViewCell _viewCell;
		/// <summary>
		/// The _view
		/// </summary>
		private UIView _view;

		/// <summary>
		/// Gets or sets the view cell.
		/// </summary>
		/// <value>The view cell.</value>
		public ViewCell ViewCell {
			get {
				return _viewCell;
			}
			set {
				if (_viewCell == value) {
					return;
				}
				UpdateCell (value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewCell"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		[Export ("initWithFrame:")]
		public GridViewCell (CGRect frame) : base (frame)
		{
			// SelectedBackgroundView = new GridItemSelectedViewOverlay (frame);
			// this.BringSubviewToFront (SelectedBackgroundView);

		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		/// <param name="cell">The cell.</param>
		private void UpdateCell (ViewCell cell)
		{
			if (_viewCell != null) {
				//this.viewCell.SendDisappearing ();
				_viewCell.PropertyChanged -= new PropertyChangedEventHandler (HandlePropertyChanged);
			}
			_viewCell = cell;
			_viewCell.PropertyChanged += new PropertyChangedEventHandler (HandlePropertyChanged);
			//this.viewCell.SendAppearing ();
			UpdateView ();
		}

		/// <summary>
		/// Handles the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			UpdateView ();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		private void UpdateView ()
		{

			if (_view != null) {
				_view.RemoveFromSuperview ();
			}
			_view = RendererFactory.GetRenderer (_viewCell.View).NativeView;
			_view.AutoresizingMask = UIViewAutoresizing.All;
			_view.ContentMode = UIViewContentMode.ScaleToFill;

			AddSubview (_view);
		}

		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			CGRect frame = ContentView.Frame;
			frame.X = (Bounds.Width - frame.Width) / 2;
			frame.Y = (Bounds.Height - frame.Height) / 2;
			ViewCell.View.Layout (frame.ToRectangle ());
			_view.Frame = frame;
		}
	}

	//SelectedView Overlay Windows8 style
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
			BackgroundColor = UIColor.Clear;
		}

		/// <summary>
		/// Draws the specified rect.
		/// </summary>
		/// <param name="rect">The rect.</param>
		public override void Draw (CGRect rect)
		{
			using (var g = UIGraphics.GetCurrentContext ()) {
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


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
        /// Layouts the subviews.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = this.ContentView.Frame;
            frame.X = (this.Bounds.Width - frame.Width) / 2;
            frame.Y = (this.Bounds.Height - frame.Height) / 2;
            this.ViewCell.View.Layout(frame.ToRectangle());
            this._view.Frame = frame;
        }

        /// <summary>
        /// Gets or sets the view cell.
        /// </summary>
        /// <value>The view cell.</value>
        public ViewCell ViewCell
        {
            get
            {
                return this._viewCell;
            }
            set
            {
                if (this._viewCell != value)
                {
                    UpdateCell(value);
                }
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        private void UpdateCell (ViewCell cell)
        {
            if (this._viewCell != null) 
            {
                //this.viewCell.SendDisappearing ();
                this._viewCell.PropertyChanged -= this.HandlePropertyChanged;
            }

            this._viewCell = cell;
            this._viewCell.PropertyChanged += this.HandlePropertyChanged;
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
            if (this._view != null) 
            {
                this._view.RemoveFromSuperview ();
            }

            this._view = RendererFactory.GetRenderer (this._viewCell.View).NativeView;
            this._view.AutoresizingMask = UIViewAutoresizing.All;
            this._view.ContentMode = UIViewContentMode.ScaleToFill;

            AddSubview (this._view);
        }
    }
}


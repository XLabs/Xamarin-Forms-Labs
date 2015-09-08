using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System;
	using System.ComponentModel;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	/// Class ExtendedScrollViewRenderer.
	/// </summary>
	public class ExtendedScrollViewRenderer : ScrollViewRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
		{
			base.OnElementChanged(e);

			LayoutUpdated += (sender, ev) =>
			{
				var scrollView = (ExtendedScrollView)Element;
				var bounds = new Rectangle(Control.HorizontalOffset, Control.VerticalOffset, Control.ScrollableWidth, Control.ScrollableHeight);
				scrollView.UpdateBounds(bounds);
			};

			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;

			e.NewElement.PropertyChanged += OnElementPropertyChanged;
		}

		/// <summary>
		/// The _epsilon
		/// </summary>
		double _epsilon = 0.1;

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName)
			{
				var scrollView = (ExtendedScrollView)Element;
				var position = scrollView.Position;

				if (Math.Abs(Control.VerticalOffset - position.Y) < _epsilon
					&& Math.Abs(Control.HorizontalOffset - position.X) < _epsilon)
					return;

				Control.ScrollToVerticalOffset(position.Y);
				Control.UpdateLayout();
			}
		}

	}
}


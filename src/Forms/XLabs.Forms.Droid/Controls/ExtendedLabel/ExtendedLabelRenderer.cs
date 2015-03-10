using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRender))]
namespace XLabs.Forms.Controls
{
	using System;

	using Android.Graphics;
	using Android.Widget;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using XLabs.Forms.Extensions;

	/// <summary>
	/// Class ExtendedLabelRender.
	/// </summary>
	public class ExtendedLabelRender : LabelRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedLabelRender"/> class.
		/// </summary>
		public ExtendedLabelRender()
		{
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null && e.NewElement != null) {

				var view = (ExtendedLabel) e.NewElement;
				var control = Control;

				UpdateUi (view, control);
			}

		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">The event arguments</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == ExtendedLabel.IsUnderlineProperty.PropertyName ||
				e.PropertyName == ExtendedLabel.IsDropShadowProperty.PropertyName ||
				e.PropertyName == ExtendedLabel.IsStrikeThroughProperty.PropertyName
				) {
				var view = (ExtendedLabel) Element;
				var control = Control;
				UpdateUi (view,control);
			}
		}

		/// <summary>
		/// Updates the UI.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="control">The control.</param>
		void UpdateUi(ExtendedLabel view, TextView control)
		{
			if (view == null || control == null)
				return;

			if(view.IsUnderline)
			{
				control.PaintFlags = control.PaintFlags | PaintFlags.UnderlineText;
			}

			if(view.IsStrikeThrough)
			{
				control.PaintFlags = control.PaintFlags | PaintFlags.StrikeThruText;
			}

			if (view.IsDropShadow) {
				//TODO:: Needs implementation
			}

		}
	}
}


using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof (ExtendedLabel), typeof (ExtendedLabelRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	/// The extended label renderer.
	/// </summary>
	public class ExtendedLabelRenderer : LabelRenderer
	{
		/// <summary>
		/// The on element changed callback.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null) {
				var view = (ExtendedLabel) e.NewElement;
				UpdateUi (view, Control);
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
				UpdateUi (view, control);
			}
		}


		/// <summary>
		/// Updates the UI.
		/// </summary>
		/// <param name="view">
		/// The view.
		/// </param>
		/// <param name="control">
		/// The control.
		/// </param>
		private void UpdateUi(ExtendedLabel view, TextBlock control)
		{
			if (view == null || control == null)
				return;

			////need to do this ahead of font change due to unexpected behaviour if done later.
			if (view.IsStrikeThrough)
			{             
				//isn't perfect, but it's a start
				var border = new Border
				{
					Height = 2, 
					Width = Control.ActualWidth, 
					Background = control.Foreground, 
					HorizontalAlignment = HorizontalAlignment.Center
				};
				Canvas.SetTop(border, control.FontSize*.72);
				//Canvas.SetTop(border, (this.Control.ActualHeight / 2) - 0.5);
				this.Children.Add(border);

				//// STILL IN DEVELOPMENT === alternative and possibly more flexible grid method - Got stuck making this controls parent the grid below

				//var strikeThroughGrid = new System.Windows.Controls.Grid();
				//strikeThroughGrid.VerticalAlignment = VerticalAlignment.Top;
				//strikeThroughGrid.HorizontalAlignment = HorizontalAlignment.Left;
				////define rows
				//var colDef1 = new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) };
				//var colDef2 = new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) };
				//strikeThroughGrid.RowDefinitions.Add(colDef1);
				//strikeThroughGrid.RowDefinitions.Add(colDef2);

				////add textblock
				//strikeThroughGrid.Children.Add(control);
				//System.Windows.Controls.Grid.SetRowSpan(control,2);

				////add border
				//var strikethroughBorder = new Border{BorderThickness = new System.Windows.Thickness(0,0,0,2) , BorderBrush = control.Foreground , Padding = new System.Windows.Thickness(0,0,0,control.FontSize*0.3)};
				//strikeThroughGrid.Children.Add(strikethroughBorder);

			}

			if (view.IsUnderline)
				control.TextDecorations = TextDecorations.Underline;

			if (view.IsDropShadow) {
				//TODO: Implement dropshadow
			}

		}

	}
}
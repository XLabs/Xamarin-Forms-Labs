using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof (ExtendedLabel), typeof (ExtendedLabelRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Media.Effects;

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

			var view = (ExtendedLabel) Element;
			UpdateUi(view, Control);
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
			if (view.FontSize > 0)
				control.FontSize = (float) view.FontSize;
			//control.FontSize = (view.FontSize > 0) ? (float)view.FontSize : 12.0f;

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

			if (!string.IsNullOrEmpty(view.FontName))
			{
				string filename = view.FontName;
				//if no extension given then assume and add .ttf
				if (filename.LastIndexOf(".", StringComparison.Ordinal) != filename.Length - 4)
				{
					filename = string.Format("{0}.ttf", filename);
				}

			    if (LocalFontFileExists(filename)) //only substitute custom font if exists
			    {
			        control.FontFamily =
			            new FontFamily(string.Format(@"\Assets\Fonts\{0}#{1}", filename,
			                string.IsNullOrEmpty(view.FriendlyFontName)
			                    ? filename.Substring(0, filename.Length - 4)
			                    : view.FriendlyFontName));
			    }
			}

			if (view.IsUnderline)
				control.TextDecorations = TextDecorations.Underline;

			if (view.IsDropShadow)
			{
				control.Effect = 
					new DropShadowEffect
					{
						Color = view.DropShadowColor,
						Direction = 315,
						ShadowDepth = 0,
						Opacity = 0.5
					};
			}
		}

		/// <summary>
		/// Checks if a local resource font file exists
		/// </summary>
		/// <param name="filename">the filename including extension, but not path</param>
		/// <returns></returns>
		private static bool LocalFontFileExists(string filename)
		{
			return
                System.Windows.Application.GetResourceStream(new Uri(string.Format(@"Assets/Fonts/{0}", filename), UriKind.Relative)) != null;            
		}
	}
}
using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.ComponentModel;
	using System.Windows.Controls;

	using Microsoft.Phone.Tasks;

	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	/// Class HyperLinkLabelRenderer.
	/// </summary>
	public class HyperLinkLabelRenderer : ViewRenderer<HyperLinkLabel, HyperlinkButton>
	{
		/// <summary>
		/// The _font applied
		/// </summary>
		private bool _fontApplied;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<HyperLinkLabel> e)
		{
			base.OnElementChanged(e);
			var element = new HyperlinkButton();
			element.Click += (sender, args) =>
				{
					if (Element.NavigateUri.Contains("@"))
					{
						var emailComposeTask = new EmailComposeTask { Subject = Element.Subject, To = "mailto:" + Element.NavigateUri };
						emailComposeTask.Show();
					}
					else if (Element.NavigateUri.Contains("www.") || Element.NavigateUri.Contains("http:"))
					{
						var uri = Element.NavigateUri.StartsWith("http:")
							          ? new Uri(Element.NavigateUri)
							          : new Uri(@"http://" + Element.NavigateUri);

						var webBrowserTask = new WebBrowserTask { Uri = uri };
						webBrowserTask.Show();
					}
				};

			SetNativeControl(element);
			UpdateContent();
			if (Element.BackgroundColor != Color.Default)
			{
				UpdateBackground();
			}
			if (Element.TextColor != Color.Default)
			{
				UpdateTextColor();
			}

			UpdateFont();
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if ((e.PropertyName == HyperLinkLabel.TextProperty.PropertyName))
			{
				UpdateContent();
			}
			else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				UpdateBackground();
			}
			else if (e.PropertyName == HyperLinkLabel.TextColorProperty.PropertyName)
			{
				UpdateTextColor();
			}
			else if (e.PropertyName == HyperLinkLabel.FontProperty.PropertyName)
			{
				UpdateFont();
			}
		}

		/// <summary>
		/// Updates the background.
		/// </summary>
		private void UpdateBackground()
		{
			//  base.Control.Background = (base.Element.BackgroundColor != Color.Default) ? new System.Windows.Media.SolidColorBrush(base.Element.BackgroundColor.ToMediaColor(): ((System.Windows.Media.Brush)Application.Current.Resources["PhoneBackgroundBrush"]);
		}

		/// <summary>
		/// Updates the content.
		/// </summary>
		private void UpdateContent()
		{
			Control.Content = Element.Text;
		}

		/// <summary>
		/// Updates the font.
		/// </summary>
		private void UpdateFont()
		{
			if (((Control != null) && (Element != null)) && ((Element.Font != Font.Default) || _fontApplied))
			{
				Font font = (Element.Font == Font.Default) ? Font.SystemFontOfSize(NamedSize.Medium) : Element.Font;
				Control.ApplyFont(font);
				_fontApplied = true;
			}
		}

		/// <summary>
		/// Updates the color of the text.
		/// </summary>
		private void UpdateTextColor()
		{
			//     base.Control.Foreground = (base.Element.TextColor != Color.Default) ? new System.Windows.Media.SolidColorBrush(base.Element.TextColor.ToMediaColor(): ((System.Windows.Media.Brush)Application.Current.Resources["PhoneForegroundBrush"]);         
		}
	}
}
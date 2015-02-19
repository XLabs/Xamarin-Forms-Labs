using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace XLabs.Forms.Controls
{
	using System.ComponentModel;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;

	using Microsoft.Phone.Controls;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.WinPhone;

	using XLabs.Forms.Extensions;

	using TextAlignment = Xamarin.Forms.TextAlignment;

	/// <summary>
	/// Class ExtendedEntryRenderer.
	/// </summary>
	public class ExtendedEntryRenderer :  EntryRenderer
	{
		/// <summary>
		/// The _this password box
		/// </summary>
		private PasswordBox _thisPasswordBox;
		/// <summary>
		/// The _this phone text box
		/// </summary>
		private PhoneTextBox _thisPhoneTextBox;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;

			//Because Xamarin EntryRenderer switches the type of control we need to find the right one
			if (view.IsPassword)
			{
				_thisPasswordBox = (PasswordBox) Control.Children.FirstOrDefault(c => c is PasswordBox);
			}
			else
			{
				_thisPhoneTextBox = (PhoneTextBox) Control.Children.FirstOrDefault(c => c is PhoneTextBox);
			}

			SetFont(view);
			SetTextAlignment(view);
			SetBorder(view);
			SetPlaceholderTextColor(view);
            SetMaxLength(view);

		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (ExtendedEntry)Element;

			if(e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
				SetFont(view);
			if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
				SetTextAlignment(view);
			if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
				SetBorder(view);
			if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor(view);

		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(ExtendedEntry view)
		{
			if (view.IsPassword && _thisPasswordBox != null)
			{
				_thisPasswordBox.BorderThickness = view.HasBorder ? new System.Windows.Thickness(2) :  new System.Windows.Thickness(0);
			}
			else if (!view.IsPassword && _thisPhoneTextBox != null)
			{
				_thisPhoneTextBox.BorderThickness = view.HasBorder ? new System.Windows.Thickness(2) : new System.Windows.Thickness(0);
			}
		}

		/// <summary>
		/// Sets the text alignment.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetTextAlignment(ExtendedEntry view)
		{
			if (view.IsPassword && _thisPasswordBox != null)
			{
				switch (view.XAlign)
				{
					//NotCurrentlySupported: Text alaignement not available on Windows Phone for Password Entry
				}                
			}
			else if (!view.IsPassword && _thisPhoneTextBox != null)
			{
				switch (view.XAlign)
				{
					case TextAlignment.Center:
						_thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Center;
						break;
					case TextAlignment.End:
						_thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Right;
						break;
					case TextAlignment.Start:
						_thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Left;
						break;
				}              
			}
		}

		/// <summary>
		/// Sets the font.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetFont(ExtendedEntry view)
		{
			if (view.Font != Font.Default)
				if (view.IsPassword)
				{
					if (_thisPasswordBox != null)
					{
						_thisPasswordBox.FontSize = view.Font.GetHeight();
					}
				}
				else
				{
					if (_thisPhoneTextBox != null)
					{
						_thisPhoneTextBox.FontSize = view.Font.GetHeight();
					}
				}
		}

		/// <summary>
		/// Sets the color of the placeholder text.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetPlaceholderTextColor(ExtendedEntry view)
		{
			//the EntryRenderer renders two child controls. A PhoneTextBox or PasswordBox
			// depending on the IsPassword property of the Xamarin form control

			if (view.IsPassword)
			{
				//NotCurrentlySupported: Placeholder text color is not supported on Windows Phone Password control
			}
			else
			{
				if (view.PlaceholderTextColor != Color.Default && _thisPhoneTextBox != null)
				{
					var hintStyle = new System.Windows.Style(typeof(System.Windows.Controls.ContentControl));
					hintStyle.Setters.Add(
						new System.Windows.Setter(
							System.Windows.Controls.Control.ForegroundProperty,
							view.PlaceholderTextColor.ToBrush())
						);
					_thisPhoneTextBox.HintStyle = hintStyle;
				}
			}        
		}

        /// <summary>
        /// Sets the MaxLength Characters.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(ExtendedEntry view)
        {
            if (_thisPhoneTextBox != null) _thisPhoneTextBox.MaxLength = view.MaxLength;
        }
	}
}
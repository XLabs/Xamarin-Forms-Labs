using Xamarin.Forms;

namespace XForms.Toolkit.Controls
{
	public class ToggleButton : ImageButton
	{
		/// <summary>
		/// If the button is selected or not
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create<ToggleButton, bool>(
				p => p.IsSelected, default(bool));

		public bool IsSelected
		{
			get { return (bool)GetValue (IsSelectedProperty); }
			set { SetValue (IsSelectedProperty, value);	}
		}

		#region Selected Values

		/// <summary>
		/// The name of the image without path or file type information for when the button is selected
		/// Android: There should be a drable resource with the same name
		/// iOS: There should be an image in the Resources folder with a build action of BundleResource.
		/// Windows Phone: There should be an image in the Images folder with a type of .png and build action set to resource.
		/// </summary>
		public static readonly BindableProperty SelectedImageProperty =
			BindableProperty.Create<ToggleButton, string>(
				p => p.SelectedImage, default(string));

		public string SelectedImage
		{
			get { return (string)GetValue(SelectedImageProperty); }
			set { SetValue (SelectedImageProperty, value); } 
		}

		public static readonly BindableProperty SelectedTextColorProperty =
			BindableProperty.Create<ToggleButton, Color>(
				p => p.SelectedTextColor, default(Color));

		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue (SelectedTextColorProperty, value); }
		}

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create<ToggleButton, Color>(
				p => p.SelectedBackgroundColor, default(Color));

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue (SelectedBackgroundColorProperty, value); }
		}

		#endregion

		#region Unselected Values

		/// <summary>
		/// The name of the image without path or file type information for when the button is unselected
		/// Android: There should be a drable resource with the same name
		/// iOS: There should be an image in the Resources folder with a build action of BundleResource.
		/// Windows Phone: There should be an image in the Images folder with a type of .png and build action set to resource.
		/// </summary>
		public static readonly BindableProperty UnSelectedImageProperty =
			BindableProperty.Create<ToggleButton, string>(
				p => p.UnSelectedImage, default(string));

		public string UnSelectedImage
		{
			get { return (string)GetValue(UnSelectedImageProperty); }
			set { SetValue (UnSelectedImageProperty, value); }
		}

		public static readonly BindableProperty UnSelectedTextColorProperty =
			BindableProperty.Create<ToggleButton, Color>(
				p => p.UnSelectedTextColor, default(Color));

		public Color UnSelectedTextColor
		{
			get { return (Color)GetValue(UnSelectedTextColorProperty); }
			set { SetValue (UnSelectedTextColorProperty, value); }
		}

		public static readonly BindableProperty UnSelectedBackgroundColorProperty =
			BindableProperty.Create<ToggleButton, Color>(
				p => p.UnSelectedBackgroundColor, default(Color));

		public Color UnSelectedBackgroundColor
		{
			get { return (Color)GetValue(UnSelectedBackgroundColorProperty); }
			set { SetValue (UnSelectedBackgroundColorProperty, value); }
		}

		#endregion

		public ToggleButton()
		{
			this.Clicked += HandleClicked;
		}

		private void HandleClicked (object sender, System.EventArgs e)
		{
			this.IsSelected = !this.IsSelected;
		}
	}
}


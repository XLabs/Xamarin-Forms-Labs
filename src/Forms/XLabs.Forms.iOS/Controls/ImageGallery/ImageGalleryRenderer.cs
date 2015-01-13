using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace XLabs.Forms.Controls
{
	using System.Collections.ObjectModel;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ImageGalleryRenderer.
	/// </summary>
	public class ImageGalleryRenderer : ViewRenderer<ImageGallery,ImageGalleryView>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageGalleryRenderer"/> class.
		/// </summary>
		public ImageGalleryRenderer ()
		{


		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<ImageGallery> e)
		{
			base.OnElementChanged (e);

			var imageGalleryView = new ImageGalleryView (e.NewElement.ItemsSource as ObservableCollection<string>);
			Bind (e.NewElement);
			SetNativeControl(imageGalleryView);

		}
		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(ImageGallery newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;

			}
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{

			}
		}
		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{

			}
		}

	}
}
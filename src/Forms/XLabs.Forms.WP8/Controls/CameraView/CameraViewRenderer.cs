using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace XLabs.Forms.Controls
{
	using System.Windows.Controls;
	using System.Windows.Media;

	using Microsoft.Devices;

	using Xamarin.Forms.Platform.WinPhone;

	using XLabs.Ioc;
	using XLabs.Platform.Mvvm;

	using Orientation = XLabs.Enums.Orientation;

	/// <summary>
	/// Class CameraViewRenderer.
	/// </summary>
	public class CameraViewRenderer : ViewRenderer<CameraView, Canvas>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			System.Diagnostics.Debug.WriteLine(Parent);

			if (Control == null)
			{
				
				// TODO: determine how to dispose the camera...
				var camera = new PhotoCamera((CameraType)((int)e.NewElement.Camera));

				var app = Resolver.Resolve<IXFormsApp>();

				var rotation = camera.Orientation;
				switch (app.Orientation)
				{
					case Orientation.LandscapeLeft:
						rotation -= 90;
						break;
					case Orientation.LandscapeRight:
						rotation += 90;
						break;
				}

				var videoBrush = new VideoBrush {
					RelativeTransform = new CompositeTransform()
					{
						CenterX = 0.5,
						CenterY = 0.5,
						Rotation = rotation
					}
				};
				
				
				videoBrush.SetSource(camera);

				var canvas = new Canvas
				{
					Background = videoBrush
				};

				SetNativeControl(canvas);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			switch (e.PropertyName)
			{
				case "Camera":
					var brush = Control.Background as VideoBrush;
					var camera = new PhotoCamera((CameraType)((int)Element.Camera));
					brush.SetSource(camera);
					break;
				default:
					break;
			}
		}
	}
}

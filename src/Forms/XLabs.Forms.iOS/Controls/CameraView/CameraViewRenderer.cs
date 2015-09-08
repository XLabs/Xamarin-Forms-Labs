using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace XLabs.Forms.Controls
{
	using CoreGraphics;

	using AVFoundation;
	using Foundation;
	using UIKit;

	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class CameraViewRenderer.
	/// </summary>
	public class CameraViewRenderer : ViewRenderer<CameraView, CameraPreview>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			System.Diagnostics.Debug.WriteLine("Testing CameraView");
			base.OnElementChanged(e);

			if (Control == null)
			{
				SetNativeControl(new CameraPreview());
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
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Class CameraPreview.
	/// </summary>
	[Register("CameraPreview")]
	public class CameraPreview : UIView
	{
		/// <summary>
		/// The _preview layer
		/// </summary>
		private AVCaptureVideoPreviewLayer _previewLayer;

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraPreview"/> class.
		/// </summary>
		public CameraPreview()
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraPreview"/> class.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		public CameraPreview(CGRect bounds)
			: base(bounds)
		{
			Initialize();
		}

		/// <summary>
		/// Draws the specified rect.
		/// </summary>
		/// <param name="rect">The rect.</param>
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			_previewLayer.Frame = rect;
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		private void Initialize()
		{
			var captureSession = new AVCaptureSession();
			_previewLayer = new AVCaptureVideoPreviewLayer(captureSession)
				{
					VideoGravity = AVLayerVideoGravity.ResizeAspectFill,
					Frame = Bounds
				};

			var device = AVCaptureDevice.DefaultDeviceWithMediaType(
				AVMediaType.Video);

			if (device == null)
			{
				System.Diagnostics.Debug.WriteLine("No device detected.");
				return;
			}

			NSError error;

			var input = new AVCaptureDeviceInput(device, out error);

			captureSession.AddInput(input);

			Layer.AddSublayer(_previewLayer);

			captureSession.StartRunning();
		}
	}
}
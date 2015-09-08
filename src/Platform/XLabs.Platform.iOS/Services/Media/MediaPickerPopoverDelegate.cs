namespace XLabs.Platform.Services.Media
{
	using UIKit;

	/// <summary>
	/// Class MediaPickerPopoverDelegate.
	/// </summary>
	internal class MediaPickerPopoverDelegate : UIPopoverControllerDelegate
	{
		/// <summary>
		/// The _picker
		/// </summary>
		private readonly UIImagePickerController _picker;

		/// <summary>
		/// The _picker delegate
		/// </summary>
		private readonly MediaPickerDelegate _pickerDelegate;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerPopoverDelegate"/> class.
		/// </summary>
		/// <param name="pickerDelegate">The picker delegate.</param>
		/// <param name="picker">The picker.</param>
		internal MediaPickerPopoverDelegate(MediaPickerDelegate pickerDelegate, UIImagePickerController picker)
		{
			_pickerDelegate = pickerDelegate;
			_picker = picker;
		}

		/// <summary>
		/// Shoulds the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool ShouldDismiss(UIPopoverController popoverController)
		{
			return true;
		}

		/// <summary>
		/// Dids the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		public override void DidDismiss(UIPopoverController popoverController)
		{
			_pickerDelegate.Canceled(_picker);
		}
	}
}
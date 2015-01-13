namespace XLabs.Forms.Controls
{
	using CoreGraphics;

	using UIKit;

	/// <summary>
	/// Class NoCaretField.
	/// </summary>
	public class NoCaretField : UITextField
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NoCaretField"/> class.
		/// </summary>
		public NoCaretField() : base(default(CGRect))
		{
		}
		/// <summary>
		/// Gets the caret rect for position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>RectangleF.</returns>
		public override CGRect GetCaretRectForPosition(UITextPosition position)
		{
			return default(CGRect);
		}
	}
}


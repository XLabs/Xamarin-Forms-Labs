namespace XLabs.Forms.Controls
{
	using CoreGraphics;

	using UIKit;

	/// <summary>
	/// Class CalendarArrowView.
	/// </summary>
	public class CalendarArrowView : UIButton
	{
		/// <summary>
		/// Enum ArrowDirection
		/// </summary>
		public enum ArrowDirection {
			/// <summary>
			/// The right arrow direction.
			/// </summary>
			Right,
            /// <summary>
            /// The left arrow direction.
            /// </summary>
            Left
		}

		/// <summary>
		/// The _arrow direction
		/// </summary>
		private ArrowDirection _arrowDirection = ArrowDirection.Left;

        /// <summary>
        /// The _color
        /// </summary>
        private UIColor _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarArrowView"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public CalendarArrowView(CGRect frame)
        {
            Frame = frame;
            UserInteractionEnabled = true;
            BackgroundColor = UIColor.Clear;
        }

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public ArrowDirection Direction
        {
			get
            {
				return _arrowDirection;
			}
			set
            {
				_arrowDirection = value;
				SetBackgroundImage(GenerateImageForButton(Frame), UIControlState.Normal);
				SetNeedsDisplay();
			}
		}


		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <value>The color.</value>
		public UIColor Color
        {
			set
            {
				_color = value;
				SetNeedsDisplay();
			}
		}

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

        }

		/// <summary>
		/// Generates the image for button.
		/// </summary>
		/// <param name="rect">The rect.</param>
		/// <returns>UIImage.</returns>
		private UIImage GenerateImageForButton(CGRect rect)
        {
			UIImage image;

			if(_arrowDirection == ArrowDirection.Left)
			{
				image = UIImage.FromBundle("Images/Calendar/arrow_white_left.png");
			}
			else
			{
				image = UIImage.FromBundle("Images/Calendar/arrow_white_right.png");
			}

			return image;
		}
	}
}


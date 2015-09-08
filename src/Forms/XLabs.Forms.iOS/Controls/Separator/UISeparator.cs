namespace XLabs.Forms.Controls
{
	using System;

	using UIKit;

	/// <summary>
	/// Class UISeparator.
	/// </summary>
	public class UISeparator : UIView
	{

		/// <summary>
		/// The _thickness
		/// </summary>
		private double _thickness;

		/// <summary>
		/// Gets or sets the thickness.
		/// </summary>
		/// <value>The thickness.</value>
		public double Thickness
		{
			set
			{
				_thickness = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _thickness;
			}
		}

		/// <summary>
		/// The _spacing before
		/// </summary>
		private double _spacingBefore;

		/// <summary>
		/// Gets or sets the spacing before.
		/// </summary>
		/// <value>The spacing before.</value>
		public double SpacingBefore
		{
			set
			{
				_spacingBefore = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _spacingBefore;
			}
		}

		/// <summary>
		/// The _spacing after
		/// </summary>
		private double _spacingAfter;

		/// <summary>
		/// Gets or sets the spacing after.
		/// </summary>
		/// <value>The spacing after.</value>
		public double SpacingAfter
		{
			set
			{
				_spacingAfter = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _spacingAfter;
			}
		}

		/// <summary>
		/// The _stroke color
		/// </summary>
		private UIColor _strokeColor;

		/// <summary>
		/// Gets or sets the color of the stroke.
		/// </summary>
		/// <value>The color of the stroke.</value>
		public UIColor StrokeColor
		{
			set
			{
				_strokeColor = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _strokeColor;
			}
		}


		/// <summary>
		/// The _stroke type
		/// </summary>
		private StrokeType _strokeType;

		/// <summary>
		/// Gets or sets the type of the stroke.
		/// </summary>
		/// <value>The type of the stroke.</value>
		public StrokeType StrokeType
		{
			set
			{
				_strokeType = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _strokeType;
			}
		}

		/// <summary>
		/// The _orientation
		/// </summary>
		private SeparatorOrientation _orientation;

		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public SeparatorOrientation Orientation
		{
			set
			{
				_orientation = value;
				SetNeedsDisplayInRect(Bounds);
			}
			get
			{
				return _orientation;
			}
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		void Initialize()
		{
			BackgroundColor = UIColor.Clear;
			Opaque = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UISeparator"/> class.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		public UISeparator(CoreGraphics.CGRect bounds)
			: base(bounds)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UISeparator"/> class.
		/// </summary>
		/// <param name="handle">The handle.</param>
		public UISeparator(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UISeparator"/> class.
		/// </summary>
		public UISeparator()
		{
			Initialize();
		}

		/// <summary>
		/// Draws the specified rect.
		/// </summary>
		/// <param name="rect">The rect.</param>
		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);


			var height = Bounds.Size.Height;
			//var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;

			var context = UIGraphics.GetCurrentContext();

			context.ClearRect(rect);
			//context.SetFillColor(UIColor.Clear.CGColor);
			//context.FillRect(rect);
			context.SetStrokeColor(StrokeColor.CGColor);
			switch (StrokeType)
			{
				case StrokeType.Dashed:
					context.SetLineDash(0, new nfloat[] { 6, 2 });
					break;
				case StrokeType.Dotted:
					context.SetLineDash(0, new nfloat[] { (nfloat)Thickness, (nfloat)Thickness });
					break;
				default:

					break;
			}

			context.SetLineWidth((float)Thickness);
			var desiredTotalSpacing = SpacingAfter + SpacingBefore;

			float leftForSpacing = 0;
			float actualSpacingBefore = 0;
			float actualSpacingAfter = 0;

			if (Orientation == SeparatorOrientation.Horizontal)
			{
				leftForSpacing = (float)Bounds.Size.Height - (float)Thickness;
			}
			else
			{
				leftForSpacing = (float)Bounds.Size.Width - (float)Thickness;
			}
			if (desiredTotalSpacing > 0)
			{
				float spacingCompressionRatio = (float)(leftForSpacing / desiredTotalSpacing);
				actualSpacingBefore = (float)SpacingBefore * spacingCompressionRatio;
				actualSpacingAfter = (float)SpacingAfter * spacingCompressionRatio;
			}
			else
			{
				actualSpacingBefore = 0;
				actualSpacingAfter = 0;
			}
			float thicknessOffset = (float)Thickness / 2.0f;

			if (Orientation == SeparatorOrientation.Horizontal)
			{
				var half = Bounds.Size.Height / 2.0f;
				context.MoveTo(0, actualSpacingBefore + thicknessOffset);
				context.AddLineToPoint(rect.Width, actualSpacingBefore + thicknessOffset);
			}
			else
			{
				var half = Bounds.Size.Width / 2.0f;
				context.MoveTo(actualSpacingBefore + thicknessOffset, 0);
				context.AddLineToPoint(actualSpacingBefore + thicknessOffset, rect.Height);
			}
			context.StrokePath();
		}
	}

}


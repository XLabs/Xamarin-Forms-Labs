//http://forums.xamarin.com/discussion/19351/how-to-achieve-synchronized-scroll-views
//using and extending on msmith implementation

using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedScrollView.
	/// </summary>
	public class ExtendedScrollView : ScrollView
	{
		/// <summary>
		/// Occurs when [scrolled].
		/// </summary>
		public event Action<ScrollView, Rectangle> Scrolled;

		/// <summary>
		/// Updates the bounds.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		public void UpdateBounds(Rectangle bounds)
		{
			Position = bounds.Location;
			if (Scrolled != null)
				Scrolled (this, bounds);
		}

		/// <summary>
		/// The position property
		/// </summary>
		public static readonly BindableProperty PositionProperty = 
			BindableProperty.Create<ExtendedScrollView,Point>(
				p => p.Position, default(Point));

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Point Position {
			get { return (Point)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		/// <summary>
		/// The animate scroll property
		/// </summary>
		public static readonly BindableProperty AnimateScrollProperty = 
			BindableProperty.Create<ExtendedScrollView,bool>(
				p => p.AnimateScroll,true);

		/// <summary>
		/// Gets or sets a value indicating whether [animate scroll].
		/// </summary>
		/// <value><c>true</c> if [animate scroll]; otherwise, <c>false</c>.</value>
		public bool AnimateScroll {
			get { return (bool)GetValue (AnimateScrollProperty); }
			set { SetValue (AnimateScrollProperty, value); }
		}

	}
}


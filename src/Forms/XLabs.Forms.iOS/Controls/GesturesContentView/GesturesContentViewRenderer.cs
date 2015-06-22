using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(GesturesContentView),typeof(GesturesContentViewRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Collections.Generic;

	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using XLabs.Forms.Behaviors;

	/// <summary>
	/// Implements the renderer required by <see cref="GesturesContentView"/>
	/// </summary>
	/// Element created at 07/11/2014,3:29 PM by Charles
	public class GesturesContentViewRenderer : ViewRenderer<GesturesContentView,UIView>
	{
		/// <summary>The List of recoginizers to be attached/detached to views as we go along</summary>
		/// Element created at 07/11/2014,3:30 PM by Charles
		private readonly List<UIGestureRecognizer> _recognizers= new List<UIGestureRecognizer>();

		/// <summary>Swipre supports all diretions</summary>
		/// Element created at 07/11/2014,3:30 PM by Charles
		private const UISwipeGestureRecognizerDirection ALL_DIRECTIONS = UISwipeGestureRecognizerDirection.Down | UISwipeGestureRecognizerDirection.Up | UISwipeGestureRecognizerDirection.Left | UISwipeGestureRecognizerDirection.Right;

		/// <summary>Store the start point of a swipe gesture</summary>
		/// Element created at 07/11/2014,3:31 PM by Charles
		private Point _swipeStart;

		/// <summary>
		/// Initializes a new instance of the <see cref="GesturesContentViewRenderer"/> class.
		/// </summary>
		/// Element created at 07/11/2014,3:31 PM by Charles
		public GesturesContentViewRenderer()
		{    
			// Single Tap
			_recognizers.Add(new UITapGestureRecognizer((x) =>
				{
					if (x.State == UIGestureRecognizerState.Ended)
					{
						var viewpoint = x.LocationInView(this);
						Element.ProcessGesture(new GestureResult { GestureType = GestureType.SingleTap, Direction = Directionality.None, Origin = viewpoint.ToPoint() });                        
					}
				}){NumberOfTapsRequired = 1});
			//Double Tap
			_recognizers.Add(new UITapGestureRecognizer((x) =>
				{
					if (x.State == UIGestureRecognizerState.Ended)
					{
						var viewpoint = x.LocationInView(this);
						Element.ProcessGesture(new GestureResult { GestureType = GestureType.DoubleTap, Direction = Directionality.None, Origin = viewpoint.ToPoint() });                        
					}
				}) { NumberOfTapsRequired = 2 });
			// Longpress
			_recognizers.Add(new UILongPressGestureRecognizer((x) =>
				{
					if (x.State == UIGestureRecognizerState.Ended)
					{
						var viewpoint = x.LocationInView(this);
						Element.ProcessGesture(new GestureResult { GestureType = GestureType.LongPress, Direction = Directionality.None, Origin = viewpoint.ToPoint() });                        
					}
				}));
			// Swipe
			_recognizers.Add(new UISwipeGestureRecognizer((x) =>
				{
					if (x.State == UIGestureRecognizerState.Began)
					{
						_swipeStart = x.LocationInView(this).ToPoint();
					}
					if (x.State == UIGestureRecognizerState.Ended)
					{
						var endpoint = x.LocationInView(this).ToPoint();
						var distance = _swipeStart.Distance(endpoint);
						var distanceX = Math.Abs(_swipeStart.X - endpoint.X);
						var distanceY = Math.Abs(_swipeStart.Y - endpoint.Y);
						var direction = Directionality.None;
						direction |= ((x.Direction & UISwipeGestureRecognizerDirection.Left) == UISwipeGestureRecognizerDirection.Left ? Directionality.Left : Directionality.None);
						direction |= ((x.Direction & UISwipeGestureRecognizerDirection.Right) == UISwipeGestureRecognizerDirection.Right ? Directionality.Right : Directionality.None);
						direction |= ((x.Direction & UISwipeGestureRecognizerDirection.Up) == UISwipeGestureRecognizerDirection.Up ? Directionality.Up : Directionality.None);
						direction |= ((x.Direction & UISwipeGestureRecognizerDirection.Down) == UISwipeGestureRecognizerDirection.Down ? Directionality.Down : Directionality.None);

						Element.ProcessGesture(new GestureResult { Direction = direction, GestureType = GestureType.Swipe, HorizontalDistance = distanceX, VerticalDistance = distanceY, Origin = _swipeStart, Length = distance });
					}
				}){ Direction = ALL_DIRECTIONS,NumberOfTouchesRequired = 1});
		}

		/// <summary>When the underlying element is changed we detach from the old, and attach to the new</summary>
		/// <param name="e">The <see cref="ElementChangedEventArgs{GesturesContentView}"/> instance containing the event data.</param>
		/// Element created at 07/11/2014,3:31 PM by Charles
		protected override void OnElementChanged(ElementChangedEventArgs<GesturesContentView> e)
		{
			if (e.NewElement == null)
			{
				foreach(var uir in _recognizers)
					RemoveGestureRecognizer(uir);
			}
			if (e.OldElement == null)
			{
				foreach(var uir in _recognizers)
					AddGestureRecognizer(uir);
			}
			base.OnElementChanged(e);
		}
	}
}

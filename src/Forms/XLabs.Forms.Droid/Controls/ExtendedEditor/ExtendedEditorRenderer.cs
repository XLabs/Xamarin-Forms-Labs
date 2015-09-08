using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace XLabs.Forms.Controls
{
	using System;

	using Android.Views;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	/// <summary>
	/// Class ExtendedEditorRenderer.
	/// </summary>
	public class ExtendedEditorRenderer : EditorRenderer
	{
		/// <summary>
		/// The mi n_ distance
		/// </summary>
		private const int MIN_DISTANCE = 10;
		/// <summary>
		/// The _down x
		/// </summary>
		private float _downX, _downY, _upX, _upY;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEditor)Element;

			// TODO: Set font

			if (e.NewElement == null)
			{
				this.Touch -= HandleTouch;
			}

			if (e.OldElement == null)
			{
				this.Touch += HandleTouch;
			}
		}

		/// <summary>
		/// Handles the touch.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Android.Views.View.TouchEventArgs"/> instance containing the event data.</param>
		void HandleTouch (object sender, Android.Views.View.TouchEventArgs e)
		{
			var element = this.Element as ExtendedEditor;
			switch (e.Event.Action)
			{
				case MotionEventActions.Down:
					_downX = e.Event.GetX();
					_downY = e.Event.GetY();
					return;
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
				case MotionEventActions.Move:
					_upX = e.Event.GetX();
					_upY = e.Event.GetY();

					float deltaX = _downX - _upX;
					float deltaY = _downY - _upY;

					// swipe horizontal?
					if(Math.Abs(deltaX) > Math.Abs(deltaY))
					{
						if(Math.Abs(deltaX) > MIN_DISTANCE)
						{
							// left or right
							if(deltaX < 0) { element.OnRightSwipe(this, EventArgs.Empty); return; }
							if(deltaX > 0) { element.OnLeftSwipe(this, EventArgs.Empty); return; }
						}
						else 
						{
							Android.Util.Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MIN_DISTANCE);
							return; // We don't consume the event
						}
					}
					// swipe vertical?
					//                    else 
					//                    {
					//                        if(Math.abs(deltaY) > MIN_DISTANCE){
					//                            // top or down
					//                            if(deltaY < 0) { this.onDownSwipe(); return true; }
					//                            if(deltaY > 0) { this.onUpSwipe(); return true; }
					//                        }
					//                        else {
					//                            Log.i(logTag, "Vertical Swipe was only " + Math.abs(deltaX) + " long, need at least " + MIN_DISTANCE);
					//                            return false; // We don't consume the event
					//                        }
					//                    }

					return;
			}
		}

	}
}


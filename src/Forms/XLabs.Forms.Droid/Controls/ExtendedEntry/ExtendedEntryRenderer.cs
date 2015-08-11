using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace XLabs.Forms.Controls
{
    using System;

    using Android.Views;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using XLabs.Forms.Extensions;
    using Android.Text;

    /// <summary>
    /// Class ExtendedEntryRenderer.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
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
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;

            SetFont(view);
            SetTextAlignment(view);
            //SetBorder(view);
            SetPlaceholderTextColor(view);
            SetMaxLength(view);

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
            var element = this.Element as ExtendedEntry;
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

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            //if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
            //    SetBorder(view);
            if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetBorder(ExtendedEntry view)
        {
            //NotCurrentlySupported: HasBorder peroperty not suported on Android
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(ExtendedEntry view)
        {
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(ExtendedEntry view)
        {
            if(view.Font != Font.Default) {
                Control.TextSize = view.Font.ToScaledPixel();
                Control.Typeface = view.Font.ToExtendedTypeface(Context);
            }
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(ExtendedEntry view){
            if(view.PlaceholderTextColor != Color.Default) 
                Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());			
        }

        /// <summary>
        /// Sets the MaxLength characteres.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(ExtendedEntry view)
        {
            Control.SetFilters(new IInputFilter[] { new global::Android.Text.InputFilterLengthFilter(view.MaxLength) });
        }
    }
}


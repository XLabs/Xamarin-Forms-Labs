using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTimePicker), typeof(ExtendedTimePickerRenderer))]

namespace XLabs.Forms.Controls
{
    using System;
    using System.ComponentModel;
    using CoreGraphics;

    using Foundation;
    using UIKit;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// Class ExtendedTimePickerRenderer.
    /// </summary>
    public class ExtendedTimePickerRenderer : ViewRenderer<ExtendedTimePicker, UITextField>
    {
        /// <summary>
        /// The _picker
        /// </summary>
        UIDatePicker _picker;
        /// <summary>
        /// The _pop over
        /// </summary>
        UIPopoverController _popOver;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged (ElementChangedEventArgs<ExtendedTimePicker> e)
        {
            base.OnElementChanged (e);

            if (e.OldElement != null)
            {
                // todo: handle this scenario properly
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var entry = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };
                    entry.Started += this.OnStarted;
                    entry.Ended += this.OnEnded;
                    this._picker = new UIDatePicker {
                        Mode = UIDatePickerMode.Time,
                        TimeZone = new NSTimeZone ("UTC")
                    };

                    nfloat width = UIScreen.MainScreen.Bounds.Width;
                    var uIToolbar = new UIToolbar (new CGRect (0, 0, width, 44)) {
                        BarStyle = UIBarStyle.Default,
                        Translucent = true
                    };

                    var uIBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);
                    var uIBarButtonItem2 = new UIBarButtonItem (
                        UIBarButtonSystemItem.Done,
                        delegate
                        {
                            entry.ResignFirstResponder ();
                        });

                    uIToolbar.SetItems (new[] { uIBarButtonItem, uIBarButtonItem2 }, false);

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        entry.InputView = this._picker;
                        entry.InputAccessoryView = uIToolbar;
                    }
                    else
                    {
                        entry.InputView = new UIView (CGRect.Empty);
                        entry.InputAccessoryView = new UIView (CGRect.Empty);
                    }

                    this._picker.ValueChanged += this.HandleValueChanged;
                    SetNativeControl (entry);
                }
            }
          
            UpdateTime();

            SetBorder();
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName || e.PropertyName == TimePicker.FormatProperty.PropertyName) 
            {
                UpdateTime();
            }
            else if (e.PropertyName == ExtendedTimePicker.HasBorderProperty.PropertyName)
            {
                SetBorder();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Ended" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnEnded (object sender, EventArgs eventArgs)
        {
            //base.Element.IsFocused = false;
        }

        /// <summary>
        /// Handles the <see cref="E:Started" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnStarted (object sender, EventArgs eventArgs)
        {
            //base.Element.IsFocused = true;

            if (Device.Idiom == TargetIdiom.Phone) return;

            var vc = new UIViewController {this._picker};
            vc.View.Frame = new CGRect (0, 0, 320, 200);
            vc.PreferredContentSize = new CGSize (320, 200);
            this._popOver = new UIPopoverController (vc);
            this._popOver.PresentFromRect(
                new CGRect(this.Control.Frame.Width/2, this.Control.Frame.Height-3, 0, 0), 
                this.Control, 
                UIPopoverArrowDirection.Any, 
                true);

            this._popOver.DidDismiss += (s, e) => 
            {
                this._popOver = null;
                this.Control.ResignFirstResponder();
            };
        }

        /// <summary>
        /// Updates the time.
        /// </summary>
        private void UpdateTime ()
        {
            if (this.Element == null || Control == null) return;

            this._picker.Date = new DateTime (1, 1, 1).Add (this.Element.Time).ToNSDate ();
            this.Control.Text = DateTime.Today.Add (this.Element.Time).ToString (this.Element.Format);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        private void SetBorder()
        {
            if (this.Element == null) return;

            this.Control.BorderStyle = this.Element.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        //
        // Methods
        //
        /// <summary>
        /// Handles the value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HandleValueChanged(object sender, EventArgs e)
        {
            if (this.Element == null) return;

            this.Element.Time = this._picker.Date.ToDateTime() - new DateTime(1, 1, 1);
        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            if (disposing && this._picker != null)
            {
                this._picker.ValueChanged -= this.HandleValueChanged;
                this._picker.Dispose ();
                this._picker = null;
                if (this._popOver != null)
                {
                    this._popOver.Dispose ();
                    this._popOver = null;
                }
                if (Control != null)
                {
                    Control.Started -= this.OnStarted;
                    Control.Ended -= this.OnEnded;
                }

            }
        }
    }
}


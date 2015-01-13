using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]

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
	/// Class ExtendedDatePickerRenderer.
	/// </summary>
	public class ExtendedDatePickerRenderer : ViewRenderer<ExtendedDatePicker, UITextField>
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
		/// Initializes a new instance of the <see cref="ExtendedDatePickerRenderer"/> class.
		/// </summary>
		public ExtendedDatePickerRenderer ()
		{
		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(ExtendedDatePicker view)
		{
			Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}

		//
		// Methods
		//
		/// <summary>
		/// Handles the value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void HandleValueChanged (object sender, EventArgs e)
		{
			Element.Date = _picker.Date.ToDateTime ();
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<ExtendedDatePicker> e)
		{
			base.OnElementChanged (e);
			NoCaretField entry = new NoCaretField {
				BorderStyle = UITextBorderStyle.RoundedRect
			};
			entry.Started += new EventHandler (OnStarted);
			entry.Ended += new EventHandler (OnEnded);
			_picker = new UIDatePicker {
				Mode = UIDatePickerMode.Date,
				TimeZone = new NSTimeZone ("UTC")
			};
			nfloat width = UIScreen.MainScreen.Bounds.Width;
			UIToolbar uIToolbar = new UIToolbar (new CGRect (0, 0, width, 44)) {
				BarStyle = UIBarStyle.Default,
				Translucent = true
			};
			UIBarButtonItem uIBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);
			UIBarButtonItem uIBarButtonItem2 = new UIBarButtonItem (UIBarButtonSystemItem.Done, delegate (object o, EventArgs a) {
				entry.ResignFirstResponder ();
			});
			uIToolbar.SetItems (new UIBarButtonItem[] {
				uIBarButtonItem,
				uIBarButtonItem2
			}, false);

			if (Device.Idiom == TargetIdiom.Phone) {
				entry.InputView = _picker;
				entry.InputAccessoryView = uIToolbar;
			} else {
				entry.InputView = new UIView (CGRect.Empty);
				entry.InputAccessoryView = new UIView (CGRect.Empty);
			}

			SetNativeControl (entry);
			UpdateDateFromModel (false);
			UpdateMaximumDate ();
			UpdateMinimumDate ();
			_picker.ValueChanged += new EventHandler (HandleValueChanged);

			var view = (ExtendedDatePicker)Element;

			SetBorder(view);
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName) {
				UpdateDateFromModel (true);
				return;
			}
			if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName) {
				UpdateMinimumDate ();
				return;
			}
			if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName) {
				UpdateMaximumDate ();
			}

			var view = (ExtendedDatePicker)Element;

			if (e.PropertyName == ExtendedTimePicker.HasBorderProperty.PropertyName)
				SetBorder(view);
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

			if (Device.Idiom != TargetIdiom.Phone) {
				var vc = new UIViewController ();
				vc.Add (_picker);
				vc.View.Frame = new CGRect (0, 0, 320, 200);
				vc.PreferredContentSize = new CGSize (320, 200);
				_popOver = new UIPopoverController (vc);
				_popOver.PresentFromRect(new CGRect(Control.Frame.Width/2,Control.Frame.Height-3,0,0), Control, UIPopoverArrowDirection.Any, true);
				_popOver.DidDismiss += (object s, EventArgs e) => {
					_popOver = null;
					Control.ResignFirstResponder();
				};
			}
		}

		/// <summary>
		/// Updates the date from model.
		/// </summary>
		/// <param name="animate">if set to <c>true</c> [animate].</param>
		private void UpdateDateFromModel (bool animate)
		{
			_picker.SetDate (Element.Date.ToNSDate (), animate);
			Control.Text = Element.Date.ToString (Element.Format);
		}

		/// <summary>
		/// Updates the maximum date.
		/// </summary>
		private void UpdateMaximumDate ()
		{
			_picker.MaximumDate = Element.MaximumDate.ToNSDate ();
		}

		/// <summary>
		/// Updates the minimum date.
		/// </summary>
		private void UpdateMinimumDate ()
		{
			_picker.MinimumDate = Element.MinimumDate.ToNSDate ();
		}
	}
}


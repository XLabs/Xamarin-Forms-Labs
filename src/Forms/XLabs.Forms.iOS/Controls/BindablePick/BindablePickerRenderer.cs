using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using CoreGraphics;
	using System.Linq;

	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class BindablePickerRenderer.
	/// </summary>
	public class BindablePickerRenderer : ViewRenderer<BindablePicker, UITextField>
	{
		/// <summary>
		/// Class PickerSource.
		/// </summary>
		private class PickerSource : UIPickerViewModel
		{
			/// <summary>
			/// The _model
			/// </summary>
			private readonly BindablePicker _model;
			/// <summary>
			/// Occurs when [value changed].
			/// </summary>
			public event EventHandler ValueChanged;
			/// <summary>
			/// Gets or sets the selected item.
			/// </summary>
			/// <value>The selected item.</value>
			public string SelectedItem
			{
				get;
				internal set;
			}
			/// <summary>
			/// Gets or sets the index of the selected.
			/// </summary>
			/// <value>The index of the selected.</value>
			public int SelectedIndex
			{
				get;
				internal set;
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="PickerSource"/> class.
			/// </summary>
			/// <param name="model">The model.</param>
			public PickerSource(BindablePicker model)
			{
				_model = model;
			}
			/// <summary>
			/// Gets the rows in component.
			/// </summary>
			/// <param name="picker">The picker.</param>
			/// <param name="component">The component.</param>
			/// <returns>System.Int32.</returns>
			public override nint GetRowsInComponent(UIPickerView picker, nint component)
			{
				if (_model.Items == null)
				{
					return 0;
				}
				return _model.Items.Count;
			}
			/// <summary>
			/// Gets the component count.
			/// </summary>
			/// <param name="picker">The picker.</param>
			/// <returns>System.Int32.</returns>
			public override nint GetComponentCount(UIPickerView picker)
			{
				return 1;
			}
			/// <summary>
			/// Gets the title.
			/// </summary>
			/// <param name="picker">The picker.</param>
			/// <param name="row">The row.</param>
			/// <param name="component">The component.</param>
			/// <returns>System.String.</returns>
			public override string GetTitle(UIPickerView picker, nint row, nint component)
			{
				return _model.Items[(int)row];
			}
			/// <summary>
			/// Selecteds the specified picker.
			/// </summary>
			/// <param name="picker">The picker.</param>
			/// <param name="row">The row.</param>
			/// <param name="component">The component.</param>
			public override void Selected(UIPickerView picker, nint row, nint component)
			{
				SelectedItem = _model.Items[(int)row];
				SelectedIndex = (int)row;
				EventHandler valueChanged = ValueChanged;
				if (valueChanged != null)
				{
					valueChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// The _picker
		/// </summary>
		private UIPickerView _picker;
		/// <summary>
		/// The _pop over
		/// </summary>
		private UIPopoverController _popOver;
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<BindablePicker> e)
		{
			((ObservableCollection<string>)e.NewElement.Items).CollectionChanged += new NotifyCollectionChangedEventHandler (RowsCollectionChanged);
			NoCaretField entry = new NoCaretField {
				BorderStyle = e.NewElement.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None
			};
			entry.Started += new EventHandler (OnStarted);
			entry.Ended += new EventHandler (OnEnded);
			_picker = new UIPickerView {
				DataSource = new PickerSource (e.NewElement)
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

			((PickerSource)_picker.DataSource).ValueChanged += new EventHandler (HandleValueChanged);
			SetNativeControl (entry);
			UpdatePicker ();
		}

		/// <summary>
		/// Handles the <see cref="E:Ended" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void OnEnded(object sender, EventArgs eventArgs)
		{
			//base.Element.IsFocused = false;
			if (Device.Idiom != TargetIdiom.Phone) {

			}
		}

		/// <summary>
		/// Handles the <see cref="E:Started" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void OnStarted(object sender, EventArgs eventArgs)
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
		/// Rowses the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void RowsCollectionChanged(object sender, EventArgs e)
		{
			UpdatePicker();
		}

		/// <summary>
		/// Handles the value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void HandleValueChanged(object sender, EventArgs e)
		{
			Element.SelectedIndex = ((PickerSource)sender).SelectedIndex;
			Control.Text = ((PickerSource)sender).SelectedItem;
		}

		/// <summary>
		/// Updates the picker.
		/// </summary>
		private void UpdatePicker()
		{
			Control.Placeholder = Element.Title;
			Control.Text = (Element.SelectedIndex <= -1 || Element.Items == null) ? "" : Element.Items[Element.SelectedIndex];
			_picker.ReloadAllComponents();
			if (Element.SelectedIndex > -1 && Element.Items != null && Enumerable.Any<string>(Element.Items))
			{
				_picker.Select(Element.SelectedIndex, 0, true);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Picker.TitleProperty.PropertyName)
			{
				UpdatePicker();
			}
			if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
			{
				UpdatePicker();
			}
			if (e.PropertyName == BindablePicker.HasBorderProperty.PropertyName) 
			{
				SetBorder (Element as BindablePicker);
			}
		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(BindablePicker view)
		{
			if (view != null)
				Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}
	}
}


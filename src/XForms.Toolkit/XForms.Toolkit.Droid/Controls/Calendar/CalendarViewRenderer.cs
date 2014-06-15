using Android.Views;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XForms.Toolkit.Droid.Controls.Calendar;
using Android.Renderscripts;

[assembly: ExportRenderer (typeof (XForms.Toolkit.Controls.CalendarView), typeof (CalendarViewRenderer))]
namespace XForms.Toolkit.Droid.Controls.Calendar
{

	public class CalendarViewRenderer : ViewRenderer<XForms.Toolkit.Controls.CalendarView,LinearLayout>
    {
		private const string TAG = "XForms.Toolkit.Controls.Calendar";

        XForms.Toolkit.Controls.CalendarView _view;
        CalendarPickerView _pickerView;

        public CalendarViewRenderer()
        {
        }

		protected override void OnElementChanged (ElementChangedEventArgs<XForms.Toolkit.Controls.CalendarView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				_view = e.NewElement;

				LayoutInflater inflatorservice =
					(LayoutInflater)Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
				var containerView =
					(LinearLayout)inflatorservice.Inflate(Resource.Layout.calendar_picker, null, false);

				_pickerView = containerView.FindViewById<CalendarPickerView>(Resource.Id.calendar_view);
				_pickerView.Init(new DateTime(2014, 6, 1), new DateTime(2014, 6, 30))
					.InMode(CalendarPickerView.SelectionMode.Single);

				_pickerView.OnDateSelected += (s, ef) =>
				{
					_view.NotifyDateSelected(ef.SelectedDate);
				};

				SetNativeControl(containerView);
			}

		}
    }
}
using Xamarin.Forms;
using XForms.Toolkit.iOS.Controls.Calendar;
using XForms.Toolkit.Controls;
using Xamarin.Forms.Platform.iOS;
using escoz;
using System;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XForms.Toolkit.iOS.Controls.Calendar
{
	public class CalendarViewRenderer : ViewRenderer<CalendarView,CalendarMonthView>
	{
		CalendarView _view;
		public CalendarViewRenderer()
		{
		}
		protected override void OnElementChanged (ElementChangedEventArgs<CalendarView> e)
		{
			base.OnElementChanged (e);
			_view = Element;
					
						var calendarView = new CalendarMonthView(DateTime.Now, true);
			
						calendarView.OnDateSelected += (date) =>
						{
							_view.NotifyDateSelected(date);
						};
			
						base.SetNativeControl(calendarView);
		}
//		protected override void OnModelSet(VisualElement model)
//		{
//			_view = (CalendarView)model;
//			base.OnModelSet(model);
//
//			var calendarView = new CalendarMonthView(DateTime.Now, true);
//
//			calendarView.OnDateSelected += (date) =>
//			{
//				_view.NotifyDateSelected(date);
//			};
//
//			base.SetNativeControl(calendarView);
//		}
	}
}
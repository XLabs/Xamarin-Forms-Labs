using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XForms.Toolkit.Controls;
using XForms.Toolkit.WP.Controls.Calendar;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XForms.Toolkit.WP.Controls.Calendar
{
    public class CalendarViewRenderer : ViewRenderer<CalendarView, WPControls.Calendar>
    {
        public CalendarViewRenderer()
        {

        }
        protected override void OnModelSet()
        {
            base.OnModelSet();
            var calendar = new WPControls.Calendar();
            calendar.DateClicked +=
                (object sender, WPControls.SelectionChangedEventArgs e) =>
                {
                    Model.NotifyDateSelected(e.SelectedDate);
                };
            this.SetNativeControl(calendar);
        }
    }
}

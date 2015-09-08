using WPControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class CalendarViewRenderer.
	/// </summary>
	public class CalendarViewRenderer : ViewRenderer<CalendarView, WPControls.Calendar>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarViewRenderer"/> class.
		/// </summary>
		public CalendarViewRenderer()
		{

		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e)
		{
			base.OnElementChanged(e);
			var calendar = new WPControls.Calendar();
			calendar.DateClicked +=
				(object sender, SelectionChangedEventArgs es) =>
				{
					Element.NotifyDateSelected(es.SelectedDate);
				};
			this.SetNativeControl(calendar);
		}
	}
}

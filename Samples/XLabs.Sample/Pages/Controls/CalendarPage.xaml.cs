using System.Collections.Generic;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace XLabs.Sample.Pages.Controls
{
	using System;

	using Xamarin.Forms;

	using XLabs.Forms.Controls;

	/// <summary>
	/// Class CalendarPage.
	/// </summary>
	public partial class CalendarPage : ContentPage
	{
		/// <summary>
		/// The _calendar view
		/// </summary>
		readonly CalendarView _calendarView;
		/// <summary>
		/// The _relative layout
		/// </summary>
		//readonly RelativeLayout _relativeLayout;
		/// <summary>
		/// The _stacker
		/// </summary>
		readonly StackLayout _stacker;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarPage"/> class.
		/// </summary>
		public CalendarPage()
		{
			InitializeComponent();
  
			/*_relativeLayout = new RelativeLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			Content = _relativeLayout;*/



			_calendarView = new CalendarView() {
				//DayOfWeekLabelForegroundColor = Color.Black,
				//BackgroundColor = Color.Red,
				MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
				MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
				//HighlightedDateBackgroundColor = Color.Black,
				ShouldHighlightDaysOfWeekLabels = false,
				SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
				TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
				HighlightedDaysOfWeek = new DayOfWeek[]{DayOfWeek.Saturday,DayOfWeek.Sunday},
				ShowNavigationArrows = true,
				MonthTitleFont = Font.OfSize("Open 24 Display St",NamedSize.Medium),
				HeightRequest = 230, 
				//DayOfWeekLabelBackgroundColor = Color.Red
			};

			/*relativeLayout.Children.Add(_calendarView,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height * 2/3));*/

			_stacker = new StackLayout() {
				//HorizontalOptions = LayoutOptions.FillAndExpand,
				//VerticalOptions = LayoutOptions.StartAndExpand
			};
			Content = _stacker;
			_stacker.Children.Add (_calendarView);
			/*_relativeLayout.Children.Add(_stacker,
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Height *2/3),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height *1/3)
			);
			_calendarView.DateSelected += (object sender, DateTime e) =>
			{
				_stacker.Children.Add(new Label()
					{
						Text = "Date Was Selected" + e.ToString("d"),
						VerticalOptions = LayoutOptions.Start,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
					});
			};*/

			var selectedDates = new List<DateTime> ();
			var actualDate = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
			selectedDates.Add(actualDate.AddDays(2));
			/*selectedDates.Add(actualDate.AddDays(4));
			selectedDates.Add(actualDate.AddDays(6));
			selectedDates.Add(actualDate.AddDays(10));
			selectedDates.Add(actualDate.AddDays(12));
			selectedDates.Add(actualDate.AddDays(14));
			selectedDates.Add(actualDate.AddDays(16));*/
		
			_calendarView.SelectedDates = selectedDates;



		}

		static double GetHeightCalendarByDevice(){
			var device = Resolver.Resolve<IDevice>();

			if (device.Manufacturer.ToLower ().Contains ("apple")) {
				GetHeightCalendarByDeviceIOS ();
			} else {
				GetHeightCalendarByDeviceAndroid ();
			}

			return 0;
		}

		static double GetHeightCalendarByDeviceAndroid(){
			var device = Resolver.Resolve<IDevice>();
			var screenHeight = device.Display.Height;

			return screenHeight / 2;
		}

		static double GetHeightCalendarByDeviceIOS(){
			var device = Resolver.Resolve<IDevice>();
			var screenWidth = device.Display.Width;

			var height = (230 * screenWidth) / 640;
			return height;
		}

		/*static double GetHeightCalendarByDeviceAndroid(){
			var device = Resolver.Resolve<IDevice>();
			var screenWidth = device.Display.Width;

			var height = (230 * screenWidth) / 768;
			return height;
		}*/
	}
}


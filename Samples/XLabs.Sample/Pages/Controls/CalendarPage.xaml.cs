using System.Collections.Generic;

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
		readonly RelativeLayout _relativeLayout;
		/// <summary>
		/// The _stacker
		/// </summary>
		readonly StackLayout _stacker;

		readonly Button _button;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarPage"/> class.
		/// </summary>
		public CalendarPage()
		{
			InitializeComponent();
  
			_relativeLayout = new RelativeLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			Content = _relativeLayout;



			_calendarView = new CalendarView() {
				//BackgroundColor = Color.Blue
				MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
				MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
				HighlightedDateBackgroundColor = Color.FromRgb(227	,227,	227	),
				ShouldHighlightDaysOfWeekLabels = false,
				SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
				TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
				HighlightedDaysOfWeek = new DayOfWeek[]{DayOfWeek.Saturday,DayOfWeek.Sunday},
				ShowNavigationArrows = true,
				MonthTitleFont = Font.OfSize("Open 24 Display St",NamedSize.Medium)
			};

			_relativeLayout.Children.Add(_calendarView,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height * 2/3));

			_stacker = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand
			};
			_relativeLayout.Children.Add(_stacker,
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Height *2/3),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height *1/3)
			);

			_button = new Button ();
			_button.Text = "hola";
			_button.Clicked += (object sender, EventArgs e) => {
				_calendarView.SelectedDate = DateTime.Now;
				_calendarView.NotifyDateSelected (DateTime.Now);
				_calendarView.NotifyDisplayedMonthChanged (DateTime.Now);
			};
			_stacker.Children.Add (_button);

			_calendarView.DateSelected += (object sender, DateTime e) =>
			{
				_stacker.Children.Add(new Label()
					{
						Text = "Date Was Selected" + e.ToString("d"),
						VerticalOptions = LayoutOptions.Start,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
					});


			};

			var selectedDates = new List<DateTime> ();
			var actualDate = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
			selectedDates.Add(actualDate.AddDays(2));
			selectedDates.Add(actualDate.AddDays(4));
			selectedDates.Add(actualDate.AddDays(6));
		
			_calendarView.SelectedDates = selectedDates;

		}
	}
}


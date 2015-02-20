using System.Collections;
using Android.Views;

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using Android.Content;
	using Android.Runtime;
	using Android.Support.V4.View;
	using Android.Util;
	using Android.Widget;

	using Java.Lang;

	/// <summary>
	/// Class CalendarPickerView.
	/// </summary>
	public class CalendarPickerView : ViewPager
	{
		/// <summary>
		/// Enum SelectionMode
		/// </summary>
		public enum SelectionMode
		{
			/// <summary>
			/// The single
			/// </summary>
			Single,
			/// <summary>
			/// The multi
			/// </summary>
			Multi,
			/// <summary>
			/// The range
			/// </summary>
			Range}

		;

		/// <summary>
		/// Class OnPageChangeListener.
		/// </summary>
		internal class OnPageChangeListener : SimpleOnPageChangeListener
		{
			/// <summary>
			/// The _picker
			/// </summary>
			CalendarPickerView _picker;

			/// <summary>
			/// Initializes a new instance of the <see cref="OnPageChangeListener"/> class.
			/// </summary>
			/// <param name="picker">The picker.</param>
			public OnPageChangeListener(CalendarPickerView picker)
			{
				this._picker = picker;

			}

			/// <summary>
			/// Called when [page selected].
			/// </summary>
			/// <param name="position">The position.</param>
			public override void OnPageSelected(int position)
			{
				_picker.InvokeOnMonthChanged(position);

				//base.OnPageSelected(position);
			}
		}

		/// <summary>
		/// The _context
		/// </summary>
		private readonly Context _context;
		/// <summary>
		/// My adapter
		/// </summary>
		internal readonly MonthAdapter MyAdapter;
		/// <summary>
		/// The months
		/// </summary>
		internal readonly List<MonthDescriptor> Months = new List<MonthDescriptor>();

		/// <summary>
		/// The cells
		/// </summary>
		internal readonly List<List<List<MonthCellDescriptor>>> Cells =
			new List<List<List<MonthCellDescriptor>>>();

		/// <summary>
		/// The selected cells
		/// </summary>
		internal List<MonthCellDescriptor> SelectedCells = new List<MonthCellDescriptor>();
		/// <summary>
		/// The _highlighted cells
		/// </summary>
		private readonly List<MonthCellDescriptor> _highlightedCells = new List<MonthCellDescriptor>();
		/// <summary>
		/// The selected cals
		/// </summary>
		internal List<DateTime> SelectedCals = new List<DateTime>();
		/// <summary>
		/// The _highlighted cals
		/// </summary>
		private readonly List<DateTime> _highlightedCals = new List<DateTime>();
		/// <summary>
		/// The today
		/// </summary>
		internal readonly DateTime Today = DateTime.Now;
		/// <summary>
		/// The minimum date
		/// </summary>
		internal DateTime MinDate;
		/// <summary>
		/// The maximum date
		/// </summary>
		internal DateTime MaxDate;
		/// <summary>
		/// The _month counter
		/// </summary>
		private DateTime _monthCounter;

		/// <summary>
		/// The selected dates items.
		/// </summary>
		internal ICollection<DateTime> SelectedDatesItems;

		//This provides styling to the calendar elements
		//Elements holds reference to it through Month and Week cell descriptors
		/// <summary>
		/// The _style descriptor
		/// </summary>
		private StyleDescriptor _styleDescriptor;

		/// <summary>
		/// Gets the style descriptor.
		/// </summary>
		/// <value>The style descriptor.</value>
		public StyleDescriptor StyleDescriptor {
			get {
				return _styleDescriptor;
			}
		}

		/// <summary>
		/// The month name format
		/// </summary>
		internal readonly string MonthNameFormat;
		/// <summary>
		/// The weekday name format
		/// </summary>
		internal readonly string WeekdayNameFormat;
		/// <summary>
		/// The full date format
		/// </summary>
		internal readonly string FullDateFormat;

		/// <summary>
		/// The click handler
		/// </summary>
		internal ClickHandler ClickHandler;

		/// <summary>
		/// Occurs when [on invalid date selected].
		/// </summary>
		public event EventHandler<DateSelectedEventArgs> OnInvalidDateSelected;
		/// <summary>
		/// Occurs when [on date selected].
		/// </summary>
		public event EventHandler<DateSelectedEventArgs> OnDateSelected;
		/// <summary>
		/// Occurs when [on date unselected].
		/// </summary>
		public event EventHandler<DateSelectedEventArgs> OnDateUnselected;
		/// <summary>
		/// Occurs when [on month changed].
		/// </summary>
		public event EventHandler<MonthChangedEventArgs> OnMonthChanged;
		/// <summary>
		/// Occurs when [on date selectable].
		/// </summary>
		public event DateSelectableHandler OnDateSelectable;




		/// <summary>
		/// Gets or sets the mode.
		/// </summary>
		/// <value>The mode.</value>
		public SelectionMode Mode { get; set; }

		/// <summary>
		/// Gets the current month.
		/// </summary>
		/// <value>The current month.</value>
		public DateTime CurrentMonth {
			get {
				return this.Months[this.CurrentItem].Date;
			}
		}

		/// <summary>
		/// Gets the month count.
		/// </summary>
		/// <value>The month count.</value>
		public int MonthCount {
			get {
				return this.Months.Count;
			}
		}


		/// <summary>
		/// Invokes the on month changed.
		/// </summary>
		/// <param name="position">The position.</param>
		protected void InvokeOnMonthChanged(int position)
		{
			if(this.OnMonthChanged != null) {
				var month = this.Months[position];
				this.OnMonthChanged(this, new MonthChangedEventArgs(month.Date));
			}
		}


		/// <summary>
		/// Gets the selected date.
		/// </summary>
		/// <value>The selected date.</value>
		public DateTime SelectedDate {
			get { return SelectedCals.Count > 0 ? SelectedCals[0] : DateTime.MinValue; }
		}

		/// <summary>
		/// Gets the selected dates.
		/// </summary>
		/// <value>The selected dates.</value>
		public List<DateTime> SelectedDates {
			get {
				var selectedDates = SelectedCells.Select(cal => cal.DateTime).ToList();
				selectedDates.Sort();
				return selectedDates;
			}
		}

		/**
		 * Forces a redraw of the calendar and thus applying of the styles
		 */
		/// <summary>
		/// Updates the styles.
		/// </summary>
		public void UpdateStyles()
		{
			base.SetBackgroundColor(_styleDescriptor.BackgroundColor);

			Adapter.NotifyDataSetChanged();
		}

		/// <summary>
		/// The _hlighlighted days of week
		/// </summary>
		Dictionary<int, bool> _hlighlightedDaysOfWeek;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarPickerView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		public CalendarPickerView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			ResourceIdManager.UpdateIdValues();
			_context = context;
			_styleDescriptor = new StyleDescriptor();
			MyAdapter = new MonthAdapter(context, this);
			base.Adapter = MyAdapter;


			//SetPadding(0, 0, 0, 0);

			var bgColor = base.Resources.GetColor(Resource.Color.calendar_bg);
			base.SetBackgroundColor(bgColor);

			MonthNameFormat = base.Resources.GetString(Resource.String.month_name_format);
			WeekdayNameFormat = base.Resources.GetString(Resource.String.day_name_format);
			FullDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
			ClickHandler += OnCellClicked;
			OnInvalidDateSelected += OnInvalidateDateClicked;
			SetOnPageChangeListener(new OnPageChangeListener(this));



			if(base.IsInEditMode) {
				Init(DateTime.Now, DateTime.Now.AddYears(1), new DayOfWeek[]{ }).WithSelectedDate(DateTime.Now);
			}
		}



		/// <summary>
		/// Called when [cell clicked].
		/// </summary>
		/// <param name="cell">The cell.</param>
		private void OnCellClicked(MonthCellDescriptor cell)
		{
			var clickedDate = cell.DateTime;

			if(!IsBetweenDates(clickedDate, MinDate, MaxDate) || !IsSelectable(clickedDate)) {
				if(OnInvalidDateSelected != null) {
					OnInvalidDateSelected(this, new DateSelectedEventArgs(clickedDate));
				}
			} else {
				bool wasSelected = DoSelectDate(clickedDate, cell);
				if(OnDateSelected != null) {
					if(wasSelected) {
						OnDateSelected(this, new DateSelectedEventArgs(clickedDate));
					} else if(OnDateUnselected != null) {
						OnDateUnselected(this, new DateSelectedEventArgs(clickedDate));
					}
				}
			}
		}

		/// <summary>
		/// Handles the <see cref="E:InvalidateDateClicked" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="DateSelectedEventArgs"/> instance containing the event data.</param>
		private void OnInvalidateDateClicked(object sender, DateSelectedEventArgs e)
		{
			string fullDateFormat = _context.Resources.GetString(Resource.String.full_date_format);
			string errorMsg = _context.Resources.GetString(Resource.String.invalid_date);
			errorMsg = string.Format(errorMsg, MinDate.ToString(fullDateFormat),
				MaxDate.ToString(fullDateFormat));
			Toast.MakeText(_context, errorMsg, ToastLength.Short).Show();
		}

		/// <summary>
		/// Initializes the specified minimum date.
		/// </summary>
		/// <param name="minDate">The minimum date.</param>
		/// <param name="maxDate">The maximum date.</param>
		/// <param name="highlightedDaysOfWeek">The highlighted days of week.</param>
		/// <returns>FluentInitializer.</returns>
		/// <exception cref="Java.Lang.IllegalArgumentException">
		/// minDate and maxDate must be non-zero.  +
		/// 				Debug(minDate, maxDate)
		/// or
		/// minDate must be before maxDate.  +
		/// 				Debug(minDate, maxDate)
		/// </exception>
		public FluentInitializer Init(DateTime minDate, DateTime maxDate, DayOfWeek[] highlightedDaysOfWeek)
		{
			if(minDate == DateTime.MinValue || maxDate == DateTime.MinValue) {
				throw new IllegalArgumentException("minDate and maxDate must be non-zero. " +
				Debug(minDate, maxDate));
			}
			if(minDate.CompareTo(maxDate) > 0) {
				throw new IllegalArgumentException("minDate must be before maxDate. " +
				Debug(minDate, maxDate));
			}

			HighlighDaysOfWeeks(highlightedDaysOfWeek);

			Mode = SelectionMode.Single;
			//Clear out any previously selected dates/cells.
			SelectedCals.Clear();
			SelectedCells.Clear();
			_highlightedCals.Clear();
			_highlightedCells.Clear();

			//Clear previous state.
			Cells.Clear();
			Months.Clear();
			MinDate = minDate;
			MaxDate = maxDate;
			MinDate = SetMidnight(MinDate);
			MaxDate = SetMidnight(MaxDate);

			// maxDate is exclusive: bump back to the previous day so if maxDate is the first of a month,
			// We don't accidentally include that month in the view.
			if(MaxDate.Day == 1) {
				MaxDate = MaxDate.AddMinutes(-1);
			}

			//Now iterate between minCal and maxCal and build up our list of months to show.
			_monthCounter = MinDate;
			int maxMonth = MaxDate.Month;
			int maxYear = MaxDate.Year;
			while((_monthCounter.Month <= maxMonth
				   || _monthCounter.Year < maxYear)
				   && _monthCounter.Year < maxYear + 1) {
				var month = new MonthDescriptor(_monthCounter.Month, _monthCounter.Year, _monthCounter,
								_monthCounter.ToString(MonthNameFormat), _styleDescriptor);
				Cells.Add(GetMonthCells(month, _monthCounter));
				Logr.D("Adding month {0}", month);
				Months.Add(month);
				_monthCounter = _monthCounter.AddMonths(1);
			}

			ValidateAndUpdate();
			return new FluentInitializer(this);
		}

		/// <summary>
		/// Highlighes the days of weeks.
		/// </summary>
		/// <param name="daysOfWeeks">The days of weeks.</param>
		public void HighlighDaysOfWeeks(DayOfWeek[] daysOfWeeks)
		{
			_hlighlightedDaysOfWeek = new Dictionary<int,bool>();
			for(int i = 0; i <= 6; i++) {
				_hlighlightedDaysOfWeek[i] = false;
			}
			foreach(var dOw in daysOfWeeks) {
				_hlighlightedDaysOfWeek[(int)dOw] = true;
			}
		}



		/// <summary>
		/// Gets the month cells.
		/// </summary>
		/// <param name="month">The month.</param>
		/// <param name="startCal">The start cal.</param>
		/// <returns>List&lt;List&lt;MonthCellDescriptor&gt;&gt;.</returns>
		internal List<List<MonthCellDescriptor>> GetMonthCells(MonthDescriptor month, DateTime startCal)
		{
			var cells = new List<List<MonthCellDescriptor>>();
			var cal = new DateTime(startCal.Year, startCal.Month, 1);
			var firstDayOfWeek = (int)cal.DayOfWeek;
			cal = cal.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - firstDayOfWeek);

			var minSelectedCal = GetMinDate(SelectedCals);
			var maxSelectedCal = GetMaxDate(SelectedCals);

			var numWeeks = 0;
			while(((cal.Month < month.Month + 1 || cal.Year < month.Year)
				&& cal.Year <= month.Year) || numWeeks < 5) {
				Logr.D("Building week row starting at {0}", cal);
				var weekCells = new List<MonthCellDescriptor>();
				cells.Add(weekCells);
				for(int i = 0; i < 7; i++) 
				{
					var date = cal;
					bool isCurrentMonth = cal.Month == month.Month;
					bool isSelected = isCurrentMonth && ContatinsDate(SelectedCals, cal);
					bool isSelectable = isCurrentMonth && IsBetweenDates(cal, MinDate, MaxDate);
					bool isToday = IsSameDate(cal, Today);
					bool isHighlighted = ContatinsDate(_highlightedCals, cal) || _hlighlightedDaysOfWeek[(int)cal.DayOfWeek];
					int value = cal.Day;
					bool isSelectedDatesMarked = false;
					if(SelectedDatesItems != null){
						isSelectedDatesMarked = SelectedDatesItems.Any (x => x.Equals (date));
					}

					var rangeState = RangeState.None;
					if(SelectedCals.Count > 1) {
						if(IsSameDate(minSelectedCal, cal)) {
							rangeState = RangeState.First;
						} else if(IsSameDate(maxSelectedCal, cal)) {
							rangeState = RangeState.Last;
						} else if(IsBetweenDates(cal, minSelectedCal, maxSelectedCal)) {
							rangeState = RangeState.Middle;
						}
					}

					weekCells.Add(new MonthCellDescriptor(date, isCurrentMonth, isSelectable, isSelected,
						isToday, isHighlighted, value, rangeState, _styleDescriptor, isSelectedDatesMarked));
					cal = cal.AddDays(1);
				}
				numWeeks++;
			}
			return cells;
		}

		/// <summary>
		/// Scrolls to selected month.
		/// </summary>
		/// <param name="selectedIndex">Index of the selected.</param>
		internal void ScrollToSelectedMonth(int selectedIndex)
		{
			ScrollToSelectedMonth(selectedIndex, false);
		}

		/// <summary>
		/// Scrolls to selected month.
		/// </summary>
		/// <param name="selectedIndex">Index of the selected.</param>
		/// <param name="smoothScroll">if set to <c>true</c> [smooth scroll].</param>
		internal void ScrollToSelectedMonth(int selectedIndex, bool smoothScroll)
		{
//            Task.Factory.StartNew(() =>
//            {
//                if (smoothScroll)
//                {
//                    SmoothScrollToPosition(selectedIndex);
//                }
//                else
//                {
//                    SetSelection(selectedIndex);
//                }
//            });
			this.SetCurrentItem(selectedIndex, smoothScroll);

		}

		/// <summary>
		/// Gets the month cell with index by date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>MonthCellWithMonthIndex.</returns>
		private MonthCellWithMonthIndex GetMonthCellWithIndexByDate(DateTime date)
		{
			int index = 0;

			foreach(var monthCell in Cells) {
				foreach(var actCell in from weekCell in monthCell
										from actCell in weekCell
										where IsSameDate(actCell.DateTime, date) && actCell.IsSelectable
										select actCell)
					return new MonthCellWithMonthIndex(actCell, index);
				index++;
			}
			return null;
		}

		/// <summary>
		/// Called when [measure].
		/// </summary>
		/// <param name="widthMeasureSpec">horizontal space requirements as imposed by the parent.
		/// The requirements are encoded with
		/// <c><see cref="T:Android.Views.View+MeasureSpec" /></c>.</param>
		/// <param name="heightMeasureSpec">vertical space requirements as imposed by the parent.
		/// The requirements are encoded with
		/// <c><see cref="T:Android.Views.View+MeasureSpec" /></c>.</param>
		/// <exception cref="System.InvalidOperationException">Must have at least one month to display. Did you forget to call Init()?</exception>
		/// <since version="Added in API level 1" />
		/// <altmember cref="P:Android.Views.View.MeasuredWidth" />
		/// <altmember cref="P:Android.Views.View.MeasuredHeight" />
		/// <altmember cref="M:Android.Views.View.SetMeasuredDimension(System.Int32, System.Int32)" />
		/// <altmember cref="M:Android.Views.View.get_SuggestedMinimumHeight" />
		/// <altmember cref="M:Android.Views.View.get_SuggestedMinimumWidth" />
		/// <altmember cref="M:Android.Views.View.MeasureSpec.GetMode(System.Int32)" />
		/// <altmember cref="M:Android.Views.View.MeasureSpec.GetSize(System.Int32)" />
		/// <remarks><para tool="javadoc-to-mdoc" />
		/// <para tool="javadoc-to-mdoc">
		/// Measure the view and its content to determine the measured width and the
		/// measured height. This method is invoked by <c><see cref="M:Android.Views.View.Measure(System.Int32, System.Int32)" /></c> and
		/// should be overriden by subclasses to provide accurate and efficient
		/// measurement of their contents.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <i>CONTRACT:</i> When overriding this method, you
		/// <i>must</i> call <c><see cref="M:Android.Views.View.SetMeasuredDimension(System.Int32, System.Int32)" /></c> to store the
		/// measured width and height of this view. Failure to do so will trigger an
		/// <c>IllegalStateException</c>, thrown by
		/// <c><see cref="M:Android.Views.View.Measure(System.Int32, System.Int32)" /></c>. Calling the superclass'
		/// <c><see cref="M:Android.Views.View.OnMeasure(System.Int32, System.Int32)" /></c> is a valid use.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		/// The base class implementation of measure defaults to the background size,
		/// unless a larger size is allowed by the MeasureSpec. Subclasses should
		/// override <c><see cref="M:Android.Views.View.OnMeasure(System.Int32, System.Int32)" /></c> to provide better measurements of
		/// their content.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		/// If this method is overridden, it is the subclass's responsibility to make
		/// sure the measured height and width are at least the view's minimum height
		/// and width (<c><see cref="M:Android.Views.View.get_SuggestedMinimumHeight" /></c> and
		/// <c><see cref="M:Android.Views.View.get_SuggestedMinimumWidth" /></c>).
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#onMeasure(int, int)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if(Months.Count == 0) {
				throw new InvalidOperationException(
					"Must have at least one month to display. Did you forget to call Init()?");
			}

			Logr.D("PickerView.OnMeasure w={0} h={1}", MeasureSpec.ToString(widthMeasureSpec),
				MeasureSpec.ToString(heightMeasureSpec));
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}

		/// <summary>
		/// Sets the midnight.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>DateTime.</returns>
		private static DateTime SetMidnight(DateTime date)
		{
			return date.Subtract(date.TimeOfDay);
		}

		/// <summary>
		/// Determines whether the specified date is selectable.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns><c>true</c> if the specified date is selectable; otherwise, <c>false</c>.</returns>
		private bool IsSelectable(DateTime date)
		{
			return OnDateSelectable == null || OnDateSelectable(date);
		}

		/// <summary>
		/// Applies the multi select.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="selectedCal">The selected cal.</param>
		/// <returns>DateTime.</returns>
		private DateTime ApplyMultiSelect(DateTime date, DateTime selectedCal)
		{
			foreach(var selectedCell in SelectedCells) {
				if(selectedCell.DateTime == date) {
					//De-select the currently selected cell.
					selectedCell.IsSelected = false;
					SelectedCells.Remove(selectedCell);
					date = DateTime.MinValue;
					break;
				}
			}

			foreach(var cal in SelectedCals) {
				if(IsSameDate(cal, selectedCal)) {
					SelectedCals.Remove(cal);
					break;
				}
			}
			return date;
		}

		/// <summary>
		/// Clears the old selection.
		/// </summary>
		private void ClearOldSelection()
		{
			if (SelectedDatesItems == null) {
				foreach (var selectedCell in SelectedCells) {
					//De-select the currently selected cell.
					selectedCell.IsSelected = false;
				}
				SelectedCells.Clear ();
				SelectedCals.Clear ();
			} 
			else {
				foreach (var selectedCell in SelectedCells) {
					if (!SelectedDatesItems.Any (x => x.Equals (selectedCell.DateTime))) {
						selectedCell.IsSelected = false;
					}
				}
			}
		}

		/// <summary>
		/// Deselects the date.
		/// </summary>
		public void DeselectDate()
		{
			if(SelectedDate != DateTime.MinValue) {
				DoSelectDate(DateTime.MinValue, null);
			}
		}

		/// <summary>
		/// Does the select date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="cell">The cell.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		/// <exception cref="Java.Lang.IllegalStateException">Unknown SelectionMode  + Mode</exception>
		internal bool DoSelectDate(DateTime date, MonthCellDescriptor cell)
		{
			var newlySelectedDate = date;
			SetMidnight(newlySelectedDate);

			//Clear any remaining range state.
			foreach(var selectedCell in SelectedCells) {
				selectedCell.RangeState = RangeState.None;
			}

			switch(Mode) {
			case SelectionMode.Range:
				if(SelectedCals.Count > 1) {
					//We've already got a range selected: clear the old one.
					ClearOldSelection();
				} else if(SelectedCals.Count == 1 && newlySelectedDate.CompareTo(SelectedCals[0]) < 0) {
					//We're moving the start of the range back in time: clear the old start date.
					ClearOldSelection();
				}
				break;
			case SelectionMode.Multi:
				date = ApplyMultiSelect(date, newlySelectedDate);
				break;
			case SelectionMode.Single:
				ClearOldSelection();
				break;
			default:
				throw new IllegalStateException("Unknown SelectionMode " + Mode);
			}

			if(date > DateTime.MinValue) {
				if(SelectedCells.Count == 0 || !SelectedCells[0].Equals(cell)) {
					SelectedCells.Add(cell);
					cell.IsSelected = true;
				}
				SelectedCals.Add(newlySelectedDate);

				if(Mode == SelectionMode.Range && SelectedCells.Count > 1) {
					//Select all days in between start and end.
					var startDate = SelectedCells[0].DateTime;
					var endDate = SelectedCells[1].DateTime;
					SelectedCells[0].RangeState = RangeState.First;
					SelectedCells[1].RangeState = RangeState.Last;

					foreach(var month in Cells) {
						foreach(var week in month) {
							foreach(var singleCell in week) {
								var singleCellDate = singleCell.DateTime;
								if(singleCellDate.CompareTo(startDate) >= 0
									&& singleCellDate.CompareTo(endDate) <= 0
									&& singleCell.IsSelectable) {
									singleCell.IsSelected = true;
									singleCell.RangeState = RangeState.Middle;
									SelectedCells.Add(singleCell);
								}
							}
						}
					}
				}
			}
			ValidateAndUpdate();
			return date > DateTime.MinValue;
		}

		internal bool DoSelectDates(DateTime date, MonthCellDescriptor cell)
		{
			var newlySelectedDate = date;
			SetMidnight(newlySelectedDate);

			//Clear any remaining range state.
			foreach(var selectedCell in SelectedCells) {
				selectedCell.RangeState = RangeState.None;
			}


			date = ApplyMultiSelect(date, newlySelectedDate);
				

			if(date > DateTime.MinValue) 
			{
				if(SelectedCells.Count == 0 || !SelectedCells[0].Equals(cell)) {
					cell.IsSelected = true;
					cell.IsSelectedDatesMarked = true;
					SelectedCells.Add(cell);
				}
				SelectedCals.Add(newlySelectedDate);
			}
			ValidateAndUpdate();
			return date > DateTime.MinValue;
		}

		/// <summary>
		/// Validates the and update.
		/// </summary>
		internal void ValidateAndUpdate()
		{
			if(Adapter == null) {
				Adapter = MyAdapter;
			}
			MyAdapter.NotifyDataSetChanged();
		}

		/// <summary>
		/// Selects the date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		internal bool SelectDate(DateTime date)
		{
			return SelectDate(date, false);
		}

		internal bool SelectDates(ICollection<DateTime> dates)
		{
			SelectedDatesItems = dates;

			foreach(var date in dates)
			{
				ValidateDate (date);

				var cell = GetMonthCellWithIndexByDate(date);
				if(cell == null || !IsSelectable(date)) {
					return false;
				}

				DoSelectDates(date, cell.Cell);
			}

			return true;
		}

		/// <summary>
		/// Selects the date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="smoothScroll">if set to <c>true</c> [smooth scroll].</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool SelectDate(DateTime date, bool smoothScroll)
		{
			ValidateDate(date);

			var cell = GetMonthCellWithIndexByDate(date);
			if(cell == null || !IsSelectable(date)) {
				return false;
			}

			bool wasSelected = DoSelectDate(date, cell.Cell);
			if(wasSelected) {
				ScrollToSelectedMonth(cell.MonthIndex, smoothScroll);
			}
			return wasSelected;
		}

		/// <summary>
		/// Validates the date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <exception cref="Java.Lang.IllegalArgumentException">
		/// Selected date must be non-zero.
		/// or
		/// </exception>
		private void ValidateDate(DateTime date)
		{
			if(date == DateTime.MinValue) {
				throw new IllegalArgumentException("Selected date must be non-zero.");
			}
			if(date.CompareTo(MinDate) < 0 || date.CompareTo(MaxDate) > 0) {
				throw new IllegalArgumentException(
					string.Format("Selected date must be between minDate and maxDate. "
					+ "minDate: {0}, maxDate: {1}, selectedDate: {2}.",
						MinDate.ToShortDateString(), MaxDate.ToShortDateString(), date.ToShortDateString()));
			}
		}

		/// <summary>
		/// Gets the minimum date.
		/// </summary>
		/// <param name="selectedCals">The selected cals.</param>
		/// <returns>DateTime.</returns>
		private static DateTime GetMinDate(List<DateTime> selectedCals)
		{
			if(selectedCals == null || selectedCals.Count == 0) {
				return DateTime.MinValue;
			}
			selectedCals.Sort();
			return selectedCals[0];
		}

		/// <summary>
		/// Gets the maximum date.
		/// </summary>
		/// <param name="selectedCals">The selected cals.</param>
		/// <returns>DateTime.</returns>
		private static DateTime GetMaxDate(List<DateTime> selectedCals)
		{
			if(selectedCals == null || selectedCals.Count == 0) {
				return DateTime.MinValue;
			}
			selectedCals.Sort();
			return selectedCals[selectedCals.Count - 1];
		}

		/// <summary>
		/// Determines whether [is between dates] [the specified date].
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="minCal">The minimum cal.</param>
		/// <param name="maxCal">The maximum cal.</param>
		/// <returns><c>true</c> if [is between dates] [the specified date]; otherwise, <c>false</c>.</returns>
		private static bool IsBetweenDates(DateTime date, DateTime minCal, DateTime maxCal)
		{
			return (date.Equals(minCal) || date.CompareTo(minCal) > 0)// >= minCal
			&& date.CompareTo(maxCal) <= 0; // && < maxCal
		}

		/// <summary>
		/// Determines whether [is same date] [the specified cal].
		/// </summary>
		/// <param name="cal">The cal.</param>
		/// <param name="selectedDate">The selected date.</param>
		/// <returns><c>true</c> if [is same date] [the specified cal]; otherwise, <c>false</c>.</returns>
		private static bool IsSameDate(DateTime cal, DateTime selectedDate)
		{
			return cal.Month == selectedDate.Month
			&& cal.Year == selectedDate.Year
			&& cal.Day == selectedDate.Day;
		}

		/// <summary>
		/// Determines whether [is same month] [the specified cal].
		/// </summary>
		/// <param name="cal">The cal.</param>
		/// <param name="month">The month.</param>
		/// <returns><c>true</c> if [is same month] [the specified cal]; otherwise, <c>false</c>.</returns>
		internal static bool IsSameMonth(DateTime cal, MonthDescriptor month)
		{
			return (cal.Month == month.Month && cal.Year == month.Year);
		}

		/// <summary>
		/// Contatinses the date.
		/// </summary>
		/// <param name="selectedCals">The selected cals.</param>
		/// <param name="cal">The cal.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private static bool ContatinsDate(IEnumerable<DateTime> selectedCals, DateTime cal)
		{
			return selectedCals.Any(selectedCal => IsSameDate(cal, selectedCal));
		}

		/// <summary>
		/// Highlights the dates.
		/// </summary>
		/// <param name="dates">The dates.</param>
		public void HighlightDates(ICollection<DateTime> dates)
		{
			foreach(var date in dates) {
				ValidateDate(date);

				var monthCellWithMonthIndex = GetMonthCellWithIndexByDate(date);
				if(monthCellWithMonthIndex != null) {
					var cell = monthCellWithMonthIndex.Cell;
					_highlightedCells.Add(cell);
					_highlightedCals.Add(date);
					cell.IsHighlighted = true;
				}
			}

			MyAdapter.NotifyDataSetChanged();
			Adapter = MyAdapter;
		}

		/// <summary>
		/// Debugs the specified minimum date.
		/// </summary>
		/// <param name="minDate">The minimum date.</param>
		/// <param name="maxDate">The maximum date.</param>
		/// <returns>System.String.</returns>
		private static string Debug(DateTime minDate, DateTime maxDate)
		{
			return "minDate: " + minDate + "\nmaxDate: " + maxDate;
		}

		/// <summary>
		/// Class MonthCellWithMonthIndex.
		/// </summary>
		private class MonthCellWithMonthIndex
		{
			/// <summary>
			/// The cell
			/// </summary>
			public readonly MonthCellDescriptor Cell;
			/// <summary>
			/// The month index
			/// </summary>
			public readonly int MonthIndex;

			/// <summary>
			/// Initializes a new instance of the <see cref="MonthCellWithMonthIndex"/> class.
			/// </summary>
			/// <param name="cell">The cell.</param>
			/// <param name="monthIndex">Index of the month.</param>
			public MonthCellWithMonthIndex(MonthCellDescriptor cell, int monthIndex)
			{
				Cell = cell;
				MonthIndex = monthIndex;
			}
		}
	}

	/// <summary>
	/// Class FluentInitializer.
	/// </summary>
	public class FluentInitializer
	{
		/// <summary>
		/// The _calendar
		/// </summary>
		private readonly CalendarPickerView _calendar;

		/// <summary>
		/// Initializes a new instance of the <see cref="FluentInitializer"/> class.
		/// </summary>
		/// <param name="calendar">The calendar.</param>
		public FluentInitializer(CalendarPickerView calendar)
		{
			_calendar = calendar;
		}

		/// <summary>
		/// Ins the mode.
		/// </summary>
		/// <param name="mode">The mode.</param>
		/// <returns>FluentInitializer.</returns>
		public FluentInitializer InMode(CalendarPickerView.SelectionMode mode)
		{
			_calendar.Mode = mode;
			_calendar.ValidateAndUpdate();
			return this;
		}

		/// <summary>
		/// Withes the selected date.
		/// </summary>
		/// <param name="selectedDate">The selected date.</param>
		/// <returns>FluentInitializer.</returns>
		public FluentInitializer WithSelectedDate(DateTime selectedDate)
		{
			return WithSelectedDates(new List<DateTime> { selectedDate });
		}

		/// <summary>
		/// Withes the selected dates.
		/// </summary>
		/// <param name="selectedDates">The selected dates.</param>
		/// <returns>FluentInitializer.</returns>
		/// <exception cref="Java.Lang.IllegalArgumentException">SINGLE mode can't be used with multiple selectedDates</exception>
		public FluentInitializer WithSelectedDates(ICollection<DateTime> selectedDates)
		{
			if(_calendar.Mode == CalendarPickerView.SelectionMode.Single && _calendar.SelectedDates.Count > 1) {
				throw new IllegalArgumentException("SINGLE mode can't be used with multiple selectedDates");
			}
			if(_calendar.SelectedDates != null) {
				foreach(var date in selectedDates) {
					_calendar.SelectDate(date);
				}
			}
			int selectedIndex = -1;
			int todayIndex = -1;
			for(int i = 0; i < _calendar.Months.Count; i++) {
				var month = _calendar.Months[i];
				if(selectedIndex == -1) {
					if(_calendar.SelectedCals.Any(
							selectedCal => CalendarPickerView.IsSameMonth(selectedCal, month))) {
						selectedIndex = i;
					}
					if(selectedIndex == -1 && todayIndex == -1 &&
						CalendarPickerView.IsSameMonth(DateTime.Now, month)) {
						todayIndex = i;
					}
				}
			}
			if(selectedIndex != -1) {
				_calendar.ScrollToSelectedMonth(selectedIndex);
			} else if(todayIndex != -1) {
				_calendar.ScrollToSelectedMonth(todayIndex);
			}

			_calendar.ValidateAndUpdate();
			return this;
		}

		/// <summary>
		/// Withes the locale.
		/// </summary>
		/// <param name="locale">The locale.</param>
		/// <returns>FluentInitializer.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public FluentInitializer WithLocale(Java.Util.Locale locale)
		{
			//Not sure how to translate this to C# flavor.
			//Leave it later.
			throw new NotImplementedException();
		}

		/// <summary>
		/// Withes the highlighted dates.
		/// </summary>
		/// <param name="dates">The dates.</param>
		/// <returns>FluentInitializer.</returns>
		public FluentInitializer WithHighlightedDates(ICollection<DateTime> dates)
		{
			_calendar.HighlightDates(dates);
			return this;
		}

		//		public FluentInitializer WithHighlightedDaysOfWeek()
		//		{
		//			_calendar.HighlighDaysOfWeeks(daysOfWeeks);
		//			return this;
		//		}

		/// <summary>
		/// Withes the highlighted date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>FluentInitializer.</returns>
		public FluentInitializer WithHighlightedDate(DateTime date)
		{
			return WithHighlightedDates(new List<DateTime> { date });
		}
	}

	/// <summary>
	/// Delegate ClickHandler
	/// </summary>
	/// <param name="cell">The cell.</param>
	public delegate void ClickHandler(MonthCellDescriptor cell);

	public delegate bool DateSelectableHandler(DateTime date);

	/// <summary>
	/// Class DateSelectedEventArgs.
	/// </summary>
	public class DateSelectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DateSelectedEventArgs"/> class.
		/// </summary>
		/// <param name="date">The date.</param>
		public DateSelectedEventArgs(DateTime date)
		{
			SelectedDate = date;
		}

		/// <summary>
		/// Gets the selected date.
		/// </summary>
		/// <value>The selected date.</value>
		public DateTime SelectedDate { get; private set; }
	}

	/// <summary>
	/// Class MonthChangedEventArgs.
	/// </summary>
	public class MonthChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MonthChangedEventArgs"/> class.
		/// </summary>
		/// <param name="date">The date.</param>
		public MonthChangedEventArgs(DateTime date)
		{
			DisplayedMonth = date;
		}

		/// <summary>
		/// Gets the displayed month.
		/// </summary>
		/// <value>The displayed month.</value>
		public DateTime DisplayedMonth { get; private set; }
	}
}
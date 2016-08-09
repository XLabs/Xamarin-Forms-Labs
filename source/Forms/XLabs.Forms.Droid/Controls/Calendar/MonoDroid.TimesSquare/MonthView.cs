// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MonthView.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using XLabs.Forms.Droid;

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	/// <summary>
	/// Class MonthView.
	/// </summary>
	public class MonthView : LinearLayout
	{
		/// <summary>
		/// The _title
		/// </summary>
		private TextView _title;
		/// <summary>
		/// The _grid
		/// </summary>
		private CalendarGridView _grid;
        /// <summary>
        /// The _cells
        /// </summary>
        private List<List<MonthCellDescriptor>> _cells;
        /// <summary>
        /// The _click handler
        /// </summary>
        private ClickHandler _clickHandler;

        private List<CalendarCellView> cellViews;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthView"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        public MonthView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		/// <summary>
		/// Creates the specified parent.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="inflater">The inflater.</param>
		/// <param name="weekdayNameFormat">The weekday name format.</param>
		/// <param name="today">The today.</param>
		/// <param name="handler">The handler.</param>
		/// <returns>MonthView.</returns>
		public static MonthView Create(ViewGroup parent, LayoutInflater inflater, string weekdayNameFormat,
									   DateTime today, ClickHandler handler)
		{
			var view = (MonthView)inflater.Inflate(Resource.Layout.month, parent, false);

			var originalDay = today;

			var firstDayOfWeek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

			var headerRow = (CalendarRowView)view._grid.GetChildAt(0);

			for(int i = 0; i < 7; i++)
			{
				var offset = firstDayOfWeek - (int)today.DayOfWeek + i;
				today = today.AddDays(offset);
				var textView = (TextView)headerRow.GetChildAt(i);
				textView.Text = today.ToString(weekdayNameFormat);
				today = originalDay;
			}
			view._clickHandler = handler;
			return view;
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
			Logr.D("Month.OnMeasure w={0} h={1}", MeasureSpec.ToString(widthMeasureSpec),
				MeasureSpec.ToString(heightMeasureSpec));
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}

		/// <summary>
		/// Initializes the specified month.
		/// </summary>
		/// <param name="month">The month.</param>
		/// <param name="cells">The cells.</param>
		public void Init(MonthDescriptor month, List<List<MonthCellDescriptor>> cells)
		{
			Logr.D("Initializing MonthView ({0:d}) for {1}", GetHashCode(), month);
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			
			_title.Text = month.Label;
			if(_title.Typeface != null)
			{
				_title.Typeface = month.Style.MonthTitleFont;
			}
			_title.SetTextColor(month.Style.TitleForegroundColor);
			_title.SetBackgroundColor(month.Style.TitleBackgroundColor);

			_grid.DividerColor = month.Style.DateSeparatorColor;
            _cells = cells;

            cellViews = new List<CalendarCellView>();

            var headerRow = (CalendarRowView)_grid.GetChildAt(0);
			if(headerRow != null)
			{
				headerRow.SetBackgroundColor(month.Style.DayOfWeekLabelBackgroundColor);
				var week = cells[0];
				for(int c = 0; c <= 6; c++)
				{
					var headerText = (TextView)headerRow.GetChildAt(c);

					if(month.Style.ShouldHighlightDaysOfWeekLabel && week[c].IsHighlighted)
					{
						headerText.SetBackgroundColor(month.Style.HighlightedDateBackgroundColor);
					}

					headerText.SetTextColor(month.Style.DayOfWeekLabelForegroundColor);
				}
			}

			int numOfRows = cells.Count;
			_grid.NumRows = numOfRows;
			for(int i = 0; i < 6; i++)
			{
				var weekRow = (CalendarRowView)_grid.GetChildAt(i + 1);
				weekRow.ClickHandler = _clickHandler;
				if(i < numOfRows)
				{
					weekRow.Visibility = ViewStates.Visible;
					var week = cells[i];
					for(int c = 0; c < week.Count; c++)
					{
						var cell = week[c];
						var cellView = (CalendarCellView)weekRow.GetChildAt(c);

						cellView.Text = cell.Value.ToString();
						cellView.Enabled = cell.IsCurrentMonth;
						cellView.Selectable = cell.IsSelectable;
						cellView.Selected = cell.IsSelected;
						cellView.IsCurrentMonth = cell.IsCurrentMonth;
						cellView.IsToday = cell.IsToday;
						cellView.IsHighlighted = cell.IsHighlighted;
						cellView.RangeState = cell.RangeState;
						cellView.Tag = cell;
						cellView.SetStyle(month.Style);

                        cellViews.Add(cellView);
						//Logr.D("Setting cell at {0} ms", stopWatch.ElapsedMilliseconds);
					}
				} else
				{
					weekRow.Visibility = ViewStates.Gone;
				}
			}
			stopWatch.Stop();
			Logr.D("MonthView.Init took {0} ms", stopWatch.ElapsedMilliseconds);
        }

        public void SetHighlightedDaysWithEvents(DateTime[] daysWithEvents)
        {
            int numOfDaysWithEvents = daysWithEvents.Length;
            int numOfDays = cellViews.Count;

            for (int i = 0; i < numOfDays; i++)
            {
                var cellView = cellViews[i];

                for(int k = 0; k < numOfDaysWithEvents; k++)
                {
                    if ((cellView.Tag as MonthCellDescriptor).DateTime.Date == daysWithEvents[k].Date)
                    {
                        cellView.SetBackgroundColor(cellView.styleDescriptor.HighlightedDateBackgroundColor);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Finalize inflating a view from XML.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Finalize inflating a view from XML.  This is called as the last phase
        /// of inflation, after all child views have been added.
        /// </para>
        /// <para tool="javadoc-to-mdoc">Even if the subclass overrides onFinishInflate, they should always be
        /// sure to call the super method, so that we get called.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/view/View.html#onFinishInflate()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override void OnFinishInflate()
		{
			base.OnFinishInflate();
			_title = FindViewById<TextView>(Resource.Id.title);
			_grid = FindViewById<CalendarGridView>(Resource.Id.calendar_grid);
		}
	}
}
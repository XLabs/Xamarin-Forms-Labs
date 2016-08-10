// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MonthGridView.cs" company="XLabs Team">
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
using System.Globalization;
using CoreGraphics;
using UIKit;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class MonthGridView.
	/// </summary>
	public class MonthGridView : UIView
	{
		/// <summary>
		/// The _calendar month view
		/// </summary>
		private readonly CalendarMonthView _calendarMonthView;

		/// <summary>
		/// Gets or sets the current date.
		/// </summary>
		/// <value>The current date.</value>
		public DateTime CurrentDate {get;set;}
		/// <summary>
		/// The _current month
		/// </summary>
		private DateTime _currentMonth;

		/// <summary>
		/// The day tiles
		/// </summary>
		protected readonly IList<CalendarDayView> DayTiles = new List<CalendarDayView>();
		/// <summary>
		/// Gets or sets the lines.
		/// </summary>
		/// <value>The lines.</value>
		public int Lines { get; set; }
		/// <summary>
		/// Gets or sets the selected day view.
		/// </summary>
		/// <value>The selected day view.</value>
		protected CalendarDayView SelectedDayView {get;set;}
		/// <summary>
		/// The weekday of first
		/// </summary>
		public int WeekdayOfFirst;
		/// <summary>
		/// Gets or sets the marks.
		/// </summary>
		/// <value>The marks.</value>
		public IList<DateTime> Marks { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MonthGridView"/> class.
		/// </summary>
		/// <param name="calendarMonthView">The calendar month view.</param>
		/// <param name="month">The month.</param>
		public MonthGridView(CalendarMonthView calendarMonthView, DateTime month)
		{
			_calendarMonthView = calendarMonthView;
			_currentMonth = month.Date;

			var tapped = new UITapGestureRecognizer(PTapped);
			AddGestureRecognizer(tapped);
		}

		/// <summary>
		/// ps the tapped.
		/// </summary>
		/// <param name="tapRecg">The tap recg.</param>
		void PTapped(UITapGestureRecognizer tapRecg)
		{
			var loc = tapRecg.LocationInView(this);
			if (SelectDayView(loc)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, (int)SelectedDayView.Tag));
		}

		/// <summary>
		/// Updates this instance.
		/// </summary>
		public void Update(){
			foreach (var v in DayTiles)
				UpdateDayView(v);

			SetNeedsDisplay();
		}

		/// <summary>
		/// Updates the day view.
		/// </summary>
		/// <param name="dayView">The day view.</param>
		public void UpdateDayView(CalendarDayView dayView){
			dayView.Marked = _calendarMonthView.IsDayMarkedDelegate != null && _calendarMonthView.IsDayMarkedDelegate(dayView.Date);
			dayView.Available = _calendarMonthView.IsDateAvailable == null || _calendarMonthView.IsDateAvailable(dayView.Date);
			dayView.Highlighted = _calendarMonthView.HighlightedDaysOfWeek[(int)dayView.Date.DayOfWeek];
		}

		/// <summary>
		/// Builds the grid.
		/// </summary>
		public void BuildGrid ()
		{
			//DateTime dt = DateTime.Now;
			var previousMonth = _currentMonth.AddMonths (-1);
			var nextMonth = _currentMonth.AddMonths (1);
			var daysInPreviousMonth = DateTime.DaysInMonth (previousMonth.Year, previousMonth.Month);
			var daysInMonth = DateTime.DaysInMonth (_currentMonth.Year, _currentMonth.Month);
			var firstDayOfWeek = (int) CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			WeekdayOfFirst = (int)_currentMonth.DayOfWeek;
			//var lead = daysInPreviousMonth  - ((weekdayOfFirst+firstDayOfWeek) - 1);

			var boxWidth = _calendarMonthView.BoxWidth;
			var boxHeight = _calendarMonthView.BoxHeight;

			var numberOfLastMonthDays = (WeekdayOfFirst - firstDayOfWeek);
			if(numberOfLastMonthDays < 0){
				numberOfLastMonthDays = 7 - (WeekdayOfFirst + firstDayOfWeek);
			}
			var lead = daysInPreviousMonth - numberOfLastMonthDays + 1;
			// build last month's days
			for (var i = 1; i <= numberOfLastMonthDays; i++)
			{
				var viewDay = new DateTime (previousMonth.Year, previousMonth.Month, lead);
				var dayView = new CalendarDayView(_calendarMonthView)
					              {
						              Frame =
							              new CGRect((i - 1) * boxWidth, 0, boxWidth, boxHeight),
						              Date = viewDay,
						              Text = lead.ToString()
					              };
				UpdateDayView (dayView);
				AddSubview (dayView);
				DayTiles.Add (dayView);
				lead++;
			}

			var position = WeekdayOfFirst-firstDayOfWeek + 1;
			if(position == 0){
				position = 7;
			}
			var line = 0;

			// current month
			for (int i = 1; i <= daysInMonth; i++)
			{
				var viewDay = new DateTime (_currentMonth.Year, _currentMonth.Month, i);
				var dayView = new CalendarDayView(_calendarMonthView)
					              {
						              Frame =
							              new CGRect(
							              (position - 1) * boxWidth,
							              line * boxHeight,
							              boxWidth,
							              boxHeight),
						              Today = (CurrentDate.Date == viewDay.Date),
						              Text = i.ToString(),
						              Active = true,
						              Tag = i,
						              Selected =
							              (viewDay.Date == _calendarMonthView.CurrentSelectedDate.Date),
						              Date = viewDay
					              };

				UpdateDayView (dayView);

				if (dayView.Selected)
					SelectedDayView = dayView;

				AddSubview (dayView);
				DayTiles.Add (dayView);

				position++;
				if (position > 7)
				{
					position = 1;
					line++;
				}
			}

			//next month
			int dayCounter = 1;
			if (position != 1)
			{
				for (int i = position; i < 8; i++)
				{
					var viewDay = new DateTime (nextMonth.Year, nextMonth.Month, dayCounter);
					var dayView = new CalendarDayView (_calendarMonthView)
					{
						Frame = new CGRect((i - 1) * boxWidth, line * boxHeight, boxWidth, boxHeight),
						Text = dayCounter.ToString(),
					};
					dayView.Date = viewDay;
					UpdateDayView (dayView);

					AddSubview (dayView);
					DayTiles.Add (dayView);
					dayCounter++;
				}
			}

//Why to add unnecesarry unclickable dates of next month?
//			while (line < 5)
//			{
//				line++;
//				for (int i = 1; i < 8; i++)
//				{
//					var viewDay = new DateTime (_currentMonth.Year, _currentMonth.Month, i);
//					var dayView = new CalendarDayView (_calendarMonthView)
//					{
//						Frame = new RectangleF((i - 1) * boxWidth -1, line * boxHeight, boxWidth, boxHeight),
//						Text = dayCounter.ToString(),
//					};
//					dayView.Date = viewDay;
//					updateDayView (dayView);
//
//					AddSubview (dayView);
//					_dayTiles.Add (dayView);
//					dayCounter++;
//				}
//			}

			Frame = new CGRect(Frame.Location, new CGSize(Frame.Width, (line + 1) * boxHeight));

			Lines = (position == 1 ? line - 1 : line);

			if (SelectedDayView!=null)
				BringSubviewToFront(SelectedDayView);
			//Console.WriteLine("Building the grid took {0} msecs",(DateTime.Now-dt).TotalMilliseconds);
		}

		/*public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			if (SelectDayView((UITouch)touches.AnyObject)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag));
		}
		
		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			if (SelectDayView((UITouch)touches.AnyObject)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag));
		}
		
		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			if (_calendarMonthView.OnFinishedDateSelection==null) return;
			var date = new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag);
			if (_calendarMonthView.IsDateAvailable == null || _calendarMonthView.IsDateAvailable(date))
				_calendarMonthView.OnFinishedDateSelection(date);
		}*/

		/// <summary>
		/// Selects the day view.
		/// </summary>
		/// <param name="p">The p.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool SelectDayView(CGPoint p){

			int index = ((int)p.Y / _calendarMonthView.BoxHeight) * 7 + ((int)p.X / _calendarMonthView.BoxWidth);
			if(index<0 || index >= DayTiles.Count) return false;

			var newSelectedDayView = DayTiles[index];
			if (newSelectedDayView == SelectedDayView) 
				return false;

			if (!newSelectedDayView.Active){
				var day = int.Parse(newSelectedDayView.Text);
				_calendarMonthView.MoveCalendarMonths(day <= 15, true);
				return false;
			} else if (!newSelectedDayView.Active && !newSelectedDayView.Available){
				return false;
			}

			if (SelectedDayView!=null)
				SelectedDayView.Selected = false;

			BringSubviewToFront(newSelectedDayView);
			newSelectedDayView.Selected = true;

			SelectedDayView = newSelectedDayView;
			_calendarMonthView.CurrentSelectedDate =  SelectedDayView.Date;
			SetNeedsDisplay();
			return true;
		}

		/// <summary>
		/// Deselects the day view.
		/// </summary>
		public void DeselectDayView(){
			if (SelectedDayView==null) return;
			SelectedDayView.Selected= false;
			SelectedDayView = null;
			SetNeedsDisplay();
		}
	}

}

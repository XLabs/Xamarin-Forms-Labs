//
//  CalendarMonthView.cs
//
//  Converted to MonoTouch on 1/22/09 - Eduardo Scoz || http://escoz.com
//  Originally csreated by Devin Ross on 7/28/09  - tapku.com || http://github.com/devinross/tapkulibrary
//  Adapted for Xamarin.Forms.Labs project by Vratislav Kalenda on 22/09/14 http://www.applifting.cz
/*
 
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
 
 */

namespace XLabs.Forms.Controls
{
	using System;
	using System.Collections.Generic;
	using CoreGraphics;
	using System.Globalization;

	using Foundation;
	using UIKit; 

	public delegate void DateSelected(DateTime date);

	public delegate void MonthChanged(DateTime monthSelected);

	/// <summary>
	/// Class CalendarMonthView.
	/// </summary>
	public class CalendarMonthView : UIView
	{
		/// <summary>
		/// The _calendar is loaded
		/// </summary>
		private bool _calendarIsLoaded;

		/// <summary>
		/// The _left arrow
		/// </summary>
		private CalendarArrowView _leftArrow;

		/// <summary>
		/// The _max date time
		/// </summary>
		private DateTime? _maxDateTime;

		/// <summary>
		/// The _min date time
		/// </summary>
		private DateTime? _minDateTime;

		/// <summary>
		/// The _month grid view
		/// </summary>
		private MonthGridView _monthGridView;

		/// <summary>
		/// The _right arrow
		/// </summary>
		private CalendarArrowView _rightArrow;

		/// <summary>
		/// The _scroll view
		/// </summary>
		private UIScrollView _scrollView;

		/// <summary>
		/// The _header height
		/// </summary>
		private readonly int _headerHeight;

		/// <summary>
		/// The _show header
		/// </summary>
		private readonly bool _showHeader;

		/// <summary>
		/// The _show nav arrows
		/// </summary>
		private readonly bool _showNavArrows;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMonthView"/> class.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="showHeader">if set to <c>true</c> [show header].</param>
        /// <param name="showNavArrows">if set to <c>true</c> [show nav arrows].</param>
        /// <param name="width">The width.</param>
        public CalendarMonthView(DateTime selectedDate, bool showHeader, bool showNavArrows, float width = 320)
        {
            _showHeader = showHeader;
            _showNavArrows = showNavArrows;

            if (_showNavArrows)
            {
                _showHeader = true;
            }

            StyleDescriptor = new StyleDescriptor();
            HighlightDaysOfWeeks(new DayOfWeek[] { });

            if (_showHeader && _headerHeight == 0)
            {
                _headerHeight = showNavArrows ? 40 : 20;
            }

            Frame = _showHeader ? new CGRect(0, 0, width, 198 + _headerHeight) : new CGRect(0, 0, width, 198);

            BoxWidth = Convert.ToInt32(Math.Ceiling(width / 7));

            BackgroundColor = UIColor.White;

            ClipsToBounds = true;
            CurrentDate = DateTime.Now.Date;
            CurrentMonthYear = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);

            CurrentSelectedDate = selectedDate;

            var swipeLeft = new UISwipeGestureRecognizer(MonthViewSwipedLeft)
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            AddGestureRecognizer(swipeLeft);

            var swipeRight = new UISwipeGestureRecognizer(MonthViewSwipedRight)
            {
                Direction =
                    UISwipeGestureRecognizerDirection.Right
            };
            AddGestureRecognizer(swipeRight);

            var swipeUp = new UISwipeGestureRecognizer(MonthViewSwipedUp) { Direction = UISwipeGestureRecognizerDirection.Up };
            AddGestureRecognizer(swipeUp);
        }

		/// <summary>
		/// The box height
		/// </summary>
		public int BoxHeight = 30;

		/// <summary>
		/// The box width
		/// </summary>
		public int BoxWidth = 46;

		/// <summary>
		/// The current month year
		/// </summary>
		public DateTime CurrentMonthYear;

		/// <summary>
		/// The current selected date
		/// </summary>
		public DateTime CurrentSelectedDate;

		/// <summary>
		/// The selected dates items.
		/// </summary>
		public ICollection<DateTime> SelectedDatesItems;

		/// <summary>
		/// The is date available
		/// </summary>
		public Func<DateTime, bool> IsDateAvailable;

		/// <summary>
		/// The is day marked delegate
		/// </summary>
		public Func<DateTime, bool> IsDayMarkedDelegate;

		/// <summary>
		/// The month changed
		/// </summary>
		public Action<DateTime> MonthChanged;

		/// <summary>
		/// The on date selected
		/// </summary>
		public Action<DateTime> OnDateSelected;

		/// <summary>
		/// The on finished date selection
		/// </summary>
		public Action<DateTime> OnFinishedDateSelection;

		/// <summary>
		/// The swiped up
		/// </summary>
		public Action SwipedUp;

		/// <summary>
		/// Gets the highlighted days of week.
		/// </summary>
		/// <value>The highlighted days of week.</value>
		public Dictionary<int, bool> HighlightedDaysOfWeek { get; private set; }

		/// <summary>
		/// Gets or sets the current date.
		/// </summary>
		/// <value>The current date.</value>
		protected DateTime CurrentDate { get; set; }

		/// <summary>
		/// Gets the style descriptor.
		/// </summary>
		/// <value>The style descriptor.</value>
		public StyleDescriptor StyleDescriptor { get; private set; }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(StyleDescriptor.TitleBackgroundColor.CGColor);
                //Console.WriteLine("Title background color is {0}",_styleDescriptor.TitleBackgroundColor.ToString());
                context.FillRect(new CGRect(0, 0, 320, 18 + _headerHeight));
            }

            DrawDayLabels(rect);

            if (_showHeader)
            {
                DrawMonthLabel(rect);
            }
        }

		/// <summary>
		/// Sets the date.
		/// </summary>
		/// <param name="newDate">The new date.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		public void SetDate(DateTime newDate, bool animated)
		{
			var right = true;

			CurrentSelectedDate = newDate;

			var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
			if (monthsDiff != 0)
			{
				if (monthsDiff < 0)
				{
					right = false;
					monthsDiff = -monthsDiff;
				}

				for (var i = 0; i < monthsDiff; i++)
				{
					MoveCalendarMonths(right, animated);
				}
			}
			else
			{
				//If we have created the layout already
				if (_scrollView != null)
				{
					RebuildGrid(true, animated);
				}
			}
		}

		public void SetDates(ICollection<DateTime> dates, bool animated)
		{
			SelectedDatesItems = dates;

			if (_scrollView != null)
			{
				RebuildGrid(true, animated);
			}

			/*var right = true;

			CurrentSelectedDate = newDate;

			var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
			if (monthsDiff != 0)
			{
				if (monthsDiff < 0)
				{
					right = false;
					monthsDiff = -monthsDiff;
				}

				for (var i = 0; i < monthsDiff; i++)
				{
					MoveCalendarMonths(right, animated);
				}
			}
			else
			{
				//If we have created the layout already
				if (_scrollView != null)
				{
					RebuildGrid(true, animated);
				}
			}*/
		}

		/// <summary>
		/// Sets the maximum allowed date.
		/// </summary>
		/// <param name="maxDate">The maximum date.</param>
		public void SetMaxAllowedDate(DateTime? maxDate)
		{
			_maxDateTime = maxDate;
		}

		/// <summary>
		/// Sets the minimum allowed date.
		/// </summary>
		/// <param name="minDate">The minimum date.</param>
		public void SetMinAllowedDate(DateTime? minDate)
		{
			_minDateTime = minDate;
		}

		/// <summary>
		/// Highlights the days of weeks.
		/// </summary>
		/// <param name="daysOfWeeks">The days of weeks.</param>
		public void HighlightDaysOfWeeks(DayOfWeek[] daysOfWeeks)
		{
			HighlightedDaysOfWeek = new Dictionary<int, bool>();
			for (var i = 0; i <= 6; i++)
			{
				HighlightedDaysOfWeek[i] = false;
			}
			foreach (var dOw in daysOfWeeks)
			{
				HighlightedDaysOfWeek[(int)dOw] = true;
			}
		}

		/// <summary>
		/// Sets the displayed month year.
		/// </summary>
		/// <param name="newDate">The new date.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		public void SetDisplayedMonthYear(DateTime newDate, bool animated)
		{
			var right = true;
			var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
			if (monthsDiff != 0)
			{
				if (monthsDiff < 0)
				{
					right = false;
					monthsDiff = -monthsDiff;
				}

				for (var i = 0; i < monthsDiff; i++)
				{
					MoveCalendarMonths(right, animated);
				}
			}
			else
			{
				//If we have created the layout already
				if (_scrollView != null)
				{
					RebuildGrid(true, animated);
				}
			}
		}

		/// <summary>
		/// Sets the needs display.
		/// </summary>
		public override void SetNeedsDisplay()
		{
			base.SetNeedsDisplay();
			if (_monthGridView != null)
			{
				_monthGridView.Update();
			}
		}

		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			if (_calendarIsLoaded)
			{
				return;
			}

			_scrollView = new UIScrollView
				              {
					              ContentSize = new CGSize(320, 260),
					              ScrollEnabled = false,
					              Frame = new CGRect(0, 16 + _headerHeight, 320, Frame.Height - 16),
					              BackgroundColor = StyleDescriptor.BackgroundColor
				              };

			//_shadow = new UIImageView(UIImage.FromBundle("Images/Calendar/shadow.png"));

			//LoadButtons();

			LoadNavArrows();
			SetNavigationArrows(false);
			LoadInitialGrids();

			BackgroundColor = UIColor.Clear;

			AddSubview(_scrollView);

			//AddSubview(_shadow);

			_scrollView.AddSubview(_monthGridView);

			_calendarIsLoaded = true;
		}

		/// <summary>
		/// Deselects the date.
		/// </summary>
		public void DeselectDate()
		{
			if (_monthGridView != null)
			{
				_monthGridView.DeselectDayView();
			}
		}

		/// <summary>
		/// Moves the calendar months.
		/// </summary>
		/// <param name="right">if set to <c>true</c> [right].</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		public void MoveCalendarMonths(bool right, bool animated)
		{
			var newDate = CurrentMonthYear.AddMonths(right ? 1 : -1);
			if ((_minDateTime != null && newDate < _minDateTime.Value.Date)
			    || (_maxDateTime != null && newDate > _maxDateTime.Value.Date))
			{
				if (animated)
				{
					var oldX = _monthGridView.Center.X;

					_monthGridView.Center = new CGPoint(oldX, _monthGridView.Center.Y);
					Animate(
						0.25,
						() => _monthGridView.Center = new CGPoint(_monthGridView.Center.X - (right ? 40 : -40), _monthGridView.Center.Y),
						() => { Animate(0.25, () => { _monthGridView.Center = new CGPoint(oldX, _monthGridView.Center.Y); }); });
				}
				return;
			}

			CurrentMonthYear = newDate;
			SetNavigationArrows(animated);
			//If we have created the layout already
			if (_scrollView != null)
			{
				RebuildGrid(right, animated);
			}
		}

        /// <summary>
        /// Rebuilds the grid.
        /// </summary>
        /// <param name="right">if set to <c>true</c> [right].</param>
        /// <param name="animated">if set to <c>true</c> [animated].</param>
        public void RebuildGrid(bool right, bool animated)
        {
            UserInteractionEnabled = false;

            var gridToMove = CreateNewGrid(CurrentMonthYear);
            var pointsToMove = (right ? Frame.Width : -Frame.Width);

            /*if (left && gridToMove.weekdayOfFirst==0)
                pointsToMove += 44;
            if (!left && _monthGridView.weekdayOfFirst==0)
                pointsToMove -= 44;*/

            gridToMove.Frame = new CGRect(new CGPoint(pointsToMove, 0), gridToMove.Frame.Size);

            _scrollView.AddSubview(gridToMove);

            if (animated)
            {
                BeginAnimations("changeMonth");
                SetAnimationDuration(0.4);
                SetAnimationDelay(0.1);
                SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
            }

            _monthGridView.Center = new CGPoint(_monthGridView.Center.X - pointsToMove, _monthGridView.Center.Y);
            gridToMove.Center = new CGPoint(gridToMove.Center.X - pointsToMove, gridToMove.Center.Y);

            _monthGridView.Alpha = 0;

            /*_scrollView.Frame = new RectangleF(
                _scrollView.Frame.Location,
                new SizeF(_scrollView.Frame.Width, this.Frame.Height-16));
			
            _scrollView.ContentSize = _scrollView.Frame.Size;*/

            SetNeedsDisplay();

            if (animated)
            {
                CommitAnimations();
            }

            _monthGridView = gridToMove;

            UserInteractionEnabled = true;

            if (MonthChanged != null)
            {
                MonthChanged(CurrentMonthYear);
            }
        }

        /// <summary>
        /// Loads the nav arrows.
        /// </summary>
        private void LoadNavArrows()
        {
            _leftArrow = new CalendarArrowView(new CGRect(10, 9, 18, 22)) { Color = StyleDescriptor.TitleForegroundColor };
            _leftArrow.TouchUpInside += HandlePreviousMonthTouch;
            _leftArrow.Direction = CalendarArrowView.ArrowDirection.Left;
            AddSubview(_leftArrow);
            _rightArrow = new CalendarArrowView(new CGRect(320 - 22 - 10, 9, 18, 22))
            {
                Color =
                    StyleDescriptor.TitleForegroundColor
            };
            _rightArrow.TouchUpInside += HandleNextMonthTouch;
            _rightArrow.Direction = CalendarArrowView.ArrowDirection.Right;
            AddSubview(_rightArrow);
        }

        /*private void LoadButtons()
        {
            _leftButton = UIButton.FromType(UIButtonType.Custom);
            _leftButton.TouchUpInside += HandlePreviousMonthTouch;
            _leftButton.SetImage(UIImage.FromBundle("Images/Calendar/leftarrow.png"), UIControlState.Normal);
            AddSubview(_leftButton);
            _leftButton.Frame = new RectangleF(10, 0, 44, 42);
			
            _rightButton = UIButton.FromType(UIButtonType.Custom);
            _rightButton.TouchUpInside += HandleNextMonthTouch;
            _rightButton.SetImage(UIImage.FromBundle("Images/Calendar/rightarrow.png"), UIControlState.Normal);
            AddSubview(_rightButton);
            _rightButton.Frame = new RectangleF(320 - 56, 0, 44, 42);
        }*/

        /// <summary>
        /// Handles the previous month touch.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HandlePreviousMonthTouch(object sender, EventArgs e)
        {
            MoveCalendarMonths(false, true);
        }

        /// <summary>
        /// Handles the next month touch.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HandleNextMonthTouch(object sender, EventArgs e)
        {
            MoveCalendarMonths(true, true);
        }

		/// <summary>
		/// Sets the navigation arrows.
		/// </summary>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		private void SetNavigationArrows(bool animated)
		{
			var isMin = false;
			var isMax = false;
			if (_minDateTime != null)
			{
				isMin = CurrentMonthYear.Month == _minDateTime.Value.Month && CurrentMonthYear.Year == _minDateTime.Value.Year;
			}
			if (_maxDateTime != null)
			{
				isMax = CurrentMonthYear.Month == _maxDateTime.Value.Month && CurrentMonthYear.Year == _maxDateTime.Value.Year;
			}

		    if (!_showNavArrows) return;

		    Action action = () =>
		    {
                if (isMin && _leftArrow.Enabled)
                {
                    _leftArrow.Enabled = false;
                    _leftArrow.Alpha = 0;
                }
                else
                {
                    _leftArrow.Enabled = true;
                    _leftArrow.Alpha = 1;
                }

                if (isMax && _rightArrow.Enabled)
                {
                    _rightArrow.Enabled = false;
                    _rightArrow.Alpha = 0;
                }
                else
                {
                    _rightArrow.Enabled = true;
                    _rightArrow.Alpha = 1;
                }
		    };

		    if (animated)
		    {
		        Animate(0.250, action);
		    }
		    else
		    {
		        action();
		    }
		}

        /// <summary>
        /// ps the month view swiped up.
        /// </summary>
        /// <param name="ges">The ges.</param>
        private void MonthViewSwipedUp(UISwipeGestureRecognizer ges)
        {
            if (SwipedUp != null)
            {
                SwipedUp();
            }
        }

        /// <summary>
        /// ps the month view swiped right.
        /// </summary>
        /// <param name="ges">The ges.</param>
        private void MonthViewSwipedRight(UISwipeGestureRecognizer ges)
        {
            MoveCalendarMonths(false, true);
        }

        /// <summary>
        /// ps the month view swiped left.
        /// </summary>
        /// <param name="ges">The ges.</param>
        private void MonthViewSwipedLeft(UISwipeGestureRecognizer ges)
        {
            MoveCalendarMonths(true, true);
        }

		/// <summary>
		/// Creates the new grid.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>MonthGridView.</returns>
		private MonthGridView CreateNewGrid(DateTime date)
		{
			var grid = new MonthGridView(this, date) 
			{ 
				CurrentDate = CurrentDate, 
				SelectedDatesItems = SelectedDatesItems 
			};

			grid.BuildGrid();
			grid.Frame = new CGRect(0, 0, 320, Frame.Height - 16);
			return grid;
		}

		/// <summary>
		/// Loads the initial grids.
		/// </summary>
		private void LoadInitialGrids()
		{
			_monthGridView = CreateNewGrid(CurrentMonthYear);

			/*var rect = _scrollView.Frame;
			rect.Size = new SizeF { Height = (_monthGridView.Lines + 1) * 44, Width = rect.Size.Width };
			_scrollView.Frame = rect;*/

			//Frame = new RectangleF(Frame.X, Frame.Y, _scrollView.Frame.Size.Width, _scrollView.Frame.Size.Height+16);

			/*var imgRect = _shadow.Frame;
			imgRect.Y = rect.Size.Height - 132;
			_shadow.Frame = imgRect;*/
		}

		/// <summary>
		/// Draws the month label.
		/// </summary>
		/// <param name="rect">The rect.</param>
		private void DrawMonthLabel(CGRect rect)
		{
			var r = new CGRect(new CGPoint(0, 2), new CGSize { Width = 320, Height = _headerHeight });
			//			_styleDescriptor.TitleForegroundColor.SetColor();
			//			DrawString(CurrentMonthYear.ToString("MMMM yyyy"), 
			//				r, _styleDescriptor.MonthTitleFont,
			//				UILineBreakMode.WordWrap, UITextAlignment.Center);
			DrawCenteredString(
				(NSString)CurrentMonthYear.ToString("MMMM yyyy"),
				StyleDescriptor.TitleForegroundColor,
				r,
				StyleDescriptor.MonthTitleFont);
		}

		/// <summary>
		/// Draws the day labels.
		/// </summary>
		/// <param name="rect">The rect.</param>
		private void DrawDayLabels(CGRect rect)
		{
			var font = StyleDescriptor.DateLabelFont;

			var context = UIGraphics.GetCurrentContext();
			context.SaveState();
			var firstDayOfWeek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			var today = CurrentDate;
			var originalDay = today;
			for (var i = 0; i < 7; i++)
			{
				var offset = firstDayOfWeek - (int)today.DayOfWeek + i;
				today = today.AddDays(offset);
				var dateRectangle = new CGRect(i * BoxWidth, 2 + _headerHeight, BoxWidth, 15);
				if (StyleDescriptor.ShouldHighlightDaysOfWeekLabel && HighlightedDaysOfWeek[(int)today.DayOfWeek])
				{
					context.SetFillColor(StyleDescriptor.HighlightedDateBackgroundColor.CGColor);
				}
				else
				{
					context.SetFillColor(StyleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);
				}

				context.FillRect(dateRectangle);
				if (StyleDescriptor.ShouldHighlightDaysOfWeekLabel && HighlightedDaysOfWeek[(int)today.DayOfWeek])
				{
					StyleDescriptor.HighlightedDateForegroundColor.SetColor();
				}
				else
				{
					StyleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
				}
			
				DrawCenteredString( new NSString(today.ToString("ddd")),UIColor.White, dateRectangle, font);
				today = originalDay;
			}

			//			var i = 0;
			//			foreach (var d in Enum.GetNames(typeof(DayOfWeek)))
			//			{
			//				var dateRectangle = new RectangleF(i*BoxWidth, 2 + headerHeight, BoxWidth, 10);
			//				context.SetFillColorWithColor(_styleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);
			//				context.FillRect(dateRectangle);
			//				_styleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
			//				DrawString(d.Substring(0, 3),dateRectangle, font,
			//					UILineBreakMode.WordWrap, UITextAlignment.Center);
			//				i++;
			//			}
			context.RestoreState();
		}

        /// <summary>
        /// Draws the centered string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="font">The font.</param>
        private static void DrawCenteredString(NSString text, UIColor color, CGRect rect, UIFont font)
        {
            var paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
            paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
            paragraphStyle.Alignment = UITextAlignment.Center;
            var attrs = new UIStringAttributes { Font = font, ForegroundColor = color, ParagraphStyle = paragraphStyle };
            var size = text.GetSizeUsingAttributes(attrs);
            var targetRect = new CGRect(
                rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
                rect.Y + (float)Math.Floor((rect.Height - size.Height) / 2f),
                size.Width,
                size.Height);
            text.DrawString(targetRect, attrs);
        }
	}
}
using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class CalendarViewRenderer.
	/// </summary>
	public class CalendarViewRenderer : ViewRenderer<CalendarView, CalendarMonthView>
	{


		/// <summary>
		/// The _is element changing
		/// </summary>
		bool _isElementChanging;
		/// <summary>
		/// The _view
		/// </summary>
		CalendarView _view;
		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarViewRenderer"/> class.
		/// </summary>
		public CalendarViewRenderer()
		{
			_isElementChanging = false;
		}
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e)
		{
			base.OnElementChanged(e);
			_view = Element;
			var calendarView = new CalendarMonthView(DateTime.MinValue, true, Element.ShowNavigationArrows);

			calendarView.OnDateSelected += (date) =>
			{
				ProtectFromEventCycle(() =>
				{
					_view.NotifyDateSelected(date);
				});
			};
			calendarView.MonthChanged += (date) =>
			{
				ProtectFromEventCycle(() =>
				{
					_view.NotifyDisplayedMonthChanged(date);
				});
			};

			SetNativeControl(calendarView);
			Control.HighlightDaysOfWeeks(Element.HighlightedDaysOfWeek);
			SetColors();
			SetFonts();

			Control.SetMinAllowedDate(Element.MinDate);
			Control.SetMaxAllowedDate(Element.MaxDate);
			calendarView.SetDisplayedMonthYear(e.NewElement.DisplayedMonth, false);

		}

		/// <summary>
		/// Protects from event cycle.
		/// </summary>
		/// <param name="action">The action.</param>
		private void ProtectFromEventCycle(Action action)
		{
			if (_isElementChanging == false)
			{
				_isElementChanging = true;
				action.Invoke();
				_isElementChanging = false;
			}
		}

		/// <summary>
		/// Sets the fonts.
		/// </summary>
		private void SetFonts()
		{
			if (Element.DateLabelFont != Font.Default)
			{
				Control.StyleDescriptor.DateLabelFont = Element.DateLabelFont.ToUIFont();
			}
			if (Element.MonthTitleFont != Font.Default)
			{
				Control.StyleDescriptor.MonthTitleFont = Element.MonthTitleFont.ToUIFont();
			}
		}

		/// <summary>
		/// Sets the colors.
		/// </summary>
		private void SetColors()
		{
			if (Element.BackgroundColor != Color.Default)
			{
				BackgroundColor = Element.BackgroundColor.ToUIColor();
				Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
				Control.StyleDescriptor.BackgroundColor = Element.BackgroundColor.ToUIColor();
			}

			//Month title
			if (Element.ActualMonthTitleBackgroundColor != Color.Default)
				Control.StyleDescriptor.TitleBackgroundColor = Element.ActualMonthTitleBackgroundColor.ToUIColor();
			if (Element.ActualMonthTitleForegroundColor != Color.Default)
				Control.StyleDescriptor.TitleForegroundColor = Element.ActualMonthTitleForegroundColor.ToUIColor();

			//Navigation color arrows
			//			if(Element.ActualNavigationArrowsColor != Color.Default){
			//				_leftArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
			//				_rightArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
			//			}else{
			//				_leftArrow.Color = Control.StyleDescriptor.TitleForegroundColor;
			//				_rightArrow.Color = Control.StyleDescriptor.TitleForegroundColor;
			//			}

			//Day of week label
			if (Element.ActualDayOfWeekLabelBackroundColor != Color.Default)
			{
				Control.StyleDescriptor.DayOfWeekLabelBackgroundColor = Element.ActualDayOfWeekLabelBackroundColor.ToUIColor();
			}
			if (Element.ActualDayOfWeekLabelForegroundColor != Color.Default)
			{
				Control.StyleDescriptor.DayOfWeekLabelForegroundColor = Element.ActualDayOfWeekLabelForegroundColor.ToUIColor();
			}

			Control.StyleDescriptor.ShouldHighlightDaysOfWeekLabel = Element.ShouldHighlightDaysOfWeekLabels;

			//Default date color
			if (Element.ActualDateBackgroundColor != Color.Default)
			{
				Control.StyleDescriptor.DateBackgroundColor = Element.ActualDateBackgroundColor.ToUIColor();
			}
			if (Element.ActualDateForegroundColor != Color.Default)
			{
				Control.StyleDescriptor.DateForegroundColor = Element.ActualDateForegroundColor.ToUIColor();
			}

			//Inactive Default date color
			if (Element.ActualInactiveDateBackgroundColor != Color.Default)
			{
				Control.StyleDescriptor.InactiveDateBackgroundColor = Element.ActualInactiveDateBackgroundColor.ToUIColor();
			}
			if (Element.ActualInactiveDateForegroundColor != Color.Default)
			{
				Control.StyleDescriptor.InactiveDateForegroundColor = Element.ActualInactiveDateForegroundColor.ToUIColor();
			}

			//Today date color
			if (Element.ActualTodayDateBackgroundColor != Color.Default)
			{
				Control.StyleDescriptor.TodayBackgroundColor = Element.ActualTodayDateBackgroundColor.ToUIColor();
			}
			if (Element.ActualTodayDateForegroundColor != Color.Default)
			{
				Control.StyleDescriptor.TodayForegroundColor = Element.ActualTodayDateForegroundColor.ToUIColor();
			}

			//Highlighted date color
			if (Element.ActualHighlightedDateBackgroundColor != Color.Default)
			{
				Control.StyleDescriptor.HighlightedDateBackgroundColor = Element.ActualHighlightedDateBackgroundColor.ToUIColor();
			}
			if (Element.ActualHighlightedDateForegroundColor != Color.Default)
			{
				Control.StyleDescriptor.HighlightedDateForegroundColor = Element.ActualHighlightedDateForegroundColor.ToUIColor();
			}



			//Selected date
			if (Element.ActualSelectedDateBackgroundColor != Color.Default)
				Control.StyleDescriptor.SelectedDateBackgroundColor = Element.ActualSelectedDateBackgroundColor.ToUIColor();
			if (Element.ActualSelectedDateForegroundColor != Color.Default)
				Control.StyleDescriptor.SelectedDateForegroundColor = Element.ActualSelectedDateForegroundColor.ToUIColor();

			//Selection styles
			Control.StyleDescriptor.SelectionBackgroundStyle = Element.SelectionBackgroundStyle;
			Control.StyleDescriptor.TodayBackgroundStyle = Element.TodayBackgroundStyle;

			//Divider
			//TODO: Implement it on iOS
			if (Element.DateSeparatorColor != Color.Default)
				Control.StyleDescriptor.DateSeparatorColor = Element.DateSeparatorColor.ToUIColor();

		}



		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			ProtectFromEventCycle(() =>
			{
				if (e.PropertyName == CalendarView.DisplayedMonthProperty.PropertyName)
				{
					Control.SetDisplayedMonthYear(Element.DisplayedMonth, false);
				}
				if (e.PropertyName == CalendarView.SelectedDateProperty.PropertyName)
				{
					//Maybe someone will find time to make date deselectable...
					if (Element.SelectedDate != null)
					{
						Control.SetDate(Element.SelectedDate.Value, false);
					}
				}
				if (e.PropertyName == CalendarView.MinDateProperty.PropertyName)
				{
					Control.SetMinAllowedDate(Element.MinDate);
				}
				if (e.PropertyName == CalendarView.MaxDateProperty.PropertyName)
				{
					Control.SetMaxAllowedDate(Element.MaxDate);
				}
			});

		}
	}
}
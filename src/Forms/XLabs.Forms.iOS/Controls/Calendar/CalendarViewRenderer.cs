// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CalendarViewRenderer.cs" company="XLabs Team">
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
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CalendarViewRenderer.
    /// </summary>
    public class CalendarViewRenderer : ViewRenderer<CalendarView, CalendarMonthView>
    {
        private readonly object elementLock = new object();
        private bool isElementChanging;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewRenderer"/> class.
        /// </summary>
        public CalendarViewRenderer()
        {
            this.isElementChanging = false;
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e)
        {
            base.OnElementChanged(e);

            if (disposed || e.NewElement == null) return;

            if (Control == null)
            {
                var calendarView = new CalendarMonthView(DateTime.MinValue, true, e.NewElement.ShowNavigationArrows);
                SetNativeControl (calendarView);
                calendarView.OnDateSelected += OnDateSelected;
                calendarView.MonthChanged += MonthChanged;
            }

            this.Control.HighlightDaysOfWeeks(e.NewElement.HighlightedDaysOfWeek);
            SetColors();
            SetFonts();

            this.Control.SetMinAllowedDate(e.NewElement.MinDate);
            this.Control.SetMaxAllowedDate(e.NewElement.MaxDate);
            this.Control.SetDisplayedMonthYear(e.NewElement.DisplayedMonth, false);
        }

        private void MonthChanged(DateTime dateTime)
        {
            ProtectFromEventCycle(() =>
            {
                this.Element.NotifyDisplayedMonthChanged(dateTime);
            });
        }

        private void OnDateSelected(DateTime dateTime)
        {
            ProtectFromEventCycle(() =>
            {
                this.Element.NotifyDateSelected(dateTime);
            });
        }

        /// <summary>
        /// Protects from event cycle.
        /// </summary>
        /// <param name="action">The action.</param>
        private void ProtectFromEventCycle(Action action)
        {
            bool changing;

            lock (this.elementLock)
            {
                changing = this.isElementChanging;
            }

            if (changing) return;

            try
            {
                this.isElementChanging = true;
                action();
            }
            finally
            {
                this.isElementChanging = false;            
            }
        }

        /// <summary>
        /// Sets the fonts.
        /// </summary>
        private void SetFonts()
        {
            if (this.Element.DateLabelFont != Font.Default)
            {
                this.Control.StyleDescriptor.DateLabelFont = this.Element.DateLabelFont.ToUIFont();
            }
            if (this.Element.MonthTitleFont != Font.Default)
            {
                this.Control.StyleDescriptor.MonthTitleFont = this.Element.MonthTitleFont.ToUIFont();
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
                    this.Control.SetDisplayedMonthYear(this.Element.DisplayedMonth, false);
                }
                else if (e.PropertyName == CalendarView.SelectedDateProperty.PropertyName)
                {
                    //Maybe someone will find time to make date deselectable...
                    if (this.Element.SelectedDate != null)
                    {
                        this.Control.SetDate(this.Element.SelectedDate.Value, false);
                    }
                }
                else if (e.PropertyName == CalendarView.MinDateProperty.PropertyName)
                {
                    this.Control.SetMinAllowedDate(this.Element.MinDate);
                }
                else if (e.PropertyName == CalendarView.MaxDateProperty.PropertyName)
                {
                    this.Control.SetMaxAllowedDate(this.Element.MaxDate);
                }
            });

        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.Control != null)
                {
                    this.Control.OnDateSelected -= OnDateSelected;
                    this.Control.MonthChanged -= MonthChanged;
                    this.Control.Dispose ();
                }
                this.disposed = true;
            }
            base.Dispose (disposing);
        }
    }
}

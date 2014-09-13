using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample
{
  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class CalendarPage : BaseView
    {
        CalendarView _calendarView;
        StackLayout _stacker;

        public CalendarPage()
        {
            InitializeComponent();
            _stacker = new StackLayout();
            Content = _stacker;

            _calendarView = new CalendarView()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            _stacker.Children.Add(_calendarView);
            _calendarView.DateSelected += (object sender, DateTime e) =>
            {
                _stacker.Children.Add(new Label()
                    {
                        Text = "Date Was Selected" + e.ToString("d"),
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    });
            };
        }
    }
}


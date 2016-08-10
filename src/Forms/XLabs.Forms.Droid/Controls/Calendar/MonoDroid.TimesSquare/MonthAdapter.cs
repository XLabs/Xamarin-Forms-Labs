// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MonthAdapter.cs" company="XLabs Team">
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

using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Java.Interop;
using System;
using System.Diagnostics;

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	/// <summary>
	/// Class MonthAdapter.
	/// </summary>
	public class MonthAdapter : PagerAdapter
	{
		/// <summary>
		/// The _inflater
		/// </summary>
		private readonly LayoutInflater _inflater;
		/// <summary>
		/// The _calendar
		/// </summary>
		private readonly CalendarPickerView _calendar;
		/// <summary>
		/// The _reusable month view
		/// </summary>
		private MonthView _reusableMonthView = null;
        /// <summary>
        /// The _active month views
        /// </summary>
        Dictionary<int,MonthView> _activeMonthViews;

        public Dictionary<int, MonthView> ActiveMonthViews {
            get { return _activeMonthViews; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthAdapter"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="calendar">The calendar.</param>
        public MonthAdapter(Context context, CalendarPickerView calendar)
		{
			_calendar = calendar;
			_inflater = LayoutInflater.From(context);
			_activeMonthViews = new Dictionary<int, MonthView>();

		}




		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public override int Count {
			get { return _calendar.Months.Count; }
		}

		#region implemented abstract members of PagerAdapter

		/// <summary>
		/// Determines whether [is view from object] [the specified view].
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="object">The object.</param>
		/// <returns><c>true</c> if [is view from object] [the specified view]; otherwise, <c>false</c>.</returns>
		public override bool IsViewFromObject(Android.Views.View view, Java.Lang.Object @object)
		{
			return view == @object;
		}

		#endregion

		/// <summary>
		/// Gets the width of the page.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>System.Single.</returns>
		public override float GetPageWidth(int position)
		{
			return 1f;
		}




		/// <summary>
		/// Instantiates the item.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="position">The position.</param>
		/// <returns>Java.Lang.Object.</returns>
		public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            Java.Lang.Object obj = container;
			var pager = obj.JavaCast<ViewPager>();

            MonthView monthView = null;

            if (_reusableMonthView == null)
			{
				monthView = MonthView.Create(pager, _inflater, _calendar.WeekdayNameFormat, _calendar.Today, _calendar.ClickHandler);
			} else
			{
				monthView = _reusableMonthView;
				_reusableMonthView = null;
			}
            
			monthView.Init(_calendar.Months[position], _calendar.Cells[position]);
			//monthView.SetBackgroundColor(global::Android.Graphics.Color.Orange);

			pager.AddView(monthView);
			_activeMonthViews[position] = monthView;
			return monthView;
        }

        /// <summary>
        /// Notifies the data set changed.
        /// </summary>
        public override void NotifyDataSetChanged()
		{

			foreach(var position in _activeMonthViews.Keys)
			{
				var view = _activeMonthViews[position];
				view.Init(_calendar.Months[position], _calendar.Cells[position]);
			}
			base.NotifyDataSetChanged();
		}


		/// <summary>
		/// Destroys the item.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="position">The position.</param>
		/// <param name="object">The object.</param>
		public override void DestroyItem(Android.Views.View container, int position, Java.Lang.Object @object)
		{
			//activePickerViews[position].OnDateSelected -= HandleOnDateSelected;
			//activePickerViews.Remove(position);
			var monthView = @object.JavaCast<MonthView>();
			(container.JavaCast<Android.Support.V4.View.ViewPager>()).RemoveView(monthView);
			_reusableMonthView = monthView;
			_activeMonthViews.Remove(position);
		}

		//        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
		//        {
		//			MonthView monthView = null;
		//
		//
		//			monthView = (MonthView)convertView ??
		//				               MonthView.Create(parent, _inflater, _calendar.WeekdayNameFormat, _calendar.Today,
		//					               _calendar.ClickHandler);
		//			monthView.Init(_calendar.Months[position], _calendar.Cells[position]);
		//
		//			return monthView;
		//
		//        }
	}
}
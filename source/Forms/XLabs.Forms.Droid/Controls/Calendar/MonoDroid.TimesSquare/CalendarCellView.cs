// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CalendarCellView.cs" company="XLabs Team">
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
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	/// <summary>
	/// Class CalendarCellView.
	/// </summary>
	public class CalendarCellView : TextView
	{

		/// <summary>
		/// The _is selectable
		/// </summary>
		private bool _isSelectable;
		/// <summary>
		/// The _is current month
		/// </summary>
		private bool _isCurrentMonth;
		/// <summary>
		/// The _is today
		/// </summary>
		private bool _isToday;
		/// <summary>
		/// The _is highlighted
		/// </summary>
		private bool _isHighlighted;
		/// <summary>
		/// The _range state
		/// </summary>
		private RangeState _rangeState = RangeState.None;

        public StyleDescriptor styleDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarCellView"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="transfer">The transfer.</param>
        public CalendarCellView(IntPtr handle, JniHandleOwnership transfer)
			: base(handle, transfer)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarCellView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public CalendarCellView(Context context)
			: base(context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarCellView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		public CalendarCellView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarCellView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		/// <param name="defStyle">The definition style.</param>
		public CalendarCellView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="CalendarCellView"/> is selectable.
		/// </summary>
		/// <value><c>true</c> if selectable; otherwise, <c>false</c>.</value>
		public bool Selectable {
			set {
				_isSelectable = value;
			}
		}

		/// <summary>
		/// Sets a value indicating whether this instance is current month.
		/// </summary>
		/// <value><c>true</c> if this instance is current month; otherwise, <c>false</c>.</value>
		public bool IsCurrentMonth {
			set {
				_isCurrentMonth = value;
			}
		}

		/// <summary>
		/// Sets a value indicating whether this instance is today.
		/// </summary>
		/// <value><c>true</c> if this instance is today; otherwise, <c>false</c>.</value>
		public bool IsToday {
			set {
				_isToday = value;
			}
		}

		/// <summary>
		/// Sets a value indicating whether this instance is highlighted.
		/// </summary>
		/// <value><c>true</c> if this instance is highlighted; otherwise, <c>false</c>.</value>
		public bool IsHighlighted {
			set {
				_isHighlighted = value;
			}
		}

		/// <summary>
		/// Sets the state of the range.
		/// </summary>
		/// <value>The state of the range.</value>
		public RangeState RangeState {
			set {
				_rangeState = value;
			}
		}

		/// <summary>
		/// Sets the style.
		/// </summary>
		/// <param name="style">The style.</param>
		public void SetStyle(StyleDescriptor style)
		{
            styleDescriptor = style;

            if (style.DateLabelFont != null)
			{
				this.Typeface = (style.DateLabelFont);
			}
			if(this.Selected)
			{
				SetBackgroundColor(style.SelectedDateBackgroundColor);
				SetTextColor(style.SelectedDateForegroundColor);
			} else if(_isToday)
			{
				SetBackgroundColor(style.TodayBackgroundColor);
				SetTextColor(style.TodayForegroundColor);
			} else if(_isHighlighted)
			{
				SetBackgroundColor(style.HighlightedDateBackgroundColor);
				if(_isCurrentMonth)
				{
					SetTextColor(style.HighlightedDateForegroundColor);
				} else
				{
					SetTextColor(style.InactiveDateForegroundColor);
				}
			} else if(!_isCurrentMonth)
			{
				SetBackgroundColor(style.InactiveDateBackgroundColor);
				SetTextColor(style.InactiveDateForegroundColor);
			} else
			{
				SetBackgroundColor(style.DateBackgroundColor);
				SetTextColor(style.DateForegroundColor);
			}
		}
	}
}

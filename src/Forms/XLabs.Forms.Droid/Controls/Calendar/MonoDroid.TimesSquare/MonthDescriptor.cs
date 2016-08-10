// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MonthDescriptor.cs" company="XLabs Team">
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

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	/// <summary>
	/// Class MonthDescriptor.
	/// </summary>
	public class MonthDescriptor
	{
		/// <summary>
		/// Gets the month.
		/// </summary>
		/// <value>The month.</value>
		public int Month { get; private set; }

		/// <summary>
		/// Gets the year.
		/// </summary>
		/// <value>The year.</value>
		public int Year { get; private set; }

		/// <summary>
		/// Gets the date.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date { get; private set; }

		/// <summary>
		/// Gets the label.
		/// </summary>
		/// <value>The label.</value>
		public string Label { get; private set; }

		/// <summary>
		/// Gets the style.
		/// </summary>
		/// <value>The style.</value>
		public StyleDescriptor Style { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MonthDescriptor"/> class.
		/// </summary>
		/// <param name="month">The month.</param>
		/// <param name="year">The year.</param>
		/// <param name="date">The date.</param>
		/// <param name="label">The label.</param>
		/// <param name="style">The style.</param>
		public MonthDescriptor(int month, int year, DateTime date, string label, StyleDescriptor style)
		{
			Month = month;
			Year = year;
			Date = date;
			Label = label;
			Style = style;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return "MonthDescriptor{"
			+ "label=" + Label + ""
			+ ", month=" + Month
			+ ", year=" + Year
			+ "}";
		}
	}
}
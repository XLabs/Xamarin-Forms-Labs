// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SegmentControl.cs" company="XLabs Team">
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

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class SegmentControl.
	/// </summary>
	public class SegmentControl : ContentView
	{
		/// <summary>
		/// The layout
		/// </summary>
		private readonly StackLayout _layout;

		/// <summary>
		/// The tint color
		/// </summary>
		private Color _tintColor = Color.Black;

		/// <summary>
		/// Gets or sets the color of the tint.
		/// </summary>
		/// <value>The color of the tint.</value>
		public Color TintColor
		{
			get { return _tintColor; }
			set
			{
				_tintColor = value;

				if (_layout == null)
				{
					return;
				}

				_layout.BackgroundColor = _tintColor;

				for (var iBtn = 0; iBtn < _layout.Children.Count; iBtn++)
				{
					SetSelectedState(iBtn, iBtn == _selectedSegment, true);
				}
			}
		}

		/// <summary>
		/// The selected segment
		/// </summary>
		private int _selectedSegment;

		/// <summary>
		/// Gets or sets the selected segment.
		/// </summary>
		/// <value>The selected segment.</value>
		public int SelectedSegment
		{
			get
			{
				return _selectedSegment;
			}
			set
			{
				//reset the original selected segment
				if (value == _selectedSegment)
				{
					return;
				}

				SetSelectedState(_selectedSegment, false);
				_selectedSegment = value;

				if (value < 0 || value >= _layout.Children.Count)
				{
					return;
				}

				SetSelectedState(_selectedSegment, true);
			}
		}

		/// <summary>
		/// Occurs when [selected segment changed].
		/// </summary>
		public event EventHandler<int> SelectedSegmentChanged;

		/// <summary>
		/// The clicked command
		/// </summary>
		private readonly Command _clickedCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="SegmentControl"/> class.
		/// </summary>
		public SegmentControl()
		{
			_layout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(1),
				Spacing = 1
			};

			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.Start;
			Padding = new Thickness(0, 0);
			Content = _layout;

			_selectedSegment = 0;
			_clickedCommand = new Command(SetSelectedSegment);
		}

		/// <summary>
		/// Adds the segment.
		/// </summary>
		/// <param name="segmentText">The segment text.</param>
		public void AddSegment(string segmentText)
		{
			// TODO: TextColor needs to be a bound property
			var button = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BorderColor = TintColor,
				BorderRadius = 0,
				BorderWidth = 0,
				Text = segmentText,
				TextColor = TintColor,
				BackgroundColor = Color.White,
				Command = _clickedCommand,
				CommandParameter = _layout.Children.Count,
			};

			_layout.BackgroundColor = TintColor;
			_layout.Children.Add(button);

			SetSelectedState(_layout.Children.Count - 1, _layout.Children.Count - 1 == _selectedSegment);
		}

		/// <summary>
		/// Sets the selected segment.
		/// </summary>
		/// <param name="o">The o.</param>
		private void SetSelectedSegment(object o)
		{
			var selectedIndex = (int)o;

			SelectedSegment = selectedIndex;

			if (SelectedSegmentChanged != null)
			{
				SelectedSegmentChanged(this, selectedIndex);
			}
		}

		/// <summary>
		/// Sets the segment text.
		/// </summary>
		/// <param name="iSegment">The i segment.</param>
		/// <param name="segmentText">The segment text.</param>
		/// <exception cref="System.IndexOutOfRangeException">SetSegmentText: Attempted to change segment text for a segment doesn't exist.</exception>
		public void SetSegmentText(int iSegment, string segmentText)
		{
			if (iSegment >= _layout.Children.Count || iSegment < 0)
			{
				throw new IndexOutOfRangeException("SetSegmentText: Attempted to change segment text for a segment doesn't exist.");
			}

			((Button)_layout.Children[iSegment]).Text = segmentText;
		}

		/// <summary>
		/// Sets the state of the selected.
		/// </summary>
		/// <param name="indexer">The indexer.</param>
		/// <param name="isSelected">if set to <c>true</c> [is selected].</param>
		/// <param name="setBorderColor">if set to <c>true</c> [set border color].</param>
		private void SetSelectedState(int indexer, bool isSelected, bool setBorderColor = false)
		{
			if (_layout.Children.Count <= indexer)
			{
				return; //Out of bounds
			}

			var button = (Button)_layout.Children[indexer];

			// TODO: TextColor needs to be a bound property
			if (isSelected)
			{
				button.BackgroundColor = TintColor;
				button.TextColor = Color.White;
			}
			else
			{
				button.BackgroundColor = Color.White;
				button.TextColor = TintColor;
			}

			if (setBorderColor)
			{
				button.BorderColor = TintColor;
			}
		}
	}
}
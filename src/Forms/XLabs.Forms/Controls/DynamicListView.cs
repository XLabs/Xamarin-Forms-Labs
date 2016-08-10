// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DynamicListView.cs" company="XLabs Team">
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
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// The dynamic native list view.
	/// </summary>
	/// <typeparam name="T">
	/// Type of items in the list view
	/// </typeparam>
	public class DynamicListView<T> : View
	{
		////private Predicate<T> filter;

		/// <summary>
		/// The data.
		/// </summary>
		private ObservableCollection<T> _data;

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicListView{T}"/> class.
		/// </summary>
		public DynamicListView()
		{
			this.Data = new ObservableCollection<T>();
		}

		//public Predicate<T> Filter
		//{
		//    get { return this.filter; }
		//    set
		//    {
		//        this.filter = value;
		//        this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		//    }
		//}

		/// <summary>
		/// Occurs when item is selected.
		/// </summary>
		public event EventHandler<EventArgs<T>> OnSelected;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
		/// <summary>
		/// The requested event occurs when an observer requests an item.
		/// </summary>
		/// <remarks>The sender will be the requesting observer, f.e. a ListView in Android
		/// or UITableView in iOS.</remarks>
		//public event EventHandler<EventArgs<int>> OnRequested;
		
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
		/// <summary>
		/// Add items to data collection.
		/// </summary>
		/// <param name="item">
		/// The item.
		/// </param>
		public void Add(T item)
		{
			this.Data.Add(item);
		}

		/// <summary>
		/// Replaces an object in collection.
		/// </summary>
		/// <param name="original">
		/// The original object.
		/// </param>
		/// <param name="replacement">
		/// The replacement object.
		/// </param>
		/// <returns>
		/// <see cref="bool"/>, true if replacement was successful, false if original object was not found.
		/// </returns>
		public bool Replace(T original, T replacement)
		{
			var index = this.Data.IndexOf (original);

			if (index < 0) 
			{
				return false;
			}

			this.Data[index] = replacement;

			return true;
		}

		/// <summary>
		/// The remove item method.
		/// </summary>
		/// <param name="item">
		/// The item to remove.
		/// </param>
		public void Remove(T item)
		{
			this.Data.Remove(item);
		}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public ObservableCollection<T> Data 
		{
			get 
			{
				return this._data;
			}

			set 
			{
				this.OnPropertyChanging();
				this._data = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Invokes the item selected event.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="item">
		/// Item that was selected.
		/// </param>
		public void InvokeItemSelectedEvent(object sender, T item)
		{
			if (this.OnSelected != null)
			{
				this.OnSelected.Invoke (sender, new EventArgs<T>(item));
			}
		}

	
		//public void InvokeItemRequestedEvent(object sender, int index)
		//{
		//    if (this.OnRequested != null)
		//    {
		//        this.OnRequested(sender, new EventArgs<int>(index));
		//    }
		//}
	}
}

// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="OnPlatformList.cs" company="XLabs Team">
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

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms
{
    //From the Xamarin forums 
    //http://forums.xamarin.com/discussion/comment/68739#Comment_68739

    /// <summary>
    /// Class OnPlatformList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnPlatformList<T> : ObservableCollection<T>
    {
        private ObservableCollection<T> _realData;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnPlatformList{T}"/> class.
        /// </summary>
        public OnPlatformList()
        {
            iOS = new ObservableCollection<T>();
            Android = new ObservableCollection<T>();
            WinPhone = new ObservableCollection<T>();

            // Unfortunately, there's no way to see the complete initialization of the tag in XAML,
            // in this case, the ctor fires before we've seen the collection data
            // so hook the collections and wait for each one to change - 
            // when we find the right one, we'll pull it's data and ignore the others.
            iOS.CollectionChanged += OnInitialize;
            Android.CollectionChanged += OnInitialize;
            WinPhone.CollectionChanged += OnInitialize;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets the i os.
        /// </summary>
        /// <value>The i os.</value>
        public ObservableCollection<T> iOS { get; private set; }
        /// <summary>
        /// Gets the android.
        /// </summary>
        /// <value>The android.</value>
        public ObservableCollection<T> Android { get; private set; }
        /// <summary>
        /// Gets the win phone.
        /// </summary>
        /// <value>The win phone.</value>
        public ObservableCollection<T> WinPhone { get; private set; }

        void OnInitialize(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool foundCollection = false;
            if (Device.OS == TargetPlatform.iOS && sender == iOS)
            {
                Setup(iOS);
                foundCollection = true;
            }
            else if (Device.OS == TargetPlatform.Android && sender == Android)
            {
                Setup(Android);
                foundCollection = true;
            }
            else if (Device.OS == TargetPlatform.WinPhone && sender == WinPhone)
            {
                Setup(WinPhone);
                foundCollection = true;
            }

            if (foundCollection)
            {
                iOS.CollectionChanged -= OnInitialize;
                Android.CollectionChanged -= OnInitialize;
                WinPhone.CollectionChanged -= OnInitialize;
            }
        }

        void Setup(ObservableCollection<T> data)
        {
            _realData = data;
            foreach (var item in data) Add(item);

            data.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems.Cast<T>()) Add(item);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems.Cast<T>()) Remove(item);
                        break;
                    // TODO: add other operations.
                    default:
                        Clear();
                        foreach (var item in _realData) Add(item);
                        break;
                }

            };
        }
    }
}

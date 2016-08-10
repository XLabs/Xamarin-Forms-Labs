// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BindablePicker.cs" company="XLabs Team">
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
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class BindablePicker.
    /// </summary>
    public class BindablePicker : View
    {
        /// <summary>
        /// The HasBorder property
        /// </summary>
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedEntry), true);

        /// <summary>
        /// The title property
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create((BindablePicker w) => w.Title, null);

        /// <summary>
        /// The selected index property
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create<BindablePicker, int>(w => w.SelectedIndex, -1, BindingMode.TwoWay, null, delegate(BindableObject bindable, int oldvalue, int newvalue)
        {
            var selectedIndexChanged = ((BindablePicker)bindable).SelectedIndexChanged;
            if (selectedIndexChanged != null)
            {
                selectedIndexChanged(bindable, EventArgs.Empty);
            }
        }, null, CoerceSelectedIndex);

        /// <summary>
        /// Occurs when [selected index changed].
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindablePicker"/> class.
        /// </summary>
        public BindablePicker()
        {
            this.Items = new ObservableCollection<string>();
            this.Items.CollectionChanged += this.OnItemsCollectionChanged;
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        /// <summary>
        /// Gets or sets the souce item label converter.
        /// </summary>
        /// <value>The souce item label converter.</value>
        public Func<object, string> SourceItemLabelConverter { get; set; }

        /// <summary>
        /// The items source property
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IList>(o => o.ItemsSource, default(IList), propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// The selected item property
        /// </summary>
        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object), BindingMode.TwoWay,propertyChanged: OnSelectedItemChanged);

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public ObservableCollection<string> Items
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets if the border should be shown or not
        /// </summary>
        /// <value><c>true</c> if this instance has border; otherwise, <c>false</c>.</value>
        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }	

        /// <summary>
        /// Called when [items source changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">The oldvalue.</param>
        /// <param name="newvalue">The newvalue.</param>
        private static void OnItemsSourceChanged(BindableObject bindable, IList oldvalue, IList newvalue)
        {
            var picker = bindable as BindablePicker;

            if (picker == null) return;

            picker.Items.Clear();

            if (newvalue == null) return;

            foreach (var item in newvalue)
            {
                picker.Items.Add(picker.SourceItemLabelConverter != null
                    ? picker.SourceItemLabelConverter(item)
                    : item.ToString());
            }
        }

        /// <summary>
        /// Handles the <see cref="E:SelectedIndexChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            this.SelectedItem = (SelectedIndex < 0 || SelectedIndex > Items.Count - 1) ? null : ItemsSource[SelectedIndex];
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">The oldvalue.</param>
        /// <param name="newvalue">The newvalue.</param>
        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as BindablePicker;

            if (picker == null) return;

            if (newvalue != null) 
            {
                var title = (picker.SourceItemLabelConverter != null) ? picker.SourceItemLabelConverter(newvalue) : newvalue.ToString();
                picker.SelectedIndex = picker.Items.IndexOf(title);
            } 
            else 
            {
                picker.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Coerces the index of the selected.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        private static int CoerceSelectedIndex(BindableObject bindable, int value)
        {
            var picker = bindable as BindablePicker;
            return (picker == null || picker.Items == null) ? - 1 : value.Clamp(-1, picker.Items.Count-1);
        }

        /// <summary>
        /// Handles the <see cref="E:ItemsCollectionChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedItem == null) return;

            var title = SourceItemLabelConverter != null ? SourceItemLabelConverter (SelectedItem) : SelectedItem.ToString ();
            SelectedIndex = Items.IndexOf (title);
        }
    }
}


// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BindableRadioGroup.cs" company="XLabs Team">
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
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class BindableRadioGroup.
    /// </summary>
    public class BindableRadioGroup : StackLayout
    { 

        /// <summary>
        /// The items
        /// </summary>
        public ObservableCollection<CustomRadioButton> Items;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableRadioGroup"/> class.
        /// </summary>
        public BindableRadioGroup()
        {
            Items = new ObservableCollection<CustomRadioButton>();
        }

        /// <summary>
        /// The items source property
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
                    BindableProperty.Create<BindableRadioGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// The selected index property
        /// </summary>
        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<BindableRadioGroup, int>(o => o.SelectedIndex, default(int), BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged);


        /// <summary>
        /// The text color property
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create<CheckBox, Color>(
                p => p.TextColor, Color.Black);

        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create<CheckBox, double>(
                p => p.FontSize, -1);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty =
            BindableProperty.Create<CheckBox, string>(
                p => p.FontName, string.Empty);

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set{ SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get
            {
                return (string)GetValue(FontNameProperty);
            }
            set
            {
                SetValue(FontNameProperty, value);
            }
        }

        /// <summary>
        /// Occurs when [checked changed].
        /// </summary>
        public event EventHandler<int> CheckedChanged;

        private void OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (e.Value == false)
            {
                return;
            }

            var selectedItem = sender as CustomRadioButton;

            if (selectedItem == null)
            {
                return;
            }

            foreach (var item in Items)
            {
                if (!selectedItem.Id.Equals(item.Id))
                {
                    item.Checked = false;
                }
                else
                {
                    SelectedIndex = selectedItem.Id;
                    if (CheckedChanged != null)
                    {
                        CheckedChanged.Invoke(sender, item.Id);
                    }
                }
            }
        }

        private static void OnSelectedIndexChanged(BindableObject bindable, int oldvalue, int newvalue)
        {
            if (newvalue == -1)
            {
                return;
            }

            var bindableRadioGroup = bindable as BindableRadioGroup;

            if (bindableRadioGroup == null)
            {
                return;
            }

            foreach (var button in bindableRadioGroup.Items.Where(button => button.Id == bindableRadioGroup.SelectedIndex))
            {
                button.Checked = true;
            }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        {
            var radButtons = bindable as BindableRadioGroup;


            foreach (var item in radButtons.Items) {
                item.CheckedChanged -= radButtons.OnCheckedChanged;
            }
                
            radButtons.Children.Clear();

            var radIndex = 0;

            foreach (var item in radButtons.ItemsSource)
            {
                var button = new CustomRadioButton
                {
                    Text = item.ToString(),
                    Id = radIndex++,
                    TextColor = radButtons.TextColor,
                    FontSize = Device.GetNamedSize(NamedSize.Small, radButtons),
                    FontName = radButtons.FontName
                };

                button.CheckedChanged += radButtons.OnCheckedChanged;

                radButtons.Items.Add(button);

                radButtons.Children.Add(button);
            }
        }
    }
}

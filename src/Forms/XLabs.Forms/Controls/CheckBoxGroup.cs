using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;

namespace XLabs.Forms.Controls
{
    public class CheckBoxGroup : StackLayout
    {
        public ObservableCollection<CheckBox> CheckBoxGroupItems;

        public CheckBoxGroup()
        {
            this.CheckBoxGroupItems = new ObservableCollection<CheckBox>();
            this.SelectedIndex = -1;
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<CheckBoxGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable),
                propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<CheckBoxGroup, int>(o => o.SelectedIndex, default(int), BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public bool ShowTitle
        {
            get { return this.GetValue<bool>(ShowTitleProperty); }
            set { this.SetValue(ShowTitleProperty, value); }
        }

        public event EventHandler<int> CheckedChanged;

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var checkBoxGroup = bindable as CheckBoxGroup;

            checkBoxGroup.CheckBoxGroupItems.Clear();
            checkBoxGroup.Children.Clear();
            if (newvalue != null)
            {
                int index = 0;
                foreach (var item in newvalue)
                {
                    var checkBox = new CheckBox();

                    if (item is ICheckBoxGroupItem)
                    {
                        var extendedCheckBoxGroupItem = (item as IExtendedCheckBoxGroupItem);
                        checkBox.DefaultText = checkBoxGroup.ShowTitle ? extendedCheckBoxGroupItem.Item.ToString() : string.Empty;
                        checkBox.CheckedImage = extendedCheckBoxGroupItem.ImageSelected;
                        checkBox.UncheckedImage = extendedCheckBoxGroupItem.ImageUnselected;
                    }
                    else
                    {
                        checkBox.DefaultText = checkBoxGroup.ShowTitle ? item.ToString() : string.Empty;
                    }

                    checkBox.Id = index;
                    checkBox.CheckedChanged += checkBoxGroup.OnCheckedChanged;
                    checkBox.HorizontalOptions = LayoutOptions.FillAndExpand;

                    checkBoxGroup.CheckBoxGroupItems.Add(checkBox);

                    checkBoxGroup.Children.Add(checkBox);
                    index++;
                }
            }
        }

        private void OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (e.Value == false) return;
            var checkBox = sender as CheckBox;

            foreach (var checkBox in this.CheckBoxGroupItems)
            {
                if (!checkBox.Id.Equals(checkBox.Id))
                {
                    checkBox.Checked = false;
                }
                else
                {
                    if (CheckedChanged != null)
                    {
                        CheckedChanged.Invoke(sender, checkBox.Id);
                    }
                }
            }
        }

        private static void OnSelectedIndexChanged(BindableObject bindable, int oldvalue, int newvalue)
        {
            if (newvalue == -1) return;

            var checkBoxGroup = bindable as CheckBoxGroup;

            foreach (var checkBox in checkBoxGroup.CheckBoxGroupItems)
            {
                if (checkBox.Id == checkBoxGroup.SelectedIndex)
                {
                    checkBox.Checked = true;
                }
            }
        }
    }
}

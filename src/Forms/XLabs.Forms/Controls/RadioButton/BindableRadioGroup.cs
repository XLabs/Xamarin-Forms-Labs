using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public class BindableRadioGroup : StackLayout
    {
        public ObservableCollection<CustomRadioButton> Items;

        public BindableRadioGroup()
        {
            Items = new ObservableCollection<CustomRadioButton>();
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindableRadioGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable));

        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<BindableRadioGroup, int>(o => o.SelectedIndex, default(int), BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged);


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

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);

                Items.Clear();
                Children.Clear();

                var radIndex = 0;

                foreach (var item in ItemsSource)
                {
                    var button = new CustomRadioButton
                    {
                        Text = item.ToString(),
                        Id = radIndex++,
                        TextColor = TextColor,
                        FontSize = Device.GetNamedSize(NamedSize.Small, this), 
                        FontName = FontName
                    };

                    button.CheckedChanged += OnCheckedChanged;

                    Items.Add(button);

                    Children.Add(button);
                }
            }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

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
    }
}

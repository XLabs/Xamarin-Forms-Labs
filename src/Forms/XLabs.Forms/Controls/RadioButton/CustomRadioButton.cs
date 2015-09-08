using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public class CustomRadioButton : View
    {
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create<CustomRadioButton, bool>(
                p => p.Checked, false);

        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<CustomRadioButton, string>(
                p => p.Text, string.Empty);

        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create<CustomRadioButton, Color>(
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
        ///     The checked changed event.
        /// </summary>
        public EventHandler<EventArgs<bool>> CheckedChanged;

        /// <summary>
        ///     Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get { return this.GetValue<bool>(CheckedProperty); }

            set
            {
                SetValue(CheckedProperty, value);

                var eventHandler = CheckedChanged;

                if (eventHandler != null)
                {
                    eventHandler.Invoke(this, value);
                }
            }
        }

        public string Text
        {
            get { return this.GetValue<string>(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return this.GetValue<Color>(TextColorProperty); }

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

        public int Id { get; set; }
    }
}
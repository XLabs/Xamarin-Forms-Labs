using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: 
    InternalsVisibleTo("XLabs.Forms.Droid"),
    InternalsVisibleTo("XLabs.Forms.iOS"),
    InternalsVisibleTo("XLabs.Forms.WP8")]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// An extended entry cell control that allows set IsPassword
    /// </summary>
    public class ExtendedEntryCell : EntryCell
    {

        /// <summary>
        /// The IsPassword property
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create<ExtendedEntryCell,bool> ( p => p.IsPassword, false);

        /// <summary>
        /// Gets or sets IsPassword 
        /// </summary>
        public bool IsPassword 
        {
            get { return (bool)GetValue (IsPasswordProperty); }
            set { SetValue (IsPasswordProperty,value); }
        }
    }
}


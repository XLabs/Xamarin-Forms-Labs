using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace XLabs.Forms.Controls
{
    using Android.Content;
    using Android.Text.Method;
    using Android.Views;
    using Android.Widget;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// Class ExtendedEntryCellRenderer.
    /// </summary>
    public class ExtendedEntryCellRenderer : EntryCellRenderer
    {
        /// <summary>
        /// Gets the cell core.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="convertView">The convert view.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="context">The context.</param>
        /// <returns>Android.Views.View.</returns>
        protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore (item, convertView, parent, context);
            if (cell != null) {
            
                var textField = ((EntryCellView) cell).EditText as TextView;
                
                if (textField != null && textField.TransformationMethod != PasswordTransformationMethod.Instance) 
                {
                    textField.TransformationMethod = PasswordTransformationMethod.Instance;
                }
            }
            return cell;
        }
    }
}


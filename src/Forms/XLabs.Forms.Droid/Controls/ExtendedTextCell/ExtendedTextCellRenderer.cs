using Android.Content.Res;
using Android.Graphics.Drawables;
using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTextCell), typeof(ExtendedTextCellRenderer))]
namespace XLabs.Forms.Controls
{
    using System;

    using Android.Content;
    using Android.Graphics;
    using Android.Views;
    using Android.Widget;

    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// Class ExtendedTextCellRenderer.
    /// </summary>
    public class ExtendedTextCellRenderer : TextCellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedTextCellRenderer"/> class.
        /// </summary>
        public ExtendedTextCellRenderer() { }
        /// <summary>
        /// The _context
        /// </summary>
        private Context _context;

        /// <summary>
        /// Handles the <see cref="E:CellPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);
        }
        /// <summary>
        /// Gets the cell core.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="convertView">The convert view.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="context">The context.</param>
        /// <returns>Android.Views.View.</returns>
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _context = context;

            var view = (ExtendedTextCell)item;

            if (convertView == null)
            {
                convertView = new BaseCellView(context, item);
            }

            var cellView = convertView as BaseCellView;

            //Incase convertView is not longer a BaseCellView 
            if (cellView != null)
            {
                //This is a text cell, no image required
                cellView.SetImageVisible(false);

                //Set text values
                cellView.MainText = view.Text;
                cellView.DetailText = view.Detail;

                //Set text colors
                cellView.SetMainTextColor(view.TextColor);
                cellView.SetDetailTextColor(view.DetailColor);

                //Set Background color
                cellView.SetBackgroundColor(view.BackgroundColor.ToAndroid());

                //Set Accessory view
                //TODO:  For some reason ShowDisclousure (sic, eventually fix spelling) is default true, which might be annoying for android users
                if (view.ShowDisclousure)
                {
                    //TODO: Consider different default icon, perhaps no default at all
                    var resourceId = Android.Resource.Drawable.IcMenuSend;
                    if (!string.IsNullOrWhiteSpace(view.DisclousureImage))
                    {
                        //Incase someone decides to add the extension to the file name
                        var fileName = System.IO.Path.GetFileNameWithoutExtension(view.DisclousureImage);
                        resourceId = _context.Resources.GetIdentifier(fileName, "drawable", context.PackageName);
                    }

                    var image = new ImageView(_context);
                    image.SetImageResource(resourceId);

                    cellView.SetAccessoryView(image);
                }

                var control = ((LinearLayout)convertView);
                var linearLayout = control.GetChildAt(1) as LinearLayout;
                if (linearLayout != null)
                {
                    var mainTextView = (TextView)linearLayout.GetChildAt(0);
                    var detailTextView = (TextView)linearLayout.GetChildAt(1);

                    UpdateTextViewFont(view, mainTextView);
                    UpdateTextViewFont(view, detailTextView);

                    //if (view.ShowSeparator)
                    //{
                    //    linearLayout.ShowDividers = ShowDividers.None;
                    //    linearLayout.DividerPadding = (int)view.SeparatorPadding.VerticalThickness;


                    //    linearLayout.SetDividerDrawable(new ColorDrawable(view.SeparatorColor.ToAndroid()));
                    //}
                }
            }

            return convertView;
        }

        /// <summary>
        /// Updates the Text View.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="control">The control.</param>
        void UpdateTextViewFont(ExtendedTextCell view, TextView control)
        {
            if (control != null)
            {
                //Should FontNameAndroid override FontName? 
                if (!string.IsNullOrWhiteSpace(view.FontNameAndroid))
                {
                    control.Typeface = TrySetFont(view.FontNameAndroid);
                }
                else if (!string.IsNullOrWhiteSpace(view.FontName))
                {
                    control.Typeface = TrySetFont(view.FontName);
                }

                if (view.FontSize > 0)
                {
                    control.TextSize = (float)view.FontSize;
                }
            }
        }

        /// <summary>
        /// Tries the set font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <returns>Typeface.</returns>
        private Typeface TrySetFont(string fontName)
        {
            var typeface = Typeface.Default;

            try
            {
                typeface = Typeface.CreateFromAsset(_context.Assets, fontName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("not found in assets {0}", ex);

                try
                {
                    typeface = Typeface.CreateFromFile(fontName);
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1);
                }
            }

            return typeface;
        }
    }
}


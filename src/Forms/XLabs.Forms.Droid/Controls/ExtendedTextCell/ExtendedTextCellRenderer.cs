using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer (typeof (ExtendedTextCell), typeof (ExtendedTextCellRenderer))]
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
	public class ExtendedTextCellRenderer :  TextCellRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTextCellRenderer"/> class.
		/// </summary>
		public ExtendedTextCellRenderer (){}
		/// <summary>
		/// The _context
		/// </summary>
		private Context _context;

		/// <summary>
		/// Handles the <see cref="E:CellPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnCellPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnCellPropertyChanged (sender, args);
		}
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
			_context = context;

			var view = (ExtendedTextCell)item;
			if (convertView == null) {
				convertView = new BaseCellView (context);
			} 
			var control = ((LinearLayout)convertView);

			var mainview = (TextView)(control.GetChildAt (1) as LinearLayout).GetChildAt (0);
			var detailview = (TextView)(control.GetChildAt (1) as LinearLayout).GetChildAt (1);

			UpdateUi (view, mainview);
			UpdateUi (view, detailview);
			return  convertView;;
		}

		/// <summary>
		/// Updates the UI.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="control">The control.</param>
		void UpdateUi (ExtendedTextCell view, TextView control)
		{
			if (!string.IsNullOrEmpty (view.FontName)) {
				control.Typeface = 	TrySetFont (view.FontName);
			}
			if (!string.IsNullOrEmpty (view.FontNameAndroid)) {
				control.Typeface = 	TrySetFont (view.FontNameAndroid);;
			}
			if (view.FontSize > 0)
				control.TextSize = (float)view.FontSize;
		 }

		/// <summary>
		/// Tries the set font.
		/// </summary>
		/// <param name="fontName">Name of the font.</param>
		/// <returns>Typeface.</returns>
		private Typeface TrySetFont (string fontName)
		{
			Typeface tf = Typeface.Default;
			try {
				tf = Typeface.CreateFromAsset (_context.Assets,fontName);
				return tf;
			}
			catch (Exception ex) {
				Console.Write ("not found in assets {0}", ex);
				try {
					tf = Typeface.CreateFromFile (fontName);
					return tf;
				}
				catch (Exception ex1) {
					Console.Write (ex1);
					return Typeface.Default;
				}
			}
		}
	}
}


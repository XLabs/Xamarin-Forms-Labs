using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTableView), typeof(ExtendedTableViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ExtendedTableViewRenderer.
	/// </summary>
	public class ExtendedTableViewRenderer : TableViewRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement == null)
			{
				((ExtendedTableView)e.NewElement).DataChanged += (object sender, EventArgs args) => { Control.ReloadData(); };
			}
		}
	}
}
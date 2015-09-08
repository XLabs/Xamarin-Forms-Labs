using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTableView.
	/// </summary>
	public class ExtendedTableView : TableView
	{
		/// <summary>
		/// Occurs when [data changed].
		/// </summary>
		public event EventHandler<EventArgs> DataChanged;

		/// <summary>
		/// Called when [data changed].
		/// </summary>
		public void OnDataChanged()
		{
			var handler = this.DataChanged;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}
using System.Diagnostics;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class SegmentedControlView.
	/// </summary>
	public class SegmentedControlView : BoxView
	{

		/// <summary>
		/// The selected item property
		/// </summary>
		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create<SegmentedControlView, int>(
				p => p.SelectedItem, default(int));

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public int SelectedItem
		{
			get { return (int)GetValue(SelectedItemProperty); }
			set
			{
				Debug.WriteLine("New Value:" + value);
				SetValue(SelectedItemProperty, value);
			}
		}

		/// <summary>
		/// The segments itens property
		/// </summary>
		public static readonly BindableProperty SegmentsItensProperty =
			BindableProperty.Create<SegmentedControlView, string>(
				p => p.SegmentsItens, default(string), BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets the segments itens.
		/// </summary>
		/// <value>The segments itens.</value>
		public string SegmentsItens
		{
			get { return (string)GetValue(SegmentsItensProperty); }
			set
			{
				Debug.WriteLine("New Seg Value:" + value);
				SetValue(SegmentsItensProperty, value);
			}
		}

		/// <summary>
		/// The tint color property
		/// </summary>
		public static readonly BindableProperty TintColorProperty =
			BindableProperty.Create<SegmentedControlView, Color>(
				p => p.TintColor, Color.Blue);

		/// <summary>
		/// Gets or sets the color of the tint.
		/// </summary>
		/// <value>The color of the tint.</value>
		public Color TintColor
		{
			get { return (Color)GetValue(TintColorProperty); }
			set
			{
				Debug.WriteLine("New TintColor Value:" + value);
				SetValue(TintColorProperty, value);
			}
		}
	}
}

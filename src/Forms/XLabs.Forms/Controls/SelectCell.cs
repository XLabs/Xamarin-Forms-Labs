using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Platform;

namespace XLabs.Forms.Controls
{
	using XLabs.Enums;

	/// <summary>
	/// Class SelectCell.
	/// </summary>
	public class SelectCell : ExtendedTextCell
	{
		/// <summary>
		/// The items property
		/// </summary>
		public static readonly BindableProperty ItemsProperty = BindableProperty.Create<SelectCell, List<string>>(p => p.Items, default(List<string>));
		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
		public List<string> Items
		{
			get { return (List<string>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		/// <summary>
		/// The selected item property
		/// </summary>
		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<SelectCell, string>(p => p.SelectedItem, default(string));
		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public string SelectedItem
		{
			get { return (string)GetValue(SelectedItemProperty);  }
			set 
			{ 
				SetValue(SelectedItemProperty, value); 
				Detail = value; 

				if (_cells != null && _cells.Count > 0) {
					foreach (var cell in _cells) {
						cell.Checked = cell.Text == value;
					}

					_selectionTableView.Root = new TableRoot { new TableSection () { _cells } };
				}
			}
		}

		/// <summary>
		/// Gets or sets the navigation.
		/// </summary>
		/// <value>The navigation.</value>
		public Func<INavigation> Navigation { get; set; }
		/// <summary>
		/// The cells
		/// </summary>
		List<CheckboxCell> _cells = new List<CheckboxCell>();
		/// <summary>
		/// The selection table view
		/// </summary>
		TableView _selectionTableView;
		/// <summary>
		/// Occurs when [selected item changed].
		/// </summary>
		public event Action SelectedItemChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectCell"/> class.
		/// </summary>
		/// <param name="navigation">The navigation.</param>
		public SelectCell(Func<INavigation> navigation)
		{
			Navigation = navigation;
			ShowDisclousure = true;
			DetailLocation = TextCellDetailLocation.Right;
			this.DetailColor = Color.Gray;

			BackgroundColor = Color.White;
			this.Tapped += HandleTapped;
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <returns>ContentPage.</returns>
		protected virtual ContentPage CreatePage()
		{
			return new ContentPage();
		}

		/// <summary>
		/// Checkboxes the changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		void CheckboxChanged(object sender, EventArgs<bool> args)
		{
			foreach (var cell in _cells) {
				if (cell == sender)
					SelectedItem = cell.Text;
				else
					cell.Checked = false;
			}

			if (SelectedItemChanged != null)
				SelectedItemChanged();

			_selectionTableView.Root = new TableRoot { new TableSection () { _cells } };
		}

		/// <summary>
		/// Handles the tapped.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		void HandleTapped (object sender, EventArgs e)
		{
			var page = CreatePage();

			_cells.Clear ();
			foreach (string curItem in Items)
			{
				var checkboxCell = new CheckboxCell(true) { Text = curItem, Checked = curItem == SelectedItem };
				checkboxCell.CheckedChanged += CheckboxChanged;
				_cells.Add(checkboxCell);
			}

			_selectionTableView = new TableView
			{
				Intent = TableIntent.Settings,
				Root = new TableRoot
				{
					new TableSection()
					{
						_cells  
					}
				}
			};

			page.Content = _selectionTableView;

			Navigation().PushAsync(page);
		}
	}
}


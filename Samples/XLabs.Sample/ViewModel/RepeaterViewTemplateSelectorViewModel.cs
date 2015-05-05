namespace XLabs.Sample.ViewModel
{
	using System.Collections.ObjectModel;

	using Xamarin.Forms;

	using XLabs.Forms.Mvvm;
	using XLabs.Sample.Pages.Mvvm;

	/// <summary>
	/// Class RepeaterViewViewModel.
	/// </summary>
	[ViewType(typeof(MvvmSamplePage))]
	public class RepeaterViewTemplateSelectorViewModel : XLabs.Forms.Mvvm.ViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RepeaterViewViewModel"/> class.
		/// </summary>
        public RepeaterViewTemplateSelectorViewModel()
		{
			Things.CollectionChanged += ThingsCollectionChanged;
		}

		/// <summary>
		/// Handles the CollectionChanged event of the Things control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		void ThingsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
            RemoveFirstItem.ChangeCanExecute();
            AddThingItem.ChangeCanExecute();
            AddThing2Item.ChangeCanExecute();
		}

		/// <summary>
		/// The _things
		/// </summary>
		private ObservableCollection<Thing> _things =  new ObservableCollection<Thing>();
		/// <summary>
		/// The things property name
		/// </summary>
		public const string ThingsPropertyName = "Things";
		/// <summary>
		/// The things name property name
		/// </summary>
		public const string ThingsNamePropertyName = "Name";
		/// <summary>
		/// The things description property name
		/// </summary>
		public const string ThingsDescriptionPropertyName = "Description";
		/// <summary>
		/// Gets or sets the things.
		/// </summary>
		/// <value>The things.</value>
		public ObservableCollection<Thing> Things
		{
			get { return _things; }
			set 
			{ 
				SetProperty(ref _things, value);
			}
		}

		/// <summary>
		/// The _next item number
		/// </summary>
		private int _nextItemNumber;
		/// <summary>
		/// The next item number property name
		/// </summary>
		public const string NextItemNumberPropertyName = "NextItemNumber";
		/// <summary>
		/// Gets or sets the next item number.
		/// </summary>
		/// <value>The next item number.</value>
		public int NextItemNumber
		{
			get { return _nextItemNumber; }
			set { SetProperty(ref _nextItemNumber, value); }
		}

		/// <summary>
		/// The _remove first item
		/// </summary>
		private Command _removeFirstItem;
		/// <summary>
		/// The remove first item command name
		/// </summary>
		public const string RemoveFirstItemCommandName = "RemoveFirstItem";
		/// <summary>
		/// Gets the remove first item.
		/// </summary>
		/// <value>The remove first item.</value>
		public Command RemoveFirstItem
		{
			get 
			{ 
				return _removeFirstItem ?? (_removeFirstItem = new Command(() =>
				{
					Things.RemoveAt(0);                    
				}, 
				() => Things.Count > 0)); 
			}
		}

		/// <summary>
		/// The _add item
		/// </summary>
		private Command _addThingItem;
		/// <summary>
		/// The add item command name
		/// </summary>
		public const string AddThingItemCommandName = "AddThingItem";
		/// <summary>
		/// Gets the add item.
		/// </summary>
		/// <value>The add item.</value>
		public Command AddThingItem
		{
			get
			{
				return _addThingItem ?? (_addThingItem = new Command(() =>
				{
					Things.Add(new Thing { Name = string.Format("Thing {0}", NextItemNumber), Description = string.Format("This is thing #{0}", NextItemNumber++) });
				},
				() => Things.Count < 5));
			}
		}

        /// <summary>
        /// The _add item
        /// </summary>
        private Command _addThing2Item;
        /// <summary>
        /// The add item command name
        /// </summary>
        public const string AddThing2ItemCommandName = "AddThing2Item";

        public Command AddThing2Item
        {
            get
            {
                return _addThing2Item ?? (_addThing2Item = new Command(() =>
                {
                    Things.Add(new Thing2 { Name = string.Format("Thing {0}", NextItemNumber), Description = string.Format("This is thing #{0}", NextItemNumber++) });
                },
                () => Things.Count < 5));
            }
        }

		/// <summary>
		/// Loads the data.
		/// </summary>
		public void LoadData()
		{
			Things.Add(new Thing { Name = "Thing 1", Description = "This is thing #1." });
			Things.Add(new Thing2 { Name = "Thing 2", Description = "This is thing #2." });
			NextItemNumber = 3;
		}
	}

	/// <summary>
	/// Class Thing2
	/// </summary>
	public class Thing2 : Thing
	{
	}
}

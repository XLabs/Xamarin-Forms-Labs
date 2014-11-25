using Xamarin.Forms.Labs.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms.Labs.Sample.Model;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class AutoCompleteViewModel : ViewModel
    {
        private ObservableCollection<object> _items;
        private Command<string> _searchCommand;
        private Command<object> _cellSelectedCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteViewModel"/> class.
        /// </summary>
        public AutoCompleteViewModel()
        {
            Items = new ObservableCollection<object>();
            for (var i = 0; i < 10; i++)
            {
                Items.Add(new TestPerson
                {
                    FirstName = string.Format("FirstName {0}", i),
                    LastName = string.Format("LastName {0}", i)
                });
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ObservableCollection<object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }

        /// <summary>
        /// Gets the selected cell command.
        /// </summary>
        /// <value>
        /// The selected cell command.
        /// </value>
        public Command<object> CellSelectedCommand
        {
            get
            {
                return _cellSelectedCommand ?? (_cellSelectedCommand = new Command<object>((object parameter) =>
                {
                    var person = ((TestPerson)parameter);
                    Debug.WriteLine(person.FirstName + person.LastName + person.Age);
                }));
            }
        }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>
        /// The search command.
        /// </value>
        public Command<string> SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(
                    obj => { },
                    obj => !string.IsNullOrEmpty(obj.ToString())));
            }
        }
    }
}


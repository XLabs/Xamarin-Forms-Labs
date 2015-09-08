using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample
{
    [ViewType(typeof(MvvmSamplePage))]
    public class RepeaterViewViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
    {
        public RepeaterViewViewModel()
        {
            Things.CollectionChanged += Things_CollectionChanged;
        }

        void Things_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RemoveFirstItem.ChangeCanExecute();
            AddItem.ChangeCanExecute();
        }

        private ObservableCollection<Thing> _things =  new ObservableCollection<Thing>();
        public const string ThingsPropertyName = "Things";
        public const string ThingsNamePropertyName = "Things.Name";
        public const string ThingsDescriptionPropertyName = "Things.Description";
        public ObservableCollection<Thing> Things
        {
            get { return _things; }
            set 
            { 
                SetProperty(ref _things, value);
            }
        }

        private int _nextItemNumber;
        public const string NextItemNumberPropertyName = "NextItemNumber";
        public int NextItemNumber
        {
            get { return _nextItemNumber; }
            set { SetProperty(ref _nextItemNumber, value); }
        }

        private Command _removeFirstItem;
        public const string RemoveFirstItemCommandName = "RemoveFirstItem";
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

        private Command _addItem;
        public const string AddItemCommandName = "AddItem";
        public Command AddItem
        {
            get
            {
                return _addItem ?? (_addItem = new Command(() =>
                {
                    Things.Add(new Thing { Name = string.Format("Thing {0}", NextItemNumber), Description = string.Format("This is thing #{0}", NextItemNumber++) });
                },
                () => Things.Count < 5));
            }
        }

        public void LoadData()
        {
            Things.Add(new Thing { Name = "Thing 1", Description = "This is thing #1." });
            Things.Add(new Thing { Name = "Thing 2", Description = "This is thing #2." });
            NextItemNumber = 3;
        }
    }

    public class Thing
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

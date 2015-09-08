using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms
{
    //From the Xamarin forums 
    //http://forums.xamarin.com/discussion/comment/68739#Comment_68739

    public class OnPlatformList<T> : ObservableCollection<T>
    {
        private ObservableCollection<T> _realData;

        public OnPlatformList()
        {
            iOS = new ObservableCollection<T>();
            Android = new ObservableCollection<T>();
            WinPhone = new ObservableCollection<T>();

            // Unfortunately, there's no way to see the complete initialization of the tag in XAML,
            // in this case, the ctor fires before we've seen the collection data
            // so hook the collections and wait for each one to change - 
            // when we find the right one, we'll pull it's data and ignore the others.
            iOS.CollectionChanged += OnInitialize;
            Android.CollectionChanged += OnInitialize;
            WinPhone.CollectionChanged += OnInitialize;
        }

        // ReSharper disable once InconsistentNaming
        public ObservableCollection<T> iOS { get; private set; }
        public ObservableCollection<T> Android { get; private set; }
        public ObservableCollection<T> WinPhone { get; private set; }

        void OnInitialize(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool foundCollection = false;
            if (Device.OS == TargetPlatform.iOS && sender == iOS)
            {
                Setup(iOS);
                foundCollection = true;
            }
            else if (Device.OS == TargetPlatform.Android && sender == Android)
            {
                Setup(Android);
                foundCollection = true;
            }
            else if (Device.OS == TargetPlatform.WinPhone && sender == WinPhone)
            {
                Setup(WinPhone);
                foundCollection = true;
            }

            if (foundCollection)
            {
                iOS.CollectionChanged -= OnInitialize;
                Android.CollectionChanged -= OnInitialize;
                WinPhone.CollectionChanged -= OnInitialize;
            }
        }

        void Setup(ObservableCollection<T> data)
        {
            _realData = data;
            foreach (var item in data) Add(item);

            data.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems.Cast<T>()) Add(item);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems.Cast<T>()) Remove(item);
                        break;
                    // TODO: add other operations.
                    default:
                        Clear();
                        foreach (var item in _realData) Add(item);
                        break;
                }

            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

//using PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;

namespace XLabs.Forms.Controls
{
    /// <summary>
    ///     Delegate CurrentPageChangingEventHandler.
    /// </summary>
    public delegate void CurrentPageChangingEventHandler();

    /// <summary>
    ///     Delegate CurrentPageChangedEventHandler.
    /// </summary>
    public delegate void CurrentPageChangedEventHandler();

    /// <summary>
    /// Delegate SwipeLeftEventHandler
    /// </summary>
    public delegate void SwipeLeftEventHandler();

    /// <summary>
    /// Delegate SwipeRightEventHandler
    /// </summary>
    public delegate void SwipeRightEventHandler();

    /// <summary>
    ///     Class ExtendedTabbedPage.
    /// </summary>
    public class ExtendedTabbedPage : TabbedPage
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create<ExtendedTabbedPage, Color>(
                p => p.TintColor, Color.White);

        public static readonly BindableProperty BarTintColorProperty =
            BindableProperty.Create<ExtendedTabbedPage, Color>(
                p => p.BarTintColor, Color.White);

        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create<ExtendedTabbedPage, Color>(
                p => p.BackgroundColor, Color.White);

        public static readonly BindableProperty BadgesProperty =
           BindableProperty.Create<ExtendedTabbedPage, List<string>>(
               p => p.Badges, null);

        public static readonly BindableProperty TabBarSelectedImageProperty =
            BindableProperty.Create<ExtendedTabbedPage, string>(
                p => p.TabBarSelectedImage, null);

        public static readonly BindableProperty TabBarBackgroundImageProperty =
            BindableProperty.Create<ExtendedTabbedPage, string>(
                p => p.TabBarBackgroundImage, null);

        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create<ExtendedTabbedPage, DataTemplateSelector>(x => x.ItemTemplateSelector, default(DataTemplateSelector), propertyChanged: OnDataTemplateSelectorChanged);

        private DataTemplateSelector currentItemSelector;
        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }

        private static void OnDataTemplateSelectorChanged(BindableObject bindable, DataTemplateSelector oldvalue, DataTemplateSelector newvalue)
        {
            ((ExtendedTabbedPage)bindable).OnDataTemplateSelectorChanged(oldvalue, newvalue);
        }

        protected virtual void OnDataTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
            // check to see we don't have an ItemTemplate set
            if (ItemTemplate != null && newValue != null)
                throw new ArgumentException("Cannot set both ItemTemplate and ItemTemplateSelector", "ItemTemplateSelector");

            // cache value locally
            currentItemSelector = newValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtendedTabbedPage" /> class.
        /// </summary>
        public ExtendedTabbedPage()
        {
            PropertyChanging += OnPropertyChanging;
            PropertyChanged += OnPropertyChanged;
            OnSwipeLeft += SwipeLeft;
            OnSwipeRight += SwipeRight;

            this.SwipeEnabled = false;

            Badges = new List<string>();
        }

        public Color TintColor
        {
            get
            {
                return (Color)GetValue(TintColorProperty);
            }
            set
            {
                SetValue(TintColorProperty, value);
            }
        }

        public Color BarTintColor
        {
            get
            {
                return (Color)GetValue(BarTintColorProperty);
            }
            set
            {
                SetValue(BarTintColorProperty, value);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return (Color)GetValue(BackgroundColorProperty);
            }
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        public List<string> Badges
        {
            get
            {
                return (List<string>)GetValue(BadgesProperty);
            }
            set
            {
                SetValue(BadgesProperty, value);
            }
        }

        public string TabBarSelectedImage
        {
            get
            {
                return (string)GetValue(TabBarSelectedImageProperty);
            }
            set
            {
                SetValue(TabBarSelectedImageProperty, value);
            }
        }

        public string TabBarBackgroundImage
        {
            get
            {
                return (string)GetValue(TabBarBackgroundImageProperty);
            }
            set
            {
                SetValue(TabBarBackgroundImageProperty, value);
            }
        }

        public bool SwipeEnabled { get; set; }

        /// <summary>
        ///     Occurs when [current page changing].
        /// </summary>
        public event CurrentPageChangingEventHandler CurrentPageChanging;

        /// <summary>
        ///     Occurs when [current page changed].
        /// </summary>
        public event CurrentPageChangedEventHandler CurrentPageChanged;

        /// <summary>
        /// Occurs when the TabbedPage is swipped Right
        /// </summary>
        public event EventHandler OnSwipeRight;

        /// <summary>
        /// Occurs when the TabbedPage is swipped Left
        /// </summary>
        public event EventHandler OnSwipeLeft;

        /// <summary>
        /// Invokes the item SwipeRight event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item">Item.</param>
        public void InvokeSwipeRightEvent(object sender, object item)
        {
            if (OnSwipeRight != null)
            {
                OnSwipeRight.Invoke(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Invokes the SwipeLeft event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item">Item.</param>
        public void InvokeSwipeLeftEvent(object sender, object item)
        {
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft.Invoke(sender, new EventArgs());
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:PropertyChanging" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangingEventArgs" /> instance containing the event data.</param>
        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                RaiseCurrentPageChanging();
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                RaiseCurrentPageChanged();
            }
        }

        /// <summary>
        /// Move to the previous Tabbed Page
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        private void SwipeLeft(object a, EventArgs e)
        {
            if (SwipeEnabled)
            {
                PreviousPage();
            }
        }

        /// <summary>
        /// Move to the next Tabbed Page
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        private void SwipeRight(object a, EventArgs e)
        {
            if (SwipeEnabled)
            {
                NextPage();
            }
        }
        /// <summary>
        ///     Raises the current page changing.
        /// </summary>
        private void RaiseCurrentPageChanging()
        {
            var handler = CurrentPageChanging;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///     Raises the current page changed.
        /// </summary>
        private void RaiseCurrentPageChanged()
        {
            var handler = CurrentPageChanged;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Move to the next page.
        /// Restart at the first page should you try 
        /// to move past the last page.
        /// </summary>
        private void NextPage()
        {
            var currentPage = Children.IndexOf(CurrentPage);

            currentPage++;

            if (currentPage > Children.Count - 1)
            {
                currentPage = 0;
            }

            CurrentPage = Children[currentPage];
        }

        /// <summary>
        /// Move to the previous page.
        /// If you are on the first page then return 
        /// the last page in the list
        /// </summary>
        private void PreviousPage()
        {
            var currentPage = Children.IndexOf(CurrentPage);

            currentPage--;

            if (currentPage < 0)
            {
                currentPage = Children.Count - 1;
            }

            CurrentPage = Children[currentPage];
        }

        protected override Page CreateDefault(object item)
        {
            var view = this.ViewFor(item, currentItemSelector);

            if (view != null)
            {
                var cp = new ContentPage
                {
                    BindingContext = item,
                    Content = view,
                };
                return cp;
            }


            return base.CreateDefault(item);
        }
    }
}
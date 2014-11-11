namespace Xamarin.Forms.Labs.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Input;
    using Xamarin.Forms.Labs.Exceptions;

    /// <summary>
    /// Simple list control to dispaly an ObservableCollection(T)
    /// </summary>
    /// <typeparam name="T">The type contained in teh collection to dispaly</typeparam>
    /// Element created at 10/11/2014,10:54 PM by Charles
    public class RepeaterView<T> : StackLayout
    {

        /// <summary>
        /// Definition for <see cref="ItemTemplate"/>
        /// </summary>
        /// Element created at 10/11/2014,10:57 PM by Charles
        public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create<RepeaterView<T>, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 10/11/2014,10:58 PM by Charles
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<RepeaterView<T>, ObservableCollection<T>>(p => p.ItemsSource, default(ObservableCollection<T>), BindingMode.OneWay, null, ItemsChanged);

        /// <summary>
        /// Definition for <see cref="ItemClickCommand"/>
        /// </summary>
        /// Element created at 10/11/2014,10:56 PM by Charles
        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.Create<RepeaterView<T>, ICommand>(x => x.ItemClickCommand, null);

        /// <summary>
        /// Definition for <see cref="TemplateSelector"/>
        /// </summary>
        /// Element created at 10/11/2014,10:58 PM by Charles
        public static readonly BindableProperty TemplateSelectorProperty =
            BindableProperty.Create<RepeaterView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector));

        /// <summary>
        /// Definition of the ItemAdded EventHandler delegate
        /// </summary>
        /// <param name="sender">this control</param>
        /// <param name="args">The <see cref="RepeaterViewItemAddedEventArgs"/> instance containing the event data.</param>
        /// Element created at 10/11/2014,10:59 PM by Charles
        public delegate void RepeaterViewItemAddedEventHandler(object sender, RepeaterViewItemAddedEventArgs args);

        /// <summary>Occurs when [item created].</summary>
        /// Element created at 10/11/2014,10:59 PM by Charles
        public event RepeaterViewItemAddedEventHandler ItemCreated;


        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterView{T}"/> class.
        /// </summary>
        /// Element created at 10/11/2014,11:00 PM by Charles
        public RepeaterView()
        {
            Spacing = 0;
        }

        /// <summary>
        /// The items source property
        /// Since Items source must implement INotifyCollectionChanged, it is reasonable
        /// to use ObservableCollection(T) as the most base collection class
        /// that is acceptable.
        /// </summary>
        /// Element created at 10/11/2014,10:55 PM by Charles
        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>Gets or sets the template selector.</summary>
        /// <value>The template selector picks a datatemplate based on the type of the item.</value>
        /// Element created at 10/11/2014,11:00 PM by Charles
        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        /// <summary>Gets or sets the item click command.</summary>
        /// <value>
        /// The item click command.  This command is called when an item
        /// has been tapped and the CanExecute callback is true
        /// </value>
        /// Element created at 10/11/2014,11:00 PM by Charles
        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        
        /// <summary>
        /// The item template property, this can be used on it's own
        /// or in conjunction with the template selector
        /// </summary>
        /// Element created at 10/11/2014,10:54 PM by Charles
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gives codebehind a chance to play with the
        /// newly created view object :D
        /// </summary>
        /// <param name="view">The visual view object</param>
        /// <param name="model">The item being added</param>
        protected virtual void NotifyItemAdded(View view, T model)
        {
            if (ItemCreated != null)
            {
                ItemCreated(this, new RepeaterViewItemAddedEventArgs(view, model));
            }
        }

        /// <summary>
        /// Select a datatemplate dynamically
        /// Prefer the TemplateSelector then the DataTemplate
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual DataTemplate GetTemplateFor(Type type)
        {
            DataTemplate retTemplate = null;
            if (TemplateSelector != null)
                retTemplate = TemplateSelector.TemplateFor(type);
            return retTemplate ?? ItemTemplate;
        }

        /// <summary>
        /// Creates a view based on the items type
        /// While we do have T, T could very well be
        /// a common superclass or an interface by
        /// using the items actual type we support
        /// both inheritance based polymorphism
        /// and shape based polymorphism
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A View that has been initialized with <see cref="item"/> as it's BindingContext</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>Thrown when the matched datatemplate inflates to an object not derived from either
        /// <see cref="Xamarin.Forms.View"/> or <see cref="Xamarin.Forms.ViewCell"/>
        protected virtual View ViewFor(T item)
        {
            var template = GetTemplateFor(item.GetType());
            var content = template.CreateContent();

            if (!(content is View) && !(content is ViewCell))
                throw new InvalidVisualObjectException(content.GetType());
            var view = (content is View) ? content as View : ((ViewCell)content).View;
            view.BindingContext = item;
            view.GestureRecognizers.Add(new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item });
            return view;
        }

        /// <summary>
        /// Reset the collection of bound objects
        /// Remove the old collection changed eventhandler (if any)
        /// Create new cells for each new item
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void ItemsChanged(BindableObject bindable, ObservableCollection<T> oldValue, ObservableCollection<T> newValue)
        {
            var control = bindable as RepeaterView<T>;
            if (control == null)
                throw new Exception("Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a " + bindable.GetType().Name);

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= control.ItemsSource_CollectionChanged;
            }

            newValue.CollectionChanged += control.ItemsSource_CollectionChanged;

            
            control.Children.Clear();

            foreach (var item in newValue)
            {
                var view = control.ViewFor(item);
                control.Children.Add(view);
                control.NotifyItemAdded(view, item);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Update visible controls based on changes in the bound collection
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e"><see cref="NotifyCollectionChangedEventArgs"/></param>
        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
            }
            else
            {
                //Only one item can be removed from an Observable collection
                //at any one time.
                if (e.OldItems != null)
                {
                    Children.RemoveAt(e.OldStartingIndex);
                }

                //Only one item can be added to an Observable collection
                //at any one time
                if (e.NewItems != null)
                {
                    var item = (T)e.NewItems[0];
                    var view = ViewFor(item);
                    Children.Insert(e.NewStartingIndex, view);
                    NotifyItemAdded(view, item);
                }
            }
            UpdateChildrenLayout();
            InvalidateLayout();
        }
    }

    /// <summary>
    /// Arguments passed to the <see cref="RepeaterView{T}.RepeaterViewItemAddedEventHandler"/>
    /// </summary>
    /// Element created at 10/11/2014,11:02 PM by Charles
    public class RepeaterViewItemAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterViewItemAddedEventArgs"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// Element created at 10/11/2014,11:03 PM by Charles
        public RepeaterViewItemAddedEventArgs(View view, object model)
        {
            View = view;
            Model = model;
        }

        /// <summary>Gets or sets the view.</summary>
        /// <value>The visual view.</value>
        /// Element created at 10/11/2014,11:03 PM by Charles
        public View View { get; set; }

        /// <summary>Gets or sets the model.</summary>
        /// <value>The model bound to the view.</value>
        /// Element created at 10/11/2014,11:03 PM by Charles
        public object Model { get; set; }
    }
}
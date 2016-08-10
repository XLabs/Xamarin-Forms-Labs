// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CarouselView.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Behaviors;
using XLabs.Forms.Exceptions;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Provides a View that uses swipe left and swipe right to change between displays
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CarouselView<T> : ContentView
    {
        private int currentview;

        /// <summary>
        /// The _gesture view
        /// </summary>
        private readonly GesturesContentView gestureView;

        /// <summary>
        /// The _content view
        /// </summary>
        private readonly TemplateContentView<object> contentView;

        /// <summary>
        /// The _interests
        /// </summary>
        private readonly List<GestureInterest> interests = new List<GestureInterest>();

        /// <summary>
        /// The _my grid
        /// </summary>
        private readonly Grid myGrid, marker;

        /// <summary>
        /// Constructs the Carousel view and sets defaults
        /// </summary>
        public CarouselView()
        {
            //Create my children and have fun with them :D
            //ViewModels = new ObservableCollection<T>();
            //TemplateSelector = new TemplateSelector();

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            //We need a gestureview that manages it's own gestures
            //Wrapping a contentview on the inside.
            //We will automap this TemplateSelector to the contentviews
            //Setup the gestureview
            this.gestureView = new GesturesContentView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.interests.Add(new GestureInterest
            {
                Direction = Directionality.Right,
                GestureType = GestureType.Swipe,
                GestureCommand = new RelayGesture((g, x) => SwitchView(false), (g, x) => this.currentview > 0)
            });
            this.interests.Add(new GestureInterest
            {
                Direction = Directionality.Left,
                GestureType = GestureType.Swipe,
                GestureCommand = new RelayGesture((g, x) => SwitchView(true), (g, x) => this.currentview < ViewModels.Count)
            });


            //Setup the TemplateContentView
            this.contentView = new TemplateContentView<object>
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TemplateSelector = TemplateSelector
            };
            Content = this.gestureView;
            this.marker = new Grid { BackgroundColor = TickColor };
            var s = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 5, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            this.myGrid = new Grid { BackgroundColor = BackgroundColor, ColumnSpacing = 25, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, Padding = new Thickness(35, 2.5) };
            this.myGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Absolute) });
            s.Children.Add(this.contentView);
            s.Children.Add(this.myGrid);
            this.gestureView.Content = s;
            this.gestureView.RegisterInterests(s, this.interests);

        }

        #region Bindables
        /// <summary>
        /// Property defnition for the <see cref="ViewModels" /> property
        /// </summary>
        public static BindableProperty ViewModelsProperty = BindableProperty.Create<CarouselView<T>, ObservableCollection<T>>(x => x.ViewModels, default(ObservableCollection<T>),BindingMode.OneWay,null,ViewModelsChanged);
        /// <summary>
        /// Property defnition for the <see cref="SelectedViewModel" /> property
        /// </summary>
        public static BindableProperty SelectedViewModelProperty = BindableProperty.Create<CarouselView<T>, T>(x => x.SelectedViewModel, default(T),BindingMode.TwoWay,null,SelectedViewModelChanged);
        /// <summary>
        /// Property definition for the <see cref="TemplateSelector" /> property
        /// </summary>
        public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create<CarouselView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector),BindingMode.OneWay,null,TemplateSelectorChanged);
        /// <summary>
        /// Property definition for the <see cref="TickColor" /> property
        /// </summary>
        public static readonly BindableProperty TickColorProperty =BindableProperty.Create<CarouselView<T>, Color>(x => x.TickColor, Color.Lime);
        /// <summary>
        /// Property definition for the <see cref="ShowTick" />
        /// </summary>
        public static readonly BindableProperty ShowTickProperty =BindableProperty.Create<CarouselView<T>, bool>(x => x.ShowTick, true,BindingMode.OneWay,null,ShowTickchanged);

        /// <summary>
        /// The item template selector property
        /// </summary>
        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create<CarouselView<T>, DataTemplateSelector>(x => x.ItemTemplateSelector, default(DataTemplateSelector), propertyChanged: OnDataTemplateSelectorChanged);

        private DataTemplateSelector currentItemSelector;
        /// <summary>
        /// Gets or sets the item template selector.
        /// </summary>
        /// <value>The item template selector.</value>
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

        /// <summary>
        /// Called when [data template selector changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">The oldvalue.</param>
        /// <param name="newvalue">The newvalue.</param>
        private static void OnDataTemplateSelectorChanged(BindableObject bindable, DataTemplateSelector oldvalue, DataTemplateSelector newvalue)
        {
            ((CarouselView<T>)bindable).OnDataTemplateSelectorChanged(oldvalue, newvalue);
        }

        /// <summary>
        /// Called when [data template selector changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnDataTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
            // Cache locally
            currentItemSelector = newValue;

            if (this.contentView != null)//may be constructing
            {
                this.contentView.ItemTemplateSelector = currentItemSelector;
            }
        }

        #endregion

        /// <summary>
        /// Show the tickboard.  The tickboard takes up 8dp vertically when shown.
        /// </summary>
        /// <value><c>true</c> if [show tick]; otherwise, <c>false</c>.</value>
        public bool ShowTick
        {
            get { return (bool)GetValue(ShowTickProperty); }
            set { SetValue(ShowTickProperty,value);}
        }

        /// <summary>
        /// The color for the Ticks
        /// </summary>
        /// <value>The color of the tick.</value>
        public Color TickColor
        {
            get { return (Color)GetValue(TickColorProperty); }
            set { SetValue(TickColorProperty,value);}
        }
        /// <summary>
        /// The collection of viewmodels to display in the carousel
        /// </summary>
        /// <value>The view models.</value>
        public ObservableCollection<T> ViewModels
        {
            get { return (ObservableCollection<T>)GetValue(ViewModelsProperty); }
            set { SetValue(ViewModelsProperty, value); }
        }

        /// <summary>
        /// The selected view model.
        /// </summary>
        /// <value>The selected view model.</value>
        public T SelectedViewModel {
            get { return (T)GetValue (SelectedViewModelProperty); }
            set { SetValue (SelectedViewModelProperty, value); }
        }

        /// <summary>
        /// Used to match a type with a datatemplate
        /// <see cref="TemplateSelector" />
        /// </summary>
        /// <value>The template selector.</value>
        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Invoked whenever the binding context of the <see cref="T:Xamarin.Forms.View" /> changes. Override this method to add class handling for this event.
        /// </summary>
        /// <remarks>Overriders must call the base method.</remarks>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();            
            foreach (var i in this.interests) i.BindingContext = BindingContext;
            this.gestureView.BindingContext = BindingContext;
            this.contentView.BindingContext = BindingContext;
        }

        /// <summary>
        /// Setups the tick board.
        /// </summary>
        private void SetupTickBoard()
        {
            this.myGrid.ColumnDefinitions.Clear();
            for (var i = 0; i < ViewModels.Count; i++)
            {
                this.myGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        /// <summary>
        /// Switches the view.
        /// </summary>
        /// <param name="increment">if set to <c>true</c> [increment].</param>
        private void SwitchView(bool increment)
        {
            var newval = this.currentview + (increment ? 1 : -1);
            SwitchView (newval);
        }

        /// <summary>
        /// Switches the view. Based on absolute position.
        /// </summary>
        /// <param name="newval">New position.</param>
        private void SwitchView(int newval)
        {
            if (newval < 0 || newval > ViewModels.Count() - 1) return;
            var oldview = this.currentview >=0 ?  ViewModels[this.currentview] as ICarouselView : null;
            var newview = ViewModels[newval] as ICarouselView;

            myGrid.Children.Clear();
            if(oldview != null) oldview.Hiding();
            if(newview != null) newview.Showing();
            this.currentview = newval;
            var newVm = ViewModels [this.currentview];
            this.contentView.ViewModel = newVm;
            this.SelectedViewModel = newVm;
            if(oldview != null) oldview.Hiden();
            if(newview != null) newview.Shown();
            this.myGrid.Children.Add(this.marker, this.currentview, 0);
        }

        /// <summary>
        /// Shows the tick changed.
        /// </summary>
        /// <param name="newval">if set to <c>true</c> [newval].</param>
        private void ShowTickChanged(bool newval)
        {
            this.myGrid.IsVisible = newval;
        }

        /// <summary>
        /// Selectors the changed.
        /// </summary>
        /// <param name="newval">The newval.</param>
        private void SelectorChanged(TemplateSelector newval)
        {
            if (this.contentView != null)//may be constructing
            {
                this.contentView.TemplateSelector = newval;
            }
        }

        /// <summary>
        /// Views the models changed.
        /// </summary>
        /// <param name="oldval">The oldval.</param>
        /// <param name="newval">The newval.</param>
        private void ViewModelsChanged(ObservableCollection<T> oldval, ObservableCollection<T> newval)
        {
            this.currentview = -1;
            if (this.contentView != null)
            {
                SetupTickBoard();
                SwitchView(true);
            }
            if (oldval != null) oldval.CollectionChanged -= ViewModelCollectionContentsChanged;
            newval.CollectionChanged += ViewModelCollectionContentsChanged;
        }

        /// <summary>
        /// Views the model collection contents changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ViewModelCollectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetupTickBoard();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this.currentview = -1;
                    this.contentView.Content = null;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == this.currentview) //Well damn
                        SwitchView(this.currentview == 0);
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == this.currentview)
                        SwitchView(this.currentview < ViewModels.Count);
                    break;
            }
        }

        /// <summary>
        ///The selected viewmodel changed.
        /// </summary>
        /// <param name="bo">The bo.</param>
        /// <param name="oldval">The oldval.</param>
        /// <param name="newval">The newval.</param>
        /// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
        private static void SelectedViewModelChanged(BindableObject bo, T oldval, T newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));

            var newPosition = cv.ViewModels.IndexOf (newval);
            if(newPosition >= 0 && newPosition != cv.currentview)	
                cv.SwitchView (newPosition);
        }

        /// <summary>
        /// Shows the tickchanged.
        /// </summary>
        /// <param name="bo">The bo.</param>
        /// <param name="oldval">if set to <c>true</c> [oldval].</param>
        /// <param name="newval">if set to <c>true</c> [newval].</param>
        /// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
        private static void ShowTickchanged(BindableObject bo, bool oldval, bool newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));
            cv.ShowTickChanged(newval);
        }

        /// <summary>
        /// Views the models changed.
        /// </summary>
        /// <param name="bo">The bo.</param>
        /// <param name="oldval">The oldval.</param>
        /// <param name="newval">The newval.</param>
        /// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
        private static void ViewModelsChanged(BindableObject bo, ObservableCollection<T> oldval, ObservableCollection<T> newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));
            cv.ViewModelsChanged(oldval, newval);
        }

        /// <summary>
        /// Templates the selector changed.
        /// </summary>
        /// <param name="bo">The bo.</param>
        /// <param name="oldval">The oldval.</param>
        /// <param name="newval">The newval.</param>
        /// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
        private static void TemplateSelectorChanged(BindableObject bo, TemplateSelector oldval, TemplateSelector newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
            {
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));
            }

            cv.SelectorChanged(newval);
        }
    }
}

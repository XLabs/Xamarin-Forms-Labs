using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// Define the AutoCompleteView control.
    /// </summary>
    public class AutoCompleteView : ContentView
    {
        /// <summary>
        /// The execute on suggestion click property.
        /// </summary>
        public static readonly BindableProperty ExecuteOnSuggestionClickProperty = BindableProperty.Create<AutoCompleteView, bool>(p => p.ExecuteOnSuggestionClick, false);

        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.Placeholder, string.Empty, BindingMode.TwoWay, null, PlaceHolderChanged);

        /// <summary>
        /// The search background color property.
        /// </summary>
        public static readonly BindableProperty SearchBackgroundColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.SearchBackgroundColor, Color.Red, BindingMode.TwoWay, null, SearchBackgroundColorChanged);

        /// <summary>
        /// The search border color property.
        /// </summary>
        public static readonly BindableProperty SearchBorderColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.SearchBorderColor, Color.White, BindingMode.TwoWay, null, SearchBorderColorChanged);

        /// <summary>
        /// The search border radius property.
        /// </summary>
        public static readonly BindableProperty SearchBorderRadiusProperty = BindableProperty.Create<AutoCompleteView, int>(p => p.SearchBorderRadius, 0, BindingMode.TwoWay, null, SearchBorderRadiusChanged);

        /// <summary>
        /// The search border width property.
        /// </summary>
        public static readonly BindableProperty SearchBorderWidthProperty = BindableProperty.Create<AutoCompleteView, int>(p => p.SearchBorderWidth, 1, BindingMode.TwoWay, null, SearchBorderWidthChanged);

        /// <summary>
        /// The search command property.
        /// </summary>
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create<AutoCompleteView, ICommand>(p => p.SearchCommand, null);

        /// <summary>
        /// The search horizontal options property
        /// </summary>
        public static readonly BindableProperty SearchHorizontalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.SearchHorizontalOptions, LayoutOptions.FillAndExpand, BindingMode.TwoWay, null, SearchHorizontalOptionsChanged);

        /// <summary>
        /// The search text color property.
        /// </summary>
        public static readonly BindableProperty SearchTextColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.SearchTextColor, Color.Red, BindingMode.TwoWay, null, SearchTextColorChanged);

        /// <summary>
        /// The search text property.
        /// </summary>
        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.SearchText, "Search", BindingMode.TwoWay, null, SearchTextChanged);

        /// <summary>
        /// The search vertical options property
        /// </summary>
        public static readonly BindableProperty SearchVerticalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.SearchVerticalOptions, LayoutOptions.Center, BindingMode.TwoWay, null, SearchVerticalOptionsChanged);

        /// <summary>
        /// The selected command property.
        /// </summary>
        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create<AutoCompleteView, ICommand>(p => p.SelectedCommand, null);

        /// <summary>
        /// The selected item property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<AutoCompleteView, object>(p => p.SelectedItem, null, BindingMode.TwoWay);

        /// <summary>
        /// The show search property.
        /// </summary>
        public static readonly BindableProperty ShowSearchProperty = BindableProperty.Create<AutoCompleteView, bool>(p => p.ShowSearchButton, true, BindingMode.TwoWay, null, ShowSearchChanged);

        /// <summary>
        /// The suggestion background color property.
        /// </summary>
        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.SuggestionBackgroundColor, Color.Red, BindingMode.TwoWay, null, SuggestionBackgroundColorChanged);

        /// <summary>
        /// The suggestion item data template property.
        /// </summary>
        public static readonly BindableProperty SuggestionItemDataTemplateProperty = BindableProperty.Create<AutoCompleteView, DataTemplate>(p => p.SuggestionItemDataTemplate, null, BindingMode.TwoWay, null, SuggestionItemDataTemplateChanged);

        /// <summary>
        /// The suggestion height request property.
        /// </summary>
        public static readonly BindableProperty SuggestionsHeightRequestProperty = BindableProperty.Create<AutoCompleteView, double>(p => p.SuggestionsHeightRequest, 250, BindingMode.TwoWay, null, SuggestionHeightRequestChanged);

        /// <summary>
        /// The suggestions property.
        /// </summary>
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create<AutoCompleteView, IEnumerable>(p => p.Suggestions, null);

        /// <summary>
        /// The text background color property.
        /// </summary>
        public static readonly BindableProperty TextBackgroundColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.TextBackgroundColor, Color.Transparent, BindingMode.TwoWay, null, TextBackgroundColorChanged);

        /// <summary>
        /// The text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.TextBackgroundColor, Color.Black, BindingMode.TwoWay, null, TextColorChanged);

        /// <summary>
        /// The text horizontal options property
        /// </summary>
        public static readonly BindableProperty TextHorizontalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.TextHorizontalOptions, LayoutOptions.FillAndExpand, BindingMode.TwoWay, null, TextHorizontalOptionsChanged);

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.Text, string.Empty, BindingMode.TwoWay, null, TextValueChanged);

        /// <summary>
        /// The text vertical options property.
        /// </summary>
        public static readonly BindableProperty TextVerticalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.TextVerticalOptions, LayoutOptions.Start, BindingMode.TwoWay, null, TestVerticalOptionsChanged);
        private readonly ObservableCollection<object> _availableSuggestions;
        private readonly Button _btnSearch;
        private readonly Entry _entText;
        private readonly ListView _lstSuggestions;
        private readonly StackLayout _stkBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteView"/> class.
        /// </summary>
        public AutoCompleteView()
        {
            _availableSuggestions = new ObservableCollection<object>();
            _stkBase = new StackLayout();
            var innerLayout = new StackLayout();
            _entText = new Entry
            {
                HorizontalOptions = TextHorizontalOptions,
                VerticalOptions = TextVerticalOptions,
                TextColor =  TextColor,
                BackgroundColor = TextBackgroundColor
            };
            _btnSearch = new Button
            {
                VerticalOptions = SearchVerticalOptions,
                HorizontalOptions = SearchHorizontalOptions,
                Text = SearchText
            };

            _lstSuggestions = new ListView
            {
                HeightRequest = SuggestionsHeightRequest,
                HasUnevenRows = true
            };

            innerLayout.Children.Add(_entText);
            innerLayout.Children.Add(_btnSearch);
            _stkBase.Children.Add(innerLayout);
            _stkBase.Children.Add(_lstSuggestions);

            Content = _stkBase;


            _entText.TextChanged += (s, e) =>
            {
                Text = e.NewTextValue;
                OnTextChanged(e);
            };
            _btnSearch.Clicked += (s, e) =>
            {
                if (SearchCommand != null && SearchCommand.CanExecute(Text))
                {
                    SearchCommand.Execute(Text);
                }
            };
            _lstSuggestions.ItemSelected += (s, e) =>
            {
                _entText.Text = e.SelectedItem.ToString();

                _availableSuggestions.Clear();
                ShowHideListbox(false);
                OnSelectedItemChanged(e.SelectedItem);
             
                if (ExecuteOnSuggestionClick
                   && SearchCommand != null 
                   && SearchCommand.CanExecute(Text))
                {
                    SearchCommand.Execute(e);
                }
            };
            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = _availableSuggestions;
        }
        
        /// <summary>
        /// Occurs when [selected item changed].
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        /// <summary>
        /// Occurs when [text changed].
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets the available Suggestions.
        /// </summary>
        /// <value>The available Suggestions.</value>
        public IEnumerable AvailableSuggestions
        {
            get { return _availableSuggestions; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [execute on sugestion click].
        /// </summary>
        /// <value><c>true</c> if [execute on sugestion click]; otherwise, <c>false</c>.</value>
        public bool ExecuteOnSuggestionClick
        {
            get { return (bool)GetValue(ExecuteOnSuggestionClickProperty); }
            set { SetValue(ExecuteOnSuggestionClickProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>The placeholder.</value>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the search background.
        /// </summary>
        /// <value>The color of the search background.</value>
        public Color SearchBackgroundColor
        {
            get { return (Color)GetValue(SearchBackgroundColorProperty); }
            set { SetValue(SearchBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search border color.
        /// </summary>
        /// <value>The search border brush.</value>
        public Color SearchBorderColor
        {
            get { return (Color)GetValue(SearchBorderColorProperty); }
            set { SetValue(SearchBorderColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search border radius.
        /// </summary>
        /// <value>The search border radius.</value>
        public int SearchBorderRadius
        {
            get { return (int)GetValue(SearchBorderRadiusProperty); }
            set { SetValue(SearchBorderRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the search border.
        /// </summary>
        /// <value>The width of the search border.</value>
        public int SearchBorderWidth
        {
            get { return (int)GetValue(SearchBorderWidthProperty); }
            set { SetValue(SearchBorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search horizontal options.
        /// </summary>
        /// <value>The search horizontal options.</value>
        public LayoutOptions SearchHorizontalOptions
        {
            get { return (LayoutOptions)GetValue(SearchHorizontalOptionsProperty); }
            set { SetValue(SearchHorizontalOptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the search text button.
        /// </summary>
        /// <value>The color of the search text.</value>
        public Color SearchTextColor
        {
            get { return (Color)GetValue(SearchTextColorProperty); }
            set { SetValue(SearchTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search vertical options.
        /// </summary>
        /// <value>The search vertical options.</value>
        public LayoutOptions SearchVerticalOptions
        {
            get { return (LayoutOptions)GetValue(SearchVerticalOptionsProperty); }
            set { SetValue(SearchVerticalOptionsProperty, value); }
        }


        /// <summary>
        /// Gets or sets the selected command.
        /// </summary>
        /// <value>The selected command.</value>
        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (Color)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show search button].
        /// </summary>
        /// <value><c>true</c> if [show search button]; otherwise, <c>false</c>.</value>
        public bool ShowSearchButton
        {
            get { return (bool)GetValue(ShowSearchProperty); }
            set { SetValue(ShowSearchProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the sugestion background.
        /// </summary>
        /// <value>The color of the sugestion background.</value>
        public Color SuggestionBackgroundColor
        {
            get { return (Color)GetValue(SuggestionBackgroundColorProperty); }
            set { SetValue(SuggestionBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the suggestion item data template.
        /// </summary>
        /// <value>The sugestion item data template.</value>
        public DataTemplate SuggestionItemDataTemplate
        {
            get { return (DataTemplate)GetValue(SuggestionItemDataTemplateProperty); }
            set { SetValue(SuggestionItemDataTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Suggestions.
        /// </summary>
        /// <value>The Suggestions.</value>
        public IEnumerable Suggestions
        {
            get { return (IEnumerable)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the suggestion.
        /// </summary>
        /// <value>The height of the suggestion.</value>
        public double SuggestionsHeightRequest
        {
            get { return (double)GetValue(SuggestionsHeightRequestProperty); }
            set { SetValue(SuggestionsHeightRequestProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text background.
        /// </summary>
        /// <value>The color of the text background.</value>
        public Color TextBackgroundColor
        {
            get { return (Color)GetValue(TextBackgroundColorProperty); }
            set { SetValue(TextBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text horizontal options.
        /// </summary>
        /// <value>The text horizontal options.</value>
        public LayoutOptions TextHorizontalOptions
        {
            get { return (LayoutOptions)GetValue(TextHorizontalOptionsProperty); }
            set { SetValue(TextHorizontalOptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text vertical options.
        /// </summary>
        /// <value>The text vertical options.</value>
        public LayoutOptions TextVerticalOptions
        {
            get { return (LayoutOptions)GetValue(TextVerticalOptionsProperty); }
            set { SetValue(TextVerticalOptionsProperty, value); }
        }
        /// <summary>
        /// Places the holder changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldPlaceHolderValue">The old place holder value.</param>
        /// <param name="newPlaceHolderValue">The new place holder value.</param>
        private static void PlaceHolderChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.Placeholder = newPlaceHolderValue;
            }
        }

        /// <summary>
        /// Searches the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._stkBase.BackgroundColor = newValue;
            }
        }

        /// <summary>
        /// Searches the border color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchBorderColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.BorderColor = newValue;
            }
        }

        /// <summary>
        /// Searches the border radius changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchBorderRadiusChanged(BindableObject obj, int oldValue, int newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.BorderRadius = newValue;
            }
        }

        /// <summary>
        /// Searches the border width changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchBorderWidthChanged(BindableObject obj, int oldValue, int newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.BorderWidth = newValue;
            }
        }

        /// <summary>
        /// Searches the horizontal options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchHorizontalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.HorizontalOptions = newValue;
            }
        }

        /// <summary>
        /// Searches the text changed.
        /// </summary>
        /// <param name="obj">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchTextChanged(BindableObject obj, string oldValue, string newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.Text = newValue;
            }
        }

        /// <summary>
        /// Searches the text color color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchTextColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.TextColor = newValue;
            }
        }

        /// <summary>
        /// Searches the vertical options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SearchVerticalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.VerticalOptions = newValue;
            }
        }

        /// <summary>
        /// Shows the search changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldShowSearchValue">if set to <c>true</c> [old show search value].</param>
        /// <param name="newShowSearchValue">if set to <c>true</c> [new show search value].</param>
        private static void ShowSearchChanged(BindableObject obj, bool oldShowSearchValue, bool newShowSearchValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._btnSearch.IsVisible = newShowSearchValue;
            }
        }

        /// <summary>
        /// Suggestions the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.BackgroundColor = newValue;
            }
        }

        /// <summary>
        /// Suggestions the height changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionHeightRequestChanged(BindableObject obj, double oldValue, double newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.HeightRequest = newValue;
            }
        }
        /// <summary>
        /// Suggestions the item data template changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldShowSearchValue">The old show search value.</param>
        /// <param name="newShowSearchValue">The new show search value.</param>
        private static void SuggestionItemDataTemplateChanged(BindableObject obj, DataTemplate oldShowSearchValue, DataTemplate newShowSearchValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.ItemTemplate = newShowSearchValue;
            }
        }

        /// <summary>
        /// Tests the vertical options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TestVerticalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.VerticalOptions = newValue;
            }
        }

        /// <summary>
        /// Texts the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.BackgroundColor = newValue;
            }
        }

        /// <summary>
        /// Texts the color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.TextColor = newValue;
            }
        }

        /// <summary>
        /// Texts the horizontal options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextHorizontalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.VerticalOptions = newValue;
            }
        }
        /// <summary>
        /// Texts the changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldPlaceHolderValue">The old place holder value.</param>
        /// <param name="newPlaceHolderValue">The new place holder value.</param>
        private static void TextValueChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var control = obj as AutoCompleteView;

            if (control != null)
            {
                control._btnSearch.IsEnabled = !string.IsNullOrEmpty(newPlaceHolderValue);
                
                var cleanedNewPlaceHolderValue = Regex.Replace((newPlaceHolderValue ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);
                
                if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && control.Suggestions != null)
                {
                    var filteredSuggestions = control.Suggestions.Cast<object>()
                        .Where(x => Regex.Replace(x.ToString().ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                        .OrderByDescending(x => Regex.Replace(x.ToString()
                        .ToLowerInvariant(), @"\s+", string.Empty)
                        .StartsWith(cleanedNewPlaceHolderValue)).ToList();

                    control._availableSuggestions.Clear();
                    if (filteredSuggestions.Count > 0)
                    {
                        foreach (var suggestion in filteredSuggestions)
                        {
                            control._availableSuggestions.Add(suggestion);
                        }
                        control.ShowHideListbox(true);
                    }
                    else
                    {
                        control.ShowHideListbox(false);
                    }
                }
                else
                {
                    if (control._availableSuggestions.Count > 0)
                    {
                        control._availableSuggestions.Clear();
                        control.ShowHideListbox(false);
                    }
                }
            }
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        private void OnSelectedItemChanged(object selectedItem)
        {
            SelectedItem = selectedItem;
            SelectedCommand.Execute(selectedItem);

            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, new SelectedItemChangedEventArgs(selectedItem));
            }
        }

        /// <summary>
        /// Handles the <see cref="E:TextChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        /// <summary>
        /// Shows the hide listbox.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowHideListbox(bool show)
        {
            _lstSuggestions.IsVisible = show;
        }
    }
}

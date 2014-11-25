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
        /// The search command property.
        /// </summary>
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create<AutoCompleteView, ICommand>(p => p.SearchCommand, null);

        /// <summary>
        /// The selected command property.
        /// </summary>
        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create<AutoCompleteView, ICommand>(p => p.SelectedCommand, null);

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
        /// The suggestions property.
        /// </summary>
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create<AutoCompleteView, ObservableCollection<object>>(p => p.Suggestions, null);

        /// <summary>
        /// The text property
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.Text, string.Empty, BindingMode.TwoWay, null, TextChanged);

        private readonly Button _btnSearch;
        private readonly Entry _entText;
        private readonly ListView _lstSuggestions;
        private readonly StackLayout _stkBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteView"/> class.
        /// </summary>
        public AutoCompleteView()
        {
            _stkBase = new StackLayout();
            var innerLayout = new StackLayout();
            _entText = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start
            };
            _btnSearch = new Button
            {
                VerticalOptions = LayoutOptions.Center,
                Text = "Search"
            };

            _lstSuggestions = new ListView
            {
                HeightRequest = 250,
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
            };
            _btnSearch.Clicked += (s, e) =>
            {
                if (SearchCommand != null && SearchCommand.CanExecute(Text))
                    SearchCommand.Execute(Text);
            };
            _lstSuggestions.ItemSelected += (s, e) =>
            {
                _entText.Text = e.SelectedItem.ToString();

                AvailableSuggestions.Clear();
                ShowHideListbox(false);
                SelectedCommand.Execute(e.SelectedItem);
                if (ExecuteOnSuggestionClick
                   && SearchCommand != null && SearchCommand.CanExecute(Text))
                {
                    SearchCommand.Execute(e);
                }

            };
            AvailableSuggestions = new ObservableCollection<object>();
            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = AvailableSuggestions;
        }

        /// <summary>
        /// Gets the available Suggestions.
        /// </summary>
        /// <value>The available Suggestions.</value>
        public ObservableCollection<object> AvailableSuggestions
        {
            get;
            private set;
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
        /// Gets or sets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
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
        public ObservableCollection<object> Suggestions
        {
            get { return (ObservableCollection<object>)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
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
        /// Texts the changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldPlaceHolderValue">The old place holder value.</param>
        /// <param name="newPlaceHolderValue">The new place holder value.</param>
        private static void TextChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var control = obj as AutoCompleteView;

            if (control != null)
            {
                control._btnSearch.IsEnabled = !string.IsNullOrEmpty(newPlaceHolderValue);
                string cleanedNewPlaceHolderValue = Regex.Replace((newPlaceHolderValue ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);
                if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && control.Suggestions != null)
                {
                    var filteredSuggestions = control.Suggestions.Where(x => Regex.Replace(x.ToString().ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue)).OrderByDescending(x => Regex.Replace(x.ToString().ToLowerInvariant(), @"\s+", string.Empty).StartsWith(cleanedNewPlaceHolderValue)).ToArray();

                    control.AvailableSuggestions.Clear();

                    foreach (var item in filteredSuggestions)
                    {
                        control.AvailableSuggestions.Add(item);
                    }
                    if (control.AvailableSuggestions.Count > 0)
                    {
                        control.ShowHideListbox(true);
                    }
                }
                else
                {
                    if (control.AvailableSuggestions.Count > 0)
                    {
                        control.AvailableSuggestions.Clear();
                        control.ShowHideListbox(false);
                    }
                }
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

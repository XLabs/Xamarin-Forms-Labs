namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// Define the  ViewModelLocator.
    /// </summary>
	public class ViewModelLocator
    {
		private static MainViewModel _main;
	    private static AutoCompleteViewModel _autoCompleteViewModel;

        /// <summary>
        /// Gets the main.
        /// </summary>
        /// <value>The main.</value>
		public static MainViewModel Main
        {
            get
            {
				if (_main == null)
					_main = new MainViewModel ();
				return _main;
            }
        }

        /// <summary>
        /// Gets the automatic complete view model.
        /// </summary>
        /// <value>The automatic complete view model.</value>
        public static AutoCompleteViewModel AutoCompleteViewModel
        {
            get
            {
                if (_autoCompleteViewModel == null)
                {
                    _autoCompleteViewModel = new AutoCompleteViewModel();
                }
                return _autoCompleteViewModel;
            }
        }
    }
}

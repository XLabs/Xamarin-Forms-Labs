namespace Xamarin.Forms.Labs.Mvvm.Views
{
  #region Usings

  using System.Threading.Tasks;

  #endregion

  public class BaseNavigationView : NavigationPage,
                                    INavigable,
                                    IAlertable
  {
    /// <inheritdoc />
    public string Name
    {
      get
      {
        return _name ?? GetType().Name;
      }
      protected set
      {
        _name = value;
      }
    }

    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNavigationView"/> class.
    /// </summary>
    public BaseNavigationView()
      : this(null)
    {}

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNavigationView"/> class using
    /// the root page passed in as the content of the page.
    /// </summary>
    public BaseNavigationView(INavigable root)
      : base(root as Page)
    {
      SetValue(HasNavigationBarProperty, false); //TODO: why
    }

    /// <inheritdoc />
    protected override void OnAppearing()
    {
      base.OnAppearing();

      BaseView.DoAppearing(this);
    }

    /// <inheritdoc />
    protected override void OnDisappearing()
    {
      base.OnDisappearing();

      BaseView.DoDisappearing(this);
    }

    /// <inheritdoc />
    public new async Task<INavigable> PopAsync()
    {
      return await Navigation.PopAsync() as INavigable;
    }

    /// <inheritdoc />
    public async Task<INavigable> PopModalAsync()
    {
      return await Navigation.PopModalAsync() as INavigable;
    }

    /// <inheritdoc />
    public async Task<INavigable> PopToRootAsync()
    {
      return await Navigation.PopAsync() as INavigable;
    }

    /// <inheritdoc />
    public async Task PushAsync(INavigable nav)
    {
      var view = nav as Page;
      await Navigation.PushAsync(view);
    }

    /// <inheritdoc />
    public async Task PushModalAsync(INavigable nav)
    {
      var view = nav as Page;
      await Navigation.PushModalAsync(view);
    }
  }
}
namespace Xamarin.Forms.Labs.Mvvm.Views
{
  #region Usings

  using System.Threading.Tasks;
  using Xamarin.Forms.Labs.Mvvm.ViewModels;

  #endregion

  public class BaseView : ContentPage,
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

    /// <inheritdoc />
    protected override void OnAppearing()
    {
      base.OnAppearing();

      DoAppearing(this);
    }

    /// <inheritdoc />
    protected override void OnDisappearing()
    {
      base.OnDisappearing();

      DoDisappearing(this);
    }

    public static void DoAppearing(Page it)
    {
      if (it == null) return;
      var vm = it.BindingContext as IViewModel;
      if (vm == null) return;

      vm.OnAppearing(it as INavigable);
    }

    public static bool DoDisappearing(Page it)
    {
      if (it == null) return false;
      var vm = it.BindingContext as IViewModel;
      if (vm == null) return false;

      return vm.OnDisappearing(it as INavigable);
    }

    /// <inheritdoc />
    public async Task<INavigable> PopAsync()
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
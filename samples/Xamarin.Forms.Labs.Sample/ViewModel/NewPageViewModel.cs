namespace Xamarin.Forms.Labs.Sample
{
  #region Usings

  using System;

  #endregion

  public class NewPageViewModel : Mvvm.ViewModels.ViewModel
  {
    public string Id
    {
      get
      {
        return _id;
      }
      set
      {
        SetProperty(ref _id, value);
      }
    }
    private string _id;

    public string NewPage
    {
      get
      {
        return _newPage;
      }
      set
      {
        SetProperty(ref _newPage, value);
      }
    }
    private string _newPage = string.Empty;

    public string KeyedNewPage
    {
      get
      {
        return _keyedNewPage;
      }
      set
      {
        SetProperty(ref _keyedNewPage, value);
      }
    }
    private string _keyedNewPage = string.Empty;

    public string PageTitle
    {
      get
      {
        return _pageTitle;
      }
      set
      {
        SetProperty(ref _pageTitle, value);
      }
    }
    private string _pageTitle = "New Page";

    public NewPageViewModel()
    {
      NewPage = @"This page was created by the view factory and binded to the ViewModel and injected a navigation context using the following code:
            ViewFactory.Instance.Register<NewPageView,NewPageViewModel> ();
            We can also navigate to this page from any view model using the following code: 
            await NavigationService.PushAsync<NewPageViewModel>() ";

      KeyedNewPage = @"This page was created by the view factory and binded to the ViewModel and injected a navigation context using the following code:
            ViewFactory.Instance.Register<NewPageView,NewPageViewModel> (""key"");
            We can also navigate to this page from any view model using the following code: 
            await NavigationService.PushAsync<NewPageViewModel>(""key"") ";

      Id = Guid.NewGuid().ToString();
    }
  }
}
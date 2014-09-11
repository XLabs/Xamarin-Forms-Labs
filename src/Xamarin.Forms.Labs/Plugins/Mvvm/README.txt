
Interfaces used by Mvvm:

  IAlertable - An interface to expose a method to display an alert message to the user.
               Usually implemented by the ViewModel.
  IViewModel - The interface all ViewModels need to implement.
  INavigationService - The interface used by all navigation services to navigate to another view model,
                       and thus through the ViewFactory to the view.
  INavigable - The interface a view must use to participate in the Mvvm process and provide the basic navigation to enable
               a view to goto another view, and to return to the calling view.
  IViewFactory - Interface used by view factories to manage the mapping between view models and views and creates the view on demand. 

To use Mvvm :

  Derive your ViewModels from ViewModel or implement IViewModel.

  Derive your views/pages from one of BaseCarouselView, BaseMasterDetailView, BaseNavigationView, BaseTabbedView or BaseView, or implement INavigable and IAlertable.

NavigationService:
  You can use the default one in which case you don't have to do anything. Otherwise:

  . Create your own and implement INavigationService.
  . Register your service somewhere early in application initialization. e.g, in App.cs of your PCL
  public class App
  {
    /// <summary>
    /// Initializes the application.
    /// </summary>
    public static void Init()
    {
      var container = Resolver.Resolve<IDependencyContainer>();
      container.Register<INavigationService>(new MyNavigationService());

ViewFactory:
  You can use the default one in which case you don't have to do anything. Otherwise:

  . Create your own and implement IViewFactory.
  . Register your factory somewhere early in application initialization. e.g, in App.cs of your PCL
  public class App
  {
    /// <summary>
    /// Initializes the application.
    /// </summary>
    public static void Init()
    {
      var container = Resolver.Resolve<IDependencyContainer>();
      container.Register<IViewFactory>(new MyViewFactory());
      container.Register<INavigationService>(new MyNavigationService());

You need to register your main page with the container so the navigation service has access to a real Page's Navigation methods and to also display a message to the user. E.g,

    public static Page GetMainPage()
    {
      var mainTab = new ExtendedTabbedPage() { Title = "Xamarin Forms Labs" };
      var mainPage = new BaseNavigationView(mainTab);
      
      var container = Resolver.Resolve<IDependencyContainer>();
      // The main page exists for the lifetime of the app (I think) so uses it as then main INavigable and IAlertable.
      container.Register<INavigable>(mainPage);
      container.Register<IAlertable>(mainPage);

You need to register your mappings between ViewModels and Views, somewhere during application initialization:

      ViewFactory.Instance.Register<MvvmSamplePage, MvvmSampleViewModel>();
      ViewFactory.Instance.Register<NewPageView, NewPageViewModel>();
      ViewFactory.Instance.Register<KeyedNewPageView, NewPageViewModel>("key");
      ViewFactory.Instance.Register<GeolocatorPage, GeolocatorViewModel>();

Then to navigate to another view/page:

await NavigationService.PushAsync<NewPageViewModel>();
await NavigationService.PushAsync<NewPageViewModel>("key");

Notice the use of "key" above both to register your mapping and to navigate to it.
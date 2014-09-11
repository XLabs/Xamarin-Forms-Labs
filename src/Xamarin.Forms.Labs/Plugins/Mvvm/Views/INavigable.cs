namespace Xamarin.Forms.Labs.Mvvm.Views
{
  #region Usings

  using System.Threading.Tasks;

  #endregion

  /// <summary>
  /// The interface a view must use to participate in the Mvvm process.
  /// </summary>
  public interface INavigable
  {
    /// <summary>
    /// The name of the view.
    /// </summary>
    /// <remarks>
    /// Used if during processing of events, e.g, OnDisappearing, a view or view model needs to know what view disappeared.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// The standard binding context of the view.
    /// </summary>
    object BindingContext { get; set; }

    /// <summary>
    /// Asynchronously removes the top <see cref="INavigable"/> from the navigation stack.
    /// </summary>
    /// <returns>The <see cref="INavigable"/> that had been at the top of the navigation stack.</returns>
    Task<INavigable> PopAsync();

    /// <summary>
    /// Asynchronously removes the top <see cref="INavigable"/> from the navigation stack that was pushed modally.
    /// </summary>
    /// <returns>The <see cref="INavigable"/> that had been at the top of the navigation stack.</returns>
    Task<INavigable> PopModalAsync();

    /// <summary>
    /// Asynchronously removes all <see cref="INavigable"/> from the navigation stack, except the root one.
    /// </summary>
    /// <returns>The <see cref="INavigable"/> that had been at the top of the navigation stack.</returns>
    Task<INavigable> PopToRootAsync();

    /// <summary>
    /// Asynchronously adds a <see cref="INavigable"/> to the top of the navigation stack.
    /// </summary>
    /// <param name="nav">The <see cref="INavigable"/> to push onto the stack.</param>
    Task PushAsync(INavigable nav);

    /// <summary>
    /// Asynchronously adds a <see cref="INavigable"/> modally to the top of the navigation stack.
    /// </summary>
    /// <param name="nav">The <see cref="INavigable"/> to push onto the stack modally.</param>
    Task PushModalAsync(INavigable nav);
  }
}
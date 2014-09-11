namespace Xamarin.Forms.Labs.Mvvm.Views
{
  using System.Threading.Tasks;

  /// <summary>
  /// An interface to allow a class to expose a method used to display an alert to the user.
  /// </summary>
  public interface IAlertable
  {
    /// <summary>
    /// Allows the <see cref="IAlertable"/> to display an alert message.
    /// </summary>
    /// <param name="title">The title of the message.</param>
    /// <param name="message">The message.</param>
    /// <param name="buttonText">The text to appear on the button.</param>
    /// <returns></returns>
    Task DisplayAlert(string title, string message, string buttonText = "Ok");
  }
}
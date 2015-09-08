using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Email;
using Xamarin.Forms.Labs.Services.Media;
using XLabs.Ioc;

namespace Xamarin.Forms.Labs.Sample.Pages.Services
{
    public class EmailPage : ContentPage
    {
        private IDevice Device;
        private IEmailService EmailService;
        private IMediaPicker MediaPicker;
        private readonly TaskScheduler Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private string Path;

        public string Status
        {
            get;
            set;
        }

        public EmailPage()
        {
            Padding = new Thickness(0, Forms.Device.OnPlatform(20, 0, 0), 0, 0);

            EmailService = DependencyService.Get<IEmailService>();

            SetupCamera();

            Status = "Ready";

            var stack = new StackLayout();

            if (EmailService == null || !EmailService.CanSend)
            {
                stack.Children.Add(new Label
                {
                    TextColor = Color.Red,
                    Text = "No Email support",
                });
            }
            else
            {
                var buttonTakePicture = new Button
                {
                    Text = "Take Picture",
                };

                var buttonSendEmail = new Button
                {
                    Text = "Send Email",
                };

                var buttonSendEmailWithAttachment = new Button
                {
                    Text = "Send Email with Attachment",
                    IsEnabled = false,
                };

                buttonTakePicture.Clicked += async (sender, args) =>
                {
                    await TakePicture();

                    buttonSendEmailWithAttachment.IsEnabled = !string.IsNullOrEmpty(Path);
                };

                buttonSendEmail.Clicked += (sender, args) => EmailService.ShowDraft(
                                    "Test Subject",
                                    "Test Body",
                                    true,
                                    string.Empty,
                                    Enumerable.Empty<string>());

                buttonSendEmailWithAttachment.Clicked += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(Path))
                    {
                        DisplayAlert("Error", "You must first take a pictures.", null, "OK");
                    }
                    else
                    {
                        EmailService.ShowDraft(
                            "Test Subject",
                            "Test Body",
                            true,
                            string.Empty,
                            new List<string> { Path });
                    }
                };

                var labelStatus = new Label
                {
                    Text = string.Empty,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };

                labelStatus.SetBinding(Label.TextProperty, new Binding(Status));

                stack.Children.Add(buttonTakePicture);
                stack.Children.Add(buttonSendEmail);
                stack.Children.Add(buttonSendEmailWithAttachment);
                stack.Children.Add(labelStatus);
            }

            Content = stack;
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        private void SetupCamera()
        {
            if (MediaPicker != null)
            {
                return;
            }

            Device = Resolver.Resolve<IDevice>();

            ////RM: hack for working on windows phone? 
            MediaPicker = DependencyService.Get<IMediaPicker>() ?? Device.MediaPicker;
        }

        /// <summary>
        /// Takes the picture. This is a variation from the CameraPage example.
        /// In this version, we need to return the Path to the file, since we need
        /// that path to attach it to the email.
        /// </summary>
        /// <returns>Take Picture Task.</returns>
        private async Task<MediaFile> TakePicture()
        {
            SetupCamera();

            return await MediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Status = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    Status = "Canceled";
                }
                else
                {
                    var mediaFile = t.Result;

                    Path = mediaFile.Path;

                    Status = string.Format("Path: {0}", Path);

                    return mediaFile;
                }

                return null;
            }, Scheduler);
        }
    }
}

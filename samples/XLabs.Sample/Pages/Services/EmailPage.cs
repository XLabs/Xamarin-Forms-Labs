namespace XLabs.Sample.Pages.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Xamarin.Forms;

	using XLabs.Ioc;
	using XLabs.Platform.Device;
	using XLabs.Platform.Services.Email;
	using XLabs.Platform.Services.Media;

	/// <summary>
	/// Class EmailPage.
	/// </summary>
	public class EmailPage : ContentPage
	{
		/// <summary>
		/// The device
		/// </summary>
		private IDevice _device;
		/// <summary>
		/// The email service
		/// </summary>
		private readonly IEmailService _emailService;
		/// <summary>
		/// The media picker
		/// </summary>
		private IMediaPicker _mediaPicker;
		/// <summary>
		/// The scheduler
		/// </summary>
		private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		/// <summary>
		/// The path
		/// </summary>
		private string _path;

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailPage"/> class.
		/// </summary>
		public EmailPage()
		{
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

			_emailService = Resolver.Resolve<IEmailService>();

			SetupCamera();

			Status = "Ready";

			var stack = new StackLayout();

			if (_emailService == null || !_emailService.CanSend)
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

					buttonSendEmailWithAttachment.IsEnabled = !string.IsNullOrEmpty(_path);
				};

				buttonSendEmail.Clicked += (sender, args) => _emailService.ShowDraft(
									"Test Subject",
									"Test Body",
									true,
									string.Empty,
									Enumerable.Empty<string>());

				buttonSendEmailWithAttachment.Clicked += (sender, args) =>
				{
					if (string.IsNullOrEmpty(_path))
					{
						DisplayAlert("Error", "You must first take a pictures.", null, "OK");
					}
					else
					{
						_emailService.ShowDraft(
							"Test Subject",
							"Test Body",
							true,
							string.Empty,
							new List<string> { _path });
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
			if (_mediaPicker != null)
			{
				return;
			}

			_device = Resolver.Resolve<IDevice>();

			////RM: hack for working on windows phone? 
			_mediaPicker = Resolver.Resolve<IMediaPicker>() ?? _device.MediaPicker;
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

			return await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
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

					_path = mediaFile.Path;

					Status = string.Format("Path: {0}", _path);

					return mediaFile;
				}

				return null;
			}, _scheduler);
		}
	}
}

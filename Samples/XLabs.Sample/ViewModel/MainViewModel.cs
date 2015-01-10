namespace XLabs.Sample.ViewModel
{
	using System;
	using System.Collections.ObjectModel;
	using System.Threading.Tasks;

	using Xamarin.Forms;
    
	using Ioc;
	using Platform.Device;
	using Platform.Services;

	/// <summary>
	/// The main view model.
	/// </summary>
	public class MainViewModel : Forms.Mvvm.ViewModel
	{
		private readonly IDevice _device;
		private string _numberToCall = "+1 (855) 926-2746";
		private string _textToSpeak = "Hello from Xamarin Forms Labs";
		private string _deviceTimerInfo = string.Empty;
		private ObservableCollection<string> _images;
		private Command _callCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainViewModel"/> class.
		/// </summary>
		public MainViewModel ()
		{
			SpeakCommand = new Command (() => DependencyService.Get<ITextToSpeechService>().Speak(TextToSpeak));

			Images = new ObservableCollection<string>();
		    for (var i = 0; i < 10; i++)
		    {
		        Images.Add("ad16.jpg");
		    }

		    _device = Resolver.Resolve<IDevice>();
		}

		public Task AddImages()
		{
			return Task.Run(async () => 
			{
				await Task.Delay(1000);
				for (var i = 0; i < 5; i++) 
				{
					Images.Add ("http://www.stockvault.net/data/2011/05/31/124348/small.jpg");
				}
			});
		}

		/// <summary>
		/// The start timer.
		/// </summary>
		public void StartTimer ()
		{
			Device.StartTimer (new TimeSpan (6000), () => {
				DeviceTimerInfo = "This text was updated using the Device Timer";
				return true;
			});
		}

		/// <summary>
		/// Gets the device manufacturer.
		/// </summary>
		/// <value>
		/// The device manufacturer.
		/// </value>
		public string DeviceManufacturer 
        {
			get 
            {
				return string.Format("Device was manufactured by {0}", _device.Manufacturer);
			}
		}

		/// <summary>
		/// Gets the device name.
		/// </summary>
		/// <value>
		/// The device name.
		/// </value>
		public string DeviceName {
			get {
				return string.Format ("Device is called {0}", _device.Name);
			}
		}

		/// <summary>
		/// Gets or sets the number to call.
		/// </summary>
		/// <value>
		/// The number to call.
		/// </value>
		public string NumberToCall {
			get {
				return _numberToCall;
			}
			set {
				SetProperty (ref _numberToCall, value);
			}
		}

		/// <summary>
		/// Gets or sets the text to speak.
		/// </summary>
		/// <value>
		/// The text to speak.
		/// </value>
		public string TextToSpeak {
			get {
				return _textToSpeak;
			}
			set {
				SetProperty (ref _textToSpeak, value);
			}
		}

		private string _deviceUIThreadInfo = string.Empty;

		/// <summary>
		/// Gets or sets the device UI thread info.
		/// </summary>
		/// <value>
		/// The device UI thread info.
		/// </value>
		public string DeviceUIThreadInfo {
			get {
				return _deviceUIThreadInfo;
			}
			set { 
				SetProperty (ref _deviceUIThreadInfo, value);
			}
		}

		/// <summary>
		/// Gets or sets the device timer info.
		/// </summary>
		/// <value>
		/// The device timer info.
		/// </value>
		public string DeviceTimerInfo {
			get {
				return _deviceTimerInfo;
			}
			
			set { 
				SetProperty (ref _deviceTimerInfo, value);
			}
		}
        
		/// <summary>
		/// Gets or sets the demo images.
		/// </summary>
		/// <value>
		/// The images.
		/// </value>
		public ObservableCollection<string> Images {
			get {
				return _images;
			}
			set {
				SetProperty (ref _images, value);
			}
		}

		/// <summary>
		/// Gets the speak command.
		/// </summary>
		/// <value>
		/// The speak command.
		/// </value>
		public Command SpeakCommand { get; private set; }
        
		/// <summary>
		/// Gets the call command.
		/// </summary>
		/// <value>
		/// The call command.
		/// </value>
		public Command CallCommand 
		{
			get 
			{
				return _callCommand ?? (_callCommand = new Command (
					() => _device.PhoneService.DialNumber (NumberToCall),
					() => _device.PhoneService != null)); 
			}
		}
	}
}


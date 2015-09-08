namespace XLabs.Platform.Device
{
    using Windows.System.Profile;

    public class Wp81Device : IDevice
    {

        public string Id
        {
            get { return HardwareIdentification.GetPackageSpecificToken(null).Id.ToString(); }
        }

        public IDisplay Display
        {
            get { throw new System.NotImplementedException(); }
        }

        public Services.IPhoneService PhoneService
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBattery Battery
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAccelerometer Accelerometer
        {
            get { throw new System.NotImplementedException(); }
        }

        public IGyroscope Gyroscope
        {
            get { throw new System.NotImplementedException(); }
        }

        public Services.Media.IMediaPicker MediaPicker
        {
            get { throw new System.NotImplementedException(); }
        }

        public Services.INetwork Network
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBluetoothHub BluetoothHub
        {
            get { throw new System.NotImplementedException(); }
        }

        public Services.Media.IAudioStream Microphone
        {
            get { throw new System.NotImplementedException(); }
        }

        public Services.IO.IFileManager FileManager
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FirmwareVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        public string HardwareVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Manufacturer
        {
            get { throw new System.NotImplementedException(); }
        }

        public long TotalMemory
        {
            get { throw new System.NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new System.NotImplementedException(); }
        }

        public double TimeZoneOffset
        {
            get { throw new System.NotImplementedException(); }
        }

        public string TimeZone
        {
            get { throw new System.NotImplementedException(); }
        }

        public Enums.Orientation Orientation
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Threading.Tasks.Task<bool> LaunchUriAsync(System.Uri uri)
        {
            throw new System.NotImplementedException();
        }
    }
}
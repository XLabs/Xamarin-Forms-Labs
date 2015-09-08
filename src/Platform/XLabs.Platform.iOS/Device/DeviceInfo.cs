namespace XLabs.Platform.Device
{
    /// <summary>
    /// Apple device base information.
    /// </summary>
    public class DeviceInfo
    {
        internal DeviceInfo(DeviceType type, int majorVersion, int minorVersion)
        {
            this.Type = type;
            this.MajorVersion = majorVersion;
            this.MinorVersion = minorVersion;
        }

        /// <summary>
        /// Gets the type of device.
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// Device major version.
        /// </summary>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// Device minor version.
        /// </summary>
        public int MinorVersion { get; private set; }
    }
}
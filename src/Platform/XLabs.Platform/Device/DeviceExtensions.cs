namespace XLabs.Platform.Device
{
    using Enums;

    /// <summary>
    /// <see cref="IDevice"/> extension methods.
    /// </summary>
    public static class DeviceExtensions
    {
        /// <summary>
        /// Determines if this device is in landscape mode.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True if device is in landscape, otherwise false.</returns>
        public static bool IsInLandscape(this IDevice device)
        {
            return (device.Orientation & Orientation.Landscape) == Orientation.Landscape;
        }

        /// <summary>
        /// Determines if this device is in portrait mode.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True if device is in landscape, otherwise false.</returns>
        public static bool IsInPortrait(this IDevice device)
        {
            return (device.Orientation & Orientation.Portrait) == Orientation.Portrait;
        }

        /// <summary>
        /// Convert width in inches to runtime pixels based on the current orientation.
        /// </summary>
        public static double WidthRequestInInches(this IDevice device, double inches)
        {
            return device.IsInLandscape()
                ? device.Display.HeightRequestInInches(inches)
                : device.Display.WidthRequestInInches(inches);
        }

        /// <summary>
        /// Convert height in inches to runtime pixels based on the current orientation.
        /// </summary>
        public static double HeightRequestInInches(this IDevice device, double inches)
        {
            return device.IsInLandscape()
                ? device.Display.WidthRequestInInches(inches)
                : device.Display.HeightRequestInInches(inches);
        }

        /// <summary>
        /// Screen width in inches based on the current orientation.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>System.Double.</returns>
        public static double ScreenWidthInches(this IDevice device)
        {
            return device.IsInLandscape()
                ? device.Display.ScreenHeightInches()
                : device.Display.ScreenWidthInches();
        }

        /// <summary>
        /// Screens height in inches based on the current orientation.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>System.Double.</returns>
        public static double ScreenHeightInches(this IDevice device)
        {
            return device.IsInLandscape()
                ? device.Display.ScreenWidthInches()
                : device.Display.ScreenHeightInches();
        }
    }
}
namespace XLabs.Enums
{
    using System;

    /// <summary>
    /// Enum Orientation
    /// </summary>
    [Flags]
    public enum Orientation
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The portrait
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// The landscape
        /// </summary>
        Landscape = 2,
        /// <summary>
        /// The portrait up
        /// </summary>
        PortraitUp = 5,
        /// <summary>
        /// The portrait down
        /// </summary>
        PortraitDown = 9,
        /// <summary>
        /// The landscape left
        /// </summary>
        LandscapeLeft = 18,
        /// <summary>
        /// The landscape right
        /// </summary>
        LandscapeRight = 34,
    }
}

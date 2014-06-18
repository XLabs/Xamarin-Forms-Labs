
namespace XForms.Toolkit
{
    /// <summary>
    /// Portable interface for device screen information
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        double Xdpi { get; }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        double Ydpi { get; }

        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        double WidthRequestInInches(double inches);

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        double HeightRequestInInches(double inches);
    }
}


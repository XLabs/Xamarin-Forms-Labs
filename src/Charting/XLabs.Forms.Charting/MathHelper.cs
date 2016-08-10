using System;

namespace XLabs.Forms.Charting
{
    /// <summary>
    /// Math helper functions
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <returns>Radians.</returns>
        /// <param name="degrees">Degrees.</param>
        public static double Deg2Rad(double degrees)
        {
            return Math.PI / 180 * degrees;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <returns>Degrees.</returns>
        /// <param name="radians">Radians.</param>
        public static double Rad2Deg(double radians)
        {
            return 180.0 / Math.PI * radians;
        }
    }
}


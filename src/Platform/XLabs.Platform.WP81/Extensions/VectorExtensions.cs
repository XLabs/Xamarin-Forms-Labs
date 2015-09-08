namespace XLabs.Platform
{
    using Windows.Devices.Sensors;

    /// <summary>
    /// Class VectorExtensions.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Ases the vector3.
        /// </summary>
        /// <param name="reading">The reading.</param>
        /// <returns>Vector3.</returns>
        public static Vector3 AsVector3(this AccelerometerReading reading)
        {
            return new Vector3(reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);
        }

        public static Vector3 AsVector3(this GyrometerReading reading)
        {
            return new Vector3(reading.AngularVelocityX, reading.AngularVelocityY, reading.AngularVelocityZ);
        }
    }
}
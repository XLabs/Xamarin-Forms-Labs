namespace XLabs.Forms.Charting.Events
{
	/// <summary>
	/// Class DoubleDrawingData. This class cannot be inherited.
	/// </summary>
	public sealed class DoubleDrawingData
    {
		/// <summary>
		/// Gets or sets the series no.
		/// </summary>
		/// <value>The series no.</value>
		public int SeriesNo { get; set; }
		/// <summary>
		/// Gets or sets the x from.
		/// </summary>
		/// <value>The x from.</value>
		public double XFrom { get; set; }
		/// <summary>
		/// Gets or sets the y from.
		/// </summary>
		/// <value>The y from.</value>
		public double YFrom { get; set; }
		/// <summary>
		/// Gets or sets the x to.
		/// </summary>
		/// <value>The x to.</value>
		public double XTo { get; set; }
		/// <summary>
		/// Gets or sets the y to.
		/// </summary>
		/// <value>The y to.</value>
		public double YTo { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DoubleDrawingData"/> class.
		/// </summary>
		/// <param name="xFrom">The x from.</param>
		/// <param name="yFrom">The y from.</param>
		/// <param name="xTo">The x to.</param>
		/// <param name="yTo">The y to.</param>
		/// <param name="seriesNo">The series no.</param>
		public DoubleDrawingData(double xFrom, double yFrom, double xTo, double yTo, int seriesNo)
        {
            XFrom = xFrom;
            YFrom = yFrom;
            XTo = xTo;
            YTo = yTo;
            SeriesNo = seriesNo;
        }
    }
}

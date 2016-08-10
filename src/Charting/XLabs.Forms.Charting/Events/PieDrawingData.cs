namespace XLabs.Forms.Charting.Events
{
	/// <summary>
	/// Class PieDrawingData. This class cannot be inherited.
	/// </summary>
	public sealed class PieDrawingData
    {
		/// <summary>
		/// Gets or sets the series no.
		/// </summary>
		/// <value>The series no.</value>
		public int SeriesNo { get; set; }
		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>The x.</value>
		public double X { get; set; }
		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>The y.</value>
		public double Y { get; set; }
		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>The size.</value>
		public double Size { get; set; }
		/// <summary>
		/// Gets or sets the percentages.
		/// </summary>
		/// <value>The percentages.</value>
		public double[] Percentages { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PieDrawingData"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="seriesNo">The series no.</param>
		/// <param name="size">The size.</param>
		/// <param name="percentages">The percentages.</param>
		public PieDrawingData(double x, double y, int seriesNo, double size, double[] percentages)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
            Percentages = percentages;
        }
    }
}

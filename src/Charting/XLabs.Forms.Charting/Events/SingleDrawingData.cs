namespace XLabs.Forms.Charting.Events
{
	/// <summary>
	/// Class SingleDrawingData. This class cannot be inherited.
	/// </summary>
	public sealed class SingleDrawingData
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
		/// Initializes a new instance of the <see cref="SingleDrawingData"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="seriesNo">The series no.</param>
		public SingleDrawingData(double x, double y, int seriesNo)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = 5;
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="SingleDrawingData"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="seriesNo">The series no.</param>
		/// <param name="size">The size.</param>
		public SingleDrawingData(float x, float y, int seriesNo, float size)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
        }
    }
}

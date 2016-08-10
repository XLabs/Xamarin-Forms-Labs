namespace XLabs.Forms.Charting.Events
{
	/// <summary>
	/// Class TextDrawingData.
	/// </summary>
	public class TextDrawingData
    {
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
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextDrawingData"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public TextDrawingData(string text, double x, double y)
        {
            Text = text;
            X = x;
            Y = y;
        }
    }
}

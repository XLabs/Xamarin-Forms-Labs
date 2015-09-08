namespace XLabs.Forms.Charting.Events
{
    public class TextDrawingData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Text { get; set; }

        public TextDrawingData(string text, double x, double y)
        {
            Text = text;
            X = x;
            Y = y;
        }
    }
}

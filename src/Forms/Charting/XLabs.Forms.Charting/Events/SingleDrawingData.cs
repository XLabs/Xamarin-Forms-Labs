namespace XLabs.Forms.Charting.Events
{
    public sealed class SingleDrawingData
    {
        public int SeriesNo { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }

        public SingleDrawingData(double x, double y, int seriesNo)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = 5;
        }
        public SingleDrawingData(float x, float y, int seriesNo, float size)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
        }
    }
}

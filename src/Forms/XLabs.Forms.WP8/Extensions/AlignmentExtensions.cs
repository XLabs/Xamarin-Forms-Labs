namespace XLabs.Forms
{
    using System;
    using System.Windows;
    using TextAlignment = Xamarin.Forms.TextAlignment;

    public static class AlignmentExtensions
    {
        public static VerticalAlignment ToContentVerticalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return VerticalAlignment.Center;
                case TextAlignment.End:
                    return VerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return VerticalAlignment.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        public static HorizontalAlignment ToContentHorizontalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return HorizontalAlignment.Center;
                case TextAlignment.End:
                    return HorizontalAlignment.Right;
                case TextAlignment.Start:
                    return HorizontalAlignment.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}
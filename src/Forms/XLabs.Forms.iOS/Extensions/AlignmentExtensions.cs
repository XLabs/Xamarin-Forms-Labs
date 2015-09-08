namespace XLabs.Forms.Extensions
{
    using System;
    using UIKit;
    using Xamarin.Forms;

    public static class AlignmentExtensions
    {
        public static UIControlContentVerticalAlignment ToContentVerticalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentVerticalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentVerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return UIControlContentVerticalAlignment.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        public static UIControlContentHorizontalAlignment ToContentHorizontalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentHorizontalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentHorizontalAlignment.Right;
                case TextAlignment.Start:
                    return UIControlContentHorizontalAlignment.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}
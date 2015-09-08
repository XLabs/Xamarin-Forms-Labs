namespace XLabs.Forms.Extensions
{
    using System;
    using Android.Views;
    using DroidTextAlignment = Android.Views.TextAlignment;
    using TextAlignment = Xamarin.Forms.TextAlignment;

    public static class AlignmentExtensions
    {
        public static DroidTextAlignment ToDroidTextAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return DroidTextAlignment.Center;
                case TextAlignment.End:
                    return DroidTextAlignment.ViewEnd;
                case TextAlignment.Start:
                    return DroidTextAlignment.ViewStart;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        public static GravityFlags ToDroidHorizontalGravity(this TextAlignment alignment)
        {
            switch (alignment)
            {
            case TextAlignment.Center:
                return GravityFlags.CenterHorizontal;
            case TextAlignment.End:
                return GravityFlags.Right;
            case TextAlignment.Start:
                return GravityFlags.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        public static GravityFlags ToDroidVerticalGravity(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return GravityFlags.CenterVertical;
                case TextAlignment.End:
                    return GravityFlags.Bottom;
                case TextAlignment.Start:
                    return GravityFlags.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}
namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using System;

	using Android.Support.V4.View;

	/// <summary>
	/// Class CalendarMonthPageTransformer.
	/// </summary>
	public class CalendarMonthPageTransformer : Java.Lang.Object ,ViewPager.IPageTransformer
	{


		/// <summary>
		/// The mi n_ scale
		/// </summary>
		private const float MIN_SCALE = 0.75f;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarMonthPageTransformer"/> class.
		/// </summary>
		public CalendarMonthPageTransformer()
		{
		}

		#region IPageTransformer implementation

		/// <summary>
		/// Transforms the page.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="position">The position.</param>
		public void TransformPage(Android.Views.View page, float position)
		{
			int pageWidth = page.Width;
			if(position < -1)
			{
				page.Alpha = 0;
			} else if(position <= 0)
			{
				page.Alpha = (1);
				page.TranslationX = 0;
				page.ScaleX = 1;
				page.ScaleY = 1;
			} else if(position <= 1)
			{
				page.Alpha = 1 - position;
				page.TranslationX = (pageWidth * -position);
				float scaleFactor = MIN_SCALE + (1 - MIN_SCALE) * (1 - Math.Abs(position));
				page.ScaleX = (scaleFactor);
				page.ScaleY = (scaleFactor);


			} else
			{
				page.Alpha = 0;
			}

		}

		#endregion

	}
}


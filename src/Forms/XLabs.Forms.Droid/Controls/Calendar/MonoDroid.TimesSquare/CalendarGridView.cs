namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using System;
	using System.Diagnostics;

	using Android.Content;
	using Android.Graphics;
	using Android.Util;
	using Android.Views;

	/// <summary>
	/// Class CalendarGridView.
	/// </summary>
	public class CalendarGridView : ViewGroup
	{
		/// <summary>
		/// The _divider paint
		/// </summary>
		private readonly Paint _dividerPaint = new Paint();

		/// <summary>
		/// Sets the color of the divider.
		/// </summary>
		/// <value>The color of the divider.</value>
		public Android.Graphics.Color DividerColor {
			set {
				_dividerPaint.Color = value;
			}
		}

		/// <summary>
		/// The _old width measure size
		/// </summary>
		private int _oldWidthMeasureSize;
		/// <summary>
		/// The _old number rows
		/// </summary>
		private int _oldNumRows;
		/// <summary>
		/// The float fudge
		/// </summary>
		private static readonly float FloatFudge = 0.5f;
		/// <summary>
		/// The side padding
		/// </summary>
		private static int sidePadding = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarGridView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		public CalendarGridView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			_dividerPaint.Color = base.Resources.GetColor(Resource.Color.calendar_divider);
		}


		/// <summary>
		/// Adds the view.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="index">The index.</param>
		/// <param name="params">The parameters.</param>
		public override void AddView(Android.Views.View child, int index, LayoutParams @params)
		{

			if(ChildCount == 0)
			{
				((CalendarRowView)child).IsHeaderRow = true;
			}
			base.AddView(child, index, @params);
		}

		/// <summary>
		/// Called by draw to draw the child views.
		/// </summary>
		/// <param name="canvas">the canvas on which to draw the view</param>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">Called by draw to draw the child views. This may be overridden
		/// by derived classes to gain control just before its children are drawn
		/// (but after its own view has been drawn).</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#dispatchDraw(android.graphics.Canvas)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void DispatchDraw(Canvas canvas)
		{
			base.DispatchDraw(canvas);
			var row = (ViewGroup)GetChildAt(1);
			int top = row.Top;
			int bottom = Bottom;

			//Left side border.
			int left = row.GetChildAt(0).Left;
			canvas.DrawLine(left + FloatFudge, top, left + FloatFudge, bottom, _dividerPaint);

			//Each cell's right-side border.
			for(int c = 0; c < 7; c++)
			{
				float x = left + row.GetChildAt(c).Right - FloatFudge;
				canvas.DrawLine(x, top, x, bottom, _dividerPaint);
			}
		}

		/// <summary>
		/// Draw one child of this View Group.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw the child</param>
		/// <param name="child">Who to draw</param>
		/// <param name="drawingTime">The time at which draw is occurring</param>
		/// <returns>To be added.</returns>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">Draw one child of this View Group. This method is responsible for getting
		/// the canvas in the right state. This includes clipping, translating so
		/// that the child's scrolled origin is at 0, 0, and applying any animation
		/// transformations.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/ViewGroup.html#drawChild(android.graphics.Canvas, android.view.View, long)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
		{
			bool isInvalidated = base.DrawChild(canvas, child, drawingTime);
			//Draw a bottom border
			int bottom = child.Bottom - 1;
			canvas.DrawLine(child.Left, bottom, child.Right - 2, bottom, _dividerPaint);
			return isInvalidated;
		}

		/// <summary>
		/// The _old height measure size
		/// </summary>
		int _oldHeightMeasureSize;

		/// <summary>
		/// Called when [measure].
		/// </summary>
		/// <param name="widthMeasureSpec">horizontal space requirements as imposed by the parent.
		/// The requirements are encoded with
		/// <c><see cref="T:Android.Views.View+MeasureSpec" /></c>.</param>
		/// <param name="heightMeasureSpec">vertical space requirements as imposed by the parent.
		/// The requirements are encoded with
		/// <c><see cref="T:Android.Views.View+MeasureSpec" /></c>.</param>
		/// <since version="Added in API level 1" />
		/// <altmember cref="P:Android.Views.View.MeasuredWidth" />
		/// <altmember cref="P:Android.Views.View.MeasuredHeight" />
		/// <altmember cref="M:Android.Views.View.SetMeasuredDimension(System.Int32, System.Int32)" />
		/// <altmember cref="M:Android.Views.View.get_SuggestedMinimumHeight" />
		/// <altmember cref="M:Android.Views.View.get_SuggestedMinimumWidth" />
		/// <altmember cref="M:Android.Views.View.MeasureSpec.GetMode(System.Int32)" />
		/// <altmember cref="M:Android.Views.View.MeasureSpec.GetSize(System.Int32)" />
		/// <remarks><para tool="javadoc-to-mdoc" />
		/// <para tool="javadoc-to-mdoc">
		/// Measure the view and its content to determine the measured width and the
		/// measured height. This method is invoked by <c><see cref="M:Android.Views.View.Measure(System.Int32, System.Int32)" /></c> and
		/// should be overriden by subclasses to provide accurate and efficient
		/// measurement of their contents.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <i>CONTRACT:</i> When overriding this method, you
		/// <i>must</i> call <c><see cref="M:Android.Views.View.SetMeasuredDimension(System.Int32, System.Int32)" /></c> to store the
		/// measured width and height of this view. Failure to do so will trigger an
		/// <c>IllegalStateException</c>, thrown by
		/// <c><see cref="M:Android.Views.View.Measure(System.Int32, System.Int32)" /></c>. Calling the superclass'
		/// <c><see cref="M:Android.Views.View.OnMeasure(System.Int32, System.Int32)" /></c> is a valid use.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		/// The base class implementation of measure defaults to the background size,
		/// unless a larger size is allowed by the MeasureSpec. Subclasses should
		/// override <c><see cref="M:Android.Views.View.OnMeasure(System.Int32, System.Int32)" /></c> to provide better measurements of
		/// their content.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		/// If this method is overridden, it is the subclass's responsibility to make
		/// sure the measured height and width are at least the view's minimum height
		/// and width (<c><see cref="M:Android.Views.View.get_SuggestedMinimumHeight" /></c> and
		/// <c><see cref="M:Android.Views.View.get_SuggestedMinimumWidth" /></c>).
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#onMeasure(int, int)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int widthMeasureSize = MeasureSpec.GetSize(widthMeasureSpec);
			/*if (oldWidthMeasureSize == widthMeasureSize) {
				SetMeasuredDimension(MeasuredWidth, MeasuredHeight);
				return;
			}
			oldWidthMeasureSize = widthMeasureSize;*/
			int cellSize = widthMeasureSize / 7;
			// Remove any extra pixels since /7 is unlikely to give whole nums.
			widthMeasureSize = cellSize * 7;
			int totalHeight = 0;
			int rowWidthSpec = MeasureSpec.MakeMeasureSpec(widthMeasureSize, MeasureSpecMode.Exactly);
			int rowHeightSpec = MeasureSpec.MakeMeasureSpec(cellSize, MeasureSpecMode.Exactly);
			for (int c = 0, numChildren = ChildCount; c < numChildren; c++) {
				var child = GetChildAt(c);
				if (child.Visibility == ViewStates.Visible) {
					if (c == 0) { // It's the header: height should be wrap_content.
						MeasureChild(child, rowWidthSpec, MeasureSpec.MakeMeasureSpec(cellSize, MeasureSpecMode.AtMost));
					} else {
						MeasureChild(child, rowWidthSpec, rowHeightSpec);
					}
					totalHeight += child.MeasuredHeight;
				}
			}
			int measuredWidth = widthMeasureSize + 2; // Fudge factor to make the borders show up.
			SetMeasuredDimension(measuredWidth, totalHeight);
		}

		/// <summary>
		/// Called from layout when this view should
		/// assign a size and position to each of its children.
		/// </summary>
		/// <param name="changed">This is a new size or position for this view</param>
		/// <param name="l">Left position, relative to parent</param>
		/// <param name="t">Top position, relative to parent</param>
		/// <param name="r">Right position, relative to parent</param>
		/// <param name="b">Bottom position, relative to parent</param>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">Called from layout when this view should
		/// assign a size and position to each of its children.
		/// Derived classes with children should override
		/// this method and call layout on each of
		/// their children.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/ViewGroup.html#onLayout(boolean, int, int, int, int)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			var stopwatch = Stopwatch.StartNew();
			int heightSoFar = 0;
			t = 0;
			for(int c = 0; c < ChildCount; c++)
			{
				var child = GetChildAt(c);
				int rowHeight = child.MeasuredHeight;
				child.Layout(sidePadding, heightSoFar, r, heightSoFar + rowHeight);
				heightSoFar += rowHeight;
			}

			stopwatch.Stop();
			Logr.D("Grid.OnLayout {0} ms", stopwatch.ElapsedMilliseconds);
		}


		/// <summary>
		/// Sets the number rows.
		/// </summary>
		/// <value>The number rows.</value>
		public int NumRows {
			set {
				if(_oldNumRows != value)
				{
					_oldWidthMeasureSize = 0;
				}
				_oldNumRows = value;
			}
		}
	}
}

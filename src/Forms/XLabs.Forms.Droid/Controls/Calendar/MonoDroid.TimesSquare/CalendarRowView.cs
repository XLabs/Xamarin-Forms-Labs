namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using System.Diagnostics;

	using Android.Content;
	using Android.Util;
	using Android.Views;

	/// <summary>
	/// Class CalendarRowView.
	/// </summary>
	public class CalendarRowView : ViewGroup, Android.Views.View.IOnClickListener
	{
		/// <summary>
		/// Gets or sets a value indicating whether this instance is header row.
		/// </summary>
		/// <value><c>true</c> if this instance is header row; otherwise, <c>false</c>.</value>
		public bool IsHeaderRow { get; set; }

		/// <summary>
		/// The click handler
		/// </summary>
		public ClickHandler ClickHandler;
		/// <summary>
		/// The _cell size
		/// </summary>
		private int _cellSize;

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarRowView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		public CalendarRowView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		/// <summary>
		/// Adds the view.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="index">The index.</param>
		/// <param name="params">The parameters.</param>
		public override void AddView(Android.Views.View child, int index, LayoutParams @params)
		{
			child.SetOnClickListener(this);
			base.AddView(child, index, @params);
		}

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
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			int totalWidth = MeasureSpec.GetSize(widthMeasureSpec);
			int height = MeasureSpec.GetSize(heightMeasureSpec);
			_cellSize = totalWidth / 7;
			int cellWidthSpec = MeasureSpec.MakeMeasureSpec(_cellSize, MeasureSpecMode.Exactly);
			int cellHeightSpec = IsHeaderRow
				? MeasureSpec.MakeMeasureSpec(_cellSize, MeasureSpecMode.AtMost)
				: cellWidthSpec;
			int rowHeight = 0;
			for(int c = 0; c < ChildCount; c++)
			{
				var child = GetChildAt(c);
				child.Measure(cellWidthSpec, cellHeightSpec);
				//The row height is the height of the tallest cell.
				if(child.MeasuredHeight > rowHeight)
				{
					rowHeight = child.MeasuredHeight;
				}
			}
			int widthWithPadding = totalWidth + PaddingLeft + PaddingRight;
			int heightWithPadding = rowHeight + PaddingTop + PaddingBottom;
			SetMeasuredDimension(widthWithPadding, heightWithPadding);

			stopwatch.Stop();
			Logr.D("Row.OnMeasure {0} ms", stopwatch.ElapsedMilliseconds);
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
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			int cellHeight = b - t;
			for(int c = 0; c < ChildCount; c++)
			{
				var child = GetChildAt(c);
				child.Layout(c * _cellSize, 0, (c + 1) * _cellSize, cellHeight);
			}

			stopwatch.Stop();
			Logr.D("Row.OnLayout {0} ms", stopwatch.ElapsedMilliseconds);
		}

		/// <summary>
		/// Called when [click].
		/// </summary>
		/// <param name="v">The v.</param>
		public void OnClick(Android.Views.View v)
		{
			if(ClickHandler != null)
			{
				ClickHandler((MonthCellDescriptor)v.Tag);
			}
		}
	}
}
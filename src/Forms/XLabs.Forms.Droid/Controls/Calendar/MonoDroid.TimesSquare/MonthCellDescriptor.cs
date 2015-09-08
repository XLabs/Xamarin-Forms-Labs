namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using System;

	public enum RangeState
	{
		None,
		First,
		Middle,
		Last
	}

	/// <summary>
	/// Class MonthCellDescriptor.
	/// </summary>
	public class MonthCellDescriptor : Java.Lang.Object
	{
		/// <summary>
		/// Gets or sets the date time.
		/// </summary>
		/// <value>The date time.</value>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Value { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is current month.
		/// </summary>
		/// <value><c>true</c> if this instance is current month; otherwise, <c>false</c>.</value>
		public bool IsCurrentMonth { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is today.
		/// </summary>
		/// <value><c>true</c> if this instance is today; otherwise, <c>false</c>.</value>
		public bool IsToday { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selectable.
		/// </summary>
		/// <value><c>true</c> if this instance is selectable; otherwise, <c>false</c>.</value>
		public bool IsSelectable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is highlighted.
		/// </summary>
		/// <value><c>true</c> if this instance is highlighted; otherwise, <c>false</c>.</value>
		public bool IsHighlighted { get; set; }

		/// <summary>
		/// Gets or sets the state of the range.
		/// </summary>
		/// <value>The state of the range.</value>
		public RangeState RangeState { get; set; }

		/// <summary>
		/// Gets or sets the style.
		/// </summary>
		/// <value>The style.</value>
		public StyleDescriptor Style{ get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MonthCellDescriptor"/> class.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="isCurrentMonth">if set to <c>true</c> [is current month].</param>
		/// <param name="isSelectable">if set to <c>true</c> [is selectable].</param>
		/// <param name="isSelected">if set to <c>true</c> [is selected].</param>
		/// <param name="isToday">if set to <c>true</c> [is today].</param>
		/// <param name="isHighlighted">if set to <c>true</c> [is highlighted].</param>
		/// <param name="value">The value.</param>
		/// <param name="rangeState">State of the range.</param>
		/// <param name="style">The style.</param>
		public MonthCellDescriptor(DateTime date, bool isCurrentMonth, bool isSelectable, bool isSelected,
								   bool isToday, bool isHighlighted, int value, RangeState rangeState, StyleDescriptor style)
		{
			DateTime = date;
			Value = value;
			IsCurrentMonth = isCurrentMonth;
			IsSelected = isSelected;
			IsHighlighted = isHighlighted;
			IsToday = isToday;
			IsSelectable = isSelectable;
			RangeState = rangeState;
			Style = style;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">Returns a string containing a concise, human-readable description of this
		/// object. Subclasses are encouraged to override this method and provide an
		/// implementation that takes into account the object's type and data. The
		/// default implementation is equivalent to the following expression:
		/// <example><code lang="java">
		/// getClass().getName() + '@' + Integer.toHexString(hashCode())</code></example></para>
		/// <para tool="javadoc-to-mdoc">See <see cref="!:Java.Lang.Object.writing_toString" />
		/// if you intend implementing your own <c>toString</c> method.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/java/lang/Object.html#toString()" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		public override string ToString()
		{
			return "MonthCellDescriptor{"
			+ "Date=" + DateTime
			+ ", Value=" + Value
			+ ", IsCurrentMonth=" + IsCurrentMonth
			+ ", IsSelected=" + IsSelected
			+ ", IsToday=" + IsToday
			+ ", IsSelectable=" + IsSelectable
			+ ", IsHighlighted=" + IsHighlighted
			+ ", RangeSTate=" + RangeState
			+ "}";
		}
	}
}
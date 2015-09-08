namespace XLabs.Forms.Charting.Controls
{
	using System;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Collection of Series. The collection represents all data necessary to draw a single chart.
	/// </summary>
	public class SeriesCollection : Collection<Series>
	{
		/// <summary>
		/// Add a Series to the collection.
		/// </summary>
		/// <param name="series">Series to add.</param>
		public new void Add(Series series)
		{
			base.Add(series);
		}

		/// <summary>
		/// Remove a Series from the collection.
		/// </summary>
		/// <param name="series">Series to be removed.</param>
		public new void Remove(Series series)
		{
			base.Remove(series);
		}

		/// <summary>
		/// Remove a Series from the collection.
		/// </summary>
		/// <param name="index">Index of collection to remove Series at.</param>
		public void Remove(int index)
		{
			if (index > Count - 1 || index < 0)
			{
				throw new IndexOutOfRangeException("You tried to remove a series at an index which is invalid!");
			}
			else
			{
				base.RemoveAt(index);
			}
		}

		/// <summary>
		/// Retrieve or put a Series at the index in the collection.
		/// </summary>
		/// <param name="index">Index to look for in the collection.</param>
		/// <returns>Series found at the specified index.</returns>
		public new Series this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
			}
		}
	}
}

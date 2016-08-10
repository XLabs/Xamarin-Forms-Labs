namespace XLabs.Forms.Charting.Controls
{
	using System;
	using System.Collections.ObjectModel;

	/// <summary>
    /// Collection of DataPoints. The collection represents all data necessary to draw a single series.
    /// </summary>
    public class DataPointCollection : Collection<DataPoint>
    {
        /// <summary>
        /// Add a DataPoint to the collection.
        /// </summary>
        /// <param name="dataPoint">DataPoint to add.</param>
        public new void Add(DataPoint dataPoint)
        {
            base.Add(dataPoint);
        }

        /// <summary>
        /// Remove a DataPoint from the collection.
        /// </summary>
        /// <param name="dataPoint">DataPoint to be removed.</param>
        public new void Remove(DataPoint dataPoint)
        {
            base.Remove(dataPoint);
        }

        /// <summary>
        /// Remove a DataPoint from the collection.
        /// </summary>
        /// <param name="index">Index of collection to remove DataPoint at.</param>
        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("You tried to remove a datapoint at an index which is invalid!");
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        /// Retrieve or put a DataPoint at the index in the collection.
        /// </summary>
        /// <param name="index">Index to look for in the collection.</param>
        /// <returns>DataPoint found at the specified index.</returns>
        public new DataPoint this[int index]
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

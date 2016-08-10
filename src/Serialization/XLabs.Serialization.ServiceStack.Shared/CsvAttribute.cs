// ***********************************************************************
// <copyright file="CsvAttribute.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace ServiceStack.Text
{
	/// <summary>
	/// Enum CsvBehavior
	/// </summary>
	public enum CsvBehavior
    {
		/// <summary>
		/// The first enumerable
		/// </summary>
		FirstEnumerable
	}

	/// <summary>
	/// Class CsvAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class CsvAttribute : Attribute
    {
		/// <summary>
		/// Gets or sets the CSV behavior.
		/// </summary>
		/// <value>The CSV behavior.</value>
		public CsvBehavior CsvBehavior { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="CsvAttribute"/> class.
		/// </summary>
		/// <param name="csvBehavior">The CSV behavior.</param>
		public CsvAttribute(CsvBehavior csvBehavior)
        {
            this.CsvBehavior = csvBehavior;
        }
    }
}
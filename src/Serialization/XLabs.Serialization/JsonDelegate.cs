// ***********************************************************************
// Assembly         : XLabs.Serialization
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="JsonDelegate.cs" company="">
//     Copyright © XLabs 2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace XLabs.Serialization
{
	/// <summary>
	/// Class JsonDelegate.
	/// </summary>
	public class JsonDelegate : IJsonConvert
    {
		/// <summary>
		/// The function
		/// </summary>
		private Func<object, string> func;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonDelegate"/> class.
		/// </summary>
		/// <param name="func">The function.</param>
		public JsonDelegate(Func<object, string> func)
        {
            this.func = func;
        }

		#region IJsonConvert Members

		/// <summary>
		/// To the json.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>System.String.</returns>
		public string ToJson(object obj)
        {
            return this.func(obj);
        }

        #endregion
    }
}

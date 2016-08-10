// ***********************************************************************
// <copyright file="JsvDeserializeType.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Reflection;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Jsv
{
	/// <summary>
	/// Class JsvDeserializeType.
	/// </summary>
	public static class JsvDeserializeType
	{
		/// <summary>
		/// Gets the set property method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyInfo">The property information.</param>
		/// <returns>SetPropertyDelegate.</returns>
		public static SetPropertyDelegate GetSetPropertyMethod(Type type, PropertyInfo propertyInfo)
		{
			return TypeAccessor.GetSetPropertyMethod(type, propertyInfo);
		}

		/// <summary>
		/// Gets the set field method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="fieldInfo">The field information.</param>
		/// <returns>SetPropertyDelegate.</returns>
		public static SetPropertyDelegate GetSetFieldMethod(Type type, FieldInfo fieldInfo)
		{
			return TypeAccessor.GetSetFieldMethod(type, fieldInfo);
		}
	}
}
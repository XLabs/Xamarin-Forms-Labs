//
// https://github.com/ServiceStack/ServiceStack.Text
// ServiceStack.Text: .NET C# POCO JSON, JSV and CSV Text Serializers.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2012 ServiceStack Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//
using System;
using System.Collections.Generic;
using System.IO;


namespace ServiceStack.Text.Common
{
	/// <summary>
	/// Delegate WriteListDelegate
	/// </summary>
	/// <param name="writer">The writer.</param>
	/// <param name="oList">The o list.</param>
	/// <param name="toStringFn">To string function.</param>
	internal delegate void WriteListDelegate(TextWriter writer, object oList, WriteObjectDelegate toStringFn);

	/// <summary>
	/// Delegate WriteGenericListDelegate
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="writer">The writer.</param>
	/// <param name="list">The list.</param>
	/// <param name="toStringFn">To string function.</param>
	internal delegate void WriteGenericListDelegate<T>(TextWriter writer, IList<T> list, WriteObjectDelegate toStringFn);

	/// <summary>
	/// Delegate WriteDelegate
	/// </summary>
	/// <param name="writer">The writer.</param>
	/// <param name="value">The value.</param>
	internal delegate void WriteDelegate(TextWriter writer, object value);

	/// <summary>
	/// Delegate ParseFactoryDelegate
	/// </summary>
	/// <returns>ParseStringDelegate.</returns>
	internal delegate ParseStringDelegate ParseFactoryDelegate();

	/// <summary>
	/// Delegate WriteObjectDelegate
	/// </summary>
	/// <param name="writer">The writer.</param>
	/// <param name="obj">The object.</param>
	public delegate void WriteObjectDelegate(TextWriter writer, object obj);

	/// <summary>
	/// Delegate SetPropertyDelegate
	/// </summary>
	/// <param name="instance">The instance.</param>
	/// <param name="propertyValue">The property value.</param>
	public delegate void SetPropertyDelegate(object instance, object propertyValue);

	/// <summary>
	/// Delegate ParseStringDelegate
	/// </summary>
	/// <param name="stringValue">The string value.</param>
	/// <returns>System.Object.</returns>
	public delegate object ParseStringDelegate(string stringValue);

	/// <summary>
	/// Delegate ConvertObjectDelegate
	/// </summary>
	/// <param name="fromObject">From object.</param>
	/// <returns>System.Object.</returns>
	public delegate object ConvertObjectDelegate(object fromObject);

	/// <summary>
	/// Delegate ConvertInstanceDelegate
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="type">The type.</param>
	/// <returns>System.Object.</returns>
	public delegate object ConvertInstanceDelegate(object obj, Type type);
}

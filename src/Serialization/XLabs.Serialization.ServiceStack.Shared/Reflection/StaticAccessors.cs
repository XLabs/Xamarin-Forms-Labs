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
using System.Reflection;

#if !XBOX
using System.Linq.Expressions;
#endif

namespace ServiceStack.Text.Reflection
{
	/// <summary>
	/// Class StaticAccessors.
	/// </summary>
	public static class StaticAccessors
    {
		/// <summary>
		/// Gets the value getter.
		/// </summary>
		/// <param name="propertyInfo">The property information.</param>
		/// <param name="type">The type.</param>
		/// <returns>Func&lt;System.Object, System.Object&gt;.</returns>
		public static Func<object, object> GetValueGetter(this PropertyInfo propertyInfo, Type type)
        {
#if NETFX_CORE
			var getMethodInfo = propertyInfo.GetMethod;
			if (getMethodInfo == null) return null;
			return x => getMethodInfo.Invoke(x, new object[0]);
#elif (SILVERLIGHT && !WINDOWS_PHONE) || MONOTOUCH || XBOX
			var getMethodInfo = propertyInfo.GetGetMethod();
			if (getMethodInfo == null) return null;
			return x => getMethodInfo.Invoke(x, new object[0]);
#else

            var instance = Expression.Parameter(typeof(object), "i");
            var convertInstance = Expression.TypeAs(instance, propertyInfo.DeclaringType);
            var property = Expression.Property(convertInstance, propertyInfo);
            var convertProperty = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertProperty, instance).Compile();
#endif
        }

		/// <summary>
		/// Gets the value getter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyInfo">The property information.</param>
		/// <returns>Func&lt;T, System.Object&gt;.</returns>
		public static Func<T, object> GetValueGetter<T>(this PropertyInfo propertyInfo)
        {
#if NETFX_CORE
			var getMethodInfo = propertyInfo.GetMethod;
            if (getMethodInfo == null) return null;
			return x => getMethodInfo.Invoke(x, new object[0]);
#elif (SILVERLIGHT && !WINDOWS_PHONE) || MONOTOUCH || XBOX
			var getMethodInfo = propertyInfo.GetGetMethod();
			if (getMethodInfo == null) return null;
			return x => getMethodInfo.Invoke(x, new object[0]);
#else
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);
            var convert = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(convert, instance).Compile();
#endif
        }

		/// <summary>
		/// Gets the value getter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fieldInfo">The field information.</param>
		/// <returns>Func&lt;T, System.Object&gt;.</returns>
		public static Func<T, object> GetValueGetter<T>(this FieldInfo fieldInfo)
        {
#if (SILVERLIGHT && !WINDOWS_PHONE) || MONOTOUCH || XBOX
            return x => fieldInfo.GetValue(x);
#else
            var instance = Expression.Parameter(fieldInfo.DeclaringType, "i");
            var property = Expression.Field(instance, fieldInfo);
            var convert = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(convert, instance).Compile();
#endif
        }

#if !XBOX
		/// <summary>
		/// Gets the value setter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyInfo">The property information.</param>
		/// <returns>Action&lt;T, System.Object&gt;.</returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static Action<T, object> GetValueSetter<T>(this PropertyInfo propertyInfo)
        {
            if (typeof(T) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(
                instance,
                propertyInfo.SetMethod(),
                Expression.Convert(argument, propertyInfo.PropertyType));

            return Expression.Lambda<Action<T, object>>
            (
                setterCall, instance, argument
            ).Compile();
        }
#endif

    }
}


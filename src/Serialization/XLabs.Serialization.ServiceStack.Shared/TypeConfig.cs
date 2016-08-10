// ***********************************************************************
// <copyright file="TypeConfig.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class TypeConfig.
	/// </summary>
	internal class TypeConfig
    {
		/// <summary>
		/// The type
		/// </summary>
		internal readonly Type Type;
		/// <summary>
		/// The enable anonymous field setterses
		/// </summary>
		internal bool EnableAnonymousFieldSetterses;
		/// <summary>
		/// The properties
		/// </summary>
		internal PropertyInfo[] Properties;
		/// <summary>
		/// The fields
		/// </summary>
		internal FieldInfo[] Fields;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeConfig"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		internal TypeConfig(Type type)
        {
            Type = type;
            EnableAnonymousFieldSetterses = false;
            Properties = new PropertyInfo[0];
            Fields = new FieldInfo[0];
        }
    }

	/// <summary>
	/// Class TypeConfig.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class TypeConfig<T>
    {
		/// <summary>
		/// The configuration
		/// </summary>
		private static readonly TypeConfig config;

		/// <summary>
		/// Gets or sets the properties.
		/// </summary>
		/// <value>The properties.</value>
		public static PropertyInfo[] Properties
        {
            get { return config.Properties; }
            set { config.Properties = value; }
        }

		/// <summary>
		/// Gets or sets the fields.
		/// </summary>
		/// <value>The fields.</value>
		public static FieldInfo[] Fields
        {
            get { return config.Fields; }
            set { config.Fields = value; }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [enable anonymous field setters].
		/// </summary>
		/// <value><c>true</c> if [enable anonymous field setters]; otherwise, <c>false</c>.</value>
		public static bool EnableAnonymousFieldSetters
        {
            get { return config.EnableAnonymousFieldSetterses; }
            set { config.EnableAnonymousFieldSetterses = value; }
        }

		/// <summary>
		/// Initializes static members of the <see cref="TypeConfig{T}"/> class.
		/// </summary>
		static TypeConfig()
        {
            config = new TypeConfig(typeof(T));
            
            var excludedProperties = JsConfig<T>.ExcludePropertyNames ?? new string[0];

            var properties = excludedProperties.Any()
                ? config.Type.GetSerializableProperties().Where(x => !excludedProperties.Contains(x.Name))
                : config.Type.GetSerializableProperties();
            Properties = properties.Where(x => x.GetIndexParameters().Length == 0).ToArray();

            Fields = config.Type.GetSerializableFields().ToArray();
        }

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns>TypeConfig.</returns>
		internal static TypeConfig GetState()
        {
            return config;
        }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace XLabs.Serialization.JsonNET
{
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// JSON serializer using Newtonsoft.Json library.
    /// </summary>
    /// <remarks>
    /// 
    /// Newtonsoft.Json copyright information.
    /// 
    /// The MIT License (MIT)
    /// Copyright (c) 2007 James Newton-King
    /// 
    /// https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md
    /// 
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1644:DocumentationHeadersMustNotContainBlankLines", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class JsonSerializer : StringSerializer, IJsonSerializer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializer"/> class.
        /// </summary>
        public JsonSerializer()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public JsonSerializer(JsonSerializerSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializer"/> class.
        /// </summary>
        /// <param name="typeNameHandling">The type name handling.</param>
        /// <param name="referenceLoopHandling">The reference loop handling.</param>
        /// <param name="ignoreNulls">if set to <c>true</c> [ignore nulls].</param>
        /// <param name="contractResolver">The contract resolver.</param>
        public JsonSerializer(TypeNameHandling typeNameHandling, ReferenceLoopHandling referenceLoopHandling, bool ignoreNulls = true, IContractResolver contractResolver = null)
        {
            this.Settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = typeNameHandling,
                    ReferenceLoopHandling = referenceLoopHandling,
                    NullValueHandling = ignoreNulls ? NullValueHandling.Ignore : NullValueHandling.Include,
                    ContractResolver = contractResolver
                };
        }

		/// <summary>
		/// Gets or sets the settings.
		/// </summary>
		/// <value>The settings.</value>
		public JsonSerializerSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the serialization format.
        /// </summary>
        /// <value>Serialization format.</value>
        public override SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        /// <summary>
        /// Cleans memory.
        /// </summary>
        public override void Flush()
        {
#if DEBUG
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized string of the object.</returns>
        public override string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, this.Settings);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="data">Serialized object.</param>
        /// <returns>Object of type T.</returns>
        public override T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, this.Settings);
        }

		/// <summary>
		/// Deserializes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public override object Deserialize(string data, Type type)
        {
            return JsonConvert.DeserializeObject(data, type, this.Settings);
        }
    }
}

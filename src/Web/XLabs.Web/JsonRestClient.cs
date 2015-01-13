using System;
using System.Net.Http;
using XLabs.Serialization;

namespace XLabs.Web
{
    /// <summary>
    /// The JSON rest client.
    /// </summary>
    public class JsonRestClient : RestCoreClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRestClient"/> class.
        /// </summary>
        /// <param name="serializer">
        /// The serializer.
        /// </param>
        /// <param name="httpClient">
        /// The http client.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if the serializer does not support JSON
        /// </exception>
        public JsonRestClient(IJsonSerializer serializer, HttpClient httpClient = null)
            : base(serializer, httpClient)
        {
            if (serializer.Format != SerializationFormat.Json)
            {
                throw new Exception(string.Format("Invalid serializer type: {0}. Valid type is: {1}", serializer.Format, SerializationFormat.Json));
            }
        }

        /// <summary>
        /// Gets the string content type.
        /// </summary>
        protected override string StringContentType
        {
            get { return "application/json"; }
        }
    }
}


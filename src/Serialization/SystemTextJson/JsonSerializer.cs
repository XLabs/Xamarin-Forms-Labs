namespace XLabs.Serialization.SystemTextJson
{
    using System.Text.Json;

    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonParser deserializer;
        private readonly IJsonSerializer serializer;

        public JsonSerializer(IJsonSerializer serializer)
        {
            this.deserializer = new JsonParser();
            this.serializer = serializer;
        }

        public SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        public void Flush()
        {
            this.serializer.Flush();
        }

        public byte[] SerializeToBytes<T>(T obj)
        {
            return this.serializer.SerializeToBytes(obj);
        }

        public T Deserialize<T>(byte[] data)
        {
            return this.DeserializeFromBytes<T>(data);
        }

        public object Deserialize(byte[] data, System.Type type)
        {
            return this.DeserializeFromBytes(data, type);
        }

        public void Serialize<T>(T obj, System.IO.Stream stream)
        {
            this.serializer.Serialize(obj, stream);
        }

        public T Deserialize<T>(System.IO.Stream stream)
        {
            return this.deserializer.Parse<T>(stream);
        }

        public object Deserialize(System.IO.Stream stream, System.Type type)
        {
            return this.deserializer.Parse(stream);
        }

        public string Serialize<T>(T obj)
        {
            return this.serializer.Serialize(obj);
        }

        public T Deserialize<T>(string data)
        {
            return this.deserializer.Parse<T>(data);
        }

        public object Deserialize(string data, System.Type type)
        {
            return this.deserializer.Parse(data);
        }
    }
}
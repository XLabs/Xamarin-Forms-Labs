using System;
using System.IO;
using MsgPack.Serialization;

namespace XLabs.Serialization.MsgPack
{
    public class MsgPackSerializer : IMsgPackSerializer
    {
        private readonly SerializationContext serializer;

        public MsgPackSerializer(SerializationContext serializer)
        {
            this.serializer = serializer;
            //this.serializer.Serializers.
        }

        // removed because the platform specific projects need to reference MsgPack.CLI in order to work
        //public MsgPackSerializer() : this(SerializationContext.Default)
        //{

        //}

        public byte[] SerializeToBytes<T>(T obj)
        {
            return this.serializer.GetSerializer<T>().PackSingleObject(obj);
        }

        public T Deserialize<T>(byte[] data)
        {
            return this.serializer.GetSerializer<T>().UnpackSingleObject(data);
            //return this.DeserializeFromBytes<T>(data);
        }

        public object Deserialize(byte[] data, Type type)
        {
            return this.serializer.GetSerializer(type).UnpackSingleObject(data);
            //return this.DeserializeFromBytes(data, type);
        }

        public void Serialize<T>(T obj, Stream stream)
        {
            this.serializer.GetSerializer<T>().Pack(stream, obj);
        }

        public T Deserialize<T>(Stream stream)
        {
            return this.serializer.GetSerializer<T>().Unpack(stream);
        }

        public object Deserialize(Stream stream, Type type)
        {
            return this.serializer.GetSerializer(type).Unpack(stream);
        }

        public string Serialize<T>(T obj)
        {
            return this.SerializeToString(obj);
        }

        public T Deserialize<T>(string data)
        {
            return this.DeserializeFromString<T>(data);
        }

        public object Deserialize(string data, Type type)
        {
            return this.DeserializeFromString(data, type);
        }

        public void Flush()
        {
            //todo: see if there is a way to clear the serializers
        }

        public SerializationFormat Format => SerializationFormat.MsgPack;
    }
}
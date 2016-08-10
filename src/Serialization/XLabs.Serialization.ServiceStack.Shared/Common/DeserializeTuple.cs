namespace XLabs.Serialization.ServiceStack.Common
{
    using System;
    using System.Linq;
    using global::ServiceStack.Text.Common;
    using global::ServiceStack.Text.Json;

    internal static class DeserializeTuple<TSerializer> where TSerializer : ITypeSerializer
    {
        private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

        public static object Parse(Type tupleType, string value)
        {
            var index = 0;
            Serializer.EatMapStartChar(value, ref index);
            if (JsonTypeSerializer.IsEmptyMap(value, index))
            {
                return Activator.CreateInstance(tupleType);
            }

            var genericArgs = tupleType.GetGenericArguments();
            var argValues = new object[genericArgs.Length];
            var valueLength = value.Length;
            while (index < valueLength)
            {
                var keyValue = Serializer.EatMapKey(value, ref index);
                Serializer.EatMapKeySeperator(value, ref index);
                var elementValue = Serializer.EatValue(value, ref index);
                if (keyValue == null) continue;

                var keyIndex = int.Parse(keyValue.Substring(4)) - 1;
                argValues[keyIndex] = Serializer.GetParseFn(genericArgs[keyIndex]).Invoke(elementValue);

                Serializer.EatItemSeperatorOrMapEndChar(value, ref index);
            }

            return tupleType.GetConstructors().First(x => x.GetParameters().Length == genericArgs.Length).Invoke(argValues);
        }
    }
}
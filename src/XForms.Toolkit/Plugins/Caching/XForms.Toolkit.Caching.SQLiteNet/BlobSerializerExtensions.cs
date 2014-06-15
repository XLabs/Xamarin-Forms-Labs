using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using XForms.Toolkit.Services.Serialization;

namespace XForms.Toolkit.Caching.SQLiteNet
{
    public static class BlobSerializerExtensions
    {
        public static IBlobSerializer AsBlobSerializer(this IByteSerializer serializer)
        {
            var gm = typeof(IByteSerializer).GetTypeInfo().GetDeclaredMethod("Deserialize");

            return new BlobSerializerDelegate(
                obj => serializer.Serialize(obj),
                (data, type) => gm.MakeGenericMethod(type).Invoke(serializer, new[] { data }),
                serializer.CanDeserialize);
        }

        private static bool CanDeserialize(this IByteSerializer serializer, Type type)
        {
            return true;
        }
    }
}

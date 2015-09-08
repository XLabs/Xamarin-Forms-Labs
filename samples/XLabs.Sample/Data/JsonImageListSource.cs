using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using XLabs.Serialization;

namespace XLabs.Sample.Data
{
    public class JsonImageListSource : IImageListSource
    {
        private readonly IJsonSerializer serializer;

        public JsonImageListSource(IJsonSerializer serializer)
        {
            this.serializer = serializer;
        }
        public async Task<List<ImageListItem>> GetListItems()
        {
            var assembly = typeof(JsonImageListSource).GetTypeInfo().Assembly;
            using (var reader = 
                new StreamReader(assembly.GetManifestResourceStream("XLabs.Sample.Data.PresidentList.json"))
                )
            {
                var str = await reader.ReadToEndAsync();
                return this.serializer.Deserialize<List<ImageListItem>>(str);
            }
        }
    }
}
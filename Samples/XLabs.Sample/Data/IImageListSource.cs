using System.Collections.Generic;
using System.Threading.Tasks;

namespace XLabs.Sample.Data
{
    public interface IImageListSource
    {
        Task<List<ImageListItem>> GetListItems();
    }
}
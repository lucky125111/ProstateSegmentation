using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface ISegmentationProvider
    {
        Task<IEnumerable<byte[]>> CalculateSegmentationAsync(string base64);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public class SegmentationProvider : ISegmentationProvider
    {
        public Task<IEnumerable<byte[]>> CalculateSegmentationAsync(string base64)
        {
            return null;
        }
    }
}
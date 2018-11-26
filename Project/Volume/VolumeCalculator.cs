using System.Threading.Tasks;
using Core.Model;

namespace Volume
{
    public class VolumeCalculator : IVolumeCalculator
    {
        public async Task<double> CalculateVolume(VolumeDataModel dataModel)
        {
            return await Task.FromResult(0);
        }
    }
}
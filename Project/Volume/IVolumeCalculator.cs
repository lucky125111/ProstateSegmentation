using System.Threading.Tasks;
using Core.Model;

namespace Volume
{
    public interface IVolumeCalculator
    {
        Task<double> CalculateVolume(VolumeDataModel dataModel);
    }
}
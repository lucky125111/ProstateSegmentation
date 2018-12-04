using System.Collections.Generic;
using Application.Models;

namespace VolumeService.Core.VolumeCalculator
{
    public interface IVolumeCalculator
    {
        double CalculateVolume(IEnumerable<byte[]> dicomId, ImageInformation imageInformation,
            ImageFitterType fitterType);
    }
}
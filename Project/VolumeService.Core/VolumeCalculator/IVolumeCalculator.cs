using System;
using System.Collections.Generic;
using Application.Models;
using VolumeService.Core.Fitter;

namespace VolumeService.Core.VolumeCalculator
{
    public interface IVolumeCalculator
    {
        double CalculateVolume(IEnumerable<byte[]> dicomId, ImageInformation imageInformation, ImageFitterType fitterType);
    }
}
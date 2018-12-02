using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IVolumeService
    {
        double GetVolume(int dicomId);
        double CalculateVolume(int dicomId);
        double CalculateVolume(IEnumerable<byte[]> dicomId, ImageInformation imageInformation);
    }
}
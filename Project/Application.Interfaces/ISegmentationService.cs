using System;

namespace Application.Interfaces
{
    public interface ISegmentationService : IDisposable
    {
        byte[] Calculate(int dicomId, int sliceId);
        byte[] Calculate(byte[] image);
    }
}
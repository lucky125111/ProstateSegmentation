using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISegmentationService : IDisposable
    {
        void Calculate(int dicomId, int sliceId);
        byte[] Calculate(byte[] image);
        void Calculate(int dicomId);
    }
}
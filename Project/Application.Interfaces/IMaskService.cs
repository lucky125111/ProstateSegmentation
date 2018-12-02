using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IMaskService : IDisposable
    {
        IEnumerable<MaskModel> GetAll(int dicomId);
        MaskModel GetMask(int dicomId, int sliceId);
        void UpdateMask(int dicomId, int sliceId, MaskModel value);
        void RemoveMask(int dicomId, int sliceId);
    }
}
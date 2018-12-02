using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface ISliceService : IDisposable
    {
        IEnumerable<SliceModel> GetAllSlices(int dicomId);
        SliceModel GetSlice(int dicomId, int sliceId);
        int AddNewSlice(int id, SliceModel value);
        void UpdateSlice(int dicomId, int sliceId, SliceModel value);
        void RemoveImage(int dicomId, int sliceId);
    }
}
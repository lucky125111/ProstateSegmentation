using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface ISliceService
    {
        IEnumerable<SliceModel> GetAllSlices(int dicomId);
        SliceModel GetSlice(int dicomId, int sliceId);
        void AddNewSlice(int id, SliceModel value);
        void UpdateSlice(int dicomId, int sliceId, SliceModel value);
    }
}
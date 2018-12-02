using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IMaskService
    {
        IEnumerable<MaskModel> GetAll(int dicomId);
        MaskModel GetMask(int dicomId, int sliceId);
        void UpdateMask(int dicomId, int sliceId, MaskModel value);
    }
}
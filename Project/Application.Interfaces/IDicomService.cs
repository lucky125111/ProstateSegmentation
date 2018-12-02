using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IDicomService : IDisposable
    {
        IEnumerable<DicomModel> GetAllDicoms();
        DicomModel GetDicomById(int id);
        int AddDicom(DicomModel value);
        void UpdateDicom(int id, DicomModel value);
        void DeleteDicom(int id);
        IEnumerable<int> GetImageIndexes(int id);
    }
}
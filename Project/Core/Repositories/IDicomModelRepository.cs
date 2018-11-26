using System;
using Core.Entity;

namespace Core.Repositories
{
    public interface IDicomModelRepository : IDisposable
    {
        DicomModel GetDicomModelById(int patientId);
        void InsertDicom(DicomModel dicomModel);
        void Save();
    }
}
using System;
using Core.Context;
using Core.Entity;

namespace Core.Repositories
{
    public class DicomModelRepository : IDicomModelRepository
    {
        private readonly DicomContext _dicomContext;

        private bool _disposed;

        public DicomModelRepository(DicomContext dicomContext)
        {
            _dicomContext = dicomContext;
        }

        public DicomModel GetDicomModelById(int patientId)
        {
            return _dicomContext.DicomModels.Find(patientId);
        }

        public void InsertDicom(DicomModel dicomModel)
        {
            _dicomContext.DicomModels.Add(dicomModel);
        }

        public void Save()
        {
            _dicomContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _dicomContext.Dispose();
            _disposed = true;
        }
    }
}
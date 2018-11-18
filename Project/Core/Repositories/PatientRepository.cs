using System;
using System.Collections.Generic;
using System.Linq;
using Core.Context;
using Core.Model.NewDicom;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DicomContext _dicomContext;

        public PatientRepository(DicomContext dicomContext)
        {
            _dicomContext = dicomContext;
        }

        public IEnumerable<DicomPatientData> GetPatients()
        {
            return _dicomContext.DicomPatientDatas.ToList();
        }

        public DicomPatientData GetPatientById(int patientId)
        {
            return _dicomContext.DicomPatientDatas.FirstOrDefault(x => x.DicomModelId == patientId);
        }

        public void InsertPatientData(DicomPatientData student)
        {
            _dicomContext.DicomPatientDatas.Add(student);
        }

        public void UpdatePatient(DicomPatientData student)
        {
            _dicomContext.Entry(student).State = EntityState.Modified;
        }

        public void Save()
        {
            _dicomContext.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dicomContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
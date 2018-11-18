using System;
using System.Collections.Generic;
using Core.Model.NewDicom;

namespace Core.Repositories
{
    public interface IPatientRepository : IDisposable
    {
        IEnumerable<DicomPatientData> GetPatients();
        DicomPatientData GetPatientById(int patientId);
        void InsertPatientData(DicomPatientData student);
        void UpdatePatient(DicomPatientData student);
        void Save();
    }
}
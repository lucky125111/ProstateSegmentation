using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IPatientService : IDisposable
    {
        IEnumerable<PatientDataModel> GetPatients();
        PatientDataModel GetPatient(int id);
        int UploadPatient(int id, PatientDataModel value);
        void UpdatePatient(int id, PatientDataModel value);
        void DeletePatient(int id);
    }
}
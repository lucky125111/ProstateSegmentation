using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<PatientDataModel> GetPatients();
        PatientDataModel GetPatient(int id);
        void UploadPatient(PatientDataModel value);
        void UpdatePatient(int id, PatientDataModel value);
        void DeletePatient(int id);
    }
}
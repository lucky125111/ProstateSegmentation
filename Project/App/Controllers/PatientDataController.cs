using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDataController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientDataController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [HttpGet]
        public IEnumerable<PatientDataModel> Get()
        {
            var patients = _patientService.GetPatients();
            return patients;
        }

        [HttpGet("{dicomId}")]
        public PatientDataModel Get(int dicomId)
        {
            var patient = _patientService.GetPatient(dicomId);
            return patient;
        }

        [HttpPost("{dicomId}")]
        public void Post(int dicomId, [FromBody] PatientDataModel value)
        {
            _patientService.UploadPatient(dicomId, value);
        }

        [HttpPut("{dicomId}")]
        public void Put(int dicomId, [FromBody] PatientDataModel value)
        {
            _patientService.UpdatePatient(dicomId, value);
        }

        [HttpDelete("{dicomId}")]
        public void Delete(int dicomId)
        {
            _patientService.DeletePatient(dicomId);
        }
    }
}
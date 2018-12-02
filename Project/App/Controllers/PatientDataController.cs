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

        [HttpGet("{id}")]
        public PatientDataModel Get(int id)
        {
            var patient = _patientService.GetPatient(id);
            return patient;
        }

        [HttpPost]
        public void Post([FromBody] PatientDataModel value)
        {
            _patientService.UploadPatient(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PatientDataModel value)
        {
            _patientService.UpdatePatient(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientService.DeletePatient(id);
        }
    }
}
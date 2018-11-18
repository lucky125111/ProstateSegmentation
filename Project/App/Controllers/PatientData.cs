using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using AutoMapper;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientData : ControllerBase
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PatientData(IPatientRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        ///     Get's patient data
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/PatientData
        ///     {
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="patientId">Patient PatientId</param>
        /// <returns>PatientDataViewModel</returns>
        [HttpGet]
        public PatientDataViewModel GetPatientData([FromQuery]PatientId patientId)
        {
            var patient = _repository.GetPatientById(patientId.Id);
            return _mapper.Map<PatientDataViewModel>(patient);
        }
    }
}
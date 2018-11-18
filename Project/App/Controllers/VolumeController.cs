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
    public class VolumeController : ControllerBase
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public VolumeController(IPatientRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        ///     Gets volume of prostate mask
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/VolumeVolume
        ///     {
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="patientId">Patient PatientId</param>
        /// <returns>Calculated volume</returns>
        [HttpGet]
        public VolumeModel GetVolume([FromQuery]PatientId patientId)
        {
            var patient = _repository.GetPatientById(patientId.Id);
            return _mapper.Map<VolumeModel>(patient);
        }
    }
}
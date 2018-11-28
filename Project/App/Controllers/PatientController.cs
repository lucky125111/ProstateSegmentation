using System.Collections.Generic;
using System.Linq;
using App.Models;
using AutoMapper;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _repository;

        public PatientController(IPatientRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        ///     Gets list of all patients in database
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /api/Patient
        ///     {
        ///     }
        /// </remarks>
        /// <returns>List of all patients</returns>
        [HttpGet]
        public List<PatientViewModel> GetAllPatients()
        {
            return _repository.GetPatients().Select(x => _mapper.Map<PatientViewModel>(x)).ToList();
        }
    }
}
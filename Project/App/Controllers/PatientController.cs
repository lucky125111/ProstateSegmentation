using System.Collections.Generic;
using System.Linq;
using App.Models;
using AutoMapper;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

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
        /// Sample request:
        ///
        ///     GET /api/Patient
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <returns>List of all patients</returns>
        [HttpGet]
        public List<PatientViewModel> GetAllPatients()
        {
            return _repository.GetPatients().Select(x => _mapper.Map<PatientViewModel>(x)).ToList();
        }
    }
}
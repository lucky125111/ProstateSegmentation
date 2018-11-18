using System.Threading.Tasks;
using App.Models;
using AutoMapper;
using Core.Context;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DicomSliceController : ControllerBase
    {
        private readonly IDicomSliceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DicomSliceController(IDicomSliceRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        ///     Get image for DICOM slice
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DicomMask
        ///     {
        ///        "PatientId.PatientId": 1,
        ///        "SliceIndex": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="patientId">PatientId's of DICOM and slice</param>
        /// <returns>returns DicomSliceModel representing image object</returns>
        [HttpGet]
        public DicomSliceModel GetImage([FromQuery]SliceModelId patientId)
        {
            var image = _repository.GetSliceById(patientId.PatientId.Id, patientId.SliceIndex);
            if (image == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            return new DicomSliceModel(image);
        }
    }
}
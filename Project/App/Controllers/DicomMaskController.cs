using System;
using System.Threading.Tasks;
using App.Models;
using AutoMapper;
using Core.Model;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volume;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DicomMaskController : ControllerBase
    {        
        private readonly IDicomSliceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IVolumeCalculator _calculator;
        private readonly IDicomModelRepository _dicomModelRepository;
        private readonly IPatientRepository _patientRepository;

        public DicomMaskController(IDicomSliceRepository repository, IMapper mapper, ILogger logger,
            IVolumeCalculator calculator, IDicomModelRepository dicomModelRepository,
            IPatientRepository patientRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _calculator = calculator;
            _dicomModelRepository = dicomModelRepository;
            _patientRepository = patientRepository;
        }
        /// <summary>
        ///     Get mask for DICOM slice
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
        /// <returns>returns DicomSliceModel representing mask object</returns>
        [HttpGet]
        public DicomSliceModel GetImage([FromQuery]SliceModelId patientId)
        {
            var image = _repository.GetMaskById(patientId.PatientId.Id, patientId.SliceIndex);
            if (image == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
           return new DicomSliceModel(image);
        }

        /// <summary>
        ///     Uploads new mask for DICOM slice
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DicomMask
        ///     {
        ///         "sliceModelId": {
        ///             "patientId": {
        ///                 "id": 0
        ///             },
        ///             "sliceIndex": 0
        ///         },
        ///         "newMask": "string"
        ///     }
        ///
        /// null value passed as 
        /// </remarks>
        /// <param name="maskModel">New mask</param>
        /// <returns></returns>
        [HttpPost]
        public async void UploadMask(NewMaskModel maskModel)
        {
            var patientId = UpdateMask(maskModel);
            await RecalculateVolume(patientId);

            _repository.Save();
        }

        private int UpdateMask(NewMaskModel maskModel)
        {
            var sliceIndex = maskModel.SliceModelId.SliceIndex;
            var patientId = maskModel.SliceModelId.PatientId.Id;
            var dicomSlice = _repository.GetDicomSlice(patientId, sliceIndex);
            dicomSlice.Mask = maskModel.NewMask != null ? Convert.FromBase64String(maskModel.NewMask) : null;
            _repository.UpdateMask(dicomSlice);
            return patientId;
        }

        private async Task RecalculateVolume(int patientId)
        {
            var images = _repository.GetMasks(patientId);
            var dicomModel = _dicomModelRepository.GetDicomModelById(patientId);
            var dataModel = new VolumeDataModel()
            {
                Masks = images,
                distanceBetweenSlicesmm = dicomModel.SpacingBetweenSlices,
                pixelSizeInmm = dicomModel.PixelSize
            };

            var volume = await _calculator.CalculateVolume(dataModel);
            var patient = _patientRepository.GetPatientById(patientId);
            patient.ProstateVolume = volume;
            _patientRepository.UpdatePatient(patient);
        }
    }
}
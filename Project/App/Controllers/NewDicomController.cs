using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using AutoMapper;
using Core.Dicom;
using Core.Entity;
using Core.Model;
using Core.Model.NewDicom;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volume;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewDicomController : ControllerBase
    {
        private readonly IDicomConverter _dicomConverter;
        private readonly IDicomModelRepository _dicomModelRepository;
        private readonly IDicomSliceRepository _dicomSliceRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly IVolumeCalculator _volumeCalculator;

        public NewDicomController(IDicomSliceRepository dicomSliceRepository, IMapper mapper, ILogger logger,
            IVolumeCalculator volumeCalculator, IDicomModelRepository dicomModelRepository,
            IPatientRepository patientRepository, IDicomConverter dicomConverter)
        {
            _dicomSliceRepository = dicomSliceRepository;
            _mapper = mapper;
            _logger = logger;
            _volumeCalculator = volumeCalculator;
            _dicomModelRepository = dicomModelRepository;
            _patientRepository = patientRepository;
            _dicomConverter = dicomConverter;
        }

        /// <summary>
        ///     Upload DICOM, and calculate it's mask, creates new DicomEntry
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /api/NewDicom
        ///     {
        ///     "base64Dicom": "",
        ///     }
        /// </remarks>
        /// <param name="newDicom">New dicom file as base64 encoded string</param>
        /// <returns>generated DicomModelId</returns>
        [HttpPost]
        public async Task<PatientId> UploadNewDicom(NewDicom newDicom)
        {
            var mask = await GetSegmentation(newDicom.Base64Dicom);

            var dicom = _dicomConverter.OpenDicomAndConvertToModel(newDicom.Base64Dicom);

            var volume = await CalculateVolume(mask, dicom);

            var newPatient = InsertNewPatient(dicom, volume);

            return new PatientId(newPatient.DicomModelId);
        }

        private DicomModel InsertNewPatient(NewDicomInputModel dicom, double volume)
        {
            var newPatient = _mapper.Map<DicomModel>(dicom);


            _dicomModelRepository.InsertDicom(newPatient);
            _dicomModelRepository.Save();

            var newPatientData = _mapper.Map<DicomPatientData>(dicom.DicomPatientData);
            newPatientData.DicomModelId = newPatient.DicomModelId;
            newPatientData.ProstateVolume = volume;

            var slices = dicom.DicomSlices.Select(x =>
            {
                var dicomSlice = _mapper.Map<DicomSlice>(x);
                dicomSlice.DicomModelId = newPatient.DicomModelId;
                return dicomSlice;
            });

            _patientRepository.InsertPatientData(newPatientData);
            _dicomSliceRepository.InsertSlices(slices);
            _dicomSliceRepository.Save();

            return newPatient;
        }

        private async Task<double> CalculateVolume(IEnumerable<byte[]> mask, NewDicomInputModel dicom)
        {
            var dataModel = new VolumeDataModel
            {
                Masks = mask,
                distanceBetweenSlicesmm = dicom.SpacingBetweenSlices,
                pixelSizeInmm = dicom.PixelSize
            };

            var volume = await _volumeCalculator.CalculateVolume(dataModel);
            return volume;
        }

        private async Task<IEnumerable<byte[]>> GetSegmentation(string newDicom)
        {
            return await Task.FromResult(null as IEnumerable<byte[]>);
        }
    }
}
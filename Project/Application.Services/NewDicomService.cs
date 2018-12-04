using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Dicom;
using Application.Dicom.DicomModels;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Services
{
    public class NewDicomService : INewDicomService
    {
        private readonly DicomContext _dicomContext;
        private readonly IDicomConverter _dicomConverter;
        private readonly IMapper _mapper;
        private readonly ISegmentationService _segmentationService;

        private bool _disposed;

        public NewDicomService(DicomContext dicomContext,
            IMapper mapper,
            IDicomConverter dicomConverter,
            ISegmentationService segmentationService)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
            _dicomConverter = dicomConverter;
            _segmentationService = segmentationService;
        }

        public int UploadNewDicom(NewDicomFileModel value)
        {
            var model = _dicomConverter.OpenDicomAndConvertFromBase64(value.Base64Dicom);

            if (PatientExits(model.DicomPatientData.PatientId))
                throw new AppException($"Patient {model.DicomPatientData.PatientId} already exists");

            var dicomModel = _mapper.Map<DicomModelEntity>(model);

            _dicomContext.DicomModels.Add(dicomModel);
            _dicomContext.SaveChanges();

            var slice = ConvertToSliceEntity(model, dicomModel.DicomModelId);
            var patientData = CreatePatientDataEntity(model, dicomModel);

            _dicomContext.DicomPatientDatas.Add(patientData);
            _dicomContext.DicomSlices.Add(slice);
            _dicomContext.SaveChanges();

            return dicomModel.DicomModelId;
        }

        public void AddToDicom(int id, NewDicomFileModel value)
        {
            if (_dicomContext.DicomModels.Find(id) == null)
                throw new AppException($"Patient {id} is not present in database");

            var model = _dicomConverter.OpenDicomAndConvertFromBase64(value.Base64Dicom);

            var patientId = _dicomContext.DicomPatientDatas.Find(id).PatientId;
            if (model.DicomPatientData.PatientId != patientId)
                throw new AppException(
                    $"It's not the same patient dicomId {id} has patient {patientId}, file patient id {model.DicomPatientData.PatientId}");

            var slice = ConvertToSliceEntity(model, id);
            _dicomContext.DicomSlices.Add(slice);
            _dicomContext.SaveChanges();
        }

        public int UploadNewDicoms(IEnumerable<NewDicomFileModel> value)
        {
            var newDicomFileModels = value.ToList();

            var dicomId = UploadNewDicom(newDicomFileModels.First());

            foreach (var newDicomFileModel in newDicomFileModels.Skip(1)) AddToDicom(dicomId, newDicomFileModel);

            return dicomId;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool PatientExits(string patientId)
        {
            return _dicomContext.DicomPatientDatas.Any(x => x.PatientId == patientId);
        }

        private DicomPatientDataEntity CreatePatientDataEntity(NewDicomModel model, DicomModelEntity dicomModel)
        {
            var patientData = _mapper.Map<DicomPatientDataEntity>(model.DicomPatientData);
            patientData.DicomModelId = dicomModel.DicomModelId;
            return patientData;
        }

        private DicomSliceEntity ConvertToSliceEntity(NewDicomModel model, int dicomModel)
        {
            var slice = _mapper.Map<DicomSliceEntity>(model.DicomSlices);
            slice.DicomModelId = dicomModel;
            slice.Mask = _segmentationService.Calculate(slice.Image);
            return slice;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _dicomContext.Dispose();
            _disposed = true;
        }
    }
}
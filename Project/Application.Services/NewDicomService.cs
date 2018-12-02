using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Dicom;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using NewDicomModel = Application.Dicom.DicomModels.NewDicomModel;

namespace Application.Services
{
    public class NewDicomService : INewDicomService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;
        private readonly IDicomConverter _dicomConverter;
        private readonly ISegmentationService _segmentationService;
        private readonly IVolumeService _volumeService;

        private bool _disposed;

        public NewDicomService(DicomContext dicomContext,
            IMapper mapper,
            IDicomConverter dicomConverter,
            ISegmentationService segmentationService,
            IVolumeService volumeService)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
            _dicomConverter = dicomConverter;
            _segmentationService = segmentationService;
            _volumeService = volumeService;
        }

        public int UploadNewDicom(NewDicomFileModel value)
        {
            var bytes = Convert.FromBase64String(value.Base64Dicom);
            var model = _dicomConverter.OpenDicomAndConvertFromByte(bytes);

            
            var dicomModel = _mapper.Map<DicomModelEntity>(model);
            _dicomContext.DicomModels.Add(dicomModel);

            var slice = ConvertToSliceEntity(model, dicomModel.DicomModelId);

            var patientData = _mapper.Map<DicomPatientDataEntity>(model.DicomPatientData);
            patientData.DicomModelId = dicomModel.DicomModelId;

            _dicomContext.DicomPatientDatas.Add(patientData);
            _dicomContext.DicomSlices.Add(slice);

            _dicomContext.SaveChanges();

            return dicomModel.DicomModelId;
        }

        public void AddToDicom(int id, NewDicomFileModel value)
        {
            var dicom = _dicomContext.DicomModels.Find(id);

            if (dicom == null)
                return;

            dicom.NumberOfImages++;

            var bytes = Convert.FromBase64String(value.Base64Dicom);
            var model = _dicomConverter.OpenDicomAndConvertFromByte(bytes);

            var slice = ConvertToSliceEntity(model, id);

            _dicomContext.DicomSlices.Add(slice);
            _dicomContext.DicomModels.Update(dicom);
            _dicomContext.SaveChanges();

        }

        public int UploadNewDicoms(IEnumerable<NewDicomFileModel> value)
        {
            var newDicomFileModels = value.ToList();
            var f = newDicomFileModels.First();

            var id = UploadNewDicom(f);

            foreach (var newDicomFileModel in newDicomFileModels.Skip(1))
            {
                AddToDicom(id, newDicomFileModel);
            }

            return id;
        }

        private DicomSliceEntity ConvertToSliceEntity(NewDicomModel model, int dicomModel)
        {
            var slice = _mapper.Map<DicomSliceEntity>(model.DicomSlices);
            slice.DicomModelId = dicomModel;
            slice.Mask = _segmentationService.Calculate(slice.Image);
            return slice;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
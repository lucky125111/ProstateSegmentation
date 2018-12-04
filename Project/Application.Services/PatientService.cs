using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public PatientService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public IEnumerable<PatientDataModel> GetPatients()
        {
            var dto = _dicomContext.DicomPatientDatas;
            return dto.Select(x => _mapper.Map<PatientDataModel>(x));
        }

        public PatientDataModel GetPatient(int id)
        {
            var dto = _dicomContext.DicomPatientDatas.Find(id);

            if (dto == null)
                throw new AppException($"patient data for dicom {id} was not found");

            return _mapper.Map<PatientDataModel>(dto);
        }

        public int UploadPatient(int id, PatientDataModel value)
        {
            if (_dicomContext.DicomPatientDatas.Any(x => x.PatientId == value.PatientId))
                throw new AppException($"patient data for dicom {id} is already present");

            var dto = _mapper.Map<DicomPatientDataEntity>(value);
            dto.DicomModelId = id;
            _dicomContext.DicomPatientDatas.Add(dto);
            _dicomContext.SaveChanges();
            return dto.DicomModelId;
        }

        public void UpdatePatient(int id, PatientDataModel value)
        {
            var dto = _mapper.Map<DicomPatientDataEntity>(value);
            dto.DicomModelId = id;
            var update = _dicomContext.DicomPatientDatas.Find(id);

            if (update == null)
                throw new AppException($"No patient data for dicom {id} is present");

            update = dto;
            _dicomContext.Entry(update).CurrentValues.SetValues(dto);
            _dicomContext.SaveChanges();
        }

        public void DeletePatient(int id)
        {
            var dto = _dicomContext.DicomPatientDatas.Find(id);

            if (dto == null)
                throw new AppException($"No patient data for dicom {id} is present");

            _dicomContext.DicomPatientDatas.Remove(dto);
            _dicomContext.SaveChanges();
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
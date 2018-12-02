using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class DicomService : IDicomService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public DicomService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public IEnumerable<DicomModel> GetAllDicoms()
        {
            var dto = _dicomContext.DicomModels;
            return dto.Select(x => _mapper.Map<DicomModel>(x));
        }

        public DicomModel GetDicomById(int id)
        {
            var dto = _dicomContext.DicomModels.Find(id);
            if(dto == null)
                throw new AppException($"Dicom {id} was not found");
            return _mapper.Map<DicomModel>(dto);
        }

        public int AddDicom(DicomModel value)
        {
            var dto = _mapper.Map<DicomModelEntity>(value);
            _dicomContext.DicomModels.Add(dto);
            _dicomContext.SaveChanges();
            return dto.DicomModelId;
        }

        public void UpdateDicom(int id, DicomModel value)
        { 
            var dto = _mapper.Map<DicomModelEntity>(value);
            dto.DicomModelId = id;
            var update = _dicomContext.DicomModels.Find(id);
            
            if(update == null)
                throw new AppException($"Dicom {id} was not found");

            _dicomContext.Entry(update).CurrentValues.SetValues(dto);
            _dicomContext.SaveChanges();
        }

        public void DeleteDicom(int id)
        {
            if(_dicomContext.DicomSlices.Any(x => x.DicomModelId == id) || _dicomContext.DicomPatientDatas.Any(x => x.DicomModelId == id))
                throw new AppException($"Cannot remove dicom {id} because it has dependencies");;

            var dto = _dicomContext.DicomModels.Find(id);
            
            if(dto == null)
                throw new AppException($"Dicom {id} was not found");

            _dicomContext.DicomModels.Remove(dto);
            _dicomContext.SaveChanges();
        }

        public IEnumerable<int> GetImageIndexes(int id)
        {
            if(!_dicomContext.DicomSlices.Any(x => x.DicomModelId == id))
                throw new AppException($"No images found for dicom {id}");

            var dto = _dicomContext.DicomSlices.Where(x => x.DicomModelId == id).Select(x => x.InstanceNumber);
            return dto;
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
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
    public class SliceService : ISliceService
    {        
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public SliceService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public IEnumerable<SliceModel> GetAllSlices(int dicomId)
        {
            var dto = _dicomContext.DicomSlices.Where(x => x.DicomModelId == dicomId);
            return dto.Select(x => _mapper.Map<SliceModel>(x));
        }

        public SliceModel GetSlice(int dicomId, int sliceId)
        {
            var dto = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            return _mapper.Map<SliceModel>(dto);
        }

        public void AddNewSlice(int id, SliceModel value)
        {
            var dto = _mapper.Map<DicomSliceEntity>(value);
            _dicomContext.DicomSlices.Add(dto);
            _dicomContext.SaveChanges();
        }

        public void UpdateSlice(int dicomId, int sliceId, SliceModel value)
        {            
            var dto = _mapper.Map<DicomSliceEntity>(value);
            var update = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            
            if(update == null)
                return;
            
            update = dto;
            _dicomContext.DicomSlices.Update(update);
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
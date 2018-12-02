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

        public int AddNewSlice(int id, SliceModel value)
        {
            var dto = _mapper.Map<DicomSliceEntity>(value);
            _dicomContext.DicomSlices.Add(dto);
            _dicomContext.SaveChanges();
            return dto.InstanceNumber;
        }

        public void UpdateSlice(int dicomId, int sliceId, SliceModel value)
        {            
            var dto = _mapper.Map<DicomSliceEntity>(value);
            dto.DicomModelId = dicomId;
            dto.InstanceNumber = sliceId;

            var update = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            
            if(update == null)
                return;
            
            _dicomContext.Entry(update).CurrentValues.SetValues(dto);
            _dicomContext.SaveChanges();
        }

        public void RemoveImage(int dicomId, int sliceId)
        {            
            var dto = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            _dicomContext.DicomSlices.Remove(dto);
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
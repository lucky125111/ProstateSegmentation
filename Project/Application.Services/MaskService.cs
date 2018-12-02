using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data.Context;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Services
{
    public class MaskService : IMaskService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public MaskService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }
        public IEnumerable<MaskModel> GetAll(int dicomId)
        {
            var dto = _dicomContext.DicomSlices.Where(x => x.DicomModelId == dicomId);
            return dto.Select(x => _mapper.Map<MaskModel>(x));
        }

        public MaskModel GetMask(int dicomId, int sliceId)
        {
            var dto = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            return _mapper.Map<MaskModel>(dto);
        }

        public void UpdateMask(int dicomId, int sliceId, MaskModel value)
        {            
            var update = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            
            if(update == null)
                return;
            
            update.Image = value.Mask;
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
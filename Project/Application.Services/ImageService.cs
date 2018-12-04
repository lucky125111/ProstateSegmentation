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
    public class ImageService : IImageService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public ImageService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public IEnumerable<ImageModel> GetAllImages(int dicomId)
        {
            var dto = _dicomContext.DicomSlices.Where(x => x.DicomModelId == dicomId);

            if (dto == null)
                throw new AppException($"No image for dicom {dicomId} were not found");

            return dto.Select(x => _mapper.Map<ImageModel>(x));
        }

        public ImageModel GetImage(int dicomId, int sliceId)
        {
            var dto = _dicomContext.DicomSlices.Find(dicomId, sliceId);

            if (dto == null)
                throw new AppException($"No image for dicom {dicomId}, index {sliceId} were not found");

            return _mapper.Map<ImageModel>(dto);
        }

        public int AddImage(int dicomId, ImageModel value)
        {
            var dto = _mapper.Map<DicomSliceEntity>(value);
            dto.DicomModelId = dicomId;
            _dicomContext.DicomSlices.Add(dto);
            _dicomContext.SaveChanges();
            return dto.InstanceNumber;
        }

        public void UdateImage(int dicomId, int sliceId, ImageModel value)
        {
            var update = _dicomContext.DicomSlices.Find(dicomId, sliceId);

            if (update == null)
                throw new AppException($"No image for dicom {dicomId}, index {sliceId} were not found");

            update.Image = value.Image;
            _dicomContext.Entry(update).Property(p => p.Image).IsModified = true;
            _dicomContext.SaveChanges();
        }

        public void RemoveImage(int dicomId, int sliceId)
        {
            var dto = _dicomContext.DicomSlices.Find(dicomId, sliceId);

            if (dto == null)
                throw new AppException($"No patient data for dicom {dicomId}, slice {sliceId} is present");

            dto.Image = null;
            _dicomContext.Entry(dto).Property(p => p.Image).IsModified = true;
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
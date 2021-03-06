﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Application.Data.Context;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Services
{
    public class VolumeService : IVolumeService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public VolumeService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public double GetVolume(int dicomId)
        {
            var dicomModelEntity = _dicomContext.DicomModels.Find(dicomId);

            return dicomModelEntity.ProstateVolume;
        }

        public void CalculateVolume(int dicomId, string type)
        {
            var dto = _dicomContext.DicomSlices.Where(x => x.DicomModelId == dicomId);
            
            var masks = dto.Select(x => x.Mask);
            
            var dicomModelEntity = _dicomContext.DicomModels.Find(dicomId);
            
            var imageInformation = _mapper.Map<ImageInformation>(dicomModelEntity);

            var volume = CalculateVolume(masks, imageInformation, type);

            UpdateVolume(dicomId, volume);
        }

        private void UpdateVolume(int dicomId, double volume)
        {
            var entry = _dicomContext.DicomModels.Find(dicomId);
            entry.ProstateVolume = volume;
            _dicomContext.Entry(entry).Property(p => p.ProstateVolume).IsModified = true;
            _dicomContext.SaveChanges();
        }

        public double CalculateVolume(IEnumerable<byte[]> dicomId, ImageInformation imageInformation, string type)
        {
            Console.WriteLine($"url http://volume/api/Volume/{type}");
            var client = new RestClient($"http://volume/api/Volume/{type}");
            var request = new RestRequest(Method.POST);
            var requestObject = new VolumeRequest
            {
                Masks = dicomId,
                ImageInformation = imageInformation
            };
            var json = JsonConvert.SerializeObject(requestObject);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response);
            Console.WriteLine(response.StatusCode);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new AppException("Calculate volume failed");

            var volume = JsonConvert.DeserializeObject<VolumeResponse>(response.Content);
            return volume.Volume;
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

        private class VolumeRequest
        {
            public IEnumerable<byte[]> Masks { get; set; }
            public ImageInformation ImageInformation { get; set; }
        }

        private class VolumeResponse
        {
            public double Volume { get; set; }
        }
    }
}
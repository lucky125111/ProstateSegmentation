using System;
using System.Net;
using Application.Data.Context;
using Application.Interfaces;
using AutoMapper;
using RestSharp;
using RestSharp.Deserializers;

namespace Application.Services
{
    public class SegmentationService : ISegmentationService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;

        public SegmentationService(DicomContext dicomContext, IMapper mapper)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
        }

        public byte[] Calculate(int dicomId, int sliceId)
        {
            var image = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            return Calculate(image.Image);
        }

        public byte[] Calculate(byte[] image)
        {            
            var sliceBase64 = Convert.ToBase64String(image);
            var client = new RestClient("http://localhost:5000/predict_mask/");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json", "{\"image\": \""+sliceBase64+"\"}", ParameterType.RequestBody);
            var response = client.Execute(request);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var maskBase64 = new JsonDeserializer().Deserialize<string>(response);
                return Convert.FromBase64String(maskBase64);
            }

            return null;
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
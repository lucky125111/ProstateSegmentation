using System;
using System.Net;
using Application.Data.Context;
using Application.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using RestSharp;

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
            var client = new RestClient("http://web:5000/predict_mask/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"image\": \"" + sliceBase64 + "\"}", ParameterType.RequestBody);
            var response = client.Execute(request);
            Console.WriteLine("RESPONSE");
            Console.WriteLine(response);
            Console.WriteLine(response.Content);
            Console.WriteLine(response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("RESPONSE WAS SUCCESSFUL");
                var maskBase64 = JsonConvert.DeserializeObject<SegmentationResult>(response.Content);
                return Convert.FromBase64String(maskBase64.mask);
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

        private class SegmentationResult
        {
            public string mask { get; set; }
        }
    }
}
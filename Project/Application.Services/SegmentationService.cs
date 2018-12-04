using System;
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
    public class SegmentationService : ISegmentationService
    {
        private readonly DicomContext _dicomContext;
        private readonly IMapper _mapper;

        private bool _disposed;
        private IMaskService _maskService;

        public SegmentationService(DicomContext dicomContext, IMapper mapper, IMaskService maskService)
        {
            _dicomContext = dicomContext;
            _mapper = mapper;
            _maskService = maskService;
        }

        public void Calculate(int dicomId)
        {
            var images = _dicomContext.DicomSlices.Where(x => x.DicomModelId == dicomId).Select(x => x.InstanceNumber);
            foreach (var image in images)
            {
                Calculate(dicomId, image);
            }
        }

        public void Calculate(int dicomId, int sliceId)
        {
            var image = _dicomContext.DicomSlices.Find(dicomId, sliceId);
            var mask = Calculate(image.Image);
            var maskModel = new MaskModel
            {
                Mask = mask
            };
            _maskService.UpdateMask(dicomId, sliceId, maskModel);
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
            
            if (response.StatusCode != HttpStatusCode.OK)
                throw new AppException("Segmentation failed");
            
            Console.WriteLine("RESPONSE WAS SUCCESSFUL");
            var maskBase64 = JsonConvert.DeserializeObject<SegmentationResult>(response.Content);
            return Convert.FromBase64String(maskBase64.mask);

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
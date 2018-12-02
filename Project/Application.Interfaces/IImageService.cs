using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface IImageService : IDisposable
    {
        IEnumerable<ImageModel> GetAllImages(int dicomId);
        ImageModel GetImage(int dicomId, int sliceId);
        int AddImage(int dicomId, ImageModel value);
        void UdateImage(int dicomId, int sliceId, ImageModel value);
        void RemoveImage(int dicomId, int sliceId);
    }
}
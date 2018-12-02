using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{dicomId}")]
        public IEnumerable<ImageModel> Get(int dicomId)
        {
            var imageModels = _imageService.GetAllImages(dicomId);
            return imageModels;
        }

        [HttpGet("{dicomId}&{sliceId}")]
        public ImageModel Get(int dicomId, int sliceId)
        {
            var imageModels = _imageService.GetImage(dicomId, sliceId);
            return imageModels;
        }

        [HttpPut("{dicomId}&{sliceId}")]
        public void Put(int dicomId, int sliceId, [FromBody] ImageModel value)
        {
            _imageService.UdateImage(dicomId, sliceId, value);
        }
    }
}
using System;
using Microsoft.AspNetCore.Mvc;
using VolumeService.Core;
using VolumeService.Core.VolumeCalculator;
using VolumeService.Model;

namespace VolumeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        private readonly IVolumeCalculator _volumeCalculator;

        public VolumeController(IVolumeCalculator volumeCalculator)
        {
            _volumeCalculator = volumeCalculator;
        }

        [HttpPost("ConvexHull")]
        public VolumeResponse CalculateVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            var volume =  _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation,
                ImageFitterType.ConvexHull);
            return new VolumeResponse()
            {
                Volume = volume
            };
        }

        [HttpPost("Simple")]
        public VolumeResponse CalculateSimpleVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            var volume = _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation, ImageFitterType.Simple);
            return new VolumeResponse()
            {
                Volume = volume
            };
        }

        [HttpPost("CountPixels")]
        public VolumeResponse CalculateCountPixelsVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            var volume =  _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation,
                ImageFitterType.CountPixels);
            return new VolumeResponse()
            {
                Volume = volume
            };
        }
    }
}
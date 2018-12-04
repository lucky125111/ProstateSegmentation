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
        public double CalculateVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            return _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation,
                ImageFitterType.ConvexHull);
        }

        [HttpPost("Simple")]
        public double CalculateSimpleVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            return _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation, ImageFitterType.Simple);
        }

        [HttpPost("CountPixels")]
        public double CalculateCountPixelsVolume([FromBody] VolumeRequest request)
        {
            Console.WriteLine(request);
            return _volumeCalculator.CalculateVolume(request.Masks, request.ImageInformation,
                ImageFitterType.CountPixels);
        }
    }
}
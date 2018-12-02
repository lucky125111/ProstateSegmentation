using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        private readonly IVolumeService _volumeService;

        public VolumeController(IVolumeService volumeService)
        {
            _volumeService = volumeService;
        }
        
        [HttpGet("Calculate/{id}")]
        public double Recalculate(int dicomId)
        {
            var volume = _volumeService.CalculateVolume(dicomId);
            return volume;
        }

        [HttpGet("{id}")]
        public double Get(int dicomId)
        {
            var volume = _volumeService.GetVolume(dicomId);
            return volume;
        }
    }
}
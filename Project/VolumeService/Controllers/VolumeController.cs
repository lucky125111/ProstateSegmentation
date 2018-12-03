using Microsoft.AspNetCore.Mvc;
using VolumeService.Model;

namespace VolumeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        [HttpPost]
        public double CalculateVolume(VolumeRequest request)
        {
            return 1;
        }
    }
}
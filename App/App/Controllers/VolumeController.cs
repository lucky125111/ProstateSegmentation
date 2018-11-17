using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        /// <summary>
        ///     Upload DICOM slices, and recalculate mask
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /NewDicom{id}
        ///     {
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public async Task<VolumeModel> GetVolume(PatientId patientId)
        {
            return null;
        }
    }
}
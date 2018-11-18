using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewDicomController : ControllerBase
    {
        /// <summary>
        ///     Upload DICOM, and calculate it's mask
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /NewDicom
        ///     {
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <returns>generated patient id</returns>
        [HttpPost]
        public async Task<PatientId> UploadNewDicom(NewDicom newDicom)
        {
            return null;
        }
    }
}
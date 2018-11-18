using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
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
        public async Task<PatientViewModel> GetAllPatients()
        {
            return null;
        }
    }
}
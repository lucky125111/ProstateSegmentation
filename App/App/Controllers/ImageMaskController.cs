using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMaskController : ControllerBase
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
        public async Task<DicomSliceModel> GetImage([FromQuery]SliceModelId patientId)
        {
            return null;
        }

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
        [HttpPost]
        public async Task<bool> UpdateMask(NewMaskModel maskModel)
        {
            return false;
        }
    }
}
using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewDicomController : ControllerBase
    {
        private readonly INewDicomService _dicomService;

        public NewDicomController(INewDicomService dicomService)
        {
            _dicomService = dicomService;
        }

        [HttpPost]
        public int Post([FromBody] NewDicomFileModel value)
        {
            var modelId = _dicomService.UploadNewDicom(value);
            return modelId;
        }

        [HttpPost("{id}")]
        public void Post(int id, [FromBody] NewDicomFileModel value)
        {
            _dicomService.AddToDicom(id, value);
        }

        [HttpPost("UploadList")]
        public int Post([FromBody] IEnumerable<NewDicomFileModel> value)
        {
            var modelId = _dicomService.UploadNewDicoms(value);
            return modelId;
        }
    }
}
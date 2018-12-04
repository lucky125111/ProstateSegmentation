using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DicomController : ControllerBase
    {
        private readonly IFileCreatorService _creatorService;
        private readonly IDicomService _dicomService;

        public DicomController(IDicomService dicomService, IFileCreatorService creatorService)
        {
            _dicomService = dicomService;
            _creatorService = creatorService;
        }

        [HttpGet]
        public IEnumerable<DicomModel> Get()
        {
            var dicomModels = _dicomService.GetAllDicoms();

            return dicomModels;
        }

        [HttpGet("{id}")]
        public DicomModel Get(int id)
        {
            var dicomModel = _dicomService.GetDicomById(id);
            return dicomModel;
        }

        [HttpGet("Indexes/{id}")]
        public IEnumerable<int> GetIndexes(int id)
        {
            var dicomModel = _dicomService.GetImageIndexes(id);
            return dicomModel;
        }

        [HttpGet("File/{id}")]
        public DicomFile CreateDicom(int id)
        {
            var dicomModel = _creatorService.CreateDicom(id);
            return dicomModel;
        }

        [HttpPost]
        public void Post([FromBody] DicomModel value)
        {
            _dicomService.AddDicom(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] DicomModel value)
        {
            _dicomService.UpdateDicom(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _dicomService.DeleteDicom(id);
        }
    }
}
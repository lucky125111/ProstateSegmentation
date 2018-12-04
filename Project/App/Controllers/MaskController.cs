using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaskController : ControllerBase
    {
        private readonly IMaskService _maskService;
        private readonly ISegmentationService _segmentationService;

        public MaskController(IMaskService maskService, ISegmentationService segmentationService)
        {
            _maskService = maskService;
            _segmentationService = segmentationService;
        }

        [HttpGet("{dicomId}")]
        public IEnumerable<MaskModel> Get(int dicomId)
        {
            var maskModels = _maskService.GetAll(dicomId);
            return maskModels;
        }

        [HttpGet("{dicomId}&{sliceId}")]
        public MaskModel Get(int dicomId, int sliceId)
        {
            var maskModels = _maskService.GetMask(dicomId, sliceId);
            return maskModels;
        }

        [HttpPut("{dicomId}&{sliceId}")]
        public void Put(int dicomId, int sliceId, [FromBody] MaskModel value)
        {
            _maskService.UpdateMask(dicomId, sliceId, value);
        }

        [HttpPost("Recalculate/{dicomId}&{sliceId}")]
        public void Post(int dicomId, int sliceId)
        {
            _segmentationService.Calculate(dicomId, sliceId);
        }
        
        [HttpPost("Recalculate/{dicomId}")]
        public void RecalculateAll(int dicomId)
        {
            _segmentationService.Calculate(dicomId);
        }

        [HttpDelete("{dicomId}&{sliceId}")]
        public void Delete(int dicomId, int sliceId)
        {
            _maskService.RemoveMask(dicomId, sliceId);
        }
    }
}
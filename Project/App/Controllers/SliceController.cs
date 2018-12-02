using System.Collections.Generic;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliceController : ControllerBase
    {
        private readonly ISliceService _sliceService;

        public SliceController(ISliceService sliceService)
        {
            _sliceService = sliceService;
        }
        [HttpGet("{dicomId}")]
        public IEnumerable<SliceModel> Get(int dicomId)
        {
            var slices = _sliceService.GetAllSlices(dicomId);
            return slices;
        }

        [HttpGet("{dicomId}&{sliceId}")]
        public SliceModel Get(int dicomId, int sliceId)
        {
            var slices = _sliceService.GetSlice(dicomId, sliceId);
            return slices;
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] SliceModel value)
        {
            _sliceService.AddNewSlice(id, value);
        }

        [HttpPost("{dicomId}&{sliceId}")]
        public void Post(int dicomId, int sliceId, [FromBody] SliceModel value)
        {
            _sliceService.UpdateSlice(dicomId, sliceId, value);
        }
    }
}
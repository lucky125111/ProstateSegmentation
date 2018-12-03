using System.Collections.Generic;
using Application.Models;

namespace VolumeService.Model
{
    public class VolumeRequest
    {
        public IEnumerable<byte[]> Masks { get; set; }
        public ImageInformation ImageInformation { get; set; }
    }
}
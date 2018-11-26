using System.Collections.Generic;

namespace Core.Model
{
    public class VolumeDataModel
    {
        public IEnumerable<byte[]> Masks { get; set; }
        public double? pixelSizeInmm { get; set; }
        public double? distanceBetweenSlicesmm { get; set; }
    }
}
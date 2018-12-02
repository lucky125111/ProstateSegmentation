namespace Application.Models
{
    public class ImageInformation
    {
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical { get; set; }
        public double? PixelSpacingHorizontal { get; set; }
        public double? SliceThickness { get; set; }
        public double? SpacingBetweenSlices { get; set; }
        public double ProstateVolume { get; set; }
    }
}
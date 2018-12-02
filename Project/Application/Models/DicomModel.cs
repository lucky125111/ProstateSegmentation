namespace Application.Models
{
    public class DicomModel
    {
        public int DicomModelId { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int NumberOfImages { get; set; }
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical { get; set; }
        public double? PixelSpacingHorizontal { get; set; }
        public double? SliceThickness { get; set; }
        public double? SpacingBetweenSlices { get; set; }
        public double ProstateVolume { get; set; }
    }
}
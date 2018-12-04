namespace Application.Dicom.DicomModels
{
    public class NewDicomModel
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical { get; set; }
        public double? PixelSpacingHorizontal { get; set; }
        public double? SliceThickness { get; set; }
        public double? SpacingBetweenSlices { get; set; }

        public NewDicomPatientData DicomPatientData { get; set; }
        public NewDicomSlice DicomSlices { get; set; }
    }
}
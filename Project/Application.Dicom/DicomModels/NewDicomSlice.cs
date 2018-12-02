namespace Application.Dicom.DicomModels
{
    public class NewDicomSlice
    {
        public byte[] Image { get; set; }
        public int InstanceNumber { get; set; }
        public double SliceLocation { get; set; }
    }
}
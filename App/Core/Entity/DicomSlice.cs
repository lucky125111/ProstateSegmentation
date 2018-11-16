namespace Core.Entity
{
    public class DicomSlice
    {
        public byte[] Image { get; set; }
        public byte[] Mask { get; set; }
        public int SliceIndex { get; set; }
        public string DicomModelId { get; set; }

        public DicomModel DicomModel { get; set; }

        public DicomSlice(byte[] x, string dicomModelId, int sliceIndex)
        {
            Image = x;
            DicomModelId = dicomModelId;
            SliceIndex = sliceIndex;
        }

        public DicomSlice()
        {
            
        }
    }
}
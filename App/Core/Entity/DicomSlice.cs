namespace Core.Entity
{
    public class DicomSlice
    {
        public byte[] Image { get; set; }
        public byte[] Mask { get; set; }
        public int SliceIndex { get; set; }
        public int DicomModelId { get; set; }

        public DicomModel DicomModel { get; set; }

        public DicomSlice(byte[] x, int sliceIndex, int dicomModelId)
        {
            Image = x;
            SliceIndex = sliceIndex;
            DicomModelId = dicomModelId;
        }

        public DicomSlice(byte[] image, byte[] mask, int sliceIndex)
        {
            Image = image;
            Mask = mask;
            SliceIndex = sliceIndex;
        }

        public DicomSlice()
        {
            
        }
    }
}
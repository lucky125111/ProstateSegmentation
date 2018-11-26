namespace Core.Model.DicomInput
{
    public class NewDicomSlice
    {
        public NewDicomSlice(byte[] x, int sliceIndex)
        {
            Image = x;
            SliceIndex = sliceIndex;
        }

        public byte[] Image { get; set; }
        public int SliceIndex { get; set; }
    }
}
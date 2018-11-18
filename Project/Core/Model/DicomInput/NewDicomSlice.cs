namespace Core.Model.DicomInput
{
    public class NewDicomSlice
    {
        public byte[] Image { get; set; }
        public int SliceIndex { get; set; }

        public NewDicomSlice(byte[] x, int sliceIndex)
        {
            Image = x;
            SliceIndex = sliceIndex;
        }
    }
}
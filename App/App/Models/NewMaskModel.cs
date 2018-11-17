namespace App.Models
{
    public class NewMaskModel
    {
        public SliceModelId patientId { get; set; }
        public byte[] NewMask { get; set; }
    }
}
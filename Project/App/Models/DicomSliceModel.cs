using System;

namespace App.Models
{
    public class DicomSliceModel
    {
        public string Image { get; set; }

        public DicomSliceModel(byte[] image)
        {
            Image = Convert.ToBase64String(image);
        }
    }
}
using System;

namespace App.Models
{
    public class DicomSliceModel
    {
        public DicomSliceModel(byte[] image)
        {
            Image = Convert.ToBase64String(image);
        }

        public string Image { get; set; }
    }
}
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Dicom;
using Dicom.Imaging;

namespace Core
{
    public class DicomConverter : IDicomConverter
    {
        public void ConvertAndOpenDicom(string dicomBase64)
        {
            ConvertToDicom(dicomBase64);
        }
        
        public DicomFile OpenDicom(string filePath)
        {
            var f = DicomFile.Open(filePath);
            return f;
        }

        public Bitmap ConvertToDicom(string dicomBase64)
        {
            using (var stream = new MemoryStream((Convert.FromBase64String(dicomBase64))))
            {
                var image = DicomFile.Open(stream);
                var dcm = new DicomImage(image.Dataset);
                //var image2 = new DicomImage(f2.Dataset);

                var x = (byte[]) dcm.RenderImage().AsBytes();
                var bitmap = new Bitmap(dcm.Width, dcm.Width, PixelFormat.Format32bppArgb);
                var bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(x, 0, bitmap_data.Scan0, x.Length);
                bitmap.UnlockBits(bitmap_data);
                return bitmap;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Dicom.Imaging;
namespace Core.Dicom
{
    public static class DicomImageExtensions
    {
        public static byte[] ToBytes(this DicomImage dcm, int frame = 0)
        {
            return (byte[]) dcm.RenderImage(frame).AsBytes();
        }

        public static Bitmap RenderAsBitmap(this DicomImage dcm, int frame = 0)
        {
            var x = dcm.ToBytes(frame);
            var bitmap = new Bitmap(dcm.Width, dcm.Width, PixelFormat.Format32bppArgb);
            var bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(x, 0, bitmap_data.Scan0, x.Length);
            bitmap.UnlockBits(bitmap_data);
            return bitmap;
        }

        public static byte[] ToBytes(this Bitmap image)
        {
            var bmpdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            int numbytes = bmpdata.Stride * image.Height;
            byte[] bytedata = new byte[numbytes];
            IntPtr ptr = bmpdata.Scan0;

            Marshal.Copy(ptr, bytedata, 0, numbytes);

            return bytedata;
        }

    }
}
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Dicom.Imaging;

namespace Core.Dicom
{
    public static class DicomImageExtensions
    {
        public static byte[] ToBytes(this DicomImage dcm, int frame = 0)
        {
            return dcm.RenderImage(frame).AsBytes();
        }

        public static Bitmap ToBitmap(this DicomImage dcm, int frame = 0)
        {
            var x = dcm.ToBytes(frame);
            return x.RenderBitmap(dcm.Width, dcm.Height);
        }

        public static byte[] ToBytesPng(this Bitmap bitmap)
        {
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                result = stream.ToArray();
            } 
            return result;
        }

        public static Bitmap RenderBitmap(this byte[] bytes, int dcmWidth, int dcmHeight)
        {
            var bitmap = new Bitmap(dcmWidth, dcmHeight, PixelFormat.Format32bppArgb);
            var bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            Marshal.Copy(bytes, 0, bitmap_data.Scan0, bytes.Length);
            bitmap.UnlockBits(bitmap_data);
            return bitmap;
        }

        public static Bitmap RenderBitmap(this byte[] bytes)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(bytes))
            {
                bmp = new Bitmap(ms);
            }

            return bmp;
        }

        public static byte[] ToBytesMarshall(this Bitmap image)
        {
            var bmpdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                image.PixelFormat);
            var numbytes = bmpdata.Stride * image.Height;
            var bytedata = new byte[numbytes];
            var ptr = bmpdata.Scan0;

            Marshal.Copy(ptr, bytedata, 0, numbytes);

            return bytedata;
        }

        public static string Join(this IEnumerable<string> strings, string sep)
        {
            return string.Join(sep, strings.ToArray());
        }
    }
}
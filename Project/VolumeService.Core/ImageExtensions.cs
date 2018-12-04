using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VolumeService.Core
{
    public static class ImageExtensions
    {
        public static Bitmap RenderBitmap(this byte[] bytes)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(bytes))
            {
                bmp = new Bitmap(ms);
            }

            return bmp;
        }

        public static byte[] ImageToByte(this Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
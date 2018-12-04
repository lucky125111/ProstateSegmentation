using System.Collections.Generic;
using System.Drawing;
using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace VolumeService.Core.Fitter
{
    public interface IImageFitter
    {
        IEnumerable<Point> FitImage(Mat bitmap);
    }
}
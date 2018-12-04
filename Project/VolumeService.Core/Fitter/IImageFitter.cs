using System.Collections.Generic;
using OpenCvSharp;

namespace VolumeService.Core.Fitter
{
    public interface IImageFitter
    {
        IEnumerable<Point> FitImage(Mat bitmap);
    }
}
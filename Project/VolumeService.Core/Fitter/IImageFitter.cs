using System.Collections.Generic;
using OpenCvSharp;

namespace VolumeService.Core.Fitter
{
    public interface IImageFitter
    {
        double? FitImage(Mat bitmap);
    }
}
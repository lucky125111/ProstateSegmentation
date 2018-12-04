using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Application.Models;
using OpenCvSharp;
using VolumeService.Core.Fitter;

namespace VolumeService.Core.VolumeCalculator
{
    public class VolumeCalculator : IVolumeCalculator
    {
        private readonly Func<ImageFitterType, IImageFitter> _generatorFactory;

        public VolumeCalculator(Func<ImageFitterType, IImageFitter> generatorFactory)
        {
            _generatorFactory = generatorFactory;
        }

        public double CalculateVolume(IEnumerable<byte[]> dicomId, ImageInformation imageInformation,
            ImageFitterType fitterType)
        {
            var contours = FitImages(dicomId, fitterType);

            var spacing = imageInformation.PixelSpacingHorizontal ?? 1;

            CalculateAreas(contours, spacing);

            //calculate volume from areas



            return 0;
        }

        private static List<double> CalculateAreas(List<IEnumerable<Point>> contours, double spacing)
        {
            var segmentsArea = new List<double>();

            foreach (var contour in contours)
            {
                segmentsArea.Add(Cv2.ContourArea(contour) * spacing);
            }

            return segmentsArea;
        }

        private List<IEnumerable<Point>> FitImages(IEnumerable<byte[]> dicomId, ImageFitterType fitterType)
        {
            var fitter = _generatorFactory(fitterType);

            var contours = new List<IEnumerable<Point>>();

            foreach (var image in dicomId.Select(CreateMat))
            {
                contours.Add(fitter.FitImage(image).ToList());
            }

            return contours;
        }

        private static Mat CreateMat(byte[] x)
        {
            var guid = Guid.NewGuid();
            var filename = $"{guid}.png";
            x.RenderBitmap().Save(filename, ImageFormat.Png);
            Mat mat = new Mat(filename, ImreadModes.Grayscale);
            File.Delete(filename);
            return mat;
        }
    }
}
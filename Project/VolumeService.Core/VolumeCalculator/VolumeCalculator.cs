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
            if (dicomId == null)
                return 0;

            var contours = FitImages(dicomId, fitterType);

            var spacing = imageInformation.PixelSpacingHorizontal ?? 1;
            var segmentsArea = CalculateAreas(contours, spacing);
            File.WriteAllText($"res{fitterType}",
                $"segment area {fitterType} {Environment.NewLine} {string.Join(Environment.NewLine, segmentsArea)}");
            var distance = imageInformation.SpacingBetweenSlices ?? 1;
            var volume = CalculateVolume(distance, segmentsArea);

            return volume;
        }

        public static double CalculateVolume(double distance, List<double> segmentsArea)
        {
            var volume = 0.0;

            for (var i = 0; i < segmentsArea.Count - 1; i++)
            {
                if (Math.Abs(segmentsArea[i]) < double.Epsilon)
                    continue;

                var inc = 1;
                var j = i;
                while (segmentsArea[j + inc] < double.Epsilon)
                {
                    inc++;
                    if (j + inc != segmentsArea.Count)
                        continue;
                    if (volume > 0)
                        return volume;

                    j = i - 1;
                    inc = 1;
                    break;
                }

                var sliceVolume = (segmentsArea[i] + segmentsArea[j + inc]) * distance * inc / 2;
                volume += sliceVolume;
            }

            return volume;
        }

        public static List<double> CalculateAreas(IEnumerable<IEnumerable<Point>> contours, double spacing)
        {
            return contours.Select(contour => Cv2.ContourArea(contour) * spacing).ToList();
        }

        public List<IEnumerable<Point>> FitImages(IEnumerable<byte[]> dicomId, ImageFitterType fitterType)
        {
            var fitter = _generatorFactory(fitterType);

            return dicomId.Select(CreateMat).Select(image => fitter.FitImage(image).ToList()).Cast<IEnumerable<Point>>()
                .ToList();
        }

        private static Mat CreateMat(byte[] x)
        {
            var guid = Guid.NewGuid();
            var filename = $"{guid}.png";
            x.RenderBitmap().Save(filename, ImageFormat.Png);
            var mat = new Mat(filename, ImreadModes.Grayscale);
            File.Delete(filename);
            return mat;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            var fitter = _generatorFactory(fitterType);

            var spacingX = imageInformation.PixelSpacingHorizontal ?? 0.5;
            var spacingY = imageInformation.PixelSpacingHorizontal ?? 0.5;

            //var spacingX = imageInformation.PixelSpacingHorizontal ?? 1;
            //var spacingY = imageInformation.PixelSpacingHorizontal ?? 1;

            var contours = dicomId.Select(CreateMat).Select(image => fitter.FitImage(image) * spacingX * spacingY)
                .ToList();

            var distance = imageInformation.SpacingBetweenSlices ?? 4;
            //var distance = imageInformation.SpacingBetweenSlices ?? 2;
            var volume = CalculateVolume(distance, contours);

            return volume;
        }

        public static double CalculateVolume(double distance, List<double?> segmentsArea)
        {
            var volume = 0.0;

            var any = false;
            for (int i = 0; i < segmentsArea.Count; i++)
            {
                if (!any && segmentsArea[i] > 0)
                {
                    any = true;
                }

                if(segmentsArea[i].HasValue)
                    volume += segmentsArea[i].Value;
                else if (!segmentsArea[i].HasValue && any)
                {
                    var min = -1;
                    var max = -1;
                    
                    for (int j = i; j < segmentsArea.Count; j++)
                    {
                        if (segmentsArea[j].HasValue)
                        {
                            max = j;
                            break;
                        }
                    }

                    if (max == -1)
                        break;

                    for (int j = i; j >= 0; j--)
                    {
                        if (segmentsArea[j].HasValue)
                        {
                            min = j;
                            break;
                        }
                    }

                    volume += (segmentsArea[min].Value * (i - min) / i * segmentsArea[max].Value * (max - i) / i);
                }
            }
            
            return volume * distance;
        }

        public static Mat CreateMat(byte[] x)
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
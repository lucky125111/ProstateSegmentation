using System;
using System.IO;
using System.Linq;
using Application.Models;
using VolumeService.Core;
using VolumeService.Core.Fitter;
using VolumeService.Core.VolumeCalculator;
using Xunit;

namespace VolumeService.Tests
{
    public class Patient4Benchmarks : VolumeServiceTestBase
    {
        public Patient4Benchmarks()
        {
            IImageFitter Func(ImageFitterType c)
            {
                switch (c)
                {
                    case ImageFitterType.Simple:
                        return new SquareFitter();
                    case ImageFitterType.CountPixels:
                        return new BiggestAreaFitter();
                    case ImageFitterType.ConvexHull:
                        return new ConvexHullFitter();
                    default:
                        throw new ArgumentException();
                }
            }

            _volumeCalculator = new VolumeCalculator(Func);
        }

        private readonly VolumeCalculator _volumeCalculator;

        [Fact]
        public void BiggestAreaVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.CountPixels);
            File.WriteAllText($"resvol{ImageFitterType.CountPixels}",$"volume {ImageFitterType.CountPixels} {volume}");
        }

        [Fact]
        public void ConvexHullVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.ConvexHull);
            File.WriteAllText($"resvol{ImageFitterType.ConvexHull}",$"volume {ImageFitterType.ConvexHull} {volume}");
        }

        [Fact]
        public void SquareVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.Simple);
            File.WriteAllText($"resvol{ImageFitterType.Simple}",$"volume {ImageFitterType.Simple} {volume}");
        }

        [Fact]
        public void Hispital()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var contours = _volumeCalculator.FitImages(bytes, ImageFitterType.Simple);

            var segmentsArea = _volumeCalculator.CalculateAreas(contours, 1);

            var maxVol = segmentsArea.Max(x => x);
            var distance = 1;

            var min = -1;
            var max = -1;

            for (int i = 0; i < segmentsArea.Count; i++)
            {
                if(segmentsArea[i] > 0 && min == -1)
                    min = i;

                if (max == -1 && segmentsArea[segmentsArea.Count - i - 1] > 0)
                    max = segmentsArea.Count - i;
            }

            var volume = maxVol * (max - min);
            File.WriteAllText($"res",$"area {maxVol}");
            File.WriteAllText($"resvol",$"volume {volume}");
        }
    }
}
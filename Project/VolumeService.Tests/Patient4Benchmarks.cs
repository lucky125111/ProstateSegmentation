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

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.CountPixels);
        }

        [Fact]
        public void ConvexHullVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.ConvexHull);
        }

        [Fact]
        public void SquareVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.Simple);
        }
    }
}
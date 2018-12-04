using System;
using System.IO;
using System.Linq;
using Application.Models;
using OpenCvSharp;
using VolumeService.Core;
using VolumeService.Core.Fitter;
using VolumeService.Core.VolumeCalculator;
using Xunit;

namespace VolumeService.Tests
{
    public class Patient4Benchmarks : VolumeServiceTestBase
    {
        private readonly VolumeCalculator _volumeCalculator;

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
        
        [Fact]
        public void SquareVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.Simple);
        }

        [Fact]
        public void ConvexHullVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.ConvexHull);
        }

        [Fact]
        public void BiggestAreaVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.CountPixels);
        }
    }
}
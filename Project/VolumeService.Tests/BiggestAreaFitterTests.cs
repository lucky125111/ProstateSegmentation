using System;
using System.IO;
using OpenCvSharp;
using VolumeService.Core.Fitter;
using Xunit;

namespace VolumeService.Tests
{
    public class BiggestAreaFitterTests : VolumeServiceTestBase          
    {
        private readonly BiggestAreaFitter _convexHullFitter;

        public BiggestAreaFitterTests()
        {
            _convexHullFitter = new BiggestAreaFitter();
        }

        [Fact]
        public void Prostate1Test()
        {
            var bitmap = new Mat(Prostate1, ImreadModes.Grayscale);

            var newBitmap = _convexHullFitter.FitImage(bitmap);
        }
        [Fact]
        public void Prostate2Test()
        {
            var bitmap = new Mat(Prostate1, ImreadModes.Grayscale);

            var newBitmap = _convexHullFitter.FitImage(bitmap);
        }
    }
}

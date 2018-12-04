using OpenCvSharp;
using VolumeService.Core.Fitter;
using Xunit;

namespace VolumeService.Tests
{
    public class BiggestAreaFitterTests : VolumeServiceTestBase
    {
        public BiggestAreaFitterTests()
        {
            _convexHullFitter = new BiggestAreaFitter();
        }

        private readonly BiggestAreaFitter _convexHullFitter;

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
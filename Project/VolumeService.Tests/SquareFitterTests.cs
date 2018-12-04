using OpenCvSharp;
using VolumeService.Core.Fitter;
using Xunit;

namespace VolumeService.Tests
{
    public class SquareFitterTests : VolumeServiceTestBase
    {
        private readonly SquareFitter _convexHullFitter;

        public SquareFitterTests()
        {
            _convexHullFitter = new SquareFitter();
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
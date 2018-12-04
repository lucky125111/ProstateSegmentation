using OpenCvSharp;
using VolumeService.Core.Fitter;
using Xunit;

namespace VolumeService.Tests
{
    public class ConvexHullFitterTests : VolumeServiceTestBase
    {
        public ConvexHullFitterTests()
        {
            _convexHullFitter = new ConvexHullFitter();
        }

        private readonly ConvexHullFitter _convexHullFitter;

        [Fact]
        public void Prostate1Test()
        {
            var bitmap = new Mat(Prostate1, ImreadModes.Grayscale);

            var newBitmap = _convexHullFitter.FitImage(bitmap);
        }

        [Fact]
        public void Prostate2Test()
        {
            var bitmap = new Mat(Prostate2, ImreadModes.Grayscale);

            var newBitmap = _convexHullFitter.FitImage(bitmap);
        }
    }
}
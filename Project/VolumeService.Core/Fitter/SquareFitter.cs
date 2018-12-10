using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace VolumeService.Core.Fitter
{
    public class SquareFitter : IImageFitter
    {
        public double? FitImage(Mat mat)
        {
            var guid = Guid.NewGuid();
            mat.SaveImage($"{guid}Grayscale.png");
            var contour = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (!contour.Any())
                return null;

            var cont = contour.SelectMany(x => x).ToList();
            var maxX = cont.Max(x => x.X);
            var maxY = cont.Max(x => x.Y);
            var minX = cont.Min(x => x.X);
            var minY = cont.Min(x => x.Y);
            var hull = new List<Point>
            {
                new Point(minX, maxY),
                new Point(minX, minY),
                new Point(maxX, minY),
                new Point(maxX, maxY)
            };
            Cv2.FillConvexPoly(mat, hull, Scalar.White, LineTypes.AntiAlias);
            mat.SaveImage($"{guid}HullSimple.png");
            return Cv2.CountNonZero(mat);
        }
    }
}
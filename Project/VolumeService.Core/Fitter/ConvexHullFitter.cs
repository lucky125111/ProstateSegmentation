using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;

namespace VolumeService.Core.Fitter
{
    public class ConvexHullFitter : IImageFitter
    {
        public IEnumerable<Point> FitImage(Mat mat)
        {
            var guid = Guid.NewGuid();
            mat.SaveImage($"{guid}Grayscale.png");
            var contour = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (!contour.Any())
                return new List<Point>();

            var cont = contour.SelectMany(x => x);
            var hull = Cv2.ConvexHull(cont);
            Cv2.FillConvexPoly(mat, hull, Scalar.White, LineTypes.AntiAlias);
            mat.SaveImage($"{guid}HullConvex.png");
            return hull;
        }
    }
}
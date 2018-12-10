using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace VolumeService.Core.Fitter
{
    public class ConvexHullFitter : IImageFitter
    {
        public double? FitImage(Mat mat)
        {
            var guid = Guid.NewGuid();
            mat.SaveImage($"{guid}Grayscale.png");
            var z = Cv2.CountNonZero(mat);
            var contour = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (!contour.Any())
                return null;

            var cont = contour.SelectMany(x => x);
            var hull = Cv2.ConvexHull(cont);
            Cv2.FillConvexPoly(mat, hull, Scalar.White, LineTypes.AntiAlias);
            mat.SaveImage($"{guid}HullConvex.png");
            var zz = Cv2.CountNonZero(mat);
            return zz * 0.25;
        }
    }
}
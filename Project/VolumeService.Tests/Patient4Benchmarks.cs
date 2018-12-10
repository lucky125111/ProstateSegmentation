using System;
using System.Collections.Generic;
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

            var volume = CalculateBiggestAreaVolume(bytes);
            File.WriteAllText($"resvol{ImageFitterType.CountPixels}",$"volume {ImageFitterType.CountPixels} {volume}");
        }

        [Fact]
        public void ConvexHullVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);
            var volume = CalculateConvexHullVolume(bytes);
            File.WriteAllText($"resvol{ImageFitterType.ConvexHull}",$"volume {ImageFitterType.ConvexHull} {volume}");
        }

        [Fact]
        public void SquareVolume()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var volume = CalculateSquareVolume(bytes);
            File.WriteAllText($"resvol{ImageFitterType.Simple}",$"volume {ImageFitterType.Simple} {volume}");
        }

        [Fact]
        public void Hispital()
        {
            var bytes = Directory.GetFiles(Patient4).Select(File.ReadAllBytes);

            var volume = CalculateHospitalVolume(bytes);
            File.WriteAllText($"resvol",$"volume {volume}");
        }

        private double CalculateHospitalVolume(IEnumerable<byte[]> bytes)
        {
            var fitter = new SquareFitter();

            var contours = bytes.Select(VolumeCalculator.CreateMat).Select(image => fitter.FitImage(image) * 0.5 * 0.5)
                .ToList();

            var distance =  4;
            var volume = contours.Sum(x => x ?? 0) * distance;
            
            return volume;
        }

        
        private double CalculateVoxelVolume(IEnumerable<byte[]> bytes)
        {
            var fitter = new ConvexHullFitter();

            var contours = bytes.Select(VolumeCalculator.CreateMat).Select(image => fitter.FitImage(image) * 0.5 * 0.5)
                .ToList();

            var distance =  4;
            var volume = contours.Sum(x => x ?? 0) * distance;
            
            return volume;
        }

        private double CalculateSquareVolume(IEnumerable<byte[]> bytes)
        {
            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.Simple);
            return volume;
        }

        private double CalculateBiggestAreaVolume(IEnumerable<byte[]> bytes)
        {
            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.CountPixels);
            return volume;
        }

        private double CalculateConvexHullVolume(IEnumerable<byte[]> bytes)
        {
            var volume = _volumeCalculator.CalculateVolume(bytes, new ImageInformation(), ImageFitterType.ConvexHull);
            return volume;
        }
        
        protected readonly string Patients =
            Path.Combine(@"D:\Downloads\Chrome\images3\images");

        [Fact]
        public void InzTest()
        {
            var patients = Directory.GetDirectories(Patients);
            if(File.Exists($"Wyniki.csv"))
                File.Delete($"Wyniki.csv");

            //File.WriteAllText($"Wyniki.csv",$"pacjent,szpital, trapez, otoczka najwiekszy,kwadrat,otoczka cala{Environment.NewLine}");
            File.WriteAllText($"Wyniki.csv",$"pacjent,hosp,trp, biggest, square, hull{Environment.NewLine}");
            foreach (var patient in patients)
            {
                if(!Directory.Exists(Path.Combine(patient, "contest"))
                   || !Directory.Exists(Path.Combine(patient, "mask")))
                    continue;

                var bytesCont = Directory.GetFiles(Path.Combine(patient, "contest")).OrderBy(x => {
                    var num = Path.GetFileNameWithoutExtension(x).Split('_').Last();
                    int o;
                    Int32.TryParse(num, out o);
                    return o;
                }).Select(File.ReadAllBytes);
                var volumeHos = (int) CalculateHospitalVolume(bytesCont) / 1000;
                var volumeTr = (int) CalculateVoxelVolume(bytesCont) / 1000;

                var bytesPred = Directory.GetFiles(Path.Combine(patient, "mask")).OrderBy(x => {
                    var num = Path.GetFileNameWithoutExtension(x).Split('_').Last();
                    int o;
                    Int32.TryParse(num, out o);
                    return o;
                }).Select(File.ReadAllBytes);

                var volumeBiggest = (int) CalculateBiggestAreaVolume(bytesPred) / 1000;
                var volumeSquare = (int) CalculateSquareVolume(bytesPred) / 1000;
                var volumeHull = (int) CalculateConvexHullVolume(bytesPred) / 1000;
                File.AppendAllText($"Wyniki.csv",$"{Path.GetFileName(patient)},{volumeHos},{volumeTr},{volumeBiggest},{volumeSquare},{volumeHull}{Environment.NewLine}");
            }
        }

    }
}
using System;
using System.IO;

namespace VolumeService.Tests
{
    public class VolumeServiceTestBase
    {
        protected readonly string Patient4 =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\Patient4");

        protected readonly string Prostate1 =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\prostate1.png");

        protected readonly string Prostate2 =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\prostate2.png");
    }
}
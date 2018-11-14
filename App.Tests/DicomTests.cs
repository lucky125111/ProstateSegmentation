using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Core;
using Dicom.Imaging;
using Xunit;

namespace App.Tests
{
    public class DicomTests
    {
        private readonly string _pathToTestData = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Data");

        [Fact]
        public void Test()
        {
        }
    }
}

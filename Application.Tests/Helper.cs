using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Application.Dicom;
using Dicom;
using Xunit;
using Xunit.Abstractions;

namespace Application.Tests
{
    public class Helper
    {
        public Helper(ITestOutputHelper output)
        {
            this.output = output;
        }

        private readonly string _pathToTestData =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Data");

        private readonly string _prostate000Path =
            @"D:\Inzynierka\ProstateX-0000\1.3.6.1.4.1.14519.5.2.1.7311.5101.158323547117540061132729905711\1.3.6.1.4.1.14519.5.2.1.7311.5101.160028252338004527274326500702";

        private readonly ITestOutputHelper output;

        [Fact(Skip = "helpers")]
        public void GenerateBase64FromDicom()
        {
            var f = Directory.GetFiles(_prostate000Path).Skip(1).First();
            var b = File.ReadAllBytes(f);
            var base64 = Convert.ToBase64String(b);
            File.WriteAllText("dicomBase64_1", base64);
        }

        [Fact(Skip = "helpers")]
        public void GenerateDicomTagClass()
        {
            var sb = new StringBuilder();

            sb.Append("public class PatientData {");
            sb.Append(Environment.NewLine);
            foreach (var tag in typeof(DicomTag).GetFields().Where(x => x.FieldType == typeof(DicomTag)))
            {
                sb.Append("public " + tag.FieldType.Name + " " + tag.Name + " {get; set;}");
                sb.Append(Environment.NewLine);
            }

            sb.Append(@"}");
            File.WriteAllText("PatientData.cs", sb.ToString() ?? "null");
        }

        [Fact(Skip = "helpers")]
        public void GenerateImageFromSlice()
        {
            var path = Directory.GetFiles(_prostate000Path).First();
            var c = new DicomConverter();

            var x = c.OpenDicomAndConvertFromFile(path);

            var b = x.DicomSlices.Image.RenderBitmap(x.ImageWidth, x.ImageHeight);

            b.Save("obraz.png", ImageFormat.Png);
        }
    }
}
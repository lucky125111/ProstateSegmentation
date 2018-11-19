using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using Core;
using Core.Dicom;
using Dicom;
using Dicom.Imaging;
using Xunit;
using Xunit.Abstractions;

namespace App.Tests
{
    public class Helpers
    {
        private readonly string _pathToTestData = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Data");
        private readonly string _prostate000Path = @"D:\Inzynierka\ProstateX-0000\1.3.6.1.4.1.14519.5.2.1.7311.5101.158323547117540061132729905711\1.3.6.1.4.1.14519.5.2.1.7311.5101.160028252338004527274326500702";
        
        private readonly ITestOutputHelper output;

        public Helpers(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
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

        [Fact]
        public void GenerateBase64FromDicom()
        {
            var f = Directory.GetFiles(_prostate000Path).First();
            var b = File.ReadAllBytes(f);
            var base64 = Convert.ToBase64String(b);
            File.WriteAllText("dicomBase64", base64);
        }

        [Fact]
        public void GenerateImageFromSlice()
        {
            var path = Directory.GetFiles(_prostate000Path).First();
            var c = new DicomConverter();

            var x = c.OpenDicomAndConvertFromFile(path);

            var b = x.DicomSlices.First().Image.RenderBitmap(x.ImageWidth, x.ImageHeight);

            b.Save("obraz.bmp");
        }

        [Fact]
        public void GetDicomDetails()
        {
            var path = Directory.GetFiles(_prostate000Path).Take(3).Last();
            var dcm = DicomFile.Open(path);
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientName) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientID) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.IssuerOfPatientID) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.TypeOfPatientID) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.IssuerOfPatientIDQualifiersSequence) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.SourcePatientGroupIdentificationSequence) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.GroupOfPatientsIdentificationSequence) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.SubjectRelativePositionInImage) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientBirthDate) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientBirthTime) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientBirthDateInAlternativeCalendar) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientDeathDateInAlternativeCalendar) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientAlternativeCalendar) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientSex) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientBirthName) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientAge) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientSize) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientSizeCodeSequence) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientBodyMassIndex) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.MeasuredAPDimension) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.MeasuredLateralDimension) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientWeight) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientAddress) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PatientMotherBirthName) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.PixelSpacing) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.SliceThickness) ?? "null");
output.WriteLine(DicomConverter.GetDicomTag(dcm, DicomTag.SpacingBetweenSlices) ?? "null");
        }
    }
}

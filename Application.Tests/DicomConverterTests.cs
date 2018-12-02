using System.IO;
using Application.Dicom;
using FluentAssertions;
using Xunit;

namespace Application.Tests
{
    public class DicomConverterTests : ServiceTestBase
    {
        private DicomConverter _dicomConverter;

        public DicomConverterTests()
        {
            _dicomConverter = new DicomConverter();
        }

        [Fact]
        public void OpenDicomAndConvertFromFileTest()
        {
            var testFilePath = @"D:\Inzynierka\src\Data\Prostate3T-03-0001\1.3.6.1.4.1.14519.5.2.1.7307.2101.182382809090179976301292139745\1.3.6.1.4.1.14519.5.2.1.7307.2101.287009217605941401146066177219\000000.dcm";

            var dicomModel = _dicomConverter.OpenDicomAndConvertFromFile(testFilePath);

            dicomModel.ImageHeight.Should().Be(384);
            dicomModel.ImageWidth.Should().Be(384);

            dicomModel.DicomPatientData.PatientId.Should().Be("Prostate3T-03-0001");
            dicomModel.DicomPatientData.PatientName.Should().Be("Prostate3T-03-0001");

            dicomModel.DicomSlices.SliceLocation.Should().Be(28.530969227533);
            dicomModel.DicomSlices.InstanceNumber.Should().Be(1);
        }

        [Fact]
        public void OpenDicomAndConvertFromByteTest()
        {
            var testFilePath = @"D:\Inzynierka\src\Data\Prostate3T-03-0001\1.3.6.1.4.1.14519.5.2.1.7307.2101.182382809090179976301292139745\1.3.6.1.4.1.14519.5.2.1.7307.2101.287009217605941401146066177219\000000.dcm";
            var testBytes = File.ReadAllBytes(testFilePath);
            var dicomModel = _dicomConverter.OpenDicomAndConvertFromByte(testBytes);
            
            dicomModel.ImageHeight.Should().Be(384);
            dicomModel.ImageWidth.Should().Be(384);

            dicomModel.DicomPatientData.PatientId.Should().Be("Prostate3T-03-0001");
            dicomModel.DicomPatientData.PatientName.Should().Be("Prostate3T-03-0001");

            dicomModel.DicomSlices.SliceLocation.Should().Be(28.530969227533);
            dicomModel.DicomSlices.InstanceNumber.Should().Be(1);
        }
    }
}
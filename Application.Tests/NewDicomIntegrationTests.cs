using System.Collections.Generic;
using Application.Data.Context;
using Application.Dicom;
using Application.Dicom.DicomModels;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class NewDicomIntegrationTests : ServiceTestBase
    {
        private readonly NewDicomService _newDicomServce;
        private readonly DicomContext _dicomContext;

        public NewDicomIntegrationTests()
        {
            var segmentationServiceMoq = new Mock<ISegmentationService>();
            segmentationServiceMoq.Setup(foo => foo.Calculate(It.IsAny<byte[]>())).Returns(new byte[1]);
            
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            var dicomConverterMoq = new Mock<IDicomConverter>();
            dicomConverterMoq.Setup(foo => foo.OpenDicomAndConvertFromBase64(It.IsAny<string>()))
                .Returns(new NewDicomModel()
                {
                    DicomPatientData = new NewDicomPatientData()
                    {
                        PatientId = "a",
                    },
                    DicomSlices = new NewDicomSlice()
                });

            _newDicomServce = new NewDicomService(_dicomContext,
                _mapper,
                dicomConverterMoq.Object,
                segmentationServiceMoq.Object);
        }

        [Fact]
        public void UploadNewDicomTest()
        {
            var newDicomModel = new NewDicomFileModel();
            var id = _newDicomServce.UploadNewDicom(newDicomModel);
        }
    }
}
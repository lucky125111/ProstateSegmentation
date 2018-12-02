using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Models;
using AutoFixture;
using Dicom.Network;
using FluentAssertions;
using Xunit;
using DicomService = Application.Services.DicomService;

namespace Application.Tests
{
    public class DicomServiceIntegrationTests : ServiceTestBase
    {
        private readonly DicomContext _dicomContext;
        private readonly DicomService _dicomService;

        public DicomServiceIntegrationTests()
        {
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            _dicomService = new DicomService(_dicomContext, _mapper);
        }

        [Fact]
        public void GetAllDicomsTest()
        {
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);

            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .CreateMany(3).ToList();
            _dicomContext.DicomModels.AddRange(i);
            _dicomContext.SaveChanges();

            var d = _dicomService.GetAllDicoms().ToList();

            d.Should().HaveCount(3);

            d.Should().BeEquivalentTo(i, o => o.Excluding(x => x.DicomPatientDataEntity).Excluding(x => x.DicomImages));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetDicomByIdTest()
        {
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);

            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var d = _dicomService.GetDicomById(i.DicomModelId);

            d.Should().BeEquivalentTo(i, o => o.Excluding(x => x.DicomPatientDataEntity).Excluding(x => x.DicomImages));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void AddDicomTest()
        {
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);

            var i = _fixture.Build<DicomModel>()
                .With(p => p.DicomModelId, 0)
                .Create();

            var id = _dicomService.AddDicom(i);
            _dicomContext.SaveChanges();

            var d = _dicomContext.DicomModels.Find(id);

            d.Should().NotBeNull();

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UpdateDicomTest()
        {
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);

            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            i.NumberOfImages = 5;

            _dicomService.UpdateDicom(i.DicomModelId, _mapper.Map<DicomModel>(i));
            
            var d = _dicomContext.DicomModels.Find(i.DicomModelId);

            d.Should().NotBeNull();
            d.NumberOfImages.Should().Be(5);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void DeleteDicomTest()
        {
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);

            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            _dicomService.DeleteDicom(i.DicomModelId);

            _dicomContext.DicomModels.Find(i.DicomModelId).Should().BeNull();
        }
    }
}
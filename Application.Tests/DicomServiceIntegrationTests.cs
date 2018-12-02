using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Models;
using Application.Services;
using AutoFixture;
using FluentAssertions;
using Xunit;

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
        public void GetAllDicoms_ShouldBeEmptyTest()
        {
            var d = _dicomService.GetAllDicoms().ToList();
            d.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetDicomByIdTest()
        {
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
        public void GetDicomById_ShouldBeNullTest()
        {
            var d = _dicomService.GetDicomById(-1);
            d.Should().BeNull();
        }

        [Fact]
        public void AddDicomTest()
        {
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

        [Fact]
        public void DeleteDicom_ShouldNotRemoveTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);

            var s = _fixture.Build<DicomSliceEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();
            
            _dicomContext.DicomSlices.Add(s);
            _dicomContext.SaveChanges();

            _dicomService.DeleteDicom(i.DicomModelId);

            _dicomContext.DicomModels.Find(i.DicomModelId).Should().NotBeNull();
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }
    }
}
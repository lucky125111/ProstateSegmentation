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
    public class MaskServiceIntegrationTests : ServiceTestBase
    {
        public MaskServiceIntegrationTests()
        {
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            _imageService = new MaskService(_dicomContext, _mapper);
        }

        private readonly DicomContext _dicomContext;
        private readonly MaskService _imageService;

        [Fact]
        public void GetAllTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomSliceEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .CreateMany(3).ToList();

            _dicomContext.DicomSlices.AddRange(ii);
            _dicomContext.SaveChanges();

            var d = _imageService.GetAll(i.DicomModelId).ToList();

            d.Count.Should().BeGreaterOrEqualTo(3);

            d.Select(x => x.Mask).Should().BeEquivalentTo(ii.Select(x => x.Mask));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetMaskTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomSliceEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomSlices.Add(ii);
            _dicomContext.SaveChanges();

            var d = _imageService.GetMask(i.DicomModelId, ii.InstanceNumber);

            d.Mask.Should().BeEquivalentTo(ii.Mask);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void RemoveMaskTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomSliceEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomSlices.Add(ii);
            _dicomContext.SaveChanges();

            _imageService.RemoveMask(i.DicomModelId, ii.InstanceNumber);

            var slice = _dicomContext.DicomSlices.Find(i.DicomModelId, ii.InstanceNumber);
            slice.Mask.Should().BeNull();

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UpdateMaskTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomSliceEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomSlices.Add(ii);
            _dicomContext.SaveChanges();

            var image = _fixture.Create<MaskModel>();

            _imageService.UpdateMask(i.DicomModelId, ii.InstanceNumber, image);

            var slice = _dicomContext.DicomSlices.Find(i.DicomModelId, ii.InstanceNumber);
            slice.Mask.Should().BeEquivalentTo(image.Mask);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }
    }
}
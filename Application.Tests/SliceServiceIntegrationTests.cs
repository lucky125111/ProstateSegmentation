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
    public class SliceServiceIntegrationTests: ServiceTestBase
    {
        private readonly DicomContext _dicomContext;
        private readonly SliceService _imageService;

        public SliceServiceIntegrationTests()
        {
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            _imageService = new SliceService(_dicomContext, _mapper);
        }

        [Fact]
        public void GetAllSlicesTest()
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

            var d = _imageService.GetAllSlices(i.DicomModelId).ToList();

            d.Count.Should().BeGreaterOrEqualTo(3);

            d.Should().BeEquivalentTo(ii, o => o.Excluding(x => x.DicomModelEntity));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetSliceTest()
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

            var d = _imageService.GetSlice(i.DicomModelId, ii.InstanceNumber);

            d.Should().BeEquivalentTo(ii, o => o.Excluding(x => x.DicomModelEntity));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void AddNewSliceTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();
            
            var ii = _fixture.Build<SliceModel>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Create();
            
            var instanceNumber = _imageService.AddNewSlice(i.DicomModelId, ii);
            
            _dicomContext.DicomSlices.Find(i.DicomModelId, instanceNumber);
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UpdateSliceTest()
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

            var image = _fixture.Build<SliceModel>()
                .With(p => p.InstanceNumber, ii.InstanceNumber)
                .Create();

            _imageService.UpdateSlice(i.DicomModelId, ii.InstanceNumber, image);
            
            var slice = _dicomContext.DicomSlices.Find(i.DicomModelId, ii.InstanceNumber);
            slice.Image.Should().BeEquivalentTo(image.Image);
            slice.Mask.Should().BeEquivalentTo(image.Mask);
            slice.DicomModelId.Should().Be(i.DicomModelId);
            slice.SliceLocation.Should().Be(ii.SliceLocation);
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void RemoveImageTest()
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


            _imageService.RemoveImage(i.DicomModelId, ii.InstanceNumber);
            
            var slice = _dicomContext.DicomSlices.Find(i.DicomModelId, ii.InstanceNumber);
            slice.Should().BeNull();
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }
    }
}
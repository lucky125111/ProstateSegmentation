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
    public class ImageServiceIntegrationTests: ServiceTestBase
    {
        private readonly DicomContext _dicomContext;
        private readonly ImageService _imageService;

        public ImageServiceIntegrationTests()
        {
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            _imageService = new ImageService(_dicomContext, _mapper);
        }

        [Fact]
        public void GetAllImagesTest()
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

            var d = _imageService.GetAllImages(i.DicomModelId).ToList();

            d.Count.Should().BeGreaterOrEqualTo(3);

            d.Select(x => x.Image).Should().BeEquivalentTo(ii.Select(x => x.Image));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetImageTest()
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

            var d = _imageService.GetImage(i.DicomModelId, ii.InstanceNumber);

            d.Image.Should().BeEquivalentTo(ii.Image);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void AddImageTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();
            
            var ii = _fixture.Build<ImageModel>()
                .Create();
            
            var instanceNumber = _imageService.AddImage(i.DicomModelId, ii);
            
            _dicomContext.DicomSlices.Find(i.DicomModelId, instanceNumber);
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UdateImageTest()
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

            var image = _fixture.Create<ImageModel>();

            _imageService.UdateImage(i.DicomModelId, ii.InstanceNumber, image);
            
            var slice = _dicomContext.DicomSlices.Find(i.DicomModelId, ii.InstanceNumber);
            slice.Image.Should().BeEquivalentTo(image.Image);
            
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
            slice.Image.Should().BeNull();
            
            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.SaveChanges();
        }
    }
}
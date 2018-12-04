using Application.Automapper;
using Application.Data.Entity;
using Application.Models;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests
{
    public class MappingTests : ServiceTestBase
    {
        public MappingTests()
        {
            _fixture = new Fixture();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DicomInputToEntityModel()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ConfigurationIsValidTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DicomInputToEntityModel()));
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void DicomModelEntityToImageInformationTest()
        {
            var o = _fixture.Build<DicomModelEntity>().Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity).Create();

            var res = _mapper.Map<ImageInformation>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void DicomModelToEnityTest()
        {
            var o = _fixture.Create<DicomModel>();

            var res = _mapper.Map<DicomModelEntity>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void DicomPatientDataEntityToPatientDataModelTest()
        {
            var o = _fixture.Build<DicomPatientDataEntity>().Without(p => p.DicomModelEntity).Create();

            var res = _mapper.Map<PatientDataModel>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void DicomSliceEntityToImageModelTest()
        {
            var o = _fixture.Build<DicomSliceEntity>().Without(p => p.DicomModelEntity).Create();

            var res = _mapper.Map<ImageModel>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void DicomSliceEntityToMaskModelTest()
        {
            var o = _fixture.Build<DicomSliceEntity>().Without(p => p.DicomModelEntity).Create();

            var res = _mapper.Map<MaskModel>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void DicomSliceEntityToSliceModelTest()
        {
            var o = _fixture.Build<DicomSliceEntity>().Without(p => p.DicomModelEntity).Create();

            var res = _mapper.Map<SliceModel>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void EnityToDicomModelTest()
        {
            var o = _fixture.Build<DicomModelEntity>().Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity).Create();

            var res = _mapper.Map<DicomModel>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void ImageModelToDicomSliceEntityTest()
        {
            var o = _fixture.Build<ImageModel>().Create();

            var res = _mapper.Map<DicomSliceEntity>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void MaskModelModelToDicomSliceEntityTest()
        {
            var o = _fixture.Build<MaskModel>().Create();

            var res = _mapper.Map<DicomSliceEntity>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void PatientDataModelModelToDicomPatientDataEntityTest()
        {
            var o = _fixture.Build<PatientDataModel>().Create();

            var res = _mapper.Map<DicomPatientDataEntity>(o);

            res.Should().NotBeNull();
        }

        [Fact]
        public void SliceModelModelToDicomSliceEntityTest()
        {
            var o = _fixture.Build<SliceModel>().Create();

            var res = _mapper.Map<DicomSliceEntity>(o);

            res.Should().NotBeNull();
        }
    }
}
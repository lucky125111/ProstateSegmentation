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
    public class PatientServieIntegrationTests : ServiceTestBase
    {
        public PatientServieIntegrationTests()
        {
            var connString = "Server=DESKTOP\\MSSQL2016DB;Database=DicomApp;Trusted_Connection=True;";
            _dicomContext = new DicomContext(connString);

            _patientService = new PatientService(_dicomContext, _mapper);
        }

        private readonly DicomContext _dicomContext;
        private readonly PatientService _patientService;

        [Fact]
        public void DeletePatientTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomPatientDataEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomPatientDatas.Add(ii);
            _dicomContext.SaveChanges();

            _patientService.DeletePatient(i.DicomModelId);

            var x = _dicomContext.DicomPatientDatas.Find(i.DicomModelId);

            x.Should().BeNull();

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomPatientDatas.RemoveRange(_dicomContext.DicomPatientDatas);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetPatientsTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomPatientDataEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .CreateMany(1)
                .ToList();

            _dicomContext.DicomPatientDatas.AddRange(ii);
            _dicomContext.SaveChanges();

            var d = _patientService.GetPatients().ToList();

            d.Count.Should().BeGreaterOrEqualTo(1);

            d.Should().BeEquivalentTo(ii, o => o.Excluding(x => x.DicomModelEntity).Excluding(x => x.DicomModelId));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.DicomPatientDatas.RemoveRange(_dicomContext.DicomPatientDatas);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void GetPatientTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomPatientDataEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomPatientDatas.Add(ii);
            _dicomContext.SaveChanges();

            var d = _patientService.GetPatient(i.DicomModelId);

            d.Should().BeEquivalentTo(ii, o => o.Excluding(x => x.DicomModelEntity).Excluding(x => x.DicomModelId));

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomSlices.RemoveRange(_dicomContext.DicomSlices);
            _dicomContext.DicomPatientDatas.RemoveRange(_dicomContext.DicomPatientDatas);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UpdatePatientTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<DicomPatientDataEntity>()
                .With(p => p.DicomModelId, i.DicomModelId)
                .Without(p => p.DicomModelEntity)
                .Create();

            _dicomContext.DicomPatientDatas.Add(ii);
            _dicomContext.SaveChanges();

            var n = _fixture.Build<PatientDataModel>()
                .Create();

            _patientService.UpdatePatient(i.DicomModelId, n);

            var x = _dicomContext.DicomPatientDatas.Find(i.DicomModelId);

            x.DicomModelId.Should().Be(x.DicomModelId);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomPatientDatas.RemoveRange(_dicomContext.DicomPatientDatas);
            _dicomContext.SaveChanges();
        }

        [Fact]
        public void UploadPatientTest()
        {
            var i = _fixture.Build<DicomModelEntity>()
                .Without(p => p.DicomModelId)
                .Without(p => p.DicomImages)
                .Without(p => p.DicomPatientDataEntity)
                .Create();

            _dicomContext.DicomModels.Add(i);
            _dicomContext.SaveChanges();

            var ii = _fixture.Build<PatientDataModel>()
                .Create();

            var instanceNumber = _patientService.UploadPatient(i.DicomModelId, ii);

            var x = _dicomContext.DicomPatientDatas.Find(i.DicomModelId);

            x.DicomModelId.Should().Be(x.DicomModelId);

            _dicomContext.DicomModels.RemoveRange(_dicomContext.DicomModels);
            _dicomContext.DicomPatientDatas.RemoveRange(_dicomContext.DicomPatientDatas);
            _dicomContext.SaveChanges();
        }
    }
}
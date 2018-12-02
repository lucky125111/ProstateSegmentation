using System;
using Application.Automapper;
using Application.Data.Context;
using AutoFixture;
using AutoMapper;

namespace Application.Tests
{
    public class ServiceTestBase
    {
        protected  Fixture _fixture;
        protected  IMapper _mapper;

        public ServiceTestBase()
        {
            _fixture = new Fixture();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DicomInputToEntityModel()));
            _mapper = config.CreateMapper();
        }
    }
}
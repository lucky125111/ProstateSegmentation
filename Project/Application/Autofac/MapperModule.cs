using System;
using System.Linq;
using Autofac;
using AutoMapper;

namespace Application.Autofac
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var autoMapperProfileTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
                a.GetTypes().Where(p => typeof(Profile).IsAssignableFrom(p) && p.IsPublic && !p.IsAbstract));
            var autoMapperProfiles = autoMapperProfileTypes.Select(p => (Profile) Activator.CreateInstance(p));
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in autoMapperProfiles) cfg.AddProfile(profile);
            }));
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>()
                .PropertiesAutowired();
        }
    }
}
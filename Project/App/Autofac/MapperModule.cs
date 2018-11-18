using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using Core.Entity;
using Module = Autofac.Module;

namespace App.Autofac
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.BaseType == typeof(Profile)
                            && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in ctx.Resolve<IEnumerable<Profile>>())
                    cfg.AddProfile(profile);
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>()
                    .CreateMapper())
                .As<IMapper>()
                .SingleInstance();
        }
    }
}
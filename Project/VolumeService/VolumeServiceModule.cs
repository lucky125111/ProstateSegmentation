using System;
using Autofac;
using VolumeService.Core;
using VolumeService.Core.Fitter;
using VolumeService.Core.VolumeCalculator;

namespace VolumeService
{
    public class VolumeServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SquareFitter>().AsSelf();
            builder.RegisterType<BiggestAreaFitter>().AsSelf();
            builder.RegisterType<ConvexHullFitter>().AsSelf();
            builder.RegisterType<VolumeCalculator>().As<IVolumeCalculator>();

            builder.Register<Func<ImageFitterType, IImageFitter>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return (language) =>
                {
                    switch (language)
                    {
                        case ImageFitterType.Simple:
                            return context.Resolve<SquareFitter>();
                        case ImageFitterType.CountPixels:
                            return context.Resolve<BiggestAreaFitter>();
                        case ImageFitterType.ConvexHull:
                            return context.Resolve<ConvexHullFitter>();
                        default:
                            throw new ArgumentException();
                    }
                };
            });
        }
    }
}
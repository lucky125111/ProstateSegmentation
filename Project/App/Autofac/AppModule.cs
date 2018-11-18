using Autofac;
using Autofac.Core;
using Core;
using Core.Dicom;
using Core.Repositories;
using Volume;

namespace App
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DicomSliceRepository>()
                .As<IDicomSliceRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PatientRepository>()
                .As<IPatientRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DicomModelRepository>()
                .As<IDicomModelRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<VolumeCalculator>()
                .As<IVolumeCalculator>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<SegmentationProvider>()
                .As<ISegmentationProvider>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<DicomConverter>()
                .As<IDicomConverter>()
                .InstancePerLifetimeScope();
        }
    }
}
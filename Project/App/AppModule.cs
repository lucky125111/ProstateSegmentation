using Application.Dicom;
using Application.Interfaces;
using Application.Services;
using Autofac;

namespace App
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DicomService>().As<IDicomService>();
            builder.RegisterType<FileCreatorService>().As<IFileCreatorService>();
            builder.RegisterType<ImageService>().As<IImageService>();
            builder.RegisterType<MaskService>().As<IMaskService>();
            builder.RegisterType<NewDicomService>().As<INewDicomService>();
            builder.RegisterType<PatientService>().As<IPatientService>();
            builder.RegisterType<SegmentationService>().As<ISegmentationService>();
            builder.RegisterType<SliceService>().As<ISliceService>();
            builder.RegisterType<VolumeService>().As<IVolumeService>();
            builder.RegisterType<DicomConverter>().As<IDicomConverter>();
        }
    }
}
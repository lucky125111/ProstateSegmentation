using Application.Data.Entity;
using Application.Dicom.DicomModels;
using Application.Models;
using AutoMapper;

namespace Application.Automapper
{
    public class DicomInputToEntityModel : Profile
    {
        public DicomInputToEntityModel()
        {
            CreateMap<DicomModel, DicomModelEntity>()
                .ForMember(dest => dest.DicomPatientDataEntity, opt => opt.Ignore())
                .ForMember(dest => dest.DicomImages, opt => opt.Ignore())
                ;
            CreateMap<DicomModelEntity, DicomModel>()
                ;

            CreateMap<ImageModel, DicomSliceEntity>()
                .ForMember(dest => dest.Mask, opt => opt.Ignore())
                .ForMember(dest => dest.InstanceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.SliceLocation, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                ;
            CreateMap<DicomSliceEntity, ImageModel>()
                ;

            CreateMap<MaskModel, DicomSliceEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.InstanceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.SliceLocation, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                ;
            CreateMap<DicomSliceEntity, MaskModel>()
                ;

            CreateMap<DicomSliceEntity, SliceModel>()
                ;
            CreateMap<SliceModel, DicomSliceEntity>()
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                ;

            CreateMap<PatientDataModel, DicomPatientDataEntity>()
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                ;
            CreateMap<DicomPatientDataEntity, PatientDataModel>()
                ;


            CreateMap<NewDicomModel, DicomModelEntity>()
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomImages, opt => opt.Ignore())
                .ForMember(dest => dest.DicomPatientDataEntity, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfImages, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.ProstateVolume, opt => opt.Ignore())
                ;
            CreateMap<NewDicomSlice, DicomSliceEntity>()
                .ForMember(dest => dest.Mask, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                ;
            CreateMap<NewDicomPatientData, DicomPatientDataEntity>()
                .ForMember(dest => dest.DicomModelEntity, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                ;

            CreateMap<DicomModelEntity, ImageInformation>()
                ;
        }
    }
}
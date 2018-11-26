using AutoMapper;
using Core.Entity;
using Core.Model.DicomInput;
using Core.Model.NewDicom;

namespace App.Automapper
{
    public class DicomInputToEntityModel : Profile
    {
        public DicomInputToEntityModel()
        {
            CreateMap<NewDicomInputModel, DicomModel>()
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomImages, opt => opt.Ignore())
                .ForMember(dest => dest.DicomPatientData, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfImages, opt => opt.MapFrom(src => src.DicomSlices.Count))
                ;

            CreateMap<NewDicomPatientData, DicomPatientData>()
                .ForMember(dest => dest.ProstateVolume, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModel, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                ;

            CreateMap<NewDicomSlice, DicomSlice>()
                .ForMember(dest => dest.Mask, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModelId, opt => opt.Ignore())
                .ForMember(dest => dest.DicomModel, opt => opt.Ignore())
                ;
        }
    }
}
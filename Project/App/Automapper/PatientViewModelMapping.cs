using App.Models;
using AutoMapper;
using Core.Model.NewDicom;

namespace App.Automapper
{
    public class PatientViewModelMapping : Profile
    {
        public PatientViewModelMapping()
        {
            CreateMap<DicomPatientData, PatientViewModel>()
                .ForMember(dest => dest.DicomId, opt => opt.MapFrom(src => src.DicomModelId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PatientName))
                ;
        }
    }
}
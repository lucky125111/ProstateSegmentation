using App.Models;
using AutoMapper;
using Core.Model.NewDicom;

namespace App.Automapper
{
    public class PatientDataViewModelMapping : Profile
    {
        public PatientDataViewModelMapping()
        {
            CreateMap<DicomPatientData, PatientDataViewModel>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientName))
                .ForMember(dest => dest.NumberOfImages, opt => opt.MapFrom(src => src.DicomModelId))
                ;
        }
    }
}
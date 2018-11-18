using App.Models;
using AutoMapper;
using Core.Model.NewDicom;

namespace App.Automapper
{
    public class VolumeModelMapping : Profile
    {
        public VolumeModelMapping()
        {
            CreateMap<DicomPatientData, VolumeModel>()
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.ProstateVolume))
                ;
        }
    }
}
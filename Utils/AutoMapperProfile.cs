using AutoMapper;
using LOT_TASK.Dtos;
using LOT_TASK.Models;

namespace LOT_TASK.Utils
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<FlightDto, FlightModel>()
                .ForMember(dest => dest.AircraftType, opt => opt.MapFrom(src => Enum.Parse(typeof(AircraftType), src.AircraftType, true)));
            CreateMap<FlightModel, FlightDto>()
                .ForMember(dest => dest.AircraftType, opt => opt.MapFrom(src => src.AircraftType.ToString()));
        }
    }
}

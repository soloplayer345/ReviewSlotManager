using AutoMapper;
using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Slot, SlotDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ReviewRound, ReviewRoundDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<GroupSlotRegistration, GroupSlotRegistrationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ReviewerSlotRegistration, ReviewerSlotRegistrationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}

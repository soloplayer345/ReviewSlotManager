using AutoMapper;
using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Slot
        CreateMap<Slot, SlotDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<SlotDto, Slot>();

        // ReviewRound
        CreateMap<ReviewRound, ReviewRoundDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<ReviewRoundDto, ReviewRound>();

        // GroupSlotRegistration
        CreateMap<GroupSlotRegistration, GroupSlotRegistrationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // ReviewerSlotRegistration
        CreateMap<ReviewerSlotRegistration, ReviewerSlotRegistrationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Semester
        CreateMap<Semester, SemesterDto>();
        CreateMap<SemesterDto, Semester>();
        CreateMap<CreateSemesterDto, Semester>();
        CreateMap<UpdateSemesterDto, Semester>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // User
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<UserDto, User>();

        // Group
        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
        CreateMap<CreateGroupDto, Group>();

        // GroupMember
        CreateMap<GroupMember, GroupMemberDto>();
        CreateMap<GroupMemberDto, GroupMember>();
        CreateMap<CreateGroupMemberDto, GroupMember>();

        // ReviewerSlotConfig
        CreateMap<ReviewerSlotConfig, ReviewerSlotConfigDto>();
        CreateMap<ReviewerSlotConfigDto, ReviewerSlotConfig>();
        CreateMap<CreateReviewerSlotConfigDto, ReviewerSlotConfig>();

        // Notification
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        CreateMap<NotificationDto, Notification>();
    }
}

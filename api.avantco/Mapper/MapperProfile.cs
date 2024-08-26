
using api.avantco.Model.Newsletter;
using api.avantco.Model.scheduleAppointment;
using api.avantco.Model.Users;
using AutoMapper;
using biz.avantco.Entities;

namespace api.avantco.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ScheduleAppointment, SheduleAppointmentGet>().ReverseMap();
        CreateMap<ScheduleAppointment, SheduleAppointmentPost>().ReverseMap();

        CreateMap<Newsletter, NewsletterDTO>().ReverseMap();

        CreateMap<Users, UsersDTO>().ReverseMap();
        CreateMap<Users, UsersGetDTO>().ReverseMap();

    }
}
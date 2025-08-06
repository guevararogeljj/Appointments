using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Entities;
using AutoMapper;

namespace Appointments.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
            .ForPath(dest => dest.Patient.FirstName,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FirstName : string.Empty))
            .ForPath(dest => dest.Patient.LastName,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.LastName : string.Empty))
            .ForPath(dest => dest.Patient.BirthDate,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.BirthDate : default(DateTime)))
            .ForPath(dest => dest.Patient.PhoneNumber,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PhoneNumber : string.Empty));
        CreateMap<AppointmentDto, Appointment>();
    }
}

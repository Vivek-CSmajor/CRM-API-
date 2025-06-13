using AutoMapper;
using MockCRM.Models;

namespace MockCRM.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.DaysSinceLastContact, opt => opt.MapFrom(src =>
                src.LastContactDate.HasValue
                    ? (int?)(DateTime.UtcNow - src.LastContactDate.Value).TotalDays
                    : null));
    }
}
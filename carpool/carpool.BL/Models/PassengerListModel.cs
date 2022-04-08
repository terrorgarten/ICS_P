using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record PassengerListModel(
        Guid PassengerId,
        string Name, //ID??
        string Surname) : ModelBase
    {

        public Guid PassengerId { get; set; } = PassengerId;
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
#nullable disable
        public PassengerListModel() : this(default, default, default) { }
#nullable enable
    }
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRideEntity, PassengerListModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Passenger.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Passenger.Surname))
                .ReverseMap()
                .ForMember(entity => entity.Passenger, expression => expression.Ignore())
                .ForMember(entity => entity.Ride, expression => expression.Ignore())
                .ForMember(entity => entity.RideId, expression => expression.Ignore());
        }
        public static PassengerListModel Empty => new(default, "Unknown", "Unknown");

    }


}

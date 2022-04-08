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
    }
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRideEntity, PassengerListModel>()
                .ReverseMap()
                .ForMember(entity => entity.Passenger, expression => expression.Ignore())
                .ForMember(entity => entity.Ride, expression => expression.Ignore())
                .ForMember(entity => entity.RideId, expression => expression.Ignore());
        }
    }
}

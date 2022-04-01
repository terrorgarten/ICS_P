using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record UserDetailModel(
        string Name,
        string Surname,
        string? PhotoUrl) : ModelBase
    {
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string? PhotoUrl { get; set; }

        public List<CarEntity> OwnedCar { get; init; } = new();
        public List<RideEntity> DriverRides { get; init; } = new();
        //public List<UserRideEntity> PassengerRides { get; init; } = new();


        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserEntity, UserDetailModel>()
                    .ReverseMap();
            }
        }

        //public static UserDetailModel Empty => new(string.Empty, string.Empty);
    }
}
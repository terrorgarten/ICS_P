using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record RideDetailModel(
        string Start,
        string End,
        DateTime BeginTime,
        TimeSpan ApproxRideTime,
        Manufacturer Manufacturer,
        int SeatCapacity,
        CarType CarType,
        Guid UserId, 
        Guid CarId) : ModelBase
    {
        public string Start { get; set; } = Start;
        public string End { get; set; } = End;
        public TimeSpan ApproxRideTime { get; set; } = ApproxRideTime;
        public DateTime BeginTime { get; set; } = BeginTime;
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;

        //   public UserEntity User { get; init; }
        //   public CarEntity Car { get; init; }
        // Odkaz na spolucestující

        public Guid UserId { get; set; } = UserId;
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid CarId { get; set; } = CarId;
        public int SeatCapacity { get; set; } = SeatCapacity;
        public string PhotoUrl { get; set; }
        public DateTime RegistrationDate { get; set; }

        //public List<PassengerListModel> Passengers { get; init; } = new();


        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideDetailModel>()
                    .ForMember(dst => dst.SeatCapacity, opt => opt.MapFrom(src => src.Car.SeatCapacity))
                    .ForMember(dst => dst.PhotoUrl, opt => opt.MapFrom(src => src.Car.PhotoUrl))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.User.Name))
                    .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.User.Surname));
            }
        }

        public static RideDetailModel Empty => new("Unknown", "Unknown", default, default, default, default, default, default, default);
    }
}
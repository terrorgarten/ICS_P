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
        TimeSpan ApproxRideTime) : ModelBase
    {
        public string Start { get; set; } = Start;
        public string End { get; set; } = End;
        public TimeSpan ApproxRideTime { get; set; } = ApproxRideTime;
        public DateTime BeginTime { get; set; } = BeginTime;

        public UserEntity User { get; init; }
        public CarEntity Car { get; init; }
        //public List<UserRideEntity> PassengerRides { get; init; } = new();

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideDetailModel>()
                    .ReverseMap();
            }
        }

        //public static CarDetailModel Empty => new(string.Empty, string.Empty, default, default);
    }
}
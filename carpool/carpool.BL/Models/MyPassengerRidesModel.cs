using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record MyPassengerRidesModel() : ModelBase
    {
        public List<RideListModel> PassengerRides { get; init; } = new();

        public class MyPassengerRides : Profile
        {
            public MyPassengerRides()
            {
                CreateMap<RideEntity, MyPassengerRidesModel>()
                    .ReverseMap();
            }
        }

        //public static WelcomeModel Empty => new("Unknown", "Unknown");
    }
}


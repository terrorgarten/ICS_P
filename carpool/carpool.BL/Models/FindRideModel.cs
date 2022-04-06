using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record FindRideModel() : ModelBase
    {
        public List<RideListModel> ExistingRides { get; init; } = new();

        public class WelcomeMapperProfile : Profile
        {
            public WelcomeMapperProfile()
            {
                CreateMap<UserEntity, WelcomeModel>()
                    .ReverseMap();
            }
        }

        //public static WelcomeModel Empty => new("Unknown", "Unknown");
    }
}

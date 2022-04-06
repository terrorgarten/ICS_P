using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record CreatedByMeModel() : ModelBase
    {
        public List<RideListModel> DriverRides { get; init; } = new();

        public class CreatedByMeMapper : Profile
        {
            public CreatedByMeMapper()
            {
                CreateMap<UserRideEntity, CreatedByMeModel>()
                    .ReverseMap();  
            }
        }

    }
}


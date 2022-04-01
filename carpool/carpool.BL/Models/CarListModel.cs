using System;
using AutoMapper;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record CarListModel(
        CarType CarType,
        string? PhotoUrl ) : ModelBase
    {
        public CarType CarType { get; set; } = CarType;
        public string? PhotoUrl { get; set; }

        public UserEntity Owner { get; init; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarListModel>();
            }
        }
    }
}
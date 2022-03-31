using System;
using AutoMapper;
using carpool.common.Enums;
using carpool.DAL.Entities;

namespace carpool.BL.Models
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
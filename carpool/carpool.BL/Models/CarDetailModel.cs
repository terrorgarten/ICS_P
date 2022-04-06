using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record CarDetailModel (
        Manufacturer Manufacturer,
        CarType CarType,
        //SPZ
        //Typ karoserie
        int SeatCapacity,
        DateTime RegistrationDate) : ModelBase
    {
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;
        public int SeatCapacity { get; set; } = SeatCapacity;
        public DateTime RegistrationDate { get; set; } = RegistrationDate;
        public string? PhotoUrl { get; set; }

        public class CarMapperProfile : Profile
        {
            public CarMapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>()
                    .ReverseMap();
            }

        }

        public static CarDetailModel Empty => new(default, default, 4, default);
    }


    /*public record CarDetailModel(
        Manufacturer Manufacturer,
        CarType CarType,
        //DateOnly RegistrationDate,
        string? PhotoUrl,
        int SeatCapacity ) : ModelBase
    {
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;
        //public DateOnly RegistrationDate { get; set; } = RegistrationDate;
        public string? PhotoUrl { get; set; }
        public UserEntity Owner { get; init; }
        
        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>()
                    .ReverseMap();
            }
        }

        //public static CarDetailModel Empty => new(string.Empty, string.Empty, default, default);
    }*/
}

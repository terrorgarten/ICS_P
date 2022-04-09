﻿using System;
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
        int SeatCapacity,
        DateTime RegistrationDate
        ) : ModelBase
    {
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;
        public int SeatCapacity { get; set; } = SeatCapacity;
        public DateTime RegistrationDate { get; set; } = RegistrationDate;
        public string? PhotoUrl { get; set; }

        //TODO: vymysliet Ignore
        //public string Name = "Unknown";
        //public  string Surname = "Unknown";

        public class CarMapperProfile : Profile
        {
            public CarMapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>()
                    .ReverseMap();
                   // .ForMember(dst => dst.OwnerId, opt => opt.Ignore());
            }

        }

        public static CarDetailModel Empty => new(default, default, default, default);
    }
}

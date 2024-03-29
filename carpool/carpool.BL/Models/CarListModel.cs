﻿using AutoMapper;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record CarListModel(
    Manufacturer Manufacturer,
    CarType CarType
) : ModelBase
{
    public Manufacturer Manufacturer { get; set; } = Manufacturer;
    public CarType CarType { get; set; } = CarType;
    public string? PhotoUrl { get; set; }

    public class CarListMapperProfile : Profile
    {
        public CarListMapperProfile()
        {
            CreateMap<CarEntity, CarListModel>();
        }
    }
}
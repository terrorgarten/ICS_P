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
        int SeatCapacity,
        DateTime RegistrationDate,
        Guid OwnerId) : ModelBase
    {
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;
        public int SeatCapacity { get; set; } = SeatCapacity;
        public DateTime RegistrationDate { get; set; } = RegistrationDate;
        public string? PhotoUrl { get; set; }

        public Guid OwnerId { get; set; } = OwnerId;
        //TODO: vymysliet Ignore
        //public string Name = "Unknown";
        //public  string Surname = "Unknown";

        public class CarMapperProfile : Profile
        {
            public CarMapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>()
                    //.ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Owner.Name))
                    //.ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Owner.Surname))
                    .ReverseMap();
            }

        }

        public static CarDetailModel Empty => new(default, default, default, default, default);
    }
}

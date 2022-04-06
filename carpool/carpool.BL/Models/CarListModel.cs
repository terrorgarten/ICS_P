using System;
using AutoMapper;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    /*public record CarListModel(
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
    }*/

    public record CarListModel(
        Manufacturer Manufacturer,
        CarType CarType
        )
    {
        public Manufacturer Manufacturer { get; set; } = Manufacturer;
        public CarType CarType { get; set; } = CarType;
        public string PhotoUrl { get; set; }
        //SPZ
        /*public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }*/

        public class CarListMapperProfile : Profile
        {
            public CarListMapperProfile()
            {
                CreateMap<CarEntity, CarListModel>();
                     //V listě nemá být reverseMap
                    /*.ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Owner.Name))
                    .ForMember(dst => dst.UserSurname, opt => opt.MapFrom(src => src.Owner.Surname));*/
            }
        }
    }
}
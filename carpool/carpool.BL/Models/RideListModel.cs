using System;
using AutoMapper;
using Carpool.BL.Models;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record RideListModel(
        string Start,
        string End,
        DateTime BeginTime,
        int SeatCapacity) : ModelBase
    {
        public string Start { get; set; } = Start;
        public string End { get; set; } = End;
        public DateTime BeginTime { get; set; } = BeginTime;
        public int SeatCapacity { get; set; } = SeatCapacity;

#nullable disable
        public RideListModel() : this(default, default, default, default) { }
#nullable enable

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideListModel>()
                .ForMember(dst => dst.SeatCapacity, opt => opt.MapFrom(src => src.Car.SeatCapacity));
            }
        }
    }
}
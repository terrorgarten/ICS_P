using System;
using AutoMapper;
using carpool.BL.Models;
using carpool.common.Enums;
using carpool.DAL.Entities;

namespace carpool.BL.Models
{
    public record RideListModel(
        string Start,
        string End,
        DateTime BeginTime ) : ModelBase
    {
        public string Start { get; set; } = Start;
        public string End { get; set; } = End;
        public DateTime BeginTime { get; set; } = BeginTime;


        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideListModel>();
            }
        }
    }
}
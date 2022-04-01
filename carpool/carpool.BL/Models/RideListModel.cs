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
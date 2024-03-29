﻿using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record RideDetailModel(
    string Start,
    string End,
    DateTime BeginTime,
    TimeSpan ApproxRideTime,
    Guid UserId,
    Guid? CarId
) : ModelBase
{
#nullable disable
    public RideDetailModel() : this(default!, default!, default, default, default, default)
    {
    }
#nullable enable
    public CarDetailModel? Car { get; set; }
    public UserDetailModel? User { get; set; }
    public string Start { get; set; } = Start;
    public string End { get; set; } = End;
    public TimeSpan ApproxRideTime { get; set; } = ApproxRideTime;
    public DateTime BeginTime { get; set; } = BeginTime;

    public Guid? CarId { get; set; } = CarId;
    public Guid UserId { get; set; } = UserId;

    public static RideDetailModel Empty => new("Unknown", "Unknown", default, default, default, default);

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.Car!.OwnerId))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.Car!.Owner))
                .ReverseMap()
                .ForMember(dst => dst.Car, opt => opt.Ignore());
        }
    }
}